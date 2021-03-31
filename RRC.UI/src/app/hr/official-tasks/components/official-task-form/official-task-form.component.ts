import { ArabicDataService } from './../../../../arabic-data.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { OfficialTaskService } from '../../services/official-task.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { Employee } from '../../model/employee.model';
import { OfficialTask } from '../../model/official-task.model';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';
import { Compensation } from '../../model/compensation.model';
import * as _ from 'lodash';
import { CompensationModalComponent } from '../../modal/compensation-modal/compensation-modal.component';
import { ErrorModalComponent } from 'src/app/modal/error-modal/error-modal.component';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-official-task-form',
  templateUrl: './official-task-form.component.html',
  styleUrls: ['./official-task-form.component.scss']
})
export class OfficialTaskFormComponent implements OnInit,OnDestroy {
  editMode:boolean = true;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  id:any;
  type:any;
  status:any;
  refId:any;
  date:Date;
  sourceOU:any;
  sourceName:any;
  employeeDepartments: Array<any> = [];
  employeeDepartment:any;
  employeeList: Array<any> = [];
  employee:any;
  addedEmployeeList: Array<any> = [];
  selectedEmpData: Array<Employee> = [];
  officialTaskTypes: Array<any> = [];
  officialTaskType:any;
  startDate:Date;
  endDate:Date;
  dateFromErr:string = '';
  dateToErr:string = '';
  noOfDays:any;
  approverDepartment:any;
  approverList:Array<any> = [];
  approver:any;
  hospitality:any;
  taskDescription:string = '';
  compensationDescription:string = '';
  newComment:any;
  Comments:any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  valid:boolean = false;
  inProgress:boolean = false;
  showEmpDropDown:boolean = true;

  showCommentBox: boolean = false;
  OfficialTaskID: any;
  CompensationID: any;
  RequestComments: any;
  message:string = '';
  bsModalRef: BsModalRef;

  submitType:string = 'NEW_OFFICIAL_TASK_REQUEST';

  // buttons to show
  showSubmitBtn:boolean = false;
  showApproveBtn:boolean = false;
  showRejectBtn:boolean = false;
  showReturnBtn:boolean = false;
  showEscalateBtn:boolean = false;
  showAssignBtn:boolean = false;
  showAssignToMeBtn:boolean = false;
  showCloseBtn:boolean = false;
  showGenerateBtn:boolean = false;
  showMarkAsCompleteBtn:boolean = false;
  showCloseOffBtn:boolean = false;
  approvepopup :boolean=false;
  isInEmpList:boolean = false;
  showHospitalityOption:boolean = false;

  officialTask:OfficialTask = new OfficialTask();
  compensation:Compensation = new Compensation();

  officialTaskUrl:string = '/OfficialTask/';
  compensationUrl:string = '/Compensation/';
  updateUrl:string = this.compensationUrl;

  hospitalityDisabled:boolean = false;
  disableCompDescription:boolean = false;

  currentApprover: any;
  isApprover:boolean = false;
  HRManagerUserID:any;
  OrgUnitID: Number;
  IsOrgHead:boolean = false;
  AssigneeId:any;
  isAssigned:boolean = false;
  isAssignedToMe: boolean = false;
  IsCompensationAvailable: boolean = false;
  IsClosed: boolean = false;

  commentsModuleInfo = {
    commentType:'OfficialTask',
    moduleId:this.OfficialTaskID,
    moduleNameID:'OfficialTaskID'
  };
  hideOfficial: boolean;
  canComment: boolean = true;
  lang: string;
  arWords: any;

  creatorId: any;
  creatorDeptId: any;
  isCreator: boolean = true;

  empProfileImg: string = 'assets/home/user_male.png';
  userDetail: any;
  cur_user: string;
  commentSubscriber: any;

  constructor(
    private route: ActivatedRoute,
    public common: CommonService,
    public utils: UtilsService,
    public modalService: BsModalService,
    public router: Router,
    public dropdownService: DropdownsService,
    public officialTaskService: OfficialTaskService,
    public arabicService: ArabicDataService,
    private endpoint: EndPointService,
    private commentSectionService: CommentSectionService
  ) {
    this.lang = this.common.currentLang;
    this.arWords = this.arabicService.words;
    this.employeeDepartment = this.currentUser.DepartmentID;
    this.onDepartmentSelect('employee');
  }

  ngOnInit() {
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 0;
    this.id = this.route.snapshot.paramMap.get("id");
    this.type = this.route.snapshot.paramMap.get("type");
    if (this.id) {
      if (this.lang == 'en') {
        this.common.breadscrumChange('HR', 'Official Task Request View', '');
      } else {
        this.common.breadscrumChange(this.arWords.humanresource, this.arWords.officialtaskrequestview, '');
      }
      this.editMode = false;
      this.showEmpDropDown = false;
      if (this.type == 'official' || this.type == 'create-compensation') {
        this.commentsModuleInfo = {
          commentType:'OfficialTask',
          moduleId:this.OfficialTaskID,
          moduleNameID:'OfficialTaskID'
        };
        this.loadOfficialTask();
      } else if (this.type == 'compensation') {
        this.commentsModuleInfo = {
          commentType:'Compensation',
          moduleId:this.CompensationID,
          moduleNameID:'CompensationID'
        };
        this.loadCompensation();
      }
    } else {
      if (this.lang == 'en') {
        this.common.breadscrumChange('HR', 'Official Task Request Assignment', '');
      } else {
        this.common.breadscrumChange(this.arWords.humanresource, this.arWords.officialtaskrequestassignment, '');
      }
      this.editMode = true;
      this.showSubmitBtn = true;
      this.showCommentBox = true;
      this.submitType = 'NEW_OFFICIAL_TASK_REQUEST';
      this.initForm();
    }
    this.loadOfficialTaskTypes();
    if(this.type == 'compensation' || this.type == 'COMPENSATION_REQUEST' || this.type == 'create-compensation' ){
      this.hideOfficial = true;
    }else{
      this.hideOfficial = false;
    }

    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        if (this.type == 'official' || this.type == 'create-compensation') {
          this.loadOfficialTask();

        } else if (this.type == 'compensation') {
          this.loadCompensation();
        }
      }
    });
    // getting user profile picture
    this.userDetail = JSON.parse(localStorage.getItem('User'));
    this.cur_user = this.userDetail.username;
    if (this.userDetail.AttachmentGuid && this.userDetail.AttachmentName) {
      this.empProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.userDetail.AttachmentName + '&guid=' + this.userDetail.AttachmentGuid;
    }
  }

  initForm() {
    this.sourceOU = this.currentUser.departmentID;
    this.sourceName = this.currentUser.UserID;
    this.officialTaskService.getById(this.officialTaskUrl, 0, this.currentUser.id)
      .subscribe((response:any) => {
        if (response != null) {
          this.employeeDepartments = response.OrganizationList;
        }
      });
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }


  loadOfficialTaskTypes() {
    this.dropdownService.getOfficialTaskType(this.currentUser.id)
      .subscribe((taskTypes:any) => {
        this.officialTaskTypes = taskTypes;
      });
  }

  onDepartmentSelect(type:string) {
    let id:number;
    switch (type) {
      case 'employee': id = this.employeeDepartment;break;
      case 'approver': id = this.approverDepartment;break;
    }
    let params = [{
      "OrganizationID": id,
      "OrganizationUnits": ""
    }];
    if(type == 'employee'){
      this.common.getUserList(params,0).subscribe((data: any) => {
        this.employeeList = data;
      });
    }else if(type == 'approver'){
      this.common.getUserList(params,this.currentUser.id).subscribe((data: any) => {
        this.approverList = data;
      });
    }
  }

  onEmployeeSelect() {
    this.common.getEmpDetails(this.employee, this.currentUser.id)
      .subscribe((emp:any) => {
        let empFilter = _.filter(this.addedEmployeeList, { 'EmployeeCode': emp.EmployeeCode});
        if (empFilter.length < 1) {
          this.addedEmployeeList.push({
            EmployeeCode: emp.EmployeeCode,
            EmployeePosition: emp.EmployeePosition,
            Grade: emp.Grade,
            OfficialTaskEmployeeID: emp.UserProfileId
          });
          this.selectedEmpData.push({
            OfficialTaskEmployeeID: emp.UserProfileId,
            OfficialTaskEmployeeName: emp.EmployeeName
          });
          this.showEmpDropDown = false;
          this.employee = null;
          this.validate();
        }
      });
  }

  removeEmp(emp:any) {
    _.remove(this.addedEmployeeList, emp);
    _.remove(this.selectedEmpData, { OfficialTaskEmployeeID: emp.OfficialTaskEmployeeID });
    if (this.addedEmployeeList.length < 1) {
      this.showEmpDropDown = true;
    }
  }

  addEmployee() {
    this.showEmpDropDown = true;
  }

  validateDates(event, type:string) {
    switch(type) {
      case 'start':this.startDate = event;break;
      case 'end':this.endDate = event;break;
    }
    this.dateFromErr = '';
    this.dateToErr = '';
    if (this.startDate && !this.endDate) {
      this.dateToErr = this.lang === 'ar' ? this.arWords.plsSelEndDate : 'Please Select End Date';
      this.valid = false;
    } else if (this.endDate && !this.startDate) {
      this.dateFromErr = this.lang === 'ar' ? this.arWords.plsSelEndDate : 'Please Select Start Date';
      this.valid = false;
    } else if (this.startDate && this.endDate && !this.utils.isValidFromToDates(this.startDate, this.endDate)) {
      this.dateToErr = this.lang === 'ar' ? this.arWords.plsSelValDateRange : 'Please Select Valid Date Range!';
      this.valid = false;
    } else if (this.startDate && this.endDate ) {
      this.valid = true;
      this.calculateDays();
    }
    this.validate();
  }

  calculateDays() {
    this.noOfDays = Math.round(Math.abs(this.endDate.getTime() - this.startDate.getTime()) / (1000 * 60 * 60 * 24)) + 1;
  }

  validate() {
    this.valid = true;
    if (this.utils.isEmptyObject(this.selectedEmpData)
    || this.utils.isEmptyString(this.officialTaskType)
    || !this.startDate || !this.endDate
    || (this.editMode && this.utils.isEmptyString(this.noOfDays))
    || (!this.editMode && this.utils.isEmptyString(this.compensationDescription))
    || (!this.editMode && this.utils.isEmptyString(this.hospitality))
    || (this.editMode && this.utils.isEmptyString(this.taskDescription))) {
      this.valid = false;
    }
  }

  onSubmit() {
    switch (this.submitType) {
      case 'NEW_OFFICIAL_TASK_REQUEST': this.createOfficialTask(); break;
      case 'COMPENSATION_REQUEST_RESUBMIT': this.createCompensationResubmitRequest(); break;
    }
  }

  createOfficialTask() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      this.officialTask.Date = new Date();
      this.officialTask.SourceOU = this.currentUser.DepartmentID;
      this.officialTask.SourceName = this.currentUser.UserID;
      this.officialTask.EmployeeNameID = this.selectedEmpData;
      this.officialTask.OfficialTaskType = this.officialTaskType;
      this.officialTask.StartDate = this.startDate;
      this.officialTask.EndDate = this.endDate;
      this.officialTask.NumberofDays = this.noOfDays;
      this.officialTask.OfficialTaskDescription = this.taskDescription;
      this.officialTask.CreatedBy = this.currentUser.id;
      this.officialTask.CreatedDateTime = new Date();
      this.officialTask.Action = 'Submit';
      this.officialTask.Comments = this.Comments;

      // checking user availablity for task
      this.officialTaskService.checkUsersAvailabitity(
        this.selectedEmpData,
        this.currentUser.UserID,
        new Date(this.startDate).toJSON(),
        new Date(this.endDate).toJSON()
      ).subscribe((validUsers: any) => {
        if (validUsers) {
          this.officialTaskService.create(this.officialTaskUrl, this.officialTask)
            .subscribe((response: any) => {
              if (response.OfficialTaskID) {
                this.message = this.lang === 'ar' ? this.arWords.offcTaskReqSubSuc : 'Official Task Request Submitted Successfully';
                this.bsModalRef = this.modalService.show(SuccessComponent);
                this.bsModalRef.content.message = this.message;
                let newSubscriber = this.modalService.onHide.subscribe(() => {
                  newSubscriber.unsubscribe();
                  this.router.navigate(['/' + this.lang + '/app/hr/dashboard']);
                });
              }
              this.inProgress = false;
            })
        } else {
          this.inProgress = false;
          this.message = this.lang === 'ar' ? this.arWords.emphasanothertask : 'Employee has  another official task within the same period';
          this.bsModalRef = this.modalService.show(ErrorModalComponent);
          this.bsModalRef.content.message = this.message;
        }
      });
    }
  }


  createCompensationResubmitRequest() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      const compensationData = {
        CompensationID: this.CompensationID,
        SourceOU: this.currentUser.DepartmentID,
        SourceName: this.currentUser.UserID,
        ApproverDepartmentID: this.approverDepartment,
        ApproverID: this.approver,
        OfficialTaskType: this.officialTaskType,
        StartDate: this.startDate,
        EndDate: this.endDate,
        NumberofDays: this.noOfDays,
        CompensationDescription: this.compensationDescription,
        NeedCompensation: this.hospitality === 'yes' ? true : false,
        AssigneeID: this.currentUser.id,
        UpdatedBy: this.currentUser.id,
        UpdatedDateTime: new Date(),
        Action: 'Resubmit',
        Comments: this.Comments,
        DeleteFlag: true
      };
      this.officialTaskService.reSubmit(this.compensationUrl, compensationData)
        .subscribe((response: any) => {
          if (response.CompensationID) {
            this.message = this.lang === 'ar' ? this.arWords.comReqReSubSuc : 'Compensation Request Resubmitted Successfully';
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['/' + this.lang + '/app/hr/dashboard']);
            });
          }
          this.inProgress = false;
        });
    }
  }

  loadOfficialTask() {
    this.officialTaskService.getById(this.officialTaskUrl, this.id, this.currentUser.id)
      .subscribe((officialTask:any) => {
        if (officialTask.OfficialTaskID) {
          this.isThisCreator(officialTask.CreatedBy);
          this.OfficialTaskID = officialTask.OfficialTaskID;
          this.refId = officialTask.ReferenceNumber;
          this.date = new Date(officialTask.Date);
          this.getSouceName(officialTask.SourceName,officialTask.SourceOU);
          this.creatorId = officialTask.CreatedBy;
          this.creatorDeptId = officialTask.SourceOU;
          // this.sourceOU = officialTask.SourceOU;
          // this.sourceName = officialTask.SourceName;
          this.employeeDepartments = officialTask.OrganizationList;
          this.addedEmployeeList = officialTask.EmployeeNameID;
          this.officialTaskType = parseInt(officialTask.OfficialTaskType);
          this.startDate = new Date(officialTask.StartDate);
          this.endDate = new Date(officialTask.EndDate);
          this.noOfDays = officialTask.NumberofDays;
          this.taskDescription = officialTask.OfficialTaskDescription;
          this.RequestComments = this.setOfficialTaskComments(officialTask.OfficialTaskCommunicationHistory);
          this.status = parseInt(officialTask.Status);
          this.IsCompensationAvailable = officialTask && officialTask.IsCompensationAvailable ? true : false
          this.IsClosed = officialTask && officialTask.IsClosed ? true : false
          this.checkIsInEmpList();
          this.decideBtnsToShow();
        }
      });
  }

  isThisCreator(creatorID?: any) {
    if (this.currentUser.UserID === creatorID) {
      this.isCreator = true;
    } else {
      this.isCreator = false;
    }
  }

  loadCompensation() {
    this.showHospitalityOption = true;
    this.submitType = 'COMPENSATION_REQUEST';
    this.hospitalityDisabled = true;
    this.disableCompDescription = true;
    this.officialTaskService.getById(this.compensationUrl, this.id, this.currentUser.id)
      .subscribe((compensation:any) => {
          if (compensation.CompensationID) {
          this.isThisCreator(compensation.CreatedBy);
          this.CompensationID = compensation.CompensationID;
          this.refId = compensation.ReferenceNumber;
          this.date = new Date(compensation.CreatedDateTime);
          this.getSouceName(compensation.SourceName,compensation.SourceOU);
          // this.sourceOU = compensation.SourceOU;
          // this.sourceName = compensation.SourceName;
          this.employeeDepartments = compensation.OrganizationList;
          this.addedEmployeeList = compensation.EmployeeNameID;
          this.officialTaskType = parseInt(compensation.OfficialTaskType);
          this.startDate = new Date(compensation.StartDate);
          this.endDate = new Date(compensation.EndDate);
          this.noOfDays = compensation.NumberofDays;
          this.compensationDescription = compensation.CompensationDescription;
          this.hospitality = compensation.NeedCompensation ? 'yes' : 'no';
          this.RequestComments = this.setOfficialTaskComments(compensation.CompensationCommunicationHistory);
          this.status = parseInt(compensation.Status);
          this.currentApprover = compensation.CurrentApprover;
          this.HRManagerUserID = compensation.HRManagerUserID;
          this.AssigneeId = compensation.AssigneeID;

          this.checkIsInEmpList();
          this.checkIsApprover();
          this.checkIfAssignedToMe();

          this.decideBtnsToShow();
        }
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
    this.officialTaskService.getById(this.compensationUrl, this.id, this.currentUser.id, this.type)
      .subscribe((res:any) => {
        this.sourceOU= res.OrganizationList.find(x=> x.OrganizationID == DepID).OrganizationUnits;
    });
  }

  checkIsApprover() {
    if (this.currentApprover && this.currentApprover.length > 0) {
      this.currentApprover.forEach((approver:any) => {
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

  checkIsInEmpList() {
    if (this.addedEmployeeList && this.addedEmployeeList.length > 0) {
      this.addedEmployeeList.forEach((emp:any) => {
        if (emp.EmployeeID == this.currentUser.id) {
          this.isInEmpList = true;
        }
        this.selectedEmpData.push({
          OfficialTaskEmployeeID: emp.EmployeeID,
          OfficialTaskEmployeeName: emp.EmployeeName
        });
      });
    }
  }

  isCommentEmpty() {
    if (this.utils.isEmptyString(this.Comments)) {
      return true;
    }
    return false;
  }

  decideBtnsToShow() {
    this.showSubmitBtn = false;
    this.showApproveBtn = false;
    this.showRejectBtn = false;
    this.showReturnBtn = false;
    this.showEscalateBtn = false;
    this.showAssignBtn = false;
    this.showAssignToMeBtn = false;
    this.showCloseBtn = false;
    this.showGenerateBtn = false;
    this.showMarkAsCompleteBtn = false;
    this.showCloseOffBtn = false;
    this.showCommentBox = false;

    if (this.IsCompensationAvailable) {
      this.showMarkAsCompleteBtn = true;
      this.showCommentBox = true;
    } else if(this.IsClosed) {
      this.showCloseOffBtn = true;
      this.showCommentBox = true;
    } else if(this.status == 113 && this.isInEmpList && this.CompensationID) {
      this.showCloseBtn = true;
      this.showCommentBox = true;
    } else if(this.status == 107 && this.isInEmpList && this.type == 'create-compensation') {
      this.showSubmitBtn = true;
      this.showHospitalityOption = true;
      this.showCommentBox = true;
      this.submitType = 'COMPENSATION_REQUEST';
    } else if (this.status == 108 && this.isApprover) {
      // first level approval
      this.showHospitalityOption = true;
      this.submitType = 'COMPENSATION_REQUEST';
      this.showApproveBtn = true;
      this.showEscalateBtn = true;
      this.showRejectBtn = true;
      this.showCommentBox = true;
    } else if (this.status == 109 && !this.HRManagerUserID
      && this.IsOrgHead && this.OrgUnitID == 9) {
      // second level approval
      this.showApproveBtn = true;
      this.showRejectBtn = true;
      this.showReturnBtn = true;
      this.showCommentBox = true;
    } else if(this.status == 109 && this.HRManagerUserID
      && this.IsOrgHead && this.OrgUnitID == 9) {
        if (!this.isAssignedToMe) {
          this.showAssignBtn = true;
          this.showCommentBox = true;
        }
        if (this.isAssigned) {
          this.showGenerateBtn = true;
          this.showCommentBox = true;
        }
        if(this.AssigneeId==this.currentUser.id) {
          this.showCloseBtn = true;
          this.showCommentBox = true;
        }
        this.showGenerateBtn = true;
      } else if (this.status == 109 && this.HRManagerUserID
      && !this.IsOrgHead && this.OrgUnitID == 9) {
      if (!this.isAssigned) {
        this.showAssignToMeBtn = true;
        this.showCommentBox = true;
      } else
      if(this.isAssigned &&(this.AssigneeId!=this.currentUser.id))
      {
        this.showAssignToMeBtn = true;
        this.showCommentBox = true;
      }
      if(this.isAssigned && this.isAssignedToMe) {
        this.showCloseBtn = true;
        this.showCommentBox = true;
      }
      this.showGenerateBtn = true;
    } else if (this.status === 110) {
      this.showSubmitBtn = true;
      this.submitType = 'COMPENSATION_REQUEST_RESUBMIT';
      this.showHospitalityOption = true;
      if (this.isCreator) {
        this.showCommentBox = true;
        this.hospitalityDisabled = false;
        this.disableCompDescription = false;
      }
    }
  }

  onAssignTo() {
    this.inProgress = true;
    let initialState;
    if (this.approvepopup) {
      initialState = {
        id: this.CompensationID,
        ApiString: "/Compensation",
        message: this.lang === 'ar' ? this.arWords.compReqAppAssSuc : "Compensation Request Approved and Assigned Successfully",
        Title: this.lang === 'ar' ? this.arWords.assignedto : "Assign To",
        redirectUrl: '/app/hr/dashboard',
        ApiTitleString: this.lang === 'ar' ? this.arWords.assignedto : "Assign To",
      };
    } else {
      initialState = {
        id: this.CompensationID,
        ApiString: "/Compensation",
        message: this.lang === 'ar' ? this.arWords.compReqAssSuc : "Compensation Request Assigned Successfully",
        Title: this.lang === 'ar' ? this.arWords.assignedto : "Assign To",
        redirectUrl: '/app/hr/dashboard',
        ApiTitleString: this.lang === 'ar' ? this.arWords.assignedto : "Assign To",
      };
    }
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      // if(this.approvepopup) {
      //   this.router.navigate(['app/hr/dashboard']);
      // }
      this.inProgress = false;
    });
  }

  onEscalate() {
    this.inProgress = true;
    let initialState = {
      id: this.CompensationID,
      ApiString: "Compensation",
      message: this.lang === 'ar' ? this.arWords.compReqEsclSuc : "Compensation Request Escalated Successfully",
      Title: this.lang === 'ar' ? this.arWords.escalate : "Escalate",
      comments: this.Comments,
      redirectPath: '/app/hr/dashboard'
    };
    this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  administrationDecision() {
    this.officialTaskService.generateAdministrativeReport(this.OfficialTaskID || this.CompensationID, this.currentUser.id)
      .subscribe((response: any) => {
        if(response){
          var a = document.createElement('a');
          document.body.appendChild(a);
          a.setAttribute('style', 'display: none');
          a.href = this.endpoint.pdfDownloads + '/' + this.refId + '.pdf';
          a.download = this.refId + '.pdf';
          a.click();
          a.remove();
          this.common.deleteGeneratedFiles('files/delete', this.refId + '.pdf').subscribe(result => {
            console.log(result);
          });
        }
      });
  }

  downloadGeneralAdministration() {
    this.officialTaskService.DownloadGenerateAdministrative()
      .subscribe((data: Blob) => {
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = this.endpoint.pdfDownloads + '/GeneralAdministrativeReport.pdf';
        a.download = 'GeneralAdministrativeReport.pdf';
        a.click();
        a.remove();
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

  update(dataToUpdate:any, action:string) {
    this.inProgress = true;
    switch(action) {
      case 'Approve':
        this.message = this.lang === 'ar' ? this.arWords.compReqAppSuc : "Compensation Request Approved Successfully";
        break;
      case 'Reject':
        this.message = this.lang === 'ar' ? this.arWords.compReqRejSuc : "Compensation Request Rejected Successfully";
        break;
      case 'ReturnForInfo':
        this.message = this.lang === 'ar' ? this.arWords.compReqReturnedSuc : "Compensation Request Returned For Info successfully";
        break;
      case 'AssignToMe':
        this.message = this.lang === 'ar' ? this.arWords.compReqAssSuc : "Compensation Request Assigned Successfully";
        break;
      case 'Close':
        this.message = this.lang === 'ar' ? this.arWords.compReqClosedSuc : "Compensation Request Closed Successfully";
        break;
      case 'MarkasComplete':
        this.message = this.lang === 'ar' ? this.arWords.offcReqCmpltSuc : "Official Task Request Completed Successfully";
        this.updateUrl = this.officialTaskUrl;
        break;
    }
    this.officialTaskService.update(this.updateUrl, this.id, dataToUpdate)
    .subscribe((response:any) => {
      if (response.OfficialTaskID || response.CompensationID) {
        if (this.status == 109 && !this.HRManagerUserID
          && this.IsOrgHead && this.OrgUnitID == 9 &&action=='Approve') {
          this.approvepopup=true;
          this.onAssignTo();
        }
        if(!this.approvepopup) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
        }
        if (this.type == 'official' || this.type == 'create-compensation') {
          this.loadOfficialTask();
        } else if (this.type == 'compensation') {
          this.loadCompensation();
        }
        
          if(action == 'Approve' && response.HRManagerUserID == this.currentUser.id) {
            this.onAssignTo();
          } else if(!this.approvepopup) {
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['/' + this.lang + '/app/hr/dashboard']);
            });           
          }
        
      }
      this.inProgress = false;
    });
  }

  // compensation modal
  openCompensationModal() {
    this.inProgress = true;
    this.compensation.OfficialTaskID = this.OfficialTaskID;
    this.compensation.SourceOU = this.currentUser.DepartmentID;
    this.compensation.SourceName = this.currentUser.UserID;
    this.compensation.ApproverDepartmentID = this.creatorDeptId;
    this.compensation.ApproverID = this.creatorId;
    this.compensation.OfficialTaskType = this.officialTaskType;
    this.compensation.StartDate = this.startDate;
    this.compensation.EndDate = this.endDate;
    this.compensation.NumberofDays = this.noOfDays;
    this.compensation.CompensationDescription = '';
    this.compensation.NeedCompensation = null
    this.compensation.CreatedBy = this.currentUser.id;
    this.compensation.CreatedDateTime = new Date();
    this.compensation.Action = 'Submit';
    this.compensation.Comments = this.Comments;

    const initialState = {
      OfficialTaskID: this.OfficialTaskID,
      compensation: this.compensation,
      comments: this.Comments
    };

    this.bsModalRef = this.modalService.show(CompensationModalComponent, { initialState });
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      this.inProgress = false;
      newSubscriber.unsubscribe();
      this.bsModalRef.content.compensationCreated = false
    });
  }

  arabic(word) {
    word = word.replace(/ +/g, "").toLowerCase();
    return this.common.arabic.words[word];
  }

  private setOfficialTaskComments(commentSectionArr:any,parentCommunicationID?:any){
    let recursiveCommentsArr = [];
    if(!parentCommunicationID){
      parentCommunicationID = 0;
    }
    commentSectionArr.forEach((commObj:any)=>{
      if(commObj.ParentCommunicationID == parentCommunicationID){
        let replies:any = this.setOfficialTaskComments(commentSectionArr,commObj.CommunicationID);
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
}
