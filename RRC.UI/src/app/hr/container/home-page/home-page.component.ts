import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsModalService, BsDatepickerConfig, BsModalRef, Utils } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { HrService } from '../../service/hr.service';
import { CommonService } from 'src/app/common.service';
import { HrRequestTypes } from 'src/app/shared/enum/hr-request-types/hr-request-types.enum';
import { HrReportModalComponent } from 'src/app/shared/modal/hr-report-modal/hr-report-modal.component';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { UtilsService } from 'src/app/shared/service/utils.service';

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
  filterBy: any = {
    Status: 'All',
    RequestType: '',
    Creator: null,
    ReqDateFrom: null,
    ReqDateTo: null,
    SmartSearch: null
  };
  isHRDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.departmentID == 9;
  isHRDepartmentUser = this.currentUser.departmentID == 9;
  isHRDepartmentTeamUserID = this.currentUser.departmentID == 9 && !this.currentUser.IsOrgHead;
  rows: Array<any> = [];
  columns: Array<any> = [];

  hrProgresscardDetails = [
    {
      'image': 'assets/hr-dashboard/file.png',
      'count': 0,
      'progress': 0,
      countType: 'LeaveRequests',
      requestCode: HrRequestTypes['Leave Requests'],
      // requestType:'Leave',
      requestType: 'Leave Requests',
      isHRDepartmentRequest: true,
      viewLink: '/' + this.common.currentLang + '/app/hr/leave/request-view/'
    },
    {
      'image': 'assets/hr-dashboard/agreement.png',
      'count': 0,
      'progress': 0,
      countType: 'SalaryCertificate',
      requestCode: HrRequestTypes["Salary Certificate"],
      // requestType:'SalaryCertificate',
      requestType: 'Salary Certificate',
      isHRDepartmentRequest: true,
      viewLink: '/' + this.common.currentLang + '/app/hr/salary-certificate/view/'
    },
    {
      'image': 'assets/hr-dashboard/concept.png',
      'count': 0,
      'progress': 0,
      countType: 'ExperienceCertificate',
      requestCode: HrRequestTypes["Experience Certificate"],
      // requestType:'ExperienceCertificate',
      requestType: 'Experience Certificate',
      isHRDepartmentRequest: true,
      viewLink: '/' + this.common.currentLang + '/app/hr/experience-certificate/view/'
    },
    {
      'image': 'assets/hr-dashboard/notes.png',
      'count': 0,
      'progress': 0,
      requestCode: HrRequestTypes["New Baby Addition"],
      countType: 'NewBabyAddition',
      // requestType:'BabyAddition',
      requestType: 'New Baby Addition',
      isHRDepartmentRequest: true,
      viewLink: '/' + this.common.currentLang + '/app/hr/new-baby-addition/request-view/'
    },
    {
      'image': 'assets/hr-dashboard/layout.png',
      'count': 0,
      'progress': 0,
      requestCode: HrRequestTypes["Announcement Requests"],
      countType: 'AnnouncementRequests',
      // requestType:'Announcement',
      requestType: 'Announcement Requests',
      isHRDepartmentRequest: true,
      viewLink: '/' + this.common.currentLang + '/app/hr/announcement/announcement-view/'
    },
    {
      'image': 'assets/hr-dashboard/concept-1.png',
      'count': 0,
      'progress': 0,
      requestCode: HrRequestTypes["Training Requests"],
      countType: 'TrainingRequests',
      // requestType:'Training',
      requestType: 'Training Requests',
      isHRDepartmentRequest: true,
      viewLink: '/' + this.common.currentLang + '/app/hr/training-request/request-view/'
    },
    {
      'image': 'assets/hr-dashboard/file.png',
      'count': 0,
      'progress': 0,
      requestCode: HrRequestTypes["Official Requests"],
      countType: 'OfficialTaskRequests',
      // requestType:'OfficialTask',
      requestType: 'Official Requests',
      isHRDepartmentRequest: true,
      viewLink: '/' + this.common.currentLang + '/app/hr/official-tasks/request-view/official/'
    },
    {
      'image': 'assets/hr-dashboard/document.png',
      'count': 0,
      'progress': 0,
      requestCode: HrRequestTypes["Raise Complaints/ Suggestions"],
      countType: 'RaiseComplaintSuggestions',
      // requestType:'CompliantSuggestions',
      requestType: 'Raise Complaints/Suggestions',
      isHRDepartmentRequest: true,
      viewLink: '/' + this.common.currentLang + '/app/hr/raise-complaint-suggestion-view/'
    },
    // {
    //   'image':'assets/employee-profile/insurance.png',
    //   'count': 0,
    //   'progress': 0,
    //   countType:'MyPendingActions',
    //   requestType:'MyPendingRequests',
    //   requestName:'My Pending Requests',
    //   forDepartment:false
    // },
    // {
    //   'image':'assets/employee-profile/passport.png',
    //   'count': 0,
    //   'progress': 0,
    //   countType:'MyRequests',
    //   requestType:'MyOwnRequests',
    //   requestName:'My Own Requests',
    //   forDepartment:false
    // }
  ];

  commonRequests: any = {
    MyPendingActions: {
      'image': 'assets/employee-profile/insurance.png',
      'count': 0,
      'progress': 0,
      countType: 'MyPendingActions',
      requestType: 'MyPendingRequests',
      requestName: 'My Pending Actions',
      isHRDepartmentRequest: false,
      isReqSelected: false
    },
    MyOwnRequests: {
      'image': 'assets/employee-profile/passport.png',
      'count': 0,
      'progress': 0,
      countType: 'MyRequests',
      requestType: 'MyOwnRequests',
      requestName: 'My Own Requests',
      isHRDepartmentRequest: false,
      isReqSelected: false
    },
    MyProcessedRequests: {
      'image': 'assets/employee-profile/passport.png',
      'count': 0,
      'progress': 0,
      countType: 'MyProcessedRequests',
      requestType: 'MyProcessedRequests',
      requestName: 'My Processed Request',
      isHRDepartmentRequest: false,
      isReqSelected: false
    }
  };

  userCards: any = [];

  hrCreateModalDetails = [
    {
      'image': 'assets/hr-dashboard/file.png',
      requestType: 'Leave Requests',
      requestCode: HrRequestTypes['Leave Requests'],
      createLink: '/' + this.common.currentLang + '/app/hr/leave/request-create'
    },
    {
      'image': 'assets/hr-dashboard/agreement.png',
      requestType: 'Salary Certificate',
      requestCode: HrRequestTypes["Salary Certificate"],
      createLink: '/' + this.common.currentLang + '/app/hr/salary-certificate/create'
    },
    {
      'image': 'assets/hr-dashboard/concept.png',
      requestType: 'Experience Certificate',
      requestCode: HrRequestTypes["Experience Certificate"],
      createLink: '/' + this.common.currentLang + '/app/hr/experience-certificate/create'
    },
    {
      'image': 'assets/hr-dashboard/notes.png',
      requestType: 'New Baby Addition',
      requestCode: HrRequestTypes["New Baby Addition"],
      createLink: '/' + this.common.currentLang + '/app/hr/new-baby-addition/request-create'
    },
    {
      'image': 'assets/hr-dashboard/layout.png',
      requestType: 'Announcement Requests',
      requestCode: HrRequestTypes["Announcement Requests"],
      createLink: '/' + this.common.currentLang + '/app/hr/announcement/announcement-create'
    },
    {
      'image': 'assets/hr-dashboard/concept-1.png',
      requestType: 'Training Requests',
      requestCode: HrRequestTypes["Training Requests"],
      createLink: '/' + this.common.currentLang + '/app/hr/training-request/request-create'
    },
    {
      'image': 'assets/hr-dashboard/file.png',
      requestType: 'Official Requests',
      requestCode: HrRequestTypes["Official Requests"],
      createLink: '/' + this.common.currentLang + '/app/hr/official-tasks/request-create'
    },
    {
      'image': 'assets/hr-dashboard/document.png',
      requestType: 'Raise Complaints/Suggestions',
      requestCode: HrRequestTypes["Raise Complaints/ Suggestions"],
      createLink: '/' + this.common.currentLang + '/app/hr/raise-complaint-suggestion'
    }
  ];

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
  statusids: any = [];
  statusList: Array<any>;
  requestTypeList: Array<any> = [];
  isApiLoading: boolean = false;
  lang: string;
  tableMessages: any = {
    emptyMessage: ''
  };

  validateStartEndDate: any = {
    isValid: true,
    msg: ''
  };
  progress = false;
  constructor(private modalService: BsModalService,
    private router: Router,
    private hrService: HrService,
    public common: CommonService,
    public arabicService: ArabicDataService,
    public utilsService: UtilsService) {

  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabicfn('nodatatodisplay')
    };
    if (this.common.currentLang == 'en') {
      this.common.breadscrumChange('HR', 'Home Page', '');
      this.common.topBanner(true, 'Dashboard', '+ CREATE REQUESTS', 'hr/dashboard');
    } else if (this.common.currentLang == 'ar') {
      this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('homepage'), '');
      this.common.topBanner(true, this.arabicfn('dashboard'), '+ ' + this.arabicfn('createrequests'), 'hr/dashboard');
    }
    this.columns = [
      { name: 'Ref ID', prop: 'ReferenceNumber' },
      { name: 'Creator', prop: 'Creator' },
      { name: 'Request Type', prop: 'RequestType', width: 180 },
      { name: 'Status', prop: 'Status' },
      { name: 'Request Date', cellTemplate: this.creationDateTemplate },
      { name: 'Action', cellTemplate: this.actionTemplate },
    ];
    if (this.isHRDepartmentHeadUserID) {
      this.userCards = [
        {
          image: 'assets/hr-dashboard/curriculums.png',
          title: 'CV Bank',
          requestCode: HrRequestTypes['CV Bank'],
          pageLink: '/' + this.common.currentLang + '/app/hr/cv-bank/cv-bank-list',
          isOrgHeadOnly: false
        },
        {
          image: 'assets/hr-dashboard/approved.png',
          title: 'Employee Profile',
          requestCode: HrRequestTypes['Employee Profile'],
          pageLink: '/' + this.common.currentLang + '/app/hr/employee/list',
          isOrgHeadOnly: false
        },
        {
          image: 'assets/hr-dashboard/inbox.png',
          requestCode: 'HR Documents',
          title: 'HR Documents',
          pageLink: '/' + this.common.currentLang + '/app/hr/documents',
          isOrgHeadOnly: false
        }
      ];
    } else if (this.isHRDepartmentUser) {
      this.userCards = [
        {
          image: 'assets/hr-dashboard/curriculums.png',
          title: 'CV Bank',
          requestCode: HrRequestTypes['CV Bank'],
          pageLink: '/' + this.common.currentLang + '/app/hr/cv-bank/cv-bank-list',
          isOrgHeadOnly: false
        },
        {
          image: 'assets/hr-dashboard/approved.png',
          title: 'Employee Profile',
          requestCode: HrRequestTypes['Employee Profile'],
          pageLink: '/' + this.common.currentLang + '/app/hr/employee/list',
          isOrgHeadOnly: false
        },
        {
          image: 'assets/hr-dashboard/inbox.png',
          requestCode: 'HR Documents',
          title: 'HR Documents',
          pageLink: '/' + this.common.currentLang + '/app/hr/documents',
          isOrgHeadOnly: false
        }
      ];
    } else {
      this.userCards = [
        {
          image: 'assets/hr-dashboard/inbox.png',
          requestCode: 'HR Documents',
          title: 'HR Documents',
          pageLink: '/' + this.common.currentLang + '/app/hr/documents',
          isOrgHeadOnly: false
        }
      ];
    }
    let th = this;
    let reqTypes = Object.keys(HrRequestTypes);
    Object.keys(HrRequestTypes).slice(reqTypes.length / 2).forEach((type) => {
      if (type != 'CV Bank' && type != 'Employee Profile') {
        th.requestTypeList.push({ value: HrRequestTypes[type], label: type });
      }
    });
    this.loadRequestCounts();
    if (this.isHRDepartmentHeadUserID || this.isHRDepartmentTeamUserID) {
      this.filterBy.RequestType = HrRequestTypes["Leave Requests"];
      this.loadListBasedOnRequestType(this.filterBy.RequestType);
    } else {
      this.commonRequests.MyPendingActions.isReqSelected = true;
      this.filterBy.Creator = '';
      this.loadMyPendingRequests();
    }

    this.hrProgresscardDetails.forEach((hpdObj) => {
      if (this.lang == 'ar') {
        if (hpdObj.requestCode == 10) {
          hpdObj.requestType = this.arabicService.words['raisecomplaintssuggestions'];
        } else if (hpdObj.requestCode == 2) {
          hpdObj.requestType = this.arabicService.words['newbabyadditioncardtitle'];
        } else if (hpdObj.requestCode == 3) {
          hpdObj.requestType = this.arabicService.words['salarycertificatecardtitle'];
        } else if (hpdObj.requestCode == 4) {
          hpdObj.requestType = this.arabicService.words['experiencecertificatecardtitle'];
        } else {
          hpdObj.requestType = this.arabicService.words[HrRequestTypes[hpdObj.requestCode].trim().replace(/\s+/g, '').toLowerCase()];
        }
      }
      if (this.lang == 'en') {
        hpdObj.requestType = HrRequestTypes[hpdObj.requestCode];
      }
    });
    this.hrCreateModalDetails.forEach((hcmdObj) => {
      if (this.lang == 'ar') {
        if (hcmdObj.requestCode == 10) {
          hcmdObj.requestType = this.arabicService.words['raisecomplaintssuggestions'];
        } else if (hcmdObj.requestCode == 2) {
          hcmdObj.requestType = this.arabicService.words['newbabyadditioncardtitle'];
        } else if (hcmdObj.requestCode == 3) {
          hcmdObj.requestType = this.arabicService.words['salarycertificatecardtitle'];
        } else if (hcmdObj.requestCode == 4) {
          hcmdObj.requestType = this.arabicService.words['experiencecertificatecardtitle'];
        } else {
          hcmdObj.requestType = this.arabicService.words[HrRequestTypes[hcmdObj.requestCode].trim().replace(/\s+/g, '').toLowerCase()];
        }
      }
      if (this.lang == 'en') {
        hcmdObj.requestType = HrRequestTypes[hcmdObj.requestCode];
      }
    });

    this.userCards.forEach((ucObj) => {
      if (this.lang == 'ar') {
        if (ucObj.requestCode != 'HR Documents') {
          ucObj.title = this.arabicService.words[HrRequestTypes[ucObj.requestCode].trim().replace(/\s+/g, '').toLowerCase()];
        } else if (ucObj.requestCode == 'HR Documents') {
          ucObj.title = this.arabicService.words[ucObj.requestCode.trim().replace(/\s+/g, '').toLowerCase()];
        }
      }
      if (this.lang == 'en') {
        if (ucObj.requestCode != 'HR Documents') {
          ucObj.title = HrRequestTypes[ucObj.requestCode];
        } else if (ucObj.requestCode == 'HR Documents') {
          ucObj.title = ucObj.requestCode;
        }
      }
    });
    if (this.lang == 'ar') {
      if (this.isHRDepartmentHeadUserID || this.isHRDepartmentTeamUserID) {
        this.columns = [
          { name: this.arabicfn('refid'), prop: 'ReferenceNumber' },
          { name: this.arabicfn('creator'), prop: 'Creator' },
          { name: this.arabicfn('requesttype'), prop: 'RequestType', width: 180 },
          { name: this.arabicfn('status'), prop: 'Status' },
          { name: this.arabicfn('assignedto'), prop: 'AssignedTo' },
          { name: this.arabicfn('requestdate'), cellTemplate: this.creationDateTemplate },
          { name: this.arabicfn('action'), cellTemplate: this.actionTemplate },
        ];
      } else {
        this.columns = [
          { name: this.arabicfn('refid'), prop: 'ReferenceNumber' },
          { name: this.arabicfn('creator'), prop: 'Creator' },
          { name: this.arabicfn('requesttype'), prop: 'RequestType', width: 180 },
          { name: this.arabicfn('status'), prop: 'Status' },
          { name: this.arabicfn('requestdate'), cellTemplate: this.creationDateTemplate },
          { name: this.arabicfn('action'), cellTemplate: this.actionTemplate },
        ];
      }
      th.requestTypeList = [];
      th.requestTypeList.push({ value: "", label: this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()] });
      Object.keys(HrRequestTypes).slice(reqTypes.length / 2).forEach((type) => {
        if (type != 'CV Bank' && type != 'Employee Profile') {
          if (type == 'Raise Complaints/ Suggestions') {
            th.requestTypeList.push({ value: HrRequestTypes[type], label: this.arabicService.words['raisecomplaintssuggestions'] });
          } else {
            th.requestTypeList.push({ value: HrRequestTypes[type], label: this.arabicService.words[type.trim().replace(/\s+/g, '').toLowerCase()] });
          }
        }
      });
      this.filterBy.Status = this.arabicfn('all');
    } else if (this.lang == 'en') {
      if (this.isHRDepartmentHeadUserID || this.isHRDepartmentTeamUserID) {
        this.columns = [
          { name: 'Ref ID', prop: 'ReferenceNumber' },
          { name: 'Creator', prop: 'Creator' },
          { name: 'Request Type', prop: 'RequestType', width: 180 },
          { name: 'Status', prop: 'Status' },
          { name: 'Assigned To', prop: 'AssignedTo' },
          { name: 'Request Date', cellTemplate: this.creationDateTemplate },
          { name: 'Action', cellTemplate: this.actionTemplate },
        ];
      } else {
        this.columns = [
          { name: 'Ref ID', prop: 'ReferenceNumber' },
          { name: 'Creator', prop: 'Creator' },
          { name: 'Request Type', prop: 'RequestType', width: 180 },
          { name: 'Status', prop: 'Status' },
          { name: 'Request Date', cellTemplate: this.creationDateTemplate },
          { name: 'Action', cellTemplate: this.actionTemplate },
        ];
      }
      th.requestTypeList = [];
      th.requestTypeList.push({ value: "", label: "All" });
      Object.keys(HrRequestTypes).slice(reqTypes.length / 2).forEach((type) => {
        if (type != 'CV Bank' && type != 'Employee Profile') {
          th.requestTypeList.push({ value: HrRequestTypes[type], label: type });
        }
      });
      this.filterBy.Status = 'All';
    }
  }

  createFormDialog() {
    this.bsModalRef = this.modalService.show(this.template);
  }

  closeDialog() {
    this.bsModalRef.hide();
  }

  changeList() { }

  requestCreateLink(reqInd) {
    if (reqInd >= 0 && this.hrCreateModalDetails[reqInd].createLink) {
      this.bsModalRef.hide();
      this.router.navigate([this.hrCreateModalDetails[reqInd].createLink]);
    }
  }

  loadListBasedOnRequestType(requestCode?: any) {
    this.progress = true;
    let toSendReq: any = {
      PageNumber: this.config.page,
      PageSize: this.config.itemsPerPage,
      UserID: this.currentUser.id,
      RequestType: this.filterBy.RequestType,
      UserName: this.currentUser.username,
      Creator: this.filterBy.Creator,
      SmartSearch: this.filterBy.SmartSearch
    };
    if (!this.filterBy.Status) {
      this.filterBy.Status = 'All';
      if (this.lang != 'en') {
        this.filterBy.Status = this.arabicfn('all');
      }
    }
    if (this.filterBy.Status && this.filterBy.Status != "All" && this.filterBy.Status != this.arabicfn('all')) {
      toSendReq.Status = this.filterBy.Status;
    }


    if (this.filterBy.ReqDateFrom) {
      toSendReq.ReqDateFrom = new Date(this.filterBy.ReqDateFrom).toJSON();
    }

    if (this.filterBy.ReqDateTo) {
      toSendReq.ReqDateTo = new Date(this.filterBy.ReqDateTo).toJSON();
    }

    if (requestCode) {
      toSendReq.RequestType = requestCode;
      this.filterBy.RequestType = requestCode;
    } else {
      if (this.isHRDepartmentHeadUserID || this.isHRDepartmentTeamUserID) {
        toSendReq.RequestType = HrRequestTypes['Leave Requests'];
        this.filterBy.RequestType = HrRequestTypes['Leave Requests'];
      } else {
        toSendReq.RequestType = '';
        this.filterBy.RequestType = '';
      }

    }
    this.hrService.getHRAllHRModulesList(toSendReq).subscribe((allHRModuleRes: any) => {
      if (allHRModuleRes) {
        this.rows = allHRModuleRes.Collection;
        this.config.totalItems = allHRModuleRes.Count;
        let AllStatusList = allHRModuleRes.M_LookupsList;
        if (this.common.currentLang == "en") {
          this.statusids.push({ DisplayName: "All", });
        } else if (this.common.currentLang == "ar") {
          this.statusids.push({ DisplayName: this.arabicService.words["all"] });
        }
        AllStatusList.forEach((item) => {
          this.statusids.push({ DisplayName: item.DisplayName });
        });
        this.statusList = this.statusids;
      }
      this.progress = false;

    });
  }
  loadMyProcessedRequests() {
    this.progress = true;
    let toSendReq: any = {
      PageNumber: this.config.page,
      PageSize: this.config.itemsPerPage,
      UserID: this.currentUser.id,
      RequestType: this.filterBy.RequestType,
      UserName: this.currentUser.username,
      Creator: this.filterBy.Creator,
      SmartSearch: this.filterBy.SmartSearch
    };
    if (!this.filterBy.Status) {
      this.filterBy.Status = 'All';
      if (this.lang != 'en') {
        this.filterBy.Status = this.arabicfn('all');
      }
    }
    if (this.filterBy.Status && this.filterBy.Status != "All" && this.filterBy.Status != this.arabicfn('all')) {
      toSendReq.Status = this.filterBy.Status;
    }

    if (!this.filterBy.RequestType || this.filterBy.RequestType == "All" || this.filterBy.RequestType == this.arabicfn('all')) {
      toSendReq.RequestType = '';
    }

    if (!this.filterBy.RequestType) {
      this.filterBy.RequestType = '';
    }

    if (this.filterBy.ReqDateFrom) {
      toSendReq.ReqDateFrom = new Date(this.filterBy.ReqDateFrom).toJSON();
    }

    if (this.filterBy.ReqDateTo) {
      toSendReq.ReqDateTo = new Date(this.filterBy.ReqDateTo).toJSON();
    }

    this.hrService.getHRMyProcessedRequestList(toSendReq).subscribe((allPendingRes: any) => {
      if (allPendingRes) {
        this.rows = allPendingRes.Collection;
        this.config.totalItems = allPendingRes.Count;
        var AllStatusList = allPendingRes.M_LookupsList;
        if (this.common.currentLang == "en") {
          this.statusids.push({ DisplayName: "All" });
        } else if (this.common.currentLang == "ar") {
          this.statusids.push({ DisplayName: this.arabicService.words["all"] });
        }
        AllStatusList.forEach((item) => {
          this.statusids.push({ DisplayName: item.DisplayName });
        });
        this.statusList = this.statusids;
      }
      this.progress = false;
    });
  }
  loadMyPendingRequests() {
    this.progress = true;
    let toSendReq: any = {
      PageNumber: this.config.page,
      PageSize: this.config.itemsPerPage,
      UserID: this.currentUser.id,
      RequestType: this.filterBy.RequestType,
      UserName: this.currentUser.username,
      Creator: this.filterBy.Creator,
      SmartSearch: this.filterBy.SmartSearch
    };
    if (!this.filterBy.Status) {
      this.filterBy.Status = 'All';
      if (this.lang != 'en') {
        this.filterBy.Status = this.arabicfn('all');
      }
    }

    if (this.filterBy.Status != "All" && this.filterBy.Status != this.arabicfn('all')) {
      toSendReq.Status = this.filterBy.Status;
    }

    if (!this.filterBy.RequestType || this.filterBy.RequestType == "All" || this.filterBy.RequestType == this.arabicfn('all')) {
      toSendReq.RequestType = '';
    }

    if (!this.filterBy.RequestType) {
      this.filterBy.RequestType = '';
    }

    if (this.filterBy.ReqDateFrom) {
      toSendReq.ReqDateFrom = new Date(this.filterBy.ReqDateFrom).toJSON();
    }

    if (this.filterBy.ReqDateTo) {
      toSendReq.ReqDateTo = new Date(this.filterBy.ReqDateTo).toJSON();
    }

    this.hrService.getHRMyPendingRequestList(toSendReq).subscribe((allPendingRes: any) => {
      if (allPendingRes) {
        this.rows = allPendingRes.Collection;
        this.config.totalItems = allPendingRes.Count;
        var AllStatusList = allPendingRes.M_LookupsList;
        if (this.common.currentLang == "en") {
          this.statusids.push({ DisplayName: "All" });
        } else if (this.common.currentLang == "ar") {
          this.statusids.push({ DisplayName: this.arabicService.words['all'] });
        }
        AllStatusList.forEach((item) => {
          this.statusids.push({ DisplayName: item.DisplayName });
        });
        this.statusList = this.statusids;
      }
      this.progress = false;
    });
  }

  loadMyOwnRequests() {
    let toSendReq: any = {
      PageNumber: this.config.page,
      PageSize: this.config.itemsPerPage,
      UserID: this.currentUser.id,
      RequestType: this.filterBy.RequestType,
      UserName: this.currentUser.username,
      Creator: this.filterBy.Creator,
      SmartSearch: this.filterBy.SmartSearch
    };
    if (!this.filterBy.Status) {
      this.filterBy.Status = 'All';
      if (this.lang != 'en') {
        this.filterBy.Status = this.arabicfn('all');
      }
    }

    if (this.filterBy.Status != "All" && this.filterBy.Status != this.arabicfn('all')) {
      toSendReq.Status = this.filterBy.Status;
    }

    if (!this.filterBy.RequestType || this.filterBy.RequestType == "All" || this.filterBy.RequestType == this.arabicfn('all')) {
      toSendReq.RequestType = '';
    }

    if (!this.filterBy.RequestType) {
      this.filterBy.RequestType = '';
    }

    if (this.filterBy.ReqDateFrom) {
      toSendReq.ReqDateFrom = new Date(this.filterBy.ReqDateFrom).toJSON();
    }

    if (this.filterBy.ReqDateTo) {
      toSendReq.ReqDateTo = new Date(this.filterBy.ReqDateTo).toJSON();
    }
    this.progress = true;
    this.hrService.getHRMyOwnRequestList(toSendReq).subscribe((allOwnRes: any) => {
      if (allOwnRes) {
        this.rows = allOwnRes.Collection;
        this.config.totalItems = allOwnRes.Count;
        var AllStatusList = allOwnRes.M_LookupsList;
        if (this.common.currentLang == "en") {
          this.statusids.push({ DisplayName: "All" });
        } else if (this.common.currentLang == "ar") {
          this.statusids.push({ DisplayName: this.arabicService.words['all'] });
        }
        AllStatusList.forEach((item) => {
          this.statusids.push({ DisplayName: item.DisplayName });
        });
        this.statusList = this.statusids;
      }
      this.progress = false;
    });
  }

  loadRequestCounts() {
    this.hrService.getHRDashboardCount(this.currentUser.id).subscribe((modCountRes: any) => {
      if (modCountRes) {
        this.hrProgresscardDetails.forEach((cardObj) => {
          if (modCountRes[cardObj.countType] && modCountRes[cardObj.countType] > 0) {
            cardObj.count = modCountRes[cardObj.countType];
          } else {
            cardObj.count = 0;
          }
          if (modCountRes.MyPendingActions && modCountRes.MyPendingActions > 0) {
            this.commonRequests.MyPendingActions.count = modCountRes.MyPendingActions;
          } else {
            this.commonRequests.MyPendingActions.count = 0;
          }
          if (modCountRes.MyRequests && modCountRes.MyRequests > 0) {
            this.commonRequests.MyOwnRequests.count = modCountRes.MyRequests;
          } else {
            this.commonRequests.MyOwnRequests.count = 0;
          }
          if (modCountRes.MyProcessedRequests && modCountRes.MyProcessedRequests > 0) {
            this.commonRequests.MyProcessedRequests.count = modCountRes.MyProcessedRequests;
          } else {
            this.commonRequests.MyProcessedRequests.count = 0;
          }
        });
      }
    });
  }

  onChangePage(config, event) {
    if (!this.commonRequests.MyPendingActions.isReqSelected && !this.commonRequests.MyOwnRequests.isReqSelected && !this.commonRequests.MyProcessedRequests.isReqSelected) {
      this.loadListBasedOnRequestType(this.filterBy.RequestType);
    }

    if (this.commonRequests.MyPendingActions.isReqSelected && !this.commonRequests.MyOwnRequests.isReqSelected) {
      this.loadMyPendingRequests();
    }

    if (!this.commonRequests.MyPendingActions.isReqSelected && this.commonRequests.MyOwnRequests.isReqSelected) {
      this.loadMyOwnRequests();
    }
    if (!this.commonRequests.MyPendingActions.isReqSelected && !this.commonRequests.MyOwnRequests.isReqSelected && this.commonRequests.MyProcessedRequests.isReqSelected) {

      this.loadMyProcessedRequests();
    }
  }

  onSearch() {
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    if (!this.commonRequests.MyPendingActions.isReqSelected && !this.commonRequests.MyOwnRequests.isReqSelected && !this.commonRequests.MyProcessedRequests.isReqSelected) {
      this.loadListBasedOnRequestType(this.filterBy.RequestType);
    }

    if (this.commonRequests.MyPendingActions.isReqSelected && !this.commonRequests.MyOwnRequests.isReqSelected && !this.commonRequests.MyProcessedRequests.isReqSelected) {
      this.loadMyPendingRequests();
    }

    if (!this.commonRequests.MyPendingActions.isReqSelected && this.commonRequests.MyOwnRequests.isReqSelected && !this.commonRequests.MyProcessedRequests.isReqSelected) {
      this.loadMyOwnRequests();
    }
    if (!this.commonRequests.MyPendingActions.isReqSelected && !this.commonRequests.MyOwnRequests.isReqSelected && this.commonRequests.MyProcessedRequests.isReqSelected) {
      this.loadMyProcessedRequests();
    }
  }

  reqSelect(pendingReq, OwnReq, myproc, reqType?: string) {
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.filterBy = {
      Status: 'All',
      RequestType: '',
      Creator: null,
      ReqDateFrom: null,
      ReqDateTo: null,
      SmartSearch: null
    };
    if (this.lang != 'en') {
      this.filterBy.Status = this.arabicfn('all');
    }
    if (myproc) {
      this.commonRequests.MyPendingActions.isReqSelected = false;
      this.commonRequests.MyOwnRequests.isReqSelected = false;
      this.commonRequests.MyProcessedRequests.isReqSelected = true;
      this.loadMyProcessedRequests();

    }
    if (!pendingReq && !OwnReq && !myproc) {
      this.commonRequests.MyPendingActions.isReqSelected = false;
      this.commonRequests.MyOwnRequests.isReqSelected = false;
      this.commonRequests.MyProcessedRequests.isReqSelected = false;
      this.loadListBasedOnRequestType(reqType);
    }

    if (pendingReq && !OwnReq) {
      this.commonRequests.MyPendingActions.isReqSelected = true;
      this.commonRequests.MyOwnRequests.isReqSelected = false;
      this.commonRequests.MyProcessedRequests.isReqSelected = false;
      this.loadMyPendingRequests();
    }

    if (!pendingReq && OwnReq) {
      this.commonRequests.MyPendingActions.isReqSelected = false;
      this.commonRequests.MyOwnRequests.isReqSelected = true;
      this.commonRequests.MyProcessedRequests.isReqSelected = false;
      this.filterBy.Creator = this.currentUser.username;
      this.loadMyOwnRequests();
    }
    this.hrService.triggerScrollTo();
  }

  viewData(value: any) {
    let viewRouterUrl = '';
    this.hrProgresscardDetails.forEach((cardObj) => {
      if (this.common.currentLang != 'ar') {
        if (HrRequestTypes[cardObj.requestCode].toLowerCase() == value.RequestType.toLowerCase()) {
          viewRouterUrl = cardObj.viewLink;
        }
      } else if (this.common.currentLang == 'ar') {
        let toTranslte = this.removeWordSpaces(HrRequestTypes[cardObj.requestCode].toLowerCase());
        let arabicWord = this.arabicfn(toTranslte);
        if (arabicWord == value.RequestType.trim()) {
          viewRouterUrl = cardObj.viewLink;
        }
      }

      if (cardObj.requestCode == 10) {
        this.router.navigate(["/" + this.common.currentLang + "/app/hr/raise-complaint-suggestion-view/" + value.ID]);
      }
    });
    if (viewRouterUrl != '') {
      this.router.navigate([viewRouterUrl + value.ID]);
    }
    if (value.IsCompensationRequest) {
      this.router.navigate(['/' + this.common.currentLang + '/app/hr/official-tasks/request-view/compensation/' + value.ID]);
    }
  }

  goToCompensationForm(task: any) {
    this.router.navigate(['/' + this.common.currentLang + '/app/hr/official-tasks/request-view/create-compensation/' + task.ID]);
  }

  openReport() {
    let initialState = {
      myPendingSelected: this.commonRequests.MyPendingActions.isReqSelected,
      myRequestsSelected: this.commonRequests.MyOwnRequests.isReqSelected,
      myProcessedSelected: this.commonRequests.MyProcessedRequests.isReqSelected
    };
    this.bsModalRef = this.modalService.show(HrReportModalComponent, Object.assign({ class: 'modal-lg' }, {}, { initialState }));
  }

  removeWordSpaces(words: string) {
    return words.replace(/\s+/g, '');
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }

  dateChange(eve, isStartEnd) {
    if (eve) {
      if (isStartEnd == 'start') {
        this.filterBy.ReqDateFrom = eve;
      }
      if (isStartEnd == 'end') {
        this.filterBy.ReqDateTo = eve;
      }
    }
  }

  checkStartEndDiff() {
    let toRetVal = true;
    if (this.utilsService.isValidDate(this.filterBy.ReqDateFrom)
      && this.utilsService.isValidDate(this.filterBy.DateTo)) {
      if (this.filterBy.ReqDateFrom.getTime() <= this.filterBy.DateTo.getTime()) {
        toRetVal = true;
      } else {
        toRetVal = false;
      }
    } else {
      if (!this.utilsService.isDate(this.filterBy.ReqDateFrom) || !this.utilsService.isDate(this.filterBy.DateTo)) {
        toRetVal = false;
      }
    }
    this.validateStartEndDate.isValid = toRetVal;
    if (this.validateStartEndDate.isValid) {
      this.validateStartEndDate.msg = '';
    } else {
      this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabicfn('errormsgvalidenddate');
    }
  }

  dateValidation() {
    this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabicfn('errormsgvalidenddate');
    let showDateValidationMsg = false;
    if (this.filterBy.ReqDateFrom && this.filterBy.ReqDateTo) {
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
