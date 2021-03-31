import {Component, OnInit, ViewChild} from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {QrcService} from '../../services/qrc.service';
import { Paginator } from 'primeng/components/paginator/paginator';
import _ from 'lodash'


@Component({
  selector: 'app-qrc-reports',
  templateUrl: './qrc-reports.component.html',
  styleUrls: ['./qrc-reports.component.css']
})
export class QrcReportsComponent implements OnInit {
  reports: any;
  totalReportsCount: number;
  pageStart: number;
  pageSize: number;
  reportsList: any;
  searchKeyword  : string = "";
  sortType: string = 'desc';
  sortField : number = 0;
  Clickfield :string = '';
  spanColNo : any;
  searchbackup : any;

  @ViewChild('pp') paginator: Paginator;

  constructor(private spinner: NgxSpinnerService, private QRCService: QrcService, private messageService: MessageService) {
  }

  ngOnInit() {
    this.reports = [];
    this.totalReportsCount = 0;
    this.pageStart = 0;
    this.pageSize = 10;
    this.reportsList = [];
    this.getQrcReports();
  }

  getQrcReports() {
    this.spinner.show();
    this.QRCService.getQRCReport().subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.reports = response.returnedObject;
        this.searchbackup = response.returnedObject;
        this.totalReportsCount = response.returnedObject.length;
        this.spinner.hide();
        this.setReports();
      } else {
        this.messageService.add({severity: 'error', summary: response.message});
        this.spinner.hide();
      }
    });
  }

  search(){
    this.reports = this.searchbackup;
    this.reports && this.reports.length ?  this.paginator.changePage(0) : null;
    debugger;
    const versiondata = [];
    var reportdata = this.reports;
    var s = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    if(s == "" ) {
      this.reports && this.reports.length ?  this.paginator.changePage(0) : null;
      this.searchKeyword = "";
      this.Clickfield = "";
      this.getQrcReports();
    } else {
      reportdata &&  reportdata.length
      ?  reportdata.map(function mm(i, index) {
        if (i.name.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
        else if (i.userCount.toString() == s.toString()) {
            versiondata.push(i);
           }
           else if (i.approveCount.toString() == s.toString()) {
            versiondata.push(i);
           }
           else if (i.rejectCount.toString() == s.toString()) {
            versiondata.push(i);
           }
           else if (i.pendingAction.toString() == s.toString()) {
            versiondata.push(i);
           }
           else if (i.submission.toString() == s.toString()) {
            versiondata.push(i);
           }
        }) : [];
        reportdata = versiondata;
        this.reports = reportdata;
        this.totalReportsCount =  this.reports.length
        this.setReports();
    }
  }

  Clickingevent(event,columnNo) {
    debugger;
    var reportdata = this.reports;
    if(this.Clickfield == event.target.id) {
      this.sortType = this.sortType == 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.sortType = 'asc'
    }
    this.sortField = columnNo;
    this.reports && this.reports.length ?  this.paginator.changePage(0) : null;
    this.spanColNo = this.sortType+"-"+columnNo;
    var sortedArray = _.sortBy(reportdata, function(patient) {
      if(event.target.id == 'qrcname'){
        return patient.name;
      }
      else if(event.target.id == 'usercount') {
        return patient.userCount;
      }
      else if(event.target.id == 'approvedcount') {
        return patient.approveCount;
      }
      else if(event.target.id == 'rejectioncount') {
        return patient.rejectCount;
      }
      else if(event.target.id == 'actionpending') {
        return patient.pendingAction;
      }
      else if(event.target.id == 'submission') {
        return patient.submission;
      }
  });
  if(this.sortType == 'desc') {
    sortedArray =   sortedArray.reverse();
  }
    this.reports = sortedArray;
    this.setReports();
    debugger;
    }

  clearSearch(){
    this.reports && this.reports.length ?  this.paginator.changePage(0) : null;
    this.searchKeyword = "";
    this.Clickfield = "";
    this.getQrcReports();
  }

  setReports() {
    this.reportsList = [];
    let i: number;
    for (i = 0; i < this.pageSize; i++) {
      if (this.reports[i + this.pageStart]) {
        this.reportsList.push(this.reports[i + this.pageStart]);
      } else {
        break;
      }
    }
  }

  paginate(event) {
    this.pageStart = this.pageSize * event.page;
    this.setReports();
  }
}
