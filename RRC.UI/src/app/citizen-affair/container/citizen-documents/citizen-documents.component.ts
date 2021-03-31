import { Component, OnInit, ViewChild, TemplateRef, AfterViewInit } from '@angular/core';
import { BsModalRef, BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/common.service';
import { CitizenDocumentsModalComponent } from 'src/app/shared/modal/citizen-documents-modal/citizen-documents-modal.component';
import { UploadService } from 'src/app/shared/service/upload.service';
import { CitizenAffairService } from '../../service/citizen-affair.service';

@Component({
  selector: 'app-citizen-documents',
  templateUrl: './citizen-documents.component.html',
  styleUrls: ['./citizen-documents.component.scss']
})
export class CitizenDocumentsComponent implements OnInit {

  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('creationDateTemplate') creationDateTemplate: TemplateRef<any>;
  @ViewChild('fileDeletetemplate') fileDeletetemplate: TemplateRef<any>;
  @ViewChild('template') template: TemplateRef<any>;
  bsModalRef: BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  IsDocumentLoad: boolean = false;
  filterBy: any = {
    Creator: null,
    SmartSearch: null
  };

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
    dateInputFormat: 'DD-MM-YYYY'
  };
  statusList: Array<any>;
  requestTypeList: Array<any> = [];
  isApiLoading: boolean = false;
  documentData: any;
  language: any;
  isCurrentUnit: number;

  constructor(private modalService: BsModalService,
    private router: Router, private service: CitizenAffairService, private common: CommonService, private uploadService: UploadService) {
    this.language = this.common.language;
    this.common.breadscrumChange('Citizen Affair', 'Documents List Page', '');
    if (this.language != 'English')
      this.common.breadscrumChange(this.arabic('citizenaffair'), this.arabic('documentlistpage'), '');
  }

  ngOnInit() {
    this.getRowClass();
    this.isCurrentUnit = this.service.unitId.find(res => res == this.currentUser.OrgUnitID);

    if (this.isCurrentUnit) {
      this.IsDocumentLoad = true;
    }
    var filename = (this.common.language == 'English') ? 'Filename' : this.arabic('filename'),
      createdby = (this.common.language == 'English') ? 'Created By' : this.arabic('createdby'),
      upload = (this.common.language == 'English') ? 'Uploaded Date' : this.arabic('upload'),
      action = (this.common.language == 'English') ? 'Action' : this.arabic('action');
    this.columns = [
      // { name: 'Document ID', prop: 'AttachmentID' },
      { name: filename, prop: 'AttachmentsName' },
      { name: createdby, prop: 'CreatedBy' },
      { name: upload, cellTemplate: this.creationDateTemplate },
      { name: action, cellTemplate: this.actionTemplate },
    ];

    let th = this;
    // let reqTypes = Object.keys(HrRequestTypes);
    // Object.keys(HrRequestTypes).slice(reqTypes.length/2).forEach((type)=>{
    //   th.requestTypeList.push({value:HrRequestTypes[type],label:type});
    // });

    this.loadHrDocumentsList();
  }

  uploadNewFile() {
    this.bsModalRef = this.modalService.show(CitizenDocumentsModalComponent);
    this.modalService.onHide.subscribe((uploaderEmit) => {
      this.config.page = 1;
      this.config.itemsPerPage = 10;
      this.config.maxSize = 10;
      this.config.paging = true;
      this.filterBy = {
        Creator: null,
        SmartSearch: null
      };
      this.loadHrDocumentsList();
    });
  }

  closeDialog() {
    this.bsModalRef.hide();
  }

  getRowClass() {
    var lang = this.common.language;
    var lanClass = '';
    if (lang && lang != 'English') {
      lanClass = 'ar-rtl'
    }
    return lanClass;
  }
  loadHrDocumentsList() {
    let toSendReq: any = {
      PageNumber: this.config.page,
      PageSize: this.config.itemsPerPage,
      UserID: this.currentUser.id,
      UserName: this.currentUser.username,
      Creator: this.filterBy.Creator,
      SmartSearch: this.filterBy.SmartSearch
    };
    this.service.getHrDocumentsList(toSendReq).subscribe((allDocsRes: any) => {
      if (allDocsRes) {
        this.rows = allDocsRes.Collection;
        this.statusList = allDocsRes.M_LookupsList;
        this.config.totalItems = allDocsRes.Count;
      }
    });
  }

  onChangePage(config, event) {
    this.loadHrDocumentsList();
  }

  onSearch() {
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadHrDocumentsList();
  }


  viewData(value) {
    let viewRouterUrl = '';
    if (viewRouterUrl != '') {
      this.router.navigate([viewRouterUrl + value.ID]);
    }
  }

  // openReport() {
  //   let initialState = { };
  //   this.bsModalRef = this.modalService.show(CitizenDocumentsModalComponent,Object.assign({}, {}, { initialState }));
  // }

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
      // this.bsModalRef.hide();
    });
  }

  deleteDocument() {
    if (this.documentData) {
      this.isApiLoading = true;
      let reqData = {
        AttachmentID: this.documentData.AttachmentID,
        UserID: this.currentUser.id
      };
      this.service.deleteHrDocument(reqData).subscribe((HrDocRes: any) => {
        this.isApiLoading = false;
        if (HrDocRes) {
          this.loadHrDocumentsList();
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
