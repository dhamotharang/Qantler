import { Component, OnInit, ViewChild, TemplateRef, AfterViewInit } from '@angular/core';
import { BsModalRef, BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { HrService } from '../../service/hr.service';
import { CommonService } from 'src/app/common.service';
import { HrDocumentsModalComponent } from 'src/app/shared/modal/hr-documents-modal/hr-documents-modal.component';
import { UploadService } from 'src/app/shared/service/upload.service';
import { HrRequestTypes } from 'src/app/shared/enum/hr-request-types/hr-request-types.enum';

@Component({
  selector: 'app-hr-documents',
  templateUrl: './hr-documents.component.html',
  styleUrls: ['./hr-documents.component.scss']
})
export class HrDocumentsComponent implements OnInit {

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
  isHRDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 9;
  isHRDepartmentTeamUserID = this.currentUser.OrgUnitID == 9 && !this.currentUser.IsOrgHead;
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

  constructor(private modalService: BsModalService,
    private router: Router, private hrService:HrService, private common: CommonService,private uploadService:UploadService) {
      this.common.breadscrumChange('Human Resources','Documents List Page','');
    }

  ngOnInit() {
    this.common.homeScrollTop();
    this.columns = [
      // { name: 'Document ID', prop: 'AttachmentID' },
      { name: 'Filename', prop: 'AttachmentsName' },
      { name: 'Created By', prop: 'CreatedBy' },
      // { name: 'Request Type', prop: 'RequestType' },
      // { name: 'Filename', prop: 'Status' },
      { name: 'Uploaded Date', cellTemplate:this.creationDateTemplate },
      { name: 'Action', cellTemplate: this.actionTemplate },
    ];

    let th = this;
    let reqTypes = Object.keys(HrRequestTypes);
    Object.keys(HrRequestTypes).slice(reqTypes.length/2).forEach((type)=>{
      th.requestTypeList.push({value:HrRequestTypes[type],label:type});
    });

    this.loadHrDocumentsList();
  }

  uploadNewFile(){
    this.bsModalRef = this.modalService.show(HrDocumentsModalComponent);
    this.modalService.onHide.subscribe((uploaderEmit)=>{
        this.config.page = 1;
        this.config.itemsPerPage = 10;
        this.config.maxSize = 10;
        this.config.paging = true;
        this.filterBy = {
          Creator:null,
          SmartSearch:null
        };
        this.loadHrDocumentsList();
    });
  }

  closeDialog(){
    this.bsModalRef.hide();
  }

  loadHrDocumentsList(){
    let toSendReq:any = {
      PageNumber:this.config.page,
      PageSize:this.config.itemsPerPage,
      UserID:this.currentUser.id,
      UserName:this.currentUser.username,
      Creator:this.filterBy.Creator,
      SmartSearch:this.filterBy.SmartSearch
    };
    this.hrService.getHrDocumentsList(toSendReq).subscribe((allDocsRes:any) => {
      if(allDocsRes){
        this.rows = allDocsRes.Collection;
        this.statusList = allDocsRes.M_LookupsList;
        this.config.totalItems = allDocsRes.Count;
      }
    });
  }

  onChangePage(config,event){
    this.loadHrDocumentsList();
  }

  onSearch(){
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadHrDocumentsList();
  }


  viewData(value){
    let viewRouterUrl='';
    if(viewRouterUrl != ''){
      this.router.navigate([viewRouterUrl + value.ID]);
    }
  }

  openReport() {
    let initialState = { };
    this.bsModalRef = this.modalService.show(HrDocumentsModalComponent,Object.assign({}, {}, { initialState }));
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

  deleteHrDocument(){
    if(this.documentData){
      this.isApiLoading = true;
      let reqData = {
        AttachmentID:this.documentData.AttachmentID,
        UserID:this.currentUser.id
      };
      this.hrService.deleteHrDocument(reqData).subscribe((HrDocRes:any) =>{
        this.isApiLoading = false;
        if(HrDocRes){
          this.loadHrDocumentsList();
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
