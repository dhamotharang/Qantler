using System;
using System.Collections.Generic;
using Core.Model;

namespace Finance.Model
{
  public static class TransactionCodes
  {
    public const string ApplicationFeeNormal = "APPFN";
    public const string ApplicationFeeExpress = "APPFX";

    public const string PoultryCertFee = "PACF";
    public const string PoultryTag = "PALF";

    public const string StorageCertFee = "SFCF";

    public const string EndoresementCertFee = "ENCF";
    public const string EndoresementHalalCertMark = "ENHCM";

    public const string CentralKitchenSmallCertFee = "FPACKSCF";
    public const string CentralKitchenLargeCertFee = "FPACKLCF";
    public const string PreSchoolKitchenCertFee = "FPAPSKCF";
    public const string CateringCertFee = "FPACACF";

    public const string Hawker1CertFee = "EEH1CF";
    public const string Hawker2CertFee = "EEH2CF";
    public const string Restaurant1CertFee = "EER1CF";
    public const string Restaurant2CertFee = "EER2CF";
    public const string SnackBarBakeryCertFee = "EESBBCF";
    public const string HalalSectionCertFee = "EEHSCF";
    public const string SchoolCanteenStallCertFee = "EESCCF";
    public const string ShortTermCertFee = "EESTCF";
    public const string FoodStationCertFee = "EEFSCF";
    public const string FoodKioskCertFee = "EEFKCF";
    public const string StaffCanteenCertFee = "EESCCF";

    public const string Product1CertFee = "PR1CF";
    public const string Product2CertFee = "PR2CF";
    public const string Product3CertFee = "PR3CF";
    public const string Product4CertFee = "PR4CF";
    public const string ProductHalalCertMark = "PRHCM";

    public const string WholePlant1CertFee = "WP1CF";
    public const string WholePlant2CertFee = "WP2CF";
    public const string WholePlant3CertFee = "WP3CF";
    public const string WholePlant4CertFee = "WP4CF";
    public const string WholePlantHalalCertMark = "WPHCM";

    public const string CertifiedTrueCopyFee = "CTCFEE";

    public const string CompositionSum = "COMSUM";
  }

  public class TransactionCode
  {
    public long ID { get; set; }

    public string Code { get; set; }

    public string GLEntry { get; set; }

    public string Text { get; set; }

    public string Currency { get; set; }

    public bool IsBillable { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public IList<Price> PriceHistory { get; set; }

    public IList<Condition> Conditions { get; set; }

    public IList<Log> Logs { get; set; }

    #endregion
  }
}
