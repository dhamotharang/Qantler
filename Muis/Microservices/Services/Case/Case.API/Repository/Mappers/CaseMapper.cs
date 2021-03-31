using Case.Model;
using Core.Model;
using System;
using System.Collections.Generic;

namespace Case.API.Repository.Mappers
{
  public class CaseMapper
  {
    readonly Dictionary<long, Model.Case> dict =
      new Dictionary<long, Model.Case>();

    readonly Dictionary<Guid, Master> _breachCategoriesCache =
      new Dictionary<Guid, Master>();

    readonly Dictionary<Guid, Master> _offencesCache =
     new Dictionary<Guid, Master>();

    readonly Dictionary<long, Attachment> _attachmentCache =
     new Dictionary<long, Attachment>();

    readonly Dictionary<long, Certificate> _certificatesCache =
     new Dictionary<long, Certificate>();

    readonly Dictionary<long, Premise> _premiseCache =
     new Dictionary<long, Premise>();

    readonly Dictionary<long, SanctionInfo> _sanctionInfoCache =
     new Dictionary<long, SanctionInfo>();

    readonly Dictionary<long, Activity> _activityCache =
     new Dictionary<long, Activity>();

    readonly Dictionary<long, Attachment> _activityAttachmentCache =
      new Dictionary<long, Attachment>();

    readonly Dictionary<long, Letter> _lettersCache =
      new Dictionary<long, Letter>();

    readonly Dictionary<long, ContactInfo> _contactInfoCache =
      new Dictionary<long, ContactInfo>();

    readonly Dictionary<long, ContactInfo> _offenderContactInfoCache =
      new Dictionary<long, ContactInfo>();

    public Model.Case Map(Model.Case @case,
      Offender offender = null,
      ContactInfo offenderContact = null,
      Officer managedBy = null,
      Officer assignedTo = null,
      Person reportedBy = null,
      ContactInfo reporterContact = null,
      Master breachCategories = null,
      Master offences = null,
      Attachment attachment = null,
      Certificate certificate = null,
      Premise premise = null,
      SanctionInfo sanctionInfo = null)
    {
      if (!dict.TryGetValue(@case.ID, out Model.Case result))
      {
        dict[@case.ID] = @case;

        _contactInfoCache.Clear();
        _breachCategoriesCache.Clear();
        _offencesCache.Clear();
        _attachmentCache.Clear();
        _certificatesCache.Clear();
        _premiseCache.Clear();
        _sanctionInfoCache.Clear();
        _activityCache.Clear();
        _offenderContactInfoCache.Clear();

        if (!string.IsNullOrEmpty(offender?.Name))
        {
          @case.Offender = offender;
        }

        if (!string.IsNullOrEmpty(managedBy?.Name))
        {
          @case.ManagedBy = managedBy;
        }

        if (!string.IsNullOrEmpty(assignedTo?.Name))
        {
          @case.AssignedTo = assignedTo;
        }

        if (!string.IsNullOrEmpty(reportedBy?.Name))
        {
          @case.ReportedBy = reportedBy;
        }
        result = @case;
      }

      if ((reporterContact?.ID ?? 0) > 0
          && !_contactInfoCache.ContainsKey(reporterContact.ID))
      {
        _contactInfoCache[reporterContact.ID] = reporterContact;

        if (result.ReportedBy.ContactInfos == null)
        {
          result.ReportedBy.ContactInfos = new List<ContactInfo>();
        }
        result.ReportedBy.ContactInfos.Add(reporterContact);
      }

      if ((offenderContact?.ID ?? 0) > 0
          && !_offenderContactInfoCache.ContainsKey(offenderContact.ID))
      {
        _offenderContactInfoCache[offenderContact.ID] = offenderContact;

        if (result.Offender.ContactInfos == null)
        {
          result.Offender.ContactInfos = new List<ContactInfo>();
        }
        result.Offender.ContactInfos.Add(offenderContact);
      }

      if ((breachCategories?.ID ?? Guid.Empty) != Guid.Empty
          && !_breachCategoriesCache.ContainsKey(breachCategories.ID))
      {
        _breachCategoriesCache[breachCategories.ID] = breachCategories;

        if (result.BreachCategories == null)
        {
          result.BreachCategories = new List<Master>();
        }
        result.BreachCategories.Add(breachCategories);
      }

      if ((offences?.ID ?? Guid.Empty) != Guid.Empty
          && !_offencesCache.ContainsKey(offences.ID))
      {
        _offencesCache[offences.ID] = offences;

        if (result.Offences == null)
        {
          result.Offences = new List<Master>();
        }
        result.Offences.Add(offences);
      }

      if ((attachment?.ID ?? 0) > 0
          && !_attachmentCache.ContainsKey(attachment.ID))
      {
        _attachmentCache[attachment.ID] = attachment;

        if (result.Attachments == null)
        {
          result.Attachments = new List<Attachment>();
        }
        result.Attachments.Add(attachment);
      }

      if ((certificate?.ID ?? 0) > 0
          && !_certificatesCache.ContainsKey(certificate.ID))
      {
        _certificatesCache[certificate.ID] = certificate;

        if (result.Certificates == null)
        {
          result.Certificates = new List<Certificate>();
        }
        result.Certificates.Add(certificate);
      }

      if ((premise?.ID ?? 0) > 0
          && !_premiseCache.ContainsKey(premise.ID))
      {
        _premiseCache[premise.ID] = premise;

        if (result.Premises == null)
        {
          result.Premises = new List<Premise>();
        }
        result.Premises.Add(premise);
      }

      if ((sanctionInfo?.ID ?? 0) > 0
          && !_sanctionInfoCache.ContainsKey(sanctionInfo.ID))
      {
        _sanctionInfoCache[sanctionInfo.ID] = sanctionInfo;

        if (result.SanctionInfos == null)
        {
          result.SanctionInfos = new List<SanctionInfo>();
        }
        result.SanctionInfos.Add(sanctionInfo);
      }

      return result;
    }

    public Activity MapActivity(
      Activity activity = null,
      Officer activityUser = null,
      Attachment activityAttachment = null,
      Letter letter = null,
      Email email = null)
    {
      if (!_activityCache.TryGetValue(activity.ID, out Model.Activity result))
      {
        _activityCache[activity.ID] = activity;
        if (!string.IsNullOrEmpty(activityUser?.Name))
        {
          activity.User = activityUser;
        }
        result = activity;
      }

      Attachment outAttachment = null;
      if ((activityAttachment?.ID ?? 0) > 0
          && !_activityAttachmentCache.TryGetValue(activityAttachment.ID, out outAttachment))
      {
        _activityAttachmentCache[activityAttachment.ID] = activityAttachment;

        if (result.Attachments == null)
        {
          result.Attachments = new List<Attachment>();
        }

        result.Attachments.Add(activityAttachment);
      }

      Letter outLetter = null;
      if ((letter?.ID ?? 0) > 0
          && !_lettersCache.TryGetValue(letter.ID, out outLetter))
      {
        _lettersCache[letter.ID] = letter;

        if (result.Letters == null)
        {
          result.Letters = new List<Letter>();
        }

        if (!string.IsNullOrEmpty(email?.From))
        {
          letter.Email = email;
        }

        result.Letters.Add(letter);
      }

      return result;
    }
  }
}