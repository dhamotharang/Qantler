import { Component, OnInit, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import Chart from 'chart.js';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/common.service';
import { EmployeeService } from '../../service/employee.service';
// import { data } from 'src/app/task/task-dashboard/data';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  public columns: Array<any> = [];
  
  userList: any = [];
  priorityList: string[];
  public page: number = 1;
  public itemsPerPage: number = 10;
  public maxSize: number = 10;
  public numPages: number = 1;
  public length: number = 0;
  config: any = {
    paging: true,
    page: 1,
    maxSize: 10,
    itemsPerPage:10,
    totalItems:0
  };
  rows: any = [];
  cardDetails: any = [];
  dashBoard: any;
  department: any = [];
  filter_data: any = [];
  filter = false;
  userName: any;
  smartSearch: any;
  organizations = "";
  jobTitle: any;
  type = 0;
  lang: any;
  isEngLang: boolean = true;
  // isHRDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 9;
  isHRDepartmentHeadUserID = this.currentUser.OrgUnitID == 9;
  isHRDepartmentTeamUserID = this.currentUser.OrgUnitID == 9 && !this.currentUser.IsOrgHead;
  tableMessages: { emptyMessage: any; };

  constructor(private common: CommonService, public router:Router, private service: EmployeeService,) {
    this.getEmpDashboard();
    this.getUserProfileList(this.type);
  }

  ngOnInit() {
    this.common.homeScrollTop();
    this.common.topBanner(false, '', '', '');
    if(!this.isHRDepartmentHeadUserID){
      this.router.navigate(['/error']);
    }
    this.lang = this.common.currentLang;
    if(this.lang == 'en'){
      this.isEngLang = true;
      this.common.breadscrumChange('HR', `Employee's Profile List`, '');
    }else {
      this.isEngLang = false;
      this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('employeesprofilelist'), '');
    }
    this.columns = [
      { name: this.isEngLang ? 'Ref ID':'الرقم المرجعي', prop: 'referenceNumber' },
      { name: this.isEngLang ? 'Office/Department':'المكتب / الإدارة', prop: 'DepartmentName' },
      { name: this.isEngLang ? 'Name':'اسم', prop: 'EmployeeName' },
      { name: this.isEngLang ? 'Job Title': this.arabicfn('designation'), prop: 'JobTitle' },
      { name: this.isEngLang ? 'Action':'عرض', prop: '', cellTemplate: this.actionTemplate },
    ];
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabicfn('nodatatodisplay')
    };

    switch(this.common.currentLang)
      {
        case "en": {this.organizations = "All";break;}
        case "ar": {this.organizations = this.arabicfn('all');break;}
      }
  }

  viewData(type, value){
    if(type=='edit'){
      this.router.navigate(['app/hr/employee/edit/' + value.UserProfileId + '/list']);
    }else{
      this.router.navigate(['app/hr/employee/view/' + value.UserProfileId]);
    }
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }

  async getUserProfileList(type, search='') {
    this.filter_data = this;
    this.filter = true;
    let department = '';
    let username = '';
    let smartsearch = '';
    let jobTitle = '';
    
    if (this.organizations && search && this.organizations != "All" &&  this.organizations != this.arabicfn('all')) {
      department = this.organizations;
    }

    if (this.userName && search) {
      username = this.userName;
    }
    if (this.smartSearch && search) {
      smartsearch = this.smartSearch;
    }
    if (type) {
      this.type = type;
    }
    if (this.jobTitle && search) {
      jobTitle = this.jobTitle;
    }
    await this.service.getUserProfileList(this.page, this.maxSize, department, username, smartsearch, jobTitle, this.type).subscribe(data => {
      this.userList = data;
      this.rows = this.userList.Collection;
      this.department = this.userList.M_OrganizationList;
      switch(this.common.currentLang)
      {
        case "en": {this.department.splice(0, 0, "All");break;}
        case "ar": {this.department.splice(0,0,this.arabicfn('all'));break;}
      }
      this.config.totalItems = this.userList.Count;
    });
    this.service.triggerScrollTo();
  }

  public onChangePage(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
    this.page = page;
    this.getUserProfileList(this.type);

  }

  async getEmpDashboard() {
    await this.service.getEmpDashboard(this.currentUser.id).subscribe(data => {
      const mapped = Object.keys(data).map(key => ({name: key, count: data[key]}));
      this.dashBoard = mapped;
      for(var i = 0; i < this.dashBoard.length; i++) {
        if(this.dashBoard[i].name == "EmployeesRegistered") {
          this.cardDetails.push({
              'image': 'assets/employee-profile/employee.png', 
              'count': this.dashBoard[i].count,
              'name': this.isEngLang ? 'Employee Registered' : 'الموظفين المسجلين',
              'type': 0,
              'progress': 50 })
        }
        if(this.dashBoard[i].name == "ExpiredPassportNumber") {
          this.cardDetails.push({
              'image': 'assets/employee-profile/passport.png', 
              'count': this.dashBoard[i].count,
              'name': this.isEngLang ? 'Expired Passport Number' : 'جوازات السفر منتهية الصلاحية',
              'type': 1,
              'progress': 50 })
        }
        if(this.dashBoard[i].name == "ExpiredInsuranceNumber") {
          this.cardDetails.push({
              'image': 'assets/employee-profile/insurance.png', 
              'count': this.dashBoard[i].count,
              'name': this.isEngLang ? 'Expired Insurance Number' : 'التأمينات الصحية منتهية الصلاحية',
              'type': 2,
              'progress': 50 })
        }
        if(this.dashBoard[i].name == "ExpiredEmiratesID") {
          this.cardDetails.push({
              'image': 'assets/employee-profile/emirates_id.png', 
              'count': this.dashBoard[i].count,
              'name': this.isEngLang ? 'Expired Emirates ID' : 'الهويات الإماراتية منتهية الصلاحية',
              'type': 3,
              'progress': 50 })
        }
        if(this.dashBoard[i].name == "ExpiredLabourContract") {
          this.cardDetails.push({
              'image': 'assets/employee-profile/contract.png', 
              'count': this.dashBoard[i].count,
              'name': this.isEngLang ? 'Expired Labour Contract' : 'عقود العمل منتهية الصلاحية',
              'type': 4,
              'progress': 50 })
        }
        if(this.dashBoard[i].name == "ExpiredVisa") {
          this.cardDetails.push({
              'image': 'assets/employee-profile/expired_visa.png', 
              'count': this.dashBoard[i].count,
              'name': this.isEngLang ? 'Expired Visa' : 'التأشيرات منتهية الصلاحية',
              'type': 5,
              'progress': 50 })
        }
      }
    });
  }
  
  changeList() {
    this.getUserProfileList(this.type);
  }
  
  createProfile() {
    this.router.navigate(['app/hr/employee/create']);
  }
}
