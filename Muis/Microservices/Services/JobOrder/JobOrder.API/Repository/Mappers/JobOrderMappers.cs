using JobOrder.Model;
using System;
using System.Collections.Generic;
using Core.Model;
using System.Linq;

namespace JobOrder.API.Repository.Mappers
{
  public class JobOrderMapper
  {
    readonly Dictionary<long, Model.JobOrder> dict =
      new Dictionary<long, Model.JobOrder>();

    readonly Dictionary<long, Attendee> _attendeeCache =
      new Dictionary<long, Attendee>();

    readonly Dictionary<long, Log> _logCache =
      new Dictionary<long, Log>();

    readonly Dictionary<long, Attachment> _findingsLineItemAttachmentCache =
      new Dictionary<long, Attachment>();

    readonly Dictionary<long, JobOrderLineItem> _liCache =
      new Dictionary<long, JobOrderLineItem>();

    readonly Dictionary<long, Findings> _findingsCache =
      new Dictionary<long, Findings>();

    readonly Dictionary<long, FindingsLineItem> _findingsLineItemCache =
      new Dictionary<long, FindingsLineItem>();

    readonly Dictionary<long, ContactInfo> _contactInfoCache =
      new Dictionary<long, ContactInfo>();

    readonly IDictionary<Guid, Person> _personCache =
      new Dictionary<Guid, Person>();

    public Model.JobOrder Map(Model.JobOrder joborder,
      JobOrderLineItem lineitem = null,
      Premise premise = null,
      Customer customer = null,
      Person person = null,
      ContactInfo contactInfo = null,
      Attendee attendee = null,
      Officer officer = null,
      Attachment attachment = null,
      Officer invitee = null,
      Log log = null,
      Findings findings = null,
      Officer findingsOfficer = null,
      FindingsLineItem findingsLineItem = null,
      Attachment findingsLineItemAttachment = null,
      Attachment signature = null)
    {
      if (!dict.TryGetValue(joborder.ID, out Model.JobOrder result))
      {
        dict[joborder.ID] = joborder;

        if (!string.IsNullOrEmpty(officer?.Name))
        {
          joborder.Officer = officer;
        }

        if (!string.IsNullOrEmpty(customer?.Name))
        {
          joborder.Customer = customer;
        }

        result = joborder;
      }

      if ((lineitem?.ID ?? 0) > 0
          && !_liCache.ContainsKey(lineitem.ID))
      {
        _liCache[lineitem.ID] = lineitem;

        if (result.LineItems == null)
        {
          result.LineItems = new List<JobOrderLineItem>();
        }
        result.LineItems.Add(lineitem);
      }

      if ((premise?.ID ?? 0) > 0
          && (result.Premises?.FirstOrDefault(e => e.ID == premise.ID) == null))
      {
        if (result.Premises == null)
        {
          result.Premises = new List<Premise>();
        }
        result.Premises.Add(premise);
      }

      if ((attendee?.ID ?? 0) > 0
          && !_attendeeCache.ContainsKey(attendee.ID))
      {
        _attendeeCache[attendee.ID] = attendee;

        if (result.Attendees == null)
        {
          result.Attendees = new List<Attendee>();
        }

        result.Attendees.Add(attendee);
      }

      if ((log?.ID ?? 0) > 0
          && !_logCache.ContainsKey(log.ID))
      {
        _logCache[log.ID] = log;

        if (result.Logs == null)
        {
          result.Logs = new List<Log>();
        }
        result.Logs.Add(log);
      }

      if (!string.IsNullOrEmpty(invitee?.Name)
          && result.Invitees?.FirstOrDefault(e => e.ID == invitee.ID) == null)
      {
        if (result.Invitees == null)
        {
          result.Invitees = new List<Officer>();
        }

        result.Invitees.Add(invitee);
      }

      Findings outFindings = null;
      if ((findings?.ID ?? 0) > 0
          && !_findingsCache.TryGetValue(findings.ID, out outFindings))
      {
        _findingsCache[findings.ID] = findings;

        if (result.Findings == null)
        {
          result.Findings = new List<Findings>();
        }

        if (!string.IsNullOrEmpty(findingsOfficer?.Name))
        {
          findings.Officer = findingsOfficer;
        }

        if ((signature?.ID ?? 0) > 0)
        {
          findings.Attachment = signature;
        }

        result.Findings.Add(findings);
        outFindings = findings;
      }

      FindingsLineItem outFindingsLineItem = null;
      if ((findingsLineItem?.ID ?? 0) > 0
          && !_findingsLineItemCache.TryGetValue(findingsLineItem.ID, out outFindingsLineItem))
      {
        _findingsLineItemCache[findingsLineItem.ID] = findingsLineItem;

        if (outFindings.LineItems == null)
        {
          outFindings.LineItems = new List<FindingsLineItem>();
        }

        outFindings.LineItems.Add(findingsLineItem);

        outFindingsLineItem = findingsLineItem;
      }

      if ((findingsLineItemAttachment?.ID ?? 0) > 0
        && !_findingsLineItemAttachmentCache.ContainsKey(findingsLineItemAttachment.ID))
      {
        _findingsLineItemAttachmentCache[findingsLineItemAttachment.ID] = findingsLineItemAttachment;

        if (outFindingsLineItem.Attachments == null)
        {
          outFindingsLineItem.Attachments = new List<Attachment>();
        }
        outFindingsLineItem.Attachments.Add(findingsLineItemAttachment);
      }

      Person outContactPerson = null;
      if (person?.ID != null
        && person.ID != Guid.Empty
        && !_personCache.TryGetValue(person.ID, out outContactPerson))
      {
        _personCache[person.ID] = person;

        outContactPerson = person;        
      }

      result.ContactPerson = outContactPerson;


      if ((contactInfo?.ID ?? 0L) != 0L
        && !_contactInfoCache.ContainsKey(contactInfo.ID))
      {
        _contactInfoCache[contactInfo.ID] = contactInfo;

        if (outContactPerson.ContactInfos == null)
        {
          outContactPerson.ContactInfos = new List<ContactInfo>();
        }
        outContactPerson.ContactInfos.Add(contactInfo);
      }

      return result;
    }
  }
}