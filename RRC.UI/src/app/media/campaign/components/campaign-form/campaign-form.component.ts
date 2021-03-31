import { Component, OnInit, ViewChild, ElementRef, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/common.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { CampaignService } from '../../services/campaign.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { HttpEventType } from '@angular/common/http';
import { UploadService } from 'src/app/shared/service/upload.service';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { Campaign } from '../../models/campaign.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-campaign-form',
  templateUrl: './campaign-form.component.html',
  styleUrls: ['./campaign-form.component.scss']
})
export class CampaignFormComponent implements OnInit,OnDestroy {
  @ViewChild('fileInput') fileInput: ElementRef;
  id: string;
  editMode: boolean = true;
  status: Number;
  refId: string = '';
  date: Date;
  sourceou: string = '';
  sourceName: string = '';
  approverList: Array<any> = [];
  approver: any;
  approverDeptList: Array<any> = [];
  approverDept: any;
  initiativeProjectActivity: string = '';
  campaignStartDate: Date;
  campaignPeriod: string = '';
  diwanRole: string = '';
  otherEntities: string = '';
  targetAudience: string = '';
  location: string = '';
  languageList: Array<any> = [];
  language: any;
  mediaChannelList: Array<any> = [];
  mediaChannel: any;
  notes: string = '';
  requestDetails: string = '';
  generalInformation: string = '';
  mainObjective: string = '';
  mainIdea: string = '';
  planVision: string = '';
  attachments: Array<any> = [];
  comment: string = '';
  CampaignID: any;
  RequestDetails: string;
  RequestComments: any;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  valid: boolean = false;
  inProgress: boolean = false;
  campaign: Campaign = new Campaign();
  message: string = '';
  bsModalRef: BsModalRef;
  OrgUnitID: Number;
  IsOrgHead: boolean = false;
  isAssigned: boolean = false;
  isAssignedToMe: boolean = false;
  isApprover: boolean = false;
  AssigneeId: any;
  CurrentApproverID: any = [];
  reSubmit: boolean = false;
  uploadPercentage: number;
  uploadProcess: boolean = false;
  CreatedDateTime: Date;
  isComment: boolean = false;
  @Input() screenStatus: String;
  canComment: boolean = false;
  CreatedBy: any;
  ApproverId: any;
  Departments = [];
  lang: string;
  popupMsg: string;
  isFirstApprover:boolean = false;
  userProfileImg: string;
  downloadUrl: string;
  commentSubscriber: any;

  constructor(private route: ActivatedRoute,
    public campaignService: CampaignService,
    public utils: UtilsService,
    public dropdownService: DropdownsService,
    public modalService: BsModalService,
    public router: Router,
    public upload: UploadService,
    public common: CommonService,
    private commentSectionService: CommentSectionService,
    private endpoint:EndPointService) {
      this.approverDept = this.currentUser.DepartmentID;
      this.onDepartmentSelect();
      this.currentUser = JSON.parse(localStorage.getItem('User'));
      this.downloadUrl = this.endpoint.fileDownloadUrl;
    }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.id = this.route.snapshot.paramMap.get("id");
    if (this.currentUser.AttachmentGuid && this.currentUser.AttachmentName) {
      this.userProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.currentUser.AttachmentName + '&guid=' + this.currentUser.AttachmentGuid;
    }else{
      this.userProfileImg = 'assets/home/user_male.png';
    }
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 0;
    if (this.id) {
      if (this.screenStatus == "edit") {
        if (this.lang == 'en') {
          this.common.breadscrumChange('Media', 'Request for Campaign', '');
        } else {
          this.common.breadscrumChange('الإعلام', this.arabic('requestforcampaign'), '');
        }
        this.editMode = true;
      } else {
        if (this.lang == 'en') {
          this.common.breadscrumChange('Media', 'Request for Campaign View', '');
        } else {
          this.common.breadscrumChange('الإعلام', this.arabic('requestforcampaignview'), '');
        }
        this.editMode = false;
      }
      this.loadCampaignRequest();
    } else {
      if (this.lang == 'en') {
        this.common.breadscrumChange('Media', 'Request for Campaign', '');
      } else {
        this.common.breadscrumChange('الإعلام', this.arabic('requestforcampaign'), '');
      }
      this.editMode = true;
      this.initForm();
    }
    this.loadLanguages();
    this.loadMediaChannels();
    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if (newComment) {
        this.loadCampaignRequest();
      }
    });
  }

  initForm() {
    this.canComment = true;
    this.sourceou = this.currentUser.UserID;
    this.sourceName = this.currentUser.departmentID;
    this.loadApproverList();
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  loadApproverList() {
    this.campaignService.getCampaignRequestById(0, this.currentUser.id)
      .subscribe((response: any) => {
        this.approverDeptList = response.M_ApproverDepartmentList;
        this.Departments = response.OrganizationList;
      });
  }

  loadLanguages() {
    this.dropdownService.getLanguages(this.currentUser.id)
      .subscribe((languages: any) => {
        this.languageList = languages;
      });
  }

  loadMediaChannels() {
    this.dropdownService.getMediaChannels(this.currentUser.id)
      .subscribe((data: any) => {
        this.mediaChannelList = data;
      });
  }

  private setRequestComments(commentSectionArr: any, parentCommunicationID?: any) {
    let recursiveCommentsArr = [];
    if (!parentCommunicationID) {
      parentCommunicationID = null;
    }
    commentSectionArr.forEach((commObj: any) => {
      if (commObj.ParentCommunicationID == parentCommunicationID) {
        let replies: any = this.setRequestComments(commentSectionArr, commObj.CommunicationID);
        if (replies.length > 0) {
          replies.forEach(repObj => {
            repObj.hideReply = true;
          });
          commObj.Replies = replies;
        }
        // commObj.UserProfileImg = commObj.Photo;
        // if(!commObj.Photo){
        //   commObj.Photo = 'assets/home/user_male.png';
        // }
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  onDepartmentSelect() {
    if (this.approver)
      this.approver = null;
    if (!this.approverDept)
      return;
    let params = [{
      "OrganizationID": this.approverDept,
      "OrganizationUnits": ""
    }];
    let id = 0;
    if (this.ApproverId == this.currentUser.id) {
      id = 0;
    } else {
      id = this.currentUser.id;
    }
    this.common.getmemoUserList(params, id).subscribe((data: any) => {
      this.approverList = data;
      this.approver = this.ApproverId;
    });
  }

  handleFileUpload(event: any) {
    if (event.target.files.length > 0) {
      const files = event.target.files;
      this.uploadProcess = true;
      this.upload.uploadAttachment(files)
        .subscribe((response: any) => {
          if (response.type === HttpEventType.UploadProgress) {
            this.uploadPercentage = Math.round(response.loaded / response.total) * 100;
          } else if (response.type === HttpEventType.Response) {
            this.uploadProcess = false;
            this.uploadPercentage = 0;
            for (let i = 0; i < response.body.FileName.length; i++) {
              const attachment = {
                AttachmentGuid: response.body.Guid,
                AttachmentsName: response.body.FileName[i],
                CampaignID: 0
              };
              this.attachments.push(attachment);
            }
          }
          this.validate();
          this.fileInput.nativeElement.value = '';
        }, (error) => {
          this.uploadProcess = false;
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
    if (this.utils.isEmptyString(this.campaignPeriod)
      || this.utils.isEmptyString(this.diwanRole)
      || this.utils.isEmptyString(this.otherEntities)
      || this.utils.isEmptyString(this.targetAudience)
      || this.utils.isEmptyString(this.location)
      || this.utils.isEmptyString(this.language)
      || this.utils.isEmptyString(this.mediaChannel)
      || this.utils.isEmptyString(this.notes)
      || this.utils.isEmptyString(this.requestDetails)
      || this.utils.isEmptyString(this.generalInformation)
      || this.utils.isEmptyString(this.mainObjective)
      || this.utils.isEmptyString(this.mainIdea)
      || this.utils.isEmptyString(this.planVision)) {
      this.valid = false;
    }
    return this.valid;
  }

  commentValid() {
    this.isComment = true;
    if (this.utils.isEmptyString(this.comment)) {
      this.isComment = false;
    }
  }

  sendRequest() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      this.campaign.Date = new Date();
      this.campaign.SourceOU = this.currentUser.DepartmentID;
      this.campaign.SourceName = this.currentUser.UserID;
      this.campaign.InitiativeProjectActivity = this.initiativeProjectActivity;
      this.campaign.CampaignStartDate = new Date(this.campaignStartDate);
      this.campaign.CampaignPeriod = this.campaignPeriod;
      this.campaign.DiwansRole = this.diwanRole;
      this.campaign.OtherEntities = this.otherEntities;
      this.campaign.TargetAudience = this.targetAudience;
      this.campaign.Location = this.location;
      this.campaign.Languages = this.language;
      this.campaign.MediaChannels = this.mediaChannel;
      this.campaign.Notes = this.notes;
      this.campaign.RequestDetails = this.requestDetails;
      this.campaign.GeneralInformation = this.generalInformation;
      this.campaign.MainObjective = this.mainObjective;
      this.campaign.MainIdea = this.mainIdea;
      this.campaign.StrategicGoals = this.planVision;
      this.campaign.ApproverID = this.approver ? this.approver : 0;
      this.campaign.ApproverDepartmentID = this.approverDept ? this.approverDept : 0;
      this.campaign.CreatedBy = this.currentUser.id;
      this.campaign.CreatedDateTime = new Date();
      this.campaign.Action = 'Submit';
      this.campaign.Comments = this.comment;
      this.campaign.Attachments = this.attachments;

      this.campaignService.create(this.campaign)
        .subscribe((response: any) => {
          if (response.CampaignID) {
            if (this.lang == 'en') {
              this.message = "Campaign Request Submitted Successfully";
            } else {
              this.message = this.arabic('campaignsubmitmsg');
            }
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['/app/media/protocol-home-page']);
            });
          }
          this.inProgress = false;
        });
    }
  }

  loadCampaignRequest() {
    this.campaignService.getCampaignRequestById(this.id, this.currentUser.id)
      .subscribe((response: any) => {
        this.CampaignID = response.CampaignID;
        this.date = response.Date;
        this.refId = response.ReferenceNumber;
        this.getSouceName(response.SourceName,response.SourceOU);
        var tempSourceOU = response.OrganizationList.find(x=> x.OrganizationID == response.SourceOU).OrganizationUnits;
       this.sourceou = tempSourceOU;
        this.initiativeProjectActivity = response.InitiativeProjectActivity;
        this.campaignStartDate = new Date(response.CampaignStartDate);
        this.campaignPeriod = response.CampaignPeriod;
        this.diwanRole = response.DiwansRole;
        this.otherEntities = response.OtherEntities;
        this.targetAudience = response.TargetAudience;
        this.location = response.Location;
        this.language = response.Languages;
        this.mediaChannel = response.MediaChannels;
        this.notes = response.Notes;
        this.requestDetails = response.RequestDetails;
        this.generalInformation = response.GeneralInformation;
        this.mainObjective = response.MainObjective;
        this.mainIdea = response.MainIdea;
        this.planVision = response.StrategicGoals;
        this.approverDept = response.ApproverDepartmentID;
        this.attachments = response.Attachments;
        this.status = response.Status;
        this.CurrentApproverID = response.CurrentApprover;
        this.CreatedBy = response.CreatedBy;
        this.CreatedDateTime = new Date(response.CreatedDateTime);
        this.ApproverId = response.ApproverID;
        this.onDepartmentSelect();
        // this.approverDeptList = response.OrganizationList;
        this.approverDeptList = response.M_ApproverDepartmentList;
        this.RequestComments = this.setRequestComments(response.CampaignCommunicationHistory);
        this.AssigneeId = response.AssigneeId;
        this.isFirstApprover = false;
        this.checkIsAssignedToMe();
        this.checkIsReturn();
        this.validate();
      });
  }

  async getSouceName(UserID,DepID) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.sourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });

   }

  checkIsReturn() {
    if (this.status == 83) {
      if (this.CreatedBy == this.currentUser.id) {
        this.editMode = true;
        this.reSubmit = true;
        this.canComment = true;
      }
    }
    if (this.status == 82) {
      if ((this.IsOrgHead && ((this.OrgUnitID == 4) || (this.OrgUnitID == 17))) || (!this.IsOrgHead && ((this.OrgUnitID == 4) || (this.OrgUnitID == 17)))) {
        this.canComment = true;
      }
    }
    if (this.status == 84) {
      this.canComment = false;
    }
    // if ((this.CreatedBy == this.currentUser.id) && (this.status == 87)) { //Rejected
    //   this.canComment = true;
    // }
    this.checkIsApprover();
  }

  checkIsApprover() {
    if (this.CurrentApproverID && this.CurrentApproverID.length > 0) {
      this.CurrentApproverID.forEach((assignee: any) => {
        if (assignee.ApproverId == this.currentUser.id) {
          this.isApprover = true;
          this.canComment = true;
          this.isFirstApprover = true;
          if (this.status == 83 || this.status == 82) {
            this.canComment = false;
          }
        }
      });
    }
  }


  checkIsAssignedToMe() {
    if (this.AssigneeId && this.AssigneeId.length > 0) {
      this.isAssigned = true;
      this.AssigneeId.forEach((assignee: any) => {
        if (assignee.AssigneeId == this.currentUser.id) {
          this.isAssignedToMe = true;
        }
      });
    }
  }

  onAssignTo() {
    if (this.lang == 'en') {
      this.popupMsg = "Campaign Request Assigned Successfully";
    } else {
      this.popupMsg = this.arabic('campaignassignedmsg');
    }
    this.inProgress = true;
    const initialState = {
      id: this.CampaignID,
      ApiString: "/Campaign",
      message: this.popupMsg,
      Title: "Assign To",
      redirectUrl: '/app/media/protocol-home-page',
      ApiTitleString: "Assign To",
      comments: this.comment,
    };
    if(this.common.currentLang == 'ar'){
      initialState.ApiTitleString = this.arabic('assignto');
    }
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  onEscalate() {
    if (this.lang == 'en') {
      this.popupMsg = "Campaign Request Escalated Successfully";
    } else {
      this.popupMsg = this.arabic('campaignescalatemsg');
    }
    this.inProgress = true;
    let initialState = {
      id: this.CampaignID,
      ApiString: "/Campaign",
      message: this.popupMsg,
      Title: "Escalate",
      comments: this.comment,
      redirectPath: '/app/media/protocol-home-page',
      isFirstApprover: this.isFirstApprover
    };
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
        "value": this.comment,
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
    if (this.lang == 'en') {
      switch (action) {
        case 'Approve':
          this.message = "Campaign Request Approved Successfully";
          break;
        case 'Reject':
          this.message = "Campaign Request Rejected Successfully";
          break;
        case 'ReturnForInfo':
          this.message = "Campaign Request Returned For Info Successfully";
          break;
        case 'AssignToMe':
          this.message = "Campaign Request Assigned Successfully";
          break;
        case 'Close':
          this.message = "Campaign Request Closed Successfully";
          break;
      }
    } else {
      switch (action) {
        case 'Approve':
          this.message = this.arabic('campaignapprovedmsg');
          break;
        case 'Reject':
          this.message = this.arabic('campaignrejectdmsg');
          break;
        case 'ReturnForInfo':
          this.message = this.arabic('campaignreturnedmsg');
          break;
        case 'AssignToMe':
          this.message = this.arabic('campaignassignedmsg');
          break;
        case 'Close':
          this.message = this.arabic('campaignclosedmsg');
          break;
      }
    }

    this.campaignService.update(this.id, dataToUpdate)
      .subscribe((response: any) => {
        if (response.CampaignID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/app/media/protocol-home-page']);
          });
        }
        this.inProgress = false;
      });
  }

  ReSubmit() {
    this.validate();
    if (this.valid) {
      if (this.AssigneeId && this.AssigneeId.length == 0) {
        this.AssigneeId = 0;
      }
      this.inProgress = true;
      this.campaign.CampaignID = this.CampaignID;
      this.campaign.Date = new Date();
      this.campaign.SourceOU = this.currentUser.DepartmentID;
      this.campaign.SourceName = this.currentUser.UserID;
      this.campaign.InitiativeProjectActivity = this.initiativeProjectActivity;
      this.campaign.CampaignStartDate = new Date(this.campaignStartDate);
      this.campaign.CampaignPeriod = this.campaignPeriod;
      this.campaign.DiwansRole = this.diwanRole;
      this.campaign.OtherEntities = this.otherEntities;
      this.campaign.TargetAudience = this.targetAudience;
      this.campaign.Location = this.location;
      this.campaign.Languages = this.language;
      this.campaign.MediaChannels = this.mediaChannel;
      this.campaign.Notes = this.notes;
      this.campaign.RequestDetails = this.requestDetails;
      this.campaign.GeneralInformation = this.generalInformation;
      this.campaign.MainObjective = this.mainObjective;
      this.campaign.MainIdea = this.mainIdea;
      this.campaign.StrategicGoals = this.planVision;
      this.campaign.ApproverID = this.approver ? this.approver : 0;
      this.campaign.ApproverDepartmentID = this.approverDept ? this.approverDept : 0;
      this.campaign.CreatedBy = this.currentUser.id;
      this.campaign.CreatedDateTime = this.CreatedDateTime;
      this.campaign.Comments = this.comment;
      this.campaign.Attachments = this.attachments;
      this.campaign.AssigneeID = this.AssigneeId;
      this.campaign.UpdatedBy = this.currentUser.id;
      this.campaign.UpdatedDateTime = new Date();
      this.campaign.DeleteFlag = true;
      this.campaign.Action = "Resubmit";

      this.campaignService.reSubmit(this.campaign)
        .subscribe((response: any) => {
          if (response.CampaignID) {
            if (this.lang == 'en') {
              this.message = "Campaign Request Resubmitted Successfully";
            } else {
              this.message = this.arabic('campaignresubmitdmsg');
            }
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['/app/media/protocol-home-page']);
            });
          }
          this.inProgress = false;
        });
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
