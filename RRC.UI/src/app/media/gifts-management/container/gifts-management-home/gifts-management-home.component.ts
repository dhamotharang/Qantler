import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { GiftsManagementService } from '../../service/gifts-management.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { CommonService } from 'src/app/common.service';
import { GiftReportModalComponent } from 'src/app/modal/gift-report-modal/gift-report-modal.component';

@Component({
  selector: 'app-gifts-management-home',
  templateUrl: './gifts-management-home.component.html',
  styleUrls: ['./gifts-management-home.component.scss']
})
export class GiftsManagementHomeComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('template') template : TemplateRef<any>;
  bsModalRef:BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  filterBy:any = {
    Status:null,
    GiftType:null,
    RecievedPurchasedBy:null,
    SmartSearch:null
  };
  isProtocolDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 4;
  isProtocolDepartmentTeamUserID = this.currentUser.OrgUnitID == 4 && !this.currentUser.IsOrgHead;
  rows: Array<any> = [];
  columns: Array<any> = [];
  arabicColumns: Array<any> = [];
  config: any = {
    paging: true,
    page: 1,
    maxSize: 10,
    itemsPerPage:10,
    totalItems:0
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  statusList:Array<any>;
  requestTypeList:Array<any> = [];
  isApiLoading:boolean = false;
  lang:string;
  giftTypeList:Array<any> = [];
  tableMessages: { emptyMessage: any; };
  constructor(private modalService: BsModalService,
    private router: Router,
    private giftsManagementService:GiftsManagementService,
    private common: CommonService,
    public arabicService:ArabicDataService) {
      if(this.common.currentLang == 'en'){
        // this.common.breadscrumChange('Gifts Management','List Page','');
        this.common.breadscrumChange('Gift','Gift List Page','');
        this.common.topBanner(true, 'Dashboard', '+ CREATE REQUEST', '/'+this.common.currentLang+'/app/media/gifts-management/request-create');
      } else if(this.common.currentLang == 'ar'){
        this.common.breadscrumChange(this.arabic('gift'), this.arabic('giftlistpage'), '');
        this.common.topBanner(true, this.arabic('dashboard'), '+ طلبات إنشاء', '/'+this.common.currentLang+'/app/media/gifts-management/request-create');
      }
    }

  ngOnInit() {
    this.filterBy.Status = '';
    this.filterBy.GiftType = '';
    this.tableMessages = {
      emptyMessage: this.common.currentLang === 'en' ? 'No Items Found' : this.arabic('noItemsFound')
    }
    this.columns = [
      { name: 'Gift Ref.No', prop: 'ReferenceNumber' },
      { name: 'Gift Type', prop: 'GiftType' },
      { name: 'Received from / Purchased by', prop: 'PurchasedBy' },
      { name: 'Status', prop: 'Status' },
      { name: 'Action', cellTemplate: this.actionTemplate },
    ];
    this.arabicColumns = [
      { name: 'الرقم المرجعي للهدية', prop: 'ReferenceNumber' },
      { name: 'نوع الهدية', prop: 'GiftType' },
      { name: 'وردت من / تم شراؤها بواسطة', prop: 'PurchasedBy' },
      { name: 'الحالة', prop: 'Status' },
      { name: 'عرض', cellTemplate: this.actionTemplate },
    ];

    this.lang = this.common.currentLang;
    if(this.lang == 'ar'){
      // this.columns = [
      //   { name: 'مرجع معرف', prop: 'ReferenceNumber' },
      //   { name: 'المنشئ', prop: 'Creator' },
      //   { name: 'نوع الطلب', prop: 'RequestType' },
      //   { name: 'الحالة', prop: 'Status' },
      //   { name: 'تاريخ الطلب', cellTemplate:this.creationDateTemplate },
      //   { name: 'عمل', cellTemplate: this.actionTemplate },
      // ];
      this.giftTypeList =[{label: this.arabic('all'), value:''},{label: this.arabic('giftreceived'), value:1},{label:this.arabic('giftpushed'), value:2}];
    }else if(this.lang == 'en'){
      // this.columns = [
      //   { name: 'Ref ID', prop: 'ReferenceNumber' },
      //   { name: 'Creator', prop: 'Creator' },
      //   { name: 'Request Type', prop: 'RequestType' },
      //   { name: 'Status', prop: 'Status' },
      //   { name: 'Request Date', cellTemplate:this.creationDateTemplate },
      //   { name: 'Action', cellTemplate: this.actionTemplate },
      // ];
      this.giftTypeList =[{label: 'All',value:''},{label:'Gifts Received', value:1},{label:'Gifts Purchased', value:2}];
    }
    this.loadListBasedOnRequestType();
  }

  closeDialog(){
    this.bsModalRef.hide();
  }

  loadListBasedOnRequestType(requestCode?:any){
    let toSendReq:any = {
      PageNumber:this.config.page,
      PageSize:this.config.itemsPerPage,
      UserID:this.currentUser.id,
      Status:this.filterBy.Status,
      GiftType:this.filterBy.GiftType,
      UserName:this.currentUser.username,
      RecievedPurchasedBy:this.filterBy.RecievedPurchasedBy,
      SmartSearch:this.filterBy.SmartSearch
    };

    // if(!this.filterBy.GiftType){
    //   toSendReq.GiftType = 'All';
    //   this.filterBy.GiftType = 'All';
    // }

    this.giftsManagementService.getGiftRequestList(toSendReq).subscribe((allProtocolModuleRes:any) => {
      if(allProtocolModuleRes){
        this.rows = allProtocolModuleRes.Collection;
        this.statusList = [].concat(allProtocolModuleRes.M_LookupsList);
        this.config.totalItems = allProtocolModuleRes.Count;
        this.statusList[0].LookupsID = '';
        // this.filterBy.Status = '';
        // this.filterBy.GiftType = '';
      }
    });
  }


  onChangePage(config,event){
    this.loadListBasedOnRequestType();
  }

  onSearch(){
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;

    this.loadListBasedOnRequestType();
  }

  viewData(value:any){
    // let viewRouterUrl='';
    // if(viewRouterUrl != ''){
      this.router.navigate([`/${this.common.currentLang}/app/media/gifts-management/request-view/${value.GiftID}`]);
    // }
  }

  openReport() {
    this.bsModalRef = this.modalService.show(GiftReportModalComponent,{class:'modal-lg'});
  }

  removeWordSpaces(words:string){
   return  words.replace(/\s+/g, '');
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
