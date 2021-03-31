import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { TaskService } from 'src/app/task/service/task.service';
import { TaskDashboardComponent } from 'src/app/task/container/component/task-dashboard/task-dashboard.component';
import { UtilsService } from 'src/app/shared/service/utils.service';
//import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-task-report-modal',
    templateUrl: './task-report-modal.component.html',
    styleUrls: ['./task-report-modal.component.scss'],
    providers: []
})
export class TaskReportModalComponent {
    bsConfig: Partial<BsDatepickerConfig>= {
      dateInputFormat:'DD/MM/YYYY'
    };
    statusOptions: any;
    status: any;
    sourceouOptions: any;
    destinationOptions: any;
    priorityList: string[];
    M_LookupsList: any;
    creatorUserList: any;
    assUserList: any;
    statusDisable: any;
    user = {
        id: 0
    };
    language: string;
    currentUser: any;
    report: any;
    all: any = 'All';

    
  validateStartEndDate: any = {
    isValid: true,
    msg: ''
  };

  validateDueStartEndDate: any = {
    isValid: true,
    msg: ''
  };


    constructor(private taskservice: TaskService,public util:UtilsService, private common: CommonService, public bsModalRef: BsModalRef, ) {
        this.language = (this.common.language == 'English') ? 'en' : 'ar';
        this.all = (this.language == 'en') ? 'All' : this.arabic('all');
        this.report = {
            status: this.all,
            creator: this.all,
            assignee: this.all,
            priority: this.all,
            duedatefrom: '',
            duedateto: '',
            datefrom: '',
            dateto: '',
            lable: '',
            linkto: '',
            smartSearch: '',
            participants: 'both',
            userID: 0
        }
    }



    arabic(word) {
        return this.common.arabic.words[word];
    }

    minDate(date) {
        return this.util.minDateCheck(date);
      }

    Download() {
        this.report.userID = this.currentUser.id;
        this.report.status = (!this.report.status || this.report.status == this.all) ? '' : this.report.status;
        this.report.creator = (!this.report.creator || this.report.creator == this.all) ? '' : this.report.creator;
        this.report.assignee = (!this.report.assignee || this.report.assignee == this.all) ? '' : this.report.assignee;
        this.report.priority = (!this.report.priority || this.report.priority == this.all) ? '' : this.report.priority;
        this.taskservice.downlaodExcel(this.report);
        this.bsModalRef.hide();
    }

    dateValidation() {
        this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
        let showDateValidationMsg = false;
        if (!this.report.datefrom && this.report.dateto) {
          showDateValidationMsg = false;
        } else if (this.report.datefrom && this.report.dateto) {
          let startDate = new Date(this.report.datefrom).getTime();
          let endDate = new Date(this.report.dateto).getTime();
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
        if (!this.report.duedatefrom && this.report.duedateto) {
          showDueDateValidationMsg = false;
        } else if (this.report.duedatefrom && this.report.duedateto) {
          let startDate = new Date(this.report.duedatefrom).getTime();
          let endDate = new Date(this.report.duedateto).getTime();
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
