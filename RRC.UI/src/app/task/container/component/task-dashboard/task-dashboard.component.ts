import { Component, OnInit, ViewChild, TemplateRef, ElementRef } from '@angular/core';
import Chart from 'chart.js';
import { CommonService } from '../../../../common.service';
import { data } from './data';
import { TaskService } from '../../../service/task.service';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { TaskEvent } from '../../../service/task.event';
import { TaskReportModalComponent } from 'src/app/modal/task-report-modal/task-report-modal.component';
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: 'app-task-dashboard',
  templateUrl: './task-dashboard.component.html',
  styleUrls: ['./task-dashboard.component.scss']
})
export class TaskDashboardComponent implements OnInit {
  yellowTaskBTReminderEndDate = 0;
  grayNoOfNewTask = 0;
  assUserList: any = '';
  creatorUserList: any;
  Creator: any = (this.common.language == 'English') ? 'All' : this.arabic('all');
  Priority: string = (this.common.language == 'English') ? 'All' : this.arabic('all');
  Lable: string = '';
  LinkTo: string = '';
  Participants: string = 'both';
  SmartSearch: string = '';
  CreationDateTo: any;
  CreationDateFrom: any;
  DueDateTo: any;
  DueDateFrom: any;
  Status = (this.common.language == 'English') ? 'All' : this.arabic('all');
  Assignee: any = (this.common.language == 'English') ? 'All' : this.arabic('all');
  TaskCompleted: any;
  TaskInprogress: any;
  TaskParticipations: any;
  TaskClosed: any;
  chartLable: string[];
  pieChartLable: string[];
  tableMessages: any;
  // templates for custom columns in ngx-datatable
  @ViewChild('refIDTemplate') refIDTemplate: TemplateRef<any>;
  @ViewChild('titleTemplate') titleTemplate: TemplateRef<any>;
  @ViewChild('creatorTemplate') creatorTemplate: TemplateRef<any>;
  @ViewChild('assigneeTemplate') assigneeTemplate: TemplateRef<any>;
  @ViewChild('statusTemplate') statusTemplate: TemplateRef<any>;
  @ViewChild('priorityTemplate') priorityTemplate: TemplateRef<any>;
  @ViewChild('creationDateTemplate') creationDateTemplate: TemplateRef<any>;
  @ViewChild('dueDateTemplate') dueDateTemplate: TemplateRef<any>;
  @ViewChild('lastUpdateTemplate') lastUpdateTemplate: TemplateRef<any>;
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;

  @ViewChild('pieChart') pieChart: ElementRef;
  @ViewChild('barChart') barChart: ElementRef;
  public rows: Array<any> = [];
  public columns: Array<any> = [];
  protected bsModalRef: BsModalRef;
  public cardDetails = [
    {
      'image': 'assets/task/clipboards1.png',
      'count': 0,
      'name': 'My Task',
      'taskid': 1,
      'progress': 50
    },
    {
      'image': 'assets/task/clipboards2.png',
      'count': 0,
      'name': 'Task I have assigned',
      'taskid': 2,
      'progress': 50
    },
    {
      'image': 'assets/task/clipboards3.png',
      'count': 0,
      'taskid': 3,
      'name': 'Task Participations',
      'progress': 50
    }
  ];
  userList: Object;
  priorityList: string[];
  page = 1;
  itemsPerPage = 10;
  maxSize = 10;
  masterData = new data;
  status = this.masterData.data.status;
  taskList = this.masterData.data.Collection;
  M_LookupsList = [];
  count = this.masterData.data.Count;
  currentUser: any;
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  actionType = 1;
  length: number;
  progress = false;
  taskStatus = {
    Mytask: 0,
    Participants: 0,
    Assignee: 0
  };
  task_type: any = "My Task";
  task_title = 'Task Dashboard';
  horizontalBarChartData: { data: { label: string; backgroundColor: string; data: number[]; }[]; };
  greenTaskBTStartReminderDate = 0;
  redTaskEndDateGtActualdate = 0;

  validateStartEndDate: any = {
    isValid: true,
    msg: ''
  };

  validateDueStartEndDate: any = {
    isValid: true,
    msg: ''
  };

  constructor(public common: CommonService,
    public router: Router,
    protected modalService: BsModalService,
    public event: TaskEvent,
    public taskservice: TaskService,
    public datePipe: DatePipe,
    public util: UtilsService) {
    this.priorityList = this.common.priorityList;
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    var filderData = {
      Status: '',
      Creator: '',
      Assignee: '',
      Priority: '',
      Lable: this.Lable,
      LinkTo: this.LinkTo,
      DueDateFrom: (this.DueDateFrom) ? this.DueDateFrom.toJSON() : '',
      DueDateTo: (this.DueDateTo) ? this.DueDateTo.toJSON() : '',
      CreationDateFrom: (this.CreationDateFrom) ? this.CreationDateFrom.toJSON() : '',
      CreationDateTo: (this.CreationDateTo) ? this.CreationDateTo.toJSON() : '',
      Participants: this.Participants,
      SmartSearch: this.SmartSearch,
    };
    this.getTaskList(this.actionType, filderData);
    if (this.common.language == 'English')
      this.common.breadscrumChange('Duty Tasks', 'Tasks List', 0);
    else
      this.common.breadscrumChange(this.arabic('dutytask'), this.arabic('tasklist'), 0);
    this.taskservice.getTaskCount(this.currentUser.id).subscribe((res: any) => {
      this.cardDetails[0].count = res.MyTask;
      this.cardDetails[1].count = res.AssignedTask;
      this.cardDetails[2].count = res.TaskParticipations;
      this.taskStatus.Mytask = res.MyTask;
      this.taskStatus.Assignee = res.AssignedTask;
      this.taskStatus.Participants = res.TaskParticipations;
      this.greenTaskBTStartReminderDate = res.TaskBTStartReminderDate;
      this.redTaskEndDateGtActualdate = res.TaskEndDateGtActualdate;
      this.yellowTaskBTReminderEndDate = res.TaskBTReminderEndDate;
      this.TaskCompleted = (res.TaskCompleted) ? res.TaskCompleted : 0;
      this.TaskInprogress = (res.TaskInprogress) ? res.TaskInprogress : 0;
      this.TaskClosed = (res.TaskClosed) ? res.TaskClosed : 0;
      this.grayNoOfNewTask = res.NoOfNewTask;
      this.chartProperties();
      this.event.changeTaskStatus(this.taskStatus);
    });
    this.event.changeSelectType$.subscribe(res => {
      this.changeType(res);
    });
  }

  ngOnInit() {
    this.tableMessages = {
      emptyMessage: (this.common.language == 'English') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
    if (this.common.language == 'English') {
      this.pieChartLable = ['TaskInprogress', 'TaskCompleted', 'TaskClosed'];
      // this.columns = [
      //   { name: 'Ref ID', prop: '', cellTemplate: this.refIDTemplate,sortable:true },
      //   { name: 'Title', prop: '', cellTemplate: this.titleTemplate },
      //   { name: 'Creator', prop: '', cellTemplate: this.creatorTemplate },
      //   { name: 'Assignee', prop: '', cellTemplate: this.assigneeTemplate },
      //   { name: 'Status', prop: '', cellTemplate: this.statusTemplate },
      //   { name: 'Priority', prop: '', cellTemplate: this.priorityTemplate },
      //   { name: 'Creation Date', prop: '', cellTemplate: this.creationDateTemplate },
      //   { name: 'Due Date', prop: '', cellTemplate: this.dueDateTemplate },
      //   { name: 'Last Update', prop: '', cellTemplate: this.lastUpdateTemplate },
      //   { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
      // ];
      this.columns = [
        { name: 'Ref ID', prop: 'TaskReferenceNumber' },
        { name: 'Title', prop: 'Title' },
        { name: 'Creator', prop: 'Creator' },
        { name: 'Assignee', prop: 'Assignee' },
        { name: 'Status', prop: 'Status' },
        { name: 'Priority', prop: 'Priority' },
        { name: 'Creation Date', prop: 'CreationDate' },
        { name: 'Due Date', prop: 'DueDate' },
        { name: 'Last Update', prop: 'LastUpdate' },
        { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
      ];
    }
    else {
      this.pieChartLable = [this.arabic('taskinprogress'), this.arabic('taskcompleted'), this.arabic('taskclosed')];
      // this.columns = [
      //   { name: this.arabic('refid'), prop: '', cellTemplate: this.refIDTemplate },
      //   { name: this.arabic('tasktitle'), prop: '', cellTemplate: this.titleTemplate },
      //   { name: this.arabic('creator'), prop: '', cellTemplate: this.creatorTemplate },
      //   { name: this.arabic('assignee'), prop: '', cellTemplate: this.assigneeTemplate },
      //   { name: this.arabic('status'), prop: '', cellTemplate: this.statusTemplate },
      //   { name: this.arabic('priority'), prop: '', cellTemplate: this.priorityTemplate },
      //   { name: this.arabic('createdate'), prop: '', cellTemplate: this.creationDateTemplate },
      //   { name: this.arabic('duedate'), prop: '', cellTemplate: this.dueDateTemplate },
      //   { name: this.arabic('lastupdate'), cellTemplate: this.lastUpdateTemplate },
      //   { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
      // ];
      this.columns = [
        { name: this.arabic('refid'), prop: 'TaskReferenceNumber' },
        { name: this.arabic('title'), prop: 'Title' },
        { name: this.arabic('creator'), prop: 'Creator' },
        { name: this.arabic('assignee'), prop: 'Assignee' },
        { name: this.arabic('status'), prop: 'Status' },
        { name: this.arabic('priority'), prop: 'Priority' },
        { name: this.arabic('createdate'), prop: 'CreationDate' },
        { name: this.arabic('duedate'), prop: 'DueDate' },
        { name: this.arabic('lastupdate'), prop: 'LastUpdate' },
        { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
      ];
      this.cardDetails = [
        {
          'image': 'assets/task/clipboards1.png',
          'count': 0,
          'name': this.arabic('mytask'),
          'taskid': 1,
          'progress': 50
        },
        {
          'image': 'assets/task/clipboards2.png',
          'count': 0,
          'name': this.arabic('taskihaveassigned'),
          'taskid': 2,
          'progress': 50
        },
        {
          'image': 'assets/task/clipboards3.png',
          'count': 0,
          'taskid': 3,
          'name': this.arabic('taskparticipants'),
          'progress': 50
        }
      ];
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  // generating charts
  chartProperties() {
    // generating barchart
    var barctx = document.getElementById('barChart');

    //preparing data for barchart
    var horizontalBarChartData = [{
      label: (this.common.language == 'English') ? 'High' : this.arabic('high'),
      backgroundColor: 'rgba(240, 120, 90, 1)',
      data: [this.redTaskEndDateGtActualdate]
    }, {
      label: (this.common.language == 'English') ? 'Medium' : this.arabic('medium'),
      backgroundColor: 'rgba(240, 196, 25, 1)',
      data: [this.grayNoOfNewTask]
    }, {
      label: (this.common.language == 'English') ? 'Low' : this.arabic('low'),
      backgroundColor: 'rgba(113 ,194 ,133 , 1)',
      data: [this.greenTaskBTStartReminderDate]
    }, {
      label: (this.common.language == 'English') ? 'Very Low' : this.arabic('verylow'),
      backgroundColor: 'rgba(85 ,96 ,128, 1)',
      data: [this.yellowTaskBTReminderEndDate]
    }];

    new Chart(barctx, {
      type: 'horizontalBar',
      data: {
        labels: [this.common.language == 'English' ? 'Priority' : this.arabic('priority')],
        datasets: horizontalBarChartData
      },
      options: {
        maintainAspectRatio: false,
        legend: {
          display: true,
          position: 'bottom',
          fullWidth: true
        },
        scales: {
          xAxes: [{
            ticks: {
              beginAtZero: true,
              stepSize: 1
            },
            gridLines: {
              display: true,
            }
          }],
          yAxes: [{
            ticks: {
              display: false,
              autoSkip: false
            },
            gridLines: {
              display: false,
              color: "rgba(0, 0, 0, 0)",
            }
          }]
        }
      }
    });

    // generating pie chart
    var piectx = document.getElementById('pieChart');
    new Chart(piectx, {
      type: 'pie',
      data: {
        labels: this.pieChartLable,
        datasets: [{
          label: '# of Votes',
          data: [this.TaskInprogress, this.TaskCompleted, this.TaskClosed], // data for pie chart
          backgroundColor: [
            'rgba(240, 120, 90, 1)',
            'rgba(113 ,194 ,133 , 1)',
            'rgba(240, 196, 25, 1)',
            'rgba(85 ,96 ,128, 1)',
          ],
          borderColor: [
            'rgba(255, 255, 255, 1)',
            'rgba(255, 255, 255, 1)',
            'rgba(255, 255, 255, 1)',
            'rgba(255, 255, 255, 1)'
          ],
          borderWidth: 10
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        legend: {
          display: true,
          position: 'bottom',
          fullWidth: true
        },
        scales: {
          xAxes: [{
            ticks: {
              display: false
            },
            gridLines: {
              display: false,
              color: "rgba(0, 0, 0, 0)",
            }
          }],
          yAxes: [{
            ticks: {
              display: false
            },
            gridLines: {
              display: false,
              color: "rgba(0, 0, 0, 0)",
            }
          }]
        }
      }
    });
  }


  changeType(type) {
    if (type == 1) {
      this.Status = (this.common.language == 'English') ? 'InProgress' : this.arabic('inprogress');
    }
    if (type == 2) {
      this.Status = (this.common.language == 'English') ? 'All' : this.arabic('all');
    }
    if (type == 3) {
      this.Status = (this.common.language == 'English') ? 'All' : this.arabic('all');
    }
    this.actionType = type;
    this.clearFilter();
    this.getTaskList(type);
    this.taskservice.triggerScrollTo();
  }

  clearFilter() {
    this.Creator = (this.common.language == 'English') ? 'All' : this.arabic('all');
    this.Assignee = (this.common.language == 'English') ? 'All' : this.arabic('all');
    this.Priority = (this.common.language == 'English') ? 'All' : this.arabic('all');
    this.Lable = '';
    this.LinkTo = '';
    this.DueDateFrom = '';
    this.DueDateTo = '';
    this.CreationDateFrom = '';
    this.CreationDateTo = '';
    this.Participants = 'both';
    this.SmartSearch = '';
  }

  filterTask() {
    var all = (this.common.language == 'English') ? 'All' : this.arabic('all');
    var filderData = {
      Status: (!this.Status || this.Status == all) ? '' : this.Status,
      Creator: (!this.Creator || this.Creator == all) ? '' : this.Creator,
      Assignee: (!this.Assignee || this.Assignee == all) ? '' : this.Assignee,
      Priority: (!this.Priority || this.Priority == all) ? '' : this.Priority,
      Lable: this.Lable,
      LinkTo: this.LinkTo,
      DueDateFrom: (this.util.isValidDate(this.DueDateFrom)) ? this.DueDateFrom.toJSON() : '',
      DueDateTo: (this.util.isValidDate(this.DueDateTo)) ? this.DueDateTo.toJSON() : '',
      CreationDateFrom: (this.util.isValidDate(this.CreationDateFrom)) ? this.CreationDateFrom.toJSON() : '',
      CreationDateTo: (this.util.isValidDate(this.CreationDateTo)) ? this.CreationDateTo.toJSON() : '',
      Participants: this.Participants,
      SmartSearch: this.SmartSearch,
    }
    this.getTaskList(this.actionType, filderData);
  }

  getTaskList(type, filderData: any = '') {
    switch (type) {
      case 1:
        this.common.sideNavResponse('task', 'My Task');
        break;
      case 2:
        this.common.sideNavResponse('task', 'Assigned Task');
        break;
      case 3:
        this.common.sideNavResponse('task', 'Participants');
        break;
    }
    this.progress = true;
    this.taskservice.getTaskList(this.page, this.maxSize, this.currentUser.id, type, filderData).subscribe((res: any) => {
      this.progress = false;
      this.taskList = res;
      this.M_LookupsList = res.M_LookupsList;
      this.rows = res.Collection;
      this.creatorUserList = res.Creator;

      // this.Creator = res.Creator[0].UserID;
      this.assUserList = res.Assignee;
      this.length = res.Count;
      if (this.rows.length > 0) {
        this.rows.forEach(val => {
          if (this.common.language == 'English') {
            if (val['Priority'] == 'High') {
              val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == 'Medium') {
              val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == 'Low') {
              val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == 'VeryLow') {
              val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
          } else {
            if (val['Priority'] == 'فائق الأهمية') {
              val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == 'متوسط الأهمية') {
              val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == 'منخفض الأهمية') {
              val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == 'غير مهم') {
              val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
          }
          val.CreationDate = this.datePipe.transform(val.CreationDate, "dd/MM/yyyy");
          val.DueDate = this.datePipe.transform(val.DueDate, "dd/MM/yyyy");
          val.LastUpdate = this.datePipe.transform(val.LastUpdate, "dd/MM/yyyy");
        });
      }
    });
  }

  public onChangePage(page): any {
    this.page = page;
    this.getTaskList(this.actionType);
  }

  async getUserList(data) {
    await this.common.getUserList(data, this.currentUser.id).subscribe(list => {
      this.userList = list;
    });
  }

  async viewData(type, value: any = '') {
    var path = '';
    if (this.common.language == 'English')
      path = 'en/app/task/task-view/';
    else
      path = 'ar/app/task/task-view/';
    var param = {
      type: 'view',
      pageInfo: {
        type: 'task',
        id: value.TaskID,
        title: this.task_type
      }
    };
    await this.common.action(param);
    if (type == 'edit') {
      this.router.navigate([path + value.TaskID]);
    } else {
      this.router.navigate([path + value.TaskID]);
    }
  }

  openReport() {
    this.bsModalRef = this.modalService.show(TaskReportModalComponent, { class: 'modal-lg' });
    this.bsModalRef.content.M_LookupsList = this.M_LookupsList;
    this.bsModalRef.content.creatorUserList = this.creatorUserList;
    this.bsModalRef.content.assUserList = this.assUserList;
    this.bsModalRef.content.priorityList = this.priorityList;
    this.bsModalRef.content.currentUser = this.currentUser;
  }

  getRowClass(row) {
    if (row.DeleteFlag)
      return {
        'striked': true
      };
  }

  minDate(date) {
    return this.util.minDateCheck(date);
  }
  maxDate(date) {
    return this.util.maxDateCheck(date);
  }


  dateValidation() {
    this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
    let showDateValidationMsg = false;
    if (!this.CreationDateFrom && this.CreationDateTo) {
      showDateValidationMsg = false;
    } else if (this.CreationDateFrom && this.CreationDateTo) {
      let startDate = new Date(this.CreationDateFrom).getTime();
      let endDate = new Date(this.CreationDateTo).getTime();
      if (endDate < startDate) {
        showDateValidationMsg = true;
      } else {
        showDateValidationMsg = false;
      }
    } else {
      showDateValidationMsg = false;
    }
    return showDateValidationMsg;
  }

  duedateValidation() {
    this.validateDueStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
    let showDueDateValidationMsg = false;
    if (!this.DueDateFrom && this.DueDateTo) {
      showDueDateValidationMsg = false;
    } else if (this.DueDateFrom && this.DueDateTo) {
      let startDate = new Date(this.DueDateFrom).getTime();
      let endDate = new Date(this.DueDateTo).getTime();
      if (endDate < startDate) {
        showDueDateValidationMsg = true;
      } else {
        showDueDateValidationMsg = false;
      }
    } else {
      showDueDateValidationMsg = false;
    }
    return showDueDateValidationMsg;
  }
}

