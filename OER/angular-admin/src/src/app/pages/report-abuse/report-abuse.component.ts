import {Component, OnInit, ViewChild} from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {AbuseReportService} from '../../services/abuse-report/abuse-report.service';
import {EncService} from '../../services/enc.service';
import {ProfileService} from '../../services/profile.service';
import _ from 'lodash'
import { Paginator } from 'primeng/components/paginator/paginator';

@Component({
  selector: 'app-report-abuse',
  templateUrl: './report-abuse.component.html',
  styleUrls: ['./report-abuse.component.css']
})
export class ReportAbuseComponent implements OnInit {

  abuses: any;
  abuseList: any;
  abuseEnd: any;
  length: any;
  abuseStart: number;
  reason: string;
  deleteRequestItem: any;
  reasonModal: boolean;
  userportalUrl: string;
  abuseSubmit: boolean;
  sortFL: boolean;
  page: number;
  searchKeyword  : string = "";
  sortType: string = 'desc';
  sortField : number = 0;
  Clickfield :string = '';
  spanColNo : any;
  searchbackup : any;

  @ViewChild('pp') paginator: Paginator;

  constructor(private abuseService: AbuseReportService,
              private profileService: ProfileService,
              private coolDialogs: ConfirmationService,
              private spinner: NgxSpinnerService,
              private messageService: MessageService,
              public encService: EncService) {
  }

  ngOnInit() {
    this.userportalUrl = environment.userClientUrl;
    this.sortFL = true;
    this.reasonModal = false;
    this.abuseSubmit = false;
    this.abuses = [];
    this.abuseEnd = 0;
    this.abuseStart = 0;
    this.length = 0;
    this.page = 0;
    this.abuseEnd = [];
    this.getAbuses();
  }

  getAbuses() {
    this.abuses = [];
    this.abuseEnd = 0;
    this.abuseStart = 0;
    // this.length = 0;
    this.abuseList = [];
    this.spinner.show();
    this.abuseService.getAllAbuses().subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.abuses = response.returnedObject;
        for (let i = 0; i < this.abuses.length; i++) {
          var reasonsSplit = this.abuses[i].reportReasons.split(',');
          var reportReasons = "";
          for(let j = 0; j < reasonsSplit.length; j++)
          {
            if(j==0)
            {
              reportReasons = reasonsSplit[j] == '1' ? "Spam" : (reasonsSplit[j] == '2' ? "Offensive" :(reasonsSplit[j] == '3' ? "Misleading" :(reasonsSplit[j] == '4' ? "Other" : "")));
            }
            else
            {
              reportReasons = reportReasons + ", " + (reasonsSplit[j] == '1' ? "Spam" : (reasonsSplit[j] == '2' ? "Offensive" :(reasonsSplit[j] == '3' ? "Misleading" :(reasonsSplit[j] == '4' ? "Other" : ""))));
            }
          }
          this.abuses[i].reportReasons=reportReasons;
        }

        this.searchbackup = response.returnedObject;
        this.length = this.abuses.length;
        this.abuseEnd = this.abuses.length;
        this.sort();
        this.spinner.hide();
      } else {
        this.messageService.add({severity: 'error', summary: response.message});
        this.spinner.hide();
      }
    });


  }

  showCat(catgory, start = this.page) {
    const end = (this.abuseEnd - start) > 10 ? 10 : (this.abuseEnd - start);
    this.abuseList = [];
    for (let i = 0; i < end; i++) {
      this.abuseList[i] = catgory[start + i];
    }
  }

  paginateCat(event) {
    this.page = event.first;
    this.showCat(this.abuses);
  }

  getReason(item) {
    this.deleteRequestItem = item;
    this.reasonModal = true;
  }

  deleteItem() {
    this.abuseSubmit = true;
    if (this.reason && this.deleteRequestItem) {
      this.spinner.show();
      this.abuseService.postDeleteAbuse(
        this.deleteRequestItem.id, this.deleteRequestItem.contentType, this.reason)
        .subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.reason = null;
          this.deleteRequestItem = null;
          this.reasonModal = false;
          this.abuseSubmit = false;
          this.sortFL = !this.sortFL;
          this.getAbuses();
          this.messageService.add({severity: 'success', summary: 'Successfully removed item'});
          this.spinner.hide();
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
          this.spinner.hide();
        }
      });
    }
  }


  search(){
    this.abuses = this.searchbackup;
    this.abuses &&  this.abuses.length ? this.paginator.changePage(0) : null;
    debugger;
    const versiondata = [];
    var userdata = this.abuses;
    var s = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    if(s == "" ) {
      this.abuses &&  this.abuses.length ? this.paginator.changePage(0) : null;
      this.searchKeyword = "";
      this.Clickfield = "";
      this.getAbuses();
    } else {
      userdata &&  userdata.length
      ?  userdata.map(function mm(i) {
        if (i.title.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.reportReasons.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.description.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if ("Course".toLowerCase().includes(s.toLowerCase())) {
            if(i.contentType === 1) {
              versiondata.push(i);
            }
           }
           else if ("Resource".toLowerCase().includes(s.toLowerCase())) {
            if(i.contentType === 2) {
              versiondata.push(i);
            }
           }
           else if ("Resource Comment".toLowerCase().includes(s.toLowerCase())) {
            if(i.contentType === 3) {
              versiondata.push(i);
            }
           }
           else if ("Course Comment".toLowerCase().includes(s.toLowerCase())) {
            if(i.contentType === 4) {
              versiondata.push(i);
            }
           }
           else if (i.reportAbuseCount.toString() == s.toString()) {
            versiondata.push(i);
           }
           else if (i.updatedDate == s) { 
            versiondata.push(i);
           }
        }) : [];
        userdata = versiondata;
        this.length = userdata.length;
        this.abuseEnd = userdata.length;
        this.abuses = userdata;
        this.showCat(userdata);
    }
  }

  Clickingevent(event,columnNo) {
    debugger;
    var userdata = this.abuses;
    if(this.Clickfield == event.target.id) {
      this.sortType = this.sortType == 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.sortType = 'asc'
    }
    this.sortField = columnNo;
    this.abuses &&  this.abuses.length ? this.paginator.changePage(0) : null;
    this.spanColNo = this.sortType+"-"+columnNo;
    var sortedArray = _.sortBy(userdata, function(patient) {
      if(event.target.id == 'title'){
        return patient.title;
      }
      else if(event.target.id == 'desc'){
        return patient.description;
      }
      else if(event.target.id == 'type'){
        var data = "";
        if(patient.contentType === 1) {
          data = "Course"
        }
        if(patient.contentType === 2) {
          data = "Resource"
        }
        if(patient.contentType === 3) {
          data = "Resource Comment"
        }
        if(patient.contentType === 4) {
          data = "Course Comment"
        }
        return data.toLowerCase();
      }
      else if(event.target.id == 'abusecount'){
        return patient.reportAbuseCount;
      }
      else if(event.target.id == 'date'){
        return patient.updatedDate;
      }
  });
  if(this.sortType == 'desc') {
    sortedArray =   sortedArray.reverse();
  }
    this.length = sortedArray.length;
    this.abuseEnd = userdata.length;
    this.abuses = sortedArray;
    this.showCat(this.abuses);
    debugger;
    }

  clearSearch(){
    this.abuses &&  this.abuses.length ? this.paginator.changePage(0) : null;
    this.searchKeyword = "";
    this.Clickfield = "";
    this.getAbuses();
  }

  sort() {
    let abuses = null;
    if (this.sortFL) {
      abuses = this.abuses.sort((a, b) => {
        return <any>new Date(b.updatedDate) - <any>new Date(a.updatedDate);
      });
    } else {
      abuses = this.abuses.sort((a, b) => {
        return <any>new Date(a.updatedDate) - <any>new Date(b.updatedDate);
      });
    }
    this.sortFL = !this.sortFL;
    this.abuses = abuses;
    this.showCat(this.abuses);
  }

}
