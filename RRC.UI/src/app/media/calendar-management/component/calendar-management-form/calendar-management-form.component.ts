import { ArabicDataService } from 'src/app/arabic-data.service';
import { DatePipe } from '@angular/common';
import { CommonService } from './../../../../common.service';
import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { BsDatepickerConfig, BsModalService, BsModalRef, TabHeadingDirective } from 'ngx-bootstrap';
import { CalendarManagement } from '../../model/calendar-management.model';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { CalendarManagementService } from '../../service/calendar-management.service';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { ActivatedRoute, Router } from '@angular/router';
import { ApologiesModalComponent } from 'src/app/modal/apologies-modal/apologies-modal.component';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';

@Component({
  selector: 'app-calendar-management-form',
  templateUrl: './calendar-management-form.component.html',
  styleUrls: ['./calendar-management-form.component.scss']
})
export class CalendarManagementFormComponent implements OnInit, OnDestroy {
  @Input() mode: string;
  bsModalRef: BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  lang: string;
  message: string;
  EventTypes: Array<any>;
  timeList: Array<any>;
  calendarManagementRequestComments: any[] = [];
  calendarModel: CalendarManagement = new CalendarManagement();
  calendarForm: any = {
    ReferenceID: '',
    Status: '',
    EventRequestor: '',
    EventType: '1',
    EventDetails: '',
    DateFrom: null,
    DateTo: null,
    AllDayEvents: false,
    Location: '',
    City: '',
    ApproverID: '',
    CreatedBy: '',
    CreatedDateTime: '',
    Action: '',
    Comments: '',
    ParentReferenceNumber: null,
    StartTime: '',
    EndTime: ''
  }
  CurrentStatus: any;
  cityList: Array<any>;
  locationList: Array<any>;
  statusList: Array<any>;
  approverList: Array<any>;
  userList: Array<any>;
  editMode: boolean;
  calendarID: any;
  bulkEvent: boolean;
  addMoreEvent: boolean;
  showValidTimeAlert: boolean;
  month: any;
  ApproveButton: boolean;
  ReturnForInfoButton: boolean;
  RejectButton: boolean;
  valid: boolean = false;
  inProgress: boolean = false;
  EscalateButton: boolean;
  showOptions: boolean;
  markAscompleted: boolean;
  submitButton: boolean;
  isFirstApprover: boolean = false;
  parentID: any;
  popupMsg: string;
  isApprover: boolean = false;
  commentSubscriber: any;
  returnForInfo: boolean = false;

  constructor(
    private common: CommonService,
    private commentSectionService: CommentSectionService,
    public modalService: BsModalService,
    public router: Router,
    public route: ActivatedRoute,
    public datepipe: DatePipe,
    public arabic: ArabicDataService,
    public calendarManagementService: CalendarManagementService
  ) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.calendarID = this.route.snapshot.paramMap.get('id');
    if (this.mode === 'create') {
      if (this.lang === 'en') {
        this.timeList = [
          { "value": "24:00", "label": "12:00 AM" },
          { "value": "00:30", "label": "12:30 AM" },
          { "value": "01:00", "label": "01:00 AM" },
          { "value": "01:30", "label": "01:30 AM" },
          { "value": "02:00", "label": "02:00 AM" },
          { "value": "02:30", "label": "02:30 AM" },
          { "value": "03:00", "label": "03:00 AM" },
          { "value": "03:30", "label": "03:30 AM" },
          { "value": "04:00", "label": "04:00 AM" },
          { "value": "04:30", "label": "04:30 AM" },
          { "value": "05:00", "label": "05:00 AM" },
          { "value": "05:30", "label": "05:30 AM" },
          { "value": "06:00", "label": "06:00 AM" },
          { "value": "06:30", "label": "06:30 AM" },
          { "value": "07:00", "label": "07:00 AM" },
          { "value": "07:30", "label": "07:30 AM" },
          { "value": "08:00", "label": "08:00 AM" },
          { "value": "08:30", "label": "08:30 AM" },
          { "value": "09:00", "label": "09:00 AM" },
          { "value": "09:30", "label": "09:30 AM" },
          { "value": "10:00", "label": "10:00 AM" },
          { "value": "10:30", "label": "10:30 AM" },
          { "value": "11:00", "label": "11:00 AM" },
          { "value": "11:30", "label": "11:30 AM" },
          { "value": "12:00", "label": "12:00 PM" },
          { "value": "12:30", "label": "12:30 PM" },
          { "value": "13:00", "label": "01:00 PM" },
          { "value": "13:30", "label": "01:30 PM" },
          { "value": "14:00", "label": "02:00 PM" },
          { "value": "14:30", "label": "02:30 PM" },
          { "value": "15:00", "label": "03:00 PM" },
          { "value": "15:30", "label": "03:30 PM" },
          { "value": "16:00", "label": "04:00 PM" },
          { "value": "16:30", "label": "04:30 PM" },
          { "value": "17:00", "label": "05:00 PM" },
          { "value": "17:30", "label": "05:30 PM" },
          { "value": "18:00", "label": "06:00 PM" },
          { "value": "18:30", "label": "06:30 PM" },
          { "value": "19:00", "label": "07:00 PM" },
          { "value": "19:30", "label": "07:30 PM" },
          { "value": "20:00", "label": "08:00 PM" },
          { "value": "20:30", "label": "08:30 PM" },
          { "value": "21:00", "label": "09:00 PM" },
          { "value": "21:30", "label": "09:30 PM" },
          { "value": "22:00", "label": "10:00 PM" },
          { "value": "22:30", "label": "10:30 PM" },
          { "value": "23:00", "label": "11:00 PM" },
          { "value": "23:30", "label": "11:30 PM" },
        ];
        this.editMode = true;
        this.common.breadscrumChange('Protocol Calendar', 'Create Events', '');
      } else {
        this.timeList = [
          { "value": "24:00", "label": "12:00" + this.arabicfn('am') },
          { "value": "00:30", "label": "12:30" + this.arabicfn('am') },
          { "value": "01:00", "label": "01:00" + this.arabicfn('am') },
          { "value": "01:30", "label": "01:30" + this.arabicfn('am') },
          { "value": "02:00", "label": "02:00" + this.arabicfn('am') },
          { "value": "02:30", "label": "02:30" + this.arabicfn('am') },
          { "value": "03:00", "label": "03:00" + this.arabicfn('am') },
          { "value": "03:30", "label": "03:30" + this.arabicfn('am') },
          { "value": "04:00", "label": "04:00" + this.arabicfn('am') },
          { "value": "04:30", "label": "04:30" + this.arabicfn('am') },
          { "value": "05:00", "label": "05:00" + this.arabicfn('am') },
          { "value": "05:30", "label": "05:30" + this.arabicfn('am') },
          { "value": "06:00", "label": "06:00" + this.arabicfn('am') },
          { "value": "06:30", "label": "06:30" + this.arabicfn('am') },
          { "value": "07:00", "label": "07:00" + this.arabicfn('am') },
          { "value": "07:30", "label": "07:30" + this.arabicfn('am') },
          { "value": "08:00", "label": "08:00" + this.arabicfn('am') },
          { "value": "08:30", "label": "08:30" + this.arabicfn('am') },
          { "value": "09:00", "label": "09:00" + this.arabicfn('am') },
          { "value": "09:30", "label": "09:30" + this.arabicfn('am') },
          { "value": "10:00", "label": "10:00" + this.arabicfn('am') },
          { "value": "10:30", "label": "10:30" + this.arabicfn('am') },
          { "value": "11:00", "label": "11:00" + this.arabicfn('am') },
          { "value": "11:30", "label": "11:30" + this.arabicfn('am') },
          { "value": "12:00", "label": "12:00" + this.arabicfn('pm') },
          { "value": "12:30", "label": "12:30" + this.arabicfn('pm') },
          { "value": "13:00", "label": "01:00" + this.arabicfn('pm') },
          { "value": "13:30", "label": "01:30" + this.arabicfn('pm') },
          { "value": "14:00", "label": "02:00" + this.arabicfn('pm') },
          { "value": "14:30", "label": "02:30" + this.arabicfn('pm') },
          { "value": "15:00", "label": "03:00" + this.arabicfn('pm') },
          { "value": "15:30", "label": "03:30" + this.arabicfn('pm') },
          { "value": "16:00", "label": "04:00" + this.arabicfn('pm') },
          { "value": "16:30", "label": "04:30" + this.arabicfn('pm') },
          { "value": "17:00", "label": "05:00" + this.arabicfn('pm') },
          { "value": "17:30", "label": "05:30" + this.arabicfn('pm') },
          { "value": "18:00", "label": "06:00" + this.arabicfn('pm') },
          { "value": "18:30", "label": "06:30" + this.arabicfn('pm') },
          { "value": "19:00", "label": "07:00" + this.arabicfn('pm') },
          { "value": "19:30", "label": "07:30" + this.arabicfn('pm') },
          { "value": "20:00", "label": "08:00" + this.arabicfn('pm') },
          { "value": "20:30", "label": "08:30" + this.arabicfn('pm') },
          { "value": "21:00", "label": "09:00" + this.arabicfn('pm') },
          { "value": "21:30", "label": "09:30" + this.arabicfn('pm') },
          { "value": "22:00", "label": "10:00" + this.arabicfn('pm') },
          { "value": "22:30", "label": "10:30" + this.arabicfn('pm') },
          { "value": "23:00", "label": "11:00" + this.arabicfn('pm') },
          { "value": "23:30", "label": "11:30" + this.arabicfn('pm') },
        ];
        this.editMode = true;
        this.common.breadscrumChange('بروتوكول التقويم', 'إنشاء الأحداث', '');
      }
      this.actionButtonsControl();
    } else if (this.mode === 'view') {
      if (this.lang === 'en') {
        this.timeList = [
          { "value": "24:00", "label": "12:00 AM" },
          { "value": "00:30", "label": "12:30 AM" },
          { "value": "01:00", "label": "01:00 AM" },
          { "value": "01:30", "label": "01:30 AM" },
          { "value": "02:00", "label": "02:00 AM" },
          { "value": "02:30", "label": "02:30 AM" },
          { "value": "03:00", "label": "03:00 AM" },
          { "value": "03:30", "label": "03:30 AM" },
          { "value": "04:00", "label": "04:00 AM" },
          { "value": "04:30", "label": "04:30 AM" },
          { "value": "05:00", "label": "05:00 AM" },
          { "value": "05:30", "label": "05:30 AM" },
          { "value": "06:00", "label": "06:00 AM" },
          { "value": "06:30", "label": "06:30 AM" },
          { "value": "07:00", "label": "07:00 AM" },
          { "value": "07:30", "label": "07:30 AM" },
          { "value": "08:00", "label": "08:00 AM" },
          { "value": "08:30", "label": "08:30 AM" },
          { "value": "09:00", "label": "09:00 AM" },
          { "value": "09:30", "label": "09:30 AM" },
          { "value": "10:00", "label": "10:00 AM" },
          { "value": "10:30", "label": "10:30 AM" },
          { "value": "11:00", "label": "11:00 AM" },
          { "value": "11:30", "label": "11:30 AM" },
          { "value": "12:00", "label": "12:00 PM" },
          { "value": "12:30", "label": "12:30 PM" },
          { "value": "13:00", "label": "01:00 PM" },
          { "value": "13:30", "label": "01:30 PM" },
          { "value": "14:00", "label": "02:00 PM" },
          { "value": "14:30", "label": "02:30 PM" },
          { "value": "15:00", "label": "03:00 PM" },
          { "value": "15:30", "label": "03:30 PM" },
          { "value": "16:00", "label": "04:00 PM" },
          { "value": "16:30", "label": "04:30 PM" },
          { "value": "17:00", "label": "05:00 PM" },
          { "value": "17:30", "label": "05:30 PM" },
          { "value": "18:00", "label": "06:00 PM" },
          { "value": "18:30", "label": "06:30 PM" },
          { "value": "19:00", "label": "07:00 PM" },
          { "value": "19:30", "label": "07:30 PM" },
          { "value": "20:00", "label": "08:00 PM" },
          { "value": "20:30", "label": "08:30 PM" },
          { "value": "21:00", "label": "09:00 PM" },
          { "value": "21:30", "label": "09:30 PM" },
          { "value": "22:00", "label": "10:00 PM" },
          { "value": "22:30", "label": "10:30 PM" },
          { "value": "23:00", "label": "11:00 PM" },
          { "value": "23:30", "label": "11:30 PM" },
        ];
        this.editMode = false;
        this.common.breadscrumChange('Protocol Calendar', 'Events View', '');
      } else {
        this.timeList = [
          { "value": "24:00", "label": "12:00" + this.arabicfn('am') },
          { "value": "00:30", "label": "12:30" + this.arabicfn('am') },
          { "value": "01:00", "label": "01:00" + this.arabicfn('am') },
          { "value": "01:30", "label": "01:30" + this.arabicfn('am') },
          { "value": "02:00", "label": "02:00" + this.arabicfn('am') },
          { "value": "02:30", "label": "02:30" + this.arabicfn('am') },
          { "value": "03:00", "label": "03:00" + this.arabicfn('am') },
          { "value": "03:30", "label": "03:30" + this.arabicfn('am') },
          { "value": "04:00", "label": "04:00" + this.arabicfn('am') },
          { "value": "04:30", "label": "04:30" + this.arabicfn('am') },
          { "value": "05:00", "label": "05:00" + this.arabicfn('am') },
          { "value": "05:30", "label": "05:30" + this.arabicfn('am') },
          { "value": "06:00", "label": "06:00" + this.arabicfn('am') },
          { "value": "06:30", "label": "06:30" + this.arabicfn('am') },
          { "value": "07:00", "label": "07:00" + this.arabicfn('am') },
          { "value": "07:30", "label": "07:30" + this.arabicfn('am') },
          { "value": "08:00", "label": "08:00" + this.arabicfn('am') },
          { "value": "08:30", "label": "08:30" + this.arabicfn('am') },
          { "value": "09:00", "label": "09:00" + this.arabicfn('am') },
          { "value": "09:30", "label": "09:30" + this.arabicfn('am') },
          { "value": "10:00", "label": "10:00" + this.arabicfn('am') },
          { "value": "10:30", "label": "10:30" + this.arabicfn('am') },
          { "value": "11:00", "label": "11:00" + this.arabicfn('am') },
          { "value": "11:30", "label": "11:30" + this.arabicfn('am') },
          { "value": "12:00", "label": "12:00" + this.arabicfn('pm') },
          { "value": "12:30", "label": "12:30" + this.arabicfn('pm') },
          { "value": "13:00", "label": "01:00" + this.arabicfn('pm') },
          { "value": "13:30", "label": "01:30" + this.arabicfn('pm') },
          { "value": "14:00", "label": "02:00" + this.arabicfn('pm') },
          { "value": "14:30", "label": "02:30" + this.arabicfn('pm') },
          { "value": "15:00", "label": "03:00" + this.arabicfn('pm') },
          { "value": "15:30", "label": "03:30" + this.arabicfn('pm') },
          { "value": "16:00", "label": "04:00" + this.arabicfn('pm') },
          { "value": "16:30", "label": "04:30" + this.arabicfn('pm') },
          { "value": "17:00", "label": "05:00" + this.arabicfn('pm') },
          { "value": "17:30", "label": "05:30" + this.arabicfn('pm') },
          { "value": "18:00", "label": "06:00" + this.arabicfn('pm') },
          { "value": "18:30", "label": "06:30" + this.arabicfn('pm') },
          { "value": "19:00", "label": "07:00" + this.arabicfn('pm') },
          { "value": "19:30", "label": "07:30" + this.arabicfn('pm') },
          { "value": "20:00", "label": "08:00" + this.arabicfn('pm') },
          { "value": "20:30", "label": "08:30" + this.arabicfn('pm') },
          { "value": "21:00", "label": "09:00" + this.arabicfn('pm') },
          { "value": "21:30", "label": "09:30" + this.arabicfn('pm') },
          { "value": "22:00", "label": "10:00" + this.arabicfn('pm') },
          { "value": "22:30", "label": "10:30" + this.arabicfn('pm') },
          { "value": "23:00", "label": "11:00" + this.arabicfn('pm') },
          { "value": "23:30", "label": "11:30" + this.arabicfn('pm') },
        ];
        this.editMode = false;
        this.common.breadscrumChange('بروتوكول التقويم', this.arabicfn('viewevent'), '');
      }
    }
    if (this.calendarID) {
      this.viewEvent();
    }
    this.calendarManagementService.getById(0, this.currentUser.id).subscribe(
      (response: any) => {
        if (response != null) {
          this.statusList = response.M_LookupsList;
        }
      }
    );
    this.loadCityList();
    this.loadLocationList();
    this.loadUserList();
    this.bulkEvent = false;
    this.addMoreEvent = true;

    this.loadEventTypes();
    this.loadAllUserList();
    this.isApproverOrHead('');

    this.common.escalateModalClose$.subscribe((isEscalateModalClose) => {
      if (isEscalateModalClose == 'close') {
        this.inProgress = false;
      }
    });
    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if (newComment) {
        this.viewEvent();
      }
    });
  }

  loadCityList() {
    this.calendarManagementService.cityList(this.currentUser.id).subscribe(
      (response: any) => {
        this.cityList = response;
      }
    );
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  loadLocationList() {
    this.calendarManagementService.locationList(this.currentUser.id).subscribe(
      (response: any) => {
        this.locationList = response;
      }
    );
  }

  loadEventTypes() {
    this.calendarManagementService.eventTypes(this.currentUser.id).subscribe(
      (response: any) => {
        this.EventTypes = response;
      }
    );
  }

  loadUserList() {
    const params = [{
      'OrganizationID': 10,
      'OrganizationUnits': ''
    }];
    let userID = this.currentUser.id;
    if (this.mode == 'view') {
      userID = this.calendarForm.CreatedBy
    }
    this.common.getmemoUserList(params, userID).subscribe(
      (response: any) => {
        this.approverList = response;
      }
    );
  }

  loadAllUserList() {
    const params = [{
      'OrganizationID': '',
      'OrganizationUnits': ''
    }];
    this.common.getUserList(params, this.currentUser.id).subscribe(
      (response: any) => {
        this.userList = response;
      }
    );
  }

  onChecked() {
    if (this.calendarForm.AllDayEvents) {
      // this.calendarForm.DateFrom = null;
      // this.calendarForm.DateTo = null;
      this.calendarForm.StartTime = null;
      this.calendarForm.EndTime = null
    }
    this.validate();
  }

  validate() {
    this.valid = true;
    if (!this.calendarForm.EventRequestor || this.calendarForm.EventRequestor.trim() === ""
      || !this.calendarForm.EventType
      || !this.calendarForm.EventDetails || this.calendarForm.EventDetails.trim() === ""
      || (!this.calendarForm.DateFrom)
      || (!this.calendarForm.DateTo)
      || (!this.calendarForm.AllDayEvents && !this.calendarForm.StartTime)
      || (!this.calendarForm.AllDayEvents && !this.calendarForm.EndTime)
      || !this.calendarForm.Location
      || !this.calendarForm.City
      || !this.calendarForm.ApproverID) {
      this.valid = false;
    }
  }

  isCommentEmpty() {
    if (!this.calendarForm.Comments) {
      return true;
    }
    return false;
  }

  createEvent(isLastEventForBulk) {
    if (this.mode == 'create') {
      if (isLastEventForBulk == 'lastEventForBulk') {
        this.createEventforBulk();
      } else {
        this.validate();
        if (this.valid) {
          this.inProgress = true;
          this.calendarModel.EventRequestor = this.calendarForm.EventRequestor;
          this.calendarModel.EventType = parseInt(this.calendarForm.EventType);
          this.calendarModel.EventDetails = this.calendarForm.EventDetails;
          this.calendarModel.DateFrom = this.concatDateAndTime(this.calendarForm.DateFrom, this.calendarForm.StartTime);
          this.calendarModel.DateTo = this.concatDateAndTime(this.calendarForm.DateTo, this.calendarForm.EndTime);
          this.calendarModel.AllDayEvents = this.calendarForm.AllDayEvents;
          this.calendarModel.Location = this.calendarForm.Location;
          this.calendarModel.City = this.calendarForm.City;
          this.calendarModel.ApproverID = this.calendarForm.ApproverID;
          this.calendarModel.CreatedBy = this.currentUser.id;
          this.calendarModel.CreatedDateTime = new Date();
          this.calendarModel.Action = 'Submit';
          this.calendarModel.Comments = this.calendarForm.Comments;
          // this.calendarModel.ParentReferenceNumber = '';
          this.calendarManagementService.createEvent(this.calendarModel).subscribe(
            (response: any) => {
              if (response.CalendarID) {
                this.inProgress = false;
                if (this.common.currentLang == 'ar') {
                  this.message = this.arabic.words['calendarreqcreatemsg'];
                } else {
                  this.message = 'Event Request Submitted Successfully';
                }
                this.bsModalRef = this.modalService.show(SuccessComponent);
                this.bsModalRef.content.message = this.message;
                const newSubscriber = this.modalService.onHide.subscribe(() => {
                  newSubscriber.unsubscribe();
                  this.router.navigate(['/' + this.common.currentLang + '/app/media/calendar-management/list']);
                });
              }
            }
          );
        }
      }
    } else if (this.mode == 'view') {
      this.validate();
      if (this.valid) {
        this.inProgress = true;
        this.calendarModel.EventRequestor = this.calendarForm.EventRequestor;
        this.calendarModel.EventType = parseInt(this.calendarForm.EventType);
        this.calendarModel.EventDetails = this.calendarForm.EventDetails;
        this.calendarModel.DateFrom = this.concatDateAndTime(this.calendarForm.DateFrom, this.calendarForm.StartTime);
        this.calendarModel.DateTo = this.concatDateAndTime(this.calendarForm.DateTo, this.calendarForm.EndTime);
        this.calendarModel.AllDayEvents = this.calendarForm.AllDayEvents;
        this.calendarModel.Location = this.calendarForm.Location;
        this.calendarModel.City = this.calendarForm.City;
        this.calendarModel.ApproverID = this.calendarForm.ApproverID;
        this.calendarModel.UpdatedBy = this.currentUser.id;
        this.calendarModel.UpdatedDateTime = new Date();
        this.calendarModel.Action = 'Resubmit';
        this.calendarModel.Comments = this.calendarForm.Comments;
        this.calendarModel.ParentReferenceNumber = this.calendarForm.ParentReferenceNumber;
        this.calendarModel.CalendarID = this.calendarID;
        this.calendarManagementService.updateEvent(this.calendarModel).subscribe(
          (response: any) => {
            if (response.CalendarID) {
              this.inProgress = false;
              if (this.common.currentLang == 'ar') {
                this.message = this.arabic.words['calendarreqcreatemsg'];
              } else {
                this.message = 'Event Request Resubmitted Successfully';
              }
              this.bsModalRef = this.modalService.show(SuccessComponent);
              this.bsModalRef.content.message = this.message;
              const newSubscriber = this.modalService.onHide.subscribe(() => {
                newSubscriber.unsubscribe();
                if (this.calendarForm.ParentID) {
                  this.router.navigate(['/' + this.lang + '/app/media/calendar-management/event-list/' + this.calendarForm.ParentID]);
                } else {
                  this.router.navigate(['/' + this.lang + '/app/media/calendar-management/list']);
                }
              });
            }
          }
        );
      }
    }
  }

  fillBulkEventDetails(response: any) {
    this.validate();
    if (response) {
      this.clearBulkEventFields();
      this.calendarManagementService.getevent(response.CalendarID, this.currentUser.id).subscribe(
        (recentEvent: any) => {
          this.calendarForm.ReferenceID = recentEvent.ReferenceNumber;
          this.calendarModel.ReferenceNumber = recentEvent.ReferenceNumber;
          this.calendarForm.ApproverID = recentEvent.ApproverID;
          this.calendarModel.ParentReferenceNumber = recentEvent.ReferenceNumber;
          this.calendarModel.Status = recentEvent.Status;
          this.calendarForm.Status = recentEvent.Status;
          this.calendarForm.CreatedBy = recentEvent.CreatedBy;
        }
      );
      this.createEvent('lastEventForBulk');
    }
  }

  createEventforBulk() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      this.calendarModel.EventRequestor = this.calendarForm.EventRequestor;
      this.calendarModel.EventType = parseInt(this.calendarForm.EventType);
      this.calendarModel.EventDetails = this.calendarForm.EventDetails;
      this.calendarModel.DateFrom = this.concatDateAndTime(this.calendarForm.DateFrom, this.calendarForm.StartTime);
      this.calendarModel.DateTo = this.concatDateAndTime(this.calendarForm.DateTo, this.calendarForm.EndTime);
      this.calendarModel.AllDayEvents = this.calendarForm.AllDayEvents;
      this.calendarModel.Location = this.calendarForm.Location;
      this.calendarModel.City = this.calendarForm.City;
      this.calendarModel.ApproverID = this.calendarForm.ApproverID;
      this.calendarModel.CreatedBy = this.currentUser.id;
      this.calendarModel.CreatedDateTime = new Date();
      this.calendarModel.Action = 'Submit';
      this.calendarModel.Comments = this.calendarForm.Comments;
      // this.calendarModel.ParentReferenceNumber = '';
      this.calendarManagementService.createEvent(this.calendarModel).subscribe(
        (response: any) => {
          if (response.CalendarID) {
            this.inProgress = false;
            this.fillBulkEventDetails(response);
            this.bulkEvent = true;
            this.addMoreEvent = false;
            // this.message = 'Event Request Submitted Successfully';
            // this.bsModalRef = this.modalService.show(SuccessComponent);
            // this.bsModalRef.content.message = this.message;
            // const newSubscriber = this.modalService.onHide.subscribe(() => {
            //   newSubscriber.unsubscribe();
            //   this.fillBulkEventDetails(response);
            //   this.bulkEvent = true;
            //   this.addMoreEvent = false;
            // });
          }
        }
      );
    }
  }

  clearBulkEventFields() {
    this.calendarForm.EventRequestor = '';
    this.calendarForm.EventType = '';
    this.calendarForm.EventDetails = '';
    this.calendarForm.DateFrom = '';
    this.calendarForm.StartTime = '';
    this.calendarForm.AllDayEvents = false;
    this.calendarForm.DateTo = '';
    this.calendarForm.EndTime = '';
    this.calendarForm.Location = '';
    this.calendarForm.City = '';
  }

  concatDateAndTime(StartDate, StartTime) {
    if (StartDate) {
      const StartDateVal = this.datepipe.transform(StartDate, 'yyyy-MM-dd');
      let hh = new Date(StartDateVal).toJSON();
      if (StartTime) {
        var StartTime = StartTime.split(' ');
        StartTime = StartTime[0];
        hh = new Date(StartDateVal + ' ' + StartTime).toJSON();
      }
      return hh;
    }
  }

  viewEvent() {
    this.calendarManagementService.getevent(this.calendarID, this.currentUser.id).subscribe(
      (response: any) => {
        this.calendarForm.ReferenceID = response.ReferenceNumber;
        this.calendarForm.Status = response.Status;
        // this.CurrentStatus = response.Status;
        this.showOptions = false;
        this.calendarForm.CreatedBy = response.CreatedBy;
        this.calendarForm.EventRequestor = response.EventRequestor;
        this.calendarForm.EventType = response.EventType;
        this.calendarForm.EventDetails = response.EventDetails;
        this.calendarForm.DateFrom = response.DateFrom ? new Date(response.DateFrom) : null;
        this.calendarForm.DateTo = response.DateTo ? new Date(response.DateTo) : null;
        this.calendarForm.StartTime = this.formatAMPM(response.DateFrom);
        this.calendarForm.EndTime = this.formatAMPM(response.DateTo);
        this.calendarForm.AllDayEvents = response.AllDayEvents;
        this.calendarForm.Location = response.Location;
        this.calendarForm.City = response.City;
        this.calendarForm.ApproverID = response.ApproverID;
        this.calendarForm.ParentReferenceNumber = response.ParentReferenceNumber;
        this.calendarForm.ParentID = response.ParentID;
        // this.parentID = this.calendarForm.ParentID;
        this.isApprover = false;
        this.calendarManagementRequestComments = this.setCalendarRequestComments(response.CalendarCommunicationHistory);
        this.isApproverOrHead(response.CurrentApprover);
        this.actionButtonsControl();
      }
    );
  }

  isApproverOrHead(Approver) {
    if (Approver && Approver.findIndex(ca => ca.ApproverId == this.currentUser.id) > -1) {
      this.showOptions = true;
      this.isApprover = true;
    }
  }

  formatAMPM(date) {
    var time;
    var mins;
    var hours;
    if (date) {
      let timeSplit = date.split("T")[1];
      let isoMinutes = timeSplit.split(":")[1];
      let isoHours = timeSplit.split(":")[0];
      if (!(isoMinutes == '00' && isoHours == '00')) {
        mins = new Date(date).getMinutes();
        hours = new Date(date).getHours();
        mins = (parseInt(mins) % 60) < 10 ? '0' + (parseInt(mins) % 60) : (parseInt(mins) % 60);
        hours = (parseInt(hours) % 60) < 10 ? '0' + (parseInt(hours) % 60) : (parseInt(hours) % 60);
        time = hours + ":" + mins;
      } else {
        time = '';
      }
    } else {
      time = '';
    }
    return time;
  }

  minDate(days) {
    if (this.calendarForm.DateFrom) {
      var today = new Date(this.calendarForm.DateFrom);
      this.month = today.getMonth() + 1;
      if (this.month < 10) {
        var m = '0' + this.month;
      } else {
        m = this.month;
      }
      var dateLimit = (today.getFullYear()) + '/' + this.month + '/' + (today.getDate() + days);
      var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  maxDate(days) {
    if (this.calendarForm.DateTo) {
      var endDate = new Date(this.calendarForm.DateTo);
      this.month = endDate.getMonth();
      if (this.month < 10) {
        this.month = '0' + (endDate.getMonth() + 1);
      }
      var dateLimit = (endDate.getFullYear()) + '/' + this.month + '/' + (endDate.getDate() + days);
      var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  checkStartTime() {
    if (!this.calendarForm.DateFrom) {
      // this.Meeting.EndDateTime = '';
      return true;
    }
  }

  dateToStringFormation(date) {
    var day = date.getDate();
    if (day < 10) {
      day = '0' + day;
    }
    var month = date.getMonth() + 1;
    if (month < 10) {
      month = '0' + month;
    }
    var year = date.getFullYear();
    var formattedDate = year + '/' + month + '/' + day;
    return formattedDate;
  }

  splitHour(hour) {
    if (hour) {
      let Hour = hour.substring(0, 2);
      return Hour;
    }
  }

  splitMinutes(hour) {
    if (hour) {
      let Minutes = hour.substring(3, 5);
      return Minutes;
    }
  }

  onStartTimeSelect() {
    this.isMarkAsComplete();
    if (this.calendarForm.EndTime != '') {
      this.showValidTimeAlert = false;
      this.valid = false;
      this.onEndTimeSelect();
    }
  }

  isMarkAsComplete() {
    var currentDate = new Date().getTime();
    if ((new Date(this.calendarForm.DateFrom).getTime() < currentDate) || (new Date(this.calendarForm.DateTo).getTime() < currentDate)) {
      this.markAscompleted = true;
    } else {
      this.markAscompleted = false;
    }
    return this.markAscompleted;
  }

  onEndTimeSelect() {
    var startDate = new Date(this.calendarForm.DateFrom);
    var startDateString = this.dateToStringFormation(startDate);
    var endDate = new Date(this.calendarForm.DateTo);
    var endDateString = this.dateToStringFormation(endDate);
    var startTime = this.calendarForm.StartTime;
    var startHour = this.splitHour(startTime);
    var startMinutes = this.splitMinutes(startTime);
    var endTime = this.calendarForm.EndTime;
    var endHour = this.splitHour(endTime);
    var endMinutes = this.splitMinutes(endTime);
    if (startDateString == endDateString) {
      if (endHour < startHour) {
        this.showValidTimeAlert = true;
      } else if (endHour == startHour) {
        if (startMinutes > endMinutes) {
          this.showValidTimeAlert = true;
          this.valid = true;
        } else {
          this.showValidTimeAlert = false;
          this.valid = false;
        }
      } else {
        this.showValidTimeAlert = false;
        this.valid = false;
      }
    }
    this.validate();
  }

  checkStartEndDateDiff() {
    var StartDateTime = this.concatDateAndTime(this.calendarForm.DateFrom, this.calendarForm.StartTime);
    var EndDateTime = this.concatDateAndTime(this.calendarForm.DateTo, this.calendarForm.EndTime);
    var StartDate = this.datepipe.transform(this.calendarForm.DateFrom, 'yyyy-MM-dd');
    var EndDate = this.datepipe.transform(this.calendarForm.DateTo, 'yyyy-MM-dd');
    var flag = true;
    if ((StartDate <= EndDate)) {
      return false;
    } else if (((StartDate <= EndDate) && (StartDateTime <= EndDateTime))) {

    } else {
      return true;
    }
  }

  updateAction(action: string) {
    this.inProgress = true;
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "Replace"
      }, {
        "value": this.calendarForm.Comments,
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
    switch (action) {
      case 'Approve':
        this.message = 'Event Request Approved Successfully';
        if (this.common.currentLang == 'ar') {
          this.message = this.arabic.words['calendarreqapprovemsg'];
        }
        break;
      case 'Reject':
        this.message = 'Event Request Rejected Successfully';
        if (this.common.currentLang == 'ar') {
          this.message = this.arabic.words['calendarreqrejectmsg'];
        }
        break;
      case 'ReturnForInfo':
        this.message = 'Event Request Returned For Info successfully';
        if (this.common.currentLang == 'ar') {
          this.message = this.arabic.words['calendarreqreturnmsg'];
        }
        break;
      case 'Escalate':
        this.message = 'Event Request Escalated Successfully';
        if (this.common.currentLang == 'ar') {
          this.message = this.arabic.words['calendarreqescalatedmsg'];
        }
        break;
    }
    if (action != 'Escalate') {
      this.inProgress = true;
      this.calendarManagementService.update(this.calendarID, dataToUpdate)
        .subscribe((response: any) => {
          if (response.CalendarID) {
            if (action == 'Reject') {
              this.openRejectModal(dataToUpdate);
            } else {
              this.bsModalRef = this.modalService.show(SuccessComponent);
              this.bsModalRef.content.message = this.message;
              let newSubscriber = this.modalService.onHide.subscribe(() => {
                newSubscriber.unsubscribe();
                if (this.calendarForm.ParentID) {
                  this.router.navigate(['/' + this.lang + '/app/media/calendar-management/event-list/' + this.calendarForm.ParentID]);
                } else {
                  this.router.navigate(['/' + this.lang + '/app/media/calendar-management/list']);
                }
                this.inProgress = false;
              });
            }
          }
        });
    } else {
      this.inProgress = true;
      if (this.common.currentLang == 'en') {
        this.popupMsg = 'Event Requests Escalated Successfully';
      } else {
        this.popupMsg = this.arabic.words['calendarreqescalatedmsg'];
      }
      var initialState = {
        id: this.calendarID,
        ApproverID: null,
        ApiString: '/Calendar',
        message: this.popupMsg,
        ApiTitleString: 'Escalate',
        Title: 'Escalate',
        redirectPath: '/app/media/calendar-management/list',
        isBulkEvent: false,
        approverNode: 'ApproverID',
        isDepartmentNotNeeded: true,
        data: dataToUpdate,
        comments: this.calendarForm.Comments,
        isFirstApprover: this.isFirstApprover,
        departmentOrgID: 10
      };
      if (this.common.currentLang == 'ar') {
        initialState.message = this.arabic.words['calendarreqescalatedmsg'];
      }
      if (this.calendarForm.ParentID) {
        initialState.redirectPath = '/app/media/calendar-management/event-list/' + this.calendarForm.ParentID;
      }
      this.bsModalRef = this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
      let newSubscriber = this.modalService.onHide.subscribe(() => {
        newSubscriber.unsubscribe();
        this.inProgress = false;
      });
    }

  }

  actionButtonsControl() {
    if (this.mode == 'create') {
      this.submitButton = true;
    } else if (this.mode == 'view') {
      this.submitButton = false;
      this.ApproveButton = false;
      this.ReturnForInfoButton = false;
      this.RejectButton = false;
      this.EscalateButton = false;
      this.isFirstApprover = false;
      if (this.calendarForm.Status == 120 && this.isApprover) {
        this.ApproveButton = true;
        this.RejectButton = true;
        this.ReturnForInfoButton = true;
        this.EscalateButton = true;
        this.isFirstApprover = true;
      } else if (this.calendarForm.Status == 122 && this.currentUser.id == this.calendarForm.CreatedBy) {
        this.editMode = true;
        this.returnForInfo = true;
        this.submitButton = true;
        this.isApprover = true;
        this.validate();
      }
    }
  }



  MarkAsCompleted() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      this.calendarModel.EventRequestor = this.calendarForm.EventRequestor;
      this.calendarModel.EventType = parseInt(this.calendarForm.EventType);
      this.calendarModel.EventDetails = this.calendarForm.EventDetails;
      this.calendarModel.DateFrom = this.concatDateAndTime(this.calendarForm.DateFrom, this.calendarForm.StartTime);
      this.calendarModel.DateTo = this.concatDateAndTime(this.calendarForm.DateTo, this.calendarForm.EndTime);
      this.calendarModel.AllDayEvents = this.calendarForm.AllDayEvents;
      this.calendarModel.Location = this.calendarForm.Location;
      this.calendarModel.City = this.calendarForm.City;
      this.calendarModel.ApproverID = this.calendarForm.ApproverID;
      this.calendarModel.CreatedBy = this.currentUser.id;
      this.calendarModel.CreatedDateTime = new Date();
      this.calendarModel.Status = 124;
      this.calendarModel.Action = 'MarkAsCompleted';
      this.calendarModel.Comments = this.calendarForm.Comments;
      // this.calendarModel.ParentReferenceNumber = '';
      this.calendarManagementService.createEvent(this.calendarModel).subscribe(
        (response: any) => {
          if (response.CalendarID) {
            this.inProgress = false;
            if (this.common.currentLang == 'ar') {
              this.message = this.arabic.words['calendarreqmarkascompletemsg'];
            } else {
              this.message = 'Event Request Marked As Completed Successfully';
            }
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            const newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              if (this.calendarForm.ParentID) {
                this.router.navigate(['/' + this.lang + '/app/media/calendar-management/event-list/' + this.calendarForm.ParentID]);
              } else {
                this.router.navigate(['/' + this.lang + '/app/media/calendar-management/list']);
              }
            });
          }
        }
      );
    }
  }

  sendMessage() {
    if (this.calendarForm.Comments && (this.calendarForm.Comments.trim() != '')) {
      this.inProgress = true;
      let chatData: any = {
        Message: this.calendarForm.Comments,
        ParentCommunicationID: 0,
        CreatedBy: this.currentUser.id,
        CreatedDateTime: new Date(),
        CalendarID: this.calendarID
      };
      this.commentSectionService.sendComment('Calendar', chatData).subscribe((chatRes: any) => {
        this.commentSectionService.newCommentCreated(true);
        this.calendarForm.Comments = '';
        this.inProgress = false;
      });
    }
  }

  private setCalendarRequestComments(commentSectionArr: any, parentCommunicationID?: any) {
    let recursiveCommentsArr = [];
    if (!parentCommunicationID) {
      parentCommunicationID = null;
    }
    commentSectionArr.forEach((commObj: any) => {
      if (commObj.ParentCommunicationID == parentCommunicationID) {
        let replies: any = this.setCalendarRequestComments(commentSectionArr, commObj.CommunicationID);
        if (replies.length > 0) {
          replies.forEach(repObj => {
            repObj.hideReply = true;
          });
          commObj.Replies = replies;
        }
        commObj.Message = commObj.Comment;
        // commObj.UserProfileImg = 'assets/home/user_male.png';
        if (!commObj.Photo) {
          commObj.Photo = 'assets/home/user_male.png';
        }
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  openRejectModal(toSendData?: any) {
    this.inProgress = true;
    if (this.common.currentLang == 'en') {
      this.popupMsg = 'Event Requests Rejected Successfully';
    } else {
      this.popupMsg = this.arabic.words['calendarreqrejectmsg'];
    }
    var initialState = {
      id: this.calendarID,
      ApproverID: null,
      ApiString: '/Calendar',
      message: this.popupMsg,
      ApiTitleString: 'APOLOGIES',
      redirectUrl: '/app/media/calendar-management/list',
      isBulkEvent: false,
      data: toSendData
    };
    if (this.common.currentLang == 'ar') {
      initialState.message = this.arabic.words['calendarreqrejectmsg'];
    }
    if (this.calendarForm.ParentID) {
      initialState.redirectUrl = '/app/media/calendar-management/event-list/' + this.calendarForm.ParentID;
    }
    this.bsModalRef = this.modalService.show(ApologiesModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }
}
