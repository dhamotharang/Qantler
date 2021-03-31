import { Component, OnInit, OnDestroy, ViewChild, TemplateRef, ChangeDetectorRef, Input, ElementRef, Renderer2, Inject } from '@angular/core';
import { BsModalRef, BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { DatePipe, DOCUMENT } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { LeaveService } from '../../service/leave.service';
import { ModalComponent } from 'src/app/modal/modalcomponent/modal.component';
import { LeaveRequest } from '../../model/leave-request/leave-request.model';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Attachment } from '../../model/attachment/attachment.model';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { CommunicationHistory } from '../../model/communication-history/communication-history.model';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { HttpEventType } from '@angular/common/http';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';
import { EndPointService } from 'src/app/api/endpoint.service';
import { environment } from 'src/environments/environment';
import { AdminService } from 'src/app/admin/service/admin/admin.service';

@Component({
  selector: 'app-leave-request-form',
  templateUrl: './leave-request-form.component.html',
  styleUrls: ['./leave-request-form.component.scss']
})
export class LeaveRequestFormComponent implements OnInit,OnDestroy {
  @Input() mode: string;
  @Input() requestId: number;
  @ViewChild('template') template : TemplateRef<any>;
  @ViewChild('fileInput') fileInput :ElementRef;
  bsModalRef: BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  screenStatus = 'Create';
  leaveType = [];
  department = [];
  approverDepartment = [];
  attachmentFiles = [];
  dropdownSettings: any;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  leaveRequestModel:LeaveRequest = {
    LeaveID:0,
    SourceOU: '',
    SourceName: '',
    ApproverID: null,
    ApproverDepartmentID: null,
    DOANameID: null,
    DOADepartmentID: null,
    Reason: '',
    Attachments: [],
    CreatedBy: 0,
    UpdatedBy: null,
    CreatedDateTime: new Date(),
    UpdatedDateTime: '',
    Status: 0,
    StartDate:'',
    EndDate:'',
    BalanceLeave:'',
    LeaveType:'',
    Action:'',
    LeaveTypeOther:null
  };

  leaveFormGroup:FormGroup = this._formBuilder.group({
    ReferenceNumber:[this.leaveRequestModel && this.leaveRequestModel.ReferenceNumber],
    LeaveID:[this.leaveRequestModel && this.leaveRequestModel.LeaveID || 0],
    SourceOU: [this.leaveRequestModel && this.leaveRequestModel.SourceOU || ''],
    SourceName: [this.leaveRequestModel && this.leaveRequestModel.SourceName || ''],
    ApproverID: [this.leaveRequestModel && this.leaveRequestModel.ApproverID || null, [Validators.required]],
    ApproverDepartmentID: [this.leaveRequestModel && this.leaveRequestModel.ApproverDepartmentID || null, [Validators.required]],
    DOANameID: [this.leaveRequestModel && this.leaveRequestModel.DOANameID || null, [Validators.required]],
    DOADepartmentID: [this.leaveRequestModel && this.leaveRequestModel.DOADepartmentID || null,[Validators.required]],
    Reason: [this.leaveRequestModel && this.leaveRequestModel.Reason || ''],
    StartDate:[this.leaveRequestModel && this.leaveRequestModel.StartDate || ''],
    EndDate:[this.leaveRequestModel && this.leaveRequestModel.EndDate || ''],
    BalanceLeave:[this.leaveRequestModel && this.leaveRequestModel.BalanceLeave || ''],
    LeaveType:[this.leaveRequestModel && this.leaveRequestModel.LeaveType || null, [Validators.required]],
    Comments:[this.leaveRequestModel && this.leaveRequestModel.Comments || ''],
    CreatedDateTime:[this.leaveRequestModel && this.leaveRequestModel.CreatedDateTime || new Date()],
    Attachments:[],
    LeaveTypeOther:[this.leaveRequestModel && this.leaveRequestModel.LeaveTypeOther || null]
  });

  img_file: any;
  message : any;
  attachments: any = [];
  userDestination: any;
  userReceiver: any;
  commonMes: any;
  DestinationDepartmentId: any[];
  submitBtn = false;
  approveBtn = false;
  rejectBtn = false;
  escalateBtn = false;
  returnForInfoBtn = false;
  actionDateTime = new Date('10-02-2019');
  actionBy = 'Mohammed';
  assingBtn = false;
  assignToMeBtn = false;
  closeBtnShow = false;
  viewBtnShow = false;
  leaveRequestComments:Array<any> = [];
  userApproverList:Array<any> = [];
  userDOAList:any;
  attachementModel = new FormControl();
  isApiLoading:boolean = false;
  popupvar :boolean=false;
  uploadProcess: boolean = false;
  uploadPercentage: number;
  isStartEndDiff:boolean = false;
  // lang: string;
  leaveReqModal = {
    modalTitle:'',
    message:''
  };
  isHRDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 9;
  isHRDepartmentTeamUserID = this.currentUser.OrgUnitID == 9 && !this.currentUser.IsOrgHead;
  isFirstApprover:boolean = false;
  downloadUrl;
  lang: any;
  isOtherLeaveType: boolean=false;
  otherLeaveTypes: any=[];
  BalanceLeave: any;
  noLeave: boolean=false;
  commentSubscriber: any;
  empProfileImg: string = 'assets/home/user_male.png';

  constructor(private changeDetector:ChangeDetectorRef,
    public common: CommonService,
    public router: Router,
    private route: ActivatedRoute,
    public datepipe: DatePipe,
    private modalService: BsModalService,
    private leaveService:LeaveService,
    private _formBuilder:FormBuilder,
    private commentSectionService:CommentSectionService,
    private utilsService:UtilsService,
    private enpointService:EndPointService,
    private adminservice: AdminService,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document, ) {
    route.url.subscribe(() => {
      this.screenStatus = route.snapshot.data.title;
    });
    // this.lang = this.common.currentLang;
    this.currentUser.userProfileImg = 'assets/home/user_male.png';
    this.downloadUrl = this.enpointService.fileDownloadUrl;
  }



  async ngOnInit(){
    this.lang = this.common.currentLang;
    this.getLeaveTypes();
    if (this.common.currentLang == 'en') {
      this.leaveType = ['Vacation','Sick Leave'];
    } else {
      this.leaveType = [this.arabic('vacation'), this.arabic('sickleave')];
    }

    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.route.params.subscribe(params => {
      if(params['id']){
        if (this.common.currentLang == 'en') {
          this.common.breadscrumChange('HR', 'Leave Request', this.requestId);
        } else {
          this.common.breadscrumChange(this.arabic('humanresource'), this.arabic('Leave Requests'),this.requestId);
        }
        this.isApiLoading = true;
        this.loadData(this.requestId);
      }
    });
    if(this.mode == 'create'){
      if (this.common.currentLang == 'en') {
        this.common.breadscrumChange('HR', 'Leave Request', 'Creation');
      } else {
        this.common.breadscrumChange(this.arabic('humanresource'), this.arabic('Leave Requests'),this.arabic('Creation'));
      }
      this.isApiLoading = true;
      this.initPage();
    }else{
      if (this.common.currentLang == 'en') {
        this.common.breadscrumChange('HR', 'Leave Request', this.requestId);
      } else {
        this.common.breadscrumChange(this.arabic('humanresource'), this.arabic('Leave Requests'),this.requestId);
      }
      this.leaveFormGroup.disable();
    }

    this.actionDateTime.setHours(9);
    this.actionDateTime.setMinutes(10);
    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });

    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        this.loadData(this.leaveRequestModel.LeaveID);
      }
    });
    this.leaveService.getLeaveRequestById(0,0).subscribe((data: any) => {
      var calendar_id = environment.calendar_id;
      this.department = data.OrganizationList.filter(res => calendar_id != res.OrganizationID);
      this.approverDepartment = data.M_ApproverDepartmentList;
      // this.leaveFormGroup.patchValue({
      //   BalanceLeave:data.BalanceLeave
      // });
    });
    this.leaveFormGroup.controls['ReferenceNumber'].disable();
    this.leaveFormGroup.controls['LeaveID'].disable();
    this.leaveFormGroup.controls['SourceOU'].disable();
    this.leaveFormGroup.controls['SourceName'].disable();
    this.leaveFormGroup.controls['BalanceLeave'].disable();
    this.leaveFormGroup.controls['CreatedDateTime'].disable();
    if (this.currentUser.AttachmentGuid && this.currentUser.AttachmentName) {
      this.empProfileImg = this.enpointService.fileDownloadUrl + '?filename=' + this.currentUser.AttachmentName + '&guid=' + this.currentUser.AttachmentGuid;
    }
  }

  ngAfterViewInit() {}

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  onChangeApproverDepartment() {
    this.leaveFormGroup.patchValue({
      ApproverID:null
    });
    this.getApproverUserList(+this.leaveFormGroup.value.ApproverDepartmentID);
  }

  onChangeDOADepartment(){
    this.leaveFormGroup.patchValue({
      DOANameID:null
    });
    this.getDOAUserList(+this.leaveFormGroup.value.DOADepartmentID);
  }

  async getApproverUserList(id) {
    let params = [{
      "OrganizationID": id,
      "OrganizationUnits": "string"
    }];

    if(this.mode == 'create') {
      this.common.getmemoUserList(params,this.currentUser.id).subscribe((data: any) => {
      this.userApproverList = data;
      });
    } else if(this.mode == 'view') {
      this.common.getmemoUserList(params, 0).subscribe((data: any) => {
      this.userApproverList = data;
      });
    }
  }

  async getDOAUserList(id){
    let params = [{
      "OrganizationID": id,
      "OrganizationUnits": "string"
    }];
    if (this.mode == 'create') {
      this.common.getUserList(params, this.currentUser.id).subscribe((data: any) => {
        this.userDOAList = data;
      });
    } else if (this.mode == 'view') {
      this.common.getUserList(params, 0).subscribe((data: any) => {
        this.userDOAList = data;
      });
    }
  }

  async loadData(id) {
    if (id) {
      this.leaveService.getLeaveRequestById(id,this.currentUser.id).subscribe((leaveRequestRes:LeaveRequest) => {
        this.leaveRequestModel = leaveRequestRes;
        this.setFormData(this.leaveRequestModel);
        this.isApiLoading = false;
      },(err:any) => {
        this.initPage();
      });
    }
  }

  setData(data) {
    this.leaveRequestModel = data;
  }

  closemodal(){
    this.modalService.hide(1);
    if(!this.noLeave) {
      setTimeout(function(){ location.reload(); }, 1000);
    }
    this.renderer.removeClass(this.document.body, 'modal-open');
  }


  initPage() {
    this.leaveRequestModel.LeaveID = 0;
    this.leaveRequestModel.SourceOU = this.currentUser.departmentID;
    this.leaveRequestModel.SourceName = this.currentUser.UserID;
    this.leaveRequestModel.ApproverID = null;
    this.leaveRequestModel.ApproverDepartmentID = this.currentUser.DepartmentID;
    this.leaveRequestModel.DOANameID = null;
    this.leaveRequestModel.DOADepartmentID = this.currentUser.DepartmentID;
    this.leaveRequestModel.Reason = '';
    this.leaveRequestModel.Attachments = [];
    this.leaveRequestModel.CreatedBy = this.currentUser.id;
    this.leaveRequestModel.CreatedDateTime = new Date();
    this.leaveRequestModel.UpdatedDateTime = '';
    this.isApiLoading = false;
    // this.leaveRequestModel.Status = 0;
    this.buttonControl();
    this.leaveService.getLeaveRequestById(0,0).subscribe((data: any) => {
      var calendar_id = environment.calendar_id;
      this.department = data.OrganizationList.filter(res => calendar_id != res.OrganizationID);
    });
    this.setFormData(this.leaveRequestModel);
  }

  leaveAttachments(event) {
    this.img_file = event.target.files;
    if(this.img_file.length > 0){
      this.isApiLoading = true;
      this.uploadProcess = true;
      this.leaveService.uploadLeaveRequestAttachment(this.img_file).subscribe((attachementRes:any) => {
        this.isApiLoading = false;
        if (attachementRes.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(attachementRes.loaded / attachementRes.total) * 100;
        } else if (attachementRes.type === HttpEventType.Response) {
          this.isApiLoading = false;
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          for (var i = 0; i < attachementRes.body.FileName.length; i++) {
            if(this.leaveRequestModel && this.leaveRequestModel.LeaveID){
              this.attachments.push({'AttachmentGuid':attachementRes.body.Guid,'AttachmentsName':attachementRes.body.FileName[i],'LeaveID':this.leaveRequestModel.LeaveID,currentUpload:true});
            }else{
              this.attachments.push({'AttachmentGuid':attachementRes.body.Guid,'AttachmentsName':attachementRes.body.FileName[i],'LeaveID':0,currentUpload:true});
            }
          }
          this.leaveRequestModel.Attachments = this.attachments;
          this.fileInput.nativeElement.value = "";
        }
      });
    }
  }


  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.fileInput.nativeElement.value = "";
  }



  buttonControl() {
    if (this.mode == 'create') {
      this.submitBtn = true;
    } else if (this.mode == 'view') {
      this.changeDetector.detectChanges();
      this.leaveFormGroup.disable();
      this.submitBtn = false;
      this.approveBtn = false;
      this.rejectBtn = false;
      this.escalateBtn = false;
      this.returnForInfoBtn = false;
      this.assingBtn = false;
      this.assignToMeBtn = false;
      this.closeBtnShow = false;
      this.isFirstApprover = false;
      if(this.leaveRequestModel && (this.leaveRequestModel.LeaveID > 0)){
        if(this.leaveRequestModel.Status == 7 && (this.leaveRequestModel.CurrentApprover.findIndex(ca => ca.ApproverId == this.currentUser.id) > -1)){
          this.approveBtn = true;
          this.rejectBtn = true;
          this.escalateBtn = true;
          this.returnForInfoBtn = true;
          this.isFirstApprover = true;
        }
        if(this.leaveRequestModel.Status == 8 && !this.leaveRequestModel.HRManagerUserID && (this.currentUser.OrgUnitID == 9 && this.currentUser.IsOrgHead)){
          this.approveBtn = true;
          this.rejectBtn = true;
          this.escalateBtn = false;
          this.returnForInfoBtn = true;
        }
        if(this.leaveRequestModel.Status == 8 && !this.leaveRequestModel.AssigneeID && this.leaveRequestModel.HRManagerUserID){
          if(this.isHRDepartmentHeadUserID){
            this.assingBtn = true;
          }else if(this.isHRDepartmentTeamUserID){
            this.assignToMeBtn = true;
          }
          this.changeDetector.detectChanges();
          this.leaveFormGroup.disable();
        }
        if((this.leaveRequestModel.AssigneeID && this.leaveRequestModel.AssigneeID === this.currentUser.id && this.leaveRequestModel.Status !=11)){
          this.closeBtnShow = true;
          this.changeDetector.detectChanges();
          this.leaveFormGroup.disable();
        }
        if((this.leaveRequestModel.AssigneeID && this.leaveRequestModel.AssigneeID !== this.currentUser.id && this.leaveRequestModel.Status !=11)){
          if(this.isHRDepartmentHeadUserID){
            this.assingBtn = true;
          }else if(this.isHRDepartmentTeamUserID){
            this.assignToMeBtn = true;
          }
          this.changeDetector.detectChanges();
          this.leaveFormGroup.disable();
        }
        if(this.leaveRequestModel.Status == 10 && this.leaveRequestModel.CreatedBy == this.currentUser.id){
          this.submitBtn = true;
          this.changeDetector.detectChanges();
          this.leaveFormGroup.enable();
          this.leaveFormGroup.controls['ReferenceNumber'].disable();
          this.leaveFormGroup.controls['LeaveID'].disable();
          this.leaveFormGroup.controls['SourceOU'].disable();
          this.leaveFormGroup.controls['SourceName'].disable();
          this.leaveFormGroup.controls['BalanceLeave'].disable();
          this.leaveFormGroup.controls['CreatedDateTime'].disable();
        }
        this.changeDetector.detectChanges();
        this.leaveFormGroup.controls['Comments'].enable();
      }
    }
  }



  popup(status: any) {
    this.bsModalRef = this.modalService.show(ModalComponent);
    this.bsModalRef.content.status = status;
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
   if(val == 'Escalate'){
    data.push({
      "value": approver,
      "path": 'ApproverId',
      "op": "Replace"
    });
   }
    return data;
  }

  // hisLog(status){
  //   if(status == 'Submit' || status == 'Reject' || status == 'Redirect'){
  //     return status+'ed';
  //   }else{
  //     return status+'d';
  //   }
  // }

  setFormData(leaveRequestData?:LeaveRequest){
    this.leaveFormGroup = this._formBuilder.group({
      ReferenceNumber:[leaveRequestData && leaveRequestData.ReferenceNumber || 0],
      LeaveID:[leaveRequestData && leaveRequestData.LeaveID || 0],
      SourceOU: [leaveRequestData && leaveRequestData.SourceOU || this.currentUser.department],
      SourceName: [leaveRequestData && leaveRequestData.SourceName || this.currentUser.username],
      ApproverID: [leaveRequestData && leaveRequestData.ApproverID || null, [Validators.required]],
      ApproverDepartmentID: [leaveRequestData && leaveRequestData.ApproverDepartmentID || null, [Validators.required]],
      DOANameID: [leaveRequestData && leaveRequestData.DOANameID || null, [Validators.required]],
      DOADepartmentID: [leaveRequestData && leaveRequestData.DOADepartmentID || null,[Validators.required]],
      Reason: [leaveRequestData && leaveRequestData.Reason || ''],
      StartDate:[leaveRequestData && leaveRequestData.StartDate && new Date(leaveRequestData.StartDate) || '',[Validators.required]],
      EndDate:[leaveRequestData && leaveRequestData.EndDate && new Date(leaveRequestData.EndDate) || '',[Validators.required]],
      BalanceLeave:[leaveRequestData && leaveRequestData.BalanceLeave || ''],
      LeaveType:[leaveRequestData && leaveRequestData.LeaveType || null, [Validators.required]],
      Comments:[leaveRequestData && leaveRequestData.Comments || null],
      CreatedDateTime:[leaveRequestData && leaveRequestData.CreatedDateTime && new DatePipe(navigator.language).transform(new Date(leaveRequestData.CreatedDateTime),'dd/MM/yyyy') || new Date()],
      Attachments:[],
      LeaveTypeOther:[leaveRequestData && leaveRequestData.LeaveTypeOther || null],

    });
    this.leaveFormGroup.controls["Attachments"] = this._formBuilder.array(leaveRequestData && leaveRequestData.Attachments || new Array<Attachment>());
    if(leaveRequestData && leaveRequestData.LeaveID){
      this.attachments = [];
      if (leaveRequestData.Attachments)
      this.attachments = leaveRequestData.Attachments;
      this.leaveRequestComments = [];
      this.leaveRequestComments = this.setLeaveRequestComments(leaveRequestData.LeaveCommunicationHistory);
    }
    this.BalanceLeave = leaveRequestData.BalanceLeave;
    if(this.mode == 'create'){
      this.leaveService.getUserProfileForBalanceLeave(leaveRequestData.CreatedBy).subscribe((userRes:any) => {
        if(userRes){
          leaveRequestData.BalanceLeave = userRes.BalanceLeave;
          this.BalanceLeave = userRes.BalanceLeave;
          this.leaveFormGroup.patchValue({
            BalanceLeave:userRes.BalanceLeave
          });
        }
      });
    }
    if(leaveRequestData.LeaveType == "1") {
      this.isOtherLeaveType = true;
    }
    this.getSouceName(leaveRequestData.SourceName,leaveRequestData.SourceOU);
    // this.leaveFormGroup.controls['LeaveType'].setValue(this.leaveType[leaveRequestData.LeaveType]);
    this.getApproverUserList(+this.leaveFormGroup.value.ApproverDepartmentID);
    this.getDOAUserList(+this.leaveFormGroup.value.DOADepartmentID);
    this.buttonControl();
    this.checkStartEndDateDiff();
  }

  async getSouceName(UserID,DepID) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
      let Users = data;
      if(this.mode == 'view'){
        this.leaveFormGroup.controls['SourceOU'].setValue(
          this.department.find(x=> x.OrganizationID == DepID) ? 
          this.department.find(x=> x.OrganizationID == DepID).OrganizationUnits : '');
        for(let i=0; i<Users.length;i++)
        {
          if(Users[i].UserID==parseInt(UserID))
          this.leaveFormGroup.controls['SourceName'].setValue( Users.find(x=> x.UserID == UserID).EmployeeName.toString());
        }
      }
     });

   }

  userAction(actionType:string,allowSubmit?:boolean){
    this.isApiLoading = true;
    if(actionType == 'submit'){
      let leaveRequestBody:LeaveRequest = {
        SourceOU:this.currentUser.DepartmentID,
        SourceName:this.currentUser.UserID,
        Reason:this.leaveFormGroup.value.Reason,
        LeaveType:this.leaveFormGroup.value.LeaveType,
        ApproverID:this.leaveFormGroup.value.ApproverID,
        ApproverDepartmentID:this.leaveFormGroup.value.ApproverDepartmentID,
        DOANameID:this.leaveFormGroup.value.DOANameID,
        DOADepartmentID:this.leaveFormGroup.value.DOADepartmentID,
        StartDate:this.leaveFormGroup.value.StartDate,
        EndDate:this.leaveFormGroup.value.EndDate,
        BalanceLeave: this.BalanceLeave,
        Attachments:this.attachments,
        LeaveTypeOther: this.leaveFormGroup.value.LeaveTypeOther,
      };
      if(this.leaveRequestModel.LeaveID){
        leaveRequestBody.LeaveID = this.leaveRequestModel.LeaveID;
        leaveRequestBody.UpdatedBy = this.currentUser.id;
        leaveRequestBody.UpdatedDateTime = new Date();
        leaveRequestBody.Action = 'Resubmit';
        this.message = 'Leave Request Resubmitted Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('leaveupdatemsg');
        }
        if(!this.checklLeaveBalance()) {
          return false;
        }
        this.leaveService.resubmitLeaveRequest(leaveRequestBody).subscribe((leaveRes:any) => {
          if (leaveRes.LeaveID) {
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(r=>{
              newSubscriber.unsubscribe();
              this.router.navigate(['app/hr/dashboard']);
            });
            this.loadData(this.requestId);
          }
        });
      }else{
          leaveRequestBody.CreatedBy = this.currentUser.id;
          leaveRequestBody.CreatedDateTime = new Date();
          leaveRequestBody.Action = 'Submit';
          this.message = 'Leave Request Submitted Successfully';
          if(this.common.currentLang == 'ar'){
            this.message = this.arabic('leavecreatedmsg');
          }
          if(!this.checklLeaveBalance()) {
            return false;
          }
          if(allowSubmit || this.checkPastDate(allowSubmit)){
            this.leaveService.addLeaveRequest(leaveRequestBody).subscribe((leaveRes:any) => {
              if (leaveRes.LeaveID) {
                this.bsModalRef = this.modalService.show(SuccessComponent);
                this.bsModalRef.content.message = this.message;
                let newSubscriber = this.modalService.onHide.subscribe(r=>{
                  newSubscriber.unsubscribe();
                  this.router.navigate(['app/hr/dashboard']);
                });
                this.leaveFormGroup.reset();
              }
            });
          }
      }
    }else{
      let toSendData:any;
      if(!this.leaveFormGroup.valid || ((this.leaveFormGroup.value.Comments) && (this.leaveFormGroup.value.Comments.trim() != ''))){
        if(actionType == 'reject'){
          this.message = 'Leave Request Rejected Successfully';
          if(this.common.currentLang == 'ar'){
            this.message = this.arabic('leaverejectmsg');
          }
          toSendData = this.formatPatch('Reject',this.leaveFormGroup.value.Comments);
        }
        if(actionType == 'escalate'){
          this.utilsService.currentRedirectUrl = '/app/hr/dashbaord';
          let initialState = {
            id: this.leaveRequestModel.LeaveID,
            ApiString: "/Leave",
            message: "Leave Request Escalated Successfully",
            Title: "Escalate",
            comments:this.leaveFormGroup.value.Comments,
            redirectPath: 'app/hr/dashboard',
            isFirstApprover:this.isFirstApprover
          };
          if (this.common.currentLang == 'ar') {
            initialState.message = this.arabic('leaveescalatemsg');
          }
          this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
          this.isApiLoading = false;
          // let orgHeadIndex = this.userApproverList.findIndex(ual => ((ual.OrgUnitID == this.currentUser.OrgUnitID) && ual.IsOrgHead));
          // toSendData = this.formatPatch('Escalate',this.leaveFormGroup.value.Comments,this.userApproverList[orgHeadIndex].UserID);
        }
        if(actionType == 'redirect'){
          this.message = 'Leave Request Returned For Info Successfully';
          if(this.common.currentLang == 'ar'){
            this.message = this.arabic('leavereturnedmsg');
          }
          toSendData = this.formatPatch('ReturnForInfo',this.leaveFormGroup.value.Comments);

        }
      }
      if(actionType == 'assign'){
        this.onAssignTo();
      }
      if(actionType == 'approve'){
        this.message = 'Leave Request Approved Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('leaveapprovedmsg');
        }
        toSendData = this.formatPatch('Approve',this.leaveFormGroup.value.Comments);

      }
      if(actionType == 'assigntome'){
        this.message = 'Leave Request Assigned Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('leaveassignmsg');
        }
        toSendData = this.formatPatch('AssignToMe',this.leaveFormGroup.value.Comments);

      }
      if(actionType == 'close'){
        this.message = 'Leave Request Closed Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic('leavecloseemsg');
        }
        toSendData = this.formatPatch('Close',this.leaveFormGroup.value.Comments);

      }
      if(actionType != 'assign' &&  actionType != 'escalate'){
        this.leaveService.updateLeaveRequestStatus(this.leaveRequestModel.LeaveID,toSendData).subscribe((leaveRequestPatchRes:any)=>{
          if (leaveRequestPatchRes.LeaveID) {

            if(this.leaveRequestModel.Status == 8 && actionType == 'approve'){
              this.popupvar=true;
              this.onAssignTo();
            }
            if(!this.popupvar){
              this.bsModalRef = this.modalService.show(SuccessComponent);
              this.bsModalRef.content.message = this.message;
              let newSubscriber = this.modalService.onHide.subscribe(r=>{
                newSubscriber.unsubscribe();
                if(!this.popupvar){
                  this.router.navigate(['app/hr/dashboard']);
                }
              });
            }
            this.loadData(this.requestId);         
          }
        });
      }
    }

  }

  sendMessage(){
    if(this.leaveFormGroup.value.Comments && (this.leaveFormGroup.value.Comments.trim() != '')){
      this.isApiLoading = true;
      let chatData:any = {
        Message:this.leaveFormGroup.value.Comments,
        ParentCommunicationID:0,
        CreatedBy:this.currentUser.id,
        CreatedDateTime:new Date(),
        LeaveID:this.leaveRequestModel.LeaveID
      };
      this.commentSectionService.sendComment('Leave',chatData).subscribe((chatRes:any)=>{
        this.commentSectionService.newCommentCreated(true);
        this.leaveFormGroup.patchValue({
          Comments:''
        });
        this.isApiLoading = false;
      });
    }
  }

  private setLeaveRequestComments(commentSectionArr:any,parentCommunicationID?:any){
    let recursiveCommentsArr = [];
    if(!parentCommunicationID){
      parentCommunicationID = 0;
    }
    commentSectionArr.forEach((commObj:any)=>{
      if(commObj.ParentCommunicationID == parentCommunicationID){
        let replies:any = this.setLeaveRequestComments(commentSectionArr,commObj.CommunicationID);
        if(replies.length > 0){
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
    let initialState;
    if(this.popupvar) {
      initialState = {
        id: this.leaveRequestModel.LeaveID,
        ApiString: "/Leave",
        message: "Leave Request Approved and Assigned Successfully",
        ApiTitleString: "Assign To",
        redirectUrl: '/app/hr/dashboard',
        comments:this.leaveFormGroup.value['Comments']
      };
      if(this.common.currentLang == 'ar'){
        initialState.message = this.arabic('leaveapprovedandassignsuccessmsg');
      }
    }
    else {
      initialState = {
        id: this.leaveRequestModel.LeaveID,
        ApiString: "/Leave",
        message: "Leave Request Assigned Successfully",
        ApiTitleString: "Assign To",
        redirectUrl: '/app/hr/dashboard',
        comments:this.leaveFormGroup.value['Comments']
      };
      if(this.common.currentLang == 'ar'){
        initialState.message = this.arabic('leavereqassignmsg');
      }
    }

    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      // if(this.popupvar) {
      //   this.router.navigate(['app/hr/dashboard']);
      // }
    });
    this.isApiLoading = false;
  }

  checkStartEndDateDiff(){
    if(this.leaveFormGroup.value.StartDate && this.leaveFormGroup.value.EndDate){
      if(new Date(this.leaveFormGroup.value.StartDate).getTime() <= new Date(this.leaveFormGroup.value.EndDate).getTime()){
        this.isStartEndDiff = false;
        return false;
      }else{
        this.isStartEndDiff = true;
        return true;
      }
    }
  }

  checkPastDate(allowSubmit?:boolean){
    this.noLeave = false;
    if(!allowSubmit){
    if((this.leaveFormGroup.value.LeaveType && this.leaveFormGroup.value.LeaveType == 'Vacation') || (this.leaveFormGroup.value.LeaveType && this.leaveFormGroup.value.LeaveType == this.arabic('vacation'))){
        if((this.leaveFormGroup.value.StartDate &&  (new Date(this.leaveFormGroup.value.StartDate).getTime() < new Date().getTime())) || (this.leaveFormGroup.value.EndDate &&  (new Date(this.leaveFormGroup.value.EndDate).getTime() < new Date().getTime()))){
          this.leaveReqModal.modalTitle = 'Confirmation';
          this.leaveReqModal.message = "You are requesting for vacation on a past date. Are you sure you want to continue (Yes/No)?";
          if(this.common.currentLang == 'ar'){
            this.leaveReqModal.modalTitle = this.arabic('confirmation');
            this.leaveReqModal.message = this.arabic('confirmationleavereqpastdate');
          }
          this.bsModalRef = this.modalService.show(this.template);
          this.isApiLoading = false;
          return false;
        }
      }
    }
    return true;
  }

  pastDateYes(){
    this.bsModalRef.hide();
    this.userAction('submit',true);
  }

  pastDateNo(){
    this.leaveFormGroup.patchValue({
      StartDate:'',
      EndDate:''
    });
    this.bsModalRef.hide();
  }

  arabic(word) {
    word = word.replace(/ +/g, "").toLowerCase();
    return this.common.arabic.words[word];
  }
  getLeaveTypes() {
    const leaveType = 17
    this.adminservice.getLeaveTypes(this.currentUser.id, leaveType).subscribe((userRes:any) => {
      if(userRes){
        this.otherLeaveTypes = userRes;
      }
    });
  }

  checkLeaveType() {
    if(this.leaveFormGroup.value.LeaveType && this.leaveFormGroup.value.LeaveType == 1){
      this.isOtherLeaveType = true;
      if(this.mode == 'create'){
        this.leaveFormGroup.patchValue({
          LeaveTypeOther: null
        });
      }
      this.noLeave = false;
    }else {
      // this.checklLeaveBalance();
      this.isOtherLeaveType = false;
    }
  }

  checklLeaveBalance(){
    this.noLeave = true;
    let flag = true;
    var date1 = new Date(this.leaveFormGroup.value.StartDate);
    var date2 = new Date(this.leaveFormGroup.value.EndDate);
    // To calculate the time difference of two dates
    var Difference_In_Time = date2.getTime() - date1.getTime();
    // To calculate the no. of days between two dates
    var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);
      if(this.leaveFormGroup.value.LeaveType == 0 &&
        ((this.BalanceLeave == null || (this.BalanceLeave && this.BalanceLeave == 0)) ||
        Difference_In_Days > this.BalanceLeave)) {
        this.leaveReqModal.modalTitle = 'Confirmation';
        this.leaveReqModal.message = "Sorry, you don't have enough leave balance. Please contact your HR unit.";
        if(this.common.currentLang == 'ar'){
          this.leaveReqModal.modalTitle = this.arabic('confirmation');
          this.leaveReqModal.message = this.arabic('nobalanceleavedesc');
        }
        this.bsModalRef = this.modalService.show(this.template);
        this.isApiLoading = false;
        flag = false;
      }
    return flag;
  }

  checklLeaveBalanceValid(){
    let flag = true;
      if(this.leaveFormGroup.value.LeaveType == 0 && (this.BalanceLeave == null || (this.BalanceLeave && this.BalanceLeave == 0))) {
        flag = false;
      }
    return flag;
  }

  checklOtherLeaveValid(){
    let flag = true;
      if(this.leaveFormGroup.value.LeaveType == 1) {
        if(!this.leaveFormGroup.value.LeaveTypeOther) {
          flag = false;
        }else {
          flag = true;
        }
      }
    return flag;
  }
}
