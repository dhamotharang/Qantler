import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { BsModalRef, BsModalService, BsDatepickerConfig } from 'ngx-bootstrap';
import { VehicleMgmtServiceService } from 'src/app/vehicle-mgmt/service/vehicle-mgmt-service.service';
import { SuccessComponent } from '../success-popup/success.component';
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: 'app-vehicle-rent-car-create-modal',
  templateUrl: './vehicle-rent-car-create-modal.component.html',
  styleUrls: ['./vehicle-rent-car-create-modal.component.scss']
})
export class VehicleRentCarCreateModalComponent implements OnInit {

  contactNumber: any;
  contactName: any;
  companyName: any;
  createDate: any;
  screenStatus: any;
  message: string;
  showType: any;
  showData: any;

  formlabels: {
    title: any
    createddate: any
    companyname: any
    contactname: any
    contactnumber: any
    submit: any
    delete: any
  };

  formModel: {
    CarCompanyID: number
    CompanyName: String
    ContactName: String
    ContactNumber: String
    DeleteFlag: number
    CreatedBy: number
    UpdatedBy: number
    UpdatedDateTime: any
    CreatedDateTime: any
  }
  User: any;
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };

  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  disable: boolean = false;
  screenType = 'Create';
  carCompanyID: any;
  disableBtn: boolean;
  lang: any;

  constructor(
    public common: CommonService,
    public bsModalRef: BsModalRef,
    public api: VehicleMgmtServiceService,
    public modalService: BsModalService,
    public util: UtilsService
  ) {
    this.User = JSON.parse(localStorage.getItem('User'));
  }

  ngOnInit() {

    this.contactNumber = '';
    this.contactName = '';
    this.companyName = '';
    this.createDate = new Date();
    this.screenStatus = 'Create'

    if(this.common.language == 'English'){
      this.lang = 'en';
    }else{
      this.lang = 'ar';
    }
    this.formlabels = {
      title: (this.common.language == 'English') ? "ADD RENT A CAR COMPANY" : this.arabic('addrentacarcompany'),
      createddate: (this.common.language == 'English') ? "Created Date" : this.arabic('createddate'),
      companyname: (this.common.language == 'English') ? "Company Name" : this.arabic('companyname'),
      contactname: (this.common.language == 'English') ? "Contact Name" : this.arabic('contactname'),
      contactnumber: (this.common.language == 'English') ? "Contact Number" : this.arabic('contactnumber'),
      submit: (this.common.language == 'English') ? "SUBMIT" : this.arabic('submit'),
      delete: (this.common.language == 'English') ? "DELETE" : this.arabic('delete')
    };
    this.reset();
    setTimeout(() => {
      this.setData();
    }, 500);
  }


  reset() {
    this.formModel = {
      CarCompanyID: 0,
      CompanyName: '',
      ContactName: '',
      ContactNumber: '',
      DeleteFlag: 0,
      CreatedBy: 0,
      UpdatedBy: 0,
      UpdatedDateTime: '',
      CreatedDateTime: ''
    }
  }

  openSuccessModal() {
    this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
    this.bsModalRef.content.message = this.message;
    //this.bsModalRef.content.pagename = 'vehicle-rent-car';
    this.bsModalRef.content.closeEvent.subscribe(res => {
       this.api.reloadRentCar.next();
    });
  }

  closemodal() {
    this.disableBtn = false;
    this.bsModalRef.hide();
  }

  prepareData(type) {
    this.formModel.CompanyName = this.companyName;
    this.formModel.ContactName = this.contactName;
    this.formModel.ContactNumber = this.contactNumber;
    if (type == 'Create') {
      this.formModel.CarCompanyID = 0;
      this.formModel.CreatedBy = this.User.id;
      this.formModel.CreatedDateTime = this.createDate;
    } else {
      this.formModel.CarCompanyID = this.carCompanyID;
      this.formModel.UpdatedBy = this.User.id;
      this.formModel.UpdatedDateTime = new Date();
    }
    this.formModel.DeleteFlag = 0;
    return this.formModel;
  }

  setData() {
    if (this.showType && this.showData) {
      this.screenType = this.showType;
      if (this.showType == 'View') {
        this.disable = true;
      }
      this.carCompanyID = this.showData.CarCompanyID;
      this.companyName = this.showData.CompanyName;
      this.contactName = this.showData.ContactName;
      this.contactNumber = this.showData.ContactNumber;
      this.createDate = (this.showData.CreatedDateTime) ? new Date(this.showData.CreatedDateTime) : '';
    }
  }

  validate() {
    var flag = true;
    if (!this.util.isEmptyString(this.companyName) && 
    !this.util.isEmptyString(this.contactName) && 
    this.contactNumber && this.createDate && 
    !this.disableBtn &&
    this.util.isValidDate(this.createDate))
      flag = false;
    return flag;
  }

  save() {
    this.disableBtn = true;
    var param = this.prepareData(this.screenType);
    if (this.screenType == 'Create') {
      this.api.saveCarCompany("VehicleCarCompany", param).subscribe(res => {
        this.closemodal();
        this.message = (this.common.language == "English") ? "Rent a Car Company Created Successfully" : this.arabic('carcompanycreatedsuccessfull');
        this.openSuccessModal();
      });
    }
    else {
      this.api.updateCarCompany("VehicleCarCompany", param).subscribe(res => {
        this.closemodal();
        this.message = (this.common.language == "English") ? "Rent a Car Company Updated Successfully" : this.arabic('carcompanyupdatedsuccessfull');
        this.openSuccessModal();
      });
    }
  }

  delete() {
    this.disableBtn = true;
    this.api.deleteCompany("VehicleCarCompany", this.carCompanyID).subscribe(res => {
      this.closemodal();
      this.message = (this.common.language == "English") ? "Rent a Car Company Deleted Successfully" : this.arabic('carcompanydeletedsuccessfull');
      this.openSuccessModal();
    });
  }


  arabic(word) {
    return this.common.arabic.words[word];
  }

}
