import { Component, OnInit } from '@angular/core';
import { VehicleReleaseFormComponent } from '../vehicle-release-form/vehicle-release-form.component';

@Component({
  selector: 'app-vehicle-release-confirmation',
  templateUrl: './vehicle-release-confirmation.component.html',
  styleUrls: ['./vehicle-release-confirmation.component.scss']
})
export class VehicleReleaseConfirmationComponent extends VehicleReleaseFormComponent {
  lang: any;


  // ngOnInit() {
  //   this.lang = this.common.currentLang;

  //   this.common.breadscrumChange('Vehicle Management','Vehicle Release Confirmation','');
  //   this.common.topBanner(true, false, '', '');
  //   if(this.common.currentLang == 'ar'){
  //     this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('vehiclereleaseconfirmation'),'');
  //   }
  //   this.common.topBanner(false, false, '', '');
  // }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
