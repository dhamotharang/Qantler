import { Component, OnInit, Input, ChangeDetectorRef, Renderer2, Inject, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { CommonService } from 'src/app/common.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DatePipe, DOCUMENT } from '@angular/common';
import { BsModalService, BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { VehicleMgmtServiceService } from '../../service/vehicle-mgmt-service.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { EndPointService } from 'src/app/api/endpoint.service';
import { AdminService } from 'src/app/admin/service/admin/admin.service';
import { VehicleDetails } from '../../model/vehicle-details/vehicle-details.model';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { VehicleManagementComponent } from 'src/app/modal/vehicle-management/vehicle-management.component';
import { isDate } from 'util';

@Component({
  selector: 'app-vehicle-details-form',
  templateUrl: './vehicle-details-form.component.html',
  styleUrls: ['./vehicle-details-form.component.scss']
})
export class VehicleDetailsFormComponent implements OnInit {
  isApiLoading: boolean;
  currentUser: any;
  lang: any;
  IsOrgHead: boolean;
  OrgUnitID: any;
  @Input() mode: string;
  @Input() requestId: number;
  @ViewChild('template') template : TemplateRef<any>;
  bsModalRef: BsModalRef;
  leaveType = [];
  department = [];
  approverDepartment = [];
  attachmentFiles = [];
  dropdownSettings: any;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  vehicleDetailsRequestModel: VehicleDetails = {
    VehicleID: 0,
    ReferenceNumber: '',
    PlateNumber: '',
    PlateColor: '',
    // VehicleName:'',
    IsAlternativeVehicle: 'false',
    VehicleMake: '',
    VehicleModel: '',
    YearofManufacture: '',
    CarCompanyID: '',
    ContractNumber: '',
    ContractDuration: '',
    ContractStartDate: null,
    ContractEndDate: null,
    VehicleRegistrationNumber: '',
    VehicleRegistrationExpiry:'',
    NextService:'',
    TyreChange:'',
    Notes:'',
    CreatedBy:'',
    UpdatedBy:'',
    CreatedDateTime: new Date(),
    UpdatedDateTime:'',
    Status:'',
    CurrentMileage:0
  };
  vehicleFormGroup: FormGroup;;
  

  carCompanyList: any[] =  [];
  message: any;
  month: any;
  validateStartEndDate:any = {
    isValid:true,
    msg:''
  };

  constructor(private changeDetector:ChangeDetectorRef,
    public common: CommonService,
    public router: Router,
    private route: ActivatedRoute,
    public datepipe: DatePipe,
    private modalService: BsModalService,
    private VehicleService:VehicleMgmtServiceService,
    private _formBuilder:FormBuilder,
    private utilsService:UtilsService,
    private enpointService:EndPointService,
    private adminservice: AdminService,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document) { }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.lang = this.common.currentLang;
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 0;


    if(this.common.currentLang != 'en'){
      if(this.mode == 'create'){
        this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('vehiclecreation'),'');
      }else{
        this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('vehicleview'),'');
      }
      
      this.common.topBanner(false, '', '', '');
    }else{
      if(this.mode == 'create'){
      this.common.breadscrumChange('Vehicle Management','Vehicle Creation','');
      }else{
        this.common.breadscrumChange('Vehicle Management','Vehicle View','');
      }
      this.common.topBanner(false, '', '', '');
    }
    this.getCarCompanyList();
    if(this.requestId){
      this.initialValidators();
      this.getVehicleDetails();
    }else{
      this.initialValidators();
      this.setValidators();
      this.VehicleService.getVehicleDetailsById(0,this.currentUser.id).subscribe((vehicleData:any) => {
        if(vehicleData){
          // console.log(vehicleData);
          this.changeDetector.detectChanges();
          this.vehicleFormGroup.controls['ContractDuration'].disable();
        }
      });
    }
  }

  initialValidators(){
    this.vehicleFormGroup = this._formBuilder.group({
      VehicleID:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleID || 0],
      ReferenceNumber: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.ReferenceNumber || ''],
      // VehicleName: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleName || '', [Validators.required,this.emptyStringValidator]],
      IsAlternativeVehicle:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.IsAlternativeVehicle || ''],
      PlateNumber: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.PlateNumber || '', [Validators.required, Validators.pattern('[A-Za-z0-9]+')]],
      PlateColor: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.PlateColor || '', [Validators.required,this.emptyStringValidator]],
      VehicleMake: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleMake || '', [Validators.required,this.emptyStringValidator]],
      VehicleModel: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleModel || '', [Validators.required,this.emptyStringValidator]],
      YearOfManufacture: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.YearofManufacture || '', [Validators.required, Validators.pattern('[0-9]+')]],
      CarCompanyID: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CarCompanyID || '', [Validators.required]],
      ContractNumber: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.ContractNumber || '', [Validators.required,Validators.pattern('[A-Za-z0-9]+')]],
      ContractDuration: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.ContractDuration || '', [Validators.required]],
      ContractStartDate: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.ContractStartDate || null, [Validators.required]],
      ContractEndDate: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.ContractEndDate || null, [Validators.required]],
      VehicleRegistrationNumber: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleRegistrationNumber || '', [Validators.required,this.emptyStringValidator]],
      VehicleRegistrationExpiry:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleRegistrationExpiry || '', [Validators.required]],
      NextService:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.NextService || '', [Validators.required,Validators.pattern('[0-9]+')]],
      TyreChange:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.TyreChange || '', [Validators.required,Validators.pattern('[0-9]+')]],
      Notes:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.Notes || '', [Validators.required,this.emptyStringValidator]],
      CreatedBy:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CreatedBy || ''],
      CreatedDateTime: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CreatedDateTime || ''],
      Status: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.Status || ''],
      Department:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CreatedDepartment || '']
    });
  }

  setValidators(){
    const contractNumber = this.vehicleFormGroup.get('ContractNumber');
    const contractDuration = this.vehicleFormGroup.get('ContractDuration');
    const contractStartDate = this.vehicleFormGroup.get('ContractStartDate');
    const contractEndDate = this.vehicleFormGroup.get('ContractEndDate');
    const nextService = this.vehicleFormGroup.get('NextService');
    const tyreChange = this.vehicleFormGroup.get('TyreChange');
    this.vehicleFormGroup.get('IsAlternativeVehicle').valueChanges.subscribe(vehicleType => {
      if (vehicleType === 'true') {
        contractNumber.setValidators(null);
        contractDuration.setValidators(null);
        contractStartDate.setValidators(null);
        contractEndDate.setValidators(null);
        nextService.setValidators(null);
        tyreChange.setValidators(null);
      }
      if (vehicleType === 'false') {
        contractNumber.setValidators([Validators.required,Validators.pattern('[A-Za-z0-9]+')]);
        contractDuration.setValidators([Validators.required]);
        contractStartDate.setValidators([Validators.required]);
        contractEndDate.setValidators([Validators.required]);
        nextService.setValidators([Validators.required,Validators.pattern('[0-9]+')]);
        tyreChange.setValidators([Validators.required,this.emptyStringValidator]);
      }

      contractNumber.updateValueAndValidity();
      contractDuration.updateValueAndValidity();
      contractStartDate.updateValueAndValidity();
      contractEndDate.updateValueAndValidity();
      nextService.updateValueAndValidity();
      tyreChange.updateValueAndValidity();
    })

  }

  setValidatorsOnViewScreen(vehicleType){
    const contractNumber = this.vehicleFormGroup.get('ContractNumber');
    const contractDuration = this.vehicleFormGroup.get('ContractDuration');
    const contractStartDate = this.vehicleFormGroup.get('ContractStartDate');
    const contractEndDate = this.vehicleFormGroup.get('ContractEndDate');
    const nextService = this.vehicleFormGroup.get('NextService');
    const tyreChange = this.vehicleFormGroup.get('TyreChange');
    if(vehicleType) {
      if (vehicleType === 'true') {
        contractNumber.setValidators(null);
        contractDuration.setValidators(null);
        contractStartDate.setValidators(null);
        contractEndDate.setValidators(null);
        nextService.setValidators(null);
        tyreChange.setValidators(null);
      }
      if (vehicleType === 'false') {
        contractNumber.setValidators([Validators.required,Validators.pattern('[A-Za-z0-9]+')]);
        contractDuration.setValidators([Validators.required]);
        contractStartDate.setValidators([Validators.required]);
        contractEndDate.setValidators([Validators.required]);
        nextService.setValidators([Validators.required,Validators.pattern('[0-9]+')]);
        tyreChange.setValidators([Validators.required,this.emptyStringValidator]);
      }

      contractNumber.updateValueAndValidity();
      contractDuration.updateValueAndValidity();
      contractStartDate.updateValueAndValidity();
      contractEndDate.updateValueAndValidity();
      nextService.updateValueAndValidity();
      tyreChange.updateValueAndValidity();
    }
  }

  userAction(action?:any){

  }

  minDate(days){
    // console.log('ContractStartDate', this.vehicleFormGroup.value.ContractStartDate);
    if(this.vehicleFormGroup.value.ContractStartDate && isDate(this.vehicleFormGroup.value.ContractStartDate)){
      let today = new Date(this.vehicleFormGroup.value.ContractStartDate);
      this.month = today.getMonth()+1;
      if (today.getMonth() < 10) {
        this.month = '0' + (today.getMonth() + 1);
      }
      let dateLimit = (today.getFullYear()) + '/' + this.month + '/' + (today.getDate() + days);
      let dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  maxDate(days) {
    // console.log('ContractEndDate', this.vehicleFormGroup.value.ContractEndDate);
    if (this.vehicleFormGroup.value.ContractEndDate && isDate(this.vehicleFormGroup.value.ContractEndDate)) {
      let endDate = new Date(this.vehicleFormGroup.value.ContractEndDate);
      this.month = endDate.getMonth()+1;
      if (this.month < 10) {
        this.month = '0' + (endDate.getMonth() + 1);
      }
      let dateLimit = (endDate.getFullYear()) + '/' + this.month + '/' + (endDate.getDate() + days);
      let dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  arabic(word) {
    word = word.replace(/ +/g, "").toLowerCase();
    return this.common.arabic.words[word];
  }

  getVehicleDetails(){
    this.changeDetector.detectChanges();
    // this.vehicleFormGroup.enable();
    this.VehicleService.getVehicleDetailsById(this.requestId,this.currentUser.id).subscribe((vehicleData:any) => {
      if(vehicleData){
        this.vehicleDetailsRequestModel = vehicleData;
        this.vehicleFormGroup = this._formBuilder.group({
          VehicleID:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleID || 0],
          ReferenceNumber: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.ReferenceNumber || ''],
          PlateNumber: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.PlateNumber || ''],
          VehicleMake: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleMake || ''],
          PlateColor: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.PlateColor || ''],
          // VehicleName: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleName || '', [Validators.required,this.emptyStringValidator]],
          IsAlternativeVehicle:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.IsAlternativeVehicle || ''],
          VehicleModel: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleModel || '', [Validators.required,this.emptyStringValidator]],
          YearOfManufacture: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.YearofManufacture || '', [Validators.required,Validators.pattern('[0-9]+')]],
          CarCompanyID: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CarCompanyID || '', [Validators.required]],
          ContractNumber: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.ContractNumber || '', [Validators.required,Validators.pattern('[A-Za-z0-9]+')]],
          ContractDuration: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.ContractDuration || '', [Validators.required]],
          ContractStartDate: [this.vehicleDetailsRequestModel && new Date(this.vehicleDetailsRequestModel.ContractStartDate) || '', [Validators.required, this.emptyStringValidator]],
          ContractEndDate: [this.vehicleDetailsRequestModel && new Date(this.vehicleDetailsRequestModel.ContractEndDate) || '', [Validators.required,this.emptyStringValidator]],
          VehicleRegistrationNumber: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.VehicleRegistrationNumber || '', [Validators.required,this.emptyStringValidator,this.emptyStringValidator]],
          VehicleRegistrationExpiry:[this.vehicleDetailsRequestModel && new Date(this.vehicleDetailsRequestModel.VehicleRegistrationExpiry) || '', [Validators.required,this.emptyStringValidator]],
          NextService:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.NextService || '', [Validators.required,Validators.pattern('[0-9]+')]],
          TyreChange:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.TyreChange || '', [Validators.required,Validators.pattern('[0-9]+')]],
          Notes:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.Notes || '', [Validators.required, this.emptyStringValidator]],
          CreatedBy:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CreatedBy || ''],
          CreatedDateTime: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CreatedDateTime || ''],
          Status: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.Status || ''],
          Department:[this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CreatedDepartment || ''],
          CurrentMileage: [this.vehicleDetailsRequestModel && this.vehicleDetailsRequestModel.CurrentMileage || '']
        });
      }
      
      this.vehicleFormGroup.patchValue({
        Department: vehicleData.OrganizationList.find((fd) => fd.OrganizationID == this.vehicleDetailsRequestModel.CreatedDepartment).OrganizationUnits.toString()
      });

      this.vehicleFormGroup.patchValue({
        IsAlternativeVehicle: (this.vehicleDetailsRequestModel.IsAlternativeVehicle == true) ? 'true': 'false'
      });
      this.setValidatorsOnViewScreen(this.vehicleFormGroup.value.IsAlternativeVehicle);
      let params = [{
        'OrganizationID': this.vehicleDetailsRequestModel.CreatedDepartment,
        'OrganizationUnits': 'string'
      }];
      this.common.getUserList(params,0).subscribe((data: any) => {
        let userList = data;
        this.vehicleFormGroup.patchValue({
          CreatedBy: userList.find(x=> x.UserID == this.vehicleDetailsRequestModel.CreatedBy).EmployeeName.toString()
        });
        this.changeDetector.detectChanges();
        let compRef = this;
        setTimeout(function(){
          compRef.changeDetector.detectChanges();
          compRef.vehicleFormGroup.controls['Department'].disable();
          compRef.vehicleFormGroup.controls['CreatedBy'].disable();
          compRef.vehicleFormGroup.controls['PlateNumber'].disable();
          compRef.vehicleFormGroup.controls['PlateColor'].disable();
          compRef.vehicleFormGroup.controls['ContractDuration'].disable();
          compRef.changeDetector.detectChanges();
        },100);
      });
    });
  }

  modifyVehicleDetails(){
    this.isApiLoading =true;
    let vehicleRequestData:any = {
      PlateNumber: this.vehicleFormGroup.value.PlateNumber,
      PlateColor: this.vehicleFormGroup.value.PlateColor,
      VehicleMake: this.vehicleFormGroup.value.VehicleMake,
      VehicleModel: this.vehicleFormGroup.value.VehicleModel,
      // VehicleName: this.vehicleFormGroup.value.VehicleName,
      IsAlternativeVehicle:(this.vehicleFormGroup.value.IsAlternativeVehicle == 'true') ? true: false,
      YearOfManufacture: this.vehicleFormGroup.value.YearOfManufacture,
      CarCompanyID: this.vehicleFormGroup.value.CarCompanyID,
      ContractNumber: this.vehicleFormGroup.value.ContractNumber,
      ContractDuration: (this.vehicleFormGroup.value.IsAlternativeVehicle == 'false') ? this.vehicleFormGroup.controls['ContractDuration'].value: '',
      ContractStartDate: (this.vehicleFormGroup.value.IsAlternativeVehicle == 'false') ? this.vehicleFormGroup.value.ContractStartDate: null,
      ContractEndDate: (this.vehicleFormGroup.value.IsAlternativeVehicle == 'false') ? this.vehicleFormGroup.value.ContractEndDate: null,
      VehicleRegistrationNumber: this.vehicleFormGroup.value.VehicleRegistrationNumber,
      VehicleRegistrationExpiry: this.vehicleFormGroup.value.VehicleRegistrationExpiry,
      NextService: this.vehicleFormGroup.value.NextService,
      TyreChange: this.vehicleFormGroup.value.TyreChange,
      Notes: this.vehicleFormGroup.value.Notes
    };
    if(!this.requestId){
      vehicleRequestData.CreatedDateTime = new Date();
      vehicleRequestData.CreatedBy = this.currentUser.id;
      this.message = 'Vehicle Management Details Submitted Successfully';
      if(this.lang != 'en'){
        this.message = this.arabic('managevehiclecreatereqmsg');
      }
      this.VehicleService.saveVehicle(vehicleRequestData).subscribe((vehicleDataRes) => {
        if(vehicleDataRes){
          this.isApiLoading =false;
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(r=>{
            newSubscriber.unsubscribe();
            this.router.navigate(['/'+this.common.currentLang+'/app/vehicle-management/vehicle-list']);
          });
        }
      },error => {
        this.isApiLoading =false;
      });
    }else{
      vehicleRequestData.PlateColor = this.vehicleDetailsRequestModel.PlateColor;
      vehicleRequestData.PlateNumber = this.vehicleDetailsRequestModel.PlateNumber;  
      vehicleRequestData.VehicleID = this.requestId;
      vehicleRequestData.UpdatedDateTime = new Date();
      vehicleRequestData.UpdatedBy = this.currentUser.id;
      this.message = 'Vehicle Management Details Updated Successfully';
      if(this.lang != 'en'){
        this.message = this.arabic('managevehicleupdatereqmsg');
      }
      this.VehicleService.editVehicle(vehicleRequestData).subscribe((vehicleDataRes) => {
        if(vehicleDataRes){
          this.isApiLoading =false;
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(r=>{
            newSubscriber.unsubscribe();
            this.router.navigate(['/'+this.common.currentLang+'/app/vehicle-management/vehicle-list']);
          });
        }
      },error => {
        this.isApiLoading =false;
      });
    }
  }

  getCarCompanyList(){
    this.VehicleService.getCarCompanyList().subscribe((companyList:any) => {
      if(companyList){
        this.carCompanyList = companyList;
      }
    });
  }

  radioChange(event){
    if(this.mode == 'view'){
      this.setValidatorsOnViewScreen(event.target.value);
    }
    if(event.target.value == 'true'){

    }else if(event.target.value == 'false'){
      this.vehicleFormGroup.controls['ContractNumber'].setValue('');
      this.vehicleFormGroup.controls['ContractDuration'].setValue('');
      this.vehicleFormGroup.controls['ContractStartDate'].setValue(null);
      this.vehicleFormGroup.controls['ContractEndDate'].setValue(null);
      this.vehicleFormGroup.controls['NextService'].setValue('');
      this.vehicleFormGroup.controls['TyreChange'].setValue('');
    }
  }

  openLogModal(type: string) {
    let initialState: any = {
      id: this.requestId
    };
    if (type == 'service') {
      initialState.Message = 'Service Log Submitted Successfully';
      initialState.Title = 'LOG A SERVICE';
      if(this.lang != 'en'){
        initialState.Title = this.arabic('logaservice');
        initialState.Message = this.arabic('nextservicereqmsg');
      }
      initialState.type = 1;
      initialState.currentMileage = this.vehicleDetailsRequestModel.CurrentMileage;
    } else if (type == 'tyre') {
      initialState.Message = 'Tyre Change Log Submitted Successfully';
      initialState.Title = 'LOG A TYRE CHANGE';
      if(this.lang != 'en'){
        initialState.Title = this.arabic('logatyrechange');
        initialState.Message = this.arabic('tyrechangereqmsg');
      }
      initialState.type = 2;
      initialState.currentMileage = this.vehicleDetailsRequestModel.CurrentMileage;
    } else {
      this.router.navigate([this.common.currentLang+'/app/vehicle-management/fine-management/log-fine/'+this.vehicleDetailsRequestModel.PlateNumber]);
    }
    // if (this.common.currentLang == 'ar') {
    //   initialState.message = this.arabic('leaveescalatemsg');
    // }
    if (type == 'service' || type == 'tyre') {
      this.modalService.show(VehicleManagementComponent, Object.assign({}, {}, { initialState }));
    }
    let newSubscriber = this.modalService.onHide.subscribe(r=>{
      newSubscriber.unsubscribe();
      this.getVehicleDetails();
    });
  }

  dateChange(eve,isStartEnd){
    if(eve){
      if(isStartEnd == 'start'){
        this.vehicleFormGroup.patchValue({
          ContractStartDate: eve,
        });
        
      }
      if(isStartEnd == 'end'){
        this.vehicleFormGroup.patchValue({
          ContractEndDate: eve,
        });
      }      
    }
    this.checkStartEndDiff();
  }

  checkStartEndDiff(){
    let toRetVal = true;
    if((this.vehicleFormGroup.value.ContractStartDate && 
      this.utilsService.isValidDate(this.vehicleFormGroup.value.ContractStartDate))
    && (this.vehicleFormGroup.value.ContractEndDate && 
      this.utilsService.isValidDate(this.vehicleFormGroup.value.ContractEndDate))){
      if(this.vehicleFormGroup.value.ContractStartDate.getTime() <= this.vehicleFormGroup.value.ContractEndDate.getTime()){
        toRetVal =  true;
      }else{
        toRetVal = false;
      }
    }else{
      if(!this.utilsService.isDate(this.vehicleFormGroup.value.ContractStartDate) || !this.utilsService.isDate(this.vehicleFormGroup.value.ContractEndDate)){
        toRetVal = false;
      }
    }
    this.validateStartEndDate.isValid = toRetVal;
    if(this.validateStartEndDate.isValid){
      this.validateStartEndDate.msg = '';
      let startDate = new Date(this.vehicleFormGroup.value.ContractStartDate).getTime();
      let endDate = new Date(this.vehicleFormGroup.value.ContractEndDate).getTime();
      let duration:any = (endDate - startDate) / (1000 * 3600 * 24 * 365);
      duration = parseInt(duration);
      this.vehicleFormGroup.patchValue({
        ContractDuration:(duration >= 0) && !isNaN(duration) ? duration : ''
      });
    }else{
      this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date': this.arabic('errormsgvalidenddate');
      this.vehicleFormGroup.patchValue({
        ContractDuration:''
      });
    }
    return toRetVal;
  }

  emptyStringValidator(control: FormControl){
    if(typeof control.value == 'string'){
      const isEmptyString = (control.value || '').trim().length === 0;
      const isValid = !isEmptyString;
      return isValid ? null : { 'emptystring': true };
    }
      return null;
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
  
}
