import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { VehicleMgmtServiceService } from '../../service/vehicle-mgmt-service.service';
import { AdminService } from 'src/app/admin/service/admin/admin.service';
import { VehicleReleaseFormComponent } from '../vehicle-release-form/vehicle-release-form.component';

@Component({
  selector: 'app-vehicle-return-confirmation',
  templateUrl: './vehicle-return-confirmation.component.html',
  styleUrls: ['./vehicle-return-confirmation.component.scss']
})
export class VehicleReturnConfirmationComponent extends VehicleReleaseFormComponent {
  

  // ngOnInit() {
  //   this.lang = this.common.currentLang;

  //   this.common.breadscrumChange('Vehicle Management','Vehicle Return Confirmation','');
  //   this.common.topBanner(true, false, '', '');
  //   if(this.common.currentLang == 'ar'){
  //     this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('vehiclereturnconfirmation'),'');
  //   }
  //   this.common.topBanner(false, false, '', '');
  // }

  // arabic(word) {
  //   return this.common.arabic.words[word];
  // }
}