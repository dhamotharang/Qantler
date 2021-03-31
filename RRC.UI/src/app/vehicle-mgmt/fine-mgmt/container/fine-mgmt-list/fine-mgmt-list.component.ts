import { Component, OnInit, TemplateRef, ViewChild, Renderer2, Inject } from '@angular/core';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { DatePipe, DOCUMENT } from '@angular/common';
import { CommonService } from 'src/app/common.service';
import { Router, ActivatedRoute } from '@angular/router';
import { VehicleMgmtServiceService } from 'src/app/vehicle-mgmt/service/vehicle-mgmt-service.service';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: 'app-fine-mgmt-list',
  templateUrl: './fine-mgmt-list.component.html',
  styleUrls: ['./fine-mgmt-list.component.scss']
})
export class FineMgmtListComponent implements OnInit {
  lang: any;
  page: any;
  maxSize: any;
  tableMessages: { emptyMessage: any; };
  public columns: Array<any> = [];
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };
  rows: any;
  filter: any;
  formlabels: {
    title: any
    to: any
    department: any
    name: any
    cc: any
    send: any
  };
  departmentList: string[];
  userList: any = [];
  user: any;

  department: any;
  to: any;

  reminderModel: {
    EmailTo: String
    EmailCCDepartmentID: Number
    EmailCCUserID: Number
    VehicleFineID: Number
  }
  emailDepartment: Number;
  emailUser: Number;
  vehicleFineID: Number;
  progress: boolean;
  reportTitle: any;
  StatusList: any = [];
  Currentuser: any;
  all: any;
  dateMsg: any;
  mailTo: any;
  filterDepartmentList: any = [];
  filterUserList: any = [];

  constructor(
    private common: CommonService,
    public router: Router,
    public datepipe: DatePipe,
    public modalService: BsModalService,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document,
    private activeRoute: ActivatedRoute,
    public Api: VehicleMgmtServiceService,
    public bsModalRef: BsModalRef,
    public util: UtilsService,
    public datePipe: DatePipe,

  ) {
    this.Currentuser = JSON.parse(localStorage.getItem('User'));
    this.all = (this.common.language == 'English') ? "All" : this.arabic('all');
    this.page = 1;
    this.maxSize = 10;
    this.reminderModel = {
      EmailTo: '',//this.user.EmailId
      EmailCCDepartmentID: 0,
      EmailCCUserID: 0,
      VehicleFineID: 0
    }
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
    this.filterInit();
    this.loadList();
    this.getStatus();
    this.getFilterUser();
    this.formlabels = {
      title: (this.common.language == 'English') ? "SEND REMINDER" : this.arabic('sendremainder'),
      to: (this.common.language == 'English') ? "To" : this.arabic('to'),
      department: (this.common.language == 'English') ? "Office/Department" : this.arabic('remainderdepartment'),
      name: (this.common.language == 'English') ? "Name" : this.arabic('remaindername'),
      cc: (this.common.language == 'English') ? "CC" : this.arabic('cc'),
      send: (this.common.language == 'English') ? "SEND" : this.arabic('send')
    };

    this.reportTitle = (this.common.language == 'English') ? "Report" : this.arabic('report');

    this.common.breadscrumChange('Vehicle Management', 'Fine Management', '');
    this.common.topBanner(true, 'Fine Management Dashboard', '+ LOG A FINE', '/en/app/vehicle-management/fine-management/log-fine');

    this.columns = [
      { name: 'Issued Against Office/Department', prop: 'IssuedAgainstDepartment' },
      { name: 'Issued Against Name', prop: 'IssuedAgainstName' },
      { name: 'Plate Number', prop: 'PlateNumber' },
      { name: 'Time', prop: 'Time' },
      { name: 'Status', prop: 'Status' },
      { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
    ];

    if (this.common.currentLang == 'ar') {
      this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('finemgmt'), '');
      this.common.topBanner(true, this.arabic('dashboard'), '+ ' + this.arabic('logafine'), '/ar/app/vehicle-management/fine-management/log-fine');
      this.columns = [
        { name: this.arabic('issuedagainstofficedepartment'), prop: 'IssuedAgainstDepartment' },
        { name: this.arabic('issuedagainstname'), prop: 'IssuedAgainstName' },
        { name: this.arabic('platenumber'), prop: 'PlateNumber' },
        { name: this.arabic('time'), prop: 'Time' },
        { name: this.arabic('status'), prop: 'Status' },
        { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
      ];
    }

    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabic('nodatatodisplay')
    };



  }

  filterInit() {
    this.filter = {
      status: (this.lang == 'en') ? 'All' : this.arabic('all'),
      department: (this.lang == 'en') ? 'All' : this.arabic('all'),
      name: (this.lang == 'en') ? 'All' : this.arabic('all'),
      from: '',
      to: '',
      plateNumber: '',
      search: ''
    };
  }

  getStatus() {
    var list = [];
    list.push({ 'StatusID': 0, 'StatusName': (this.lang == 'en') ? 'All' : this.arabic('all') });
    this.Api.getStatus('VehicleRequestStatus', this.Currentuser.id).subscribe((res: any) => {
      res.map(r => list.push(r));
      this.StatusList = list;
    });
  }

  getUserList() {
    this.emailUser = null;
    this.userList = [];
    var params = [{
      "OrganizationID": this.emailDepartment, "OrganizationUnits": "string"
    }];
    this.common.getUserList(params, this.Currentuser.id).subscribe((res: any) => {
      if (res && res.length > 0)
        this.userList = res;
    });
  }

  getFilterUser() {
    var list = [];
    list.push({ 'UserID': 0, 'EmployeeName': (this.lang == 'en') ? 'All' : this.arabic('all') })

    this.filterUserList.push({ UserID: 0, EmployeeName: this.all });
    this.userList = [];
    var params = [];
    this.common.getUserList(params, this.Currentuser.id).subscribe((res: any) => {
      res.map(re => list.push(re));
      this.filterUserList = list;
    });
  }

  onChangePage(page) {
    this.page = page;
    this.loadList();
  }

  maxDate(date) {
    return this.util.maxDateCheck(date);
  }

  dateValidation() {
    if (this.filter.from && this.filter.to) {
      var msg = this.util.dateValidation(this.filter.from, this.filter.to);
      if (msg && msg.flag) {
        this.dateMsg = msg.msg;
        return msg.flag;
      } else {
        return false;
      }
    }
  }

  loadList() {
    var list = [];
    list.push({ 'OrganizationID': 0, OrganizationUnits: (this.lang == 'en') ? 'All' : this.arabic('all') })
    this.progress = true;
    let status = '', department = '', name = '', from = '', to = '', plateNumber = '', search = '';
    if (this.filter.status != null && this.filter.status != 'All' && this.filter.status != this.arabic('all'))
      status = this.filter.status;
    if (this.filter.department != null && this.filter.department != this.arabic('all') && this.filter.department != 'All')
      department = this.filter.department;
    if (this.filter.name != null && this.filter.name != this.arabic('all') && this.filter.name != 'All')
      name = this.filter.name;
    if (this.filter.from)
      from = new Date(this.filter.from).toJSON();
    if (this.filter.to)
      to = new Date(this.filter.to).toJSON();
    if (this.filter.plateNumber)
      plateNumber = this.filter.plateNumber;
    if (this.filter.search)
      search = this.filter.search;
    this.Api.getFineList('Fine', this.page, this.maxSize, status, department, name, from, to, plateNumber, search).subscribe((res: any) => {
      this.config.totalItems = res.Count;
      this.rows = res.Collection;
      this.departmentList = res.Organizations;
      this.departmentList.map((re: any) => {
        list.push(re);
      });
      this.filterDepartmentList = list;
      if (this.lang == 'en') {
        this.rows.map(res => res.Time = this.datePipe.transform(res.Time, "dd/MM/yyyy,h:mma"));
      }
      if (this.lang == 'ar') {
        this.rows.map(res => res.Time = this.datePipe.transform(res.Time, "h:mm,dd/MM/yyyy").concat(this.datePipe.transform(res.Time, 'a') == 'AM' ? this.common.arabic.words['am'] : this.common.arabic.words['pm']));
      }
      this.progress = false;
    })
  }

  viewData(data) {
    let lang = this.common.currentLang;
    this.router.navigate([lang + '/app/vehicle-management/fine-management/log-fine-view/' + data.VehicleFineID + '/' + data.PlateNumber]
      // {
      //   queryParams: {
      //     from: "List"
      //   }
      // }
    );
  }

  setReminder(row, tem) {
    this.bsModalRef = this.modalService.show(tem,
      {
        class: 'modal-lg',
        backdrop: true,
        ignoreBackdropClick: true
      });
    this.mailTo = row.DriverMailID;
    this.emailDepartment = null;
    this.emailUser = null;
    this.vehicleFineID = row.VehicleFineID;
    //this.bsModalRef.content.mail = row.DriverMailID;
  }

  validateReminder() {
    var flag = true;
    if (this.mailTo && this.emailDepartment && this.emailUser) {
      flag = false;
    }
    return flag;
  }


  saveReminder() {
    this.reminderModel.EmailTo = this.mailTo;
    this.reminderModel.EmailCCDepartmentID = this.emailDepartment;
    this.reminderModel.EmailCCUserID = this.emailUser;
    this.reminderModel.VehicleFineID = this.vehicleFineID;
    this.Api.saveReminder('SendaRemainder', this.Currentuser.id, this.reminderModel).subscribe(res => {
      if (res) {
        this.closemodal();
        this.bsModalRef = this.modalService.show(SuccessComponent, {
          backdrop: true,
          ignoreBackdropClick: true
        });
        this.bsModalRef.content.message = (this.common.language == 'English') ? "Reminder Sent Successfully" : this.arabic('remindersentsuccessfully');
      }
    });
  }

  closemodal() {
    this.filterInit();
    this.bsModalRef.hide();
  }

  openReport(temp) {
    this.filterInit();
    this.bsModalRef = this.modalService.show(temp, {
      class: 'modal-lg', backdrop: true,
      ignoreBackdropClick: true
    });
  }

  downloadReport() {
    var from = (this.filter.from) ? new Date(this.filter.from).toJSON() : '',
      to = (this.filter.to) ? new Date(this.filter.to).toJSON() : '';
    let status = '', department = '', name = '';
    if (this.filter.status != null && this.filter.status != 'All' && this.filter.status != this.arabic('all'))
      status = this.filter.status;
    if (this.filter.department != null && this.filter.department != this.arabic('all') && this.filter.department != 'All')
      department = this.filter.department;
    if (this.filter.name != null && this.filter.name != this.arabic('all') && this.filter.name != 'All')
      name = this.filter.name;
    var param = {
      'Status': status,
      'IssuedAgainstDepartment': department,
      'IssuedAgainstName': name,
      'FineDateFrom': from,
      'FineDateTo': to,
      'UserID': this.Currentuser.id,
      'SmartSearch': this.filter.search,
      'PlateNumber': this.filter.plateNumber,
    }
    this.Api.dowloadExcel('Fine', 'Fine Report-', param);
    this.closemodal();
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

}
