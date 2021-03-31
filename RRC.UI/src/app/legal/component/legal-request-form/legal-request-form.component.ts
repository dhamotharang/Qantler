import { Component, OnInit, OnDestroy, ChangeDetectorRef, ViewChild, TemplateRef, Input, ElementRef } from '@angular/core';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ModalComponent } from 'src/app/modal/modalcomponent/modal.component';
import { CommonService } from 'src/app/common.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { BsModalService, BsModalRef, BsDatepickerConfig } from 'ngx-bootstrap';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { LegalService } from '../../service/legal.service';
import { LegalRequest } from '../../model/legal-request/legal-request.model';
import { HttpEventType } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-legal-request-form',
  templateUrl: './legal-request-form.component.html',
  styleUrls: ['./legal-request-form.component.scss']
})
export class LegalRequestFormComponent implements OnInit,OnDestroy {
  @Input() mode: string;
  @Input() requestId: number;
  @ViewChild('template') template: TemplateRef<any>;
  @ViewChild('labelInput') labelInput: ElementRef<any>;
  @ViewChild('attachment') attachmentInput: ElementRef;
  bsModalRef: BsModalRef;
  createMemo: any = {};
  screenStatus = 'Create';
  uploadProcess: boolean = false;
  uploadPercentage = 0;
  status: any;
  legalRequestBody:any;
  user = [];//this.masterData.data.user;
  legalType = [];
  department = [];
  dropdownOptions = ['one', 'two', 'three', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'];
  attachmentFiles = [];
  config = {
    displayKey: "name",
    height: 'auto',
    placeholder: 'Select',
    limitTo: this.dropdownOptions.length,
    noResultsFound: 'No results found!',
    searchPlaceholder: 'Search',
    searchOnKey: 'name',
  };

  dropdownSettings: any;

  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  LegalRequests :any =[];
  legalRequestModel: LegalRequest = {
    LegalID: 0,
    ReferenceNumber: null,
    SourceOU: '',
    SourceName: '',
    Keywords: [],
    Subject: '',
    RequestDetails: '',
    Attachments: [],
    CreatedBy: 0,
    UpdatedBy: null,
    CreatedDateTime: new Date(),
    UpdatedDateTime: '',
    Status: 0,
    Action: ''
  };
  legalFormGroup: FormGroup = this._formBuilder.group({
    ReferenceNumber: [this.legalRequestModel && this.legalRequestModel.ReferenceNumber],
    LegalID: [this.legalRequestModel && this.legalRequestModel.LegalID || 0],
    SourceOU: [this.legalRequestModel && this.legalRequestModel.SourceOU || ''],
    SourceName: [this.legalRequestModel && this.legalRequestModel.SourceName || ''],
    Status: [this.legalRequestModel && this.legalRequestModel.Status || ''],
    Keywords: [this.legalRequestModel && this.legalRequestModel.Keywords || []],
    Subject: [this.legalRequestModel && this.legalRequestModel.Subject || ''],
    RequestDetails: [this.legalRequestModel && this.legalRequestModel.RequestDetails || ''],
    Comments: [this.legalRequestModel && this.legalRequestModel.Comments || ''],
    Attachments:[],
    CreatedDateTime: [this.legalRequestModel && this.legalRequestModel.CreatedDateTime || new Date()]
  });

  img_file: any;
  message: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  attachments: any = [];
  attachements :any =[];
  submitBtn = false;
  isrequired = false;
  reopenBtn = false;
  assingBtn = false;
  assignToMeBtn = false;
  closeBtnShow = false;
  viewBtnShow = false;
  returnForInfoBtn = false;
  isResubmit = false;
  legalRequestComments: Array<any> = [];
  userApproverList: any;
  userDOAList: any;
  isLegalDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 16;
  isLegalDepartmentTeamUserID = this.currentUser.OrgUnitID == 16 && !this.currentUser.IsOrgHead;
  isApiLoading: boolean = false;
  statusList: any[] = [];
  isKeywordAvailable: boolean = false;
  LabelControl :boolean =false;
  currentKeyword: string = '';
  empProfileImg: string = 'assets/home/user_male.png';
  attachmentDownloadUrl = environment.AttachmentDownloadUrl;
  commentSubscriber: any;
  hideKeywordField: boolean;

  constructor(private changeDetector: ChangeDetectorRef,
    public common: CommonService,
    public router: Router,
    private route: ActivatedRoute,
    public datepipe: DatePipe,
    private legalService: LegalService,
    private modalService: BsModalService,
    private _formBuilder: FormBuilder,
    private commentSectionService: CommentSectionService,
    private endpoint: EndPointService) {
    route.url.subscribe(() => {
      this.screenStatus = route.snapshot.data.title;
    });
    this.currentUser.userProfileImg = 'assets/home/user_male.png';
  }



  async ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.common.breadscrumChange('Legal', 'Legal Request View', '');
        if (this.common.language != 'English') {
          this.common.breadscrumChange(this.arabic('legal'), this.arabic('legalrequestview'), '');
        }
        this.isApiLoading = true;
        this.loadData(this.requestId);
      }
    });
    if (this.mode == 'create') {
      this.common.breadscrumChange('Legal', 'Legal Request', '');
      if (this.common.language != 'English') {
        this.common.breadscrumChange(this.arabic('legal'), this.arabic('legalrequest'), '');
      }
      this.isApiLoading = true;
      this.initPage();
    }
    if (this.common.language != 'English') {
      this.screenStatus = this.arabic(this.screenStatus);
    }
    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });

    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if (newComment) {
        this.loadData(this.legalRequestModel.LegalID);
      }
    });
    this.legalFormGroup.controls['ReferenceNumber'].disable();
    this.legalFormGroup.controls['LegalID'].disable();
    this.legalFormGroup.controls['SourceOU'].disable();
    this.legalFormGroup.controls['SourceName'].disable();
    this.legalFormGroup.controls['CreatedDateTime'].disable();
    this.legalFormGroup.controls['Status'].disable();
    if (this.currentUser.AttachmentGuid && this.currentUser.AttachmentName) {
      this.empProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.currentUser.AttachmentName + '&guid=' + this.currentUser.AttachmentGuid;
    }
  }

  ngAfterViewInit() { }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  async loadData(id) {
    this.legalService.getLegalRequestById(id, this.currentUser.id).subscribe((legalRequestRes: LegalRequest) => {
      this.legalRequestModel = legalRequestRes;
      this.attachements =this.legalRequestModel.Attachments;
      this.setFormData(this.legalRequestModel);
      this.isApiLoading = false;
    }, (err: any) => {
      this.initPage();
    });
  }

  setData(data) {
    this.legalRequestModel = data;
  }

  closemodal() {
    this.modalService.hide(1);
    setTimeout(function () { location.reload(); }, 1000);
  }


  initPage() {
    this.legalRequestModel.SourceOU = this.currentUser.DepartmentID;
    this.legalRequestModel.SourceName = this.currentUser.UserID;
    this.legalRequestModel.CreatedBy = this.currentUser.id;
    this.legalRequestModel.CreatedDateTime = new Date().toJSON();
    this.legalRequestModel.UpdatedDateTime = '';
    this.legalRequestModel.Comments = '';
    this.legalRequestModel.Status = 0;
    this.isApiLoading = false;
    this.buttonControl();
    this.setFormData(this.legalRequestModel);
  }

  Attachments(event:any)
  {
    let attachmetmodel =[];
    attachmetmodel=this.attachements;
    var files = event.target.files;
    if (files.length > 0) {
      let that = this;
      this.uploadProcess = true;
      this.common.postAttachment(files).subscribe((event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          this.attachmentInput.nativeElement.value = "";
          for (var i = 0; i < event.body.FileName.length; i++) {
            attachmetmodel.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'MemoID': '' });
          }
          this.attachements =attachmetmodel;
        }
      });
    }
  }
  deleteAttachment(index) {
    this.attachements.splice(index, 1);
    this.attachmentInput.nativeElement.value = "";
  }

  buttonControl() {
    if (this.mode == 'create') {
      if(this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID)
      {
        this.hideKeywordField = true;
        this.LabelControl =  true;
      }
      else
      {
        this.LabelControl=false;
        this.hideKeywordField = false; //hide label field for non legal users
        this.legalFormGroup.controls['Keywords'].disable();

      }
      this.isrequired =true;
      this.submitBtn = true;
    } else if (this.mode == 'view') {
      if(this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID)
      { this.hideKeywordField = true; }
      else{  this.hideKeywordField = false; }   //hide label field for non legal users
      this.submitBtn = false;
      this.assingBtn = false;
      this.assignToMeBtn = false;
      this.returnForInfoBtn = false;
      this.closeBtnShow = false;
      this.reopenBtn = false;
      this.isResubmit = false;
      if (this.legalRequestModel && (this.legalRequestModel.LegalID > 0)) {
        this.changeDetector.detectChanges();
        this.legalFormGroup.disable();
        if(this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID)
        {
        this.legalFormGroup.controls['Keywords'].enable();
        }
        if ((this.legalRequestModel.Status == 102||this.legalRequestModel.Status == 103) && (!this.legalRequestModel.AssigneeID || (this.legalRequestModel.AssigneeID.length <= 0))) {
          if (this.isLegalDepartmentHeadUserID) {
            this.assingBtn = true;
          } else if (this.isLegalDepartmentTeamUserID) {
            this.assignToMeBtn = true;
          }

          if (this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID) {
            if(this.legalRequestModel.Status != 102&&this.legalRequestModel.Status != 103)
            this.returnForInfoBtn = true;
            if (this.currentUser.allowLegalEdit && (this.currentUser.id != this.legalRequestModel.CreatedBy)) {
              this.changeDetector.detectChanges();
              this.legalFormGroup.controls['Keywords'].enable();
            }
          }
        }
        if (this.legalRequestModel.Status == 103 && this.legalRequestModel.AssigneeID&& (this.legalRequestModel.AssigneeID.length > 0) && (this.legalRequestModel.AssigneeID.findIndex(aid => { return aid.AssigneeId !== this.currentUser.id })) > -1)
          {
            if (this.isLegalDepartmentHeadUserID) {
              this.assingBtn = true;
            } else if (this.isLegalDepartmentTeamUserID) {
              this.assignToMeBtn = true;
            }
          }
        if ((this.legalRequestModel.Status != 104) && (this.legalRequestModel.Status != 105) &&(this.legalRequestModel.Status != 102)&& ((this.legalRequestModel.AssigneeID && (this.legalRequestModel.AssigneeID.length > 0) && (this.legalRequestModel.AssigneeID.findIndex(aid => { return aid.AssigneeId === this.currentUser.id }) > -1)))) {
          this.closeBtnShow = true;
          if (this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID) {
            this.returnForInfoBtn = true;
            if (this.currentUser.allowLegalEdit && (this.currentUser.id != this.legalRequestModel.CreatedBy)) {
              this.changeDetector.detectChanges();
              this.legalFormGroup.controls['Keywords'].enable();
            }
          }
        }
        if (this.legalRequestModel.Status == 105) {
          this.legalFormGroup.controls.Keywords.disable();
          this.reopenBtn = true;
        }
        if (this.legalRequestModel.Status == 104 && this.legalRequestModel.CreatedBy == this.currentUser.id) {
          this.isrequired=true;
          this.submitBtn = true;
          this.isResubmit = true;
          this.changeDetector.detectChanges();
          this.legalFormGroup.enable();
          this.legalFormGroup.controls['ReferenceNumber'].disable();
          this.legalFormGroup.controls['LegalID'].disable();
          this.legalFormGroup.controls['SourceOU'].disable();
          this.legalFormGroup.controls['SourceName'].disable();
          this.legalFormGroup.controls['Status'].disable();
          this.legalFormGroup.controls['CreatedDateTime'].disable();
        }
        this.changeDetector.detectChanges();
        this.legalFormGroup.controls['Comments'].enable();
        if (!this.legalRequestModel.Status) {
          this.changeDetector.detectChanges();
          this.legalFormGroup.disable();
        }
        if(this.legalRequestModel.Status == 104 &&(!this.isLegalDepartmentHeadUserID && !this.isLegalDepartmentTeamUserID))
        {
          this.legalFormGroup.controls['Keywords'].disable();
        }
      }
    }
  }



  popup(status: any) {
    this.bsModalRef = this.modalService.show(ModalComponent);
    this.bsModalRef.content.status = status;
  }

  formatPatch(val, comment, approver?: any) {
    var data = [{
      "value": val,
      "path": 'Action',
      "op": "Replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "Replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "Replace"
    },
    {
      "value": comment,
      "path": "Comments",
      "op": "Replace"
    }];
    return data;
  }

  setFormData(legalRequestData?: LegalRequest) {
    this.legalFormGroup = this._formBuilder.group({
      ReferenceNumber: [legalRequestData && legalRequestData.ReferenceNumber || 0],
      LegalID: [legalRequestData && legalRequestData.LegalID || 0],
      SourceOU: [legalRequestData && this.currentUser.departmentID || ''],
      SourceName: [legalRequestData && this.currentUser.UserID || ''],
      Status: [legalRequestData && legalRequestData.Status && this.loadStatusListAndSetStatus(this.statusList) || ''],
      Keywords: [legalRequestData && legalRequestData.Keywords || [], [Validators.required]],
      Subject: [legalRequestData && legalRequestData.Subject || '', [Validators.required]],
      RequestDetails: [legalRequestData && legalRequestData.RequestDetails || '', [Validators.required]],
      Attachments:[],
      Comments: [''],
      CreatedDateTime: [legalRequestData && legalRequestData.CreatedDateTime && new DatePipe(navigator.language).transform(new Date(legalRequestData.CreatedDateTime), 'dd/MM/yyyy') || new DatePipe(navigator.language).transform(new Date(), 'dd/MM/yyyy')],
    });
    this.LegalRequests=legalRequestData;
    if (legalRequestData && legalRequestData.LegalID > 0) {
      this.legalRequestComments = [];
      this.legalRequestComments = this.setLegalRequestComments(legalRequestData.LegalCommunicationHistory);
      if (legalRequestData.Keywords && legalRequestData.Keywords.length > 0) {
        let resKeywords: any[] = [];
        legalRequestData.Keywords.forEach((keyObj) => {
          if (keyObj.Keywords) {
            resKeywords.push({ display: keyObj.Keywords, value: keyObj.Keywords });
          }
        });
        this.legalFormGroup.patchValue({
          Keywords: resKeywords
        });
        this.isKeywordAvailable = true;
      } else {
        legalRequestData.Keywords = [];
        this.isKeywordAvailable = false;
      }
    }
    this.legalService.getLegalRequestById(0, 0).subscribe((data: any) => {
      this.department = data.OrganizationList;
      this.legalFormGroup.controls['SourceOU'].setValue(this.department.find(x=> x.OrganizationID == legalRequestData.SourceOU).OrganizationUnits);
      this.getSouceName(legalRequestData.SourceName,legalRequestData.SourceOU);
      this.loadStatusListAndSetStatus(data.M_LookupsList);
      this.buttonControl();
    }, err => {
      this.buttonControl();
    });
  }


  async getSouceName(UserID,DepID) {

    let params = [{
      "OrganizationID": DepID,
      "OrganizationUnits": "string"
    }];
    this.common.getUserList(params,0).subscribe((data: any) => {
      let Users = data;
      this.legalFormGroup.controls['SourceName'].setValue(Users.find(x=> x.UserID == UserID).EmployeeName.toString());
    });

  }

  userAction(actionType: string) {
    this.isApiLoading = true;
    let legalRequestBody: LegalRequest;
    if (actionType == 'submit') {
      legalRequestBody= {
        SourceOU: this.currentUser.DepartmentID,
        SourceName: this.currentUser.UserID,
        Status: this.legalFormGroup.value.Status,
        Subject: this.legalFormGroup.value.Subject,
        RequestDetails: this.legalFormGroup.value.RequestDetails,
        Attachments :this.attachements
      };
      if(actionType != 'submit')
      {
       legalRequestBody= {
          SourceOU: this.LegalRequests.SourceOU,
          SourceName: this.LegalRequests.SourceName,
          Status: this.LegalRequests.Status,
          Subject: this.LegalRequests.Subject,
          RequestDetails: this.LegalRequests.RequestDetails,
          Attachments :this.LegalRequests.attachements
        };
      }
      if (this.legalFormGroup.value.Keywords && this.legalFormGroup.value.Keywords.length > 0) {
        legalRequestBody.Keywords = [];
        this.legalFormGroup.value.Keywords.forEach((keyObj) => {
          if (keyObj) {
            legalRequestBody.Keywords.push({ Keywords: keyObj.display });
          }
        });
      }

      else {
        legalRequestBody.Keywords = [];
      }
      if (this.currentKeyword && this.currentKeyword.trim() != '') {
        legalRequestBody.Keywords.push({ Keywords: this.currentKeyword });
      }
      if(!this.isLegalDepartmentHeadUserID && !this.isLegalDepartmentTeamUserID)
      {
        legalRequestBody.Keywords= this.LegalRequests.Keywords;
      }
      if (this.legalRequestModel.LegalID) {
        legalRequestBody.LegalID = this.legalRequestModel.LegalID;
        legalRequestBody.UpdatedBy = this.currentUser.id;
        legalRequestBody.UpdatedDateTime = new Date();
        if(actionType == 'submit')
        legalRequestBody.Action = 'Resubmit';
        legalRequestBody.Comments = this.legalFormGroup.value.Comments;
        this.message = 'Legal Request Resubmitted Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('legalrequpdatemsg');
        }
        this.legalService.resubmitLegalRequest(legalRequestBody).subscribe((legalRes: any) => {
          if (legalRes.LegalID && actionType == 'submit') {
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(r => {
              newSubscriber.unsubscribe();
              if (this.common.language == 'English')
                this.router.navigate(['en/app/legal/dashboard']);
              else
                this.router.navigate(['ar/app/legal/dashboard']);
            });
          }
        });
      } else {
        legalRequestBody.CreatedBy = this.currentUser.id;
        legalRequestBody.CreatedDateTime = new Date();
        legalRequestBody.Action = 'Submit';
        legalRequestBody.Comments = this.legalFormGroup.value.Comments;
        this.message = 'Legal Request Submitted Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('legalreqcreatemsg');
        }
        this.legalService.addLegalRequest(legalRequestBody).subscribe((legalRes: any) => {
          if (legalRes.LegalID) {
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(r => {
              newSubscriber.unsubscribe();
              if (this.common.language == 'English')
                this.router.navigate(['en/app/legal/dashboard']);
              else
                this.router.navigate(['ar/app/legal/dashboard']);
            });
          }
        });
      }

    } if(actionType != 'submit') {
      let toSendData: any;

      // Legal team save keywords for approval action except reopen
      if (actionType != 'reopen') {
        let Keywords = [];
        if (this.legalFormGroup.value.Keywords && this.legalFormGroup.value.Keywords.length > 0) {
          Keywords = [];
          this.legalFormGroup.value.Keywords.forEach((keyObj) => {
            if (keyObj) {
              Keywords.push({ Keywords: keyObj.display });
            }
          });
        }
        
        else {
          Keywords = [];
        }
        if (this.currentKeyword && this.currentKeyword.trim() != '') {
          Keywords.push({ Keywords: this.currentKeyword });
        }
      
        if(!this.isLegalDepartmentHeadUserID && !this.isLegalDepartmentTeamUserID)
        {
          Keywords= this.LegalRequests.Keywords;
        }

        this.legalService.saveLegalKeywords(Keywords,this.legalRequestModel.LegalID, this.currentUser.id).subscribe((saveLegalKeywords: any) => {
          if(saveLegalKeywords){
          console.log("keywords saved "+saveLegalKeywords);}
        });
      }
      if (actionType == 'assign') {
       this.onAssignTo();
      }
      if (actionType == 'assigntome') {
        this.message = 'Legal Request Assigned Successfully';
        toSendData = this.formatPatch('AssignToMe', this.legalFormGroup.value.Comments);
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('legalassignedmsg');
        }
      }
      if (actionType == 'close') {
        this.message = 'Legal Request Closed Successfully';
        toSendData = this.formatPatch('Close', this.legalFormGroup.value.Comments);
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('legalclosedmsg');
        }
      }
      if (actionType == 'reopen') {
        this.message = 'Legal Request Reopened Successfully';
        toSendData = this.formatPatch('ReOpen', this.legalFormGroup.value.Comments);
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('legalreopenedmsg');
        }
      }
      if (actionType == 'redirect') {
        this.message = 'Legal Request Returned For Info Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('legalreqreturnedmsg');
        }
        toSendData = this.formatPatch('ReturnForInfo', this.legalFormGroup.value.Comments);
      }
      this.legalService.updateLegalRequestStatus(this.legalRequestModel.LegalID, toSendData).subscribe((legalRequestPatchRes: any) => {
        if (legalRequestPatchRes.LegalID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(r => {
            newSubscriber.unsubscribe();
            if (this.common.language == 'English')
              this.router.navigate(['en/app/legal/dashboard']);
            else
              this.router.navigate(['ar/app/legal/dashboard']);
          });
        }
      });
    }

  }

  sendMessage() {
    if (this.legalFormGroup.value.Comments && (this.legalFormGroup.value.Comments.trim() != '')) {
      this.isApiLoading = true;
      let chatData: any = {
        Message: this.legalFormGroup.value.Comments,
        ParentCommunicationID: 0,
        CreatedBy: this.currentUser.id,
        CreatedDateTime: new Date(),
        LegalID: this.legalRequestModel.LegalID
      };
      this.commentSectionService.sendComment('Legal', chatData).subscribe((chatRes: any) => {
        this.commentSectionService.newCommentCreated(true);
        this.legalFormGroup.patchValue({
          Comments: ''
        });
        this.isApiLoading = false;
      });
    }
  }

  private setLegalRequestComments(commentSectionArr: any, parentCommunicationID?: any) {
    let recursiveCommentsArr = [];
    if (!parentCommunicationID) {
      parentCommunicationID = 0;
    }
    commentSectionArr.forEach((commObj: any) => {
      if (commObj.ParentCommunicationID == parentCommunicationID) {
        let replies: any = this.setLegalRequestComments(commentSectionArr, commObj.CommunicationID);
        if (replies.length > 0) {
          replies.forEach(repObj => {
            repObj.hideReply = true;
          });
          commObj.Replies = replies;
        }
        commObj.UserProfileImg = 'assets/home/user_male.png';
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  onAssignTo() {
    let initialState = {
      id: this.requestId,
      ApiString: "/Legal",
      message: "Legal Request Assigned Successfully",
      Title: "Assign To",
      redirectUrl: 'app/legal/dashboard',
      ApiTitleString: "Assign To",
      comments: this.legalFormGroup.value['Comments']
    };
    if(this.common.currentLang == 'ar'){
      initialState.message = this.arabic('legalassignedmsg');
      initialState.ApiTitleString=this.arabic('assignto');
    }
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(r => {
      newSubscriber.unsubscribe();
      this.isApiLoading = false;
    });
    this.isApiLoading = true;
  }

  loadStatusListAndSetStatus(LookupsList?: any[]) {
    let tosetStatusIndex;
    if (LookupsList) {
      this.statusList = LookupsList;
      if (this.legalRequestModel && this.legalRequestModel.Status) {
        tosetStatusIndex = this.statusList.findIndex(filObj => this.legalRequestModel.Status == filObj.LookupsID);
      }
      if (tosetStatusIndex > -1) {
        this.legalFormGroup.patchValue({
          Status: this.statusList[tosetStatusIndex].DisplayName
        });
        return this.statusList[tosetStatusIndex].DisplayName;
      }
    }
    return false;
  }

  onTextChange(e) {
    if (e.trim() != '') {
      let keyw = this.legalFormGroup.value.Keywords;
      keyw.push({ display: e, value: e });
      this.legalFormGroup.patchValue({
        Keywords: keyw
      });
      this.isKeywordAvailable = true;
      this.currentKeyword = '';
      //this.saveLegalLabelKeywords();
    } else {
      if (!this.legalFormGroup.value['Keywords'] || this.legalFormGroup.value['Keywords'].length <= 0) {
        this.isKeywordAvailable = false;
        this.currentKeyword = '';
      }
    }
  }

  labelTextChange(e) {
    // let labelInputRef:any = this.labelInput;
    if (e.trim() != '') {
      this.currentKeyword = e;
      if (this.legalFormGroup.controls['Subject'].valid && this.legalFormGroup.controls['RequestDetails'].valid) {
        this.isKeywordAvailable = true;
      }
    } else {
      if (!this.legalFormGroup.value['Keywords'] || this.legalFormGroup.value['Keywords'].length <= 0) {
        this.isKeywordAvailable = false;
        this.currentKeyword = '';
      }
    }
  }

  addLabelText() {
    this.saveLegalLabelKeywords();
  }

  removeLabelText() {
    let labelInputRef: any = this.labelInput;
    if (!labelInputRef.items || labelInputRef.items.length <= 0) {
      this.isKeywordAvailable = false;
      this.currentKeyword = '';
    }
    this.saveLegalLabelKeywords();
  }

  saveLegalLabelKeywords() {
    if ((this.legalRequestModel.Status == 103 && (!this.legalRequestModel.AssigneeID || (this.legalRequestModel.AssigneeID.length <= 0))) ||
      ((this.legalRequestModel.Status != 105) && ((this.legalRequestModel.AssigneeID &&
        (this.legalRequestModel.AssigneeID.length > 0) && (this.legalRequestModel.AssigneeID.findIndex(aid => { return aid.AssigneeId === this.currentUser.id }) > -1))
        || this.isLegalDepartmentHeadUserID))) {
      this.isApiLoading = true;
      let legalDepartmentKeywords = [];
      if ((this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID) && (this.currentUser.allowLegalEdit && (this.currentUser.id != this.legalRequestModel.CreatedBy))) {
        if (this.legalFormGroup.value.Keywords && this.legalFormGroup.value.Keywords.length > 0) {
          legalDepartmentKeywords = [];
          this.legalFormGroup.value.Keywords.forEach((keyObj) => {
            if (keyObj.value) {
              legalDepartmentKeywords.push({ Keywords: keyObj.display });
            }
          });
        } else {
          legalDepartmentKeywords = [];
        }
        if (this.currentKeyword && this.currentKeyword.trim() != '') {
          legalDepartmentKeywords.push({ Keywords: this.currentKeyword });
        }
      }
      this.legalService.saveLegalKeywords(legalDepartmentKeywords, this.legalRequestModel.LegalID, this.currentUser.id).subscribe((keywordRes) => {
        this.isApiLoading = false;
      }, err => {
        this.isApiLoading = false;
      });
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

}
