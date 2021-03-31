import {
  Component,
  OnInit,
  ViewChild,
  TemplateRef,
  ElementRef,
  ChangeDetectorRef
} from "@angular/core";
import { BsDatepickerConfig } from "ngx-bootstrap";
import { FormGroup, FormControl } from "@angular/forms";
import { CreateLetterModal } from "./incoming-letter-form.modal";
import { IncomingLetterService } from "./incoming-letter-form.service";
import { Router, ActivatedRoute } from "@angular/router";
import "tinymce";
import { async } from "q";
import { DatePipe } from "@angular/common";
import { BsModalService, BsModalRef } from "ngx-bootstrap/modal";
import { ModalComponent } from "../../../../modal/modalcomponent/modal.component";
import { SuccessComponent } from "../../../../modal/success-popup/success.component";
import { CommonService } from "../../../../common.service";
import { HttpEventType } from "@angular/common/http";
import { ArabicDataService } from "src/app/arabic-data.service";
import { LinkToModalComponent } from "src/app/letter/link-to-modal/link-to-modal.component";
import { environment } from "src/environments/environment";
import { UtilsService } from "src/app/shared/service/utils.service";

declare var tinymce: any;

@Component({
  selector: "app-incoming-letter-form",
  templateUrl: "./incoming-letter-form.component.html",
  styleUrls: ["./incoming-letter-form.component.scss"]
})
export class IncomingLetterFormComponent implements OnInit {
  environment = environment;
  @ViewChild("template") template: TemplateRef<any>;
  bsModalRef: BsModalRef;
  @ViewChild("variable") myInputVariable: ElementRef;
  createLetter: CreateLetterModal = new CreateLetterModal();
  screenStatus = "Create";
  letterData: any;
  OrgUnitID = "0";
  outletter = [];
  inletter = [];
  link_list = [];
  incoming_link_list = [];
  OfficialEntity: any;
  Ismemolink: boolean = false;
  Isletterlink: boolean = false;
  status = [];
  user = [];
  department = [];
  departments = [];
  organisationEntity = [];
  dropdownOptions = [
    "one",
    "two",
    "three",
    "4",
    "5",
    "6",
    "7",
    "8",
    "9",
    "10",
    "11",
    "12",
    "13",
    "14",
    "15"
  ];
  priorityList = this.common.formPriorityList;
  documentList = this.common.documentList;
  attachmentFiles = [];
  dest_departSettings = {
    singleSelection: false,
    idField: "OrganizationID",
    textField: "OrganizationUnits",
    selectAllText: "Select All",
    unSelectAllText: "UnSelect All",
    itemsShowLimit: 3,
    allowSearchFilter: false
  };
  dest_userSettings = {
    singleSelection: false,
    idField: "UserID",
    textField: "EmployeeName",
    selectAllText: "Select All",
    unSelectAllText: "UnSelect All",
    itemsShowLimit: 3,
    allowSearchFilter: false
  };
  createdTime: string;
  createdDate: string;

  toolbar = [
    ["bold", "italic", "subscript"],
    ["fontName", "fontSize", "color"],
    ["justifyLeft", "justifyCenter", "justifyRight", "justifyFull"],
    ["undo", "redo"],
    ["indent", "outdent"],
    ["paragraph", "orderedList", "unorderedList"]
  ];
  sendDraftBtnLoad = false;
  sendBtnLoad = false;
  closeBtnLoad = false;
  shareBtnLoad = false;
  printBtnLoad = false;
  cloneBtnLoad = false;
  downloadBtnLoad = false;
  approveBtnLoad = false;
  deleteBtnLoad = false;
  returnInfoBtnLoad = false;
  rejectBtnLoad = false;
  btnLoad = false;

  dropdownSettings: any;
  letterModel: any = {
    LetterID: 0,
    LetterReferenceNumber: "",
    Status: "",
    CreatedDateTime: new Date(),
    SourceOU: "",
    SourceName: "",
    Title: "",
    DestinationOU: [],
    DestinationUsername: [],
    DestinationUserId: [],
    DestinationDepartmentId: [],
    ReceivingDate: NaN,
    ReceivedFromGovernmentEntity: "",
    ReceivedFromName: "",
    ApproverDepartmentId: 0,
    ApproverId: 0,
    RelatedToOutgoingLetter: [],
    RelatedToIncomingLetter: [],
    LetterPhysicallySend: "",
    OfficialEntity: 1,
    CreatedBy: 0,
    Action: "",
    Comments: "",
    LetterType: "Incoming",
    LetterDetails: "",
    RelatedOutgoingLetters: [],
    Notes: "",
    DocumentClassification: "",
    Priority: "",
    Attachments: [],
    Keywords: [],
    NeedReply: "",
    HistoryLog: [],
    ApproverName: 0,
    UpdatedBy: 0,
    UpdatedDateTime: new Date(),
    IsRedirect: false
  };

  bsConfig: Partial<BsDatepickerConfig>;
  bsConfigs: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  colorTheme = "theme-green";
  img_file: any;
  message: any;
  currentUser: any = JSON.parse(localStorage.getItem("User"));
  attachments: any = [];
  userDestination: any;
  userReceiver: any;
  commonMes: any;
  DestinationDepartmentId: any[];
  RelatedOutgoingLettersId: any[];
  RelatedIncomingLettersId: any[];
  relatedOutgoingLetterList: any[];
  relatedIncomingLetterList: any[];
  uploadProcess: boolean = false;
  uploadPercentage: number = 0;
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  organisationEntityNames: any;
  approverDepartment: any;
  destinationDepartment: any;
  Destination_user_name: any = [];
  sentMsg: string | typeof ArabicDataService;
  draftMsg: any;
  cloneMsg: any;
  approvedMsg: any;
  rejectedMsg: any;
  closedMsg: any;
  deletedMsg: any;

  tinyConfig = {
    plugins:
      "powerpaste casechange importcss tinydrive searchreplace directionality visualblocks visualchars fullscreen table charmap hr pagebreak nonbreaking toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons",
    language: this.common.language != "English" ? "ar" : "en",
    menubar: "file edit view insert format tools table tc help",
    toolbar:
      "undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media pageembed template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment",
    toolbar_drawer: "sliding",
    directionality: this.common.language != "English" ? "rtl" : "ltr"
  };

  // breadvrum label
  internalletter =
    this.common.language == "English"
      ? "Internal Letters"
      : this.arabic("internalletters");
  incomingletter =
    this.common.language == "English"
      ? "Incoming Letters"
      : this.arabic("incomingletters");
  create = this.common.language == "English" ? "Create" : this.arabic("create");
  letter =
    this.common.language == "English" ? "Letter" : this.arabic("letters");
  destinationBtn: boolean = false;
  canEditContact: any;
  screenTitle: any;
  selectedUsersDepartment: any;
  currentLetterID: number;
  incomingCheck: boolean = false;
  approverBtnDis: boolean = false;
  distinationBtnDis: boolean = false;
  updatedBy: any;
  contactTriggerEvent: any;
  lastContactEntity: any;
  lastContactIndex: any = 0;

  constructor(
    private changeDetector: ChangeDetectorRef,
    private incomingletterservice: IncomingLetterService,
    public common: CommonService,
    public router: Router,
    route: ActivatedRoute,
    public util: UtilsService,
    public datepipe: DatePipe,
    private modalService: BsModalService
  ) {
    this.canEditContact = this.currentUser.CanEditContact;
    this.cloneBtnLoad = false;
    this.btnLoad = false;
    route.url.subscribe(() => {
      console.log(route.snapshot.data);
      this.screenStatus = route.snapshot.data.title;
    });
    route.params.subscribe(param => {
      var id = +param.id;
      this.currentLetterID = id;
      if (id > 0) this.loadData(id, this.currentUser.id);
    });
    this.incomingletterservice.getMemo("memo", 0, 0).subscribe((data: any) => {
      this.approverDepartment = data.M_ApproverDepartmentList;
      var calendar_id = environment.calendar_id;
      this.destinationDepartment = data.OrganizationList.filter(
        res => calendar_id != res.OrganizationID
      );
    });
    this.incomingletterservice
      .getCombo("InboundLetter", 0, this.currentUser.id)
      .subscribe((data: any) => {
        if (this.screenStatus != "Create")
          this.organisationEntity = data.OrganisationEntity;
      });
    if (this.screenStatus == "Create") {
      this.relatedLetter();
      this.common.breadscrumChange(
        this.incomingletter,
        this.create,
        ''
      );
    }
  }

  async loadData(id, userid) {
    this.common.breadscrumChange(
      this.incomingletter,
      this.letter + " " + id,
      ''
    );
    await this.incomingletterservice
      .getCombo("InboundLetter", id, userid)
      .subscribe((data: any) => {
        this.letterData = data;
        if (this.screenStatus == "View" || this.screenStatus == "Edit") {
          let that = this;
          this.status = this.letterData.M_LookupsList;
          var date = this.letterModel.CreatedDateTime;
          this.letterModel.CreatedDateTime = new Date(date);
          this.setData(this.letterData);
          this.bottonControll();
        } else {
          this.initPage();
          this.bottonControll();
        }
      });
  }
  setData(data) {
    this.getDestUserList(+data.ApproverDepartmentId);
    this.getRecvPrepareUserList(data.DestinationDepartmentId);
    this.letterModel.LetterID = data.LetterID;
    this.updatedBy = data.UpdatedBy;
    this.letterModel.LetterReferenceNumber = data.LetterReferenceNumber;
    this.letterModel.Title = data.Title;
    this.letterModel.OfficialEntity = data.IsGovernmentEntity ? "1" : "2";
    this.onentitychange(this.letterModel.OfficialEntity);
    this.onEntityNamechange(
      this.letterModel.OfficialEntity,
      data.ReceivedFromGovernmentEntity
    );
    this.getSouceName(data.SourceName, data.SourceOU);
    this.letterModel.ApproverDepartmentId = +data.ApproverDepartmentId;
    this.letterModel.ReceivingDate = data.ReceivingDate
      ? new Date(data.ReceivingDate)
      : "";
    this.letterModel.ApproverId = data.ApproverId;
    this.letterModel.LetterDetails = data.LetterDetails;
    this.letterModel.OfficialEntity = data.IsGovernmentEntity ? "1" : "2";
    this.letterModel.ReceivedFromName = data.ReceivedFromName;
    this.letterModel.ReceivedFromGovernmentEntity = data.ReceivedFromEntityID;
    if (data.NeedReply == true || data.NeedReply == false) {
      this.letterModel.NeedReply = "" + data.NeedReply;
    }
    if (
      data.LetterPhysicallySend == true ||
      data.LetterPhysicallySend == false
    ) {
      this.letterModel.LetterPhysicallySend = "" + data.LetterPhysicallySend;
    }
    this.letterModel.HistoryLog = data.HistoryLog;
    const DestinationOU = [];
    const DestinationUsername = [];
    data.DestinationDepartmentId.forEach((department, index) => {
      DestinationOU.push(department.LetterDestinationDepartmentID);
      this.letterModel.DestinationOU = department.LetterDestinationDepartmentID;
    });
    this.letterModel.IsRedirect = data.IsRedirect;
    //this.letterModel.DestinationOU = DestinationOU;
    this.DestinationDepartmentId = DestinationOU;
    let RelatedOutgoingLetters: any = [];
    let RelatedincomingLetters: any = [];
    if (data.RelatedOutgoingLetters.length > 0) {
      data.RelatedOutgoingLetters.forEach(key => {
        this.letterModel.RelatedToOutgoingLetter.push({
          display: key.OutgoingLetterReferenceNo,
          value: key.OutgoingLetterReferenceNo
        });
        // this.onAddChangeout(RelatedOutgoingLetters);
      });
    } else {
      this.letterModel.RelatedToOutgoingLetter = [];
    }
    if (data.RelatedIncomingLetters.length > 0) {
      data.RelatedIncomingLetters.forEach(key => {
        this.letterModel.RelatedToIncomingLetter.push({
          display: key.OutgoingLetterReferenceNo,
          value: key.OutgoingLetterReferenceNo
        });
        // this.onAddChangein(RelatedingoingLetters);
      });
      //this.inletter.push({ display: data.RelatedToIncomingLetter, value: data.RelatedToIncomingLetter });
    }
    // this.letter.RelatedToIncomingLetter = RelatedincomingLetters;
    // this.letter.RelatedToOutgoingLetter = RelatedOutgoingLetters;
    this.link_list = data.RelatedOutgoingLetters;
    this.incoming_link_list = data.RelatedIncomingLetters;
    data.DestinationUserId.forEach((user, index) => {
      DestinationUsername.push(user.LetterDestinationUsersID);
      this.Destination_user_name.push(user.LetterDestinationUsersName);
      this.letterModel.DestinationUserId = user.LetterDestinationUsersID;
    });
    if (data.CurrentApprover.length > 0) {
      this.letterModel.ApproverName = data.CurrentApprover[0].ApproverId;
    }
    this.letterModel.DocumentClassification = this.documentList[
      data.DocumentClassification
    ];
    this.letterModel.Notes = data.Notes;
    this.letterModel.Priority = this.priorityList[Number(data.Priority)];
    this.letterModel.CreatedBy = data.CreatedBy;
    this.attachments = data.Attachments;
    data.Keywords.forEach(key => {
      this.letterModel.Keywords.push({
        display: key.Keywords,
        value: key.Keywords
      });
    });
    //this.getDestUserList(+data.ApproverDepartmentId);
    this.letterModel.Attachments = data.Attachments;
    this.letterModel.Status = data.Status;
    this.relatedLetter();
    console.log(this.letterModel.DestinationOU);
    this.letterModel.ApproverId = data.ApproverId;
    this.incomingCheck = (this.letterModel.ApproverName == this.letterModel.DestinationUserId) ? true : false;
  }

  public relatedLetter() {
    let letter_user_id = this.currentUser.id;
    if (this.screenStatus != "Create") {
      letter_user_id = this.letterModel.CreatedBy;
    }
    this.incomingletterservice
      .getReleatedOutgoingLetterCombo(
        "InboundLetter/RelatedOutgoingLetters",
        letter_user_id
      )
      .subscribe((data: any) => {
        this.relatedOutgoingLetterList = data;
      });
    this.incomingletterservice
      .getReleatedIncomingLetterCombo(
        "InboundLetter/RelatedIncomingLetters",
        letter_user_id
      )
      .subscribe((data: any) => {
        this.relatedIncomingLetterList = data;
      });
  }

  async ngOnInit() {
    this.contactTriggerEvent = this.common.contactChanged$.subscribe(res => {
      if (this.lastContactEntity)
        this.onentitychange(this.lastContactEntity);
    });
    switch (this.screenStatus) {
      case "Create":
        this.screenTitle = (this.common.language == "English")
          ? "CREATE"
          : this.arabic("create");
        break;
      case "View":
        this.screenTitle = (this.common.language == "English")
          ? "VIEW"
          : this.arabic("view");
        break;
      case "Edit":
        this.screenTitle = (this.common.language == "English")
          ? "EDIT"
          : this.arabic("edit");
        break;
    }

    this.priorityList =
      this.common.language == "English"
        ? ["High", "Medium", "Low", "Verylow"]
        : [
          this.arabic("high"),
          this.arabic("medium"),
          this.arabic("low"),
          this.arabic("verylow")
        ];
    this.common.triggerScrollTo("trigger-letter-in");
    this.sentMsg =
      this.common.language == "English"
        ? "Letter Sent Successfully."
        : this.arabic("lettersentsuccess");
    this.draftMsg =
      this.common.language == "English"
        ? "Letter Drafted Successfully."
        : this.arabic("letterdraftsaved");
    this.cloneMsg =
      this.common.language == "English"
        ? "Letter Cloned Successfully."
        : this.arabic("letterclonedsuccessfully");
    this.approvedMsg =
      this.common.language == "English"
        ? "Letter Approved Successfully."
        : this.arabic("letterapprovedsuccessfully");
    this.rejectedMsg =
      this.common.language == "English"
        ? "Letter Rejected Successfully."
        : this.arabic("letterrejectedsuccessfully");
    this.closedMsg =
      this.common.language == "English"
        ? "Letter Closed Successfully."
        : this.arabic("letterclosedsuccessfully");
    this.deletedMsg =
      this.common.language == "English"
        ? "Letter Deleted Successfully."
        : this.arabic("letterdeletedsuccessfully");
    this.bottonControll();
    this.letterModel.ApproverDepartmentId = this.currentUser.DepartmentID;
    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
    this.ApproverDestination("");
  }

  public tinyMceSettings = {
    skin_url: "assets/tinymce/skins/lightgray",
    inline: false,
    statusbar: false,
    browser_spellcheck: true,
    height: 320,
    plugins: "fullscreen"
  };

  ngAfterViewInit() { }

  closemodal() {
    this.modalService.hide(1);
    setTimeout(function () {
      location.reload();
    }, 1000);
  }

  initPage() {
    console.log("init page incoming");

    this.letterModel.LetterID = 0;
    this.letterModel.LetterReferenceNumber = "";
    this.letterModel.Status = "";
    this.letterModel.CreatedDateTime = new Date();
    this.letterModel.SourceOU = this.currentUser.department;
    this.letterModel.SourceName = this.currentUser.username;
    this.letterModel.Title = "";
    this.letterModel.DestinationOU = [];
    this.letterModel.DestinationUserId = [];
    this.letterModel.DestinationUserId = [];
    this.letterModel.DestinationDepartmentId = [];
    this.letterModel.ReceivingDate = NaN;
    this.letterModel.ReceivedFromGovernmentEntity = "";
    this.letterModel.ReceivedFromName = "";
    this.letterModel.ApproverDepartmentId = this.currentUser.DepartmentID;
    this.letterModel.ApproverId = 0;
    this.letterModel.RelatedToOutgoingLetter = [];
    this.letterModel.RelatedToIncomingLetter = [];
    this.letterModel.LetterPhysicallySend = "";
    this.letterModel.CreatedBy = this.currentUser.id;
    this.letterModel.UpdatedBy = 0;
    this.letterModel.Action = "";
    this.letterModel.Comments = "";
    this.letterModel.LetterType = "Incoming";
    this.letterModel.LetterDetails = "";
    this.letterModel.RelatedOutgoingLetters = [];
    this.letterModel.Notes = "";
    this.letterModel.DocumentClassification = "";
    this.letterModel.Priority = "";
    this.letterModel.Keywords = [];
    this.letterModel.Attachments = [];
    this.letterModel.NeedReply = "";
    this.letterModel.HistoryLog = [];
    this.letterModel.UpdatedDateTime = new Date();
  }
  Attachments(event) {
    var files = event.target.files;
    let that = this;
    this.uploadProcess = true;
    this.common.postAttachment(files).subscribe((event: any) => {
      if (event.type === HttpEventType.UploadProgress) {
        this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
      } else if (event.type === HttpEventType.Response) {
        this.uploadProcess = false;
        this.uploadPercentage = 0;
        for (var i = 0; i < event.body.FileName.length; i++) {
          this.attachments.push({
            AttachmentGuid: event.body.Guid,
            AttachmentsName: event.body.FileName[i],
            LetterID: ""
          });
        }
        this.letterModel.Attachments = this.attachments;
      }
    });
    this.myInputVariable.nativeElement.value = "";
  }

  selectChange(data) {
    console.log(this.letterModel.DestinationOU);
    this.letterModel.DestinationOU;
  }

  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.myInputVariable.nativeElement.value = "";
  }

  onAddChangeout(event) {
    if (event != "") {
      let type = "1";
      this.incomingletterservice
        .getLetters(
          "InboundLetter/RelatedInOutLetterswithRef",
          this.currentUser.id,
          type,
          event.value
        )
        .subscribe((data: any) => {
          if (
            data[0].ReferenceNo &&
            this.checkLinkList(this.outletter, event)
          ) {
            //if (this.screenStatus != 'View')
            this.outletter.push(event);
            this.letterModel.RelatedToOutgoingLetter = this.outletter;
            this.RelatedOutgoingLettersId = this.outletter;
            this.link_list.push(data[0]);
          } else {
            this.Ismemolink = true;
            setTimeout(() => {
              this.Ismemolink = false;
            }, 7000);
            this.letterModel.RelatedToOutgoingLetter = this.outletter;
            this.RelatedOutgoingLettersId = this.outletter;
          }
        });
    }
  }
  onRemoveChangeout(event) {
    for (var i = 0; i < this.outletter.length; i++) {
      if (this.outletter[i] == event) {
        this.outletter.splice(i, 1);
      }
    }
    for (var i = 0; i < this.link_list.length; i++) {
      if (
        this.link_list[i].ReferenceNo.toLowerCase() == event.value.toLowerCase()
      ) {
        this.link_list.splice(i, 1);
      }
    }
  }

  onAddChangein(event) {
    if (event != "") {
      let type = "0";
      this.incomingletterservice
        .getLetters(
          "InboundLetter/RelatedInOutLetterswithRef",
          this.currentUser.id,
          type,
          event.value
        )
        .subscribe((data: any) => {
          if (data[0].ReferenceNo && this.checkLinkList(this.inletter, event)) {
            // if (this.screenStatus != 'View')
            this.inletter.push(event);
            this.letterModel.RelatedToIncomingLetter = this.inletter;
            this.RelatedIncomingLettersId = this.inletter;
            this.incoming_link_list.push(data[0]);
          } else {
            this.Isletterlink = true;
            setTimeout(() => {
              this.Isletterlink = false;
            }, 7000);
            this.letterModel.RelatedToIncomingLetter = this.inletter;
            this.RelatedIncomingLettersId = this.inletter;
          }
        });
    }
  }
  onRemoveChangein(event) {
    for (var i = 0; i < this.inletter.length; i++) {
      if (this.inletter[i] == event) {
        this.inletter.splice(i, 1);
      }
    }
    for (var i = 0; i < this.incoming_link_list.length; i++) {
      if (
        this.incoming_link_list[i].ReferenceNo.toLowerCase() ==
        event.value.toLowerCase()
      ) {
        this.incoming_link_list.splice(i, 1);
      }
    }
  }

  prepareData() {
    this.userName("");
    this.createLetter.LetterID = this.letterModel.LetterID;
    this.createLetter.LetterReferenceNumber = this.letterModel.LetterReferenceNumber;
    this.createLetter.UpdatedBy = this.currentUser.id;
    this.createLetter.UpdatedDateTime = this.letterModel.CreatedDateTime;
    this.createLetter.Title = this.letterModel.Title;
    this.createLetter.SourceOU = this.currentUser.DepartmentID;
    this.createLetter.SourceName = this.currentUser.UserID;
    this.createLetter.ApproverDepartmentId = this.letterModel.ApproverDepartmentId;
    this.createLetter.ApproverId = this.letterModel.ApproverId;
    if (this.letterModel.ReceivingDate) {
      this.createLetter.ReceivingDate = this.letterModel.ReceivingDate;
    } else {
      this.createLetter.ReceivingDate = "";
    }
    this.createLetter.ReceivedFromGovernmentEntity = this.letterModel.ReceivedFromGovernmentEntity;
    this.createLetter.ReceivedFromEntityID = this.letterModel.ReceivedFromGovernmentEntity;
    this.createLetter.IsGovernmentEntity =
      this.letterModel.OfficialEntity == "1" ? true : false;
    this.createLetter.ReceivedFromName = this.letterModel.ReceivedFromName;
    this.createLetter.LetterDetails = this.letterModel.LetterDetails;
    this.createLetter.Notes = this.letterModel.Notes;
    this.createLetter.DocumentClassification = this.documentList
      .indexOf(this.letterModel.DocumentClassification)
      .toString();
    this.createLetter.Priority = this.priorityList
      .indexOf(this.letterModel.Priority)
      .toString();
    this.createLetter.NeedReply = this.letterModel.NeedReply;
    this.createLetter.LetterPhysicallySend = this.letterModel.LetterPhysicallySend;
    if (this.screenStatus != "Edit") {
      this.createLetter.CreatedBy = this.currentUser.id;
      this.createLetter.CreatedDateTime = this.letterModel.CreatedDateTime;
    } else {
      this.createLetter.UpdatedBy = this.currentUser.id;
      this.createLetter.UpdatedDateTime = this.letterModel.CreatedDateTime;
    }
    this.createLetter.Action = this.letterModel.Action;
    this.createLetter.Comments = this.letterModel.Comments;
    this.createLetter.LetterType = this.letterModel.LetterType;

    // if (this.letterModel.DestinationUserId) {
    //   this.letterModel.DestinationUserId.forEach(data => {
    //     this.createLetter.DestinationUserId.push({
    //       "LetterDestinationUsersID": data,
    //       'LetterDestinationUsersName': ''
    //     });
    //   });
    // }


    this.createLetter.DestinationDepartmentId.push({
      LetterDestinationDepartmentID: this.letterModel.DestinationOU,
      LetterDestinationDepartmentName: ""
    });
    this.RelatedOutgoingLettersId = this.RelatedOutgoingLettersId
      ? this.RelatedOutgoingLettersId
      : this.letterModel.RelatedToOutgoingLetter;
    this.RelatedIncomingLettersId = this.RelatedIncomingLettersId
      ? this.RelatedIncomingLettersId
      : this.letterModel.RelatedToIncomingLetter;
    if (this.RelatedIncomingLettersId) {
      this.RelatedIncomingLettersId.forEach(data => {
        this.createLetter.RelatedIncomingLetters.push({
          OutgoingLetterReferenceNo: data.value
        });
      });
    }
    if (this.RelatedOutgoingLettersId) {
      this.RelatedOutgoingLettersId.forEach(data => {
        this.createLetter.RelatedOutgoingLetters.push({
          OutgoingLetterReferenceNo: data.value
        });
      });
    }
    this.letterModel.Keywords.forEach((data, index) => {
      this.createLetter.Keywords.push({ keywords: data.value });
    });
    this.createLetter.Attachments = this.letterModel.Attachments;
    return this.createLetter;
  }

  validateForm() {
    var flag = true;
    var destination = this.letterModel.DestinationOU
      ? true
      : false;
    var Keywords = this.letterModel.Keywords
      ? this.letterModel.Keywords.length > 0
      : false;
    var username = this.letterModel.DestinationUserId
      ? true
      : false;

    if (
      destination &&
      username &&
      this.letterModel.Title.trim() &&
      this.letterModel.ReceivingDate &&
      this.letterModel.ReceivedFromGovernmentEntity &&
      this.letterModel.ApproverDepartmentId &&
      this.letterModel.ApproverId &&
      this.letterModel.DocumentClassification &&
      !this.sendBtnLoad
    ) {
      flag = false;
    }
    return flag;
  }

  bottonReferesh() {
    this.createBtnShow = false;
    this.editBtnShow = false;
    this.downloadBtnShow = true;
    this.viewBtnShow = false;
    this.approverBtn = false;
    this.receiverBtn = false;
    this.deleteBtn = false;
    this.creatorBtn = false;
    this.draftBtn = false;
    this.cloneBtn = false;
  }

  createBtnShow = false;
  editBtnShow = false;
  downloadBtnShow = true;
  viewBtnShow = false;
  approverBtn = false;
  receiverBtn = false;
  deleteBtn = false;
  creatorBtn = false;
  draftBtn = false;
  cloneBtn = false;
  id = "";

  bottonControll() {
    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let userID = userdet.id;
    this.OrgUnitID = userdet.OrgUnitID;
    if (this.screenStatus == "Create") {
      this.createBtnShow = true;
      this.downloadBtnShow = false;
    } else if (this.screenStatus == "Edit") {
      this.editBtnShow = true;
    } else if (
      this.screenStatus == "View" &&
      this.letterModel.CreatedBy == this.currentUser.id
    ) {
      this.viewBtnShow = true;
    } else if (
      this.screenStatus == "View" &&
      this.letterModel.Status == "20" &&
      this.OrgUnitID == "14"
    ) {
      this.receiverBtn = true;
    }
    if (
      this.letterModel.CreatedBy == this.currentUser.id &&
      this.letterModel.Status != "24"
    ) {
      this.creatorBtn = true;
    }
    if (
      this.letterModel.CreatedBy == this.currentUser.id &&
      (this.letterModel.Status == "0" || this.letterModel.Status == "24")
    ) {
      this.draftBtn = true;
    }
    this.distinationBtnDis = false;
    if (
      this.screenStatus == "View" && this.letterModel.IsRedirect != 1 &&
      this.letterModel.ApproverName == this.currentUser.id &&
      (this.letterModel.Status == 25 || this.letterModel.Status == 26)
    ) {
      if (this.letterModel.Status == 26 && this.incomingCheck) {
        if (!this.checkHisLog()) {
          this.approverBtnDis = true;
        }
        this.approverBtn = true;
        if (this.letterModel.Status == 25)
          this.distinationBtnDis = true;
      }
      if (this.letterModel.Status == 25) {
        if (!this.checkHisLog()) {
          this.approverBtnDis = true;
        }
        this.approverBtn = true;
        if (this.letterModel.Status == 25)
          this.distinationBtnDis = true;
      }
    }

    if (this.screenStatus == "View" && this.letterModel.IsRedirect != 1 &&
      this.letterModel.ApproverId == this.currentUser.id &&
      (this.letterModel.Status == 25 || this.letterModel.Status == 26)) {
      if (this.letterModel.Status == 25)
        this.distinationBtnDis = true;

    }

    // if (
    //   this.screenStatus == "View" &&
    //   this.letterModel.ApproverName == this.currentUser.id &&
    //   this.letterModel.Status == "25"
    // ) {
    //   this.approverBtn = true;
    // }
    if (this.letterModel.Status == 26 || this.letterModel.Status == 25) {
      if (
        this.screenStatus == "View" &&
        this.letterModel.DestinationUserId == this.currentUser.id &&
        this.letterModel.IsRedirect != 1 &&
        this.letterModel.Status != 29
      ) {
        if (this.letterModel.Status == 26)
          this.approverBtnDis = true;

        this.destinationBtn = true;
        this.receiverBtn = true;
        this.editBtnShow = false;
      } else {
        if (
          this.letterModel.IsRedirect == 0 &&
          this.letterModel.Status != 29 && this.letterModel.CreatedBy != this.currentUser.id) {
          if (this.letterModel.Status == 26)
            this.approverBtnDis = true;
          this.destinationBtn = true;
          this.receiverBtn = true;
          this.editBtnShow = false;
        }
      }
    }
    // if(this.letterModel.DestinationUserId != this.currentUser.id || this.letterModel.IsRedirect != 1){
    //   this.distinationBtnDis = true;
    // }
    // if(this.letterModel.ApproverName != this.currentUser.id){
    //   this.approverBtnDis = true;
    // }
    // this.letterModel.DestinationUserId.forEach(element => {
    //   if (element == this.currentUser.id && this.letterModel.Status == "26") {
    //     this.receiverBtn = true;
    //     this.editBtnShow = false;
    //   }
    // });
    if (
      this.letterModel.CreatedBy == this.currentUser.id &&
      this.letterModel.Status == "24"
    ) {
      this.deleteBtn = true;
    }
  }

  checkHisLog() {
    var length = this.letterModel.HistoryLog.length,
      flag = true;
    if (this.letterModel.HistoryLog.length) {
      status = this.letterModel.HistoryLog[length - 1].Action;
    }
    if (status == 'Approve') {
      flag = false;
    }
    return flag;
  }

  async Destination(event) {
    this.DestinationDepartmentId = this.letterModel.DestinationOU;
    await this.getRecvUserList(this.DestinationDepartmentId);
    await this.getDestUserList(this.DestinationDepartmentId);
  }

  async ReletedOutgoingLetterChange(event) {
    this.RelatedOutgoingLettersId = this.letterModel.RelatedToOutgoingLetter;
    let link_lists = [];
    if (this.RelatedOutgoingLettersId.length) {
      this.RelatedOutgoingLettersId.forEach(element => {
        let link_id = element;
        this.relatedOutgoingLetterList.forEach(data => {
          if (link_id == data.ReferenceNo) {
            link_lists.push(data);
          }
        });
      });
    }
    this.link_list = link_lists;
  }

  async getSouceName(UserID, DepID) {
    let params = [
      {
        OrganizationID: DepID,
        OrganizationUnits: "string"
      }
    ];
    this.common.getUserList(params, 0).subscribe((data: any) => {
      let Users = data;
      this.letterModel.SourceName = Users.find(
        x => x.UserID == UserID
      ).EmployeeName.toString();
      this.letterModel.SourceOU = this.destinationDepartment.find(
        x => x.OrganizationID == DepID
      ).OrganizationUnits;
    });
  }

  async ReletedIncomingLetterChange(event) {
    let incoming_link_lists = [];
    if (this.letterModel.RelatedToIncomingLetter) {
      this.relatedIncomingLetterList.forEach(data => {
        if (this.letterModel.RelatedToIncomingLetter == data.ReferenceNo) {
          incoming_link_lists.push(data);
        }
      });
    }
    this.incoming_link_list = incoming_link_lists;
  }

  async ApproverDestination(event) {
    this.letterModel.ApproverId = 0;
    let params = [];
    params.push({
      OrganizationID: this.letterModel.ApproverDepartmentId,
      OrganizationUnits: "string"
    });

    this.common
      .getmemoUserList(params, this.currentUser.id)
      .subscribe((data: any) => {
        this.userDestination = data;
      });
  }

  onTextChange(event) {
    if (event != "")
      this.letterModel.Keywords.push({ display: event, value: event });
  }

  userName(event) {
    this.createLetter.DestinationUserId = [];
    this.createLetter.DestinationUserId.push({
      LetterDestinationUsersID: this.letterModel.DestinationUserId,
      LetterDestinationUsersName: ""
    });
    if (event)
      this.letterModel.ApproverId = this.letterModel.DestinationUserId;
    this.selectedUsersDepartment = event;
  }
  async getRecvUserList(departments) {
    this.letterModel.DestinationUserId = '';
    this.letterModel.ApproverId = '';
    this.letterModel.ApproverDepartmentId = departments;
    let params = [];
    params.push({ OrganizationID: departments, OrganizationUnits: "string" });
    this.common.getUserList(params, 0).subscribe((data: any) => {
      this.userReceiver = data;
      // this.userDestination = this.userReceiver;
    });
  }

  async getRecvPrepareUserList(departments) {
    let params = [];
    if (departments.length) {
      departments.forEach(element => {
        params.push({
          OrganizationID: element.LetterDestinationDepartmentID,
          OrganizationUnits: "string"
        });
      });
    }
    this.common.getUserList(params, 0).subscribe((data: any) => {
      this.userReceiver = data;
    });
  }

  async getDestUserList(id) {
    if (id) {
      let params = [
        {
          OrganizationID: id,
          OrganizationUnits: "string"
        }
      ];
      this.common.getmemoUserList(params, 0).subscribe((data: any) => {
        this.userDestination = data;
      });
      // this.common.getUserList(params, 0).subscribe((data: any) => {
      //   this.userDestination = data;
      // });
    } else {
      this.userDestination = [];
    }
  }

  saveLetter(data) {
    if ((data == "Submit" || data == "Save") && this.screenStatus == "Create") {
      var requestData = this.prepareData();
      if (data == "Submit") {
        this.message = this.sentMsg;
      } else {
        this.message = this.draftMsg;
      }
      requestData.Action = data;
      this.incomingletterservice
        .saveLetter("InboundLetter", requestData)
        .subscribe(data => {
          console.log(data);
          this.bsModalRef = this.modalService.show(
            SuccessComponent,
            this.config
          );
          this.bsModalRef.content.message = this.message;
          this.bsModalRef.content.pagename = "Letter";
        });
    } else if (
      (data == "Submit" || data == "Save") &&
      this.screenStatus == "Edit"
    ) {
      requestData = this.prepareData();
      requestData.Action = data;
      if (data == "Submit") {
        this.message = this.sentMsg;
      } else {
        this.message = this.draftMsg;
      }
      this.incomingletterservice
        .updateLetter("InboundLetter", requestData)
        .subscribe(data => {
          this.bsModalRef = this.modalService.show(
            SuccessComponent,
            this.config
          );
          this.bsModalRef.content.message = this.message;
          this.bsModalRef.content.pagename = "Letter";
        });
    }
  }

  saveClone(data) {
    var requestData = this.prepareData();
    requestData.Action = data;
    this.incomingletterservice
      .saveClone(
        "InboundLetter/Clone",
        this.letterModel.LetterID,
        this.currentUser.id
      )
      .subscribe(data => {
        console.log(data);
        this.message = this.cloneMsg;
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.bsModalRef.content.message = this.message;
        if (this.common.language == "English") {
          this.bsModalRef.content.page_url =
            "app/letter/incomingletter-edit/" + data;
        } else {
          this.bsModalRef.content.page_url =
            "app/letter/incomingletter-edit/" + data;
        }
        this.bsModalRef.content.pagename = "Letter Clone";
      });
  }

  statusChange(status: any, dialog) {
    var data = this.formatPatch(status, "Action");
    this.commonMes = status;
    this.incomingletterservice
      .statusChange("InboundLetter", this.letterModel.LetterID, data)
      .subscribe(data => {
        if (status == "Approve") {
          this.message = this.approvedMsg;
          this.approveBtnLoad = false;
          this.btnLoad = false;
          this.bottonReferesh();
        } else if (status == "Reject") {
          this.message = this.rejectedMsg;
        } else if (status == "Close") {
          this.message = this.closedMsg;
        } else {
          this.message =
            this.common.language == "English"
              ? "Letter " + status + "d Successfully"
              : this.arabic(
                this.arabicSearch("Letter " + status + "d Successfully")
              );
        }
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.bsModalRef.content.message = this.message;
        this.bsModalRef.content.pagename = "Letter";
        this.bsModalRef.content.incoming = (status == "Approve") ? this.incomingCheck : false;
        this.bsModalRef.content.trigger.subscribe(result => {
          this.loadData(this.currentLetterID, this.currentUser.id);
        });
      });
  }

  popup(status: any, btn = false) {
    this.bsModalRef = this.modalService.show(ModalComponent);
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.fromScreen = "Incoming Letter";
    this.bsModalRef.content.pagename = "Letter";
    this.bsModalRef.content.destination = btn;
    this.bsModalRef.content.memoid = this.letterModel.LetterID;
    this.bsModalRef.content.Comments = this.letterModel.Comments;
    this.bsModalRef.content.onClose.subscribe(result => {
      this.btnLoad = result;
    });
  }

  downloadPdf() {
    this.incomingletterservice
      .downloandLetter(
        "InboundLetter/Download",
        this.letterModel.LetterID,
        this.currentUser.id,
        ""
      )
      .subscribe(data => {
        let name = "",
          type = "";
        if (this.letterModel.DestinationUserId.length > 1) {
          name = this.letterModel.LetterReferenceNumber;
          type = ".zip";
        } else {
          if (this.letterModel.DestinationUserId[0] == null) {
            name = this.letterModel.LetterReferenceNumber;
            type = ".pdf";
          } else {
            name =
              this.letterModel.LetterReferenceNumber +
              "_" +
              this.Destination_user_name;
            type = ".pdf";
          }
        }
        this.incomingletterservice.printPdf(name, type);
        this.downloadBtnLoad = false;
        this.btnLoad = false;
      });
  }

  createDutyTask() {
    var url =
      "/" +
      this.common.currentLang +
      "/app/task/task-create/" +
      this.letterModel.LetterReferenceNumber;
    this.router.navigate([url]);
  }
  print() { }
  delete(id) {
    this.incomingletterservice
      .deleteLetter(
        "InboundLetter",
        this.letterModel.LetterID,
        this.currentUser.id
      )
      .subscribe(data => {
        this.message = this.deletedMsg;
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.bsModalRef.content.message = this.message;
        this.bsModalRef.content.pagename = "Letter";
      });
  }

  async onentitychange(Entityid) {
    this.lastContactEntity = Entityid;
    if (Entityid == null) {
      Entityid = 0;
    }
    this.letterModel.ReceivedFromGovernmentEntity = "";
    this.letterModel.ReceivedFromName = "";
    this.incomingletterservice
      .getEntity("InboundLetter", Entityid)
      .subscribe((data: any) => {
        this.organisationEntity = data;
        console.log(this.letterModel);
      });
  }

  async onEntityNamechange(isGovEntityid, entityID) {
    if (isGovEntityid == null) {
      isGovEntityid = 0;
    }
    this.incomingletterservice
      .getEntityByName("InboundLetter", isGovEntityid, entityID)
      .subscribe((data: any) => {
        // this.organisationEntityNames = data;
        this.letterModel.ReceivedFromName = data[0].UserName;
      });
  }

  checkEntity(data) {
    this.letterModel.ReceivedFromName = "";
    var id = this.letterModel.OfficialEntity,
      entityID = data.ID;

    this.onEntityNamechange(id, entityID);
  }

  formatPatch(val, path) {
    var data = [
      {
        value: val,
        path: path,
        op: "Replace"
      },
      {
        value: this.letterModel.Comments,
        path: "Comments",
        op: "Replace"
      },
      {
        value: this.currentUser.id,
        path: "UpdatedBy",
        op: "Replace"
      },
      {
        value: new Date(),
        path: "UpdatedDateTime",
        op: "Replace"
      }
    ];
    return data;
  }

  hisLog(status) {
    if (status == "Reject") {
      return this.common.language == "English"
        ? "Rejected"
        : this.common.arabic.words["rejectedby"];
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
      return this.common.language == "English"
        ? status + "d"
        : this.common.arabic.words[(status + "dby").toLocaleLowerCase()];
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  arabicSearch(string) {
    return string.toLocaleLowerCase().replace(" ", "");
  }

  addContact() {
    this.common.showContact();
  }

  openLinkToModal(type: string) {
    const initialState = {
      type
    };
    this.bsModalRef = this.modalService.show(LinkToModalComponent, {
      initialState,
      class: "modal-lg"
    });
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      let seletedIds = this.bsModalRef.content.selectedIds;
      if (seletedIds && seletedIds.length) {
        //const value = seletedIds.join();
        seletedIds.map(res => {
          let display = res,
            value = res;
          if (type === "in") {
            this.onAddChangein({ display, value });
          } else {
            this.onAddChangeout({ display, value });
          }
        });
        this.bsModalRef.content.selectedIds = [];
      }
    });
  }

  checkLinkList(data, id) {
    var checkdata = true;
    if (data.length > 0)
      checkdata = data.find(res => res.display == id.display) ? false : true;
    return checkdata;
  }

  getRefLink(data, type) {
    return this.util.genarateLinkUrl("letter", data, type);
  }

  checkDepartmentUser(event) {
    if (this.selectedUsersDepartment.length > 0) {
      var selected = this.selectedUsersDepartment;
      this.letterModel.DestinationUserId = [];
      this.selectedUsersDepartment = [];
      this.selectedUsersDepartment = selected.filter(res => res.DepartmentID != event.value.OrganizationID);
      this.selectedUsersDepartment.map(user => {
        this.letterModel.DestinationUserId.push(user.UserID);
      });
      //this.createTask.ResponsibleUserId = this.createTask.ResponsibleUserId;
    }
  }

  Res_user_change(event) {
  }

  showSpanForEscalateRedirect(action){
    if(action === 'Redirect' || action === 'Escalate'){
      return true
    }else{
      return false
    }
  }
}
