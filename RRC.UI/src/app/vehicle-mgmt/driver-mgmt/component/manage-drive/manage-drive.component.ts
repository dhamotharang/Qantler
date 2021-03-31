import { Component, OnInit, TemplateRef, ViewChild, Inject, Renderer2, Input } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/admin/service/admin/admin.service';
import { BsModalService, BsModalRef, BsDatepickerConfig } from 'ngx-bootstrap';
import { DriverManagementModalComponent } from 'src/app/modal/driver-management-modal/driver-management-modal.component';
import { DOCUMENT } from '@angular/common';
import { VehicleMgmtServiceService } from '../../../service/vehicle-mgmt-service.service';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';

@Component({
  selector: 'app-manage-drive',
  templateUrl: './manage-drive.component.html',
  styleUrls: ['./manage-drive.component.scss']
})
export class ManageDriveComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('addExtraHourstemplate') addExtraHourstemplate: TemplateRef<any>;
  @ViewChild('addCompensiateHourstemplate') addCompensiateHourstemplate: TemplateRef<any>;
  @Input() userProfileID;
  isModified: boolean = false;
  employeeList:Array<{UserProfileID:number,EmployeeName:string}> = [];
  EmployeeArray: Array<any> = [];
  removersArray: Array<any> = [];
  removedUsers: Array<any> = [];
  lang: any;
  userListArr:any = [];
  driverListArr:any = [];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  allUserList: any=[];
  allDriverList: any=[];
  rows:Array<any> = [];
  columns:Array<any> = [];
  page:number = 1;
  pageSize: any                                                                                                                = 10;
  itemsPerPage:number = 10;
  bsModalRef: BsModalRef;
  extraHourModalRef:BsModalRef;
  compensateModalRef:BsModalRef;
  maxSize:number = 10;
  numPages:number = 1;
  length:number = 0;
  public config:any = {
    paging: true,
    sorting: {columns: this.columns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    page: 1,
    maxSize: 10,
    itemsPerPage:10,
    totalItems:0
  };
  departmentUserList:any = [];
  driverList:Array<{UserProfileID:number,EmployeeName:string}> = [];
  isUserListSet:boolean = true;
  toBeApprovers:any = [];
  approversArray: Array<any> = [];
  driverArr:any = [];
  UserProfileID: any;

  
  addExtraHoursModel:any = {
    logDate:'',
    extraHour: ''
  };

  addCompensateHoursModel:any = {
    logDate:'',
    compensateHour: ''
  };

  filterBy:any = {
    UserID: this.currentUser.UserID,
    DateRangeFrom: null,
    DateRangeTo: null
  };

  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  progress = false;
  tableMessages: { emptyMessage: any; };
  isVehicleDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 13;
  isVehicleDepartmentTeamUserID = this.currentUser.OrgUnitID == 13 && !this.currentUser.IsOrgHead;
  constructor(
    public common: CommonService,
    private modalService: BsModalService,
    public router:Router,
    private adminService: AdminService,
    private renderer: Renderer2,
    private VehicleMgmtService:VehicleMgmtServiceService,
    @Inject(DOCUMENT) private document: Document) {

  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.lang = this.common.currentLang;
    this.common.topBanner(false, false, '', '');
    this.common.breadscrumChange('Vehicle Management','Manage Driver','');
    if(this.lang == 'ar'){
      this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('managedriver'),'');
    }



    // if(this.isVehicleDepartmentHeadUserID || this.isVehicleDepartmentTeamUserID) {
    //   this.showMgmtCard = true;
    // } else {
    //   this.showMgmtCard = false;
    // }
    if(this.common.currentLang != 'en'){
      this.columns = [
        { name: this.arabic('drivername'), prop: 'DriverName' },
        { name: this.arabic('mobilenumber'), prop: 'MobileNumber' },
        { name: this.arabic('balanceextrahours'), prop: 'TotalHour' },
        { name: this.arabic('action'), cellTemplate: this.actionTemplate }
      ];
    }else{
      this.columns = [
        { name: 'Driver Name', prop: 'DriverName' },
        { name: 'Mobile Number', prop: 'MobileNumber' },
        { name: 'Balance Extra Hours', prop: 'TotalHour' },
        { name: 'Action', prop: '', cellTemplate: this.actionTemplate }
      ];
    }
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabic('nodatatodisplay')
    };

    this.getAllUsers();
    // this.getAllDrivers();
    this.getAlldriversList();
  }

  getAllUsers() {
    // this.adminService.getAllUsers(this.currentUser.id, 2, '').subscribe((userList:any)=>{
      this.VehicleMgmtService.getNonDriverUsers().subscribe((driverList:any)=>{
        this.departmentUserList = driverList.Collection;
        this.departmentUserList = this.departmentUserList.map(function(obj) { 
          obj['UserProfileID'] = obj['DriverID'];
          obj['EmployeeName'] = obj['DriverName'];
          delete obj['DriverID'];
          delete obj['DriverName']; 
          return obj; 
        });
      });
      // this.departmentUserList = userList;
      this.getAllDrivers();
    // });
  }
  getAllDrivers() {
    this.VehicleMgmtService.getdriver().subscribe((driverList:any)=>{
      this.driverList = driverList.Collection;
      this.driverList = this.driverList.map(function(obj) { 
        obj['UserProfileID'] = obj['DriverID'];
        obj['EmployeeName'] = obj['DriverName'];
        delete obj['DriverID'];
        delete obj['DriverName']; 
        return obj; 
      });
    });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  viewData(row){
    let initialState: any = {
      id: row.DriverID,
      driverName:row.DriverName
    };
    this.modalService.show(DriverManagementModalComponent, Object.assign({class:'modal-lg'}, {}, {initialState}));
    // this.router.navigate(['/'+this.common.currentLang+'/app/vehicle-management/driver-management/driver-view/' + row.DriverID]);
  }



  openAddExtraHours(row) {
    this.UserProfileID =  row.UserProfileID;
    this.extraHourModalRef = this.modalService.show(this.addExtraHourstemplate, {class: 'modal-lg'});
    this.addExtraHoursModel.logDate = this.addExtraHoursModel.extraHour = "";
    this.validateExtraHour();
  }

  validateExtraHour() {
    let flag = true;
    if(this.addExtraHoursModel.logDate && this.addExtraHoursModel.extraHour) {
      flag = false;
    }
    return flag;
  }

  addExtraHours() {
    let extraHourData: any = {
      UserProfileID: this.UserProfileID,
      LogDate: new Date(this.addExtraHoursModel.logDate),
      ExtraHours: this.addExtraHoursModel.extraHour,
      CompensateHours: 0,
      CreatedBy: this.currentUser.id,
      CreatedDateTime: new Date(),
    };
    this.VehicleMgmtService.addExtraCompensateHours(extraHourData).subscribe((allExtraHourResponse:any) => {
      this.extraHourModalRef.hide();
      if(allExtraHourResponse){
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = 'Extra Hours Added Successfully';
        this.getAlldriversList();
        if(this.lang != 'en'){
          this.bsModalRef.content.message = this.arabic('extrahourreqmsg');
        }
      }
    });
  }

  openAddCompensiateHours(row) {
    this.UserProfileID =  row.UserProfileID;
    this.compensateModalRef =this.modalService.show(this.addCompensiateHourstemplate, {class: 'modal-lg'});
    this.addCompensateHoursModel.logDate = this.addCompensateHoursModel.compensateHour = "";
    this.validateCompensateHour();
  }

  validateCompensateHour() {
    let flag = true;
    if(this.addCompensateHoursModel.logDate && this.addCompensateHoursModel.compensateHour) {
      flag = false;
    }
    return flag;
  }

  addCompensateHours() {
    let compensateHourData: any = {
      UserProfileID: this.UserProfileID,
      LogDate: new Date(this.addCompensateHoursModel.logDate),
      CompensateHours: this.addCompensateHoursModel.compensateHour,
      ExtraHours: 0,
      CreatedBy: this.currentUser.id,
      CreatedDateTime: new Date(),
    };
    this.VehicleMgmtService.addExtraCompensateHours(compensateHourData).subscribe((allCompensateHourResponse:any) => {
      this.compensateModalRef.hide();
      if(allCompensateHourResponse){
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = 'Compensate Hours Added Successfully';
        this.getAlldriversList();
        if(this.lang != 'en'){
          this.bsModalRef.content.message = this.arabic('compensatehourreqmsg');
        }
      }
    });
  }

  public onChangePage(config: any, page: any = { page: this.config.page, itemsPerPage: this.itemsPerPage }): any {
    this.page = page;
    this.getAlldriversList();
  }

  closemodal() {  
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }

  getAlldriversList() {
    // if(!this.filterBy.DateRangeFrom) {
    //   this.filterBy.DateRangeFrom = "";
    // }
    // if(!this.filterBy.DateRangeTo) {
    //   this.filterBy.DateRangeTo = "";
    // }
    this.VehicleMgmtService.getAllDrivers(this.page, this.pageSize,this.filterBy.UserID).subscribe((allDrivers:any) => {
      if(allDrivers){
        this.rows = allDrivers.Collection;
        this.config.totalItems = allDrivers.Count;
      }
      this.progress = false;
    });
  }


  moveToDriverList(){
    this.isModified = true;
    this.isSameUsers(this.toBeApprovers, 'approvers');
    this.toBeApprovers.forEach((tba)=>{
      this.driverList = [...this.driverList,tba];
    });
    let toSpliceAul:any = this.departmentUserList;
    this.toBeApprovers.forEach((tba) => {
      let tbaInd = toSpliceAul.findIndex((aul) => aul.UserProfileID == tba.UserProfileID);
      if(tbaInd > -1) {
        toSpliceAul.splice(tbaInd,1);
      }
    });
    this.toBeApprovers = [];
    this.departmentUserList = [...toSpliceAul];
  }

  isSameUsers(users, type: any) {
    if (type == 'approvers') {
      this.approversArray = users;
    } else {
      this.removersArray = users;
    }
    if (this.approversArray.length > 0 && this.removersArray.length > 0) {
      let approverDiffers = this.approversArray.filter(item => this.removersArray.indexOf(item) < 0);
      let removersDiffers = this.removersArray.filter(item => this.approversArray.indexOf(item) < 0);
      if (approverDiffers.length > 0 || removersDiffers.length > 0) {
        this.isModified = true;
      } else {
        this.isModified = false;
      }
    } else {
      this.isModified = true;
    }
    return this.isModified;
  }

  moveToAllUserList(){
    this.isModified = true;
    let removersIds = [];
    this.removedUsers = this.driverArr;
    
    this.driverArr.forEach((user:any) => {
      removersIds.push(user.UserProfileID);
    });
    if (removersIds && removersIds.length) {
        this.driverArr.forEach((apu)=>{
          this.departmentUserList = [...this.departmentUserList,apu];
        });
        let toSpliceApl = this.driverList;
        this.driverArr.forEach((apv) => {
          let apuInd = toSpliceApl.findIndex((apl) => apl.UserProfileID == apv.UserProfileID);
          if(apuInd > -1){
            toSpliceApl.splice(apuInd,1);
          }
        });
        this.driverArr = [];
        this.driverList = [...toSpliceApl];
        this.isSameUsers(this.removedUsers, 'removers');
    }
  }

  saveDriverList(){
    let toSendApproversList  = [];
    if(this.driverList.length > 0){
      this.driverList.forEach((apl)=> {
        toSendApproversList.push(apl.UserProfileID);
      });
    }
    this.VehicleMgmtService.saveDriverList(toSendApproversList,this.currentUser.id).subscribe((driverData)=> {
      if(driverData){
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = 'Driver List Updated Successfully';
        if(this.lang == 'ar') {
          this.bsModalRef.content.message = this.arabicfn('approverrequpdatemsg');
        }
        this.getAlldriversList();
        this.getAllDrivers();
        this.getAllUsers();
      }
    });
  }
}
