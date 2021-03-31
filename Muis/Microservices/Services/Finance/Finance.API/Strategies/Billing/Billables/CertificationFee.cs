using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using Finance.API.Extensions;
using Finance.API.Helpers;
using Finance.Model;

namespace Finance.API.Strategies.Billing.Billables
{
  public class CertificateFee : IBillable
  {
    readonly BillingContext _context;

    public CertificateFee(BillingContext context)
    {
      _context = context;
    }

    public async Task<IList<BillLineItem>> Generate(BillRequest request)
    {
      var result = new List<BillLineItem>();
      if (request.RequestType != BillRequestType.New
          && request.RequestType != BillRequestType.Renewal)
      {
        return result;
      }

      if (request.LineItems?.Any() ?? false)
      {
        foreach (var item in request.LineItems)
        {
          await BuildLineItem(result, request, item);
        }
      }

      return result;
    }

    async Task BuildLineItem(IList<BillLineItem> container, BillRequest request,
      BillRequestLineItem item)
    {
      // No stage 2 payments
      if (request.Type == BillType.Stage2
          && (item.Scheme == Scheme.Endorsement
             || item.SubScheme == SubScheme.Canteen
             || item.SubScheme == SubScheme.ShortTerm))
      {
        return;
      }

      var codes = await GetCertFeeCodes(request.ReferenceDate, item.Scheme, item.SubScheme);

      var code = codes.Count() == 1
        ? codes.First()
        : codes.FirstOrDefault(e => ConditionEvaluator.Evaluate(item.Area, e.Conditions?.ToArray()));

      if (code == null)
      {
        return;
      }

      if (request.Type == BillType.Stage1)
      {
        await Stage1CertFee(container, code, item);
      }
      else if (request.Type == BillType.Stage2)
      {
        await Stage2CertFee(request.RequestType, container, code, item);
      }
    }

    async Task Stage1CertFee(IList<BillLineItem> container, TransactionCode code,
      BillRequestLineItem item)
    {
      var duration = item.SubScheme == SubScheme.ShortTerm
        ? (item.ExpiresOn - item.StartsFrom).Days + 1
        : BillingUtils.CalculateNoOfMonths(item.StartsFrom.Date, item.ExpiresOn.Date);

      var factor = await _context.Stage1Factor();

      // Full payment for stage 1
      if (item.Scheme == Scheme.Endorsement
         || item.SubScheme == SubScheme.Canteen
         || item.SubScheme == SubScheme.ShortTerm)
      {
        factor = 1M;
      }
      // Short term billed per 7 days
      else if (item.SubScheme == SubScheme.ShortTerm)
      {
        factor = Math.Ceiling(duration / 7M);
      }
      else
      {
        factor = Math.Min(factor, duration / 12M);
      }

      // For case where the pro-rated value is less than stage 1 expected factor.
      factor = Math.Min(duration / 12, factor);

      var unitPrice = code.GetLatestPriceAmount();
      var gst = await _context.GST();
      var qty = factor;
      var fee = qty * unitPrice;
      var gstAmount = fee * gst;

      var section = BillingUtils.LineItemSection(BillType.Stage1);

      var yearPrefix = duration > 12 ? " - Year 1" : "";
      string prorateText = duration < 12 ? $" ({duration} of 12 months)" : "";

      container.Add(new BillLineItem
      {
        SectionIndex = section.Item1,
        Section = section.Item2,
        Qty = qty,
        CodeID = code.ID,
        Code = code.Code,
        Descr = $"{code.Text}{yearPrefix}{prorateText}",
        UnitPrice = code.GetLatestPriceAmount(),
        Amount = decimal.Round(fee, 2),
        GSTAmount = decimal.Round(gstAmount, 2),
        GST = gst,
        WillRecord = true
      });
    }

    async Task Stage2CertFee(BillRequestType requestType, IList<BillLineItem> container,
      TransactionCode code, BillRequestLineItem item)
    {
      var totalDuration = BillingUtils.CalculateNoOfMonths(item.StartsFrom.Date, item.ExpiresOn.Date);

      var stage1Factor = await _context.Stage1Factor();

      var prorate = Math.Round(totalDuration / 12M, 2);

      // No stage 2 payment for prorated application whose factor is less than stage 1 payment.
      if (   prorate < stage1Factor
          && requestType != BillRequestType.Renewal)
      {
        return;
      }

      var section = BillingUtils.LineItemSection(BillType.Stage2);

      decimal factor;

      int i = 0;
      var duration = (decimal)totalDuration;
      do
      {
        prorate = Math.Min(Math.Round(duration / 12M, 2), 1M);

        // First year factor. Renewal will only have stage 2 payment.
        if (   i == 0
            && requestType != BillRequestType.Renewal)
        {
          factor = Math.Max(Math.Min(
            1M - stage1Factor,
            prorate - stage1Factor), 0);
        }
        else
        {
          factor = Math.Min(1M, prorate);
        }

        if (factor == 0M)
        {
          break;
        }

        var unitPrice = code.GetLatestPriceAmount();
        var gst = await _context.GST();
        var qty = factor;
        var fee = qty * unitPrice;
        var gstAmount = fee * gst;

        var yearPrefix = totalDuration > 12 ? $" - Year {i + 1}" : "";
        var prorateText = duration < 12 ? $" ({(int)duration} of 12 months)" : "";

        container.Add(new BillLineItem
        {
          SectionIndex = section.Item1,
          Section = section.Item2,
          Qty = qty,
          CodeID = code.ID,
          Code = code.Code,
          Descr = $"{code.Text}{yearPrefix}{prorateText}",
          UnitPrice = code.GetLatestPriceAmount(),
          Amount = decimal.Round(fee, 2),
          GSTAmount = decimal.Round(gstAmount, 2),
          GST = gst,
          WillRecord = true
        });

        duration -= 12;
        i++;
      } while (duration > 0);
    }

    async Task<IList<TransactionCode>> GetCertFeeCodes(DateTimeOffset refDate,
      Scheme scheme,
      SubScheme? subScheme)
    {
      string[] codes = null;

      switch (scheme)
      {
        case Scheme.FoodManufacturing:

          switch (subScheme)
          {
            case SubScheme.Product:

              codes = new string[] {
                TransactionCodes.Product1CertFee,
                TransactionCodes.Product2CertFee,
                TransactionCodes.Product3CertFee,
                TransactionCodes.Product4CertFee
              };

              break;
            case SubScheme.WholePlant:

              codes = new string[] {
                TransactionCodes.WholePlant1CertFee,
                TransactionCodes.WholePlant2CertFee,
                TransactionCodes.WholePlant3CertFee,
                TransactionCodes.WholePlant4CertFee
              };

              break;
          }

          break;
        case Scheme.Poultry:
          codes = new string[] { TransactionCodes.PoultryCertFee };
          break;
        case Scheme.StorageFacility:
          codes = new string[] { TransactionCodes.StorageCertFee };
          break;
        case Scheme.Endorsement:
          codes = new string[] { TransactionCodes.EndoresementCertFee };
          break;
        case Scheme.FoodPreparationArea:

          switch (subScheme)
          {
            case SubScheme.CentralKitchen:
              codes = new string[] {
                TransactionCodes.CentralKitchenSmallCertFee,
                TransactionCodes.CentralKitchenLargeCertFee
              };
              break;
            case SubScheme.Catering:
              codes = new string[] { TransactionCodes.CateringCertFee };
              break;
            case SubScheme.PreSchoolKitchen:
              codes = new string[] { TransactionCodes.PreSchoolKitchenCertFee };
              break;
          }

          break;
        case Scheme.EatingEstablishment:

          switch (subScheme)
          {
            case SubScheme.Canteen:
              codes = new string[] {
                TransactionCodes.SchoolCanteenStallCertFee,
                TransactionCodes.WholePlant2CertFee
              };
              break;
            case SubScheme.HalalSection:
              codes = new string[] { TransactionCodes.HalalSectionCertFee };
              break;
            case SubScheme.Hawker:
              codes = new string[] {
                TransactionCodes.Hawker1CertFee,
                TransactionCodes.Hawker2CertFee
              };
              break;
            case SubScheme.FoodKiosk:
              codes = new string[] { TransactionCodes.FoodKioskCertFee };
              break;
            case SubScheme.FoodStation:
              codes = new string[] { TransactionCodes.FoodStationCertFee };
              break;
            case SubScheme.Restaurant:
              codes = new string[] {
                TransactionCodes.Restaurant1CertFee,
                TransactionCodes.Restaurant2CertFee
              };
              break;
            case SubScheme.ShortTerm:
              codes = new string[] { TransactionCodes.ShortTermCertFee };
              break;
            case SubScheme.SnackBarBakery:
              codes = new string[] { TransactionCodes.SnackBarBakeryCertFee };
              break;
            case SubScheme.StaffCanteen:
              codes = new string[] { TransactionCodes.StaffCanteenCertFee };
              break;
          }

          break;
      }

      return await _context.GetTransactionCodes(refDate, codes);
    }
  }
}
