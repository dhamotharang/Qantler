import { Component, OnInit, ViewChild, TemplateRef, ChangeDetectorRef, ElementRef } from '@angular/core';
import { BsModalRef, BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { Router } from '@angular/router';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { AdminService } from '../../service/admin/admin.service';
import { DropDownTypes } from 'src/app/shared/enum/drop-down-types/drop-down-types.enum';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { HttpEventType } from '@angular/common/http';
import { AnnouncementService } from 'src/app/hr/announcement/service/announcement.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-admin-settings',
  templateUrl: './admin-settings.component.html',
  styleUrls: ['./admin-settings.component.scss']
})
export class AdminSettingsComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('userActionTemplate') userActionTemplate: TemplateRef<any>;
  @ViewChild('typeTemplate') typeTemplate: TemplateRef<any>;
  @ViewChild('typeValueFormTemplate') typeValueFormTemplate: TemplateRef<any>;
  @ViewChild('deleteTypeValue') deleteTypeValue: TemplateRef<any>;
  @ViewChild('editUserDataTemplate') editUserDataTemplate: TemplateRef<any>;
  @ViewChild('userDataFieldTemplate') userDataFieldTemplate: TemplateRef<any>;

  bsModalRef:BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  filterBy:any = {
    Type:null,
    Value:null
  };
  rows: Array<any> = [];
  columns: Array<any> = [];
  config: any = {
    paging: true,
    page: 1,
    maxSize: 10,
    itemsPerPage:10,
    totalItems:0
  };

  userManagementRows: Array<any> = [];
  userManagementColumns: Array<any> = [];
  userManagementTableconfig: any = {
    paging: true,
    page: 1,
    maxSize: 10,
    itemsPerPage:10,
    totalItems:0
  };
  @ViewChild('tinyDetail') tinyDetail: ElementRef;
  tinyConfig = {
    plugins: 'powerpaste casechange importcss tinydrive searchreplace directionality visualblocks visualchars fullscreen table charmap hr pagebreak nonbreaking toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons',
    language: this.common.language != 'English' ? "ar" : "en",
    menubar: 'file edit view insert format tools table tc help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media pageembed template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment',
    toolbar_drawer: 'sliding',
    directionality: this.common.language != 'English' ? "rtl" : "ltr"
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  isApiLoading:boolean = false;
  lang:string;
  notificationSection:boolean = true;
  approverSection:boolean = true;
  dropDownTypeSection:boolean = true;
  userManagementSection:boolean =true;
  holidaysSection:boolean = true;
  notification:any ={
    hours:''
  };

  allUserList:any = [];
  departmentUserList:any = [];

  approverList:Array<{UserProfileID:number,EmployeeName:string}> = [];
  toBeApprovers:any = [];
  approvedUsers:any = [];
  removedUsers:any = [];
  indexArrayForToBeApprovers:any = [];
  indexArrayForApprovedUsers:any = [];
  departmentList:any = [];
  sectionList:any = [];
  unitList:any = [];
  dropDownTypeList:any = [];
  departmentSelect:any = null;
  isUserListSet:boolean = false;
  dropDownType:any;
  userSearch:any;
  adminGetData:any = {
    hours:''
  };

  dropDownSearchStrings:any = {
    deptSearchStr:'',
    userListSearchStr:'',
    approverListSearchStr:'',
    userManagementSearchStr:''
  };

  img_file :any[] = [];
  uploadProcess:boolean = false;
  uploadPercentage:number = 0;
  attachments: any = [];
  latestAttachment: any = [];

  @ViewChild('fileInput') fileInput :ElementRef;
  typeFormDataModel:any = {
    Type:'',
    Value:'',
    arValue:''
  };

  isExcelImport:boolean = true;
  typeFormTitle:string = '';
  toDeleteValue:any;
  userManagementModel:any = {
    UserProfileID:'',
    EmployeeName:'',
    DepartmentID:'',
    SectionID:'',
    UnitID:'',
    Department:'',
    Section:'',
    Unit:'',
    HOD:false,
    HOS:false,
    HOU:false,
    balanceLeave: '',
    CanRaiseOfficalRequest:false,
    CanManageNews:false,
    CanEditContact:false
  };

  userMgmntArr:any = {};
  isDepartmentListSet:boolean = false;
  isSectionListSet:boolean = false;
  isUnitListSet:boolean = false;
  isUserMgmtListSet:boolean = false;

  countryList:any = [];
  emiratesList: any;
  AnnouncementTypeNameList:any = [];
  noItemsMessage:any;
  selectedType:any;
  isModified: boolean = false;
  approversArray: Array<any> = [];
  removersArray: Array<any> = [];

  constructor(private modalService: BsModalService,
    private annoucementService: AnnouncementService,
    private router: Router,
    private common: CommonService,
    public arabicService:ArabicDataService,
    private dropDownService:DropdownsService,
    private adminService:AdminService,
    private changeDetector:ChangeDetectorRef) {
      this.lang = this.common.currentLang;
      if(this.lang == 'en'){
        this.common.breadscrumChange('Master Admin','Settings','');
        this.common.topBanner(false, '', '', '');
      } else if(this.lang== 'ar'){
        this.common.breadscrumChange(this.arabicfn('masteradmin'), this.arabicfn('settings'), '');
        this.common.topBanner(false, '', '', '');
      }
      this.noItemsMessage = {
        emptyMessage: this.lang === 'en' ? 'No data to display' : this.arabicfn('nodatatodisplay')
      }
    }

  ngOnInit() {
    debugger
    let th = this;
    let ddTypes = Object.keys(DropDownTypes);
    // Object.keys(DropDownTypes).slice(ddTypes.length/2).forEach((type)=>{
    //   th.dropDownTypeList.push({value:DropDownTypes[type],label:type});
    // });
    th.dropDownTypeList = [];
    if(this.lang == 'ar'){
      this.columns = [
        { name: this.arabicfn('refid'), prop: 'LookupsID' },
        { name: this.arabicfn('englishvalue'), prop: 'DisplayName' },
        { name: this.arabicfn('arabicvalue'), prop: 'ArDisplayName' },
        { name: this.arabicfn('type'), prop: 'Type' },
        { name: this.arabicfn('action'), cellTemplate: this.actionTemplate },
      ];
      this.userManagementColumns = [
        { name: this.arabicfn('action')},
        { name: this.arabicfn('caneditcontacts'), },
        { name: this.arabicfn('canmanagenewsphotos') },
        { name: this.arabicfn('canraiseofficialrequest'), },
        { name: this.arabicfn('headofunit'),  },
        { name: this.arabicfn('headofsection'),   },
        { name: this.arabicfn('headofdepartment')},
        { name: this.arabicfn('unit'), },
        { name: this.arabicfn('section')},
        { name: this.arabicfn('department'),  },
        { name: this.arabicfn('userlogonname')},
      ];
      // this.userManagementColumns = [
      //   { name: this.arabicfn('userlogonname')},
      //   { name: this.arabicfn('department'),  },
      //   { name: this.arabicfn('section')},
      //   { name: this.arabicfn('unit'), },
      //   { name: this.arabicfn('headofdepartment')},
      //   { name: this.arabicfn('headofsection'),   },
      //   { name: this.arabicfn('headofunit'),  },
      //   { name: this.arabicfn('canraiseofficialrequest'), },
      //   { name: this.arabicfn('canmanagenewsphotos') },
      //   { name: 'يمكن تحرير جهات الاتصال', },
      //   { name: this.arabicfn('action')},
      // ];
      ddTypes = Object.keys(DropDownTypes);
      Object.keys(DropDownTypes).slice(ddTypes.length/2).forEach((type,key)=>{
        th.dropDownTypeList.push({value:DropDownTypes[type],label:this.arabicfn(type)});
      });
    }else if(this.lang == 'en'){
      this.columns = [
        { name: 'Ref ID', prop: 'LookupsID' },
        { name: 'English Value', prop: 'DisplayName' },
        { name: 'Arabic Value', prop: 'ArDisplayName' },
        { name: 'Type', prop: 'Type' },
        { name: 'Action', cellTemplate: this.actionTemplate },
      ];
      this.userManagementColumns = [
        { name: 'User Logon Name', prop : 'EmployeeName'},
        { name: 'Department', prop: 'Department'},
        { name: 'Section', prop: 'Section'},
        { name: 'Unit', prop: 'Unit'},
        { name: 'Head Of Department', prop: 'HOD'},
        { name: 'Head Of Section'},
        { name: 'Head Of Unit',},
        { name: 'Can Raise Official Request'},
        { name: 'Can Manage News / Photos / Banner'},
        { name: 'Can Manage Contacts'},
        { name: 'Action'},
      ];
      debugger
      ddTypes = Object.keys(DropDownTypes);
      Object.keys(DropDownTypes).slice(ddTypes.length/2).forEach((type)=>{
        th.dropDownTypeList.push({value:DropDownTypes[type],label:type});
      });
    }
    this.adminService.getDepartments(this.currentUser.id).subscribe((depList:any) => {
      this.departmentList = depList;
      this.isDepartmentListSet = true;
      this.setUserManagementData();
    });
    this.adminService.getSections(this.currentUser.id).subscribe((secList:any) => {
      this.sectionList = secList;
      this.isSectionListSet = true;
      this.setUserManagementData();
    });
    this.adminService.getUnits(this.currentUser.id).subscribe((unList:any) => {
      this.unitList = unList;
      this.isUnitListSet = true;
      this.setUserManagementData();
    });
    this.adminService.getMailRemainder(this.currentUser.id).subscribe((remainderData:any) => {
      if(remainderData){
        this.notification.hours = remainderData.MailRemainder;
        this.adminGetData.hours = remainderData.MailRemainder;
      }
    });
    this.dropDownService.getCountries(this.currentUser.id).subscribe((countryData:any) => {
      if(countryData){
        this.countryList = countryData;
      }
    })
    // getting emirates list
    this.adminService.getMasterData({Type: 10},this.currentUser.id)
    .subscribe((emirates: any) => {
      this.emiratesList = emirates
    });
//getting Announcement name list
this.annoucementService.getAnnoucement(0, this.currentUser.id)
.subscribe((announcement: any) => {
  this.AnnouncementTypeNameList = announcement.AnnouncementTypeAndDescription; 
});
    this.getAllUsersByDepartment();
    this.getUserManagementListData();
    this.getLatestHolidayAttachment();
  }// end of ngOnInit

  notificationToggle(e?:any){
    if(e!=undefined || e!=null){
      this.notificationSection = e;
    }else{
      return this.notificationSection;
    }
  }

  approverToggle(e?:any){
    if(e!=undefined || e!=null){
      this.approverSection = e;
    }else{
      return this.approverSection;
    }
  }

  dropDownToggle(e?:any){
    if(e!=undefined || e!=null){
      this.dropDownTypeSection = e;
    }else{
      return this.dropDownTypeSection;
    }
  }

  userManagementToggle(e?:any){
    if(e!=undefined || e!=null){
      this.userManagementSection = e;
    }else{
      return this.userManagementSection;
    }
  }

  holidaysToggle(e?:any){
    if(e!=undefined || e!=null){
      this.holidaysSection = e;
    }else{
      return this.holidaysSection;
    }
  }

  moveToApproverList(){
    this.isModified = true;
    this.isSameUsers(this.toBeApprovers, 'approvers');
    this.toBeApprovers.forEach((tba)=>{
      this.approverList = [...this.approverList,tba];
    });
    let toSpliceAul:any = this.departmentUserList;
    this.toBeApprovers.forEach((tba) => {
      let tbaInd = toSpliceAul.findIndex((aul) => aul.UserProfileID == tba.UserProfileID);
      if(tbaInd > -1){
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
    // this.isModified = true;
    let removersIds = [];
    this.removedUsers = this.approvedUsers;
    
    this.approvedUsers.forEach((user:any) => {
      removersIds.push(user.UserProfileID);
    });
    if (removersIds && removersIds.length) {
      this.adminService.canApproverRemoved(removersIds)
      .subscribe((canRemove:any) => {
        if(canRemove) {
          this.approvedUsers.forEach((apu)=>{
            this.departmentUserList = [...this.departmentUserList,apu];
          });
          let toSpliceApl = this.approverList;
          this.approvedUsers.forEach((apv) => {
            let apuInd = toSpliceApl.findIndex((apl) => apl.UserProfileID == apv.UserProfileID);
            if(apuInd > -1){
              toSpliceApl.splice(apuInd,1);
            }
          });
          this.approvedUsers = [];
          this.approverList = [...toSpliceApl];
          this.isSameUsers(this.removedUsers, 'removers');
        }else{
          this.isModified = false;
        }
      })
    }
  }

  getAllUsersByDepartment() {
    this.departmentUserList = [];
    this.toBeApprovers = [];
    this.approverList = [];
    this.approvedUsers = [];
    this.isModified = false;
    this.adminService.getAllUsers(
      this.currentUser.id,
      this.departmentSelect,
      this.dropDownSearchStrings.userListSearchStr
    ).subscribe((userList:any)=>{
      if(this.departmentSelect){
        this.departmentUserList = userList;
        this.isUserListSet = true;
      }else{
        this.allUserList = userList;
      }
    });
    if(this.departmentSelect){
      this.getApproverListByDepartment();
    }
  }

  getApproverListByDepartment(){
    this.adminService.getApproverList(this.currentUser.id, this.departmentSelect)
    .subscribe((approverList:any) => {
      let toSetApproverList = [];
      approverList.forEach((approverID) => {
        this.allUserList.forEach((auObj) => {
          if(auObj.UserProfileID == approverID){
            toSetApproverList.push({
              UserProfileID:auObj.UserProfileID,
              EmployeeName:auObj.EmployeeName
            });
          }
        });
      });
      this.approverList = toSetApproverList;
    });
  }

  saveMailReminderHours(){
    if(this.notification && this.notification.hours){
      let toSendRemainderHours = {
        MailRemainder: this.notification.hours,
      };

      if(this.adminGetData.hours) {
        this.adminService.updateMailRemainder(toSendRemainderHours,this.currentUser.id).subscribe((remainderData:any) => {
          if(remainderData){
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = 'Mail Reminder Frequency Updated Successfully';
            if(this.lang == 'ar'){
              this.bsModalRef.content.message = this.arabicfn('mailreminderreqcreatemsg');
            }
          }
        });
      } else {
        this.adminService.saveMailRemainder(toSendRemainderHours,this.currentUser.id).subscribe((remainderData:any) => {
          if(remainderData){
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = 'Mail Reminder Frequency Saved Successfully';
            if(this.lang == 'ar'){
              this.bsModalRef.content.message = this.arabicfn('mailreminderrequpdatemsg');
            }
          }
        });
      }
    }
  }

  onUserListSearch(e){
    if(e && e.length > 2){
      this.dropDownSearchStrings.userListSearchStr = e;
    }else{
      this.dropDownSearchStrings = '';
    }
    this.getAllUsersByDepartment();
  }

  onApproverListSearch(e){
    if(e && e.length > 2){
      this.dropDownSearchStrings.approverListSearchStr = e;
    }else{
      this.dropDownSearchStrings.approverListSearchStr = '';
    }
    this.getApproverListByDepartment();
  }

  saveApproversList(){
    if(this.departmentSelect){
      let toSendApproversList  = [];
      if(this.approverList.length > 0){
        this.approverList.forEach((apl)=> {
          toSendApproversList.push(apl.UserProfileID);
        });
      }
      this.adminService.saveApproverList(toSendApproversList,this.currentUser.id,this.departmentSelect).subscribe((approverData)=> {
        if(approverData){
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = 'Approver List Updated Successfully';
          if(this.lang == 'ar'){
            this.bsModalRef.content.message = this.arabicfn('approverrequpdatemsg');
          }
          this.getAllUsersByDepartment();
        }
      });
    }
  }

  holidayAttachments(event) {
    this.img_file = event.target.files;
    if(this.img_file.length > 0){
      this.isExcelImport = true;
      for(let imf = 0; imf < this.img_file.length; imf++){
        let fileExtensionStr = this.img_file[imf].name.substring(this.img_file[imf].name.lastIndexOf('.')+1,this.img_file[imf].name.length);
        if(!(fileExtensionStr == 'xls' || fileExtensionStr == 'xlsx' || fileExtensionStr == 'csv')){
          this.isExcelImport = false;
        }
      }
      if(this.isExcelImport){
        this.isApiLoading = true;
        this.uploadProcess = true;
        this.attachments = [];
        this.adminService.uploadHolidaysAttachment(this.img_file)
          .subscribe((attachementRes:any) => {
          this.isApiLoading = false;
          if (attachementRes.type === HttpEventType.UploadProgress) {
            this.uploadPercentage = Math.round(attachementRes.loaded / attachementRes.total) * 100;
          } else if (attachementRes.type === HttpEventType.Response) {
            this.isApiLoading = false;
            this.uploadProcess = false;
            this.uploadPercentage = 0;
            let tempData = []
            for (var i = 0; i < attachementRes.body.FileName.length; i++) {
              tempData.push({'AttachmentGuid':attachementRes.body.Guid,'AttachmentsName':attachementRes.body.FileName[i],currentUpload:true});
            }
            this.attachments = tempData;
            this.fileInput.nativeElement.value = "";
          }
        });
      }else{
        this.fileInput.nativeElement.value = "";
      }
    }
  }


  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.fileInput.nativeElement.value = "";
  }

  importHolidayAttachments() {
    let toSendImportData:any = [];
    this.attachments.forEach((atObj)=>{
      toSendImportData.push({FileName:atObj.AttachmentsName,Guid:atObj.AttachmentGuid,UserID:this.currentUser.id});
    });
    if(toSendImportData[0]){
      this.adminService.sendHolidayAttachmentForImport(toSendImportData[0])
      .subscribe((holidayImportRes) => {
        if(holidayImportRes){
          this.attachments = [];
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = 'Holidays List Imported Successfully';
          if(this.lang == 'ar'){
            this.bsModalRef.content.message = this.arabicfn('holidayslistimportmsg');
          }
          this.getLatestHolidayAttachment();
        }
      });
    }
  }

  // get latest holiday attachment
  getLatestHolidayAttachment() {
    this.adminService.getLatestHolidayFile()
    .subscribe((res:any) => {
      if (res && res.length) {
        this.latestAttachment = res;
      }
    });
  }

  // prepare attachment download url
  prepareDownloadUrl(item:any) {
    return this.adminService.prepareAttachmentUrl(item);
  }

  getUserManagementListData() {
    let userManagementListOptions:any = {
      PageNumber: this.userManagementTableconfig.page,
      PageSize: this.userManagementTableconfig.itemsPerPage,
      searchStr:this.userSearch
    };
    this.adminService.getUserManagementList(userManagementListOptions,this.currentUser.id).subscribe((userManagementDataVar) => {
      if(userManagementDataVar){
        this.userMgmntArr = userManagementDataVar;
        this.userManagementRows = userManagementDataVar.Collection;
        this.userManagementTableconfig.totalItems = userManagementDataVar.Count;
        this.isUserMgmtListSet = true;
        this.setUserManagementData();
      }
    });
  }

  setUserManagementData(){
    if(this.isDepartmentListSet && this.isSectionListSet && this.isUnitListSet && this.isUserMgmtListSet){
      this.userMgmntArr.Collection.forEach((cObj) => {
        this.departmentList.forEach((dObj) => {
          if(cObj.DepartmentID == dObj.DepartmentID){
            cObj.Department = dObj.DepartmentName;
          }
        });
        this.sectionList.forEach((sObj) => {
          if(cObj.SectionID == sObj.SectionID){
            cObj.Section = sObj.SectionName;
          }
        });
        this.unitList.forEach((uObj) => {
          if(cObj.UnitID == uObj.UnitID){
            cObj.Unit = uObj.UnitName;
          }
        });
      });
      this.userManagementRows = this.userMgmntArr.Collection;
      this.userManagementTableconfig.totalItems = this.userMgmntArr.Count;
    }
  }

  getDropDownTypeList(type?: any) {
    this.annoucementService.getAnnoucement(0, this.currentUser.id)
.subscribe((announcement: any) => {
  this.AnnouncementTypeNameList = announcement.AnnouncementTypeAndDescription; 
});
    if (type == 'typeChange') {
      this.filterBy.Value = '';
    }
    if(this.dropDownType && this.dropDownType.value){
      this.selectedType = this.dropDownType.value;
      let dropDownListOptions:any = {
        PageNumber: this.config.page,
        PageSize: this.config.itemsPerPage,
        Type:this.dropDownType.value,
        searchStr: this.filterBy.Value
      };
      this.adminService.getMasterData(dropDownListOptions,this.currentUser.id)
      .subscribe((masterDataVar) => {
        if(masterDataVar){
          masterDataVar.forEach((mdvObj) => {
            mdvObj.Type = this.dropDownType.label;
          });
          this.rows = masterDataVar;
          this.config.totalItems = masterDataVar.length;
        }
      });
    }else{
      this.rows = [];
    }    
  }

  onChangeUserPage(){
    this.getUserManagementListData();
  }
  openAddEditDropDownValueModal(formType:any,data?:any) {
    this.typeFormDataModel = {
      Type:'',
      Value:'',
      arValue:''
    };
    if(this.dropDownType) {
      this.typeFormDataModel.Type = this.dropDownType.label;
    }
    if(formType == 'add'){
      this.typeFormDataModel.Value = '';
      this.typeFormDataModel.arValue = '';
      this.typeFormTitle = 'Add Value';
      if(this.lang != 'en'){
        this.typeFormTitle = this.arabicfn('addvalue');
      }
      this.bsModalRef = this.modalService.show(this.typeValueFormTemplate);
    }else if(formType == 'edit'){
      this.typeFormDataModel.Value = data.DisplayName;
      this.typeFormDataModel.arValue = data.ArDisplayName;
      this.typeFormDataModel.LookupsID = data.LookupsID;
      if((this.typeFormDataModel.Type.trim().toLowerCase() == 'city') || (this.typeFormDataModel.Type == this.arabicfn('city'))){
        this.typeFormDataModel.Country = data.CountryID;
        this.typeFormDataModel.Emirates = data.EmiratesID;
      }
      this.typeFormTitle = 'Edit Value';
      if(this.lang != 'en'){
        this.typeFormTitle = this.arabicfn('editvalue');
      }
      this.bsModalRef = this.modalService.show(this.typeValueFormTemplate);
    }

  }

  openDeleteDropDownValueModal(rowData){
    this.toDeleteValue = rowData.LookupsID;
    this.bsModalRef = this.modalService.show(this.deleteTypeValue);
  }

  openUserManagementEditModal(rowData){
    this.userManagementModel = {
      CanEditContact: rowData.CanEditContact,
      CanManageNews: rowData.CanManageNews,
      CanRaiseOfficalRequest: rowData.CanRaiseOfficalRequest,
      Department: rowData.Department,
      DepartmentID: rowData.DepartmentID,
      EmployeeName: rowData.EmployeeName,
      HOD: rowData.HOD,
      HOS: rowData.HOS,
      HOU: rowData.HOU,
      SectionID: rowData.SectionID,
      Unit: rowData.Unit,
      UnitID: rowData.UnitID,
      UserProfileID: rowData.UserProfileID,
      balanceLeave: rowData.balanceLeave
    };
    this.bsModalRef = this.modalService.show(this.editUserDataTemplate);
  }

  closemodal(){
    this.bsModalRef.hide();
  }

  onChangeCountry() {
    this.typeFormDataModel.Emirates = null;
  }

  onChangeEmirates() {
    this.typeFormDataModel.Country = null;
  }
  onChangeAnnouncement(){    
    if(this.typeFormDataModel.LookupsID){
      let types = _.filter(this.AnnouncementTypeNameList, { 'AnnouncementTypeID': this.typeFormDataModel.LookupsID});
      var type: any = types[0];
      this.typeFormDataModel.Value = type.Description;
      this.typeFormDataModel.arValue = type.DescriptionAr;
    }
    this.validateTypeDialog();
  }
  saveDropDownValueData(){
    if(!this.validateTypeDialog()){
      if(!this.typeFormDataModel.LookupsID){
        let toSendTypeModel :any = {
          DisplayName:this.typeFormDataModel.Value.trim(),
          DisplayOrder:this.rows.length,
          ArDisplayName:this.typeFormDataModel.arValue.trim()
        };

        let dropDownOptions:any = {
          Type: this.dropDownType.value
        };
        if (this.typeFormDataModel.Country) {
          dropDownOptions.Country = this.typeFormDataModel.Country
        } else if (this.typeFormDataModel.Emirates) {
          dropDownOptions.Emirates = this.typeFormDataModel.Emirates
        }
        this.adminService.createMasterData(toSendTypeModel,this.currentUser.id,dropDownOptions).subscribe((dropDownDataRes) => {
          if(dropDownDataRes){
            this.bsModalRef.hide();
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = 'Dropdown Value Added Successfully';
            if(this.lang == 'ar'){
              this.bsModalRef.content.message = this.arabicfn('dropdownreqcreatemsg');
            }
            this.getDropDownTypeList();
          }
        });
      }else{
        let toSendTypeModel :any = {
          LookupsID: this.typeFormDataModel.LookupsID,
          DisplayName:this.typeFormDataModel.Value.trim(),
          ArDisplayName:this.typeFormDataModel.arValue.trim(),
          DisplayOrder:this.rows.length
        };
        let dropDownOptions:any = {
          Type: this.dropDownType.value
        };
        if (this.typeFormDataModel.Country) {
          dropDownOptions.Country = this.typeFormDataModel.Country
        } else if (this.typeFormDataModel.Emirates) {
          dropDownOptions.Emirates = this.typeFormDataModel.Emirates
        }
        this.adminService.updateMasterData(toSendTypeModel,this.currentUser.id,dropDownOptions).subscribe((dropDownDataRes) => {
          if(dropDownDataRes){
            this.bsModalRef.hide();
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = (this.typeFormDataModel.Type.trim().toLowerCase() == 'Announcement Description'.trim().toLowerCase() && this.typeFormTitle == 'Add Value') ? 'Dropdown Value Added Successfully' : 'Dropdown Value Updated Successfully';
            if(this.lang == 'ar'){
              this.bsModalRef.content.message = (this.typeFormDataModel.Type.trim().toLowerCase() == 'Announcement Description'.trim().toLowerCase() && this.typeFormTitle == 'Add Value') ?  this.arabicfn('dropdownreqcreatemsg') :  this.arabicfn('dropdownrequpdatemsg');
            }
            this.getDropDownTypeList();
          }
        });
      }
    }
  }

  deleteDropDownValueData(lookupsID){
    debugger
    let toSendTypeModel:any = {
      LookupsID:lookupsID,
      Type: this.dropDownType.value
    };
    this.adminService.removeMasterData(toSendTypeModel,this.currentUser.id).subscribe((dropDownDelRes) => {
      if(dropDownDelRes){
        this.bsModalRef.hide();
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = 'Dropdown Value Deleted Successfully';
        if(this.lang == 'ar'){
          this.bsModalRef.content.message = this.arabicfn('dropdownreqdeletemsg');
        }
        this.getDropDownTypeList();
      }
    });
  }

  saveUserManagementData(){
    let toSendUserData:any = {
      UserProfileID:this.userManagementModel.UserProfileID,
      EmployeeName:this.userManagementModel.EmployeeName,
      DepartmentID:this.userManagementModel.DepartmentID,
      SectionID:this.userManagementModel.SectionID,
      UnitID:this.userManagementModel.UnitID,
      HOD:this.userManagementModel.HOD,
      HOS:this.userManagementModel.HOS,
      HOU:this.userManagementModel.HOU,
      balanceLeave: this.userManagementModel.balanceLeave,
      CanRaiseOfficalRequest:this.userManagementModel.CanRaiseOfficalRequest,
      CanManageNews:this.userManagementModel.CanManageNews,
      CanEditContact:this.userManagementModel.CanEditContact
    };
    this.adminService.saveUserManagementData(toSendUserData,this.currentUser.id).subscribe((usObj)=> {
      if(usObj){
        this.bsModalRef.hide();
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = 'User Data Updated Successfully';
        if(this.lang == 'ar'){
          this.bsModalRef.content.message = this.arabicfn('userdatarequpdatemsg');
        }
        this.getUserManagementListData();
      }
    });
  }

  removeWordSpaces(words:string){
    return  words.replace(/\s+/g, '');
  }

  validateTypeDialog(){
    let isInvalid = false;
    if(this.lang.toLowerCase() == 'en' ? (this.typeFormDataModel.Type.trim().toLowerCase() == 'Announcement Description'.trim().toLowerCase()) : (this.typeFormDataModel.Type == this.arabicfn('announcementdescription')))   
    {  
    this.typeFormDataModel.Value =  this.typeFormDataModel.Value == undefined ? '' : this.typeFormDataModel.Value.replace(/<[^>]*>/g, '');
    this.typeFormDataModel.Value = this.typeFormDataModel.Value == undefined ? '' : this.typeFormDataModel.Value.replace(/&nbsp;/g, '');
    this.typeFormDataModel.arValue = this.typeFormDataModel.arValue == undefined ? '' : this.typeFormDataModel.arValue.replace(/<[^>]*>/g, '');
    this.typeFormDataModel.arValue = this.typeFormDataModel.arValue == undefined ? '' :this.typeFormDataModel.arValue.replace(/&nbsp;/g, '');
    }
    if((this.typeFormDataModel.Type.trim().toLowerCase() == 'city') ||
      (this.typeFormDataModel.Type == this.arabicfn('city'))) {
      if (this.dropDownType.value &&
        this.typeFormDataModel.Type &&
        (this.typeFormDataModel.Value &&
          this.typeFormDataModel.Value.trim() ||
          this.typeFormDataModel.arValue && this.typeFormDataModel.arValue.trim()) &&
        (this.typeFormDataModel.Country || this.typeFormDataModel.Emirates)) {
        isInvalid = false;
      } else {
        isInvalid = true;
      }
    } else if(this.dropDownType.value &&
      this.typeFormDataModel.Type.trim().toLowerCase() != 'city' &&
      (this.typeFormDataModel.Type != this.arabicfn('city'))) {
      if (this.dropDownType.value &&
        this.typeFormDataModel.Type &&
        (this.typeFormDataModel.Value && this.typeFormDataModel.Value.trim() ||
        this.typeFormDataModel.arValue && this.typeFormDataModel.arValue.trim())){
        isInvalid = false;
      } else {
        isInvalid = true;
      }
    }
    return isInvalid;
  }

  arabicfn(word) {
    return this.common.arabic.words[word.trim().replace(/ +/g, "").toLowerCase()];
  }

}
