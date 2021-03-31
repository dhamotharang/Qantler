import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import 'tinymce';
import { TaskEvent } from '../../../service/task.event';
import { Router, ActivatedRoute } from '@angular/router';
import { createTask } from './modal';
import { masterData } from './masterData';
import { CommonService } from 'src/app/common.service';
import { TaskService } from '../../../service/task.service';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { HttpEventType } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { LinkToModalComponent } from 'src/app/task/modal/link-to-modal/link-to-modal.component';
import { environment } from 'src/environments/environment';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { AssignModalComponent } from 'src/app/modal/assignmodal/assignmodal.component';
declare var tinymce: any;

@Component({
  selector: 'app-create-task',
  templateUrl: './create-task.component.html',
  styleUrls: ['./create-task.component.scss']
})

export class CreateTaskComponent implements OnInit, OnDestroy {
  interval: any;
  linkMemos: any = [];
  linkMemos_list: any = [];
  linkLetter: any = [];
  canEdit = false;
  linkLetter_list: any = [];
  Ismemolink: boolean = false;
  Isletterlink: boolean = false;
  visibleChat: boolean = false;
  currentId: any;
  language: any;
  departments: any = []
  assigneeEdit: boolean = false;
  linkMeetings_list: any;
  linkMeetings: any = [];
  IsMeetinglink: boolean;
  linkMeeting_list: any = [];
  DeleteFlag: any = false;
  selectedUsersDepartment: any;
  screenTitle: any;
  tableMessages: { emptyMessage: any; };
  ngOnDestroy() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }
  tinyConfig = {
    plugins: 'powerpaste casechange importcss tinydrive searchreplace directionality visualblocks visualchars fullscreen table charmap hr pagebreak nonbreaking toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons',
    language: this.common.language != 'English' ? "ar" : "en",
    menubar: 'file edit view insert format tools table tc help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media pageembed template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment',
    toolbar_drawer: 'sliding',
    directionality: this.common.language != 'English' ? "rtl" : "ltr"
  };
  @ViewChild('attachment') attachmentInput: ElementRef;
  @ViewChild('tinyDetail') tinyDetail: ElementRef;

  taskDetails = '';
  createTask: createTask = new createTask;
  masterData: masterData = new masterData;
  bsModalRef: BsModalRef;
  chatIconShow = false;
  userList: any = [];
  department = this.masterData.department;
  viewData = this.masterData.viewData;
  assUserList: Object;
  resUserList: Object;
  priorityList: string[];
  memoList: { LinktoMemos: string; }[];
  letterList: { LinktoLetter: string; }[];
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  uploadPercentage = 0;
  uploadProcess: boolean = false;
  screenStatus: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  url: string | ArrayBuffer;
  tagUserList: any = [];
  attachments: any = [];
  AttachmentDownloadUrl = environment.AttachmentDownloadUrl;
  deleteBtnLoad = false;
  closeBtnLoad = false;
  completeBtnLoad = false;
  submitBtnLoad = false;
  assignBtnLoad = false;
  curStatus = true;
  message: string;
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  statusList: any;
  letter = [];
  countryList: any;
  defaultCountry: any;
  emiratesList: any;
  cityList: any;
  viewMode: boolean = false;

  validateStartEndDate: any = {
    isValid: true,
    msg: ''
  };

  constructor(
    public taskEvent: TaskEvent,
    public router: Router,
    public route: ActivatedRoute,
    public common: CommonService,
    public datepipe: DatePipe,
    public taskService: TaskService,
    public util: UtilsService,
    public modalService: BsModalService) {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.loadData(0, this.currentUser.id);
    this.createTask.Attachments = [];
    this.createTask.ResponsibleDepartmentId = [];
    this.createTask.ResponsibleUserId = [];
    this.createTask.AssigneeDepartmentId = this.currentUser.DepartmentID;
    this.Ass_dept_change();
    this.createTask.Labels = [];
    this.language = this.common.language;
    if (this.common.language == 'English') {
      this.priorityList = ['High', 'Medium', 'Low', 'VeryLow'];
    } else {
      this.priorityList = [this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
    }
    this.getLetterList();
    this.getMemoList();
  }

  ngOnInit() {
    this.common.triggerScrollTo('trigger-scroll');
    if (this.common.language == 'English')
      this.taskService.breadscrumChange(1, 'Task Create', 0);
    else
      this.taskService.breadscrumChange(1, 'Task Create', 0, 'ar');
    this.getUserList([]);
    this.route.url.subscribe(() => {
      this.screenStatus = this.route.snapshot.data.title;
    });
    switch (this.screenStatus) {
      case "Task Create":
        this.screenTitle = (this.common.language == 'English') ? "CREATION" : this.arabic('creation');
        break;
      case "Task Edit":
        this.screenTitle = (this.common.language == 'English') ? "EDIT" : this.arabic('edit');
        break;
      case "Task View":
        this.screenTitle = (this.common.language == 'English') ? "VIEW" : this.arabic('view');
        break;
    }
    this.route.params.subscribe(param => {
      if (param.id) {
        var id = +param.id;
        this.viewMode = true;
        if (id > 0)
          this.loadData(id, this.currentUser.id);
        if (this.common.language == 'English')
          this.taskService.breadscrumChange(1, this.screenStatus, id);
        else
          this.taskService.breadscrumChange(1, this.screenStatus, id, 'ar');
      } else if (param.letterNo) {
        let check_letter = param.letterNo;
        let link = check_letter.substring(check_letter.length - 3, check_letter.length);
        if (link != 'MTG') {
          this.letter.push({ 'display': param.letterNo, 'value': param.letterNo });
          this.onAddChangeLetter(this.letter[0]);
          this.createTask.LinkToLetter = this.letter;
        } else {
          this.letter.push({ 'display': param.letterNo, 'value': param.letterNo });
          this.onAddChangeMeeting(this.letter[0]);
          this.createTask.LinkToMeeting = this.letter;
        }
      }
    });

    this.taskService.getCountries(this.currentUser.id)
      .subscribe(countries => {
        this.countryList = countries;
        if (this.countryList && this.countryList.length) {
          this.defaultCountry = this.countryList[0].CountryID;
          if (!this.viewMode) {
            this.createTask.Country = this.countryList[0].CountryID;
            // this.loadCities(this.currentUser.id, this.createTask.Country);
          }
        }
      });

    this.taskService.getEmiratesList(this.currentUser.id)
      .subscribe(emirates => {
        this.emiratesList = emirates;
      });

    // this.interval = setInterval(() => {
    //   if (this.createTask.TaskID)
    //     this.loadChatList();
    // }, 5000);
  }

  onCountrySelected() {
    this.createTask.City = '';
    // if (this.createTask.Country == this.defaultCountry) {
    //   this.loadCities(this.currentUser.id, this.createTask.Country);
    // } else {
    //   this.createTask.City = '';
    // }
  }

  onEmiratesSelected() {
    this.createTask.City = '';
    this.loadCities(this.currentUser.id, this.createTask.Emirates);
  }

  loadCities(userId, emirateId) {
    this.taskService.getCities(userId, emirateId)
      .subscribe(cities => {
        this.cityList = cities;
      });
  }


  async loadData(id, user_id) {
    this.currentId = id;
    var calendar_id = environment.calendar_id;
    await this.taskService.viewTask('DutyTask', id, user_id).subscribe((data: any) => {
      this.taskEvent.chatReload(data.communiationHistory);
      if (id > 0)
        this.setViewData(data);
      else
        this.department = data.M_OrganizationList.filter(res => calendar_id != res.OrganizationID);
      this.departments = data.M_OrganizationList;
    });
  }

  chatOpen() {
    var param = {
      chatDet: this.createTask,
      tagUserList: this.tagUserList
    }
    // setTimeout(() => {
    //   // this.taskEvent.chatdata(param);
    //   this.taskEvent.chatReload(this.createTask.communiationHistory);
    // }, 100);
    this.router.navigateByUrl('chat', { state: { taskDetails: this.createTask, tagUserList: this.tagUserList } });

  }

  Ass_dept_change() {
    this.createTask.AssigneeUserId = 0;
    var deptId = this.createTask.AssigneeDepartmentId;
    let userId = (this.screenStatus == 'Task Create') ? this.currentUser.id : this.createTask.CreatedBy;
    //let userId = this.currentUser.id;
    var param = [{
      "OrganizationID": deptId,
      "OrganizationUnits": "string"
    }];
    if (deptId) {
      this.common.getUserList(param, 0).subscribe(list => {
        this.assUserList = list;
      });
    } else {
      this.assUserList = [];
    }
  }

  Res_dept_change() {
    //this.createTask.ResponsibleUserId = 0;
    // if (this.screenStatus != 'Task View') {
    //   this.checkDepartmentUser();
    // }
    var deptId = this.createTask.ResponsibleDepartmentId;
    let userId = (this.screenStatus == 'Task Create') ? this.currentUser.id : this.createTask.CreatedBy;
    //let userId = this.currentUser.id;
    var param = [];
    if (deptId.length) {
      deptId.forEach(element => {
        param.push({ "OrganizationID": (element.TaskResponsibleDepartmentID) ? element.TaskResponsibleDepartmentID : element, "OrganizationUnits": "string" });
      });
    }
    if (param.length > 0) {
      this.common.getUserList(param, 0).subscribe(list => {
        this.resUserList = list;
      });
    } else {
      this.resUserList = [];
    }
  }

  async getUserList(data) {
    let userId = (this.screenStatus == 'Task Create') ? this.currentUser.id : 0;
    await this.common.getUserList(data, userId).subscribe(list => {
      this.userList = list;
      this.userList.map(res => {
        if (res.UserID == this.createTask.AssigneeUserId)
          this.tagUserList.push({ 'id': res.UserID, 'name': res.EmployeeName });
      });
    });
  }

  // open link to modal
  openLinkToModal(type: string) {
    const initialState = {
      type,
    };
    this.bsModalRef = this.modalService.show(LinkToModalComponent, { initialState, class: 'modal-lg' });
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      let seletedIds = this.bsModalRef.content.selectedIds;
      if (seletedIds && seletedIds.length) {
        const value = seletedIds.join();
        if (type === 'memo') {
          this.onAddChangeMemo({ value });
        } else if (type === 'letter') {
          this.onAddChangeLetter({ value });
        } else if (type === 'meeting') {
          this.onAddChangeMeeting({ value });
        }
        this.bsModalRef.content.selectedIds = [];
      }
    });
  }

  getMemoList() {
    this.taskService.getDatabyId('DutyTask/Memos', this.currentUser.id).subscribe((res: any) => {
      this.memoList = res;
    });
  }

  getLetterList() {
    this.taskService.getDatabyId('DutyTask/Letters', this.currentUser.id).subscribe((res: any) => {
      this.letterList = res;
    });
  }

  Attachments(event) {
    var files = event.target.files;
    if (files.length > 0) {
      let that = this;
      this.uploadProcess = true;
      this.common.postAttachment(files).subscribe((event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.attachmentInput.nativeElement.value = "";
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.createTask.Attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'MemoID': '' });
          }
          this.createTask.Attachments = this.createTask.Attachments;
        }
      });
    }
  }

  deleteAttachment(index) {
    this.createTask.Attachments.splice(index, 1);
    this.attachmentInput.nativeElement.value = "";
  }

  onTextChange(event) {
    if (event != '')
      this.createTask.Labels.push({ display: event, value: event });
  }

  validateForm() {
    var flag = true;
    var ResponsibleDepartmentId = (this.createTask.ResponsibleDepartmentId) ? (this.createTask.ResponsibleDepartmentId.length > 0) : false;
    var ResponsibleUserId = (this.createTask.ResponsibleUserId) ? (this.createTask.ResponsibleUserId.length > 0) : false;
    //var city = (this.createTask.Country != this.defaultCountry) ? (this.createTask.City.trim()) : this.createTask.City;
    if (this.createTask.StartDate &&
      this.createTask.AssigneeDepartmentId &&
      this.createTask.AssigneeUserId && !this.submitBtnLoad &&
      // ResponsibleDepartmentId && ResponsibleUserId &&
      this.createTask.Country && this.createTask.City) {
      flag = false;
      if (this.createTask.Country != this.defaultCountry) {
        if (this.createTask.City)
          flag = false;
        else
          flag = true;
      }
      if (this.createTask.Country == this.defaultCountry && !this.createTask.Emirates) {
        flag = true;
      }
    }

    // if (this.createTask.EndDate && this.createTask.StartDate) {
    //   if (this.taskService.dateValidate(this.createTask.StartDate, this.createTask.EndDate)) {
    //     return
    //   } else {
    //     this.createTask.EndDate = '';
    //   }
    // }

    return flag;
  }

  dateFormater(date) {
    return this.datepipe.transform(date, 'dd-MM-Y');
  }

  async setViewData(data) {
    this.statusList = data.M_LookupsList;
    this.DeleteFlag = data.DeleteFlag;
    this.createTask.AssigneeDepartmentId = data.AssigneeDepartmentId;
    this.createTask.Country = data.Country;
    this.createTask.Emirates = data.Emirates;
    this.createTask.City = parseInt(data.City) ? parseInt(data.City) : data.City;
    if (parseInt(data.City)) {
      this.loadCities(this.currentUser.id, this.createTask.Emirates);
    }
    var res_department = [],
      res_user = [];
    data.ResponsibleDepartmentId.forEach(element => {
      res_department.push(element.TaskResponsibleDepartmentID);
    });
    data.ResponsibleUserId.forEach(element => {
      res_user.push(element.TaskResponsibleUsersID);
    });
    data.TagList.forEach(item => {
      this.tagUserList.push({
        'id': item.TaskResponsibleUsersID,
        'name': item.TaskResponsibleUsersName
      });
    });
    data.Labels.map(element => {
      this.createTask.Labels.push({ 'display': element.labels, 'value': element.labels });
    });
    var memo = [],
      letter = [],
      meeting = [];
    data.LinkToMemo.map(element => {
      memo.push({ 'display': element.MemoReferenceNumber, 'value': element.MemoReferenceNumber });
    });
    data.LinkToLetter.map(element => {
      letter.push({ 'display': element.LetterReferenceNumber, 'value': element.LetterReferenceNumber });
    });
    data.LinkToMeeting.map(element => {
      meeting.push({ 'display': element.MeetingReferenceNumber, 'value': element.MeetingReferenceNumber });
    });
    this.linkMemos_list = data.LinkToMemo;
    this.linkLetter_list = data.LinkToLetter;
    this.linkMeeting_list = data.LinkToMeeting;
    this.createTask.LinkToMemo = memo;
    this.createTask.LinkToLetter = letter;
    this.createTask.LinkToMeeting = meeting;
    this.createTask.CreatedBy = data.CreatedBy;
    this.createTask.ResponsibleDepartmentId = res_department;
    await this.Res_dept_change();
    await this.Ass_dept_change();
    this.createTask.ResponsibleUserId = res_user;
    this.createTask.AssigneeUserId = data.AssigneeUserId;
    this.createTask.TaskID = data.TaskID;
    this.createTask.TaskReferenceNumber = data.TaskReferenceNumber;
    this.createTask.Action = data.Status;
    this.createTask.Status = data.Status;
    this.createTask.CreatedBy = data.CreatedBy;
    this.createTask.CreatedDateTime = new Date(data.CreatedDateTime);
    this.createTask.SourceOU = this.departments.find(x => x.OrganizationID == data.SourceOU).OrganizationUnits;
    this.getSouceName(data.SourceName, data.SourceOU);
    this.createTask.Title = data.Title;
    this.createTask.StartDate = new Date(data.StartDate);
    this.createTask.EndDate = (data.EndDate) ? new Date(data.EndDate) : '';
    this.createTask.TaskDetails = data.TaskDetails;
    if (data.Priority >= 0)
      this.createTask.Priority = this.priorityList[data.Priority].toString();
    this.createTask.RemindMeAt = (data.RemindMeAt) ? new Date(data.RemindMeAt) : '';
    this.createTask.Attachments = data.Attachments;
    this.createTask.DelegateAssignee = data.DelegateAssignee;
    this.createTask.communiationHistory = data.communiationHistory;
    this.chatIconShow = true;

    var param = {
      chatDet: this.createTask,
      tagUserList: this.tagUserList
    }
    if (this.createTask.AssigneeUserId == this.currentUser.id || this.createTask.DelegateAssignee == this.currentUser.id) {
      if (this.createTask.Status == 30)
        this.assigneeEdit = true;
    }
    if ((this.createTask.Status == 30 && this.createTask.CreatedBy == this.currentUser.id)) {
      this.canEdit = false;
    } else {
      this.canEdit = true;
    }
    this.visibleChat = this.chatUsers(this.createTask);
    this.taskService.visibleChat = this.visibleChat;
    if (this.common.language == 'English')
      this.taskService.breadscrumChange(this.createTask.Status, this.screenStatus, this.currentId);
    else
      this.taskService.breadscrumChange(this.createTask.Status, this.screenStatus, this.currentId, 'ar');
    setTimeout(() => {
      this.taskEvent.chatdata(param);
    }, 100);
    this.curStatus = (this.createTask.Status == 32 || this.createTask.Status == 31 || this.createTask.Status == 193) ? true : false;
    if (this.tinyDetail.nativeElement)
      this.tinyDetail.nativeElement.insertAdjacentHTML('beforeend', data.TaskDetails);
  }

  prepareData() {
    var labels = this.createTask.Labels,
      resUser = this.createTask.ResponsibleUserId,
      resDept = this.createTask.ResponsibleDepartmentId;
    this.createTask.Labels = [];
    this.createTask.ResponsibleUserId = [];
    this.createTask.ResponsibleDepartmentId = [];
    labels.map((res: any) => {
      this.createTask.Labels.push({ "labels": res.value });
    });

    resUser.map((res: any) => {
      this.createTask.ResponsibleUserId.push({ "TaskResponsibleUsersId": res, "TaskResponsibleUsersName": '' });
    });

    resDept.map((res: any) => {
      this.createTask.ResponsibleDepartmentId.push({ "TaskResponsibleDepartmentId": res, "TaskResponsibleDepartmentName": '' });
    });

    this.createTask.SourceOU = this.currentUser.DepartmentID;
    this.createTask.SourceName = this.currentUser.UserID;
    this.createTask.Action = "Submit";
    this.createTask.Priority = this.priorityList.indexOf(this.createTask.Priority).toString();
    if (this.createTask.TaskID && this.createTask.TaskID > 0) {
      this.createTask.UpdatedBy = this.currentUser.id;
      this.createTask.UpdatedDateTime = new Date;
    } else {
      this.createTask.CreatedBy = this.currentUser.id;
      this.createTask.CreatedDateTime = new Date;
    }
    var linkMemo = [],
      linkLetter = [],
      linkMeeting = [];
    if (this.createTask.LinkToMemo) {
      this.createTask.LinkToMemo.forEach((element: any) => {
        linkMemo.push({ 'MemoReferenceNumber': element.value });
      });
    }
    if (this.createTask.LinkToLetter) {
      this.createTask.LinkToLetter.forEach((element: any) => {
        linkLetter.push({ 'LetterReferenceNumber': element.value });
      });
    }
    if (this.createTask.LinkToMeeting) {
      this.createTask.LinkToMeeting.forEach((element: any) => {
        linkMeeting.push({ 'MeetingReferenceNumber': element.value });
      });
    }
    this.createTask.LinkToMemo = linkMemo;
    this.createTask.LinkToLetter = linkLetter;
    this.createTask.LinkToMeeting = linkMeeting;
    this.linkLetter = this.createTask.LinkToLetter;
    this.linkMemos = this.createTask.LinkToMemo;
    this.createTask.Attachments = this.createTask.Attachments;
    return this.createTask;
  }

  chatUsers(data) {
    var flag = false, response_user = false, assign_user = false, cur_user = false;
    data.ResponsibleUserId.forEach(el => {
      if (el == this.currentUser.id) {
        response_user = true;
      }
    });
    if (data.AssigneeUserId == this.currentUser.id) {
      assign_user = true;
    }
    if (data.CreatedBy == this.currentUser.id) {
      cur_user = true;
    }
    if (cur_user || assign_user || response_user) {
      flag = true;
    }
    return flag;
  }

  // minDate(days) {
  //   var minDate = new Date();
  //   //this.maxDate = new Date();
  //   //return minDate.setDate(minDate.getDate() - 1);
  //   // this.minDate.setDate(this.minDate.getDate() - 1);
  //   // this.maxDate.setDate(this.maxDate.getDate() + 7);
  //   // if (this.createTask.StartDate)
  //   //   return this.createTask.StartDate.getDate();
  //   if (this.createTask.StartDate) {
  //     var today = new Date(this.createTask.StartDate);
  //     var month: any = today.getMonth() + 1;
  //     // if (today.getMonth() < 10) {
  //     //   month = '0' + (today.getMonth() + 1);
  //     // }
  //     var dateLimit = (today.getFullYear()) + '/' + month + '/' + (today.getDate() + days);
  //     var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
  //     return new Date(dates);
  //   }
  // }

  maxDate(days) {
    if (this.createTask.EndDate) {
      var endDate = new Date(this.createTask.EndDate);
      var month: any = endDate.getMonth() + 1;
      // if (month < 10) {
      //   month = '0' + (endDate.getMonth() + 1);
      // }
      var dateLimit = (endDate.getFullYear()) + '/' + month + '/' + (endDate.getDate() + days);
      var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }


  saveTask() {
    var param = this.prepareData();
    this.taskService.saveTask('DutyTask', param).subscribe(res => {
      this.submitBtnLoad = false;
      this.message = (this.common.language == 'English') ? "Task Submitted Successfully" : this.arabic('tasksubmittedsuccessfully');
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.screenStatus = this.screenStatus;
      this.bsModalRef.content.pagename = 'Task';
    });
  }

  viewTask(id) {
    this.taskService.viewTask('DutyTask', id, this.currentUser.id).subscribe((res: any) => {
      this.taskEvent.chatReload(res.communiationHistory);
      this.setViewData(res);
    });
  }

  formatPatchData(value, path) {
    var data = [
      {
        "value": value,
        "path": path,
        "op": "replace",
      },
      {
        "value": (this.createTask.Comments) ? this.createTask.Comments : '',
        "path": "Comments",
        "op": "replace",
      },
      {
        "value": this.currentUser.id,
        "path": "UpdatedBy",
        "op": "replace",
      },
      {
        "value": new Date(),
        "path": "UpdatedDateTime",
        "op": "replace",
      }
    ];
    return data;
  }

  async completeTask() {
    var path = "Action",
      value = "Completed";
    var param = this.formatPatchData(value, path);
    this.taskService.patchTask('DutyTask', param, this.createTask.TaskID).subscribe(res => {
      this.completeBtnLoad = false;
      this.message = (this.common.language == 'English') ? "Task Completed Successfully" : this.arabic('taskcompletedsuccessfully');
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.screenStatus = this.screenStatus;
      this.bsModalRef.content.pagename = 'Task';
    });
  }

  closeTask() {

    var path = "Action",
      value = "Close";
    var param = this.formatPatchData(value, path);
    this.taskService.patchTask('DutyTask', param, this.createTask.TaskID).subscribe(res => {
      this.deleteBtnLoad = false;
      this.closeBtnLoad = false;
      this.message = (this.common.language == 'English') ? "Task Closed Successfully" : this.arabic('taskclosedsuccessfully');
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.screenStatus = this.screenStatus;
      this.bsModalRef.content.pagename = 'Task';
    });
  }

  assignTask() {
    this.bsModalRef = this.modalService.show(AssignModalComponent, this.config);
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.fromScreen = 'Dutytask';
    this.bsModalRef.content.page = 'Dutytask';
    this.bsModalRef.content.ActionTaken = (this.createTask.Comments) ? this.createTask.Comments : '';
    this.bsModalRef.content.TaskID = this.createTask.TaskID;
  }


  deleteTask() {
    this.taskService.deleteTask('DutyTask', this.createTask.TaskID, this.currentUser.id).subscribe(res => {
      this.deleteBtnLoad = false;
      this.closeBtnLoad = false;
      this.message = (this.common.language == 'English') ? "Task Deleted Successfully" : this.arabic('taskdeletedsuccessfully');
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.screenStatus = this.screenStatus;
      this.bsModalRef.content.pagename = 'Task';
    });
  }

  loadChatList() {
    this.taskService.viewTask('DutyTask', this.createTask.TaskID, this.currentUser.id).subscribe((res: any) => {
      this.taskEvent.chatUpdateCall(res.communiationHistory);
    });
  }

  // link to memo
  onAddChangeMemo(event) {
    if (event != '') {
      this.taskService.getLetters('DutyTask/Memos', this.currentUser.id, event.value).subscribe((data: any) => {
        // var refId = event.value.split(',');
        // refId.map(res => {
        if (data && data.length) {
          data.forEach(item => {
            if (!this.checkLinkList(this.linkMemos, item.MemoReferenceNumber)) {
              this.Ismemolink = true;
              setTimeout(() => {
                this.Ismemolink = false;
              }, 7000);
              return;
            }
            this.linkMemos.push({
              display: item.MemoReferenceNumber,
              value: item.MemoReferenceNumber
            });
            this.linkMemos_list.push(item);
          });
          this.createTask.LinkToMemo = this.linkMemos;
        } else {
          this.Ismemolink = true;
          setTimeout(() => {
            this.Ismemolink = false;
          }, 7000);
          this.createTask.LinkToMemo = this.linkMemos;
        }
        // });
      });
    }
  }
  onRemoveChangeMemo(event) {
    for (var i = 0; i < this.linkMemos.length; i++) {
      if (this.linkMemos[i].value == event.value) {
        this.linkMemos.splice(i, 1);
      }
    }
    for (var i = 0; i < this.linkMemos_list.length; i++) {
      if (this.linkMemos_list[i].MemoReferenceNumber.toLowerCase() == event.value.toLowerCase()) {
        this.linkMemos_list.splice(i, 1);
      }
    }
  }
  // end of link to memo

  // link to letters
  onAddChangeLetter(event) {
    if (event != '') {
      this.taskService.getLetters('DutyTask/Letters', this.currentUser.id, event.value).subscribe((data: any) => {
        if (data && data.length) {
          data.forEach(item => {
            if (!this.checkLinkList(this.linkLetter, item.LetterReferenceNumber)) {
              this.Isletterlink = true;
              setTimeout(() => {
                this.Isletterlink = false;
              }, 6000);
              return;
            }
            this.linkLetter.push({
              display: item.LetterReferenceNumber,
              value: item.LetterReferenceNumber
            });
            this.linkLetter_list.push(item);
          });
          this.createTask.LinkToLetter = this.linkLetter
        } else {
          this.Isletterlink = true;
          setTimeout(() => {
            this.Isletterlink = false;
          }, 6000);
          this.createTask.LinkToLetter = this.linkLetter
        }
      });
    }
  }
  onRemoveChangeLetter(event) {
    for (var i = 0; i < this.linkLetter.length; i++) {
      if (this.linkLetter[i].value == event.value) {
        this.linkLetter.splice(i, 1);
      }
    }
    for (var i = 0; i < this.linkLetter_list.length; i++) {
      if (this.linkLetter_list[i].LetterReferenceNumber.toLowerCase() == event.value.toLowerCase()) {
        this.linkLetter_list.splice(i, 1);
      }
    }
  }
  // end of link to letters

  // link to meeting
  onAddChangeMeeting(event) {
    if (event != '') {
      this.taskService.getLetters('DutyTask/Meetings', this.currentUser.id, event.value).subscribe((data: any) => {
        // var refId = event.split(',');
        // refId.map(res => {
        if (data && data.length) {
          data.forEach(item => {
            if (!this.checkLinkList(this.linkMeetings, item.MeetingReferenceNumber)) {
              this.IsMeetinglink = true;
              setTimeout(() => {
                this.IsMeetinglink = false;
              }, 7000);
              return;
            }
            this.linkMeetings.push({
              display: item.MeetingReferenceNumber,
              value: item.MeetingReferenceNumber
            });
            this.linkMeeting_list.push(item);
          });
          this.createTask.LinkToMeeting = this.linkMeetings;
        } else {
          this.IsMeetinglink = true;
          setTimeout(() => {
            this.IsMeetinglink = false;
          }, 7000);
          this.createTask.LinkToMeeting = this.linkMeetings;
        }
        // });
      });
    }
  }
  onRemoveChangeMeeting(event) {
    for (var i = 0; i < this.linkMeetings.length; i++) {
      if (this.linkMeetings[i].value == event.value) {
        this.linkMeetings.splice(i, 1);
      }
    }
    for (var i = 0; i < this.linkMeeting_list.length; i++) {
      if (this.linkMeeting_list[i].MeetingReferenceNumber.toLowerCase() == event.value.toLowerCase()) {
        this.linkMeeting_list.splice(i, 1);
      }
    }
  }
  // end of link to meeting

  async getSouceName(UserID, DepID) {
    let params = [{
      "OrganizationID": DepID,
      "OrganizationUnits": "string"
    }];
    this.common.getUserList(params, 0).subscribe((data: any) => {
      let Users = data;
      this.createTask.SourceName = Users.find(x => x.UserID == UserID).EmployeeName.toString();
    });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  Res_user_change(event) {
    this.selectedUsersDepartment = event;
  }

  checkDepartmentUser(event) {
    if (this.selectedUsersDepartment.length > 0) {
      var selected = this.selectedUsersDepartment;
      this.createTask.ResponsibleUserId = [];
      this.selectedUsersDepartment = [];
      this.selectedUsersDepartment = selected.filter(res => res.DepartmentID != event.value.OrganizationID);
      this.selectedUsersDepartment.map(user => {
        this.createTask.ResponsibleUserId.push(user.UserID);
      });
      //this.createTask.ResponsibleUserId = this.createTask.ResponsibleUserId;
    }
  }

  // startdateChange(){
  //   if(this.createTask.EndDate && this.createTask.StartDate){
  //     if(this.taskService.dateValidate(this.createTask.StartDate,this.createTask.EndDate)){
  //       return
  //     }else{
  //       this.createTask.EndDate = '';
  //     }
  //   }
  // }

  checkLinkList(data, id) {
    var checkdata = true;
    if (data.length > 0)
      checkdata = data.find(res => res.display == id) ? false : true;
    return checkdata;
  }

  getRefLink(data, component) {
    return this.util.genarateLinkUrl(component, data);
  }

  minDate(date) {
    return this.util.minDateCheck(date);
  }


  dateValidation() {
    this.validateStartEndDate.msg = '';
    let showDateValidationMsg = false;
    if (!this.createTask.StartDate && this.createTask.EndDate) {
      showDateValidationMsg = false;
    } else if (this.createTask.StartDate && this.createTask.EndDate) {
      let startDate = new Date(this.createTask.StartDate.setHours(0,0,0,0)).getTime();
      let endDate = new Date(this.createTask.EndDate.setHours(0,0,0,0)).getTime();
      if (endDate < startDate) {
        showDateValidationMsg = true;
        this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
      } else {
        showDateValidationMsg = false;
        // this.createTask.EndDate = '';
      }
    } else {
      showDateValidationMsg = false;
    }
    return showDateValidationMsg;
  }

  // dateValidationRemind(){
  //   this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Remind Date' : this.arabic('errormsgvalidenddate');
  //   let showDateValidationMsg = false;
  //   if (!this.createTask.StartDate && this.createTask.EndDate) {
  //     showDateValidationMsg = false;
  //   } else if (this.createTask.StartDate && this.createTask.EndDate) {
  //     let startDate = new Date(this.createTask.StartDate).getTime();
  //     let endDate = new Date(this.createTask.EndDate).getTime();
  //     let remind = new Date(this.createTask.RemindMeAt).getTime();
  //     if (remind < endDate && remind > startDate) {
  //       showDateValidationMsg = true;
  //     } else {
  //       showDateValidationMsg = false;
  //       this.createTask.RemindMeAt = '';
  //     }
  //   } else {
  //     showDateValidationMsg = false;
  //   }
  //   return showDateValidationMsg;
  // }
}
