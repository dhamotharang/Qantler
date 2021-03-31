import { ArabicDataService } from './../../../arabic-data.service';
import { Component, OnInit, ViewChild, TemplateRef, AfterViewInit } from '@angular/core';
import { BsModalRef, BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { ItService } from '../../service/it.service';
import { CommonService } from 'src/app/common.service';
import { UploadService } from 'src/app/shared/service/upload.service';
import { ItDocumentsModalComponent } from 'src/app/modal/it-documents-modal/it-documents-modal.component';

@Component({
  selector: 'app-it-documents',
  templateUrl: './it-documents.component.html',
  styleUrls: ['./it-documents.component.scss']
})
export class ItDocumentsComponent implements OnInit {

  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('creationDateTemplate') creationDateTemplate: TemplateRef<any>;
  @ViewChild('fileDeletetemplate') fileDeletetemplate:TemplateRef<any>;
  @ViewChild('template') template : TemplateRef<any>;
  bsModalRef:BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  filterBy:any = {
    Creator:null,
    SmartSearch:null
  };
  isITDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 11;
  isITDepartmentTeamUserID = this.currentUser.OrgUnitID == 11 && !this.currentUser.IsOrgHead;
  rows: Array<any> = [];
  columns: Array<any> = [];
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
  documentData:any;
  lang: string;
  constructor(
    private modalService: BsModalService,
    private router: Router,
    private itService: ItService,
    private common: CommonService,
    public arabic: ArabicDataService,
    private uploadService: UploadService
    ) { this.lang = this.common.currentLang; }

  ngOnInit() {
    if (this.lang === 'en') {
      this.common.breadscrumChange('IT Support','Documents List Page','');
    } else {
      this.common.breadscrumChange('IT Support', 'قائمة الوثائق الصفحة', '');
    }
    this.columns = [
      // { name: 'Document ID', prop: 'AttachmentID' },
      { name: 'Filename', prop: 'AttachmentsName' },
      { name: 'Created By', prop: 'CreatedBy' },
      // { name: 'Request Type', prop: 'RequestType' },
      // { name: 'Filename', prop: 'Status' },
      { name: 'Uploaded Date', cellTemplate:this.creationDateTemplate },
      { name: 'Action', cellTemplate: this.actionTemplate },
    ];

    this.loadItDocumentsList();
  }

  loadItDocumentsList(){
    let toSendReq:any = {
      PageNumber:this.config.page,
      PageSize:this.config.itemsPerPage,
      UserID:this.currentUser.id,
      UserName:this.currentUser.username,
      Creator:this.filterBy.Creator,
      SmartSearch:this.filterBy.SmartSearch
    };
    this.itService.getItDocumentsList(toSendReq).subscribe((allDocsRes:any) => {
      if(allDocsRes){
        this.rows = allDocsRes.Collection;
        this.statusList = allDocsRes.M_LookupsList;
        this.config.totalItems = allDocsRes.Count;
      }
    });
  }

  uploadNewFile(){
    this.bsModalRef = this.modalService.show(ItDocumentsModalComponent);
    this.modalService.onHide.subscribe((uploaderEmit)=>{
        this.config.page = 1;
        this.config.itemsPerPage = 10;
        this.config.maxSize = 10;
        this.config.paging = true;
        this.filterBy = {
          Creator:null,
          SmartSearch:null
        };
        this.loadItDocumentsList();
    });
  }

  onChangePage(config,event){
    this.loadItDocumentsList();
  }

  onSearch(){
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadItDocumentsList();
  }

  viewData(value){
    let viewRouterUrl='';
    if(viewRouterUrl != ''){
      this.router.navigate([viewRouterUrl + value.ID]);
    }
  }

  openReport() {
    let initialState = { };
    this.bsModalRef = this.modalService.show(ItDocumentsModalComponent,Object.assign({}, {}, { initialState }));
  }

  downloadFile(fileDetails){
    let fileData = {
      AttachmentsName:fileDetails.AttachmentsName,
      AttachmentGuid:fileDetails.AttachmentGuid
    };
    let dateVal = new Date(), cur_date = dateVal.getDate() +'-'+(dateVal.getMonth()+1)+'-'+dateVal.getFullYear();
    this.uploadService.downloadAttachment(fileData).subscribe((resultBlob) =>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = cur_date+ '-' +fileDetails.AttachmentsName;
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
      // this.bsModalRef.hide();
    });
  }

  deleteItDocument(){
    if(this.documentData){
      this.isApiLoading = true;
      let reqData = {
        AttachmentID:this.documentData.AttachmentID,
        UserID:this.currentUser.id
      };
      this.itService.deleteItDocument(reqData).subscribe((HrDocRes:any) =>{
        this.isApiLoading = false;
        if(HrDocRes){
          this.loadItDocumentsList();
          this.bsModalRef.hide();
        }
      });
    }
  }

  openDeleteDocumentDialog(doucmentDetails){
    this.documentData = doucmentDetails;
    this.bsModalRef = this.modalService.show(this.fileDeletetemplate);
  }
}
