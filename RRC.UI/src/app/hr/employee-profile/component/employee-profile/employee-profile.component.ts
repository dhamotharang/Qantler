import { Component, OnInit, Input, ViewChild, ElementRef, TemplateRef, Inject, Renderer2 } from '@angular/core';
import { EmployeeFormModel } from '../../model/employee-form.model'
import { BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { EmployeeService } from '../../service/employee.service';
import { ActivatedRoute, Router,Event, NavigationEnd } from '@angular/router';
import { DatePipe, DOCUMENT } from '@angular/common';
import { CommonService } from 'src/app/common.service';
import { HttpEventType } from '@angular/common/http';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-employee-profile',
  templateUrl: './employee-profile.component.html',
  styleUrls: ['./employee-profile.component.scss']
})
export class EmployeeProfileComponent implements OnInit {
  @Input() screenStatus: string;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  @ViewChild('variable')myInputVariable: ElementRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));

  empFormData = {
    UserProfileId:0,
    Date :  new Date(),
    EmployeeName : "",
    EmployeeCode : "",
    OfficialMailId : "",
    PersonalMailId : "",
    Gender : "Male",
    BirthDate : null,
    Age : 0,
    CountryofResidence : null,
    MobileNumber : "",
    EmployeePhoneNumber : "",
    Religion : null,
    Nationality : null,
    PreviousNationality :null,
    JoinDate : null,
    Title : null,
    Grade : null,
    EmployeePosition : "",
    EmploymentStatus : null,
    Resigned : "No",
    ResignationDate : null,
    BalanceLeave : 0,
    NotificationPreferencesSMS : "",
    NotificationPreferencesEmail : "",
    Education : [{
        Degree : null,
        SchoolCollege: "",
        FieldStudy: "",
        TimePeriodFrom: null,
        TimePeriodTo: null,
        UserProfileEducationID:0,
        isValidDate: false
    }],
    WorkExperience : [{
        JobTitle : "",
        Company: "",
        City: "",
        TimePeriodFrom: null,
        TimePeriodTo: null,
        WorkExperienceID: 0,
        isValidDate: false
    }],
    TrainingCertifications : [{
        TrainingName: "",
        StartDate: null,
        EndDate: null,
        TrainingID: "",
        isValidDate: false,
        Attachment:[]
    }],
    OrgUnitID: null,
    RoleId: "",
    PassportNumber : "",
    PassportIssuePlace : "",
    PassportIssueDate :  null,
    PassportExpiryDate : null,
    VisaNumber : "",
    VisaIssueDate : null,
    VisaExpiryDate : null,
    EmiratesIdNumber : "",
    EmiratesIdIssueDate : null,
    EmiratesIdExpiryDate : null,
    InsuranceNumber : "",
    InsuranceIssueDate : null,
    InsuranceExpiryDate : null,
    LabourContractNumber : "",
    LabourContractIssueDate : null,
    LaborContractExpiryDate : null,
    ContractTypes: "",
    Attachment: [],
    CreatedBy : 0,
    CreatedDateTime : new Date(),
    Action: '',
    LoginUser: "",
    ProfilePhotoID: "",
    ProfilePhotoName: "",
    SignaturePhotoID:"",
    SignaturePhoto:""
  };
  @ViewChild('template') template : TemplateRef<any>;
  checked: boolean = false;
  notificationchecked: boolean;
  notificationPreference = [
    {
     name: "EMail",
     checked: false
    },
    {
      name: "SMS",
      checked: false
    }
  ]
  HRid:number;
  AttachmentType: any;
  attachments: any = [];
  titleList: Array<any> = [];
  GradeList = [];
  attachmentTypeList: Array<any> =  [];
  empFormModel: EmployeeFormModel = new EmployeeFormModel();
  nationalityList: Object;
  Nationlaity:any =[];
  religionList: Object;
  countryList: Object;
  employeeId: number;
  employeeResult: any;
  uploadProcess: boolean = false;
  uploadPercentage: number;
  qualificationList: any=[];
  employeeStatus: any=[];
  message: string;
  departmentList: any=[];
  photoattachments: any = [];
  ImageUrl: string;
  submitted =  false;
  updateValidation:any = {};
  errorEmpNameMsg = false;
  errorEmpCodeMsg = true;
  errorEmpOffMail = false;
  errorEmpGenderMsg = true;
  errorEmpCountryMsg = false;
  errorEmpbirthdayMsg = false;
  errorEmpMobileMsg = false;
  errorEmpPhoneMsg = false;
  errorEmpReligionMsg = false;
  errorEmpNationalityMsg = false;
  errorEmpPreNationalityMsg = false;
  errorEmpJoinDateMsg = false;
  errorEmpTitleMsg = false;
  errorEmpGradeMsg = true;
  errorEmpOrgMsg = false;
  errorEmpPositionMsg = true;
  errorEmpStatusMsg = false;
  errorEmpResignedMsg = false;
  errorEmpResignedDateMsg = false;
  errorEmpBalLeaveMsg = true;
  errorEmpNotifiMsg = false;
  errorEmpAgeMsg = false;
  errorEmpPerMail = false;
  errorEmpEducationDegreeMsg = false;
  errorEmpEducationFieldStudy = false;
  errorEmpEducationTimePeriodFrom = false;
  errorEmpEducationTimePeriodTo = false;
  errorEmpEducationSchoolCollege = false;
  errorattachmentsMsg = false;
  errorVisaNoMsg = false;
  errorEmpConExDateMsg = false;
  errorConIssDateMsg = false;
  errorEmpLabConNoMsg = false;
  errorEmpEmiExDateMsg = false;
  errorEmpEmiIdNoMsg = false;
  errorEmpPassExDateMsg = false;
  errorEmpPassIssDateMsg = false;
  errorEmpPassIssPlaceMsg = false;
  errorEmpPassNoMsg = false;
  errorVisaExpMsg = false;
  errorVisaIssMsg = false;
  errorEmpEmiIssDateMsg = false;
  errorAttchmentTypeMsg = false;
  isEduStartEndDiff: boolean = false;
  isWorkStartEndDiff: boolean = false;
  isTrainingStartEndDiff: any = false;
  lang: any;
  isEngLang: boolean = true;
  signatureAttachments: any = [];
  signatureUploadProcess: boolean = false;
  signatureUploadPercentage: number;
  signatureErrorattachmentsMsg = false;
  downloadUrl;
  @ViewChild('signVar')mySignVariable: ElementRef;
  isHRDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 9;
  isHRDepartmentTeamUserID = this.currentUser.OrgUnitID == 9 && !this.currentUser.IsOrgHead;
  validEmail: boolean;
  validOffcMail: boolean;
  enterOfflMail: boolean;
  editPageSource: any;
  trainingAttachments: any = [];
  Trainingattachment: string
  MyProfileClick: boolean=false;
  unsub:Subscription;
  constructor(private endpoint: EndPointService, private renderer: Renderer2, @Inject(DOCUMENT) private document: Document, public router: Router, public utils: UtilsService, public common: CommonService, private service: EmployeeService, private route: ActivatedRoute, private datePipe: DatePipe,public dropdownService: DropdownsService, public modalService: BsModalService,) {
    debugger;
    route.params.subscribe(param => {
      this.employeeId = +param.id;
      if(param.from){
      this.editPageSource = param.from;
      }
      if (this.employeeId > 0)
        this.loadData(this.employeeId);
    });
    this.downloadUrl = this.endpoint.fileDownloadUrl;
  }


  ngOnInit() {

//  this.unsub =   this.common.routervalid.subscribe(data=>{
//       if(data){
//         debugger
//         this.MyProfileClick = true;
//       }else{
//         this.MyProfileClick = false;
//       }
//     })
    this.HRid = this.currentUser.OrgUnitID;
    // if (this.screenStatus == 'View'){
    //   if(!this.isHRDepartmentHeadUserID && this.currentUser.id != this.employeeId ){
    //     this.router.navigate(['/error']);
    //   }
    // }else if(this.screenStatus == 'edit'){
    //   if(!this.isHRDepartmentHeadUserID){
    //     this.router.navigate(['/error']);
    //   }
    // }else{
    //   this.router.navigate(['/error']);
    // }
    this.common.homeScrollTop();
    this.lang = this.common.currentLang;
    this.dropdownService.getGrade(this.currentUser.id).subscribe((grade:any) => {
      this.GradeList = grade;
    });
    this.dropdownService.getTitle(this.currentUser.id).subscribe((title:any) => {
      this.titleList = title;
    });
    if(this.lang == 'en'){
      this.isEngLang = true;
      this.attachmentTypeList = ["Passport", "Visa", "Emirates ID", "Insurance", "Labour Contract"];
      // this.titleList = [{name:"Mr", value:"1"},{name:"Mrs", value:"2"}];
    }else {
      this.isEngLang = false;
      this.attachmentTypeList = [this.arabicfn('passport'), this.arabicfn('visa'), this.arabicfn('emiratesid'), this.arabicfn('insurance'), this.arabicfn('labourcontract')];
      // this.titleList = [{name:this.arabicfn('mr'), value:"1"},{name:this.arabicfn('mrs'), value:"2"}];
    }
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.getCountry();
    if (this.screenStatus == 'View'){
      if (this.isEngLang) {
        this.common.breadscrumChange('HR', 'Employee Profile', 'View Profile');
      } else {
        this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('employeeprofile'), this.arabicfn('viewprofile'));
      }
    } else if(this.screenStatus == 'edit') {
      if (this.isEngLang) {
        this.common.breadscrumChange('HR', 'Employee Profile', 'Update Profile');
      } else {
        this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('employeeprofile'), this.arabicfn('updateprofile'));
      }
    } else {
      if (this.isEngLang) {
        this.common.breadscrumChange('HR', 'Employee Profile', 'Create');
      } else {
        this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('employeeprofile'), this.arabicfn('creation'));
      }
    }
    // if (this.isEngLang) {
    //   this.titleList = ["Mr", "Mrs"];
    //   this.attachmentTypeList =  ["Passport", "Visa", "Emirates ID", "Insurance", "Labour Contract"];
    // } else {
    //   this.titleList = [this.arabicfn("mr"), this.arabicfn("mrs")];
    //   this.attachmentTypeList =  [this.arabicfn("passport"), this.arabicfn("visa"), this.arabicfn("emiratesid"), this.arabicfn("insurance"), this.arabicfn("labourcontract")];
    // }


  }

  // validateEduction() {
  //   var flag = true;
  //     for(var i = 0; i< this.empFormData.Education.length; i++) {
  //       // if(this.empFormData.Education[i].Degree && this.empFormData.Education[i].FieldStudy &&
  //       //   this.empFormData.Education[i].SchoolCollege && this.empFormData.Education[i].TimePeriodFrom &&
  //       //   this.empFormData.Education[i].TimePeriodTo) {
  //       //   flag = false;
  //       // }
  //       if(!this.empFormData.Education[i].Degree) {
  //         flag = true;
  //       }else {
  //         flag = false;
  //       }
  //       if(!this.empFormData.Education[i].SchoolCollege || !this.empFormData.Education[i].SchoolCollege.trim()) {
  //         flag = true;
  //       }else {
  //         flag = false;
  //       }
  //       if(!this.empFormData.Education[i].FieldStudy || !this.empFormData.Education[i].FieldStudy.trim()) {
  //         flag = true;
  //       }else {
  //         flag = false;
  //       }
  //       if(!this.empFormData.Education[i].TimePeriodFrom) {
  //         flag = true;
  //       }else {
  //         flag = false;
  //       }
  //       if(!this.empFormData.Education[i].TimePeriodTo) {
  //         flag = true;
  //       }else {
  //         flag = false;
  //       }
  //     }
  //   return flag;
  // }

  emailValidation() {
    if (this.empFormData.PersonalMailId) {
      return true;
    } else {
      return false;
    }
  }

  checkEduStartEndDateDiff(index, event, type){
    if(type == "from"){
      this.empFormData.Education[index].TimePeriodFrom = event;
    }
    if(type == "to"){
      this.empFormData.Education[index].TimePeriodTo = event;
    }
    if(this.empFormData.Education[index].TimePeriodFrom && this.empFormData.Education[index].TimePeriodTo){
      if(new Date(this.empFormData.Education[index].TimePeriodFrom).getTime() < new Date(this.empFormData.Education[index].TimePeriodTo).getTime()){
        this.empFormData.Education[index].isValidDate = false;
        this.isEduStartEndDiff = false;
      }else{
        this.empFormData.Education[index].isValidDate = true;
        this.isEduStartEndDiff = true;
      }
      return this.isEduStartEndDiff;
    }
  }

  checkWorkStartEndDateDiff(index, event, type){
    if(type == "from"){
      this.empFormData.WorkExperience[index].TimePeriodFrom = event;
    }
    if(type == "to"){
      this.empFormData.WorkExperience[index].TimePeriodTo = event;
    }
    if(this.empFormData.WorkExperience[index].TimePeriodFrom && this.empFormData.WorkExperience[index].TimePeriodTo){
      if(new Date(this.empFormData.WorkExperience[index].TimePeriodFrom).getTime() < new Date(this.empFormData.WorkExperience[index].TimePeriodTo).getTime()){
        this.empFormData.WorkExperience[index].isValidDate = false;
        this.isWorkStartEndDiff = false;
      }else{
        this.empFormData.WorkExperience[index].isValidDate = true;
        this.isWorkStartEndDiff = true;
      }
      return this.isWorkStartEndDiff;
    }
  }

  checkTrainingStartEndDateDiff(index, event, type){
    if(type == "from"){
      this.empFormData.TrainingCertifications[index].StartDate = event;
    }
    if(type == "to"){
      this.empFormData.TrainingCertifications[index].EndDate = event;
    }
    if(this.empFormData.TrainingCertifications[index].StartDate && this.empFormData.TrainingCertifications[index].EndDate){
      if(new Date(this.empFormData.TrainingCertifications[index].StartDate).getTime() <= new Date(this.empFormData.TrainingCertifications[index].EndDate).getTime()){
        this.empFormData.TrainingCertifications[index].isValidDate = false;
        this.isTrainingStartEndDiff = false;
      }else{
        this.empFormData.TrainingCertifications[index].isValidDate = true;
        this.isTrainingStartEndDiff = true;
      }
      return this.isTrainingStartEndDiff;
    }
  }

  // validateWork() {
  //   var flag = true;
  //   for(var i = 0; i< this.empFormData.WorkExperience.length; i++) {
  //     if(!this.empFormData.WorkExperience[i].JobTitle || !this.empFormData.WorkExperience[i].JobTitle.trim()) {
  //       flag = true;
  //     }else {
  //       flag = false;
  //     }
  //     if(!this.empFormData.WorkExperience[i].Company || !this.empFormData.WorkExperience[i].Company.trim()) {
  //       flag = true;
  //     }else {
  //       flag = false;
  //     }
  //     if(!this.empFormData.WorkExperience[i].City || !this.empFormData.WorkExperience[i].City.trim()) {
  //       flag = true;
  //     }else {
  //       flag = false;
  //     }
  //     if(!this.empFormData.WorkExperience[i].TimePeriodFrom) {
  //       flag = true;
  //     }else {
  //       flag = false;
  //     }
  //     if(!this.empFormData.WorkExperience[i].TimePeriodTo) {
  //       flag = true;
  //     }else {
  //       flag = false;
  //     }
  //   }
  //   return flag;
  // }

  // validateTraining() {
  //   var flag = true;
  //   for(var i = 0; i< this.empFormData.TrainingCertifications.length; i++) {
  //     if(!this.empFormData.TrainingCertifications[i].TrainingName || !this.empFormData.TrainingCertifications[i].TrainingName.trim()) {
  //       flag = true;
  //     }else {
  //       flag = false;
  //     }
  //     if(!this.empFormData.TrainingCertifications[i].EndDate) {
  //       flag = true;
  //     }else {
  //       flag = false;
  //     }
  //     if(!this.empFormData.TrainingCertifications[i].StartDate) {
  //       flag = true;
  //     }else {
  //       flag = false;
  //     }
  //   }
  //   return flag;
  // }

  checkNullAndTrim(value?: any) {
    // (this.screenStatus=='View'||'Edit'&& this.HRid!=9)? false:true;
    if(this.screenStatus=='View'||'Edit'&& this.HRid!=9){
      return false
    }else{
      if (value == null) {
        return true;
      } else if (!value.toString().trim()) {
        return true;
      } else {
        return false;
      }
    } 
  }

  checkInvalidDate(value?: any) {
    if (value == 'Invalid Date' || value == null || value == "") {
      return true;
    }
    return false;
  }

  async loadData(employeeId) {
    await this.service.getEmpDetails(employeeId, this.currentUser.id).subscribe((data: any) => {
      this.employeeResult = data;
      if (this.screenStatus == 'View' || this.screenStatus == 'edit') {
        this.empFormData = data;
        // this.empFormData.Title = data.Title;
        // this.empFormData.Grade = data.Grade;
        this.empFormData.PreviousNationality = Number(this.empFormData.PreviousNationality);
          this.dropdownService.getEmpStatus(this.currentUser.id)
            .subscribe((employeeStatus:any) => {
            var  tempEmployeeStatus = employeeStatus.find(x=> x.EmployeeStatusID == this.empFormData.EmploymentStatus)
        this.empFormData.EmploymentStatus = tempEmployeeStatus ? parseInt(tempEmployeeStatus.EmployeeStatusID) : '';
            });
        this.dropdownService.getGrade(this.currentUser.id).subscribe((grade:any) => {
          this.GradeList = grade;
          var grade = this.GradeList.find(x=> x.GradeID == data.Grade);
          this.empFormData.Grade = grade ? parseInt(grade.GradeID) : '';
        });
        this.dropdownService.getTitle(this.currentUser.id).subscribe((title:any) => {
          this.titleList = title;
          var title = this.titleList.find(x=> x.TitleID == data.Title)
          this.empFormData.Title = title ? parseInt(title.TitleID) : '';
        });
        
        // this.service.getDetails('Nationality', this.currentUser.id).subscribe(res => {
        //    this.Nationlaity = res;
        //   // this.empFormData.PreviousNationality =this.Nationlaity[data.Nationality].NationalityID;
        //   var tempPreviousNationality = this.Nationlaity.filter(x=> x.NationalityID == data.PreviousNationality);
        //   this.empFormData.PreviousNationality = tempPreviousNationality.NationalityName
        //   this.empFormData.Nationality = this.Nationlaity.filter(x=> x.NationalityID == data.Nationality).NationalityName;
        // });

        if(this.screenStatus == 'edit'){
          this.checkValidation();
          // this.checkEduStartEndDateDiff();
          // this.checkWorkStartEndDateDiff();
          // this.checkTrainingStartEndDateDiff();
        }
        // this.prepareData();
        this.transformDate();
        this.attachments = this.employeeResult.Attachment;
        if(this.empFormData.ProfilePhotoID && this.empFormData.ProfilePhotoName) {
          this.ImageUrl =  this.endpoint.fileDownloadUrl+"?filename="+this.empFormData.ProfilePhotoName+"&guid="+this.empFormData.ProfilePhotoID;
        }

      } else {
        // this.initPage();
        // this.bottonControll();
      }
    });
  }

  transformDate() {
    // var currentDate = this.datePipe.transform(this.empFormData.CreatedDateTime, 'MM-dd-yyyy');
    // this.empFormData.Date = new Date(currentDate);
    this.empFormData.CreatedBy = this.empFormData.CreatedBy;
    this.empFormData.CreatedDateTime = new Date(this.empFormData.CreatedDateTime);
    this.empFormData.BirthDate = this.empFormData.BirthDate ? new Date(this.empFormData.BirthDate) : "";
    this.empFormData.EmiratesIdIssueDate = this.empFormData.EmiratesIdIssueDate ? new Date(this.empFormData.EmiratesIdIssueDate) : "";
    this.empFormData.EmiratesIdIssueDate = this.empFormData.EmiratesIdIssueDate ? new Date(this.empFormData.EmiratesIdIssueDate) : "";
    this.empFormData.EmiratesIdExpiryDate = this.empFormData.EmiratesIdExpiryDate ? new Date(this.empFormData.EmiratesIdExpiryDate) : "";
    this.empFormData.VisaIssueDate = this.empFormData.VisaIssueDate ? new Date(this.empFormData.VisaIssueDate) : "";
    this.empFormData.VisaExpiryDate = this.empFormData.VisaExpiryDate ? new Date(this.empFormData.VisaExpiryDate) : "";
    this.empFormData.PassportIssueDate = this.empFormData.PassportIssueDate ? new Date(this.empFormData.PassportIssueDate) : "";
    this.empFormData.PassportExpiryDate = this.empFormData.PassportExpiryDate ? new Date(this.empFormData.PassportExpiryDate) : "";
    this.empFormData.InsuranceIssueDate = this.empFormData.InsuranceIssueDate ? new Date(this.empFormData.InsuranceIssueDate) : "";
    this.empFormData.InsuranceExpiryDate = this.empFormData.InsuranceExpiryDate ? new Date(this.empFormData.InsuranceExpiryDate) : "";
    this.empFormData.LabourContractIssueDate = this.empFormData.LabourContractIssueDate ? new Date(this.empFormData.LabourContractIssueDate) : "";
    this.empFormData.LaborContractExpiryDate = this.empFormData.LaborContractExpiryDate ? new Date(this.empFormData.LaborContractExpiryDate) : "";
    this.empFormData.ResignationDate = this.empFormData.ResignationDate ? new Date(this.empFormData.ResignationDate) : "";
    this.empFormData.JoinDate = this.empFormData.JoinDate ? new Date(this.empFormData.JoinDate) : "";
    this.attachments = this.empFormData.Attachment;

    for(var i = 0; i< this.empFormData.Education.length; i++) {
      // if(this.empFormData.Education[i].TimePeriodFrom != null){
      //   this.empFormData.Education[i].TimePeriodFrom = new Date(this.empFormData.Education[i].TimePeriodFrom);
      // }
      if(this.empFormData.Education[i].TimePeriodTo != null){
        this.empFormData.Education[i].TimePeriodTo = new Date(this.empFormData.Education[i].TimePeriodTo);
      }
      // this.checkEduStartEndDateDiff(i, this.empFormData.Education[i].TimePeriodTo , 'to');
      this.empFormData.Education[i].Degree = parseInt(this.empFormData.Education[i].Degree);
    }
    for(var i = 0; i< this.empFormData.TrainingCertifications.length; i++) {
      if(this.empFormData.TrainingCertifications[i].EndDate != null){
        this.empFormData.TrainingCertifications[i].EndDate = new Date(this.empFormData.TrainingCertifications[i].EndDate);
      }
      if(this.empFormData.TrainingCertifications[i].StartDate != null){
        this.empFormData.TrainingCertifications[i].StartDate = new Date(this.empFormData.TrainingCertifications[i].StartDate);
      }
      this.checkTrainingStartEndDateDiff(i, this.empFormData.TrainingCertifications[i].EndDate , 'to');
    }
    for(var i = 0; i< this.empFormData.WorkExperience.length; i++) {
      if(this.empFormData.WorkExperience[i].TimePeriodFrom != null){
        this.empFormData.WorkExperience[i].TimePeriodFrom = new Date(this.empFormData.WorkExperience[i].TimePeriodFrom);
      }
      if(this.empFormData.WorkExperience[i].TimePeriodTo != null){
        this.empFormData.WorkExperience[i].TimePeriodTo = new Date(this.empFormData.WorkExperience[i].TimePeriodTo);
      }
      this.checkWorkStartEndDateDiff(i, this.empFormData.WorkExperience[i].TimePeriodTo , 'to');
    }
    if(this.empFormData.SignaturePhotoID && this.empFormData.SignaturePhoto){
      this.signatureAttachments[0] = {AttachmentGuid:this.empFormData.SignaturePhotoID, AttachmentsName:this.empFormData.SignaturePhoto};
    }
    this.validateForm();
    // this.validateEduction();
    // this.validateWork();
    // this.validateTraining();
  }

  prepareData() {
    this.empFormModel.BirthDate = new Date(this.empFormData.BirthDate);
    this.empFormModel.EmiratesIdIssueDate = new Date(this.empFormData.EmiratesIdIssueDate);
    return this.empFormModel;
  }

  getNationality(){
    this.service.getDetails('Nationality', this.currentUser.id).subscribe(data => {
      this.nationalityList = data;
      this.loadEducations();
    });
  }

  getReligion(){
    this.service.getDetails('Religion', this.currentUser.id).subscribe(data => {
      this.religionList = data;
      this.getNationality();
    });
  }

  getCountry(){
    this.service.getDetails('Country', this.currentUser.id).subscribe(data => {
      this.countryList = data;
      this.getReligion();
    });
  }

  getEmpStatus() {
    this.dropdownService.getEmpStatus(this.currentUser.id)
      .subscribe((employeeStatus:any) => {
        if (employeeStatus != null) {
          this.employeeStatus = employeeStatus;
        }
        this.getDepartments();
      });
  }

  loadEducations() {
    this.dropdownService.getEducations(this.currentUser.id)
      .subscribe((educations:any) => {
        if (educations != null) {
          this.qualificationList = educations;
        }
        this.getEmpStatus();
      });
  }

  addRow(value) {
    if(value == "education") {
      this.empFormData.Education.push(this.createItem());
    }else if(value == "certificate"){
      this.empFormData.TrainingCertifications.push(this.createCertificate());
    } else {
      this.empFormData.WorkExperience.push(this.createWork());
    }
  }

  createItem() {
    return {
      UserProfileEducationID:0,
      Degree : "",
      SchoolCollege: "",
      FieldStudy: "",
      TimePeriodFrom: null,
      TimePeriodTo: null,
      isValidDate: false
    }
  }

  createCertificate() {
    return {
      TrainingName: "",
      StartDate: null,
      EndDate: null,
      TrainingID: "",
      isValidDate: false,
      Attachment:[]
    };
  }

  createWork() {
    return {
      JobTitle : "",
      Company: "",
      City: "",
      TimePeriodFrom: null,
      TimePeriodTo: null,
      WorkExperienceID: 0,
      isValidDate: false
    }
  }

  CalculateAge(event) {
    if(event !== null && event){
      var timeDiff = Math.abs(Date.now() - event);
      this.empFormData.Age = Math.floor((timeDiff / (1000 * 3600 * 24))/365);
    }
  }
  checkValidation() {
    // if(this.checkEmpName()) {
    //   this.errorEmpNameMsg = true;
    // }else {
    //   this.errorEmpNameMsg = false;
    // }
    if(this.checkEmpCode()) {
      this.errorEmpCodeMsg = true;
    }else {
      this.errorEmpCodeMsg = false;
    }

    // if (this.enterOffclMail()) {
    //   this.enterOfflMail = true;
    //   return;
    // } else {
    //   this.enterOfflMail = false;
    // }
    if(this.checkOffMail()) {
      this.errorEmpOffMail = true;
    }else {
      this.errorEmpOffMail = false;
    }


    if (this.checkValidEmail()) {
      this.validEmail = true;
    } else {
      this.validEmail = false;
    }
    // if(this.checkPersMail()) {
    //   this.errorEmpPerMail = true;
    // }else {
    //   this.errorEmpPerMail = false;
    // }



    if(this.checkEmpGender()) {
      this.errorEmpGenderMsg = true;
    }else {
      this.errorEmpGenderMsg = false;
    }
    if(this.checkEmpbirthday()) {
      this.errorEmpbirthdayMsg = true;
    }else {
      this.errorEmpbirthdayMsg = false;
    }
    if(this.checkEmpAge()) {
      this.errorEmpAgeMsg = true;
    }else {
      this.errorEmpAgeMsg = false;
    }
    // if(this.checkEmpCountry()) {
    //   this.errorEmpCountryMsg = true;
    // }else {
    //   this.errorEmpCountryMsg = false;
    // }
    // if(this.checkEmpMobile()) {
    //   this.errorEmpMobileMsg = true;
    // }else {
    //   this.errorEmpMobileMsg = false;
    // }
    // if(this.checkEmpPhone()) {
    //   this.errorEmpPhoneMsg = true;
    // }else {
    //   this.errorEmpPhoneMsg = false;
    // }
    // if(this.checkEmpReligion()) {
    //   this.errorEmpReligionMsg = true;
    // }else {
    //   this.errorEmpReligionMsg = false;
    // }
    // if(this.checkEmpNationality()) {
    //   this.errorEmpNationalityMsg = true;
    // }else {
    //   this.errorEmpNationalityMsg = false;
    // }
    // if(this.checkPreNationality()) {
    //   this.errorEmpPreNationalityMsg = true;
    // }else {
    //   this.errorEmpPreNationalityMsg = false;
    // }
    if(this.checkEmpJoinDate()) {
      this.errorEmpJoinDateMsg = true;
    }else {
      this.errorEmpJoinDateMsg = false;
    }
    // if(this.checkEmpTitle()) {
    //   this.errorEmpTitleMsg = true;
    // }else {
    //   this.errorEmpTitleMsg = false;
    // }
    if(this.checkEmpGrade()) {
      this.errorEmpGradeMsg = true;
    }else {
      this.errorEmpGradeMsg = false;
    }
    if(this.checkOrg()) {
      this.errorEmpOrgMsg = true;
    }else {
      this.errorEmpOrgMsg = false;
    }
    if(this.checkEmpPosition()) {
      this.errorEmpPositionMsg = true;
    }else {
      this.errorEmpPositionMsg = false;
    }
    // if(this.checkEmpStatus()) {
    //   this.errorEmpStatusMsg = true;
    // }else {
    //   this.errorEmpStatusMsg = false;
    // }
    // if(this.checkResigned()) {
    //   this.errorEmpResignedMsg = true;
    // }else {
    //   this.errorEmpResignedMsg = false;
    // }
    if(this.checkResignedDate()) {
      this.errorEmpResignedDateMsg = true;
    }else {
      this.errorEmpResignedDateMsg = false;
    }
    if(this.checkBalLeave()) {
      this.errorEmpBalLeaveMsg = true;
    }else {
      this.errorEmpBalLeaveMsg = false;
    }
    // if(this.checkNotification()) {
    //   this.errorEmpNotifiMsg = true;
    // }else {
    //   this.errorEmpNotifiMsg = false;
    // }
    // if(this.validateEduction()) {
    //   return;
    // }
    // if(this.validateWork()) {
    //   return;
    // }
    // if(this.validateTraining()) {
    //   return;
    // }
    // if(this.checkPassNo()) {
    //   this.errorEmpPassNoMsg = true;
    //   return;
    // }else {
    //   this.errorEmpPassNoMsg = false;
    // }
    // if(this.checkPassIssPlace()) {
    //   this.errorEmpPassIssPlaceMsg = true;
    //   return;
    // }else {
    //   this.errorEmpPassIssPlaceMsg = false;
    // }
    if(this.checkPassIssDate()) {
      this.errorEmpPassIssDateMsg = true;
      return;
    }else {
      this.errorEmpPassIssDateMsg = false;
    }
    if(this.checkPassExDate()) {
      this.errorEmpPassExDateMsg = true;
      return;
    }else {
      this.errorEmpPassExDateMsg = false;
    }
    // if(this.checkVisaNo()) {
    //   this.errorVisaNoMsg = true;
    //   return;
    // }else {
    //   this.errorVisaNoMsg = false;
    // }
    if(this.checkVisaIssue()) {
      this.errorVisaIssMsg = true;
      return;
    }else {
      this.errorVisaIssMsg = false;
    }
    if(this.checkVisaExp()) {
      this.errorVisaExpMsg = true;
      return;
    }else {
      this.errorVisaExpMsg = false;
    }
    // if(this.checkEmiIdNo()) {
    //   this.errorEmpEmiIdNoMsg = true;
    //   return;
    // }else {
    //   this.errorEmpEmiIdNoMsg = false;
    // }
    if(this.checkEmiIssDate()) {
      this.errorEmpEmiIssDateMsg = true;
      return;
    }else {
      this.errorEmpEmiIssDateMsg = false;
    }
    if(this.checkEmiExDate()) {
      this.errorEmpEmiExDateMsg = true;
      return;
    }else {
      this.errorEmpEmiExDateMsg = false;
    }

    // if(this.checkLabConNo()) {
    //   this.errorEmpLabConNoMsg = true;
    //   return;
    // }else {
    //   this.errorEmpLabConNoMsg = false;
    // }
    if(this.checkConIssDate()) {
      this.errorConIssDateMsg = true;
      return;
    }else {
      this.errorConIssDateMsg = false;
    }
    if(this.checkConExDate()) {
      this.errorEmpConExDateMsg = true;
      return;
    }else {
      this.errorEmpConExDateMsg = false;
    }
    // if(this.attachments.length == 0) {
    //   this.errorattachmentsMsg = true;
    //   return;
    // }else {
    //   this.errorattachmentsMsg = false;
    // }
  }


  validateForm() {
    var flag = true;
    let officialMainId = this.empFormData && this.empFormData.OfficialMailId ? this.empFormData.OfficialMailId.trim() : '';
    let personalMailId = this.empFormData && this.empFormData.PersonalMailId ? this.empFormData.PersonalMailId.trim() : '';
    let employeeCode = this.empFormData.EmployeeCode ? this.empFormData.EmployeeCode.trim() : '';




    
    //  (this.screenStatus=='View'||'Edit'&& this.HRid!=9) 
    if (
      // !this.utils.isEmptyString(this.empFormData.EmployeeName.trim()) &&
        // !this.utils.isEmptyString(employeeCode) &&
        this.utils.isEmail(officialMainId) &&
        this.utils.isEmail(personalMailId) &&
        this.empFormData.Gender 
      
        // this.empFormData.CountryofResidence &&
        // this.empFormData.BirthDate &&
        // this.empFormData.MobileNumber &&
        // this.empFormData.EmployeePhoneNumber &&
        // this.empFormData.Religion &&
        // this.empFormData.Nationality &&
        // this.empFormData.PreviousNationality &&
        // this.empFormData.JoinDate &&
        // this.empFormData.Title &&
        // this.empFormData.Grade &&
        // this.empFormData.OrgUnitID &&
        // this.empFormData.EmployeePosition &&
        // this.empFormData.EmploymentStatus &&
        // this.empFormData.Resigned &&
        // this.empFormData.ResignationDate &&
        // this.empFormData.BalanceLeave 
        // (this.empFormData.NotificationPreferencesSMS || this.empFormData.NotificationPreferencesEmail) &&
        // this.empFormData.PassportNumber.trim() &&
        // this.empFormData.PassportIssuePlace &&
        // this.empFormData.PassportIssueDate &&
        // this.empFormData.PassportExpiryDate &&
        // this.empFormData.EmiratesIdNumber.trim() &&
        // this.empFormData.EmiratesIdExpiryDate &&
        // this.empFormData.InsuranceNumber.trim() &&
        // this.empFormData.InsuranceIssueDate &&
        // this.empFormData.InsuranceExpiryDate &&
        // this.empFormData.LabourContractNumber.trim() &&
        // this.empFormData.LabourContractIssueDate &&
        // this.empFormData.LaborContractExpiryDate &&
        // this.empFormData.VisaNumber.trim() &&
        // this.empFormData.Education.length > 0 &&
        // this.empFormData.Age >= 18 &&
        // this.attachments.length>0
        ) {
          flag = false;
        }

    return flag;
  }

  checkEmpName() {
    if(this.utils.isEmptyString(this.empFormData.EmployeeName.trim())) {
      return true;
    }else {
      return false;
    }
  }

  checkEmpCode() {
    if(this.empFormData.EmployeeCode){
      if(this.utils.isEmptyString(this.empFormData.EmployeeCode.trim())) {
        return true;
      }else {
        return false;
      }
    }
  }

  enterOffclMail() {
    if(this.empFormData.OfficialMailId === "" || this.empFormData.OfficialMailId === null) {
      this.enterOfflMail = true;
      return true;
    }
  }

  checkOffMail() {
    if(this.empFormData.OfficialMailId){
      if(!this.utils.isEmail(this.empFormData.OfficialMailId.trim())) {
        this.validOffcMail = true;
        return true;
      }
    }
  }

  checkPersMail() {
    if(this.empFormData.PersonalMailId === "" || this.empFormData.PersonalMailId === null) {
      return true;
    }
  }
  checkValidEmail() {
    if(this.empFormData.PersonalMailId != null){
      if(!this.utils.isEmail(this.empFormData.PersonalMailId.trim())) {
        this.validEmail = true;
        return true;
      }
    }
  }

  checkEmpGender() {
    if(!this.empFormData.Gender) {
      return true;
    }
  }

  checkEmpCountry() {
    if(!this.empFormData.CountryofResidence) {
      return true;
    }
  }
  checkEmpbirthday() {
    if(this.empFormData.BirthDate == 'Invalid Date') {
      return true;
    }
  }
  checkEmpAge() {
    if(this.empFormData.Age < 18) {
      return true;
    }
  }
  checkEmpMobile() {
    if(this.screenStatus != 'edit' && (!this.empFormData.MobileNumber || !this.empFormData.MobileNumber.trim())) {
      return true;
    }
  }
  checkEmpPhone() {
    if(!this.empFormData.EmployeePhoneNumber || !this.empFormData.EmployeePhoneNumber.trim()) {
      return true;
    }
  }
  checkEmpReligion() {
    if(!this.empFormData.Religion) {
      return true;
    }
  }
  checkEmpNationality() {
    if(!this.empFormData.Nationality) {
      return true;
    }
  }
  checkPreNationality() {
    if(!this.empFormData.PreviousNationality) {
      return true;
    }
  }
  checkEmpJoinDate() {
    if(this.empFormData.JoinDate == 'Invalid Date') {
      return true;
    }
  }
  checkEmpTitle() {
    if(!this.empFormData.Title) {
      return true;
    }
  }
  checkEmpGrade() {
    if(this.screenStatus=='edit' && this.HRid!=9){
      return false
    }else{
      let gradeVal = this.GradeList.indexOf(this.empFormData.Grade);
      if(!this.empFormData.Grade) {
      return true;
    }
    }
    
  }
  checkOrg() {
    if(!this.empFormData.OrgUnitID) {
      return true;
    }
  }
  checkEmpPosition() {
 
    if(this.screenStatus=='edit' && this.HRid!=9){
      return false}else{
        if(!this.empFormData.EmployeePosition || !this.empFormData.EmployeePosition.trim()) {
          return true;
        }
      }
 
  }
  checkEmpStatus() {
    if(!this.empFormData.EmploymentStatus) {
      return true;
    }
  }

  checkResigned() {
    if(!this.empFormData.Resigned) {
      return true;
    }
  }
  checkResignedDate() {
    if(this.empFormData.ResignationDate == 'Invalid Date') {
      return true;
    }
  }
  checkBalLeave() {

    if(this.screenStatus=='edit'&& this.HRid!=9){
      return false
    }else{
      if(this.empFormData.BalanceLeave === null || !this.empFormData.BalanceLeave || this.empFormData.BalanceLeave == 0) {
        return true;
      }
    }


   
  }
  checkNotification() {
    if((!this.empFormData.NotificationPreferencesSMS && !this.empFormData.NotificationPreferencesEmail)) {
      return true;
    }
  }

  checkPassNo() {
    var flag = true;
    if(!this.empFormData.PassportNumber || !this.empFormData.PassportNumber.trim()) {
      flag = true;
    }else{
      flag = false;
    }
    return flag;
  }
  checkPassIssPlace() {
    if(!this.empFormData.PassportIssuePlace || !this.empFormData.PassportIssuePlace.trim()) {
      return true;
    }
  }
  checkPassIssDate() {
    if(this.empFormData.PassportIssueDate == 'Invalid Date') {
      return true;
    }
  }
  checkPassExDate() {
    if(this.empFormData.PassportExpiryDate == 'Invalid Date') {
      return true;
    }
  }
  checkEmiIdNo() {
    if(!this.empFormData.EmiratesIdNumber || !this.empFormData.EmiratesIdNumber.trim()) {
      return true;
    }
    return false;
  }
  checkEmiExDate() {
    if(this.empFormData.EmiratesIdExpiryDate == 'Invalid Date') {
      return true;
    }
  }
  checkEmiIssDate() {
    if(this.empFormData.EmiratesIdIssueDate == 'Invalid Date') {
      return true;
    }
  }
  checkLabConNo() {
    if(!this.empFormData.LabourContractNumber || !this.empFormData.LabourContractNumber.trim()) {
      return true;
    }
    return false;
  }
  checkConIssDate() {
    if(this.empFormData.LabourContractIssueDate == 'Invalid Date') {
      return true;
    }
  }
  checkConExDate() {
    if(this.empFormData.LaborContractExpiryDate == 'Invalid Date') {
      return true;
    }
  }

  checkVisaNo() {
    if(!this.empFormData.VisaNumber || !this.empFormData.VisaNumber.trim()) {
      return true;
    }
    return false;
  }
  checkVisaExp() {
    if(this.empFormData.VisaExpiryDate == 'Invalid Date') {
      return true;
    }
  }
  checkVisaIssue() {
    if(this.empFormData.VisaIssueDate == 'Invalid Date') {
      return true;
    }
  }
  checkUpdateValidation() {
    let valid = true;
    for (let key in this.updateValidation) {
      if (!this.updateValidation[key]) {
        valid = false;
      }
    }
    return valid;
  }

  maxDate(date?: any) {
    return new Date();
  }

  save() {
    debugger
    this.submitted = true;
    // var requestData = this.prepareData();
    // requestData.Action = 'submit';
    this.empFormData.Action = "submit";
    // console.log("this.checkEducation()", this.validateEduction());
    // console.log("this.validateWork()", this.validateWork());
    // console.log("this.checkPassNo()", this.checkPassNo());

    // if(this.checkEmpName()) {
    //   this.errorEmpNameMsg = true;
    //   return;
    // }else {
    //   this.errorEmpNameMsg = false;
    // }
    if(this.checkEmpCode()) {
      this.errorEmpCodeMsg = true;
      return;
    }else {
      this.errorEmpCodeMsg = false;
    }
    if(this.checkOffMail()) {
      this.errorEmpOffMail = true;
      return;
    }else {
      this.errorEmpOffMail = false;
    }
    // if (this.enterOffclMail()) {
    //   this.enterOfflMail = true;
    //   return;
    // } else {
    //   this.enterOfflMail = false;
    // }

    if (this.checkValidEmail()) {
      this.validEmail = true;
    } else {
      this.validEmail = false;
    }
    // if(this.checkPersMail()) {
    //   this.errorEmpPerMail = true;
    //   return;
    // }else {
    //   this.errorEmpPerMail = false;
    // }
    // if(this.checkEmpGender()) {
    //   this.errorEmpGenderMsg = true;
    //   return;
    // }else {
    //   this.errorEmpGenderMsg = false;
    // }
    // if(this.checkEmpbirthday()) {
    //   this.errorEmpbirthdayMsg = true;
    //   return;
    // }else {
    //   this.errorEmpbirthdayMsg = false;
    // }
    if(this.checkEmpAge()) {
      this.errorEmpAgeMsg = true;
      return;
    }else {
      this.errorEmpAgeMsg = false;
    }
    // if(this.checkEmpCountry()) {
    //   this.errorEmpCountryMsg = true;
    //   return;
    // }else {
    //   this.errorEmpCountryMsg = false;
    // }
    // if(this.checkEmpMobile()) {
    //   this.errorEmpMobileMsg = true;
    //   return;
    // }else {
    //   this.errorEmpMobileMsg = false;
    // }
    // if(this.checkEmpPhone()) {
    //   this.errorEmpPhoneMsg = true;
    //   return;
    // }else {
    //   this.errorEmpPhoneMsg = false;
    // }
    // if(this.checkEmpReligion()) {
    //   this.errorEmpReligionMsg = true;
    //   return;
    // }else {
    //   this.errorEmpReligionMsg = false;
    // }
    // if(this.checkEmpNationality()) {
    //   this.errorEmpNationalityMsg = true;
    //   return;
    // }else {
    //   this.errorEmpNationalityMsg = false;
    // }
    // if(this.checkPreNationality()) {
    //   this.errorEmpPreNationalityMsg = true;
    //   return;
    // }else {
    //   this.errorEmpPreNationalityMsg = false;
    // }
    if(this.checkEmpJoinDate()) {
      this.errorEmpJoinDateMsg = true;
      return;
    }else {
      this.errorEmpJoinDateMsg = false;
    }
    // if(this.checkEmpTitle()) {
    //   this.errorEmpTitleMsg = true;
    //   return;
    // }else {
    //   this.errorEmpTitleMsg = false;
    // }
    if(this.checkEmpGrade()) {
     
      this.errorEmpGradeMsg = true;
      return;
    }else {
      this.errorEmpGradeMsg = false;
    }
    if(this.checkOrg()) {
      this.errorEmpOrgMsg = true;
      return;
    }else {
      this.errorEmpOrgMsg = false;
    }
    if(this.checkEmpPosition()) {
      this.errorEmpPositionMsg = true;
      return;
    }else {
      this.errorEmpPositionMsg = false;
    }
    // if(this.checkEmpStatus()) {
    //   this.errorEmpStatusMsg = true;
    //   return;
    // }else {
    //   this.errorEmpStatusMsg = false;
    // }
    // if(this.checkResigned()) {
    //   this.errorEmpResignedMsg = true;
    //   return;
    // }else {
    //   this.errorEmpResignedMsg = false;
    // }
    if(this.checkResignedDate()) {
      this.errorEmpResignedDateMsg = true;
      return;
    }else {
      this.errorEmpResignedDateMsg = false;
    }
    if(this.checkBalLeave()) {
      this.errorEmpBalLeaveMsg = true;
      return;
    }else {
      this.errorEmpBalLeaveMsg = false;
    }
    // if(this.checkNotification()) {
    //   this.errorEmpNotifiMsg = true;
    //   return;
    // }else {
    //   this.errorEmpNotifiMsg = false;
    // }
    // if(this.validateEduction()) {
    //   return;
    // }
    // if(this.validateWork()) {
    //   return;
    // }
    // if(this.validateTraining()) {
    //   return;
    // }
    // if(this.checkPassNo()) {
    //   // console.log("this.checkPassNo()", this.checkPassNo());
    //   this.errorEmpPassNoMsg = true;
    //   return;
    // }else {
    //   this.errorEmpPassNoMsg = false;
    // }
    // if(this.checkPassIssPlace()) {
    //   this.errorEmpPassIssPlaceMsg = true;
    //   return;
    // }else {
    //   this.errorEmpPassIssPlaceMsg = false;
    // }
    if(this.checkPassIssDate()) {
      this.errorEmpPassIssDateMsg = true;
      return;
    }else {
      this.errorEmpPassIssDateMsg = false;
    }
    if(this.checkPassExDate()) {
      this.errorEmpPassExDateMsg = true;
      return;
    }else {
      this.errorEmpPassExDateMsg = false;
    }
    // if(this.checkVisaNo()) {
    //   this.errorVisaNoMsg = true;
    //   return;
    // }else {
    //   this.errorVisaNoMsg = false;
    // }
    if(this.checkVisaIssue()) {
      this.errorVisaIssMsg = true;
      return;
    }else {
      this.errorVisaIssMsg = false;
    }
    if(this.checkVisaExp()) {
      this.errorVisaExpMsg = true;
      return;
    }else {
      this.errorVisaExpMsg = false;
    }
    // if(this.checkEmiIdNo()) {
    //   this.errorEmpEmiIdNoMsg = true;
    //   return;
    // }else {
    //   this.errorEmpEmiIdNoMsg = false;
    // }
    if(this.checkEmiIssDate()) {
      this.errorEmpEmiIssDateMsg = true;
      return;
    }else {
      this.errorEmpEmiIssDateMsg = false;
    }
    if(this.checkEmiExDate()) {
      this.errorEmpEmiExDateMsg = true;
      return;
    }else {
      this.errorEmpEmiExDateMsg = false;
    }

    // if(this.checkLabConNo()) {
    //   this.errorEmpLabConNoMsg = true;
    //   return;
    // }else {
    //   this.errorEmpLabConNoMsg = false;
    // }
    if(this.checkConIssDate()) {
      this.errorConIssDateMsg = true;
      return;
    }else {
      this.errorConIssDateMsg = false;
    }
    if(this.checkConExDate()) {
      this.errorEmpConExDateMsg = true;
      return;
    }else {
      this.errorEmpConExDateMsg = false;
    }
    // if(this.attachments.length == 0) {
    //   this.errorattachmentsMsg = true;
    //   return;
    // }else {
    //   this.errorattachmentsMsg = false;
    // }
    // this.empFormData.Title=this.titleList.indexOf(this.empFormData.Title);
    let gradeVal = this.GradeList.indexOf(this.empFormData.Grade);
    // this.empFormData.Grade = gradeVal == -1 ? '' : gradeVal;

    this.empFormData.Nationality=this.Nationlaity.find(x=> x.NationalityName == this.empFormData.PreviousNationality).NationalityID;
    this.empFormData.EmploymentStatus = this.employeeStatus.find(x=> x.EmployeeStatusName == this.empFormData.EmploymentStatus).EmployeeStatusID;
    this.service.saveEmpData(this.empFormData).subscribe(data => {
      this.submitted = false;
      this.message = "Employee Profile Submitted Successfully";
      this.modalService.show(this.template);
    });
  }

  clearAttachment() {
    this.myInputVariable.nativeElement.value = "";
  }

  uploadFiles(event, fileType) {
    if(fileType == 'file') {
      if(!this.AttachmentType) {
        this.errorAttchmentTypeMsg = true;
        return;
      }else {
        this.errorAttchmentTypeMsg = false;
      }
    }
    var files = event.target.files;
    let that = this;
    this.uploadProcess = true;
    this.common.postAttachment(files).subscribe((event: any) => {
      if (event.type === HttpEventType.UploadProgress) {
        this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
      } else if (event.type === HttpEventType.Response) {
        this.uploadProcess = false;
        this.uploadPercentage = 0;
        if(fileType == 'file') {
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.attachments.push({ 'AttachmentType': this.AttachmentType, 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'UserProfileId': 0 });
          }
          this.empFormData.Attachment = this.attachments;
          this.AttachmentType = null;
          this.errorattachmentsMsg = false;
        }else {
          this.photoattachments = [];
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.photoattachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'UserProfileId': 0 });
          }
          this.empFormData.ProfilePhotoID = this.photoattachments[0].AttachmentGuid;
          this.empFormData.ProfilePhotoName = this.photoattachments[0].AttachmentsName;
          this.ImageUrl =  this.endpoint.fileDownloadUrl+"?filename="+this.empFormData.ProfilePhotoName+"&guid="+this.empFormData.ProfilePhotoID;
        }
      }
    },(error)=>{
      this.uploadProcess = false;
    });
    this.myInputVariable.nativeElement.value = "";
  }

  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    if (this.attachments.length == 0) {
      this.myInputVariable.nativeElement.value = "";
    }
  }

  update() {
    this.submitted = true;
    // if(this.checkEmpName()) {
    //   this.errorEmpNameMsg = true;
    //   this.updateValidation.errorEmpNameMsg = false;
    // }else {
    //   this.errorEmpNameMsg = false;
    //   this.updateValidation.errorEmpNameMsg = true;
    // }
    if(this.checkEmpCode()) {
      this.errorEmpCodeMsg = true;
      this.updateValidation.errorEmpCodeMsg = false;
    }else {
      this.errorEmpCodeMsg = false;
      this.updateValidation.errorEmpCodeMsg = true;
    }
    /*if(this.checkOffMail()) {
      this.errorEmpOffMail = true;
      this.updateValidation.errorEmpOffMail = false;
    }else {
      this.errorEmpOffMail = false;
      this.updateValidation.errorEmpOffMail = true;
    }*/
    if (this.checkValidEmail()) {
      this.validEmail = true;
      this.updateValidation.validEmail = false;
    } else {
      this.validEmail = false;
      this.updateValidation.validEmail = true;
    }
    // if(this.checkPersMail()) {
    //   this.errorEmpPerMail = true;
    //   this.updateValidation.errorEmpPerMail = false;
    // }else {
    //   this.errorEmpPerMail = false;
    //   this.updateValidation.errorEmpPerMail = true;
    // }
    // if(this.checkEmpGender()) {
    //   this.errorEmpGenderMsg = true;
    //   this.updateValidation.errorEmpGenderMsg = false;
    // }else {
    //   this.errorEmpGenderMsg = false;
    //   this.updateValidation.errorEmpGenderMsg = true;
    // }
    // if(this.checkEmpbirthday()) {
    //   this.errorEmpbirthdayMsg = true;
    //   this.updateValidation.errorEmpbirthdayMsg = false;
    // }else {
    //   this.errorEmpbirthdayMsg = false;
    //   this.updateValidation.errorEmpbirthdayMsg  = true;
    // }
    if(this.checkEmpAge()) {
      this.errorEmpAgeMsg = true;
      this.updateValidation.errorEmpAgeMsg = false;
    }else {
      this.errorEmpAgeMsg = false;
      this.updateValidation.errorEmpAgeMsg = true;
    }
    // if(this.checkEmpCountry()) {
    //   this.errorEmpCountryMsg = true;
    //   this.updateValidation.errorEmpCountryMsg = false;
    // }else {
    //   this.errorEmpCountryMsg = false;
    //   this.updateValidation.errorEmpCountryMsg = true;
    // }
    // if(this.checkEmpMobile()) {
    //   this.errorEmpMobileMsg = true;
    //   this.updateValidation.errorEmpMobileMsg = false;
    // }else {
    //   this.errorEmpMobileMsg = false;
    //   this.updateValidation.errorEmpMobileMsg = true;
    // }
    // if(this.checkEmpPhone()) {
    //   this.errorEmpPhoneMsg = true;
    //   this.updateValidation.errorEmpPhoneMsg = false;
    // }else {
    //   this.errorEmpPhoneMsg = false;
    //   this.updateValidation.errorEmpPhoneMsg = true;
    // }
    // if(this.checkEmpReligion()) {
    //   this.errorEmpReligionMsg = true;
    //   this.updateValidation.errorEmpReligionMsg = false;
    // }else {
    //   this.errorEmpReligionMsg = false;
    //   this.updateValidation.errorEmpReligionMsg = true;
    // }
    // if(this.checkEmpNationality()) {
    //   this.errorEmpNationalityMsg = true;
    //   this.updateValidation.errorEmpNationalityMsg = false;
    // }else {
    //   this.errorEmpNationalityMsg = false;
    //   this.updateValidation.errorEmpNationalityMsg = true;
    // }
    // if(this.checkPreNationality()) {
    //   this.errorEmpPreNationalityMsg = true;
    //   this.updateValidation.errorEmpPreNationalityMsg = false;
    // }else {
    //   this.errorEmpPreNationalityMsg = false;
    //   this.updateValidation.errorEmpPreNationalityMsg = true;
    // }
    if(this.checkEmpJoinDate()) {
      this.errorEmpJoinDateMsg = true;
      this.updateValidation.errorEmpJoinDateMsg = false;
    }else {
      this.errorEmpJoinDateMsg = false;
      this.updateValidation.errorEmpJoinDateMsg = true;
    }
    // if(this.checkEmpTitle()) {
    //   this.errorEmpTitleMsg = true;
    //   this.updateValidation.errorEmpTitleMsg = false;
    // }else {
    //   this.errorEmpTitleMsg = false;
    //   this.updateValidation.errorEmpTitleMsg = true;
    // }
    if(this.checkEmpGrade()) {
      this.errorEmpGradeMsg = true;
      this.updateValidation.errorEmpGradeMsg = false;
    }else {
      this.errorEmpGradeMsg = false;
      this.updateValidation.errorEmpGradeMsg = true;
    }
    if(this.checkOrg()) {
      this.errorEmpOrgMsg = true;
      this.updateValidation.errorEmpOrgMsg = false;
    }else {
      this.errorEmpOrgMsg = false;
      this.updateValidation.errorEmpOrgMsg = true;
    }
    // if(this.checkEmpPosition()) {
    //   this.errorEmpPositionMsg = true;
    //   this.updateValidation.errorEmpPositionMsg = false;
    // }else {
    //   this.errorEmpPositionMsg = false;
    //   this.updateValidation.errorEmpPositionMsg = true;
    // }
    // if(this.checkEmpStatus()) {
    //   this.errorEmpStatusMsg = true;
    //   this.updateValidation.errorEmpStatusMsg = false;
    // }else {
    //   this.errorEmpStatusMsg = false;
    //   this.updateValidation.errorEmpStatusMsg = true;
    // }
    // if(this.checkResigned()) {
    //   this.errorEmpResignedMsg = true;
    //   this.updateValidation.errorEmpResignedMsg = false;
    // }else {
    //   this.errorEmpResignedMsg = false;
    //   this.updateValidation.errorEmpResignedMsg = true;
    // }
    if(this.checkResignedDate()) {
      this.errorEmpResignedDateMsg = true;
      this.updateValidation.errorEmpResignedDateMsg = false;
    }else {
      this.errorEmpResignedDateMsg = false;
      this.updateValidation.errorEmpResignedDateMsg = true;
    }
    if(this.checkBalLeave()) {
      this.errorEmpBalLeaveMsg = true;
      this.updateValidation.errorEmpBalLeaveMsg = false;
    }else {
      this.errorEmpBalLeaveMsg = false;
      this.updateValidation.errorEmpBalLeaveMsg = true;
    }
    // if(this.checkNotification()) {
    //   this.errorEmpNotifiMsg = true;
    //   this.updateValidation.errorEmpNotifiMsg = false;
    // }else {
    //   this.errorEmpNotifiMsg = false;
    //   this.updateValidation.errorEmpNotifiMsg = true;
    // }
    // if(this.validateEduction()) {
    //   this.updateValidation.validateEduction = false;
    // } else {
    //   this.updateValidation.validateEduction = true;
    // }
    // if(this.validateWork()) {
    //   this.updateValidation.validateWork = false;
    // } else {
    //   this.updateValidation.validateWork = true;
    // }
    // if(this.validateTraining()) {
    //   this.updateValidation.validateTraining = false;
    // } else {
    //   this.updateValidation.validateTraining = true;
    // }
    // if(this.checkPassNo()) {
    //   this.errorEmpPassNoMsg = true;
    //   this.updateValidation.errorEmpPassNoMsg = false;
    // }else {
    //   this.errorEmpPassNoMsg = false;
    //   this.updateValidation.errorEmpPassNoMsg = true;
    // }
    // if(this.checkPassIssPlace()) {
    //   this.errorEmpPassIssPlaceMsg = true;
    //   this.updateValidation.errorEmpPassIssPlaceMsg = false;
    // }else {
    //   this.errorEmpPassIssPlaceMsg = false;
    //   this.updateValidation.errorEmpPassIssPlaceMsg = true;
    // }
    if(this.checkPassIssDate()) {
      this.errorEmpPassIssDateMsg = true;
      this.updateValidation.errorEmpPassIssDateMsg = false;
    }else {
      this.errorEmpPassIssDateMsg = false;
      this.updateValidation.errorEmpPassIssDateMsg = true;
    }
    if(this.checkPassExDate()) {
      this.errorEmpPassExDateMsg = true;
      this.updateValidation.errorEmpPassExDateMsg = false;
    }else {
      this.errorEmpPassExDateMsg = false;
      this.updateValidation.errorEmpPassExDateMsg = true;
    }
    // if(this.checkVisaNo()) {
    //   this.errorVisaNoMsg = true;
    //   this.updateValidation.errorVisaNoMsg = false;
    // }else {
    //   this.errorVisaNoMsg = false;
    //   this.updateValidation.errorVisaNoMsg = true;
    // }
    if(this.checkVisaIssue()) {
      this.errorVisaIssMsg = true;
      this.updateValidation.errorVisaIssMsg = false;
    }else {
      this.errorVisaIssMsg = false;
      this.updateValidation.errorVisaIssMsg = true;
    }
    if(this.checkVisaExp()) {
      this.errorVisaExpMsg = true;
      this.updateValidation.errorVisaExpMsg = false;
    }else {
      this.errorVisaExpMsg = false;
      this.updateValidation.errorVisaExpMsg = true;
    }
    // if(this.checkEmiIdNo()) {
    //   this.errorEmpEmiIdNoMsg = true;
    //   this.updateValidation.errorEmpEmiIdNoMsg = false;
    // }else {
    //   this.errorEmpEmiIdNoMsg = false;
    //   this.updateValidation.errorEmpEmiIdNoMsg = true;
    // }
    if(this.checkEmiIssDate()) {
      this.errorEmpEmiIssDateMsg = true;
      this.updateValidation.errorEmpEmiIssDateMsg = false;
    }else {
      this.errorEmpEmiIssDateMsg = false;
      this.updateValidation.errorEmpEmiIssDateMsg = true;
    }
    if(this.checkEmiExDate()) {
      this.errorEmpEmiExDateMsg = true;
      this.updateValidation.errorEmpEmiExDateMsg = false;
    }else {
      this.errorEmpEmiExDateMsg = false;
      this.updateValidation.errorEmpEmiExDateMsg = true;
    }
    // if(this.checkLabConNo()) {
    //   this.errorEmpLabConNoMsg = true;
    //   this.updateValidation.errorEmpLabConNoMsg = false;
    // }else {
    //   this.errorEmpLabConNoMsg = false;
    //   this.updateValidation.errorEmpLabConNoMsg = true;
    // }
    if(this.checkConIssDate()) {
      this.errorConIssDateMsg = true;
      this.updateValidation.errorConIssDateMsg = false;
    }else {
      this.errorConIssDateMsg = false;
      this.updateValidation.errorConIssDateMsg = true;
    }
    if(this.checkConExDate()) {
      this.errorEmpConExDateMsg = true;
      this.updateValidation.errorEmpConExDateMsg = false;
    }else {
      this.errorEmpConExDateMsg = false;
      this.updateValidation.errorEmpConExDateMsg = true;
    }
    // if(this.attachments.length == 0) {
    //   this.errorattachmentsMsg = true;
    //   this.updateValidation.errorattachmentsMsg = false;
    // }else {
    //   this.errorattachmentsMsg = false;
    //   this.updateValidation.errorattachmentsMsg = true;
    // }

    // if(!this.AttachmentType && this.attachments.length == 0) {
    //   this.errorAttchmentTypeMsg = true;
    //   this.updateValidation.errorAttchmentTypeMsg = false;
    // }else {
    //   this.errorAttchmentTypeMsg = false;
    //   this.updateValidation.errorAttchmentTypeMsg = true;
    // }
    // if (this.signatureAttachments.length == 0) {
    //   this.signatureErrorattachmentsMsg = true;
    //   this.updateValidation.signatureErrorattachmentsMsg  = false;
    // } else {
    //   this.signatureErrorattachmentsMsg = false;
    //   this.updateValidation.signatureErrorattachmentsMsg  = true;
    // }

    this.empFormData.UserProfileId = this.employeeId;
    // let titleIndex = this.titleList.indexOf(this.empFormData.Title);
    let gradeIndex = this.GradeList.indexOf(this.empFormData.Grade);
    // this.empFormData.Title = this.titleList[titleIndex];
    // this.empFormData.Grade = gradeIndex == -1 ? '' : this.GradeList[gradeIndex];

    // var TempNationality=this.Nationlaity.find(x=> x.NationalityName == this.empFormData.Nationality);
    // this.empFormData.Nationality = TempNationality.NationalityID;
    // this.empFormData.PreviousNationality=this.Nationlaity.find(x=> x.NationalityName == this.empFormData.PreviousNationality).NationalityID;
    let empStatusId = this.employeeStatus.find(x=> x.EmployeeStatusID == this.empFormData.EmploymentStatus);
    this.empFormData.EmploymentStatus = empStatusId ? empStatusId.EmployeeStatusID : '';
    if (this.checkUpdateValidation()) {
      this.service.updateEmpData(this.empFormData, this.currentUser.id).subscribe(data => {
        this.submitted = false;
        this.message = "Employee Profile Update Successfully";
        if(this.common.currentLang == 'ar'){
          this.message = this.arabicfn('employeeprofileupdatemsg');
        }
        this.modalService.show(this.template);
        this.loadData(this.employeeId);
      });
    }

  }

  closemodal(){
    debugger
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    if(this.editPageSource == 'list'){
    this.router.navigate(['app/hr/employee/list']);
    }else{

    }
  }

  onSelectFile(event) {

  }

  getDepartments() {
    this.service.getEmpDetails(0, this.currentUser.id)
    .subscribe((response:any) => {
      if (response != null) {
        this.departmentList = response.OrganizationList;
      }
    });
  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    var target = event.target ? event.target : event.srcElement;
    if(target.value.length == 0 && charCode == 48) {
        return false;
    }
    return true;
  }

  NotificationPreferencesEmail() {
    if(this.empFormData.NotificationPreferencesEmail) {
      this.empFormData.NotificationPreferencesEmail = "Email";
    }else {
      this.empFormData.NotificationPreferencesEmail = "";
    }
  }

   NotificationPreferencesSMS() {
    if(this.empFormData.NotificationPreferencesSMS) {
      this.empFormData.NotificationPreferencesSMS = "SMS";
    }else {
      this.empFormData.NotificationPreferencesSMS = "";
    }
  }

  uploadSignatureFiles(event) {
    // if(fileType == 'file') {
    //   if(!this.AttachmentType) {
    //     this.errorAttchmentTypeMsg = true;
    //     return;
    //   }else {
    //     this.errorAttchmentTypeMsg = false;
    //   }
    // }
    var files = event.target.files;
    let that = this;
    this.signatureUploadProcess = true;
    this.common.postAttachment(files).subscribe((event: any) => {
      if (event.type === HttpEventType.UploadProgress) {
        this.signatureUploadPercentage = Math.round(event.loaded / event.total) * 100;
      } else if (event.type === HttpEventType.Response) {
        this.signatureUploadProcess = false;
        this.signatureUploadPercentage = 0;
        // if(fileType == 'file') {
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.signatureAttachments[0] = {'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i] };
          }

          this.empFormData.SignaturePhoto = this.signatureAttachments[0].AttachmentsName;
          this.empFormData.SignaturePhotoID = this.signatureAttachments[0].AttachmentGuid;
          this.signatureErrorattachmentsMsg = false;
      }
    },(error)=>{
      this.signatureUploadProcess = false;
    });
    this.mySignVariable.nativeElement.value = "";
  }

  deleteSignatureAttachment(index) {
    this.signatureAttachments.splice(index, 1);
    if (this.signatureAttachments.length == 0) {
      this.mySignVariable.nativeElement.value = "";
      this.empFormData.SignaturePhoto = null;
      this.empFormData.SignaturePhotoID = null;
    }
  }

  arabicfn(word:string){
    word = word.replace(/ +/g, "").toLowerCase();
    return this.common.arabic.words[word];
  }


  TraininghandleFileUpload(event, index) {
    var files = event.target.files;
    if (files.length > 0) {
      this.uploadProcess = true;
      this.common.postAttachment(files).subscribe((event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.empFormData.TrainingCertifications[index].Attachment.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'AttachmentType': 'UserProfileTraining','CreatedBy':this.currentUser.UserProfileId});
          }
          this.myInputVariable.nativeElement.value = "";
          console.log(this.empFormData.TrainingCertifications);
        }
      });
    }
  }

  deleteTrainingAttachment(TrainingIndex,index) {
    this.empFormData.TrainingCertifications[TrainingIndex].Attachment.splice(index, 1);
    if (this.empFormData.TrainingCertifications[TrainingIndex].Attachment.length == 0) {
      this.myInputVariable.nativeElement.value = "";
    }
  }



}
