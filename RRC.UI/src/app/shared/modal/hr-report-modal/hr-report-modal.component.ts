import { CommonService } from './../../../common.service';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { HrRequestTypes } from '../../enum/hr-request-types/hr-request-types.enum';
import { UtilsService } from '../../service/utils.service';
import { ReportsService } from '../../service/reports.service';

@Component({
  selector: 'app-hr-report-modal',
  templateUrl: './hr-report-modal.component.html',
  styleUrls: ['./hr-report-modal.component.scss']
})
export class HrReportModalComponent implements OnInit {
  user = {
      id: 0
  };
  reportFilter:any = {
    Status:'All',
    RequestType:'All',
    Creator:null,
    ReqDateFrom:null,
    ReqDateTo:null,
    SmartSearch:null,
    UserID:0,
    UserName:'',
    PageNumber:0,
    PageSize:0,
    reportType:'Excel'
  };

  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  statusOptions: any;
  status: any;
  statusList:Array<any>;
  requestTypeList:Array<any> = [];
  statusDisable : any;
  lang: any;

  @Input() myPendingSelected:boolean;
  @Input() myRequestsSelected:boolean;
  validateStartEndDateMmsg: any;

  constructor(public bsModalRef: BsModalRef,private reportsService:ReportsService, public common: CommonService ) {
      this.user = JSON.parse(localStorage.getItem("User"));
  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    let th = this;
    let reqTypes = Object.keys(HrRequestTypes);
    if(this.lang == 'en'){
      th.requestTypeList.push({value:"All",label:"All"});
      Object.keys(HrRequestTypes).slice(reqTypes.length/2).forEach((type)=>{
        if(type != 'CV Bank' && type != 'Employee Profile'){
          th.requestTypeList.push({value:HrRequestTypes[type],label:type});
        }
      });
      this.reportFilter.Status = 'All';
    }else{
      th.requestTypeList.push({value:"All",label:this.arabicfn("All".trim().replace(/\s+/g, '').toLowerCase())});
      Object.keys(HrRequestTypes).slice(reqTypes.length/2).forEach((type)=>{
        if(type != 'CV Bank' && type != 'Employee Profile'){
          if(type == 'Raise Complaints/ Suggestions'){
            th.requestTypeList.push({value:HrRequestTypes[type],label:this.arabicfn('raisecomplaintssuggestions')});
          }else{
            th.requestTypeList.push({value:HrRequestTypes[type],label:this.arabicfn(type.trim().replace(/\s+/g, '').toLowerCase())});
          }
        }
      });
      this.reportFilter.Status = this.arabicfn('all');
    }


    if(!this.myPendingSelected  && !this.myRequestsSelected){
      let toSendReq:any = {
        Status:this.reportFilter.Status,
        RequestType:this.reportFilter.RequestType,
        Creator:this.reportFilter.Creator,
        ReqDateFrom:this.reportFilter.ReqDateFrom,
        ReqDateTo:this.reportFilter.ReqDateTo,
        SmartSearch:this.reportFilter.SmartSearch,
        UserID:this.reportFilter.UserID,
        UserName:this.reportFilter.UserName,
        PageNumber:this.reportFilter.PageNumber,
        PageSize:this.reportFilter.PageSize, };
      if(!this.reportFilter.Status || this.reportFilter.Status == "All" || this.reportFilter.Status == this.arabicfn('all')){
        toSendReq.Status = '';
      }

      if(!this.reportFilter.RequestType || this.reportFilter.RequestType == "All" || this.reportFilter.RequestType == this.arabicfn('all')){
        toSendReq.RequestType = '';
      }
      // toSendReq.RequestType = HrRequestTypes["Leave Requests"];
      this.reportsService.getHRAllHRModulesList(toSendReq).subscribe((HrRes) => {
        let allStatusList = HrRes.M_LookupsList;
        let statusids = [];
        if(this.common.currentLang == "en"){
          statusids.push({DisplayName: "All"});
        }else if(this.common.currentLang == "ar"){
          statusids.push({DisplayName:this.arabicfn("all")});
        }
        allStatusList.forEach((item)=>{
          statusids.push({DisplayName:item.DisplayName});
        });
        this.statusList= statusids;
      });
    }

    if(this.myPendingSelected && !this.myRequestsSelected){
      this.reportsService.getHRMyPendingRequestList(this.reportFilter).subscribe((HrRes) => {
        let allStatusList = HrRes.M_LookupsList;
        let statusids = [];
        if(this.common.currentLang == "en"){
          statusids.push({DisplayName: "All"});
        }else if(this.common.currentLang == "ar"){
          statusids.push({DisplayName:this.arabicfn("all")});
        }
        allStatusList.forEach((item)=>{
          statusids.push({DisplayName:item.DisplayName});
        });
        this.statusList= statusids;
      });
    }

    if(!this.myPendingSelected && this.myRequestsSelected){
      this.reportsService.getHRMyOwnRequestList(this.reportFilter).subscribe((HrRes) => {
        let allStatusList = HrRes.M_LookupsList;
        let statusids = [];
        if(this.common.currentLang == "en"){
          statusids.push({DisplayName: "All"});
        }else if(this.common.currentLang == "ar"){
          statusids.push({DisplayName:this.arabicfn("all")});
        }
        allStatusList.forEach((item)=>{
          statusids.push({DisplayName:item.DisplayName});
        });
        this.statusList= statusids;
      });
    }
  }

  Download() {
      this.reportFilter.UserID = this.user.id;
      let toSendReportFilter = {
        Status:this.reportFilter.Status,
        RequestType:this.reportFilter.RequestType,
        Creator:this.reportFilter.Creator,
        ReqDateFrom:this.reportFilter.ReqDateFrom,
        ReqDateTo:this.reportFilter.ReqDateTo,
        SmartSearch:this.reportFilter.SmartSearch,
        UserID:this.reportFilter.UserID,
        UserName:this.reportFilter.UserName,
        PageNumber:this.reportFilter.PageNumber,
        PageSize:this.reportFilter.PageSize,
        reportType:'Excel'};
      toSendReportFilter.RequestType = parseInt(this.reportFilter.RequestType);
      if(!this.reportFilter.Status || this.reportFilter.Status == "All" || this.reportFilter.Status == this.arabicfn('all')){
        toSendReportFilter.Status = '';
      }

      if(!this.reportFilter.RequestType || this.reportFilter.RequestType == "All" || this.reportFilter.RequestType == this.arabicfn('all')){
        toSendReportFilter.RequestType = '';
      }
      let dateVal = new Date(), cur_date = dateVal.getDate() +'-'+(dateVal.getMonth()+1)+'-'+dateVal.getFullYear();
      this.reportsService.downloadHrReport(toSendReportFilter).subscribe((resultBlob) =>{
        let url = window.URL.createObjectURL(resultBlob);
        let a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = 'HR Report-'+cur_date+'.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        this.bsModalRef.hide();
      });
  }

  dateValidation() {
    this.validateStartEndDateMmsg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date': this.arabicfn('errormsgvalidenddate');
    let showDateValidationMsg = false;
    if (this.reportFilter.ReqDateFrom && this.reportFilter.ReqDateTo) {
      let startDate = new Date(this.reportFilter.ReqDateFrom).getTime();
      let endDate = new Date(this.reportFilter.ReqDateTo).getTime();
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

  arabicfn(word) {
    return this.common.arabic.words[word];
  }
}
