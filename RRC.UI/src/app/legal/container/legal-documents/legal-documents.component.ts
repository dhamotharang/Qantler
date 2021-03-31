import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { LegalDocumentsModalComponent } from 'src/app/modal/legal-documents-modal/legal-documents-modal.component';
import { BsModalService, BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { LegalService } from '../../service/legal.service';
import { Router } from '@angular/router';
import { UploadService } from 'src/app/shared/service/upload.service';

@Component({
  selector: 'app-legal-documents',
  templateUrl: './legal-documents.component.html',
  styleUrls: ['./legal-documents.component.scss']
})
export class LegalDocumentsComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('creationDateTemplate') creationDateTemplate: TemplateRef<any>;
  @ViewChild('fileDeletetemplate') fileDeletetemplate: TemplateRef<any>;
  @ViewChild('template') template: TemplateRef<any>;
  bsModalRef: BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  filterBy: any = {
    Creator: null,
    SmartSearch: null
  };
  isLegalDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 16;
  isLegalDepartmentTeamUserID = this.currentUser.OrgUnitID == 16 && !this.currentUser.IsOrgHead;
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
  requestTypeList: Array<any> = [];
  isApiLoading: boolean = false;
  documentData: any;
  lang: string = 'en';
  

  constructor(private modalService: BsModalService,
    private router: Router, private legalService: LegalService, private common: CommonService, private uploadService: UploadService) {
    this.common.breadscrumChange('Legal Management', 'Documents List Page', '');
    if (this.common.language != 'English') {
      this.lang = 'ar';
    }
  }

  ngOnInit() {
    this.columns = [
      { name: 'Filename', prop: 'AttachmentsName' },
      { name: 'Created By', prop: 'CreatedBy' },
      { name: 'Uploaded Date', cellTemplate: this.creationDateTemplate },
      { name: 'Action', cellTemplate: this.actionTemplate },
    ];
    if (this.common.language != 'English') {
      this.lang = 'ar';
      this.common.breadscrumChange(this.arabic('legalmanagement'), this.arabic('documentlistpage'), '');
      this.columns = [
        { name: this.arabic('filename'), prop: 'AttachmentsName' },
        { name: this.arabic('createdby'), prop: 'CreatedBy' },
        { name: this.arabic('uploadeddate'), cellTemplate: this.creationDateTemplate },
        { name: this.arabic('action'), cellTemplate: this.actionTemplate },
      ];
    }
    this.loadLegalDocumentsList();
  }

  uploadNewFile() {
    this.bsModalRef = this.modalService.show(LegalDocumentsModalComponent);
    this.modalService.onHide.subscribe((uploaderEmit) => {
      this.config.page = 1;
      this.config.itemsPerPage = 10;
      this.config.maxSize = 10;
      this.config.paging = true;
      this.filterBy = {
        Creator: null,
        SmartSearch: null
      };
      this.loadLegalDocumentsList();
    });
  }

  closeDialog() {
    this.bsModalRef.hide();
  }

  loadLegalDocumentsList() {
    let toSendReq: any = {
      PageNumber: this.config.page,
      PageSize: this.config.itemsPerPage,
      UserID: this.currentUser.id,
      UserName: this.currentUser.username,
      Creator: this.filterBy.Creator,
      SmartSearch: this.filterBy.SmartSearch
    };
    this.legalService.getLegalDocumentsList(toSendReq).subscribe((allDocsRes: any) => {
      if (allDocsRes) {
        this.rows = allDocsRes.Collection;
        this.statusList = allDocsRes.M_LookupsList;
        this.config.totalItems = allDocsRes.Count;
      }
    });
  }

  onChangePage(config, event) {
    this.loadLegalDocumentsList();
  }

  onSearch() {
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadLegalDocumentsList();
  }


  viewData(value) {
    let viewRouterUrl = '';
    if (viewRouterUrl != '') {
      this.router.navigate([viewRouterUrl + value.ID]);
    }
  }

  openReport() {
    let initialState = {};
    this.bsModalRef = this.modalService.show(LegalDocumentsModalComponent, Object.assign({}, {}, { initialState }));
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

  deleteLegalDocument() {
    if (this.documentData) {
      this.isApiLoading = true;
      let reqData = {
        AttachmentID: this.documentData.AttachmentID,
        UserID: this.currentUser.id
      };
      this.legalService.deleteLegalDocument(reqData).subscribe((LegalDocRes: any) => {
        this.isApiLoading = false;
        if (LegalDocRes) {
          this.loadLegalDocumentsList();
          this.bsModalRef.hide();
        }
      });
    }
  }

  openDeleteDocumentDialog(doucmentDetails) {
    this.documentData = doucmentDetails;
    this.bsModalRef = this.modalService.show(this.fileDeletetemplate);
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

}
