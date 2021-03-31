import { Component, OnInit, Input } from '@angular/core';
import { BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { ReportsService } from 'src/app/shared/service/reports.service';
import { ItRequestType } from 'src/app/shared/enum/it-request-type/it-request-type.enum';
import { ItService } from 'src/app/it/service/it.service';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';

@Component({
  selector: 'app-it-report-modal',
  templateUrl: './it-report-modal.component.html',
  styleUrls: ['./it-report-modal.component.scss']
})

export class ItReportModalComponent implements OnInit {
  user = {
    id: 0
  };
  reportFilter:any = {
    Status: 'All',
    RequestType:'',
    SourceOU: '',
    Subject: '',
    RequestDateFrom:'',
    RequestDateTo:'',
    Priority: '',
    SmartSearch:'',
    UserID:0,
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY',
    adaptivePosition:true

  };
  statusOptions: any;
  // status: any;
  requestTypeList:Array<any> = [];
  statusDisable : any;

  reqTypes = [];

  OrganizationList = [];

  priority = [
    {
      value: 'Low',
      label: 'Low'
    },
    {
      value: 'High',
      label: 'High'
    }
  ];

  statusList:Array<any> = [
    {'DisplayName': "All", 'value': ""},
    {'DisplayName': "Under Process", 'value': "Under Process"},
    {'DisplayName': "Closed", 'value': "Closed"}
  ];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  isItDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 11;
  isItDepartmentTeamUserID = this.currentUser.OrgUnitID == 11 && !this.currentUser.IsOrgHead;
  lang;
  constructor(
    public bsModalRef: BsModalRef,
    private reportsService: ReportsService, private itService: ItService, private common:CommonService, public arabic:ArabicDataService ) {
      this.lang = this.common.currentLang;
  }

  ngOnInit() {
    if (this.lang == 'en') {
      this.priority = [
        {
          value: '',
          label: 'All'
        },
        {
          value: 'Low',
          label: 'Low'
        },
        {
          value: "High",
          label: 'High'
        }
      ];
      this.reportFilter.Status ='All';
      this.reportFilter.SourceOU ='All';
    } else {
      this.priority = [
        {
          value: '',
          label: this.arabic.words.all
        },
        {
          value: 'Low',
          label: this.arabic.words.low
        },
        {
          value: "High",
          label: this.arabic.words.high
        }
      ];
      this.reportFilter.Status =this.arabic.words.all;
      this.reportFilter.SourceOU =this.arabic.words.all;
    }
    let th = this;
    let requestTypes = Object.keys(ItRequestType);
    if(this.lang == 'en'){
      this.reqTypes = [{"value":'', "label": "All"}];
    }else{
      this.reqTypes = [{"value":'', "label": this.arabic.words.all}];
    }
    Object.keys(ItRequestType).slice(requestTypes.length/2).forEach((type)=>{
      if(this.lang == 'en'){
        th.reqTypes.push({value:ItRequestType[type],label:type});
      }else{
        th.reqTypes.push({value:ItRequestType[type],label:this.arabic.words[this.removeWordSpaces(type).trim().toLowerCase()]});
      }
    });
    // if(this.isItDepartmentHeadUserID || this.isItDepartmentTeamUserID){
    //   this.reportFilter.RequestType = ItRequestType["Support New"];
    // }else{
      this.reportFilter.RequestType = '';
    // }
    let SmartSearch= '';
    let Subject = '';
    let Priority = '';
    let SourceOU = '';
    let RequestType = '';
    let ReqDateFrom= '';
    let ReqDateTo= '';
    let Type = '';
    let itStatus = '';
    this.itService.getItRequestList(1, 10, RequestType, this.currentUser.id, this.currentUser.username, itStatus, SmartSearch, ReqDateFrom, ReqDateTo, SourceOU, Subject, Priority,Type ).subscribe((allOwnRes:any) => {
      if(allOwnRes){
        this.statusList = allOwnRes.M_LookupsList;
        this.OrganizationList =allOwnRes.OrganizationList;
      }
    });
  }

  Download() {
    let SourceOU = '';
    let itStatus = '';

    this.reportFilter.UserID = this.currentUser.id;
    // this.reportFilter.UserName = this.currentUser.username;
    if(this.reportFilter.Status && this.reportFilter.Status !='All' && this.reportFilter.Status != this.arabic.words.all){
      itStatus = this.reportFilter.Status;
    }
    if(this.reportFilter.SourceOU && this.reportFilter.SourceOU !='All' && this.reportFilter.SourceOU != this.arabic.words.all){
      SourceOU = this.reportFilter.SourceOU;
    }
    this.reportFilter.Status = itStatus;
    this.reportFilter.SourceOU = SourceOU;
    let dateVal = new Date(), cur_date = dateVal.getDate() +'-'+(dateVal.getMonth()+1)+'-'+dateVal.getFullYear();
    this.reportsService.downloadItReport(this.reportFilter).subscribe((resultBlob) =>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = 'IT Report-'+cur_date+'.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
      this.bsModalRef.hide();
    });
  }

  removeWordSpaces(words:string){
    return  words.replace(/\s+/g, '');
  }
}
