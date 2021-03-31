using eHS.Portal.Configs;
using eHS.Portal.Infrastructure.Providers;
using Microsoft.Extensions.Options;

namespace eHS.Portal.Client
{
  public class ApiClient
  {
    readonly UrlConfig _urlConfig;
    readonly IHttpRequestProvider _requestProvider;

    public ApiClient(IOptions<UrlConfig> urlConfig, IHttpRequestProvider requestProvider)
    {
      _urlConfig = urlConfig.Value;
      _requestProvider = requestProvider;
    }

    RequestSdk _requestSdk;
    public RequestSdk RequestSdk =>
      _requestSdk ??= new RequestSdk(_urlConfig.Request, _requestProvider);

    IdentitySdk _identitysdk;
    public IdentitySdk IdentitySdk => _identitysdk
      ??= new IdentitySdk(_urlConfig.Identity, _requestProvider);

    FileSdk _fileSdk;
    public FileSdk FileSdk =>
      _fileSdk ??= new FileSdk(_urlConfig.File, _requestProvider);

    RFASdk _rfaSdk;
    public RFASdk RFASdk =>
      _rfaSdk ??= new RFASdk(_urlConfig.Request, _requestProvider);

    ChecklistSdk _checklistSdk;
    public ChecklistSdk ChecklistSdk =>
      _checklistSdk ??= new ChecklistSdk(_urlConfig.Identity, _requestProvider);

    NotificationSdk _notificationSdk;
    public NotificationSdk NotificationSdk =>
      _notificationSdk ??= new NotificationSdk(_urlConfig.Notification, _requestProvider);

    CustomerSdk _customerSdk;
    public CustomerSdk CustomerSdk =>
      _customerSdk ??= new CustomerSdk(_urlConfig.Identity, _urlConfig.Request, _requestProvider);

    PersonSdk _personSdk;
    public PersonSdk PersonSdk =>
      _personSdk ??= new PersonSdk(_urlConfig.Identity, _requestProvider);

    JobOrderSdk _jobOrderSdk;
    public JobOrderSdk JobOrderSdk =>
      _jobOrderSdk ??= new JobOrderSdk(_urlConfig.JobOrder, _requestProvider);

    MenuSdk _menuSdk;
    public MenuSdk MenuSdk =>
      _menuSdk ??= new MenuSdk(_urlConfig.Request, _requestProvider);

    IngredientSdk _ingredientSdk;
    public IngredientSdk IngredientSdk =>
      _ingredientSdk ??= new IngredientSdk(_urlConfig.Request, _requestProvider);

    RequestSettingsSdk _requestSettingsSdk;
    public RequestSettingsSdk RequestSettingsSdk =>
      _requestSettingsSdk ??= new RequestSettingsSdk(_urlConfig.Request, _requestProvider);

    JobOrderSettingsSdk _jobOrderSettingsSdk;
    public JobOrderSettingsSdk JobOrderSettingsSdk =>
      _jobOrderSettingsSdk ??= new JobOrderSettingsSdk(_urlConfig.JobOrder, _requestProvider);

    TransactionCodeSdk _transactionCodeSdk;
    public TransactionCodeSdk TransactionCodeSdk =>
      _transactionCodeSdk ??= new TransactionCodeSdk(_urlConfig.Finance, _requestProvider);

    RequestEmailSdk _requestEmailSdk;
    public RequestEmailSdk RequestEmailSdk =>
      _requestEmailSdk ??= new RequestEmailSdk(_urlConfig.Request, _requestProvider);

    IdentityEmailSdk _identityEmailSdk;
    public IdentityEmailSdk IdentityEmailSdk =>
      _identityEmailSdk ??= new IdentityEmailSdk(_urlConfig.Identity, _requestProvider);

    ClusterSdk _clusterSdk;
    public ClusterSdk ClusterSdk =>
      _clusterSdk ??= new ClusterSdk(_urlConfig.Identity, _requestProvider);

    CertificateSdk _certificateSdk;
    public CertificateSdk CertificateSdk =>
      _certificateSdk ??= new CertificateSdk(_urlConfig.Request, _requestProvider);

    PaymentSdk _paymentSdk;
    public PaymentSdk PaymentSdk =>
      _paymentSdk ??= new PaymentSdk(_urlConfig.Finance, _requestProvider);

    BillSdk _billSdk;
    public BillSdk BillSdk =>
      _billSdk ??= new BillSdk(_urlConfig.Finance, _requestProvider);

    AuthSdk _authSdk;
    public AuthSdk AuthSdk =>
      _authSdk ??= new AuthSdk(_urlConfig.Identity, _requestProvider);

    CustomerCodeSdk _customerCodeSdk;
    public CustomerCodeSdk CustomerCodeSdk =>
      _customerCodeSdk ??= new CustomerCodeSdk(_urlConfig.Request, _requestProvider);

    JobOrderMasterSdk _jobOrderMasterSdk;
    public JobOrderMasterSdk JobOrderMasterSdk =>
      _jobOrderMasterSdk ??= new JobOrderMasterSdk(_urlConfig.JobOrder);

    RequestMasterSdk _requestMasterSdk;
    public RequestMasterSdk RequestMasterSdk =>
      _requestMasterSdk ??= new RequestMasterSdk(_urlConfig.Request);

    NotesSdk _notesSdk;
    public NotesSdk NotesSdk =>
      _notesSdk ??= new NotesSdk(_urlConfig.Request, _requestProvider);

    HalalLibrarySdk _halalLibrarySdk;
    public HalalLibrarySdk HalalLibrarySdk =>
      _halalLibrarySdk ??= new HalalLibrarySdk(_urlConfig.HalalLibrary, _requestProvider);

    JobOrderNotesSdk _jobOrderNotesSdk;
    public JobOrderNotesSdk JobOrderNotesSdk =>
      _jobOrderNotesSdk ??= new JobOrderNotesSdk(_urlConfig.JobOrder, _requestProvider);

    CaseSdk _caseSdk;
    public CaseSdk CaseSdk =>
      _caseSdk ??= new CaseSdk(_urlConfig.Case, _requestProvider);

    CaseMasterSdk _caseMasterSdk;
    public CaseMasterSdk CaseMasterSdk =>
      _caseMasterSdk ??= new CaseMasterSdk(_urlConfig.Case, _requestProvider);

    PremiseSdk _premisesdk;
    public PremiseSdk PremiseSdk =>
      _premisesdk ??= new PremiseSdk(_urlConfig.Request, _requestProvider);

    Certificate360Sdk _certificate360;
    public Certificate360Sdk Certificate360 =>
      _certificate360 ??= new Certificate360Sdk(_urlConfig.Request, _requestProvider);

    JobOrderEmailSdk _jobOrderEmailSdk;
    public JobOrderEmailSdk JobOrderEmailSdk =>
      _jobOrderEmailSdk ??= new JobOrderEmailSdk(_urlConfig.JobOrder, _requestProvider);

    CaseEmailSdk _caseEmailSdk;
    public CaseEmailSdk CaseEmailSdk =>
      _caseEmailSdk ??= new CaseEmailSdk(_urlConfig.Case, _requestProvider);

    CaseLetterSdk _caseLetterSdk;
    public CaseLetterSdk CaseLetterSdk =>
      _caseLetterSdk ??= new CaseLetterSdk(_urlConfig.Case, _requestProvider);
      
    RequestStatisticsSdk _requestStatisSdk;
    public RequestStatisticsSdk RequestStatisticsSdk =>
      _requestStatisSdk ??= new RequestStatisticsSdk(_urlConfig.Request, _requestProvider);

    FinanceMasterSdk _financeMasterSdk;
    public FinanceMasterSdk FinanceMasterSdk =>
      _financeMasterSdk ??= new FinanceMasterSdk(_urlConfig.Finance, _requestProvider);

    BankSdk _bankSdk;
    public BankSdk BankSdk =>
      _bankSdk ??= new BankSdk(_urlConfig.Finance, _requestProvider);
  }
}
