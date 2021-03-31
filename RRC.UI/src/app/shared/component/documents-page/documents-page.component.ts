import { Component, OnInit, TemplateRef, ViewChild, ElementRef, Inject, Renderer2 } from '@angular/core';
import { DocumentsPageService } from '../../service/documents-page.service';
import { CommonService } from 'src/app/common.service';
import { UploadService } from '../../service/upload.service';
import { BsModalService, BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { DOCUMENT } from '@angular/platform-browser';
import { HttpEventType } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-documents-page',
  templateUrl: './documents-page.component.html',
  styleUrls: ['./documents-page.component.scss']
})
export class DocumentsPageComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('creationDateTemplate') creationDateTemplate: TemplateRef<any>;
  @ViewChild('fileDeleteTemplate') fileDeleteTemplate: TemplateRef<any>;
  @ViewChild('fileUploadTemplate') fileUploadTemplate: TemplateRef<any>;
  @ViewChild('template') template: TemplateRef<any>;
  @ViewChild('modalFileInput') fileInput: ElementRef;
  bsModalRef: BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  filterBy: any = {
    Creator: null,
    SmartSearch: null
  };
  isDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 9;
  isDepartmentTeamUserID = this.currentUser.OrgUnitID == 9 && !this.currentUser.IsOrgHead;
  rows: Array<any> = [];
  columns: Array<any> = [];
  config: any = {
    paging: true,
    page: 1,
    maxSize: 10,
    itemsPerPage: 10,
    totalItems: 0
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  statusList: Array<any>;
  isApiLoading: boolean = false;
  documentData: any;
  moduleName: string;
  documentType: string;
  requestTypeList: Array<any> = [];
  uploadPercentage: number = 0;
  img_file: any[] = [];
  uploadProcess: boolean = false;
  attachments: any[] = [];
  requestType: any;
  message: string;
  lang: string = 'ar';
  uploadheader :any;
  tableMessages:any;

  constructor(
    private modalService: BsModalService, 
    private documentsPageService: DocumentsPageService,
    private common: CommonService,
    private uploadService: UploadService,
    private route: ActivatedRoute,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document,
    private endpointService:EndPointService) {
    this.lang = this.common.currentLang;
    if(this.moduleName == 'Legal Services'){
      this.common.breadscrumChange('Legal', 'Documents List Page', '');
    }else{
    this.common.breadscrumChange(this.moduleName, 'Documents List Page', '');
    }
    this.common.topBanner(false,'','','');
    this.tableMessages = {
      emptyMessage: this.lang === 'en' ? 'No Items Found' : this.arabic('noItemsFound')
    }
  }

  ngOnInit() {
    this.columns = [
      { name: this.lang == 'ar' ? this.arabic('filename') : 'Filename', prop: 'AttachmentsName' },
      { name: this.lang == 'ar' ? this.arabic('createdby') : 'Created By', prop: 'CreatedBy' }, 
      { name: this.lang == 'ar' ? this.arabic('uploadedDate') : 'Uploaded Date', cellTemplate: this.creationDateTemplate },
      { name: this.lang == 'ar' ? this.arabic('action') : 'Action', cellTemplate: this.actionTemplate },
    ];
    this.route.url.subscribe(() => {
      this.moduleName = this.route.snapshot.data.department;
      
      this.documentType = this.route.snapshot.data.documentType;
      if(this.moduleName == 'Legal Services'){
        this.common.breadscrumChange('Legal', 'Documents List Page', '');
      }else{
      this.common.breadscrumChange(this.moduleName, 'Documents List Page', '');
      }
      this.uploadheader='Upload'+' '+this.moduleName+' '+ 'Documents';
      this.common.topBanner(false,'','','');
      this.isDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.department == this.route.snapshot.data.department;
      this.isDepartmentTeamUserID = this.currentUser.department == this.route.snapshot.data.department && !this.currentUser.IsOrgHead;
      if(this.route.snapshot.data.department == 'Protocol' && this.currentUser.OrgUnitID == 17){
        this.isDepartmentHeadUserID = true;
        this.isDepartmentTeamUserID = true;
      }
      if(this.route.snapshot.data.department == 'Vehicle Management' && this.currentUser.OrgUnitID == 13){
        this.isDepartmentHeadUserID = true;
        this.isDepartmentTeamUserID = true;
      }
      this.loadModuleDocumentsList();
    });
    if (this.lang == 'ar') {
      this.uploadheader=this.arabic('upload') +' '+this.arabic(this.moduleName.trim().replace(/\s+/g, '').toLowerCase()) +' '+this.arabic('documents');
      if(this.moduleName == 'Legal Services'){
        this.common.breadscrumChange(this.arabic('legal'), this.arabic('documentlistpage'), '');
      }else{
        this.common.breadscrumChange(this.arabic((this.moduleName.trim().replace(/\s+/g, '').toLowerCase())), this.arabic('documentlistpage'), '');
      }
    }
  }

  uploadNewFile() {
    this.attachments.splice(0, this.attachments.length);
    this.bsModalRef = this.modalService.show(this.fileUploadTemplate);
    this.modalService.onHide.subscribe(() => {
      this.config.page = 1;
      this.config.itemsPerPage = 10;
      this.config.maxSize = 10;
      this.config.paging = true;
      this.filterBy = {
        Creator: null,
        SmartSearch: null
      };
      this.loadModuleDocumentsList();
    });
  }

  closeDialog() {
    this.bsModalRef.hide();
  }

  loadModuleDocumentsList() {
    let toSendReq: any = {
      PageNumber: this.config.page,
      PageSize: this.config.itemsPerPage,
      UserID: this.currentUser.id,
      UserName: this.currentUser.username,
      Creator: this.filterBy.Creator,
      SmartSearch: this.filterBy.SmartSearch
    };
    debugger;
    this.documentsPageService.getModuleDocumentsList(toSendReq, this.documentType).subscribe((allDocsRes: any) => {
      if (allDocsRes) {
        this.rows = allDocsRes.Collection;
        this.statusList = allDocsRes.M_LookupsList;
        this.config.totalItems = allDocsRes.Count;
      }
    });
  }

  onChangePage(config, event) {
    this.loadModuleDocumentsList();
  }

  onSearch() {
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadModuleDocumentsList();
  }

  downloadFile(fileDetails) {
    let fileData = {
      AttachmentsName: fileDetails.AttachmentsName,
      AttachmentGuid: fileDetails.AttachmentGuid
    };
    let dateVal = new Date(), cur_date = dateVal.getDate() + '-' + (dateVal.getMonth() + 1) + '-' + dateVal.getFullYear();
    this.uploadService.downloadAttachment(fileData).subscribe((resultBlob) => {
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = cur_date + '-' + fileDetails.AttachmentsName;
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
    });
  }

  deleteModuleDocument() {
    if (this.documentData) {
      this.isApiLoading = true;
      let reqData = {
        AttachmentID: this.documentData.AttachmentID,
        UserID: this.currentUser.id
      };
      this.documentsPageService.deleteModuleDocument(reqData, this.documentType).subscribe((ModuleDocRes: any) => {
        this.isApiLoading = false;
        if (ModuleDocRes) {
          this.loadModuleDocumentsList();
          this.bsModalRef.hide();
          // this.message = this.lang === 'en' ? 'File Deleted Successfully' : this.arabic('fileUpSuc');
          // this.modalService.show(this.template);
        }
      });
    }
  }

  openDeleteDocumentDialog(doucmentDetails) {
    this.documentData = doucmentDetails;
    this.bsModalRef = this.modalService.show(this.fileDeleteTemplate);
  }

  moduleAttachments(event) {
    this.img_file = event.target.files;
    if (this.img_file.length > 0) {
      this.isApiLoading = true;
      this.uploadProcess = true;
      let toSendFileData = {
        data: this.img_file[0],
      };
      this.uploadService.uploadModuleAttachment(toSendFileData).subscribe((attachementRes: any) => {
        this.isApiLoading = false;
        if (attachementRes.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(attachementRes.loaded / attachementRes.total) * 100;
        } else if (attachementRes.type === HttpEventType.Response) {
          this.isApiLoading = false;
          this.uploadProcess = false;
          this.uploadPercentage = 0;         
          this.attachments[0] = { 'AttachmentGuid': attachementRes.body.Guid, 'AttachmentsName': attachementRes.body.FileName[0] };
          this.fileInput.nativeElement.value = "";
        }
      });
    }
  }


  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.fileInput.nativeElement.value = "";
  }

  close() {
    this.bsModalRef.hide();
    this.fileInput.nativeElement.value = "";
  }

  closemodal() {
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    this.fileInput.nativeElement.value = "";
  }

  saveModuleAttachments() {
    let toSendFileData = {
      AttachmentGuid: this.attachments[0].AttachmentGuid,
      AttachmentsName: this.attachments[0].AttachmentsName,
      Type: this.documentType,
      CreatedBy: this.currentUser.id,
      CreatedDateTime: new Date().toJSON()
    };
    this.documentsPageService.sendModuleAttachments(toSendFileData, this.documentType).subscribe((uploadRes) => {
      if (uploadRes) {
        this.bsModalRef.hide();
        this.attachments = [];
        this.message = this.lang === 'en' ? 'File Uploaded Successfully' : this.arabic('fileUpSuc');
        this.modalService.show(this.template);
        this.fileInput.nativeElement.value = "";
        this.documentsPageService.documentUploadDetector.next(true);
      }
    });
  }
  arabic(word) {
    return this.common.arabic.words[word];
  }
}
