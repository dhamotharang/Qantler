using System;
using System.Collections.Generic;
using Finance.Model;
using Core.Model;
using System.Linq;

namespace Finance.API.Repository.Mappers
{
  public class PaymentMapper
  {
    readonly IDictionary<long, Payment> _cache = new Dictionary<long, Payment>();

    readonly IDictionary<long, Bill> _billCache = new Dictionary<long, Bill>();

    readonly IDictionary<long, BillLineItem> _billLineItemCache =
      new Dictionary<long, BillLineItem>();

    readonly IDictionary<long, Note> _noteCache = new Dictionary<long, Note>();

    readonly IDictionary<Guid, Person> _personCache =
      new Dictionary<Guid, Person>();

    readonly IDictionary<long, ContactInfo> _contactInfoCache =
      new Dictionary<long, ContactInfo>();

    public Payment Map(Payment payment, Bill bill = null, BillLineItem billLineItem = null,
      Note note = null, Attachment attachment = null, Officer officer = null, Log log = null,
      Person contactPerson = null, Bank bank = null, ContactInfo contactInfo = null)
    {
      if (!_cache.TryGetValue(payment.ID, out Payment result))
      {
        if (bank?.ID > 0)
        {
          payment.Bank = bank;
        }

        result = payment;
        _cache[payment.ID] = payment;
      }

      Bill outBill = null;
      if ((bill?.ID ?? 0) > 0
          && !_billCache.TryGetValue(bill.ID, out outBill))
      {
        _billCache[bill.ID] = bill;
        outBill = bill;

        if (result.Bills == null)
        {
          result.Bills = new List<Bill>();
        }
        result.Bills.Add(bill);
      }

      if ((billLineItem?.ID ?? 0) > 0
          && !_billLineItemCache.ContainsKey(billLineItem.ID))
      {
        _billLineItemCache[billLineItem.ID] = billLineItem;

        if (outBill.LineItems == null)
        {
          outBill.LineItems = new List<BillLineItem>();
        }

        outBill.LineItems.Add(billLineItem);
      }

      Note outNote = null;
      if ((note?.ID ?? 0) > 0
          && !_noteCache.TryGetValue(note.ID, out outNote))
      {
        if (!string.IsNullOrEmpty(officer?.Name))
        {
          note.Officer = officer;
        }

        _noteCache[note.ID] = note;
        outNote = note;

        if (result.Notes == null)
        {
          result.Notes = new List<Note>();
        }
        result.Notes.Add(note);
      }

      if (attachment?.ID > 0
        && !(outNote.Attachments?.Any(e => e.ID == attachment.ID) ?? false))
      {
        if (outNote.Attachments == null)
        {
          outNote.Attachments = new List<Attachment>();
        }
        outNote.Attachments.Add(attachment);
      }

      if (log?.ID > 0
       && !(result.Logs?.Any(e => e.ID == log.ID) ?? false))
      {
        if (result.Logs == null)
        {
          result.Logs = new List<Log>();
        }
        result.Logs.Add(log);
      }

      Person outContactPerson = null;
      if (contactPerson?.ID != null
        && contactPerson.ID != Guid.Empty
        && !_personCache.TryGetValue(contactPerson.ID, out outContactPerson))
      {
        _personCache[contactPerson.ID] = contactPerson;

        outContactPerson = contactPerson;
        payment.ContactPerson = contactPerson;
      }

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
