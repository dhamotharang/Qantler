import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { NewBabyAdditionService } from '../../service/new-baby-addition.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NewBabyAddition } from '../../model/new-baby-addition.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { UploadService } from 'src/app/shared/service/upload.service';
import { HttpEventType } from '@angular/common/http';
import { CommonService } from 'src/app/common.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-new-baby-addition-form',
  templateUrl: './new-baby-addition-form.component.html',
  styleUrls: ['./new-baby-addition-form.component.scss']
})
export class NewBabyAdditionFormComponent implements OnInit {
  @Input() mode: String;
  @Input() requestId: Number;
  sourceOU: string;
  sourceName: string;
  babyName: string;
  gender: string;
  countryList: any = [];
  country: any;
  cityList: any = [];
  city: any;
  hospitalName: string;
  certificate: string;
  babyAdditionDate: any;
  closedDate: any;
  id: string;
  birthday: any;
  date_to: Date;
  screenStatus = 'Create';
  refNo: string;
  comments: string;
  bsModalRef: BsModalRef;
  message: string;
  attachmentDownloadUrl = environment.AttachmentDownloadUrl;
  newBabyAddition: NewBabyAddition = new NewBabyAddition();
  historyLogs: [] = [];
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  status: Number;
  IsOrgHead: boolean = false;
  OrgUnitID = Number;
  isAssigned: boolean = false;
  ifAssignedToMe: boolean = false;
  AssigneeId: any;
  attachments: any = [];
  @ViewChild('variable') myInputVariable: ElementRef;
  uploadProcess: boolean;
  uploadPercentage: number;
  isApiLoading: boolean = false;
  lang: string;
  popupMsg: string;
  editMode: boolean = true;
  othersCountry: boolean = false;

  constructor(
    private newBabyAdditionService: NewBabyAdditionService,
    private route: ActivatedRoute,
    public modalService: BsModalService,
    public dropdownService: DropdownsService,
    public upload: UploadService,
    public router: Router, private common: CommonService,
    public utils: UtilsService) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
    // console.log("lannnn", this.lang);
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.babyAdditionDate = new Date();
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : null;
    this.id = this.route.snapshot.paramMap.get("id");
    this.closedDate = new Date();
    this.route.url.subscribe(() => {
      this.screenStatus = this.route.snapshot.data.title;
    });
    if (this.lang === 'en') {
      this.common.breadscrumChange('HR', 'New Baby Addition', '');
    } else {
      this.common.breadscrumChange(this.arabic('humanresource'), this.arabic('newbabyaddition'), '');
    }
    if (this.mode == 'view') {
      this.editMode = false;
      this.getBabyAddition();
      if (this.lang === 'en') {
        this.common.breadscrumChange('HR', 'New Baby Addition', '');
      } else {
        this.common.breadscrumChange(this.arabic('humanresource'), this.arabic('newbabyaddition'), '');
      }
    } else {
      this.isApiLoading = true;
    }
    this.IsOrgHead = this.currentUser.IsOrgHead;
    this.loadCountries();
  }

  getBabyAddition() {
    this.newBabyAdditionService.getNewBabyAddition(this.id, this.currentUser.id).subscribe((newBabyAddition: any) => {
      // this.sourceOU = newBabyAddition.sourceOU;
      // this.sourceName = newBabyAddition.SourceName;
      this.getSouceName(newBabyAddition.SourceName,newBabyAddition.SourceOU);
      this.babyName = newBabyAddition.BabyName;
      this.birthday = new Date(newBabyAddition.Birthday);
      this.country = parseInt(newBabyAddition.CountryOfBirth);
      this.city = parseInt(newBabyAddition.CityOfBirth);
      if(this.country == 5){
        this.othersCountry = true;
        this.city = newBabyAddition.CityOfBirth;
      }
      this.gender = newBabyAddition.Gender;
      this.hospitalName = newBabyAddition.HospitalName;
      this.certificate = newBabyAddition.Attachment;
      this.historyLogs = newBabyAddition.HistoryLog;
      this.refNo = newBabyAddition.ReferenceNumber;
      this.date_to = new Date(newBabyAddition.CreatedDateTime);
      this.status = newBabyAddition.Status;
      this.AssigneeId = newBabyAddition.AssigneeId;
      this.attachments = newBabyAddition.Attachments;
      this.checkIfAssignedToMe();
      // this.onCountrySelect();
      this.loadCities();
    });
  }

  async getSouceName(UserID,DepID) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;       
       this.sourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });
     this.newBabyAdditionService.getNewBabyAddition(this.id, this.currentUser.id).subscribe((res: any) => {
      this.sourceOU= res.OrganizationList.find(x=> x.OrganizationID == DepID).OrganizationUnits;
    });
    
   }

  checkIfAssignedToMe() {
    if (this.AssigneeId && this.AssigneeId.length > 0) {
      this.isAssigned = true;
      this.AssigneeId.forEach((assignee: any) => {
        if (assignee.AssigneeId == this.currentUser.id) {
          this.ifAssignedToMe = true;
        }
      });
    }
  }

  onSubmit() {
    this.isApiLoading = true;
    this.newBabyAddition.SourceOU = this.currentUser.DepartmentID;
    this.newBabyAddition.SourceName = this.currentUser.UserID;
    this.newBabyAddition.BabyName = this.babyName;
    this.newBabyAddition.Birthday = this.birthday;
    this.newBabyAddition.Gender = this.gender;
    this.newBabyAddition.HospitalName = this.hospitalName;
    this.newBabyAddition.CountryOfBirth = this.country;
    this.newBabyAddition.CityOfBirth = this.city;
    this.newBabyAddition.Action = "Submit";
    this.newBabyAddition.Comments = this.comments;
    this.newBabyAddition.CreatedBy = this.currentUser.id;
    this.newBabyAddition.CreatedDateTime = new Date();
    this.newBabyAddition.Attachments = this.attachments;
    this.newBabyAdditionService.createNewBabyAddition(this.newBabyAddition)
      .subscribe((response: any) => {
        if (response.BabyAdditionID) {
          this.message = "New Baby Addition Request Submitted Successfully";
          if (this.lang == 'ar') {
            this.message = this.arabic('newbabyadditionreqcreatedmsg');
          }
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['app/hr/dashboard']);
          });
        }
      });
  }

  onClose() {
    this.message = "New Baby Addition Closed Successfully";
    if (this.lang == 'ar') {
      this.message = this.arabic('newbabyadditionreqclosedmsg');
    }
    this.updateAction('Close');
  }

  onAssigneTo() {
    let modalTitleString;
    if (this.lang == 'ar') {
      this.popupMsg = this.arabic('newbabyadditionreqassignedmsg');
      modalTitleString = this.arabic('salaryassignto');
    } else {
      this.popupMsg = "New Baby Addition Assigned Successfully";
      modalTitleString = "Assign To";
    }
    const initialState = {
      id: this.id,
      ApiString: "/BabyAddition",
      message: this.popupMsg,
      ApiTitleString: modalTitleString,
      redirectUrl: '/app/hr/dashboard'
    };
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
  }

  onAssigneToMe() {
    this.message = "New Baby Addition Assigned Successfully";
    if (this.lang == 'ar') {
      this.message = this.arabic('newbabyadditionreqassignedmsg');
    }
    this.updateAction('AssignToMe');
  }

  updateAction(action: string) {
    const dataToUpdate = [{
      "value": action,
      "path": "Action",
      "op": "replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "replace"
    }, {
      "value": "",
      "path": "Comments",
      "op": "replace"
    }];
    this.update(dataToUpdate);
  }

  update(dataToUpdate: any) {
    this.isApiLoading = true;
    this.newBabyAdditionService.updateNewBabyAddition(this.id, dataToUpdate)
      .subscribe((response: any) => {
        if (response.BabyAdditionID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['app/hr/dashboard']);
          });
        }
      });
  }

  hisLog(status: string) {
    let sts = status.toLowerCase();
    if(this.common.currentLang != 'ar'){
      if (sts == 'submit') {
        return status + 'ted By';
      } else if (sts == 'reject' || sts == 'redirect') {
        return status + 'ed By';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        return 'Assigned By'
      } else {
        return status + 'd By';
      }
    }else if(this.common.currentLang == 'ar'){
      let arabicStatusStr = '';
      if (sts == 'reject' || sts == 'redirect') {
        arabicStatusStr = sts+'edby';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        arabicStatusStr = 'babyadditionassignedby';
      } else if(sts == 'submit' || sts == 'resubmit'){
        arabicStatusStr = 'salarysubmittedby';
      } else if(sts == 'close'){
        arabicStatusStr = 'salaryclosedby';
      } else {
        arabicStatusStr = sts+'dby';
      }
      return this.common.arabic.words[arabicStatusStr];
    }
  }

  validate() {
    this.isApiLoading = false;
    if (this.utils.isEmptyString(this.babyName)
      || this.utils.isEmptyString(this.gender)
      || this.utils.isEmptyString(this.hospitalName)
      || this.utils.isEmptyString(this.birthday)
      || this.utils.isEmptyString(this.country)
      || this.utils.isEmptyString(this.city) || !this.utils.isValidDate(this.birthday)
    ) {
      this.isApiLoading = true;
    }
  }

  maxDate() {
    return new Date();
  }

  updateDate(selectedDate){
    this.birthday = selectedDate;
    this.validate();
  }

  loadCountries() {
    this.dropdownService.getBabyAdditionCountries(this.currentUser.id, 'NewBabyAddition')
      .subscribe((countries: any) => {
        if (countries != null) {
          this.countryList = countries;
        }
      });
  }

  onCountrySelect(event) {
    this.city = null;
    if(event.CountryID == 5){
      this.othersCountry = true;
    }else{ this.othersCountry = false; }
    this.validate();
    this.loadCities();
  }

  onCountryClear() {
    this.cityList = [];
    this.city = null;
    this.validate();
  }

  loadCities() {
    this.dropdownService.getCities(this.currentUser.id, this.country)
      .subscribe((cities: any) => {
        if (cities != null) {
          this.cityList = cities;
        }
      });
  }

  handleFileUpload(event) {
    var files = event.target.files;
    if (files.length > 0) {
      this.uploadProcess = true;
      this.newBabyAdditionService.uploadAttachment(files).subscribe((event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'BabyAdditionID': '' });
          }
          this.newBabyAddition.Attachments = this.attachments;
          this.myInputVariable.nativeElement.value = "";
        }
      });
    }
  }

  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    if (this.attachments.length == 0) {
      this.myInputVariable.nativeElement.value = "";
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  alphabetsOnly(event): boolean {
    var regex = new RegExp("^[a-zA-Z\r\n]+$");
    // const charCode = (event.which) ? event.which : event.keyCode;
    const charCode = event.key;
    var buff=regex.test(charCode);
    if (!regex.test(charCode)) {
      event.preventDefault();
      return false;
   }
   return true;
  }

  validateForm() {
    debugger
    var flag = true;
    if(this.babyName != "" && this.birthday != null && this.gender != "" && this.hospitalName != "" && this.country != null && this.city != null)
     {
       flag = false;
     }
    return flag;
  }


}
