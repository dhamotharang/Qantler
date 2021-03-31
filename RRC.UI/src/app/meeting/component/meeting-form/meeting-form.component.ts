import { EndPointService } from 'src/app/api/endpoint.service';
import { Component, OnInit, OnDestroy, Input, TemplateRef, ViewChild, Inject, Renderer2 } from '@angular/core';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Meeting } from '../../model/meeting.model';
import { MeetingService } from '../../service/meeting.service';
import { CommonService } from 'src/app/common.service';
import { DatePipe, DOCUMENT } from '@angular/common';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';

@Component({
  selector: 'app-meeting-form',
  templateUrl: './meeting-form.component.html',
  styleUrls: ['./meeting-form.component.scss']
})
export class MeetingFormComponent implements OnInit, OnDestroy {
  @Input() screenStatus : string;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  lang: string;
  MeetingID: any;
  @ViewChild('template') template : TemplateRef<any>;
  MeetingModel: Meeting = new Meeting();
  Meeting: any = {
    ReferenceNumber: null,
    OrganizerDepartmentID: null,
    OrganizerUserID: null,
    Subject: '',
    Location: '',
    StartDateTime: '',
    EndDateTime: '',
    MeetingType: null,
    IsInternalInvitees: false,
    InternalInvitees:[{
      MeetingInternalInviteesID:0,
      DepartmentID: null,
      UserID: null,
    }],
    IsExternalInvitees: false,
    ExternalInvitees:[{
      MeetingExternalInviteesID: 0,
      Organization: '',
      ContactPerson: '',
      EmailID: '',
    }],
    RemindMeAt: [{
      MeetingRemindID : 0,
      RemindMeDateTime: "",
      remindTime: null,
      isValidDate: false
    }],
    CreatedBy: '',
    CreatedDateTime: new Date(),
    Action: '',
    Comments: '',
    Attachments: [],
    StartTime: null,
    EndTime: null,
    remindTime: null,
    Status: null,
    MeetingCommunicationHistory: [],
    Attendees: ''
  };
  MeetingList: Array<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  organizerDeptList: any=[];
  organizerList: any=[];
  departmentList: any=[];
  userList: any=[];
  enableInternal: false;
  enableExternal: false;
  timeList: Array<any>;
  inProgress: boolean = false;
  message: string;
  momModel:any = {
    MeetingID: '',
    PointsDiscussed: '',
    PendingPoints: '',
    Suggestion: '',
    CreatedBy: '',
    CreatedDateTime: new Date(),
  }
  OrganizerUserID: any;
  editMode: boolean= true;
  bsModalRef: BsModalRef;
  valid: boolean;
  errorOffMail: boolean = true;
  isStartEndDiff:boolean = false;
  MOMid: any;
  enableMOMDownload: boolean = false;
  month: any;
  errorStartDate: boolean;
  showValidTimeAlert: boolean = false;
  hideCancelBtn: boolean;
  cancelProgress: boolean = false;
  isRemiderTime: boolean = false;
  activateCreateMOM: boolean;
  disableReschedule: boolean;
  commentSubscriber: any;
  // canComment: boolean;

  constructor(
    public arabic: ArabicDataService,
    public route: ActivatedRoute,
    public router: Router,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document,
    public modalService: BsModalService,
    public utils: UtilsService,
    private service: MeetingService,
    public common: CommonService,
    public datepipe: DatePipe,
    public endpoint: EndPointService,
    private commentSectionService:CommentSectionService,
    private dropDownService:DropdownsService
  ) { }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.common.topSearchBanner(false, '', '', '');
    if (this.lang == 'en') {
      this.timeList = [
                        {"value":"00:00","label":"12:00 AM"},
                        {"value":"00:30","label":"12:30 AM"},
                        {"value":"01:00","label":"01:00 AM"},
                        {"value":"01:30","label":"01:30 AM"},
                        {"value":"02:00","label":"02:00 AM"},
                        {"value":"02:30","label":"02:30 AM"},
                        {"value":"03:00","label":"03:00 AM"},
                        {"value":"03:30","label":"03:30 AM"},
                        {"value":"04:00","label":"04:00 AM"},
                        {"value":"04:30","label":"04:30 AM"},
                        {"value":"05:00","label":"05:00 AM"},
                        {"value":"05:30","label":"05:30 AM"},
                        {"value":"06:00","label":"06:00 AM"},
                        {"value":"06:30","label":"06:30 AM"},
                        {"value":"07:00","label":"07:00 AM"},
                        {"value":"07:30","label":"07:30 AM"},
                        {"value":"08:00","label":"08:00 AM"},
                        {"value":"08:30","label":"08:30 AM"},
                        {"value":"09:00","label":"09:00 AM"},
                        {"value":"09:30","label":"09:30 AM"},
                        {"value":"10:00","label":"10:00 AM"},
                        {"value":"10:30","label":"10:30 AM"},
                        {"value":"11:00","label":"11:00 AM"},
                        {"value":"11:30","label":"11:30 AM"},
                        {"value":"12:00","label":"12:00 PM"},
                        {"value":"12:30","label":"12:30 PM"},
                        {"value":"13:00","label":"01:00 PM"},
                        {"value":"13:30","label":"01:30 PM"},
                        {"value":"14:00","label":"02:00 PM"},
                        {"value":"14:30","label":"02:30 PM"},
                        {"value":"15:00","label":"03:00 PM"},
                        {"value":"15:30","label":"03:30 PM"},
                        {"value":"16:00","label":"04:00 PM"},
                        {"value":"16:30","label":"04:30 PM"},
                        {"value":"17:00","label":"05:00 PM"},
                        {"value":"17:30","label":"05:30 PM"},
                        {"value":"18:00","label":"06:00 PM"},
                        {"value":"18:30","label":"06:30 PM"},
                        {"value":"19:00","label":"07:00 PM"},
                        {"value":"19:30","label":"07:30 PM"},
                        {"value":"20:00","label":"08:00 PM"},
                        {"value":"20:30","label":"08:30 PM"},
                        {"value":"21:00","label":"09:00 PM"},
                        {"value":"21:30","label":"09:30 PM"},
                        {"value":"22:00","label":"10:00 PM"},
                        {"value":"22:30","label":"10:30 PM"},
                        {"value":"23:00","label":"11:00 PM"},
                        {"value":"23:30","label":"11:30 PM"},
                      ];
      // this.MeetingList=[
      //   { "key": 1, "type": "Meeting" },
      //   { "key": 2, "type": "Workshop" },
      //   { "key": 3, "type": "Training"},
      //   { "key": 4, "type": "Others" }];
      if ((this.screenStatus == "createMOM")) {
        this.common.breadscrumChange('Meetings', 'Create MOM', '');
      } else if((this.screenStatus == "viewMOM")){
        this.common.breadscrumChange('Meetings', 'View MOM', '');
      } else if (this.screenStatus == "create") {
        this.common.breadscrumChange('Meetings', 'Create Meeting', '');
        this.common.sideNavResponse('meeting', 'Create Meeting');
      } else if (this.screenStatus == "view") {
        this.common.breadscrumChange('Meetings', 'View Meeting', '');
      } else if (this.screenStatus == "edit") {
        this.common.breadscrumChange('Meetings', 'Edit Meeting', '');
      }
    } else {
      this.timeList = [
                        {"value":"00:00","label":"12:00"+this.arabicfn('am')},
                        {"value":"00:30","label":"12:30"+this.arabicfn('am')},
                        {"value":"01:00","label":"01:00"+this.arabicfn('am')},
                        {"value":"01:30","label":"01:30"+this.arabicfn('am')},
                        {"value":"02:00","label":"02:00"+this.arabicfn('am')},
                        {"value":"02:30","label":"02:30"+this.arabicfn('am')},
                        {"value":"03:00","label":"03:00"+this.arabicfn('am')},
                        {"value":"03:30","label":"03:30"+this.arabicfn('am')},
                        {"value":"04:00","label":"04:00"+this.arabicfn('am')},
                        {"value":"04:30","label":"04:30"+this.arabicfn('am')},
                        {"value":"05:00","label":"05:00"+this.arabicfn('am')},
                        {"value":"05:30","label":"05:30"+this.arabicfn('am')},
                        {"value":"06:00","label":"06:00"+this.arabicfn('am')},
                        {"value":"06:30","label":"06:30"+this.arabicfn('am')},
                        {"value":"07:00","label":"07:00"+this.arabicfn('am')},
                        {"value":"07:30","label":"07:30"+this.arabicfn('am')},
                        {"value":"08:00","label":"08:00"+this.arabicfn('am')},
                        {"value":"08:30","label":"08:30"+this.arabicfn('am')},
                        {"value":"09:00","label":"09:00"+this.arabicfn('am')},
                        {"value":"09:30","label":"09:30"+this.arabicfn('am')},
                        {"value":"10:00","label":"10:00"+this.arabicfn('am')},
                        {"value":"10:30","label":"10:30"+this.arabicfn('am')},
                        {"value":"11:00","label":"11:00"+this.arabicfn('am')},
                        {"value":"11:30","label":"11:30"+this.arabicfn('am')},
                        {"value":"12:00","label":"12:00"+this.arabicfn('pm')},
                        {"value":"12:30","label":"12:30"+this.arabicfn('pm')},
                        {"value":"13:00","label":"01:00"+this.arabicfn('pm')},
                        {"value":"13:30","label":"01:30"+this.arabicfn('pm')},
                        {"value":"14:00","label":"02:00"+this.arabicfn('pm')},
                        {"value":"14:30","label":"02:30"+this.arabicfn('pm')},
                        {"value":"15:00","label":"03:00"+this.arabicfn('pm')},
                        {"value":"15:30","label":"03:30"+this.arabicfn('pm')},
                        {"value":"16:00","label":"04:00"+this.arabicfn('pm')},
                        {"value":"16:30","label":"04:30"+this.arabicfn('pm')},
                        {"value":"17:00","label":"05:00"+this.arabicfn('pm')},
                        {"value":"17:30","label":"05:30"+this.arabicfn('pm')},
                        {"value":"18:00","label":"06:00"+this.arabicfn('pm')},
                        {"value":"18:30","label":"06:30"+this.arabicfn('pm')},
                        {"value":"19:00","label":"07:00"+this.arabicfn('pm')},
                        {"value":"19:30","label":"07:30"+this.arabicfn('pm')},
                        {"value":"20:00","label":"08:00"+this.arabicfn('pm')},
                        {"value":"20:30","label":"08:30"+this.arabicfn('pm')},
                        {"value":"21:00","label":"09:00"+this.arabicfn('pm')},
                        {"value":"21:30","label":"09:30"+this.arabicfn('pm')},
                        {"value":"22:00","label":"10:00"+this.arabicfn('pm')},
                        {"value":"22:30","label":"10:30"+this.arabicfn('pm')},
                        {"value":"23:00","label":"11:00"+this.arabicfn('pm')},
                        {"value":"23:30","label":"11:30"+this.arabicfn('pm')},
                      ];
      // this.MeetingList=[
      //   { "key": 1, "type": this.arabicfn('meeting') },
      //   { "key": 2, "type": this.arabicfn('workshop') },
      //   { "key": 3, "type": this.arabicfn('training')},
      //   { "key": 4, "type": this.arabicfn('others') }];
      if ((this.screenStatus == "createMOM")) {
        this.common.breadscrumChange(this.arabicfn('meeting'), this.arabicfn('createmom'), '');
      } else if((this.screenStatus == "viewMOM")){
        this.common.breadscrumChange(this.arabicfn('meeting'), this.arabicfn('viewmom'), '');
      } else if (this.screenStatus == "create") {
        this.common.breadscrumChange(this.arabicfn('meeting'), this.arabicfn('createmeeting'), '');
        this.common.sideNavResponse('meeting', 'Create Meeting');
        // this.common.sideNavResponse('meeting', this.arabicfn('createmeeting'));
      } else if (this.screenStatus == "view") {
        this.common.breadscrumChange(this.arabicfn('meeting'), this.arabicfn('viewmeeting'), '');
      } else if (this.screenStatus == "edit") {
        this.common.breadscrumChange(this.arabicfn('meeting'), this.arabicfn('editmeeting'), '');
      }
    }
    // this.common.sideNavResponse('meeting', 'Create Meeting');
    this.loadOrganizerDepList();
    // this.loadDepartmentList();
    this.MeetingID = this.route.snapshot.paramMap.get('id');
    if(this.screenStatus == "view" || this.screenStatus == "createMOM") {
      this.getMeeting();
      this.enableMOMDownload = false;
    }

    if(this.screenStatus == "viewMOM") {
      this.route.queryParams.subscribe(params => {
        this.MeetingID = params["MeetingID"];
        this.MOMid = params["MOMid"];
      });
      this.enableMOMDownload = true
      // this.getMeeting();
      this.getMOM(this.MeetingID);
    }
    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        this.getMeeting();
      }
    });
    this.getMeetingTypeList();
  }

  loadOrganizerDepList() {
    this.service.getRequestById(0, this.currentUser.id)
      .subscribe((response: any) => {
        this.organizerDeptList = response.OrganizationList;
        this.departmentList = response.OrganizationList;
        this.Meeting.OrganizerDepartmentID = this.currentUser.DepartmentID;
        this.onOrganizerSelect();
      });
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  onOrganizerSelect(){
    if (this.Meeting.OrganizerUserID)
      this.Meeting.OrganizerUserID = null;
    let params = [{
      "OrganizationID": this.Meeting.OrganizerDepartmentID,
      "OrganizationUnits": ""
    }];
    let sendUserID = this.currentUser.id;
      if (this.screenStatus == "view") {
        sendUserID = 0;
      }
    this.common.getUserList(params, 0).subscribe((data: any) => {
      this.organizerList = data;
      this.Meeting.OrganizerUserID = this.currentUser.id;
    });
  }

  onDepartmentSelect(){
    for(let i = 0; i< this.Meeting.InternalInvitees.length; i++) {
      // if (this.Meeting.InternalInvitees[i].UserID)
      //   this.Meeting.InternalInvitees[i].UserID = null;
      let params = [{
        "OrganizationID": this.Meeting.InternalInvitees[i].DepartmentID,
        "OrganizationUnits": ""
      }];
      let sendUserID = this.currentUser.id;
      if (this.screenStatus == "view") {
        sendUserID = 0;
      }
      this.common.getUserList(params, sendUserID).subscribe((data: any) => {
        this.userList = data;
      });
    }
  }

  addMore(action) {
    switch (action) {
      case 'External':
        this.moreExternal();
        break;
      case 'Internal':
        this.moreInternal();
        break;
      case 'Reminder':
        this.moreReminder();
        break;
    }

  }

  moreReminder(){
    this.Meeting.RemindMeAt.push({
      MeetingRemindID : 0,
      RemindMeDateTime: "",
      remindTime: '',
      isValidDate: false
    });
  }

  moreInternal(){
    this.Meeting.InternalInvitees.push({
      MeetingInternalInviteesID:0,
      DepartmentID: null,
      UserID: null,
    });
  }

  moreExternal(){
    this.Meeting.ExternalInvitees.push({
      MeetingExternalInviteesID: 0,
      Organization: '',
      ContactPerson: '',
      EmailID: '',
    });
    this.validateExternal();
  }

  validateForm() {
    let flag = true;
    if (this.Meeting.OrganizerDepartmentID &&
      this.Meeting.OrganizerUserID &&
      !this.utils.isEmptyString(this.Meeting.Subject.trim()) &&
      !this.utils.isEmptyString(this.Meeting.Location.trim()) &&
      this.Meeting.StartDateTime &&
      this.Meeting.StartTime &&
      this.Meeting.EndDateTime &&
      this.Meeting.EndTime &&
      (this.Meeting.IsInternalInvitees || this.Meeting.IsExternalInvitees)){
        flag = false;
      }
    return flag;
  }

  canComment() {
    let flag = true;
    if (this.Meeting.Comments) {
      flag = false;
    }
    return flag;
  }

  validateInternal() {
    let flag = true;
    if(this.Meeting.IsInternalInvitees){
      for(let i = 0; i< this.Meeting.InternalInvitees.length; i++) {
        if (this.Meeting.InternalInvitees.length === 1) {
          if(this.Meeting.InternalInvitees[i].DepartmentID && this.Meeting.InternalInvitees[i].UserID) {
            flag = false;
          } else {
            flag = true;
          }
        } else {
          flag = false;
        }
      }
    }else {
      flag = false;
    }
    return flag;
  }


  validateExternal() {
    let flag = true;
    if(this.Meeting.IsExternalInvitees){
      for(let i = 0; i< this.Meeting.ExternalInvitees.length; i++) {
        if (this.Meeting.ExternalInvitees.length === 1) {
          if(this.Meeting.ExternalInvitees[i].Organization && this.Meeting.ExternalInvitees[i].ContactPerson) {
            flag = false;
          }else{
            flag = true;
          }
        } else {
          flag = false;
        }
      }
    }else {
      flag = false;
    }
    return flag;
  }

  checkOffMail() {
    for(let i = 0; i< this.Meeting.ExternalInvitees.length; i++) {
      if(!this.utils.isEmail(this.Meeting.ExternalInvitees[i].EmailID.trim())) {
        this.Meeting.ExternalInvitees[i].validMail = true;
        return true;
      }else {
        this.Meeting.ExternalInvitees[i].validMail = false;
        this.inProgress = false;
      }
    }
  }

  minDate(days){
    if(this.Meeting.StartDateTime){
      let today = new Date(this.Meeting.StartDateTime);
      this.month = today.getMonth()+1;
      if (today.getMonth() < 10) {
        this.month = '0' + (today.getMonth() + 1);
      }
      let dateLimit = (today.getFullYear()) + '/' + this.month + '/' + (today.getDate() + days);
      let dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  maxDate(days) {
    if (this.Meeting.EndDateTime) {
      let endDate = new Date(this.Meeting.EndDateTime);
      this.month = endDate.getMonth()+1;
      if (this.month < 10) {
        this.month = '0' + (endDate.getMonth() + 1);
      }
      let dateLimit = (endDate.getFullYear()) + '/' + this.month + '/' + (endDate.getDate() + days);
      let dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  // checkEndDate(){
  //   if(this.Meeting.EndDateTime){
  //     let today = new Date(this.Meeting.EndDateTime);
  //     this.month = today.getMonth();
  //     if (today.getMonth() < 10) {
  //       this.month = '0' + (today.getMonth() + 1);
  //     }
  //     let dateLimit = (today.getFullYear()) + '/' + this.month + '/' + (today.getDate());
  //     let dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
  //     return new Date(dates);
  //   }
  // }

  checkStartTime(){
    if(!this.Meeting.StartDateTime){
      // this.Meeting.EndDateTime = '';
      return true;
    }
  }

  dateToStringFormation(date) {
    let day = date.getDate();
    if (day < 10) {
      day = '0' + day;
    }
    let month = date.getMonth() + 1;
    if (month < 10) {
      month = '0' + month;
    }
    let year = date.getFullYear();
    let formattedDate = year + '/' + month + '/' + day;
    return formattedDate;
  }

  splitHour(hour) {
    if(hour){
      let Hour = hour.substring(0, 2);
      return Hour;
    }
  }

  splitMinutes(hour) {
    if(hour){
      let Minutes = hour.substring(3, 5);
      return Minutes;
    }
  }

  onStartTimeSelect() {
    this.showValidTimeAlert = false;
    this.onEndTimeSelect();
  }

  onEndTimeSelect() {
    let startDate = new Date(this.Meeting.StartDateTime);
    let startDateString = this.dateToStringFormation(startDate);
    let endDate = new Date(this.Meeting.EndDateTime);
    let endDateString = this.dateToStringFormation(endDate);
    let startTime = this.Meeting.StartTime;
    let startHour = this.splitHour(startTime);
    let startMinutes = this.splitMinutes(startTime);
    let endTime = this.Meeting.EndTime;
    let endHour = this.splitHour(endTime);
    let endMinutes = this.splitMinutes(endTime);
    if (startDateString == endDateString) {
      if (endHour < startHour) {
        this.Meeting.StartTime = '';
        this.showValidTimeAlert = true;
        this.inProgress = true;
      } else if (endHour == startHour) {
        if (startMinutes > endMinutes) {
          this.Meeting.StartTime = '';
          this.showValidTimeAlert = true;
          this.inProgress = true;
        } else {
          this.inProgress = false;
          this.showValidTimeAlert = false;
        }
      } else {
        this.inProgress = false;
        this.showValidTimeAlert = false;
      }
    }
    for(let i = 0; i< this.Meeting.RemindMeAt.length; i++) {
      this.checkMeetingStart(i);
    }
  }

  checkStartEndDateDiff(){
      let StartDateTime = this.concatDateAndTime(this.Meeting.StartDateTime, this.Meeting.StartTime);
      let EndDateTime = this.concatDateAndTime(this.Meeting.EndDateTime, this.Meeting.EndTime);
      let flag = true;
      if(new Date(StartDateTime).getTime() < new Date(EndDateTime).getTime()){
        flag = false
      }else{
        flag = true;
      }
      return flag;
  }

  getMOM(MeetingID){
    this.service.getMOM(MeetingID, this.currentUser.id).subscribe((response: any) => {
      this.momModel.PointsDiscussed = response.PointsDiscussed;
      this.momModel.PendingPoints = response.PendingPoints;
      this.momModel.Suggestion = response.Suggestion;
      this.Meeting.OrganizerDepartmentID = response.OrganizerDepartmentID;
      this.Meeting.OrganizerUserID = response.OrganizerUserID;
      this.OrganizerUserID =  response.OrganizerUserID;
      this.Meeting.Subject = response.Subject;
      this.Meeting.Location = response.Location;
      this.Meeting.ReferenceNumber = response.ReferenceNumber;
      this.Meeting.Attendees = response.Attendees;
      this.Meeting.StartDateTime = new Date(response.StartDateTime);
      this.Meeting.EndDateTime = new Date(response.EndDateTime);
      this.Meeting.StartTime = this.formatAMPM(new Date(response.StartDateTime));
      this.Meeting.EndTime = this.formatAMPM(new Date(response.EndDateTime));
      this.Meeting.EndTime = this.formatAMPM(new Date(response.EndDateTime));
      // this.Meeting.MeetingCommunicationHistory = response.MeetingCommunicationHistory;
      this.Meeting.MeetingCommunicationHistory = [];
      this.Meeting.MeetingCommunicationHistory = this.setMeetingRequestComments(response.MeetingCommunicationHistory);
      this.onOrganizerSelect();
      this.onDepartmentSelect();
    });

  }
  getMeeting(){
    this.service.getMeeting(this.MeetingID, this.currentUser.id).subscribe((response: any) => {
      this.Meeting.MeetingID = response.MeetingID;
      this.Meeting.OrganizerDepartmentID = response.OrganizerDepartmentID;
      this.Meeting.OrganizerUserID = response.OrganizerUserID;
      this.OrganizerUserID =  response.OrganizerUserID;
      this.Meeting.Subject = response.Subject;
      this.Meeting.Location = response.Location;
      this.Meeting.Attendees = response.Location;
      this.Meeting.StartDateTime = new Date(response.StartDateTime);
      this.Meeting.EndDateTime = new Date(response.EndDateTime);
      this.Meeting.ReferenceNumber = response.ReferenceNumber;
      this.Meeting.IsInternalInvitees = response.IsInternalInvitees;
      this.Meeting.IsExternalInvitees = response.IsExternalInvitees;
      this.Meeting.CreatedBy = response.CreatedBy;
      this.Meeting.MeetingType = response.MeetingType;
      this.Meeting.Status = response.Status;
      this.MOMid = response.MomID;
      this.Meeting.MeetingCommunicationHistory =  response.MeetingCommunicationHistory;
      if(response.RemindMeAt.length >0) {
        this.Meeting.RemindMeAt = response.RemindMeAt;
        for(let i = 0; i< this.Meeting.RemindMeAt.length; i++) {
          if(this.Meeting.RemindMeAt[i].RemindMeDateTime) {
            this.Meeting.RemindMeAt[i].RemindMeDateTime = this.Meeting.RemindMeAt[i].RemindMeDateTime ? new Date(this.Meeting.RemindMeAt[i].RemindMeDateTime) : "";
            this.Meeting.RemindMeAt[i].remindTime = this.Meeting.RemindMeAt[i].RemindMeDateTime ? this.formatAMPM(new Date(this.Meeting.RemindMeAt[i].RemindMeDateTime)) : "";
          }
        }
      }
      this.Meeting.ExternalInvitees = response.ExternalInvitees;
      this.Meeting.InternalInvitees = response.InternalInvitees;
      this.Meeting.StartTime = this.formatAMPM(new Date(response.StartDateTime));
      this.Meeting.EndTime = this.formatAMPM(new Date(response.EndDateTime));
      if (response.StartDateTime && response.EndDateTime) {
        const today = new Date().getTime();
        const startDate = new Date(this.Meeting.StartDateTime).getTime();
        const endDate = new Date(this.Meeting.EndDateTime).getTime();
        if ((startDate < today && endDate < today) || (startDate <= today && endDate >= today)) {
          this.cancelProgress = true;
          this.disableReschedule = true;
        }
      }
      this.onOrganizerSelect();
      this.onDepartmentSelect();
      this.checkUser();
      let currentDateForMOM = new Date().getTime();
      let endDateForMOM = new Date(response.EndDateTime).getTime();
      if (endDateForMOM < currentDateForMOM) {
        this.activateCreateMOM =  false;
      } else {
        this.activateCreateMOM =  true;
      }
      this.Meeting.MeetingCommunicationHistory = [];
      this.Meeting.MeetingCommunicationHistory = this.setMeetingRequestComments(response.MeetingCommunicationHistory);
    });
  }


  formatAMPM(date) {
    let time;
    let mins;
    let hours;
    mins = date.getMinutes();
    hours = date.getHours();
    mins = (parseInt(mins) % 60) < 10 ? '0' + (parseInt(mins) % 60) : (parseInt(mins) % 60);
    hours = (parseInt(hours) % 60) < 10 ? '0' + (parseInt(hours) % 60) : (parseInt(hours) % 60);
    time = hours+":"+ mins;
    return time;
  }

  checkUser(){
    if(this.currentUser.id == this.Meeting.CreatedBy && this.screenStatus=='view' && this.Meeting.Status == 114)  {
      this.editMode = true;
      this.isMeetingOver();
    }else {
      this.editMode = false;
    }
  }

  isMeetingOver(){
    let StartDateTime = this.concatDateAndTime(this.Meeting.StartDateTime, this.Meeting.StartTime);
    // console.log(this.concatDateAndTime(this.Meeting.StartDateTime, this.Meeting.StartTime));
    let currentTime = new Date().getTime();
    if(currentTime < new Date(StartDateTime).getTime() && this.Meeting.Status !=114 && this.Meeting.Status != 115){
      this.hideCancelBtn = false;
    }else {
      this.hideCancelBtn = true;
    }
  }

  validateReminder(){
    let flag = false;
    if(this.Meeting.RemindMeAt){
      for(let i = 0; i< this.Meeting.RemindMeAt.length; i++) {
        if(this.Meeting.RemindMeAt[i].RemindMeDateTime) {
          flag = false;
          if(!this.Meeting.RemindMeAt[i].remindTime) {
            flag = true;
          }
        }
      }
    }
    return flag;
  }

  checkMeetingStart(i){
    if(this.Meeting.StartDateTime && this.Meeting.StartTime && this.Meeting.RemindMeAt[i].RemindMeDateTime && this.Meeting.RemindMeAt[i].remindTime){
      let reminderDateTime = this.concatDateAndTime(this.Meeting.RemindMeAt[i].RemindMeDateTime, this.Meeting.RemindMeAt[i].remindTime)
      let StartDateTime = this.concatDateAndTime(this.Meeting.StartDateTime, this.Meeting.StartTime);
      if(new Date(reminderDateTime).getTime() < new Date(StartDateTime).getTime()){
        this.Meeting.RemindMeAt[i].isValidDate = false;
         this.isRemiderTime = false;
      }else {
        this.Meeting.RemindMeAt[i].isValidDate = true;
        this.isRemiderTime = true;
      }
      return this.inProgress;
    }
  }

  CreateMeetingRequest(action) {
    this.inProgress = true;
    if(this.Meeting.IsExternalInvitees) {
      if(this.checkOffMail()) {
        this.errorOffMail = true;
        return;
      }else {
        this.errorOffMail = false;
      }
    }
    this.MeetingModel.OrganizerDepartmentID = this.Meeting.OrganizerDepartmentID;
    this.MeetingModel.OrganizerUserID = this.Meeting.OrganizerUserID;
    this.MeetingModel.Subject = this.Meeting.Subject;
    this.MeetingModel.Location = this.Meeting.Location;
    this.MeetingModel.StartDateTime = this.concatDateAndTime(this.Meeting.StartDateTime, this.Meeting.StartTime);
    this.MeetingModel.EndDateTime = this.concatDateAndTime(this.Meeting.EndDateTime, this.Meeting.EndTime);
    this.MeetingModel.MeetingType = this.Meeting.MeetingType;
    this.MeetingModel.IsInternalInvitees = this.Meeting.IsInternalInvitees;
    this.MeetingModel.IsExternalInvitees = this.Meeting.IsExternalInvitees;
    this.MeetingModel.InternalInvitees =
    this.MeetingModel.MeetingInternalInviteesID = this.Meeting.MeetingInternalInviteesID;
    let internalInvitees = [];
    if(this.Meeting.IsInternalInvitees){
      for(let i = 0; i<  this.Meeting.InternalInvitees.length; i++) {
        if (this.Meeting.InternalInvitees[i].DepartmentID && this.Meeting.InternalInvitees[i].UserID) {
          internalInvitees.push({
            "MeetingInternalInviteesID": this.Meeting.InternalInvitees[i].MeetingInternalInviteesID,
            "DepartmentID": this.Meeting.InternalInvitees[i].DepartmentID,
            "UserID": this.Meeting.InternalInvitees[i].UserID
          })
        }
      }
    }
    this.MeetingModel.InternalInvitees = internalInvitees;
    let ExternalInvitees = [];
    if(this.Meeting.IsExternalInvitees){
      for(let i = 0; i<  this.Meeting.ExternalInvitees.length; i++) {
        if (this.Meeting.ExternalInvitees[i].Organization && this.Meeting.ExternalInvitees[i].ContactPerson) {
          ExternalInvitees.push({
            "MeetingExternalInviteesID": this.Meeting.ExternalInvitees[i].MeetingExternalInviteesID,
            "Organization": this.Meeting.ExternalInvitees[i].Organization,
            "ContactPerson": this.Meeting.ExternalInvitees[i].ContactPerson,
            "EmailID": this.Meeting.ExternalInvitees[i].EmailID
          })
        }
      }
    }
    this.MeetingModel.ExternalInvitees = ExternalInvitees;
    let remindMeAt = [];
    for(let i = 0; i<  this.Meeting.RemindMeAt.length; i++) {
      let reminder;
      if(this.Meeting.RemindMeAt[i].RemindMeDateTime){
        reminder = this.concatDateAndTime(this.Meeting.RemindMeAt[i].RemindMeDateTime, this.Meeting.RemindMeAt[i].remindTime)
        remindMeAt.push({
          "MeetingRemindID": this.Meeting.RemindMeAt[i].MeetingRemindID,
          "RemindMeDateTime": reminder
        })
      }
    }
    this.MeetingModel.RemindMeAt = remindMeAt;
    this.MeetingModel.Comments = this.Meeting.Comments;
    this.MeetingModel.Attachments = this.Meeting.Attachments;
    if(action == 'Submit') {
      this.MeetingModel.CreatedBy = this.currentUser.id;
      this.MeetingModel.CreatedDateTime = new Date();
      this.MeetingModel.Action = 'Submit';
      this.saveMeeting()
    }
    if(action == 'ReSchedule') {
      this.MeetingModel.MeetingID = this.MeetingID;
      this.MeetingModel.UpdatedBy = this.currentUser.id;
      this.MeetingModel.UpdatedDateTime = new Date();
      this.MeetingModel.Action = 'Reschedule';
      this.ReScheduleMeeting()
    }
  }

  saveMeeting(){
    this.hideBtns();
    this.service.saveMeeting(this.MeetingModel).subscribe(data => {
      this.inProgress = false;
      if (this.lang == 'en') {
        this.message = "Meeting Submitted Successfully";
      } else {
        this.message = this.arabicfn('momreqcreatemsg');
      }
      this.modalService.show(this.template);
      const newSubscriber = this.modalService.onHide.subscribe(() => {
        this.hideBtns();
        newSubscriber.unsubscribe();
        this.router.navigate(['/app/meeting/list']);
      });
    });
  }

  ReScheduleMeeting(){
    this.hideBtns();
    this.service.reSheduleMeeting(this.MeetingModel).subscribe(data => {
      this.inProgress = false;
      if (this.lang == 'en') {
        this.message = "Meeting Rescheduled Successfully";
      } else {
        this.message = this.arabicfn('meetingreqreschedulemsg');
      }
      this.modalService.show(this.template);
      const newSubscriber = this.modalService.onHide.subscribe(() => {
        this.hideBtns();
        newSubscriber.unsubscribe();
        this.router.navigate(['/app/meeting/list']);
      });
    });
  }

  concatDateAndTime(StartDate, StartTime){
    if(StartDate && StartTime){
      let StartDateVal = this.datepipe.transform(StartDate, 'yyyy-MM-dd');
      let startTime = StartTime.split(" ");
      startTime = startTime[0];
      let hh = new Date (StartDateVal +" "+startTime).toJSON();
      return hh;
    }
  }

  closemodal(){
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    this.router.navigate(['/app/meeting/list']);
  }

  navigateToMOM() {
    console.log('lan', this.lang);
    this.router.navigate(['/'+ this.lang +'/app/meeting/create-mom/'+ this.MeetingID]);
    this.getMeeting();
  }

  downloadMOM() {
    // this.service.downloadMOM(this.MeetingID, this.currentUser.id)
    //   .subscribe((response: any) => {
    //     const report = new Blob([response], {type: 'application/pdf'});
    //     const date = new Date();
    //     const curDate = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
    //     const url = window.URL.createObjectURL(report);
    //     const a = document.createElement('a');
    //     document.body.appendChild(a);
    //     a.setAttribute('style', 'display: none');
    //     a.href = url;
    //     window.URL.revokeObjectURL(url);
    //     a.remove();
    //   });
    this.downloadPDFMOM();
  }

  downloadPDFMOM() {
    this.service.downloadMOM(this.MeetingID, this.currentUser.id)
      .subscribe((response: any) => {
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.target = '_blank';
      a.setAttribute('style', 'display: none');
      a.href = this.endpoint.pdfDownloads+'/'+this.Meeting.ReferenceNumber+'.pdf';
      a.download = '';
      a.click();
      a.remove();
      this.common.deleteGeneratedFiles('files/delete', this.Meeting.ReferenceNumber + '.pdf').subscribe(result => {
        console.log(result);
      });
    });
  }

  navigateToViewMOM() {
    this.router.navigate(['/app/meeting/view-mom/'], { queryParams: { MeetingID: this.MeetingID} });
    this.getMeeting();
  }

  ValidateMOM(){
    this.valid = false;
    if (this.momModel.PointsDiscussed && !this.utils.isEmptyString(this.momModel.PointsDiscussed)){
      this.valid = true;
    }
    return this.valid;
  }

  createToMOM(){
    this.inProgress = true;
    this.momModel.MeetingID = this.MeetingID;
    this.momModel.PointsDiscussed = this.momModel.PointsDiscussed;
    this.momModel.PendingPoints = this.momModel.PendingPoints;
    this.momModel.Suggestion = this.momModel.Suggestion;
    this.momModel.CreatedBy = this.currentUser.id;
    this.service.saveMOM(this.momModel).subscribe((data: any) => {
      if (data.MOMID) {
        this.inProgress = false;
        if (this.lang == 'en') {
          this.message = 'MOM Submitted Successfully';
        } else {
          this.message = this.arabicfn('momreqcreatemsg');
        }
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = this.message;
        const newSubscriber = this.modalService.onHide.subscribe(() => {
          this.inProgress = true;
          newSubscriber.unsubscribe();
          this.router.navigate(['/app/meeting/list']);
        });
      }
      this.inProgress = false;
    });
  }

  createtask(){
    this.router.navigate(['/' + this.lang + '/app/task/task-create/'+this.Meeting.ReferenceNumber]);
  }

  cancelMeeting() {
    this.hideBtns();
    const dataToUpdate = [
      {
        "value": 'cancel',
        "path": "Action",
        "op": "Replace"
      }, {
        "value": 'cancel',
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
    if (this.lang == 'en') {
      this.message = "Meeting Cancelled Successfully";
    } else {
      this.message = this.arabicfn('meetingreqcancelmsg');
    }
    this.service.cancelMeeting(this.MeetingID, dataToUpdate)
    .subscribe((response: any) => {
      this.cancelProgress = false;
      this.inProgress =  false;
      if (response.MeetingID) {
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = this.message;
        let newSubscriber = this.modalService.onHide.subscribe(() => {
          this.hideBtns();
          newSubscriber.unsubscribe();
          if (this.common.language == 'English') {
            this.router.navigate(['/app/meeting/list']);
          } else {
            this.router.navigate(['/app/meeting/list']);
          }
        });
      }
    });
  }

  hisLog(status:string) {
    let sts = status.toLowerCase();
    if(this.common.currentLang != 'ar'){
      if (sts == 'submit') {
        return 'Submitted By';
      } else if (sts == 'reject' || sts == 'redirect') {
        return status + 'ed By';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        return 'Assigned By'
      } else {
        return status + 'led By';
      }
    }else if(this.common.currentLang == 'ar'){
      let arabicStatusStr = '';
      if (sts == 'reject' || sts == 'redirect') {
        arabicStatusStr = sts+'edby';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        arabicStatusStr = 'assignedby';
      } else if(sts == 'submit' || sts == 'resubmit'){
        arabicStatusStr = sts+'tedby';
      } else {
        arabicStatusStr = sts+'dby';
      }
      return this.common.arabic.words[arabicStatusStr];
    }
  }

  hideBtns(){
    this.inProgress =  true;
    this.cancelProgress =  true;
  }

  arabicfn(word){
    return this.common.arabic.words[word];
  }

  sendMessage(){
    if(this.Meeting.Comments && (this.Meeting.Comments.trim() != '')) {
      this.inProgress = true;
      let chatData: any = {
        CommunicationID: 0,
        MeetingID: this.MeetingID,
        Message: this.Meeting.Comments,
        ParentCommunicationID:0,
        CreatedBy:this.currentUser.id,
        CreatedDateTime:new Date(),
      };
      console.log("chatData&&&", chatData);
      this.commentSectionService.sendComment('Meeting',chatData).subscribe(
        (chatRes:any) => {
          console.log('chatres@@@2', chatRes);
          this.commentSectionService.newCommentCreated(true);
          this.Meeting.Comments = '';
          this.inProgress = false;
        }
      );
    }
  }
  private setMeetingRequestComments(commentSectionArr:any,parentCommunicationID?:any) {
    let recursiveCommentsArr = [];
    if(!parentCommunicationID){
      parentCommunicationID = 0;
    }
    commentSectionArr.forEach((commObj:any) => {
    if(commObj.ParentCommunicationID == parentCommunicationID) {
        let replies:any = this.setMeetingRequestComments(commentSectionArr,commObj.CommunicationID);
        if(replies.length > 0){
          replies.forEach(repObj => {
            repObj.hideReply = true;
          });
          commObj.Replies = replies;
        }
        // commObj.UserProfileImg = 'assets/home/user_male.png';
        if(!commObj.Photo){
          commObj.Photo = 'assets/home/user_male.png';
        }
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  getMeetingTypeList(){
    this.dropDownService.getMeetingType(this.currentUser.id).subscribe((meetingList:any) => {
      if(meetingList){
        this.MeetingList = meetingList;
      }
    });
  }
}
