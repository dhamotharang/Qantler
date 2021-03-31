import { Component, OnInit, ViewChild, ElementRef, TemplateRef, ChangeDetectorRef } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { FormGroup, FormControl } from '@angular/forms';
import { CreateLetterModal } from './outgoing-letter-form.modal';
import { OutgoingLetterService } from './outgoing-letter-form.service';
import { Router, ActivatedRoute } from '@angular/router';
import 'tinymce';
import { async } from 'q';
import { DatePipe } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../../../../modal/modalcomponent/modal.component';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { CommonService } from '../../../../common.service';
import { HttpEventType } from '@angular/common/http';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { environment } from 'src/environments/environment';
import { LinkToModalComponent } from 'src/app/letter/link-to-modal/link-to-modal.component';

declare var tinymce: any;

@Component({
  selector: 'app-outgoing-letter-form',
  templateUrl: './outgoing-letter-form.component.html',
  styleUrls: ['./outgoing-letter-form.component.scss']
})
export class OutgoingLetterFormComponent implements OnInit {

  @ViewChild('template') template: TemplateRef<any>;
  bsModalRef: BsModalRef;
  @ViewChild('variable') myInputVariable: ElementRef;
  createLetter: CreateLetterModal = new CreateLetterModal();
  screenStatus = 'Create';
  environment = environment;
  //masterData: MasterData = new MasterData();
  Ismemolink: boolean = false;
  Isletterlink: boolean = false;
  multipledropdown: any = [{ index: 1 }];
  dropdownSettings: any;
  letterData: any;
  OrgUnitID = '0';
  outletter = [];
  inletter = [];
  link_list = [];
  incoming_link_list = [];
  status = [];//this.masterData.data.status;
  user = [];//this.masterData.data.user;
  all_user = [];
  department = [];//this.masterData.data.department;
  organisationEntity = [];
  dropdownOptions = ['one', 'two', 'three', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'];
  priorityList = this.common.ReportpriorityList;
  documentList = this.common.documentList;
  attachmentFiles = [];
  AttachmentDownloadUrl = environment.AttachmentDownloadUrl;
  dest_departSettings = {
    singleSelection: false,
    idField: 'OrganizationID',
    textField: 'OrganizationUnits',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 3,
    allowSearchFilter: false
  };
  dest_userSettings = {
    singleSelection: false,
    idField: 'UserID',
    textField: 'EmployeeName',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
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
    ["paragraph", "orderedList", "unorderedList"],
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
  letterModel = {
    LetterID: 0,
    UserID: 0,
    DepartmentID: 0,
    LetterReferenceNumber: '',
    Status: '',
    CreatedDateTime: new Date(),
    SourceOU: '',
    SourceName: '',
    Title: '',
    DestinationOU: [],
    DestinationUsername: [],
    DestinationUserId: [],
    DestinationDepartmentId: [],
    ReceivingDate: '',
    ReceivedFromGovernmentEntity: '',
    ReceivedFromName: '',
    ApproverDepartmentId: 0,
    ApproverId: 0,
    RelatedToOutgoingLetter: [],
    RelatedToIncomingLetter: [],
    LetterPhysicallySend: '',
    CreatedBy: 0,
    Action: '',
    Comments: '',
    LetterType: 'Outgoing',
    LetterDetails: '',
    RelatedOutgoingLetters: [],
    RelatedIncomingLetters: [],
    Notes: '',
    DocumentClassification: '',
    Priority: '',
    Attachments: [],
    Keywords: [],
    NeedReply: '',
    HistoryLog: [],
    ApproverName: 0,
    IsRedirect: 0,
    OfficialEntity: []

  }

  bsConfig: Partial<BsDatepickerConfig>;
  bsConfigs: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  img_file: any;
  message: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  attachments: any = [];
  userDestination: any;
  userReceiver: any = [];
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
  // breadvrum label
  internalletter = (this.common.language == 'English') ? "Internal Letters" : this.arabic('internalletters');
  outgoingletter = (this.common.language == 'English') ? "Outgoing Letters" : this.arabic('outgoingletters');
  create = (this.common.language == 'English') ? "Create" : this.arabic('create');
  letter = (this.common.language == 'English') ? "Letter" : this.arabic('letters');

  IsExistFlag: any = [false];
  organisationEntityNames: any = [];
  organizationList: any;
  sentMsg: any;
  draftMsg: any;
  cloneMsg: any;
  approvedMsg: any;
  rejectedMsg: any;
  closedMsg: any;
  deletedMsg: any;
  currentApproverId: any;

  tinyConfig = {
    plugins: 'powerpaste casechange importcss tinydrive searchreplace directionality visualblocks visualchars fullscreen table charmap hr pagebreak nonbreaking toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons',
    language: this.common.language != 'English' ? "ar" : "en",
    menubar: 'file edit view insert format tools table tc help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media pageembed template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment',
    toolbar_drawer: 'sliding',
    directionality: this.common.language != 'English' ? "rtl" : "ltr"
  };
  canEditContact: any;
  destinationBtn: boolean = false;
  screenTitle: any;
  lastContactEntity: any;
  lastContactIndex: any = 0;
  contactTriggerEvent: any;

  constructor(private changeDetector: ChangeDetectorRef, private outgoingletterservice: OutgoingLetterService, public common: CommonService, public router: Router, route: ActivatedRoute, public datepipe: DatePipe,
    private modalService: BsModalService, public util: UtilsService) {
    this.canEditContact = this.currentUser.CanEditContact
    if (this.common.language == 'English') {
      this.priorityList = ['High', 'Medium', 'Low', 'Verylow'];
    } else { this.priorityList = [this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')]; }

    route.url.subscribe(() => {
      console.log(route.snapshot.data);
      this.screenStatus = route.snapshot.data.title;

    });
    route.params.subscribe(param => {
      var id = +param.id;
      if (id > 0)
        this.loadData(id, this.currentUser.id);
    });
    this.outgoingletterservice.getMemo('memo', 0, 0).subscribe((data: any) => {
      this.department = data.M_ApproverDepartmentList;
    });

    // this.outgoingletterservice.getCombo('OutboundLetter', 0, this.currentUser.id).subscribe((data: any) => {
    //   this.organisationEntity[0] = data.OrganisationEntity;
    // });

    if (this.screenStatus == 'Create') {
      this.relatedLetter();
      this.common.breadscrumChange(this.outgoingletter, this.create, '');
    }
  }

  async loadData(id, userid) {
    this.common.breadscrumChange(this.outgoingletter, this.letter + ' ' + id, '');
    await this.outgoingletterservice.getCombo('OutboundLetter', id, userid).subscribe((data: any) => {
      this.letterData = data;
      if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
        let that = this;
        this.status = this.letterData.M_LookupsList;
        var date = this.letterModel.CreatedDateTime;
        this.letterModel.CreatedDateTime = new Date(date);
        this.setData(this.letterData);
      } else {
        this.initPage();
        this.bottonControll();
      }
    });
  }

  async setData(data) {
    await this.getDestUserList(+data.ApproverDepartmentId);
    this.currentApproverId = data.CurrentApprover[0].ApproverId;
    if (data.DestinationDepartmentId)
      this.getRecvPrepareUserList(data.DestinationDepartmentId);
    if (data.DestinationDepartmentId)
      this.getRecvUsersList(data.DestinationDepartmentId);
    this.letterModel.LetterID = data.LetterID;
    this.letterModel.LetterReferenceNumber = data.LetterReferenceNumber
    this.letterModel.Title = data.Title;
    this.organizationList = data.OrganizationList;
    this.getSouceName(data.SourceName, data.SourceOU);
    this.letterModel.ReceivedFromName = data.ReceivedFromName;
    this.letterModel.ApproverDepartmentId = +data.ApproverDepartmentId;
    this.letterModel.ApproverId = data.ApproverId;
    this.letterModel.IsRedirect = data.IsRedirect;
    this.letterModel.LetterDetails = data.LetterDetails;
    this.letterModel.ReceivedFromGovernmentEntity = data.ReceivedFromGovernmentEntity;
    if (data.NeedReply == true || data.NeedReply == false) {
      this.letterModel.NeedReply = '' + data.NeedReply;
    }
    if (data.LetterPhysicallySend == true || data.LetterPhysicallySend == false) {
      this.letterModel.LetterPhysicallySend = '' + data.LetterPhysicallySend;
    }
    this.letterModel.HistoryLog = data.HistoryLog;
    const DestinationOU = [];
    const DestinationUsername = [];
    const IsGovernmentEntity = [];
    this.multipledropdown = [];
    if (data.DestinationEntity == '') {
      this.multipledropdown = [{ index: 1 }];
    } else {
      data.DestinationEntity.forEach((department, index) => {
        var IsEntity = (department.IsGovernmentEntity) ? '1' : '2';
        this.onentitychange(IsEntity, index);
        //this.onEntityNamechange(IsEntity, department.LetterDestinationEntityName, index);
        this.multipledropdown.push({ index: index + 1 });
        IsGovernmentEntity.push(IsEntity);
        DestinationOU.push(department.LetterDestinationEntityID);
        DestinationUsername.push(department.LetterDestinationUserName);
      });
    }
    this.letterModel.DestinationOU = DestinationOU;
    this.letterModel.DestinationUsername = DestinationUsername;
    this.DestinationDepartmentId = DestinationOU;

    this.letterModel.OfficialEntity = IsGovernmentEntity;
    const RelatedOutgoingLetters = [];
    if (data.RelatedOutgoingLetters.length > 0) {
      data.RelatedOutgoingLetters.forEach(key => {
        this.letterModel.RelatedToOutgoingLetter.push({ display: key.OutgoingLetterReferenceNo, value: key.OutgoingLetterReferenceNo });
        //this.link_list.push(key);
      });
      this.outletter = this.letterModel.RelatedToOutgoingLetter;
      this.incoming_link_list = data.RelatedIncomingLetters;
      this.link_list = data.RelatedOutgoingLetters;
    } else {
      this.letterModel.RelatedToOutgoingLetter = [];
    }
    if (data.RelatedIncomingLetters.length > 0) {
      data.RelatedIncomingLetters.forEach(element => {
        this.letterModel.RelatedToIncomingLetter.push({ display: element.OutgoingLetterReferenceNo, value: element.OutgoingLetterReferenceNo });
      });
      this.inletter = this.letterModel.RelatedToIncomingLetter;
    } else {
      this.letterModel.RelatedToIncomingLetter = [];
    }
    if (data.CurrentApprover.length > 0) {
      this.letterModel.ApproverName = data.CurrentApprover[0].ApproverId;
    }
    this.letterModel.DestinationDepartmentId = data.DestinationDepartmentId;
    this.letterModel.DocumentClassification = this.documentList[data.DocumentClassification];
    this.letterModel.Notes = data.Notes;
    this.letterModel.Priority = this.priorityList[Number(data.Priority)];
    this.letterModel.CreatedBy = data.CreatedBy;
    this.attachments = data.Attachments;
    data.Keywords.forEach(key => {
      this.letterModel.Keywords.push({ display: key.Keywords, value: key.Keywords });
    });
    this.letterModel.Attachments = data.Attachments;
    this.letterModel.Status = data.Status;
    this.letterModel.ApproverId = data.ApproverId;
    this.relatedLetter();
    this.bottonControll();
  }
  public relatedLetter() {
    let letter_user_id = this.currentUser.id;
    if (this.screenStatus != 'Create') {
      letter_user_id = this.letterModel.CreatedBy;
    }
    this.outgoingletterservice.getReleatedOutgoingLetterCombo('OutboundLetter/RelatedOutgoingLetters', letter_user_id).subscribe((data: any) => {
      this.relatedOutgoingLetterList = data;
      // this.ReletedOutgoingLetterChange();
      //
    });
    this.outgoingletterservice.getReleatedIncomingLetterCombo('OutboundLetter/RelatedIncomingLetters', letter_user_id).subscribe((data: any) => {
      this.relatedIncomingLetterList = data;
      //this.ReletedIncomingLetterChange();
      ///
    });
  }


  async ngOnInit() {

    this.contactTriggerEvent = this.common.contactChanged$.subscribe(res => {
      if (this.lastContactEntity)
        this.onentitychange(this.lastContactEntity, this.lastContactIndex);
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

    this.priorityList = (this.common.language == 'English') ? ['High', 'Medium', 'Low', 'Verylow'] : [this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
    this.common.triggerScrollTo('trigger-letter-out');
    this.sentMsg = (this.common.language == 'English') ? "Letter Sent Successfully." : this.arabic('lettersentsuccess');
    this.draftMsg = (this.common.language == 'English') ? "Letter Drafted Successfully." : this.arabic('letterdraftsaved');
    this.cloneMsg = (this.common.language == 'English') ? "Letter Cloned Successfully." : this.arabic('letterclonedsuccessfully');
    this.approvedMsg = (this.common.language == 'English') ? "Letter Approved Successfully." : this.arabic('letterapprovedsuccessfully');
    this.rejectedMsg = (this.common.language == 'English') ? "Letter Rejected Successfully." : this.arabic('letterrejectedsuccessfully');
    this.closedMsg = (this.common.language == 'English') ? "Letter Closed Successfully." : this.arabic('letterclosedsuccessfully');
    this.deletedMsg = (this.common.language == 'English') ? "Letter Deleted Successfully." : this.arabic('letterdeletedsuccessfully');

    this.bottonControll();

    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
    this.letterModel.ApproverDepartmentId = this.currentUser.DepartmentID;
    this.ApproverDestination('');
  }
  public tinyMceSettings = {
    skin_url: 'assets/tinymce/skins/lightgray',
    inline: false,
    statusbar: false,
    browser_spellcheck: true,
    height: 320,
    plugins: 'fullscreen',
  };


  ngAfterViewInit() {

  }
  addBulkCreation() {
    let index = this.multipledropdown.length;
    this.multipledropdown.push({ index: index + 1 });
  }
  closemodal() {
    this.modalService.hide(1);
    setTimeout(function () { location.reload(); }, 1000);
  }


  initPage() {
    this.letterModel.LetterID = 0
    this.letterModel.LetterReferenceNumber = '';
    this.letterModel.Status = '';
    this.letterModel.CreatedDateTime = new Date();
    this.letterModel.SourceOU = this.currentUser.department;
    this.letterModel.SourceName = this.currentUser.username;
    this.letterModel.Title = '';
    this.letterModel.DestinationOU = [];
    this.letterModel.DestinationUsername = [];
    this.letterModel.DestinationUserId = [];
    this.letterModel.DestinationDepartmentId = [];
    this.letterModel.ReceivingDate = '';
    this.letterModel.ReceivedFromGovernmentEntity = '';
    this.letterModel.ReceivedFromName = '';
    this.letterModel.ApproverDepartmentId = this.currentUser.DepartmentID;
    this.letterModel.ApproverId = 0;
    this.letterModel.RelatedToOutgoingLetter = [];
    this.letterModel.RelatedToIncomingLetter = [];
    this.letterModel.LetterPhysicallySend = '';
    this.letterModel.CreatedBy = this.currentUser.id;
    this.letterModel.OfficialEntity = [];
    this.letterModel.Action = '';
    this.letterModel.Comments = '';
    this.letterModel.LetterType = 'Outgoing';
    this.letterModel.LetterDetails = '';
    this.letterModel.RelatedOutgoingLetters = [];
    this.letterModel.Notes = '';
    this.letterModel.DocumentClassification = '';
    this.letterModel.Priority = '';
    this.letterModel.Keywords = [];
    this.letterModel.Attachments = [];
    this.letterModel.NeedReply = '';
    this.letterModel.HistoryLog = [];
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
          this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'LetterID': '' });
        }
        this.letterModel.Attachments = this.attachments;
      }
    });
    this.myInputVariable.nativeElement.value = "";
  }

  selectChange(data) {
    this.letterModel.DestinationOU;
  }

  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.myInputVariable.nativeElement.value = "";
  }

  onAddChangeout(event) {
    if (event) {
      let type = '1';
      this.outgoingletterservice.getLetters('InboundLetter/RelatedInOutLetterswithRef', this.currentUser.id, type, event.value).subscribe((data: any) => {
        if (data.length > 0 && data[0].ReferenceNo && this.checkLinkList(this.outletter, event)) {
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
      if (this.link_list[i].ReferenceNo.toLowerCase() == event.value.toLowerCase()) {
        this.link_list.splice(i, 1);
      }
    }
  }

  onAddChangein(event) {
    if (event) {
      let type = '0';
      this.outgoingletterservice.getLetters('InboundLetter/RelatedInOutLetterswithRef', this.currentUser.id, type, event.value).subscribe((data: any) => {
        if (data.length > 0 && data[0].ReferenceNo && this.checkLinkList(this.inletter, event)) {
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
      if (this.incoming_link_list[i].ReferenceNo.toLowerCase() == event.value.toLowerCase()) {
        this.incoming_link_list.splice(i, 1);
      }
    }
  }

  prepareData() {
    this.createLetter.LetterID = this.letterModel.LetterID;
    this.createLetter.LetterReferenceNumber = this.letterModel.LetterReferenceNumber;
    this.createLetter.Title = this.letterModel.Title;
    this.createLetter.SourceOU = this.currentUser.DepartmentID;
    this.createLetter.SourceName = this.currentUser.UserID;
    this.createLetter.ApproverDepartmentId = this.letterModel.ApproverDepartmentId;
    this.createLetter.ApproverId = this.letterModel.ApproverId;
    this.createLetter.ReceivingDate = this.letterModel.ReceivingDate;
    this.createLetter.ReceivedFromGovernmentEntity = this.letterModel.ReceivedFromGovernmentEntity;
    this.createLetter.ReceivedFromName = this.letterModel.ReceivedFromName;
    this.createLetter.LetterDetails = this.letterModel.LetterDetails;
    this.createLetter.Notes = this.letterModel.Notes;
    this.createLetter.DocumentClassification = this.documentList.indexOf(this.letterModel.DocumentClassification).toString();
    this.createLetter.Priority = this.priorityList.indexOf(this.letterModel.Priority).toString();
    this.createLetter.NeedReply = this.letterModel.NeedReply;
    this.createLetter.LetterPhysicallySend = this.letterModel.LetterPhysicallySend;
    if (this.screenStatus != 'Edit') {
      this.createLetter.CreatedBy = this.currentUser.id;
      this.createLetter.CreatedDateTime = this.letterModel.CreatedDateTime;
    } else {
      this.createLetter.UpdatedBy = this.currentUser.id;
      this.createLetter.UpdatedDateTime = this.letterModel.CreatedDateTime;
    }
    this.createLetter.Action = this.letterModel.Action;
    this.createLetter.Comments = this.letterModel.Comments;
    this.createLetter.LetterType = this.letterModel.LetterType;
    this.createLetter.DestinationDepartmentId = this.letterModel.DestinationDepartmentId;
    this.createLetter.IsGovernmentEntity = this.letterModel.OfficialEntity[0];

    if (this.letterModel.DestinationOU) {
      this.createLetter.DestinationDepartmentId = [];
      this.letterModel.DestinationOU.forEach((data, index) => {
        this.createLetter.DestinationEntity.push({
          "LetterDestinationID": 0,
          "LetterDestinationEntityID": data,
          "IsGovernmentEntity": (this.letterModel.OfficialEntity[index] == '1') ? true : false,
          'LetterDestinationEntityName': '',
          'LetterDestinationUserName': this.letterModel.DestinationUsername[index],
        });
      });
    }
    // }
    this.RelatedOutgoingLettersId = (this.RelatedOutgoingLettersId) ? this.RelatedOutgoingLettersId : this.letterModel.RelatedToOutgoingLetter;
    this.RelatedIncomingLettersId = (this.RelatedIncomingLettersId) ? this.RelatedIncomingLettersId : this.letterModel.RelatedToIncomingLetter;
    if (this.RelatedOutgoingLettersId) {
      this.RelatedOutgoingLettersId.forEach(data => {
        this.createLetter.RelatedOutgoingLetters.push({
          "OutgoingLetterReferenceNo": data.value
        });
      });
    }

    if (this.RelatedIncomingLettersId) {
      this.RelatedIncomingLettersId.forEach(data => {
        this.createLetter.RelatedIncomingLetters.push({
          "OutgoingLetterReferenceNo": data.value
        });
      });
    }

    this.letterModel.Keywords.forEach((data, index) => {
      this.createLetter.Keywords.push({ 'keywords': data.value });
    });
    this.createLetter.Attachments = this.letterModel.Attachments;
    return this.createLetter;
  }

  validateForm() {
    var flag = true;
    //var destination = (this.letterModel.DestinationOU) ? (this.letterModel.DestinationOU.length > 0) : false;
    var Keywords = (this.letterModel.Keywords) ? (this.letterModel.Keywords.length > 0) : false;
    var username = (this.letterModel.DestinationUsername) ? (this.letterModel.DestinationUsername.length > 0) : false;
    if (username && this.letterModel.Title.trim() && this.validateEntity() && this.letterModel.ApproverDepartmentId
      && this.letterModel.ApproverId && this.letterModel.DocumentClassification && !this.sendBtnLoad) {
      flag = false;
    }
    return flag;
  }

  validateEntity() {
    var flag = false;
    this.multipledropdown.map(i => {
      if (this.letterModel.OfficialEntity[i.index - 1] &&
        this.letterModel.DestinationOU[i.index - 1]) {
        flag = true;
      } else {
        flag = false;
      }
    });
    return flag;
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
  id = '';



  bottonControll() {
    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let userID = userdet.id;
    this.OrgUnitID = userdet.OrgUnitID;
    if (this.screenStatus == 'Create') {
      this.createBtnShow = true;
      this.downloadBtnShow = false;
    } else if (this.screenStatus == 'Edit') {
      this.editBtnShow = true;
    } else if (this.screenStatus == 'View' && this.letterModel.CreatedBy == this.currentUser.id) {
      this.viewBtnShow = true;
      this.receiverBtn = true;
    } else if (this.screenStatus == 'View' && this.letterModel.Status == '20' && this.OrgUnitID == '14') {
      this.receiverBtn = true;
    }
    if (this.letterModel.CreatedBy == this.currentUser.id && this.letterModel.Status != '18') {
      this.creatorBtn = true;
    }
    if (this.letterModel.CreatedBy == this.currentUser.id && (this.letterModel.Status == '0' || this.letterModel.Status == '18')) {
      this.draftBtn = true;
    }
    if (this.screenStatus == 'View' && this.currentApproverId == this.currentUser.id && this.letterModel.Status == '19') {
      this.approverBtn = true;
    }
    if (this.screenStatus == 'View' && this.letterModel.ApproverName == this.currentUser.id && this.letterModel.Status == '19') {
      this.approverBtn = true;
    }
    if (this.letterModel.Status == '20') {
      if (this.screenStatus == 'View' &&
        this.currentUser.UnitID == 14 &&
        !this.letterModel.IsRedirect && this.letterModel.IsRedirect == 0) {
        this.destinationBtn = true;
        this.receiverBtn = true;
      } else {
        if (!this.letterModel.IsRedirect &&
          this.letterModel.IsRedirect == 0) {
          this.destinationBtn = true;
          this.receiverBtn = true;
        }
      }
    }

    // if (this.letterModel.Status == 26) {
    //   if (
    //     this.screenStatus == "View" &&
    //     this.letterModel.DestinationUserId.find(
    //       res => res == this.currentUser.id
    //     ) &&
    //     !this.letterModel.IsRedirect &&
    //     this.letterModel.IsRedirect == 0 &&
    //     this.letterModel.Status != 29
    //   ) {
    //     this.destinationBtn = true;
    //     this.receiverBtn = true;
    //     this.editBtnShow = false;
    //   } else {
    //     if (!this.letterModel.IsRedirect &&
    //       this.letterModel.IsRedirect == 0 &&
    //       this.letterModel.Status != 29) {
    //       this.destinationBtn = true;
    //       this.receiverBtn = true;
    //       this.editBtnShow = false;
    //     }
    //   }
    // }


    this.letterModel.DestinationOU.forEach(element => {
      if (element == this.currentUser.id && this.letterModel.Status == '20') {
        this.receiverBtn = true;
        this.editBtnShow = false;
      }
    });
    if (this.letterModel.CreatedBy == this.currentUser.id && this.letterModel.Status == '18') {
      this.deleteBtn = true;
    }
  }

  addContact() {
    this.common.showContact();
  }

  async Destination(event) {
    this.DestinationDepartmentId = this.letterModel.DestinationOU;
    await this.getRecvUserList(this.DestinationDepartmentId);
  }

  async getSouceNameOU(DepID) {
    this.letterModel.SourceOU = this.department.find(x => x.OrganizationID == DepID).OrganizationUnits;
  }

  async getSouceName(UserIDs, DepID) {
    let params = [{
      "OrganizationID": DepID,
      "OrganizationUnits": "string"
    }];
    this.common.getUserList(params, 0).subscribe((data: any) => {
      this.letterModel.SourceName = data[data.findIndex(x => x.UserID == UserIDs)].EmployeeName.toString();
      this.letterModel.SourceOU = this.organizationList.find(x => x.OrganizationID == DepID).OrganizationUnits;
    });
  }


  async ReletedOutgoingLetterChange() {
    this.RelatedOutgoingLettersId = this.letterModel.RelatedToOutgoingLetter;
    let link_lists = [];
    if (this.RelatedOutgoingLettersId.length) {
      this.relatedOutgoingLetterList.forEach(element => {
        let link_id = element;
        this.RelatedOutgoingLettersId.forEach(data => {
          if (data.display.toLocaleLowerCase == link_id.ReferenceNo.toLocaleLowerCase) {
            link_lists.push(link_id);
          }
        });
      });
    }
    this.link_list = link_lists;
  }

  async ReletedIncomingLetterChange() {
    let incoming_link_lists = [];
    if (this.letterModel.RelatedToIncomingLetter.length) {
      this.letterModel.RelatedToIncomingLetter.forEach(element => {
        let link_id = element;
        this.relatedIncomingLetterList.forEach(data => {
          if (data.ReferenceNo.toLocaleLowerCase == link_id.value.toLocaleLowerCase) {
            incoming_link_lists.push(data);
          }
        });
      });
    }
    this.incoming_link_list = incoming_link_lists;
  }

  onTextChange(event) {
    if (event != '')
      this.letterModel.Keywords.push({ display: event, value: event });
  }


  async ApproverDestination(event) {
    this.letterModel.ApproverId = 0;
    let params = [];
    params.push({ "OrganizationID": this.letterModel.ApproverDepartmentId, "OrganizationUnits": "string" });

    this.common.getmemoUserList(params, this.currentUser.id).subscribe((data: any) => {
      this.userDestination = data;
    });
  }

  async getRecvUserList(departments) {
    let params = [];
    if (departments.length) {
      departments.forEach(element => {
        params.push({ "OrganizationID": element, "OrganizationUnits": "string" });
      });
      this.common.getUserList(params, 0).subscribe((data: any) => {
        this.userReceiver = data;
      });
    } else {
      this.userReceiver = [];
    }

  }

  async getRecvPrepareUserList(departments) {
    let params = [];
    if (departments.length) {
      departments.forEach((element: any, index) => {
        params.push({ "OrganizationID": element.LetterDestinationDepartmentID, "OrganizationUnits": "string" });
        this.common.getUserList(params, 0).subscribe((data: any) => {
          this.userReceiver[index] = data;
        });
      });
    }
  }

  async getRecvUsersList(departments) {
    let params = [];
    if (departments.length) {
      departments.forEach(element => {
        params: ({ "OrganizationID": element.LetterDestinationDepartmentID, "OrganizationUnits": "string" });
      });
      this.common.getUserList(params, 0).subscribe((data: any) => {
        this.userReceiver = data;
      });
    } else {
      this.userReceiver = [];
    }

  }

  async getDestUserList(id) {
    if (id) {
      let params = [{
        "OrganizationID": id,
        "OrganizationUnits": "string"
      }];
      this.common.getmemoUserList(params, 0).subscribe((data: any) => {
        this.userDestination = data;
        if (this.screenStatus != 'Create')
          this.letterModel.ApproverId = this.letterModel.ApproverId;
      });
    } else {
      this.userDestination = [];
    }
  }

  onentitychange(Entityid, id) {
    this.lastContactIndex = id;
    this.lastContactEntity = Entityid;
    this.letterModel.DestinationOU[id] = '';
    this.letterModel.DestinationUsername[id] = '';
    this.outgoingletterservice.getEntity('OutboundLetter', Entityid).subscribe((data: any) => {
      this.organisationEntity[id] = data;
    });
  }

  onEntityNamechange(Entityid, entityName, index) {
    this.outgoingletterservice.getEntityByName('OutboundLetter', Entityid, entityName).subscribe((data: any) => {
      this.organisationEntityNames[index] = data;
      // this.letterModel.DestinationUsername[index] = this.letterModel.DestinationOU[index];
      this.letterModel.DestinationUsername[index] = data[0].UserName;
    });

  }

  checkEntity(data, index) {
    debugger
    this.letterModel.DestinationUsername[index] = '';
    var id = this.letterModel.OfficialEntity[index],
      name = data.EntityName;
    this.IsExistFlag[index] = false;
    var currentArr = this.letterModel.DestinationOU;
    var IsAvail = this.util.isAlreadyExist(currentArr, id);
    if (IsAvail) {
      currentArr.splice(index, 1);
      currentArr[index] = '';
      this.IsExistFlag[index] = true;
    } else {
      this.onEntityNamechange(id, name, index);
    }
    this.letterModel.DestinationOU = currentArr;
  }

  saveLetter(data) {
    if ((data == 'Submit' || data == 'Save') && this.screenStatus == 'Create') {
      var requestData = this.prepareData();
      if (data == 'Submit') {
        // if (this.common.language == 'English') {
        this.message = this.sentMsg;
        // } else { this.message = this.arabic('lettersentsuccess'); }
      } else {
        // if (this.common.language == 'English') {
        this.message = this.draftMsg;
        // } else { this.message = this.arabic('lettersavesuccess'); }
      }
      requestData.Action = data;
      this.outgoingletterservice.saveLetter('OutboundLetter', requestData).subscribe(data => {
        console.log(data);
        //this.message = "Letter Saved Successfully";
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.bsModalRef.content.message = this.message;
        this.bsModalRef.content.pagename = 'Letter';
        //location.reload();
      });
    } else if ((data == 'Submit' || data == 'Save') && this.screenStatus == 'Edit') {
      requestData = this.prepareData();
      if (data == 'Submit') {
        // if (this.common.language == 'English') {
        this.message = this.sentMsg;
        // } else { this.message = this.arabic('lettersentsuccess'); }
      } else {
        // if (this.common.language == 'English') {
        this.message = this.draftMsg;
        // } else { this.message = this.arabic('lettersavesuccess'); }
      }
      requestData.Action = data;
      this.outgoingletterservice.updateLetter('OutboundLetter', requestData).subscribe(data => {
        //this.message = "Letter Updated Successfully";
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.bsModalRef.content.message = this.message;
        this.bsModalRef.content.pagename = 'Letter';
        //location.reload();
      });
    }
  }

  saveClone(data) {
    var requestData = this.prepareData();
    requestData.Action = data;
    this.outgoingletterservice.saveClone('OutboundLetter/Clone', this.letterModel.LetterID, this.currentUser.id).subscribe(data => {
      console.log(data);
      this.message = this.cloneMsg;
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      if (this.common.language == 'English') {
        this.bsModalRef.content.page_url = 'app/letter/outgoingletter-edit/' + data;
      } else {
        this.bsModalRef.content.page_url = 'app/letter/outgoingletter-edit/' + data;
      }
      this.bsModalRef.content.pagename = 'Letter Clone';
      //this.bsModalRef.content.pagename = 'Letter';
      //location.reload();
    });
  }


  statusChange(status: any, dialog) {
    var data = this.formatPatch(status, 'Action')
    this.commonMes = status;
    this.outgoingletterservice.statusChange('OutboundLetter', this.letterModel.LetterID, data).subscribe(data => {
      if (status == 'Approve') {
        this.message = this.approvedMsg;
      } else if (status == 'Reject') {
        this.message = this.rejectedMsg;
      } else if (status == 'Close') {
        this.message = this.closedMsg;
      } else {
        this.message = (this.common.language == 'English') ? 'Letter ' + status + 'd Successfully' : this.arabic(this.arabicSearch('Letter ' + status + 'd Successfully'));
      }
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Letter';
      this.loadData(this.letterModel.LetterID, this.currentUser.id);
      //location.reload();
    });
  }


  popup(status: any, btn?: boolean) {
    this.bsModalRef = this.modalService.show(ModalComponent, this.config);
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.fromScreen = 'Outgoing Letter';
    this.bsModalRef.content.pagename = 'Letter';
    this.bsModalRef.content.destination = btn;
    this.bsModalRef.content.memoid = this.letterModel.LetterID;
    this.bsModalRef.content.Comments = this.letterModel.Comments;
    this.bsModalRef.content.onClose.subscribe(result => {
      this.btnLoad = result;
    });
  }

  downloadPdf() {
  debugger
    this.outgoingletterservice.downloandLetter('OutboundLetter/Download', this.letterModel.LetterID, this.currentUser.id, '').subscribe(data => {
      let name = '',
        type = '';
      if (this.letterModel.DestinationUsername.length > 1) {
        name = this.letterModel.LetterReferenceNumber;
        type = '.zip';
      } else
      {
        if (this.letterModel.DestinationUsername[0] == null) {
          name = this.letterModel.LetterReferenceNumber;
          type = ".pdf";
        } else {
          name = this.letterModel.LetterReferenceNumber + "_" + this.letterModel.DestinationUsername[0].replace(' ','_');
          type = ".pdf";
        }
    }
      this.outgoingletterservice.printPdf(name, type);
      this.btnLoad = false;
      this.downloadBtnLoad = false;
    });

  }
  print() {

  }
  delete(id) {
    this.outgoingletterservice.deleteLetter('OutboundLetter', this.letterModel.LetterID, this.currentUser.id).subscribe(data => {
      this.message = this.deletedMsg;
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Letter';
    });

  }
  formatPatch(val, path) {
    var data = [{
      "value": val,
      "path": path,
      "op": "Replace"
    }, {
      "value": this.letterModel.Comments,
      "path": "Comments",
      "op": "Replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "Replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "Replace"
    }];
    return data;
  }

  hisLog(status) {
    if (status == 'Reject') {
      return (this.common.language == 'English') ? 'Rejected' : this.common.arabic.words['letterrejectdby'];
    } else if (status == 'Redirect') {
      return (this.common.language == 'English') ? 'Redirected' : this.common.arabic.words['redirectedby'];
    } else if (status == 'Submit') {
      return (this.common.language == 'English') ? 'Submitted' : this.common.arabic.words['submittedby'];
    } else if (status == 'Resubmit') {
      return (this.common.language == 'English') ? 'Resubmitted' : this.common.arabic.words['resubmittedby'];
    } else if (status == 'ReturnForInfo') {
      return (this.common.language == 'English') ? 'ReturnForInfo' : this.common.arabic.words['returnforinfoby'];
    } else if (status == 'Escalate') {
      return (this.common.language == 'English') ? 'Escalated' : this.common.arabic.words['escalatedby'];
    } else if (status == 'Approve') {
      return (this.common.language == 'English') ? 'Approved' : this.common.arabic.words['approvedby'];
    } else {
      console.log('log-beer' + (status + 'dby').toLocaleLowerCase());
      return (this.common.language == 'English') ? status + 'd' : this.common.arabic.words[(status + 'dby').toLocaleLowerCase()];
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  arabicSearch(string) {
    return string.toLocaleLowerCase().replace(' ', '')
  }

  openLinkToModal(type: string) {
    const initialState = {
      type,
    };
    this.bsModalRef = this.modalService.show(LinkToModalComponent, { initialState, class: 'modal-lg' });
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      let seletedIds = this.bsModalRef.content.selectedIds;
      if (seletedIds && seletedIds.length) {
        //const value = seletedIds.join();
        seletedIds.map(res => {
          let display = res,
            value = res;
          if (type === 'in') {
            this.onAddChangein({ display, value })
          } else {
            this.onAddChangeout({ display, value })
          }
        });
        this.bsModalRef.content.selectedIds = [];
      }
    });
  }

  checkLinkList(data, id) {
    var checkdata = true;
    if (data.length > 0)
      checkdata = (data.find(res => res.display == id.display)) ? false : true;
    return checkdata;
  }
  getRefLink(data, type) {
    return this.util.genarateLinkUrl('letter', data, type);
  }

  ngOnDestroy() {
    this.contactTriggerEvent.unsubscribe();
  }
  
  showSpanForEscalateRedirect(action){
    if(action === 'Redirect' || action === 'Escalate'){
      return true
    }else{
      return false
    }
  }

}

