import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { LegalService } from '../../service/legal.service';
import { CommonService } from 'src/app/common.service';
import { LegalReportModalComponent } from 'src/app/modal/legal-report-modal/legal-report-modal.component';
import { LegalRequestTypes } from 'src/app/shared/enum/legal-request-types/legal-request-types.enum';
import { element } from '@angular/core/src/render3';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {

  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('creationDateTemplate') creationDateTemplate: TemplateRef<any>;
  @ViewChild('template') template: TemplateRef<any>;
  bsModalRef: BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  filterBy = {
    Status: null,
    SourceOU: null,
    AttendedBy: null,
    ReqDateFrom: null,
    ReqDateTo: null,
    SmartSearch: null,
    Subject: null,
    Label: null,
    Type: null
  };
  isLegalDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 16;
  isLegalDepartmentTeamUserID = this.currentUser.OrgUnitID == 16 && !this.currentUser.IsOrgHead;
  rows: Array<any> = [];
  columns: Array<any> = [];
  legalRequestCardDetails: Array<any> = [];
  userCards: Array<any> = [];
  statusList: any[] = [];
  requestTypeList: any[] = [];
  departmentList: any[] = [];

  tableMessages: any = {
    emptyMessage: ''
  };

  config: any = {
    paging: true,
    pageNumber: 1,
    itemsPerPage: 10,
    maxSize: 10,
    totalItems: 0
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };

  validateStartEndDate: any = {
    isValid: true,
    msg: ''
  };
  constructor(protected modalService: BsModalService,
    private router: Router, protected legalService: LegalService, protected common: CommonService) {
    this.common.breadscrumChange('Legal', 'Home Page', '');
  }

  ngOnInit() {
    this.filterBy.Status = (this.common.language == 'English') ? 'All' : this.arabic('all');
    this.filterBy.SourceOU = (this.common.language == 'English') ? '' : '';
    this.tableMessages = {
      emptyMessage: (this.common.language == 'English') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
    if (this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID) {
      this.columns = [
        { name: 'Ref ID', prop: 'ReferenceNumber' },
        { name: 'Source', prop: 'SourceOU' },
        { name: 'Subject', prop: 'Subject' },
        { name: 'Status', prop: 'Status' },
        { name: 'Assigned To', prop: 'AssignedTo' },
        { name: 'Request Date', prop: 'RequestDate', cellTemplate: this.creationDateTemplate },
        { name: 'Submitted By', prop: 'Attendedby' },
        { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
      ];
    } else {
      this.columns = [
        { name: 'Ref ID', prop: 'ReferenceNumber' },
        { name: 'Source', prop: 'SourceOU' },
        { name: 'Subject', prop: 'Subject' },
        { name: 'Status', prop: 'Status' },
        { name: 'Request Date', prop: 'RequestDate', cellTemplate: this.creationDateTemplate },
        { name: 'Submitted By', prop: 'Attendedby' },
        { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
      ];
    }

    this.common.breadscrumChange('Legal', 'Home Page', '');
    this.common.topBanner(true, 'Legal Management Dashboard', '+ CREATE REQUEST', '/en/app/legal/request-create');

    if (this.common.language != 'English') {
      if (this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID) {
        this.columns = [
          { name: this.arabic('refid'), prop: 'ReferenceNumber' },
          { name: this.arabic('source'), prop: 'SourceOU' },
          { name: this.arabic('subject'), prop: 'Subject' },
          { name: this.arabic('status'), prop: 'Status' },
          { name: this.arabic('assignedto'), prop: 'AssignedTo' },
          { name: this.arabic('requestdate'), prop: 'RequestDate', cellTemplate: this.creationDateTemplate },
          { name: this.arabic('legalsubmittedby'), prop: 'Attendedby' },
          { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
        ];
      } else {
        this.columns = [
          { name: this.arabic('refid'), prop: 'ReferenceNumber' },
          { name: this.arabic('source'), prop: 'SourceOU' },
          { name: this.arabic('subject'), prop: 'Subject' },
          { name: this.arabic('status'), prop: 'Status' },
          { name: this.arabic('requestdate'), prop: 'RequestDate', cellTemplate: this.creationDateTemplate },
          { name: this.arabic('legalsubmittedby'), prop: 'Attendedby' },
          { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
        ];
      }
      this.common.breadscrumChange(this.arabic('legal'), this.arabic('homepage'), '');
      this.common.topBanner(true, this.arabic('dashboard'), '+ ' + this.arabic('createrequest'), '/ar/app/legal/request-create');
    }
    this.loadRequestCounts();
    // this.filterBy.AttendedBy = this.currentUser.username;
    let th = this;
    let reqTypes = Object.keys(LegalRequestTypes);
    Object.keys(LegalRequestTypes).slice(reqTypes.length / 2).forEach((type) => {
      if (type != 'My Own Requests') {
        th.requestTypeList.push({ value: LegalRequestTypes[type], label: type });
      }
    });
    if (this.isLegalDepartmentHeadUserID || this.isLegalDepartmentTeamUserID) {
      this.filterBy.Type = LegalRequestTypes['New/Resubmitted'];
    } else {
      // this.filterBy.Type = LegalRequestTypes['My Own Requests'];
      this.filterBy.Type = '';
    }
    this.loadLegalRequests();
    if (this.currentUser.OrgUnitID == '16') {
      this.legalRequestCardDetails = [
        {
          'image': 'assets/citizen-affair/tabs.png',
          'count': 0,
          'progress': 50,
          countType: 'New',
          requestType: LegalRequestTypes['New/Resubmitted'],
          requestTitle: (this.common.language == 'English') ? 'New/Reopened' : this.arabic('new/reopened'),
          showRequestType: true
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'progress': 50,
          countType: 'InProgressRequest',
          requestType: LegalRequestTypes['In Progress'],
          requestTitle: (this.common.language == 'English') ? 'In Progress' : this.arabic('inprogresscard'),
          showRequestType: true
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'progress': 50,
          countType: 'NeedMoreInfo',
          requestType: LegalRequestTypes['Need more info'],
          requestTitle: (this.common.language == 'English') ? 'Need More Info' : this.arabic('needmoreinfo'),
          showRequestType: true
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'progress': 50,
          countType: 'Closed',
          requestType: LegalRequestTypes['Closed'],
          requestTitle: (this.common.language == 'English') ? 'Closed/Rejected' : this.arabic('closedrejectedcard'),
          showRequestType: true
        }

      ];

    }
    else {
      this.legalRequestCardDetails = [
        {
          'image': 'assets/citizen-affair/tabs.png',
          'count': 0,
          'progress': 50,
          countType: 'MyPendingRequest',
          requestType: LegalRequestTypes['My Pending Actions'],
          requestTitle: (this.common.language == 'English') ? 'My Pending Actions' : this.arabic('mypendingactions'),
          showRequestType: true
        }, {
          'image': 'assets/citizen-affair/tabs.png',
          'count': 0,
          'progress': 50,
          countType: 'MyOwnRequest',
          requestType: LegalRequestTypes['My Own Requests'],
          requestTitle: (this.common.language == 'English') ? 'My Own Requests' : this.arabic('myownrequests'),
          showRequestType: true
        }];
    }
    this.userCards = [
      {
        image: 'assets/hr-dashboard/inbox.png',
        title: (this.common.language == 'English') ? 'Legal Documents' : this.arabic('legaldocuments'),
        pageLink: '/' + this.common.currentLang + '/app/legal/documents',
      }
    ];
  }

  closeDialog() {
    this.bsModalRef.hide();
  }

  viewData(listItem) {
    if (this.common.language != 'English')
      this.router.navigate(['ar/app/legal/request-view/' + listItem.LegalID]);
    else
      this.router.navigate(['en/app/legal/request-view/' + listItem.LegalID]);
  }

  createLegalRequest() {
    if (this.common.language != 'English')
      this.router.navigate(['ar/app/legal/request-create']);
    else
      this.router.navigate(['en/app/legal/request-create']);
  }

  loadLegalRequests() {
    let toSendReq: any = {
      PageNumber: this.config.pageNumber,
      PageSize: this.config.itemsPerPage,
      UserID: this.currentUser.id,
      Status: (this.filterBy.Status == 'All' || this.filterBy.Status == this.arabic('all')) ? null : this.filterBy.Status,
      SourceOU: (this.filterBy.SourceOU == '') ? null : this.filterBy.SourceOU,
      UserName: this.currentUser.username,
      AttendedBy: this.filterBy.AttendedBy,
      SmartSearch: this.filterBy.SmartSearch,
      Subject: this.filterBy.Subject,
      Label: this.filterBy.Label,
      Type: this.filterBy.Type
    };

    if (this.filterBy.ReqDateFrom) {
      toSendReq.ReqDateFrom = new Date(this.filterBy.ReqDateFrom).toJSON();
    }

    if (this.filterBy.ReqDateTo) {
      toSendReq.ReqDateTo = new Date(this.filterBy.ReqDateTo).toJSON();
    }

    this.legalService.getAllLegalRequestsList(toSendReq).subscribe((allOwnRes: any) => {
      if (allOwnRes) {
        let departmentlist = [];
        let statusList = [];
        this.rows = allOwnRes.Collection;
        this.statusList = allOwnRes.M_LookupsList;
        this.config.totalItems = allOwnRes.Count;
        if (this.common.language == 'English') {
          statusList.push({ DisplayName: 'All' });
          departmentlist.push({ OrganizationID: '', OrganizationUnits: "All" });
          allOwnRes.M_OrganizationList.forEach((element) => {
            departmentlist.push(element);
          });
          this.departmentList = departmentlist;
          allOwnRes.M_LookupsList.forEach((element) => {
            statusList.push(element);
          });
          this.statusList = statusList;
        }
        if (this.common.language != 'English') {
          this.rows.forEach((rows) => {
            this.statusList.forEach((StatusLookup) => {
              if (StatusLookup.DisplayName == rows.Status) {
                rows.StatusCode = StatusLookup.LookupsID;
              }
            });
          });
          statusList.push({ DisplayName: this.arabic('all') });
          departmentlist.push({ OrganizationID: '', OrganizationUnits: this.arabic('all') });
          allOwnRes.M_OrganizationList.forEach((element) => {
            departmentlist.push(element);
          });
          this.departmentList = departmentlist;
          allOwnRes.M_LookupsList.forEach((element) => {
            statusList.push(element);
          });
          this.statusList = statusList;
        }
      }
    });
  }

  loadRequestCounts() {
    this.legalService.getLegalRequestsCount(this.currentUser.id).subscribe((modCountRes: any) => {
      if (modCountRes) {
        this.legalRequestCardDetails.forEach((cardObj) => {
          if (modCountRes[cardObj.countType] && modCountRes[cardObj.countType] > 0) {
            cardObj.count = modCountRes[cardObj.countType];
          } else {
            cardObj.count = 0;
          }
        });
      }
    });
  }

  onChangePage(config, event) {
    this.loadLegalRequests();
  }

  onSearch() {
    this.config.pageNumber = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadLegalRequests();
  }

  reqSelect(reqType?: string) {
    this.config.pageNumber = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.filterBy = {
      Status: (this.common.language == 'English') ? 'All' : this.arabic('all'),
      SourceOU: (this.common.language == 'English') ? '' : '',
      AttendedBy: null,
      ReqDateFrom: null,
      ReqDateTo: null,
      SmartSearch: null,
      Subject: null,
      Label: null,
      Type: reqType
    };
    this.loadLegalRequests();
    this.legalService.triggerScrollTo();
  }

  openReport() {
    let initialState = {};
    this.bsModalRef = this.modalService.show(LegalReportModalComponent, { class: 'modal-lg' });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  dateValidation() {
    this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
    let showDateValidationMsg = false;
    if (!this.filterBy.ReqDateFrom && this.filterBy.ReqDateTo) {
      showDateValidationMsg = false;
    } else if (this.filterBy.ReqDateFrom && this.filterBy.ReqDateTo) {
      let startDate = new Date(this.filterBy.ReqDateFrom).getTime();
      let endDate = new Date(this.filterBy.ReqDateTo).getTime();
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
