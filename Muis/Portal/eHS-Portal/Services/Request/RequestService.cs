using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using eHS.Portal.Client;
using Core.Http.Exceptions;
using eHS.Portal.Model;
using Microsoft.AspNetCore.WebUtilities;
using eHS.Portal.DTO;

namespace eHS.Portal.Services
{
  public class RequestService : IRequestService
  {
    readonly ApiClient _apiClient;

    public RequestService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<IList<Request>> Search(RequestOptions options)
    {
      return await _apiClient.RequestSdk.Query(options);
    }

    public async Task<IList<Request>> GetRelatedRequest(long id)
    {
      return await _apiClient.RequestSdk.GetRelatedRequest(id);
    }

    public async Task<Request> GetByID(long id)
    {
      var result = await _apiClient.RequestSdk.GetByID(id);
      if (result == null)
      {
        throw new NotFoundException();
      }

      await FetchRequestCustomerInfo(result);

      return result;
    }

    public async Task<Request> GetByIDBasic(long id)
    {
      var result = await _apiClient.RequestSdk.GetByIDBasic(id);
      if (result == null)
      {
        throw new NotFoundException();
      }

      await FetchRequestCustomerInfo(result);

      return result;
    }

    async Task FetchRequestCustomerInfo(Request request)
    {
      request.Customer = await _apiClient.CustomerSdk.GetByID(request.CustomerID);
      request.Customer.Code = request.CustomerCode;
      request.Customer.GroupCode = request.GroupCode;

      var reqCustomer = await _apiClient.CustomerSdk.GetRequestCustomerByID(request.CustomerID);
      request.Customer.Officer = reqCustomer?.Officer;

      if (request.AgentID != null)
      {
        request.Agent = await _apiClient.PersonSdk.GetByID(request.AgentID.Value);
        request.Requestor = await _apiClient.PersonSdk.GetByID(request.RequestorID);
      }
      else
      {
        request.Requestor = await _apiClient.PersonSdk.GetByID(request.RequestorID);
      }
    }

    public async Task DoEscalate(long requestID, EscalateStatus status, string remarks)
    {
      await _apiClient.RequestSdk.EscalateRequest(requestID, status, remarks);
    }

    public async Task KIV(long id, string notes, DateTimeOffset remindOn)
    {
      await _apiClient.RequestSdk.KIV(id, notes, remindOn);
    }

    public async Task RevertKIV(long id)
    {
      await _apiClient.RequestSdk.RevertKIV(id);
    }

    public async Task<JobOrder> ScheduleInspection(long id, DateTimeOffset scheduledOn,
      DateTimeOffset scheduledOnTo, string notes, Officer officer)
    {
      var request = await _apiClient.RequestSdk.GetByID(id);
      var customer = await _apiClient.CustomerSdk.GetByID(request.CustomerID);
      var premise = request.Premises.First(e => e.IsPrimary);

      var lineItems = new List<JobOrderLineItem>();
      foreach (var reqLineItem in request.LineItems)
      {
        var checklist = _apiClient.ChecklistSdk.GetLatest(reqLineItem.Scheme.Value);

        lineItems.Add(new JobOrderLineItem
        {
          Scheme = reqLineItem.Scheme.Value,
          SubScheme = reqLineItem.SubScheme,
          ChecklistHistoryID = checklist.Id
        });
      }

      var person = request.AgentID != null
        ? await _apiClient.PersonSdk.GetByID(request.AgentID.Value)
        : await _apiClient.PersonSdk.GetByID(request.RequestorID);

      var email = person.ContactInfos?.Where(e => e.Type == ContactInfoType.Email)
        .FirstOrDefault()?.Value;

      var jobOrder = new ScheduleJobOrderParam
      {
        RefID = id,
        Type = JobOrderType.Audit,
        Status = JobOrderStatus.Pending,
        ScheduledOn = scheduledOn,
        ScheduledOnTo = scheduledOnTo,
        Notes = notes,
        Customer = customer,
        Email= email,
        Officer = new Officer
        {
          ID = officer.ID,
          Name = officer.Name
        },
        Premises = new List<Premise>
        {
          premise
        },
        LineItems = lineItems
      };

      jobOrder = await _apiClient.JobOrderSdk.Submit(jobOrder);

      await _apiClient.RequestSdk.ScheduledInspection(id, jobOrder.ID, notes,
        scheduledOn, scheduledOnTo);

      return jobOrder;
    }

    public async Task BulkReviewIngredients(IList<Ingredient> ingredients)
    {
      await _apiClient.IngredientSdk.ReviewAll(ingredients);
    }

    public async Task BulkReviewMenus(IList<Menu> menus)
    {
      await _apiClient.MenuSdk.ReviewAll(menus);
    }

    public async Task Recommend(IList<Review> reviews)
    {
      var recommendParam = new RecommendParam();

      var hasRejectedItems = reviews.Any(e => e.LineItems.Any(li => !li.Approved.Value));

      if (hasRejectedItems)
      {
        var requestID = reviews.First().RequestID;
        var request = await _apiClient.RequestSdk.GetByID(requestID);

        var premise = request.Premises?.FirstOrDefault(e => e.IsPrimary);
        string node = premise.Postal.Substring(0, 2).Trim();

        IdentityFilter identityFilter = new IdentityFilter
        {
          Permissions = new Permission[] { Permission.RequestReviewApproval },
          Nodes = new string[] { node }
        };

        var officers = await _apiClient.IdentitySdk.List(identityFilter);

        if (!officers.Any())
        {
          throw new BadRequestException("[Missing Request.Review.Approval permission] Cannot " +
            "proceed with the recommendation. No assigned supervisor for the specific " +
            "cluster. Please contact administrator.");
        }

        recommendParam.AssignedTo = new Officer
        {
          ID = officers.First().ID,
          Name = officers.First().Name
        };
      }
      recommendParam.Reviews = reviews;

      await _apiClient.RequestSdk.Recommend(recommendParam);
    }

    public async Task Review(IList<Review> reviews)
    {
      var recommendParam = new RecommendParam();

      recommendParam.Reviews = reviews;

      await _apiClient.RequestSdk.Review(recommendParam);
    }

    public async Task Approve(IList<Review> reviews)
    {
      var email = reviews.FirstOrDefault(e => e.Email != null)?.Email;
      if (email != null
        && !string.IsNullOrEmpty(email.Body))
      {
        var regex = new Regex("<img.*?src=\"(.*?)\"[^>]+>",
          RegexOptions.IgnoreCase);
        var matches = regex.Matches(email.Body);

        for (int i = 0; i < matches.Count; i++)
        {
          var key = matches[i].Groups[1].Value;
          var uri = new Uri(key.Replace("../..", "http://localhost"));
          var queries = QueryHelpers.ParseQuery(uri.Query);

          var fileID = new Guid(uri.AbsolutePath.Substring(uri.AbsolutePath.LastIndexOf("/") + 1));
          var fileName = queries["fileName"];

          var file = await _apiClient.FileSdk.Download(fileID);

          if (email.Attachments == null)
          {
            email.Attachments = new List<EmailAttachment>();
          }

          email.Attachments.Add(new EmailAttachment
          {
            Key = key,
            Data = Convert.ToBase64String(file)
          });
        }
      }

      await _apiClient.RequestSdk.Approve(reviews);
    }

    public async Task Reaudit(long requestID, string remarks)
    {
      await _apiClient.RequestSdk.Reaudit(requestID, remarks);
    }

    public async Task<IList<Review>> GetReviews(long[] requestIDs)
    {
      return await _apiClient.RequestSdk.GetReviews(requestIDs);
    }

    public async Task Reassign(long requestID, Officer toOfficer, string notes)
    {
      await _apiClient.RequestSdk.Reassign(requestID, toOfficer, notes);
    }

    public async Task UpdateRequestStatus(long id, RequestStatus status,
     RequestStatusMinor? statusMinor)
    {
      await _apiClient.RequestSdk.UpdateRequestStatus(id, status, statusMinor);
    }

    public async Task ProceedToPayment(long id, long billID, IList<BillLineItem> billLines)
    {
      if (billLines?.Any() ?? false)
      {
        await _apiClient.BillSdk.AddLineItems(billID, billLines);
      }
      await _apiClient.RequestSdk.UpdateRequestStatus(id, RequestStatus.PendingPayment, null);
    }

    public Task<IList<Notes>> GetNotes(long id)
    {
      return _apiClient.NotesSdk.GetNotes(id);
    }

    public Task<Notes> AddNotes(long id, Notes notes)
    {
      notes.RequestID = id;
      return _apiClient.NotesSdk.AddNotes(notes);
    }

    public async Task ProceedForReview(long id)
    {
      // call identity to request for officer
      // call request micro to proceed for review 
      var request = await _apiClient.RequestSdk.GetByID(id);
      if (request != null)
      {
        ProceedForReviewParam param = new ProceedForReviewParam
        {
          RequestID = id
        };
        IdentityFilter identityFilter = new IdentityFilter
        {
          RequestTypes = new RequestType[] { request.Type }
        };
        var premise = request.Premises?.FirstOrDefault(e => e.IsPrimary);
        if (premise != null)
        {
          string node = premise.Postal.Substring(0, 2).Trim();
          var officer = await _apiClient.IdentitySdk.GetCertAuditor(node, identityFilter);
          if (officer != null)
          {
            Officer assignOfficer = new Officer
            {
              ID = officer.ID,
              Name = officer.Name
            };
            param.AssignOfficer = assignOfficer;
          }
          await _apiClient.RequestSdk.ProceedForReview(param);
        }
      }
    }

    public async Task Reinstate(long requestID, ReinstateParam param)
    {
      await _apiClient.RequestSdk.Reinstate(requestID, param);
    }
  }
}