import { ArabicDataService } from './../../../arabic-data.service';
import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MaintenanceService } from '../../service/maintenance.service';
import { Maintenance } from '../../model/maintenance.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { CommonService } from 'src/app/common.service';
import { UploadService } from 'src/app/shared/service/upload.service';
import { HttpEventType } from '@angular/common/http';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { Attachment } from '../../model/attachment.model';
import * as _ from 'lodash';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-maintenance-form',
  templateUrl: './maintenance-form.component.html',
  styleUrls: ['./maintenance-form.component.scss']
})
export class MaintenanceFormComponent implements OnInit,OnDestroy {
  @ViewChild('fileInput') fileInput: ElementRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  id: string;
  editMode: boolean = true;
  showCommentBox: boolean = false;
  refId: string = '';
  status: string = '';
  statusCode: Number;
  date: Date;
  sourceou: string = '';
  sourceName: string = '';
  Comments: string;
  priority: string;
  Reason: string;
  departmentList: Array<any> = [];
  requesterDepartment: any;
  requesterList: Array<any> = [];
  requester: any;
  approverDepartment: any;
  approverList: Array<any> = [];
  approver: any;
  currentApprover: any;
  subject: string;
  MaintenanceID: any = 0;
  RequestDetails: string;
  RequestComments: any;
  statusList: any;
  maintenance: Maintenance = new Maintenance();
  bsModalRef: BsModalRef;
  message: string = '';
  attachments: Array<Attachment> = [];
  valid: boolean = false;
  inProgress: boolean = false;
  OrgUnitID: Number;
  IsOrgHead: boolean = false;
  isAssigned: boolean = false;
  isAssignedToMe: boolean = false;
  isApprover: boolean = false;
  isCreatedByMe: boolean = false;
  AssigneeId: any;
  MaintenanceManagerUserID: any;
  // buttons to show
  showSubmitBtn: boolean = false;
  showResubmitBtn: boolean = false;
  showApproveBtn: boolean = false;
  showRejectBtn: boolean = false;
  showReturnBtn: boolean = false;
  showEscalateBtn: boolean = false;
  showAssignBtn: boolean = false;
  showAssignToMeBtn: boolean = false;
  showCloseBtn: boolean = false;
  lang: string;
  approverDepartmentList: any;
  isFirstApprover:boolean = false;

  empProfileImg: string = 'assets/home/user_male.png';
  userDetail: any;
  cur_user: string;
  commentSubscriber: any;

  constructor(
    private route: ActivatedRoute,
    public modalService: BsModalService,
    private maintenanceService: MaintenanceService,
    public upload: UploadService,
    public utils: UtilsService,
    public router: Router,
    private common: CommonService,
    public arabic: ArabicDataService,
    private commentSectionService:CommentSectionService,
    private endpoint: EndPointService
  ) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get("id");
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 0;
    if (this.id) {
      if (this.lang === 'en') {
        this.common.breadscrumChange('Maintenance', 'Maintenance Request View', '');
      } else {
        this.common.breadscrumChange(this.arabic.words.maintenance, this.arabic.words.maintenancerequestview, '');
      }
      this.editMode = false;
      this.loadMaintenance();
    } else {
      if (this.lang === 'en') {
        this.common.breadscrumChange('Maintenance', 'Create Maintenance Request', '');
      } else {
        this.common.breadscrumChange(this.arabic.words.maintenance, this.arabic.words.createMaintenanceRequest, '');
      }
      this.editMode = true;
      this.showSubmitBtn = true;
      this.showCommentBox = true;
      this.initForm();
      this.approverDepartment = this.currentUser.DepartmentID;
      this.requesterDepartment = this.currentUser.DepartmentID;
      this.requester = this.currentUser.id;
      this.onDepartmentSelect('requester', false);
      this.onDepartmentSelect('approver', false);
    }
    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        this.loadMaintenance();
      }
    });
    this.userDetail = JSON.parse(localStorage.getItem('User'));
    this.cur_user = this.userDetail.username;
    if (this.userDetail.AttachmentGuid && this.userDetail.AttachmentName) {
      this.empProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.userDetail.AttachmentName + '&guid=' + this.userDetail.AttachmentGuid;
    }
  }
  
  ngOnDestroy(){
    this.commentSubscriber.unsubscribe();
  }

  initForm() {
    this.sourceou = this.currentUser.username;
    this.sourceName = this.currentUser.department;
    this.maintenanceService.getById(0, this.currentUser.id)
      .subscribe((response: any) => {
        if (response != null) {
          this.departmentList = response.OrganizationList;
          this.approverDepartmentList = response.M_ApproverDepartmentList;
        }
      });
  }

  onRequesterChange() {
    if (this.approverDepartment) {
      this.onDepartmentSelect('approver', true);
    }
  }

  onDepartmentSelect(type: string, resetUsers: boolean) {
    debugger
    let id: number;
    let userId = this.editMode ? this.requester : 0;
    let params = [{
      "OrganizationID": id,
      "OrganizationUnits": ""
    }];
    switch (type) {
      case 'requester': 
        params[0].OrganizationID = this.requesterDepartment;
        this.common.getUserList(params, 0).subscribe((data: any) => {
          this.requesterList = data;
        });
        if(resetUsers) this.requester = '';
        break;
      case 'approver': 
        params[0].OrganizationID = this.approverDepartment;
        this.common.getmemoUserList(params, userId).subscribe((data: any) => {
          this.approverList = data;                        
        });
        if(resetUsers) this.approver = '';
        break;
    }
    this.validate();
  }

  handleFileUpload(event: any) {
    if (event.target.files.length > 0) {
      const files = event.target.files;
      this.upload.uploadAttachment(files)
        .subscribe((response: any) => {
          if (response.type === HttpEventType.Response) {
            for (let i = 0; i < response.body.FileName.length; i++) {
              const attachment = {
                AttachmentGuid: response.body.Guid,
                AttachmentsName: response.body.FileName[i],
                MaintenanceID: this.MaintenanceID
              };
              this.attachments.push(attachment);
            }
          }
          this.fileInput.nativeElement.value = '';
          this.validate();
          if (this.MaintenanceID && this.MaintenanceID != 0) {
            this.maintenanceService.updateAttachment(this.MaintenanceID, this.currentUser.id, this.attachments)
              .subscribe((updatedAttachment: any) => {
              });
          }
        });
    }
  }

  handleFileDownload(file: any) {
    this.upload.downloadAttachment(file)
      .subscribe((data) => {
        var url = window.URL.createObjectURL(data);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = file.AttachmentsName;
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
      });
  }

  deleteAttachment(index: any) {
    this.attachments.splice(index, 1);
    this.validate();
  }

  validate() {
    this.valid = true;
    if (this.utils.isEmptyString(this.subject)
      || this.utils.isEmptyString(this.approverDepartment)
      || this.utils.isEmptyString(this.approver)
      || this.utils.isEmptyString(this.RequestDetails)
      || this.utils.isEmptyString(this.priority)) {
      this.valid = false;
    }
  }

  isCommentEmpty() {
    if (this.utils.isEmptyString(this.Comments)) {
      return true;
    }
    return false;
  }

  loadMaintenance() {
    this.maintenanceService.getById(this.id, this.currentUser.id)
      .subscribe((response: any) => {
        this.refId = response.ReferenceNumber;
        let _status = _.filter(response.M_LookupsList, { 'LookupsID': response.Status });
        if (_status.length >= 0) this.status = _status[0].DisplayName;
        this.statusCode = response.Status ? parseInt(response.Status) : 0;
        this.date = new Date(response.CreatedDateTime);
        this.departmentList = response.OrganizationList;
        this.approverDepartmentList = response.M_ApproverDepartmentList;
        this.getSouceName(response.SourceName, response.SourceOU);
        this.requesterDepartment = response.RequestorDepartmentID;
        this.onDepartmentSelect('requester', false);
        this.requester = response.RequestorID;
        this.subject = response.Subject;
        this.approver = response.ApproverID;
        this.approverDepartment = response.ApproverDepartmentID;
        this.onDepartmentSelect('approver', false);
        this.RequestDetails = response.RequestDetails;
        this.priority = response.Priority ? 'high' : 'low';
        this.attachments = response.Attachments;
        this.RequestComments = response.MaintenanceCommunicationHistory;
        this.AssigneeId = response.AssigneeID;
        this.MaintenanceManagerUserID = response.MaintenanceManagerUserID;
        this.MaintenanceID = response.MaintenanceID;
        this.currentApprover = response.CurrentApprover;
        if (response.CreatedBy == this.currentUser.id) {
          this.isCreatedByMe = true;
        }

        this.checkIfAssignedToMe();
        this.checkIsApprover();
        this.decideBtnsToShow();
      });
  }

  checkIsApprover() {
    if (this.currentApprover && this.currentApprover.length > 0) {
      this.currentApprover.forEach((approver: any) => {
        if (approver.ApproverId == this.currentUser.id) {
          this.isApprover = true;
        }
      });
    }
  }

  checkIfAssignedToMe() {
    if (this.AssigneeId) {
      this.isAssigned = true;
      if (this.AssigneeId == this.currentUser.id) {
        this.isAssignedToMe = true;
      }
    }
  }

  decideBtnsToShow() {
    this.showResubmitBtn = false;
    this.showApproveBtn = false;
    this.showEscalateBtn = false;
    this.showRejectBtn = false;
    this.showReturnBtn = false;
    this.showAssignBtn = false;
    this.showAssignToMeBtn = false;
    this.showCloseBtn = false;
    this.isFirstApprover = false;
    this.showCommentBox = false;
    if (this.statusCode == 53 && this.isApprover) {
      this.showApproveBtn = true;
      this.showEscalateBtn = true;
      this.showRejectBtn = true;
      this.showReturnBtn = true;
      this.isFirstApprover = true;
      this.showCommentBox = true;
    } else if (this.statusCode == 54 && this.isCreatedByMe) {
      this.showResubmitBtn = true;
      this.editMode = true;
      this.valid = true;
      this.showCommentBox = true;
    } else if (this.statusCode == 55 && !this.MaintenanceManagerUserID
      && this.IsOrgHead && this.OrgUnitID == 12) {
      this.showApproveBtn = true;
      this.showRejectBtn = true;
      this.showReturnBtn = true;
      this.showCommentBox = true;
    } else if (this.statusCode == 55 && this.MaintenanceManagerUserID
      && this.IsOrgHead && this.OrgUnitID == 12) {
      if (!this.isAssigned) {
        this.showAssignBtn = true;
        this.showCommentBox = true;
      }
      if (this.isAssigned && this.AssigneeId != this.currentUser.id) {
        this.showAssignBtn = true;
        this.showCommentBox = true;
      }
      if (this.isAssigned && this.AssigneeId == this.currentUser.id) {
        this.showCloseBtn = true;
        this.showCommentBox = true;
      }
    } else if (this.statusCode == 55 && this.MaintenanceManagerUserID
      && !this.IsOrgHead && this.OrgUnitID == 12) {
      if (!this.isAssigned) {
        this.showAssignToMeBtn = true;
      }
      if (this.isAssigned && this.AssigneeId != this.currentUser.id) {
        this.showAssignToMeBtn = true;
      } else if (this.isAssigned && this.isAssignedToMe) {
        this.showCloseBtn = true;
      }
      this.showCommentBox = true;
    }
  }

  sendRequest() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      this.maintenance.Status = 'New';
      this.maintenance.Date = new Date();
      this.maintenance.SourceOU = this.currentUser.DepartmentID;
      this.maintenance.SourceName = this.currentUser.UserID;
      this.maintenance.RequestorID = this.requester;
      this.maintenance.RequestorDepartmentID = this.requesterDepartment;
      this.maintenance.Subject = this.subject;
      this.maintenance.ApproverID = this.approver;
      this.maintenance.ApproverDepartmentID = this.approverDepartment;
      this.maintenance.RequestDetails = this.RequestDetails;
      this.maintenance.Priority = this.priority === 'high' ? true : false;
      this.maintenance.CreatedBy = this.currentUser.id;
      this.maintenance.CreatedDateTime = new Date();
      this.maintenance.Action = 'Submit';
      this.maintenance.Comments = this.Comments;
      this.maintenance.Attachments = this.attachments;
      this.maintenanceService.create(this.maintenance)
        .subscribe((response: any) => {
          if (response.MaintenanceID) {
            if (this.lang === 'en') {
              this.message = "Maintenance Request Submitted Successfully";
            } else {
              this.message = this.common.arabic.words['maintenancereqcreatemsg'];
            }
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['/app/maintenance/home']);
            });
          }
          this.inProgress = false;
        });
    }
  }

  reSubmitRequest() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      const resubmitData = {
        MaintenanceID: this.MaintenanceID,
        SourceOU: this.currentUser.DepartmentID,
        SourceName: this.currentUser.UserID,
        RequestorID: this.requester,
        RequestorDepartmentID: this.requesterDepartment,
        Subject: this.subject,
        ApproverID: this.approver,
        ApproverDepartmentID: this.approverDepartment,
        RequestDetails: this.RequestDetails,
        Priority: this.priority === 'high' ? true : false,
        AssigneeID: this.AssigneeId,
        UpdatedBy: this.currentUser.id,
        UpdatedDateTime: new Date(),
        Action: 'Resubmit',
        Comments: this.Comments,
        Attachments: this.attachments
      };
      this.maintenanceService.reSubmit(resubmitData)
        .subscribe((response: any) => {
          if (response.MaintenanceID) {
            if(this.lang == 'en'){
              this.message = "Maintenance Request Resubmitted Successfully";
            } else {
              this.message = this.common.arabic.words['maintenancerequpdatemsg'];
            }            
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['/app/maintenance/home']);
            });
          }
          this.inProgress = false;
        });
    }
  }

  onAssignTo() {
    this.inProgress = true;
    let initialState = {
      id: this.id,
      ApiString: "/Maintenance",
      message: "Maintenance Request Assigned Successfully",
      Title: "Assign To",
      redirectUrl: '/app/maintenance/home',
      ApiTitleString: "Assign To",
      comments: this.Comments
    };
    if(this.lang == 'ar') {
      initialState.message = this.common.arabic.words.maintenanceassignedmsg;
      initialState.Title = this.common.arabic.words.assignto;
      initialState.ApiTitleString = this.common.arabic.words.assignto;
    }
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  onEscalate() {
    this.inProgress = true;
    let initialState = {
      id: this.MaintenanceID,
      ApiString: "Maintenance",
      message: "Maintenance Request Escalated Successfully",
      Title: "Escalate",
      comments: this.Comments,
      redirectPath: '/app/maintenance/home',
      isFirstApprover: this.isFirstApprover
    };
    if(this.lang == 'ar') {
      initialState.message = this.common.arabic.words['maintenancereqescalatemsg'];
      initialState.Title = this.common.arabic.words.escalate;
    }
    this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  updateAction(action: string) {
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "Replace"
      }, {
        "value": this.Comments,
        "path": "Comments",
        "op": "Replace",
      }, {
        "value": this.currentUser.id,
        "path": "UpdatedBy",
        "op": "Replace"
      }, {
        "value": new Date(),
        "path": "UpdatedDateTime",
        "op": "Replace"
      }
    ];
    this.update(dataToUpdate, action);
  }

  update(dataToUpdate: any, action: string) {
    this.inProgress = true;
    switch (action) {
      case 'Approve':
        this.message = "Maintenance Request Approved Successfully";
        if(this.lang == 'ar') {
          this.message = this.common.arabic.words['maintenancereqapprovedmsg'];
        }
        break;
      case 'Reject':
        this.message = "Maintenance Request Rejected Successfully";
        if(this.lang == 'ar') {
          this.message = this.common.arabic.words['maintenancereqrejectmsg'];
        }
        break;
      case 'ReturnForInfo':
        this.message = "Maintenance Request Returned For Info Successfully";
        if(this.lang == 'ar') {
          this.message = this.common.arabic.words['maintenancereqreturnedmsg'];
        }
        break;
      case 'AssignToMe':
        this.message = "Maintenance Request Assigned Successfully";
        if(this.lang == 'ar') {
          this.message = this.common.arabic.words['maintenanceassignedmsg'];
        }
        break;
      case 'Close':
        this.message = "Maintenance Request Closed Successfully";
        if(this.lang == 'ar') {
          this.message = this.common.arabic.words['maintenanceclosedmsg'];
        }
        break;
    }
    this.maintenanceService.update(this.id, dataToUpdate)
      .subscribe((response: any) => {
        if (response.MaintenanceID) {
          if (this.currentUser.OrgUnitID == 12 && this.IsOrgHead && this.statusCode == 55 && !this.MaintenanceManagerUserID && action == "Approve") {
            this.inProgress = true;
            let initialState = {
              id: this.id,
              ApiString: "/Maintenance",
              message: "Maintenance Request Approved And Assigned Successfully",
              Title: "Assign To",
              redirectUrl: '/app/maintenance/home',
              ApiTitleString: "Assign To",
              comments: this.Comments
            };
            if(this.lang == 'ar') {
              initialState.message = this.common.arabic.words['maintenancelreqescalatemsg'];
              initialState.ApiTitleString = this.common.arabic.words.assignto;
            }
            this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.inProgress = false;
              this.router.navigate(['/app/maintenance/home']);
            });
          } else {
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['/app/maintenance/home']);
            });
          }
        }
        this.inProgress = false;
      });
  }

  async getSouceName(UserID, DepID) {
    let params = [{
      "OrganizationID": DepID,
      "OrganizationUnits": "string"
    }];
    this.common.getUserList(params, 0).subscribe((data: any) => {
      this.sourceName = data.find(x => x.UserID == UserID).EmployeeName.toString();
      this.sourceou = this.departmentList.find(x => x.OrganizationID == DepID).OrganizationUnits;
    });
  }

}
