import { Component, OnInit, Input } from '@angular/core';
import { CommonService } from '../../common.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { VehicleMgmtServiceService } from 'src/app/vehicle-mgmt/service/vehicle-mgmt-service.service';
import { SuccessComponent } from '../success-popup/success.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vehicle-management',
  templateUrl: './vehicle-management.component.html',
  styleUrls: ['./vehicle-management.component.scss']
})
export class VehicleManagementComponent implements OnInit {
  lang: string;
  @Input() currentMileage: any;
  nextMileage: any;
  @Input() id: number;
  @Input() Title: string;
  @Input() Message: any;
  @Input() type: any;
  isApiLoading:boolean = false;

  constructor(
    public common: CommonService,
    public modalRef: BsModalRef,
    public newModalRef: BsModalRef,
    private vehicleService:VehicleMgmtServiceService,
    private modalService:BsModalService,
    private newModalService:BsModalService,
    private router:Router
  ) { }

  ngOnInit() {
    this.lang = this.common.currentLang;
  }

  saveLogDetails(){
    this.isApiLoading = true;
    let toSendLogData:any = {
      CurrentMileage: this.currentMileage,
      VehicleID: this.id,
      NextMileage: this.nextMileage,
      LogType: this.type,
      DeleteFlag: true,
      CreatedBy: this.common.currentUser.id,
      CreatedDateTime: new Date()
    }
    this.vehicleService.saveVehicleLog(toSendLogData).subscribe((logResData:any)=>{
      // if(logResData){
        this.modalRef.hide();
        this.newModalRef = this.newModalService.show(SuccessComponent);
        this.newModalRef.content.message = this.Message;
        let newSubscriber = this.newModalService.onHide.subscribe(r=>{          
          newSubscriber.unsubscribe();
          // this.router.navigate(['/'+this.common.currentLang+'/app/vehicle-management/vehicle-list']);
        });
      // }
      this.isApiLoading = false;
    });
  }

  closemodal() {
    this.modalRef.hide();
  }
  arabic(word) {
    word = word.replace(/ +/g, "").toLowerCase();
    return this.common.arabic.words[word];
  }

  allowSubmit(){
    debugger;
    let canSubmit = true;
    if(this.currentMileage && this.nextMileage){
      if((isNaN(this.nextMileage) || this.nextMileage < 0) || (isNaN(this.currentMileage) || this.currentMileage <0)){
        canSubmit = false;
      }
    }else{
      canSubmit = false;
    }
    if(Number(this.currentMileage)>=Number(this.nextMileage))
    canSubmit = false;
    return canSubmit;
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
