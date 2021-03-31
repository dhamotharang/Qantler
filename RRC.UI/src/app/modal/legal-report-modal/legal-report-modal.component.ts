import { Component, OnInit, Input } from '@angular/core';
import { BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { ReportsService } from 'src/app/shared/service/reports.service';
import { LegalRequestTypes } from 'src/app/shared/enum/legal-request-types/legal-request-types.enum';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-legal-report-modal',
  templateUrl: './legal-report-modal.component.html',
  styleUrls: ['./legal-report-modal.component.scss']
})
export class LegalReportModalComponent implements OnInit {
  user = {
    id: 0
  };
  reportFilter = {
    Status: null,
    SourceOU: null,
    AttendedBy: null,
    ReqDateFrom: null,
    ReqDateTo: null,
    SmartSearch: null,
    Subject: null,
    Label: null,
    Type: null,
    UserID: 0,
    UserName: '',
    PageNumber: 0,
    PageSize: 0
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  statusOptions: any;
  status: any;
  statusList: Array<any>;
  departmentList: Array<any> = [];
  requestTypeList: Array<any> = [];
  statusDisable: any;

  @Input() myPendingSelected: boolean;
  @Input() myRequestsSelected: boolean;
  lang: string = 'en';

  validateStartEndDate:any = {
    isValid:true,
    msg:''
  };
  constructor(public bsModalRef: BsModalRef, private reportsService: ReportsService, public common: CommonService) {
    this.user = JSON.parse(localStorage.getItem("User"));
    if (this.common.language != 'English') {
      this.lang = 'ar';
    }
  }

  ngOnInit() {
    this.reportFilter.Status=(this.common.language=='English')?'All':this.arabic('all');
    this.reportFilter.SourceOU=(this.common.language=='English')? '':'';
    let toSendReq: any = this.reportFilter;
    // toSendReq.SourceOU = HrRequestTypes["Leave Requests"];
    this.reportsService.getLegalRequestsList(toSendReq).subscribe((legalRes) => {
      let departmentlist =[];
      let statusList=[];
      if(this.common.language == 'English')
        { 
        statusList.push({DisplayName:'All'});
        departmentlist.push({OrganizationID: '',OrganizationUnits: "All"});
        legalRes.M_OrganizationList.forEach((element)=>
        {
          departmentlist.push(element);
        });
        this.departmentList=departmentlist;
        legalRes.M_LookupsList.forEach((element)=>
        {
          statusList.push(element);
        });
        this.statusList=statusList;
      }
      if(this.common.language != 'English')
        { 
      statusList.push({DisplayName:this.arabic('all')});
          departmentlist.push({OrganizationID: '',OrganizationUnits: this.arabic('all')});
          legalRes.M_OrganizationList.forEach((element)=>
          {
            departmentlist.push(element);
          });
          this.departmentList=departmentlist;
          legalRes.M_LookupsList.forEach((element)=>
          {
            statusList.push(element);
          });
          this.statusList=statusList;
        }
      // this.statusList = legalRes.M_LookupsList;
      // this.departmentList = legalRes.M_OrganizationList;
    });

    let th = this;
    let reqTypes = Object.keys(LegalRequestTypes);
    Object.keys(LegalRequestTypes).slice(reqTypes.length / 2).forEach((type) => {
      if (type != 'My Own Requests') {
        th.requestTypeList.push({ value: LegalRequestTypes[type], label: type });
      }
    });
  }

  Download() {
    let toSendReq = {
      Status: (this.reportFilter.Status=='All' ||this.reportFilter.Status==this.arabic('all'))?null:this.reportFilter.Status,
      SourceOU: (this.reportFilter.SourceOU=='')?null:this.reportFilter.SourceOU,
      AttendedBy: this.reportFilter.AttendedBy,
      RequestDateFrom: this.reportFilter.ReqDateFrom,
      RequestDateTo: this.reportFilter.ReqDateTo,
      SmartSearch: this.reportFilter.SmartSearch,
      Subject: this.reportFilter.Subject,
      Label: this.reportFilter.Label,
      UserID:this.user.id
    };
    let dateVal = new Date(), cur_date = dateVal.getDate() + '-' + (dateVal.getMonth() + 1) + '-' + dateVal.getFullYear();
    this.reportsService.downloadModuleReport(toSendReq, 'legal').subscribe((resultBlob) => {
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = 'Legal Report-' + cur_date + '.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
      this.bsModalRef.hide();
    });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  dateValidation() {
    this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date': this.arabic('errormsgvalidenddate');
    let showDateValidationMsg = false;
    if (!this.reportFilter.ReqDateFrom && this.reportFilter.ReqDateTo) {
      showDateValidationMsg = false;
    } else if (this.reportFilter.ReqDateFrom && this.reportFilter.ReqDateTo) {
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

}
