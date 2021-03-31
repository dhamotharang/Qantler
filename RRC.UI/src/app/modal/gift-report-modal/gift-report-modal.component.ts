import { ArabicDataService } from './../../arabic-data.service';
import { Component, OnInit, Input } from '@angular/core';
import { ReportsService } from 'src/app/shared/service/reports.service';
import { BsModalRef, BsDatepickerConfig } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { GiftsManagementService } from 'src/app/media/gifts-management/service/gifts-management.service';

@Component({
  selector: 'app-gift-report-modal',
  templateUrl: './gift-report-modal.component.html',
  styleUrls: ['./gift-report-modal.component.scss']
})
export class GiftReportModalComponent implements OnInit {
  user = {
    id: 0
  };
  reportFilter = {
    Status:null,
    GiftType:null,
    RecievedPurchasedBy:null,
    SmartSearch:null,
    UserID: 0,
    UserName: '',
    PageNumber: 0,
    PageSize: 0
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'MM-DD-YYYY'
  };
  statusOptions: any;
  status: any;
  statusList: Array<any> = [];
  departmentList: Array<any> = [];
  statusDisable: any;
  giftTypeList: Array<any> = [];
  lang: string = 'en';
  selectstatus: string;

  constructor(
    public bsModalRef: BsModalRef,
    private reportsService: ReportsService,
    public common: CommonService,
    private giftsManagementService: GiftsManagementService,
    public arabicService: ArabicDataService) {
    this.user = JSON.parse(localStorage.getItem("User"));
    if (this.common.language != 'English') {
      this.lang = 'ar';
    }
    this.selectstatus = (this.lang == 'ar')?this.arabicService.words.selectstatus:'Select Status';
  }

  ngOnInit() {
    if (this.lang == 'ar') {
      this.giftTypeList =[{label: this.arabic('all'), value:''},{label: this.arabic('giftreceived'), value:1},{label:this.arabic('giftpushed'), value:2}];
    } else {
      this.giftTypeList =[{label:'All',value:''},{label:'Gifts Received', value:1},{label:'Gifts Purchased', value:2}];
    }
    this.loadStatusList();
    this.reportFilter.GiftType = '';
    // let toSendReq: any = this.reportFilter;
    // this.reportsService.getGiftRequestList(toSendReq).subscribe((giftRes: any) => {
    //   this.statusList = giftRes.M_LookupsList;
    //   this.departmentList = giftRes.OrganizationList;
    // });
  }

  loadStatusList(requestCode?:any){
    let toSendReq:any = {
      PageNumber: this.reportFilter.PageNumber,
      PageSize: this.reportFilter.PageSize,
      UserID: this.reportFilter.UserID,
      Status: this.reportFilter.Status,
      GiftType: this.reportFilter.GiftType,
      UserName: this.reportFilter.UserName,
      RecievedPurchasedBy: this.reportFilter.RecievedPurchasedBy,
      SmartSearch: this.reportFilter.SmartSearch
    };

    // if (!this.reportFilter.Status) {
    //   toSendReq.Status = '';
    // }

    this.reportsService.getGiftRequestList(toSendReq).subscribe((allProtocolModuleRes:any) => {
      if(allProtocolModuleRes){
        this.statusList = allProtocolModuleRes.M_LookupsList;
        this.statusList[0].LookupsID = '';
        this.reportFilter.Status = '';
      }
    });
  }

  Download() {
    this.reportFilter.UserID = this.user.id;
    let dateVal = new Date(), cur_date = dateVal.getDate() + '-' + (dateVal.getMonth() + 1) + '-' + dateVal.getFullYear();
    this.reportsService.downloadModuleReport(this.reportFilter, 'gift').subscribe((resultBlob) => {
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = 'Gift Report-' + cur_date + '.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
      this.bsModalRef.hide();
    });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

}
