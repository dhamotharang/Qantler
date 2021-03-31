import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,
  TemplateRef
} from "@angular/core";
import { CommonService } from "src/app/common.service";
import { CitizenCreateModal } from "./citizen-modal";
import { CitizenAffairService } from "../../../service/citizen-affair.service";
import { ActivatedRoute, Router } from "@angular/router";
import { citizenData } from "./data";
import { HttpEventType } from "@angular/common/http";
import {
  TabHeadingDirective,
  BsModalRef,
  BsModalService,
  BsDatepickerConfig
} from "ngx-bootstrap";
import { SuccessComponent } from "../../../../modal/success-popup/success.component";
import { ModalComponent } from "src/app/modal/modalcomponent/modal.component";
import { environment } from "src/environments/environment";
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: "app-citizen-affair-create",
  templateUrl: "./citizen-affair-create.component.html",
  styleUrls: ["./citizen-affair-create.component.scss"]
})
export class CitizenAffairCreateComponent implements OnInit {
  @ViewChild("profile_upload") profile_upload: ElementRef;
  @ViewChild("photoFile") photoFile: ElementRef;
  @ViewChild("documentFile") documentFile: ElementRef;
  requestTypes = ["FieldVisit", "Personal Report"];
  requestType = "";
  url: string | ArrayBuffer;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  citizenCreateModal: CitizenCreateModal = new CitizenCreateModal();
  Data: citizenData = new citizenData();
  screenStatus: any = "Create";
  person_UserList: Promise<void>;
  approver_UserList: Promise<void>;
  userList: Promise<void>;
  location = false;
  emailErr = "";
  environment = environment;
  citizenModal: any = {
    CitizenAffairID: 0,
    currentApprover: 0,
    SourceOU: "",
    SourceName: "",
    Status: "",
    ReferenceNumber: "",
    RequestType: "",
    ApproverId: "",
    ApproverDepartmentId: "",
    NotifyUpon: "",
    InternalRequestorID: "",
    InternalRequestorDepartmentID: "",
    ExternalRequestEmailID: "",
    CreatedBy: "",
    CreatedDateTime: "",
    HistoryLog: [],
    Action: "",
    Comments: "",
    Photos: [
      {
        AttachmentGuid: "",
        AttachmentsName: "",
        CitizenAffairID: ""
      }
    ],
    Documents: [
      {
        AttachmentGuid: "",
        AttachmentsName: "",
        CitizenAffairID: ""
      }
    ],
    PersonalReport: {
      Destination: "",
      ProfilePhotoID: "",
      ProfilePhotoName: "",
      Emirates: 0,
      EmiratesID: "",
      Employer: "",
      FindingNotes: "",
      MaritalStatus: "",
      MonthlySalary: "",
      Name: "",
      NoOfChildrens: "",
      PhoneNumber: "",
      Recommendation: "",
      ReportObjectives: "",
      City: "",
      Age: ""
    },
    FieldVisit: {
      City: 0,
      Date: "",
      EmiratesID: "",
      Emirates: 0,
      FindingNotes: "",
      ForWhom: "",
      Location: "",
      CityID: 0,
      LocationName: "",
      Name: "",
      PhoneNumber: "",
      RequetsedBy: "",
      VisitObjective: ""
    }
  };
  uploadPhotoProcess: boolean = false;
  uploadDocumentProcess: boolean = false;
  uploadDocumentPercentage: number = 0;
  uploadPhotoPercentage: number = 0;
  photoAttachmentsList: any = [];
  documentAttachmentsList: any = [];
  currentUser: any = JSON.parse(localStorage.getItem("User"));
  organizationList: any = [];
  ApproverDepartmentList: any = [];
  btnLoad: any = false;
  btnLoadSubmit = false;
  btnLoadClose = false;
  btnLoadApprove = false;
  btnLoadDraft = false;
  btnLoadInfo = false;
  btnLoadReject = false;
  btnLoadDelete = false;
  submitBtn: boolean = false;
  redirectBtn: boolean = false;
  rejectBtn: boolean = false;
  infoBtn: boolean = false;
  draftBtn: boolean = false;
  escalateBtn: boolean = false;
  approveBtn: boolean = false;
  closeBtn: boolean = false;
  printBtn: boolean = false;
  deleteBtn = false;
  emiratesList: Object;
  cityList: Object;
  locationList: Object;
  bsModalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  statusList: any;
  showPdf: boolean;
  pdfSrc: any;
  uploadProfileProcess: boolean;
  uploadProfilePercentage: number = 0;
  emailFlag: boolean = false;
  citizenTitle: string;
  fieldVisit: any;
  personal: any;
  currentViewMsg: any;
  imageURL = environment.AttachmentDownloadUrl;
  cityListFiled: any;

  validateStartEndDate: any = {
    isValid: true,
    msg: ''
  };

  // constructor
  constructor(
    private common: CommonService,
    public service: CitizenAffairService,
    public router: Router,
    route: ActivatedRoute,
    public utilsService: UtilsService,
    private modalService: BsModalService
  ) {
    var creation =
      this.common.language == "English"
        ? "Personal Report/Field Visit Report Creation"
        : this.arabic("personal/fieldvisitreportcreation"),
      view =
        this.common.language == "English"
          ? "Personal Report/Field Visit Report View"
          : this.arabic("personal/fieldvisitreportview"),
      edit = this.common.language == "English" ? "Edit" : this.arabic("edit"),
      reportText =
        this.common.language == "English"
          ? "Personal Report/Field Visit Report "
          : this.arabic("personal/fieldvisitreport");
    this.currentViewMsg = creation;
    this.citizenCreateModal = this.citizenCreateModal;
    route.url.subscribe(() => {
      console.log(route.snapshot.data);
      this.screenStatus = route.snapshot.data.title;
    });
    this.getDataById(0);
    route.params.subscribe(param => {
      var id = +param.id;
      if (id > 0) {
        this.getDataById(id);
        this.currentViewMsg =
          this.screenStatus == "View" ? view : reportText + edit;
      }
    });
    var citizen =
      this.common.language == "English"
        ? "Citizen Affair"
        : this.arabic("citizenaffair");
    this.common.breadscrumChange(citizen, this.currentViewMsg, "");
    this.citizenTitle = this.currentViewMsg.toLocaleUpperCase();
  }

  ngOnInit() {
    this.common.topBanner(false, "", "", "");
    this.fieldVisit =
      this.common.language == "English"
        ? "FieldVisit"
        : this.arabic("fieldvisit");
    this.personal =
      this.common.language == "English" ? "Personal Report" : this.arabic("personal");
    this.citizenModal.RequestType = this.fieldVisit;
    if (this.common.language != "English") {
      //this.citizenTitle = this.arabic('personal/fieldvisitcreation');
      this.citizenModal.RequestType = this.arabic("fieldvisit");
      this.requestTypes = [this.arabic("fieldvisit"), this.arabic("personal")];
      //this.common.breadscrumChange(this.arabic('citizenaffair'), this.arabic('personal/fieldvisitcreation'), '');
    }
    this.getUserList(0);
    this.buttonControl();
    this.getLocation();
    this.getCityPersonal();
    this.getEmiratses();
    this.citizenCreateModal.ApproverDepartmentId = this.currentUser.DepartmentID;
  }

  initPage() {
    this.initPersonalReport();
    this.initFieldVisit();
  }

  ininotify(type) {
    if (type == "in") {
      this.citizenModal.InternalRequestorID = "";
      this.citizenModal.InternalRequestorDepartmentID = "";
    } else {
      this.citizenModal.ExternalRequestEmailID = "";
    }
  }

  initForwhom(type) {
    if (type == "personal") {
      this.citizenModal.FieldVisit.LocationName = "";
      this.citizenModal.FieldVisit.CityID = "";
      this.citizenModal.FieldVisit.Emirates = "";
    } else {
      this.citizenModal.FieldVisit.EmiratesID = "";
      this.citizenModal.FieldVisit.Name = "";
      this.citizenModal.FieldVisit.PhoneNumber = "";
      this.citizenModal.FieldVisit.City = "";
    }
  }

  initPersonalReport() {
    this.citizenModal.PersonalReport.Destination = "";
    this.citizenModal.PersonalReport.ProfilePhotoID = "";
    this.citizenModal.PersonalReport.ProfilePhotoName = "";
    this.citizenModal.PersonalReport.Emirates = 0;
    this.url = "assets/home/user_male.png";
    this.citizenModal.PersonalReport.EmiratesID = "";
    this.citizenModal.PersonalReport.Employer = "";
    this.citizenModal.PersonalReport.FindingNotes = "";
    this.citizenModal.PersonalReport.MaritalStatus = "";
    this.citizenModal.PersonalReport.MonthlySalary = "";
    this.citizenModal.PersonalReport.Name = "";
    this.citizenModal.PersonalReport.NoOfChildrens = "";
    this.citizenModal.PersonalReport.PhoneNumber = "";
    this.citizenModal.PersonalReport.Recommendation = "";
    this.citizenModal.PersonalReport.ReportObjectives = "";
    this.citizenModal.PersonalReport.City = "";
    this.citizenModal.PersonalReport.Age = "";
  }

  initFieldVisit() {
    this.citizenModal.FieldVisit.City = 0;
    this.citizenModal.FieldVisit.Date = "";
    this.citizenModal.FieldVisit.EmiratesID = "";
    this.citizenModal.FieldVisit.Emirates = "";
    this.citizenModal.FieldVisit.CityID = "";
    this.citizenModal.FieldVisit.FindingNotes = "";
    this.citizenModal.FieldVisit.ForWhom = "";
    this.citizenModal.FieldVisit.Location = "";
    this.citizenModal.FieldVisit.LocationID = 0;
    this.citizenModal.FieldVisit.LocationName = "";
    this.citizenModal.FieldVisit.Name = "";
    this.citizenModal.FieldVisit.PhoneNumber = "";
    this.citizenModal.FieldVisit.RequetsedBy = "";
    this.citizenModal.FieldVisit.VisitObjective = "";
  }

  async getUserList(id, type = "") {
    var type = type,
      param = [],
      user: any;

    if (id != "") {
      param = [
        {
          OrganizationID: id,
          OrganizationUnits: "string"
        }
      ];
    }
    if (type == "approver") {
      // set a = 0 for view screen
      var a = this.currentUser.id;
      if (this.screenStatus == "View") a = 0;

      this.common.getmemoUserList(param, a).subscribe((res: any) => {
        this.approver_UserList = res;
        if (id == 0) {
          this.approver_UserList = this.userList;
        }
      });
    }
    this.common
      .getUserList(param, this.currentUser.id)
      .subscribe((res: any) => {
        if (type == "person") {
          this.person_UserList = res;
        } else {
          this.userList = res;
        }
        if (id == 0) {
          this.person_UserList = this.userList;
        }
      });
  }

  getDataById(id) {
    this.service
      .getDataById("CitizenAffair", id, this.currentUser.id)
      .subscribe((res: any) => {
        this.statusList = res.M_LookupsList;
        this.ApproverDepartmentList = res.M_ApproverDepartmentList;
        this.organizationList = res.OrganizationList;
        this.citizenModal.ApproverDepartmentId = 2;
        this.approverDeptChange();
        if (id > 0) this.setData(res);
      });
  }

  approverDeptChange() {
    this.citizenModal.ApproverId = "";
    this.getUserList(this.citizenModal.ApproverDepartmentId, "approver");
  }

  personDeptChange() {
    this.citizenModal.InternalRequestorID = "";
    this.getUserList(this.citizenModal.InternalRequestorDepartmentID, "person");
  }

  clickProfile() {
    this.profile_upload.nativeElement.click();
  }

  profileLoad(e) {
    if (e.target.files && e.target.files[0]) {
      var reader = new FileReader();
      reader.onload = (e: ProgressEvent) => {
        this.url = (<FileReader>e.target).result;
      };
      reader.readAsDataURL(e.target.files[0]);
    }
    this.profileAttachment(e);
  }

  setData(data) {
    this.citizenModal.CitizenAffairID = data.CitizenAffairID;
    this.citizenModal.SourceOU = data.SourceOU;
    this.citizenModal.SourceName = data.SourceName;
    this.citizenModal.ReferenceNumber = data.ReferenceNumber;
    this.citizenModal.RequestType = this.requestTypes[data.RequestType];
    this.citizenModal.ApproverDepartmentId = data.ApproverDepartmentId;
    this.citizenModal.ApproverId = data.ApproverId;
    this.citizenModal.currentApprover = data.CurrentApproverID;
    this.citizenModal.NotifyUpon = data.NotifyUpon;
    this.citizenModal.CreatedBy = data.CreatedBy;
    this.citizenModal.CreatedDateTime = new Date(data.CreatedDateTime);
    this.citizenModal.InternalRequestorID = data.InternalRequestorID;
    this.citizenModal.InternalRequestorDepartmentID =
      data.InternalRequestorDepartmentID;
    this.citizenModal.ExternalRequestEmailID = data.ExternalRequestEmailID;
    this.citizenModal.Action = data.Action;
    this.citizenModal.Comments = data.Comments;
    this.citizenModal.Documents = data.Documents;
    this.citizenModal.Photos = data.Photos;
    this.citizenModal.Status = data.Status;
    this.citizenModal.HistoryLog = data.HistoryLog;
    this.photoAttachmentsList = data.Photos;
    this.documentAttachmentsList = data.Documents;

    if (this.citizenModal.RequestType == this.fieldVisit) {
      this.citizenModal.PersonalReport = Object.assign({});
      this.getCity(data.FieldVisit.Emirates);
      this.citizenModal.FieldVisit = data.FieldVisit;
      this.citizenModal.FieldVisit.Date = data.FieldVisit.Date
        ? new Date(data.FieldVisit.Date)
        : "";
    } else {
      this.citizenModal.FieldVisit = Object.assign({});
      this.getCity(data.PersonalReport.Emirates);
      this.citizenModal.PersonalReport = data.PersonalReport;
      if (
        this.citizenModal.PersonalReport.ProfilePhotoID &&
        this.citizenModal.PersonalReport.ProfilePhotoName
      )
        this.url =
          this.imageURL + '?filename=' + 
          this.citizenModal.PersonalReport.ProfilePhotoName +
          '&guid=' +
          this.citizenModal.PersonalReport.ProfilePhotoID;
    }
    this.buttonControl();
  }

  prepareData(data, type) {
    this.citizenCreateModal.CitizenAffairID =
      this.screenStatus == "Create" ? "" : data.CitizenAffairID;
    this.citizenCreateModal.SourceOU = this.currentUser.DepartmentName;
    this.citizenCreateModal.SourceName = this.currentUser.username;
    this.citizenCreateModal.ReferenceNumber = data.ReferenceNumber;
    this.citizenCreateModal.RequestType = data.RequestType;
    this.citizenCreateModal.ApproverDepartmentId = data.ApproverDepartmentId;
    this.citizenCreateModal.ApproverId = data.ApproverId;
    this.citizenCreateModal.NotifyUpon = data.NotifyUpon;
    if (this.screenStatus != "Create") {
      this.citizenCreateModal.UpdatedBy = this.currentUser.id;
      this.citizenCreateModal.UpdatedDateTime = new Date();
    } else {
      this.citizenCreateModal.CreatedBy = this.currentUser.id;
      this.citizenCreateModal.CreatedDateTime = new Date();
    }
    this.citizenCreateModal.InternalRequestorID = data.InternalRequestorID;
    this.citizenCreateModal.InternalRequestorDepartmentID =
      data.InternalRequestorDepartmentID;
    this.citizenCreateModal.ExternalRequestEmailID =
      data.ExternalRequestEmailID;
    this.citizenCreateModal.Action =
      type == "draft" || this.citizenModal.Staus == "58" ? "Save" : "Submit";
    if (this.citizenModal.Status == 61) {
      this.citizenCreateModal.Action = "Resubmit";
    }
    this.citizenCreateModal.Comments = "";
    this.citizenCreateModal.Documents = data.Documents;
    this.citizenCreateModal.Photos = data.Photos;

    if (this.citizenCreateModal.RequestType == this.fieldVisit) {
      this.citizenCreateModal.PersonalReport = Object.assign({});
      this.citizenCreateModal.FieldVisit = data.FieldVisit;
    } else {
      this.citizenCreateModal.FieldVisit = Object.assign({});
      this.citizenCreateModal.PersonalReport = data.PersonalReport;
    }
    this.citizenCreateModal.RequestType = this.requestTypes
      .indexOf(data.RequestType)
      .toString();
    return this.citizenCreateModal;
  }

  buttonControl() {
    if (
      this.screenStatus == "Create" ||
      (this.citizenModal.Status == 58 &&
        this.citizenModal.CreatedBy == this.currentUser.id) ||
      (this.citizenModal.Status == 61 &&
        this.citizenModal.CreatedBy == this.currentUser.id)
    ) {
      this.submitBtn = true;
    }
    if (this.screenStatus == "Create" || this.citizenModal.Status == 58) {
      this.draftBtn = true;
    }
    if (
      this.citizenModal.currentApprover == this.currentUser.id &&
      this.citizenModal.Status == 59
    ) {
      this.approveBtn = true;
    }
    if (
      this.citizenModal.currentApprover == this.currentUser.id &&
      this.citizenModal.Status == 59
    ) {
      this.rejectBtn = true;
    }
    if (
      this.citizenModal.currentApprover == this.currentUser.id &&
      this.citizenModal.Status == 59
    ) {
      this.infoBtn = true;
    }
    if (
      this.citizenModal.currentApprover == this.currentUser.id &&
      this.citizenModal.Status == 59
    ) {
      this.escalateBtn = true;
    }

    this.printBtn = true;
    // this.closeBtn = true;
  }

  validateEmail() {
    if (
      this.citizenModal.ExternalRequestEmailID &&
      this.common.validateEmail(this.citizenModal.ExternalRequestEmailID)
    ) {
      // mainFlag = false;
      this.emailFlag = false;
    } else {
      // mainFlag = true;
      this.emailFlag = true;
      this.emailErr =
        this.common.language == "English"
          ? "Please Enter Valid Email ID"
          : this.arabic("emailvalidationerror");
    }
  }

  validateForm() {
    var flag = true,
      mainFlag = true,
      internalrequest =
        this.common.language == "English"
          ? "internalrequestor"
          : this.arabic("internalrequestor"),
      photo = this.validateAttachments(this.citizenModal.Photos),
      document = this.validateAttachments(this.citizenModal.Documents);
    if (
      !this.btnLoad &&
      photo &&
      document &&
      this.citizenModal.ApproverDepartmentId &&
      this.citizenModal.ApproverId &&
      this.citizenModal.NotifyUpon
    ) {
      if (this.citizenModal.NotifyUpon == 1) {
        if (
          this.citizenModal.InternalRequestorDepartmentID &&
          this.citizenModal.InternalRequestorID
        )
          mainFlag = false;
        else mainFlag = true;
      } else {
        if (
          this.citizenModal.ExternalRequestEmailID &&
          this.common.validateEmail(this.citizenModal.ExternalRequestEmailID)
        ) {
          mainFlag = false;
          this.emailFlag = false;
        } else {
          mainFlag = true;
          this.emailFlag = true;
          this.emailErr =
            this.common.language == "English"
              ? "Please Enter Valid Email ID"
              : this.arabic("emailvalidationerror");
        }
      }
    }
    if (this.citizenModal.RequestType == this.fieldVisit) {
      if (
        !mainFlag &&
        this.citizenModal.FieldVisit.Date &&
        this.citizenModal.FieldVisit.RequetsedBy.trim() &&
        this.citizenModal.FieldVisit.FindingNotes.trim() &&
        this.citizenModal.FieldVisit.VisitObjective.trim() &&
        this.citizenModal.FieldVisit.ForWhom
      ) {
        flag = this.validateForWhom(flag);
      }
    } else {
      if (
        !mainFlag &&
        this.citizenModal.PersonalReport.Name.trim() &&
        this.citizenModal.PersonalReport.Employer &&
        this.citizenModal.PersonalReport.Destination.trim() &&
        this.citizenModal.PersonalReport.Emirates &&
        this.citizenModal.PersonalReport.City &&
        this.citizenModal.PersonalReport.ReportObjectives.trim() &&
        this.citizenModal.PersonalReport.FindingNotes.trim() &&
        this.citizenModal.PersonalReport.Recommendation.trim()
      ) {
        flag = false;
      }
    }
    return flag;
  }

  validateAttachments(data) {
    if (!data || data.length == 0) {
      return false;
    }
    if (
      data[0].AttachmentGuid != "" &&
      data[0].AttachmentsName != "" &&
      data.length > 0
    ) {
      return true;
    }
  }

  validateForWhom(flag) {
    if (this.citizenModal.FieldVisit.ForWhom == "forPersonal") {
      if (
        this.citizenModal.FieldVisit.EmiratesID.trim() &&
        this.citizenModal.FieldVisit.Name.trim() &&
        this.citizenModal.FieldVisit.PhoneNumber &&
        this.citizenModal.FieldVisit.City
      ) {
        flag = false;
      } else {
        flag = true;
      }
    } else {
      if (
        this.citizenModal.FieldVisit.Emirates &&
        this.citizenModal.FieldVisit.LocationName.trim() &&
        this.citizenModal.FieldVisit.CityID
      )
        flag = false;
      else flag = true;
    }
    return flag;
  }

  async saveForm(type) {
    var param = await this.prepareData(this.citizenModal, type);
    var draftmsg =
      this.common.language == "English"
        ? "Citizen Affair Drafted Successfully"
        : this.arabic("citizendraftsuccess");
    var submitmsg =
      this.common.language == "English"
        ? "Citizen Affair Submitted Successfully"
        : this.arabic("citizensubmitsuccess");
    var resubmit =
      this.common.language == "English"
        ? "Citizen Affair Resubmitted Successfully"
        : this.arabic("citizenresubmittedsuccess");

    var msg = this.citizenCreateModal.Action == "Save" ? draftmsg : submitmsg;
    if (this.screenStatus != "Create") {
      this.service
        .saveFormPut("CitizenAffair", param, this.currentUser.id)
        .subscribe(res => {
          this.btnLoadSubmit = false;
          this.btnLoad = false;
          this.bsModalRef = this.modalService.show(
            SuccessComponent,
            this.config
          );
          if (this.common.language != "English")
            this.bsModalRef.content.language = "ar";
          msg = this.citizenModal.Status == 61 ? resubmit : msg;
          this.bsModalRef.content.message = msg;
          this.bsModalRef.content.pagename = "Citizen Affair";
        });
    } else {
      this.service
        .saveFormPost("CitizenAffair", param, this.currentUser.id)
        .subscribe(res => {
          this.btnLoadDraft = false;
          this.btnLoad = false;
          this.bsModalRef = this.modalService.show(
            SuccessComponent,
            this.config
          );
          if (this.common.language != "English")
            this.bsModalRef.content.language = "ar";
          this.bsModalRef.content.message = msg;
          this.bsModalRef.content.pagename = "Citizen Affair";
        });
    }
  }

  popup(status: any) {
    var button = "";
    this.bsModalRef = this.modalService.show(ModalComponent, this.config);
    if (status == "Citizen Affair Escalate") button = "Escalate";
    else button = "Redirect";
    if (this.common.language != "English")
      this.bsModalRef.content.language = "ar";
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.button = button;
    this.bsModalRef.content.screenStatus = this.screenStatus;
    this.bsModalRef.content.fromScreen = "Citizen Affair";
    this.bsModalRef.content.Comments = this.citizenModal.Comments;
    this.bsModalRef.content.memoid = this.citizenModal.CitizenAffairID;
    this.bsModalRef.content.onClose.subscribe(result => {
      this.btnLoad = result;
    });
  }

  patchCall(type) {
    var param = this.common.formatPatchData(
      type,
      "action",
      this.citizenModal.Comments
    );
    this.common
      .patch("CitizenAffair", param, this.citizenModal.CitizenAffairID)
      .subscribe(res => {
        var approveMsg =
          this.common.language == "English"
            ? "Citizen Affair Approved Successfully"
            : this.arabic("citizenapprved"),
          rejectMsg =
            this.common.language == "English"
              ? "Citizen Affair Rejected Successfully"
              : this.arabic("citizenrejected"),
          closedMsg =
            this.common.language == "English"
              ? "Citizen Affair Closed Successfully"
              : this.arabic("citizenclosed"),
          retunrMsg =
            this.common.language == "English"
              ? "Citizen Affair Returned For Info Successfully"
              : this.arabic("citizenreturn"),
          msg = "";
        switch (type) {
          case "Approve":
            this.btnLoadApprove = false;
            msg = approveMsg;
            break;
          case "Reject":
            this.btnLoadReject = false;
            msg = rejectMsg;
            break;
          case "Close":
            this.btnLoadClose = false;
            msg = closedMsg;
            break;
          case "ReturnForInfo":
            this.btnLoadClose = false;
            msg = retunrMsg;
            break;
        }
        this.btnLoad = false;
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        if (this.common.language != "English")
          this.bsModalRef.content.language = "ar";
        this.bsModalRef.content.message = msg;
        this.bsModalRef.content.pagename = "Citizen Affair";
      });
  }

  profileAttachment(event) {
    //this.documentAttachmentsList = [];
    var files = event.target.files;
    if (files.length > 0) {
      let that = this;
      this.uploadProfileProcess = true;
      this.common.postAttachment(files).subscribe((event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadProfilePercentage =
            Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.uploadProfileProcess = false;
          this.uploadProfilePercentage = 0;
          this.citizenModal.PersonalReport.ProfilePhotoID = event.body.Guid;
          this.citizenModal.PersonalReport.ProfilePhotoName =
            event.body.FileName[0];
        }
      });
    }
  }

  documentAttachments(event) {
    //this.documentAttachmentsList = [];
    var files = event.target.files;
    if (files.length > 0) {
      let that = this;
      this.uploadDocumentProcess = true;
      this.common.postAttachment(files).subscribe((event: any) => {
        this.documentFile.nativeElement.value = "";
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadDocumentPercentage =
            Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.uploadDocumentProcess = false;
          this.uploadDocumentPercentage = 0;
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.documentAttachmentsList.push({
              AttachmentGuid: event.body.Guid,
              AttachmentsName: event.body.FileName[i],
              MemoID: ""
            });
            this.citizenModal.Documents = this.documentAttachmentsList;
          }
        }
      });
    }
  }

  photoAttachments(event) {
    //this.photoAttachmentsList = [];
    var files = event.target.files;
    if (files.length > 0) {
      let that = this;
      this.uploadPhotoProcess = true;
      this.common.postAttachment(files).subscribe((event: any) => {
        this.photoFile.nativeElement.value = "";
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadPhotoPercentage =
            Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.uploadPhotoProcess = false;
          this.uploadPhotoPercentage = 0;
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.photoAttachmentsList.push({
              AttachmentGuid: event.body.Guid,
              AttachmentsName: event.body.FileName[i],
              MemoID: ""
            });
            this.citizenModal.Photos = this.photoAttachmentsList;
          }
        }
      });
    }
  }

  deleteDocumentAttachment(index) {
    this.citizenModal.Documents;
    this.documentAttachmentsList.splice(index, 1);
    this.documentFile.nativeElement.value = "";
  }

  deletePhotoAttachment(index) {
    this.citizenModal.Documents;
    this.photoAttachmentsList.splice(index, 1);
    this.photoFile.nativeElement.value = "";
  }

  getLocation() {
    this.service.getLocationList(this.currentUser.id).subscribe(res => {
      this.locationList = res;
    });
  }

  print(template: TemplateRef<any>) {
    this.service.printPreview("CitizenAffair/preview",this.citizenModal.CitizenAffairID,this.currentUser.id).subscribe(res => {
        if (res) {
          this.service.pdfToJson(this.citizenModal.ReferenceNumber).subscribe((data: any) => {
              this.showPdf = true;
              this.pdfSrc = data;
              this.bsModalRef = this.modalService.show(template, {class: "modal-xl"});
              this.common.deleteGeneratedFiles('files/delete', this.citizenModal.ReferenceNumber + '.pdf').subscribe(result => {
                console.log(result);
              });
            });
        }
      });
  }

  getCity(emirates) {
    this.citizenModal.FieldVisit.CityID = '';
    this.citizenModal.PersonalReport.City = '';
    if(emirates == null){
      this.cityListFiled = [];
    }else{
      this.service.getCityListbyID(this.currentUser.id, emirates).subscribe(res => {
          this.cityListFiled = res;
      });
    }
  }

  getCityPersonal() {
    this.service
      .getCityList(this.currentUser.id)
      .subscribe(res => {
        this.cityList = res;
      });
  }

  getEmiratses() {
    this.service.getEmiratesList(this.currentUser.id).subscribe(res => {
      this.emiratesList = res;
    });
  }

  // delete() {
  //   var deleteMsg = (this.common.language == 'English') ? 'Citizen Affair Deleted Successfully' : this.arabic('citizenaffairdelete');
  //   this.service.deleteList('CitizenAffair', this.citizenModal.CitizenAffairID, this.currentUser.id).subscribe(res => {
  //     this.btnLoadDelete = false;
  //     this.btnLoad = false;
  //     this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //     if (this.common.language != 'English')
  //       this.bsModalRef.content.language = 'ar';
  //     this.bsModalRef.content.message = deleteMsg;
  //     this.bsModalRef.content.pagename = 'Citizen Affair';
  //   });
  // }

  closePrintPop() {
    this.btnLoad = false;
    this.bsModalRef.hide();
  }

  printPdf(html: ElementRef<any>) {
    this.btnLoad = false;
    this.service.printPreview("CitizenAffair/preview",this.citizenModal.CitizenAffairID,this.currentUser.id).subscribe(res => {
      if (res) {
        this.common.printPdf(this.citizenModal.ReferenceNumber);
      }
    });
  }

  downloadPrint() {
    this.service.printPreview("CitizenAffair/preview",this.citizenModal.CitizenAffairID,this.currentUser.id).subscribe(res => {
      if (res) {
        this.common.previewPdf(this.citizenModal.ReferenceNumber).subscribe((data: Blob) => {
          var url = window.URL.createObjectURL(data);
          var a = document.createElement("a");
          document.body.appendChild(a);
          a.setAttribute("style", "display: none");
          a.href = url;
          a.download = this.citizenModal.ReferenceNumber + ".pdf";
          a.click();
          window.URL.revokeObjectURL(url);
          a.remove();
          this.common.deleteGeneratedFiles('files/delete', this.citizenModal.ReferenceNumber + '.pdf').subscribe(result => {
            console.log(result);
          });
        });
      }
    });
  }

  // hisLog(status) {
  //   return this.common.historyLog(status);
  // }

  hisLog(status) {
    if (status == "Reject") {
      return this.common.language == "English"
        ? "Rejected"
        : this.common.arabic.words["letterrejectdby"];
    } else if (status == "Redirect") {
      return this.common.language == "English"
        ? "Redirected"
        : this.common.arabic.words["redirectedby"];
    } else if (status == "Submit") {
      return this.common.language == "English"
        ? "Submitted"
        : this.common.arabic.words["submittedby"];
    } else if (status == "Resubmit") {
      return this.common.language == "English"
        ? "Resubmitted"
        : this.common.arabic.words["resubmittedby"];
    } else if (status == "ReturnForInfo") {
      return this.common.language == "English"
        ? "ReturnForInfo"
        : this.common.arabic.words["returnforinfoby"];
    } else if (status == "Escalate") {
      return this.common.language == "English"
        ? "Escalated"
        : this.common.arabic.words["escalatedby"];
    } else if (status == "Approve") {
      return this.common.language == "English"
        ? "Approved"
        : this.common.arabic.words["approvedby"];
    } else {
      console.log("log-beer" + (status + "dby").toLocaleLowerCase());
      return this.common.language == "English"
        ? status + "d"
        : this.common.arabic.words[(status + "dby").toLocaleLowerCase()];
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
