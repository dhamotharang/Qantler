import { Component, OnInit, Input, OnDestroy, ViewChild, TemplateRef, ChangeDetectorRef } from '@angular/core';
import { BsModalRef, BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { DiwanIdentityRequest } from '../../model/diwan-identity-request/diwan-identity-request.model';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { CommonService } from 'src/app/common.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { DiwanIdentityService } from '../../service/diwan-identity.service';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-diwan-identity-request-form',
  templateUrl: './diwan-identity-request-form.component.html',
  styleUrls: ['./diwan-identity-request-form.component.scss']
})
export class DiwanIdentityRequestFormComponent implements OnInit,OnDestroy {
  @Input() mode: string;
  @Input() requestId: number;
  bsModalRef: BsModalRef;
  screenStatus = 'Create';
  department = [];
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  diwanIdentityRequestModel:DiwanIdentityRequest = {
    ReferenceNumber:'',
    DiwanIdentityID:0,
    SourceOU: '',
    SourceName: '',
    ApproverID: null,
    ApproverDepartmentID: null,
    PurposeofUse: '',
    CreatedBy: 0,
    UpdatedBy: null,
    CreatedDateTime: new Date(),
    UpdatedDateTime: null,
    Status: 0,
    Action:'',
    Comments:''
  };

  diwanIdentityFormGroup:FormGroup = this._formBuilder.group({
    ReferenceNumber:[this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.ReferenceNumber],
    DiwanIdentityID:[this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.DiwanIdentityID || 0],
    SourceOU: [this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.SourceOU || ''],
    SourceName: [this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.SourceName || ''],
    ApproverID: [this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.ApproverID || null],
    ApproverDepartmentID: [this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.ApproverDepartmentID || null],
    PurposeofUse: [this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.PurposeofUse || ''],
    Comments:[this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.Comments || ''],
    CreatedDateTime:[this.diwanIdentityRequestModel && this.diwanIdentityRequestModel.CreatedDateTime || new Date()],
  });
  message:any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  required = false;
  submitBtn = false;
  approveBtn = false;
  rejectBtn = false;
  escalateBtn = false;
  returnForInfoBtn = false;
  assingBtn = false;
  assignToMeBtn = false;
  closeBtnShow = false;
  viewBtnShow = false;
  diwanIdentityRequestComments:Array<any> = [];
  userApproverList:any;
  isApiLoading:boolean = false;
  uploadProcess: boolean = false;
  uploadPercentage: number;
  isStartEndDiff:boolean = false;
  diwanIdentityReqModal = {
    modalTitle:'',
    message:''
  };
  isProtocolDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 4;
  isProtocolDepartmentTeamUserID = this.currentUser.OrgUnitID == 4 && !this.currentUser.IsOrgHead;
  isMediaDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 17;
  isMediaDepartmentTeamUserID = this.currentUser.OrgUnitID == 17 && !this.currentUser.IsOrgHead;
  popupMsg: any;
  isFirstApprover:boolean = false;
  approverDepartment:any = [];
  userProfileImg: string;
  commentSubscriber: any;

  constructor(private changeDetector:ChangeDetectorRef,
    public common: CommonService,
    public router: Router,
    private route: ActivatedRoute,
    public datepipe: DatePipe,
    private modalService: BsModalService,
    private diwanIdentityService:DiwanIdentityService,
    private _formBuilder:FormBuilder,
    private commentSectionService:CommentSectionService,
    private utilsService:UtilsService,
    private endpoint:EndPointService) {
    route.url.subscribe(() => {
      this.screenStatus = route.snapshot.data.title;
    });
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.currentUser.userProfileImg = 'assets/home/user_male.png';
  }



  async ngOnInit(){
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.route.params.subscribe(params => {
      if(params['id']){
        // this.common.breadscrumChange('Diwan Identity Request', this.arabic('View'), '');
        this.isApiLoading = true;
        this.loadData(this.requestId);
      }
    });
    if (this.currentUser.AttachmentGuid && this.currentUser.AttachmentName) {
      this.userProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.currentUser.AttachmentName + '&guid=' + this.currentUser.AttachmentGuid;
    }else{
      this.userProfileImg = 'assets/home/user_male.png';
    }
    if(this.mode == 'create'){
      this.common.breadscrumChange('Media', 'Request for Diwan Identity', '');
      if(this.common.currentLang != 'en'){
        this.common.breadscrumChange(this.arabic('media'), this.arabic('requestfordiwanidentity'), '');
      }
      this.isApiLoading = true;
      this.initPage();
    }else{
      this.common.breadscrumChange('Media', 'Request for Diwan Identity', '');
      if(this.common.currentLang != 'en'){
        this.common.breadscrumChange(this.arabic('media'), this.arabic('requestfordiwanidentity'), '');
      }
      this.diwanIdentityFormGroup.disable();
    }
    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });

    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        this.loadData(this.requestId);
      }
    });
    // this.diwanIdentityService.getDiwanIdentityRequestById(0,0).subscribe((data: any) => {
    //   this.department = data.OrganizationList;
    //   this.approverDepartment  = data.M_ApproverDepartmentList;
    // });
    this.diwanIdentityFormGroup.controls['ReferenceNumber'].disable();
    this.diwanIdentityFormGroup.controls['DiwanIdentityID'].disable();
    this.diwanIdentityFormGroup.controls['SourceOU'].disable();
    this.diwanIdentityFormGroup.controls['SourceName'].disable();
    this.diwanIdentityFormGroup.controls['CreatedDateTime'].disable();
  }

  onChangeApproverDepartment() {
    this.diwanIdentityFormGroup.patchValue({
      ApproverID:null
    });
    this.getApproverUserList(+this.diwanIdentityFormGroup.value.ApproverDepartmentID);
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  async getApproverUserList(id) {
    let params = [{
      "OrganizationID": id,
      "OrganizationUnits": "string"
    }];
    this.common.getmemoUserList(params,this.diwanIdentityRequestModel.CreatedBy).subscribe((data: any) => {
      this.userApproverList = data;
    });
  }

  loadData(id) {
    if(id){
      this.diwanIdentityService.getDiwanIdentityRequestById(id,this.currentUser.id).subscribe((diwanIdentityRequestRes:DiwanIdentityRequest) => {
        this.diwanIdentityRequestModel = diwanIdentityRequestRes;
        this.setFormData(this.diwanIdentityRequestModel);
        this.isApiLoading = false;
      },(err:any) => {
        this.initPage();
      });
    }
  }

  setData(data) {
    this.diwanIdentityRequestModel = data;
  }

  closemodal(){
    this.modalService.hide(1);
    setTimeout(function(){ location.reload(); }, 1000);
  }


  initPage() {
    this.diwanIdentityRequestModel.DiwanIdentityID = 0;
    this.diwanIdentityRequestModel.SourceOU = '';
    this.diwanIdentityRequestModel.SourceName = '';
    this.diwanIdentityRequestModel.ApproverID = null;
    this.diwanIdentityRequestModel.ApproverDepartmentID = this.currentUser.DepartmentID;
    this.diwanIdentityRequestModel.PurposeofUse = '';
    this.diwanIdentityRequestModel.CreatedBy = this.currentUser.id;
    this.diwanIdentityRequestModel.CreatedDateTime = new Date();
    this.diwanIdentityRequestModel.UpdatedDateTime = null;
    this.isApiLoading = false;
    // this.diwanIdentityRequestModel.Status = 0;
    this.buttonControl();
    this.setFormData(this.diwanIdentityRequestModel);
  }


  buttonControl() {
    if (this.mode == 'create') {
      this.required=true;
      this.submitBtn = true;
      this.changeDetector.detectChanges();
      this.diwanIdentityFormGroup.controls['Comments'].enable();
    } else if (this.mode == 'view') {
      this.changeDetector.detectChanges();
      this.diwanIdentityFormGroup.disable();
      this.submitBtn = false;
      this.approveBtn = false;
      this.rejectBtn = false;
      this.escalateBtn = false;
      this.returnForInfoBtn = false;
      this.assingBtn = false;
      this.assignToMeBtn = false;
      this.closeBtnShow = false;
      this.isFirstApprover = false;
      if(this.diwanIdentityRequestModel && (this.diwanIdentityRequestModel.DiwanIdentityID > 0)){
        if(this.diwanIdentityRequestModel.Status == 100 && (this.diwanIdentityRequestModel.CurrentApprover.findIndex(ca => ca.ApproverId == this.currentUser.id) > -1)){
          this.approveBtn = true;
          this.rejectBtn = true;
          this.escalateBtn = true;
          this.returnForInfoBtn = true;
          this.isFirstApprover = true;
          this.changeDetector.detectChanges();
          this.diwanIdentityFormGroup.controls['Comments'].enable();
        }
        if(this.diwanIdentityRequestModel.Status == 96 && (!this.diwanIdentityRequestModel.AssigneeId || (this.diwanIdentityRequestModel.AssigneeId.length <= 0))){
          if(this.isMediaDepartmentHeadUserID){
            this.assingBtn = true;
            this.changeDetector.detectChanges();
            this.diwanIdentityFormGroup.disable();
            this.diwanIdentityFormGroup.controls['Comments'].enable();
          }else if(this.isMediaDepartmentTeamUserID){
            this.assignToMeBtn = true;
            this.changeDetector.detectChanges();
            this.diwanIdentityFormGroup.disable();
            this.diwanIdentityFormGroup.controls['Comments'].enable();
          }else{
            this.changeDetector.detectChanges();
            this.diwanIdentityFormGroup.disable();
          }

        }
        if((this.diwanIdentityRequestModel.AssigneeId && (this.diwanIdentityRequestModel.AssigneeId.findIndex(asgn => asgn.AssigneeId  === this.currentUser.id) > -1) && this.diwanIdentityRequestModel.Status != 98)){
          this.closeBtnShow = true;
          this.changeDetector.detectChanges();
          this.diwanIdentityFormGroup.disable();
          this.diwanIdentityFormGroup.controls['Comments'].enable();
        }
        if((this.diwanIdentityRequestModel.AssigneeId && (this.diwanIdentityRequestModel.AssigneeId.findIndex(asgn => asgn.AssigneeId  !== this.currentUser.id) > -1) && this.diwanIdentityRequestModel.Status != 98)){
          if(this.isMediaDepartmentHeadUserID){
            this.assingBtn = true;
            this.changeDetector.detectChanges();
            this.diwanIdentityFormGroup.disable();
            this.diwanIdentityFormGroup.controls['Comments'].enable();
          }else if(this.isMediaDepartmentTeamUserID){
            this.assignToMeBtn = true;
            this.changeDetector.detectChanges();
            this.diwanIdentityFormGroup.disable();
            this.diwanIdentityFormGroup.controls['Comments'].enable();
          }else{
            this.changeDetector.detectChanges();
            this.diwanIdentityFormGroup.disable();
          }
        }
        if(this.diwanIdentityRequestModel.Status == 97 && this.diwanIdentityRequestModel.CreatedBy == this.currentUser.id){
          this.submitBtn = true;
          this.required=true;
          this.changeDetector.detectChanges();
          this.diwanIdentityFormGroup.enable();
          this.diwanIdentityFormGroup.controls['ReferenceNumber'].disable();
          this.diwanIdentityFormGroup.controls['DiwanIdentityID'].disable();
          this.diwanIdentityFormGroup.controls['SourceOU'].disable();
          this.diwanIdentityFormGroup.controls['SourceName'].disable();
          this.diwanIdentityFormGroup.controls['CreatedDateTime'].disable();
          this.diwanIdentityFormGroup.controls['Comments'].enable();
        }
      }
    }
  }

  formatPatch(val,comment,approver?:any) {
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

  setFormData(diwanIdentityRequestData?:DiwanIdentityRequest){
    this.diwanIdentityFormGroup = this._formBuilder.group({
      ReferenceNumber:[diwanIdentityRequestData && diwanIdentityRequestData.ReferenceNumber || 0],
      DiwanIdentityID:[diwanIdentityRequestData && diwanIdentityRequestData.DiwanIdentityID || 0],
      SourceOU: [diwanIdentityRequestData && diwanIdentityRequestData.SourceOU || this.currentUser.department],
      SourceName: [diwanIdentityRequestData && diwanIdentityRequestData.SourceName || this.currentUser.username],
      ApproverID: [diwanIdentityRequestData && diwanIdentityRequestData.ApproverID || null],
      ApproverDepartmentID: [diwanIdentityRequestData && diwanIdentityRequestData.ApproverDepartmentID || null],
      PurposeofUse: [diwanIdentityRequestData && diwanIdentityRequestData.PurposeofUse || '', [Validators.required]],
      Comments:[diwanIdentityRequestData && diwanIdentityRequestData.Comments || null],
      CreatedDateTime:[diwanIdentityRequestData && diwanIdentityRequestData.CreatedDateTime && new DatePipe(navigator.language).transform(new Date(diwanIdentityRequestData.CreatedDateTime),'dd/MM/yyyy') || new DatePipe(navigator.language).transform(new Date(),'dd/MM/yyyy')],
    });

    if(diwanIdentityRequestData && diwanIdentityRequestData.DiwanIdentityID){
      this.diwanIdentityRequestComments = [];
      this.diwanIdentityRequestComments = this.setDiwanIdentityRequestComments(diwanIdentityRequestData.DiwanIdentityCommunicationHistory);
    }
    this.getApproverUserList(+diwanIdentityRequestData.ApproverDepartmentID);
    if(diwanIdentityRequestData.SourceName != '' && diwanIdentityRequestData.SourceOU !='')
    {
    // this.getSouceName(diwanIdentityRequestData.SourceName,diwanIdentityRequestData.SourceOU);
    let params = [{
      "OrganizationID": diwanIdentityRequestData.SourceOU,
      "OrganizationUnits": "string"
    }];
    this.common.getUserList(params,0).subscribe((data: any) => {
      let Users = data;
      this.diwanIdentityFormGroup.controls['SourceName'].setValue(Users.find(x=> x.UserID == diwanIdentityRequestData.SourceName).EmployeeName.toString());
      this.buttonControl();
    });
    this.diwanIdentityService.getDiwanIdentityRequestById(0,0).subscribe((data: any) => {
      this.department = data.OrganizationList;
      this.approverDepartment  = data.M_ApproverDepartmentList;
      if(diwanIdentityRequestData.SourceOU){
        let TempSourceOU = this.department.filter(x=> x.OrganizationID == diwanIdentityRequestData.SourceOU)
        this.diwanIdentityFormGroup.controls['SourceOU'].setValue(TempSourceOU[0].OrganizationUnits);
      }
    });
    }else{
      this.diwanIdentityService.getDiwanIdentityRequestById(0,0).subscribe((data: any) => {
        this.department = data.OrganizationList;
        this.approverDepartment  = data.M_ApproverDepartmentList;
      });
      this.buttonControl();
    }
  }

  async getSouceName(UserID,DepID) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.diwanIdentityFormGroup.controls['SourceName'].setValue(Users.find(x=> x.UserID == UserID).EmployeeName.toString());
     });

   }

  userAction(actionType:string){
    this.isApiLoading = true;
    if(actionType == 'submit'){
      let diwanIdentityRequestBody:DiwanIdentityRequest = {
        SourceOU:this.currentUser.DepartmentID,
        SourceName:this.currentUser.UserID,
        PurposeofUse:this.diwanIdentityFormGroup.value.PurposeofUse,
        ApproverID:this.diwanIdentityFormGroup.value.ApproverID,
        ApproverDepartmentID:this.diwanIdentityFormGroup.value.ApproverDepartmentID,
      };
      if(this.diwanIdentityRequestModel.DiwanIdentityID){
        diwanIdentityRequestBody.DiwanIdentityID = this.diwanIdentityRequestModel.DiwanIdentityID;
        diwanIdentityRequestBody.UpdatedBy = this.currentUser.id;
        diwanIdentityRequestBody.UpdatedDateTime = new Date();
        diwanIdentityRequestBody.Action = 'Resubmit';
        diwanIdentityRequestBody.Comments = this.diwanIdentityFormGroup.value.Comments;
        this.message = (this.common.currentLang == 'en' ? 'Diwan Identity Request Resubmitted Successfully' : this.arabic('diwanidentitysubmitmsg'));
        this.diwanIdentityService.resubmitDiwanIdentityRequest(diwanIdentityRequestBody).subscribe((diwanIdentityRes:any) => {
          if (diwanIdentityRes.DiwanIdentityID) {
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(r=>{
              newSubscriber.unsubscribe();
              this.router.navigate(['/app/media/protocol-home-page']);
            });
            // this.loadData(this.requestId);
          }
        });
      }else{
          diwanIdentityRequestBody.CreatedBy = this.currentUser.id;
          diwanIdentityRequestBody.CreatedDateTime = new Date();
          diwanIdentityRequestBody.Action = 'Submit';
          diwanIdentityRequestBody.Comments = this.diwanIdentityFormGroup.value.Comments;
          if (this.common.currentLang == 'en') {
            this.message = 'Diwan Identity Request Submitted Successfully';
          } else {
            this.message = this.arabic('diwanidentitysubmitmsg');
          }
          this.diwanIdentityService.addDiwanIdentityRequest(diwanIdentityRequestBody).subscribe((diwanIdentityRes:any) => {
            if (diwanIdentityRes.DiwanIdentityID) {
              this.diwanIdentityRequestModel.DiwanIdentityID = diwanIdentityRes.DiwanIdentityID;
              this.bsModalRef = this.modalService.show(SuccessComponent);
              this.bsModalRef.content.message = this.message;
              // this.sendMessage();
              let newSubscriber = this.modalService.onHide.subscribe(r=>{
                newSubscriber.unsubscribe();
                this.router.navigate(['/app/media/protocol-home-page']);
              });
              this.diwanIdentityFormGroup.reset();
            }
          });
      }
    }else{
      let toSendData:any;
      if(!this.diwanIdentityFormGroup.valid || ((this.diwanIdentityFormGroup.value.Comments) && (this.diwanIdentityFormGroup.value.Comments.trim() != ''))){
        if(actionType == 'reject'){
          if (this.common.currentLang == 'en') {
            this.message = 'Diwan Identity Request Rejected Successfully';
          } else {
            this.message = this.arabic('diwanidentityrejectmsg');
          }
          toSendData = this.formatPatch('Reject',this.diwanIdentityFormGroup.value.Comments);
        }
        if(actionType == 'escalate'){
          this.message = (this.common.currentLang == 'en' ? 'Diwan Identity Request Escalated Successfully' : this.arabic('diwanidentityescalatemsg'));
          this.utilsService.currentRedirectUrl = '/app/media/protocol-home-page';
          let initialState = {
            id: this.diwanIdentityRequestModel.DiwanIdentityID,
            ApiString: "/DiwanIdentity",
            // message: "Diwan Identity Request Escalated Successfully",
            message: this.message,
            Title: 'Escalate',
            comments:this.diwanIdentityFormGroup.value.Comments,
            redirectPath: 'app/media/protocol-home-page',
            isFirstApprover : this.isFirstApprover
          };
          this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
          this.isApiLoading = false;
        }
        if(actionType == 'redirect'){
          this.message = 'Diwan Identity Request Returned For Info Successfully';
          if (this.common.currentLang == 'ar') {
            this.message = this.arabic('diwanidentityreturnedmsg');
          }
          toSendData = this.formatPatch('ReturnForInfo',this.diwanIdentityFormGroup.value.Comments);
        }
      }
      if(actionType == 'assign'){
        this.onAssignTo();
      }
      if(actionType == 'approve'){
        this.message = 'Diwan Identity Request Approved Successfully';
        if (this.common.currentLang == 'ar') {
          this.message = this.arabic('diwanidentityapprovedmsg');
        }
        toSendData = this.formatPatch('Approve',this.diwanIdentityFormGroup.value.Comments);
      }
      if(actionType == 'assigntome'){
        this.message = 'Diwan Identity Request Assigned Successfully';
        if (this.common.currentLang == 'ar') {
          this.message = this.arabic('diwanidentityassignedmsg');
        }
        toSendData = this.formatPatch('AssignToMe',this.diwanIdentityFormGroup.value.Comments);
      }
      if(actionType == 'close'){
        this.message = 'Diwan Identity Request Closed Successfully';
        if (this.common.currentLang == 'ar') {
          this.message = this.arabic('diwanidentityclosedmsg');
        }
        toSendData = this.formatPatch('Close',this.diwanIdentityFormGroup.value.Comments);
      }
      if(actionType != 'assign' &&  actionType != 'escalate'){
        this.diwanIdentityService.updateDiwanIdentityRequestStatus(this.diwanIdentityRequestModel.DiwanIdentityID,toSendData).subscribe((diwanIdentityRequestPatchRes:any)=>{
          if (diwanIdentityRequestPatchRes.DiwanIdentityID) {
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(r=>{
              newSubscriber.unsubscribe();
              this.router.navigate(['/app/media/protocol-home-page']);
            });
            this.loadData(this.requestId);
          }
        });
      }
    }

  }

  sendMessage(){
    if(this.diwanIdentityFormGroup.value.Comments && (this.diwanIdentityFormGroup.value.Comments.trim() != '')){
      this.isApiLoading = true;
      let chatData:any = {
        Message:this.diwanIdentityFormGroup.value.Comments,
        ParentCommunicationID:0,
        CreatedBy:this.currentUser.id,
        CreatedDateTime:new Date(),
        DiwanIdentityID:this.diwanIdentityRequestModel.DiwanIdentityID
      };
      this.commentSectionService.sendComment('DiwanIdentity',chatData).subscribe((chatRes:any)=>{
        this.commentSectionService.newCommentCreated(true);
        this.diwanIdentityFormGroup.patchValue({
          Comments:''
        });
      });
    }
  }

  private setDiwanIdentityRequestComments(commentSectionArr:any,parentCommunicationID?:any){
    let recursiveCommentsArr = [];
    if(!parentCommunicationID){
      parentCommunicationID = 0;
    }
    commentSectionArr.forEach((commObj:any)=>{
      if(commObj.ParentCommunicationID == parentCommunicationID){
        let replies:any = this.setDiwanIdentityRequestComments(commentSectionArr,commObj.CommunicationID);
        if(replies.length > 0){
          replies.forEach(repObj => {
            repObj.hideReply = true;
          });
          commObj.Replies = replies;
        }
        // commObj.UserProfileImg = 'assets/home/user_male.png';
        //if(!commObj.Photo){
         // commObj.Photo = 'assets/home/user_male.png';
        //}
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  onAssignTo() {
    if (this.common.currentLang == 'en') {
      this.popupMsg = "Diwan Identity Request Assigned Successfully";
    } else {
      this.popupMsg = this.arabic('diwanidentityassignedmsg');
    }
    let initialState = {
      id: this.diwanIdentityRequestModel.DiwanIdentityID,
      ApiString: "/DiwanIdentity",
      message: this.popupMsg,
      ApiTitleString: "Assign To",
      redirectUrl: '/app/media/protocol-home-page',
      comments:this.diwanIdentityFormGroup.value.Comments,
    };
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    this.isApiLoading = false;
  }
  arabic(word) {
    word = word.replace(/ +/g, "").toLowerCase();
    return this.common.arabic.words[word];
  }
}
