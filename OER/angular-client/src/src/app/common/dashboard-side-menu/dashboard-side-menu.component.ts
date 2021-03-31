import {Component, OnInit} from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import {QrcService} from '../../services/qrc.service';
import {ProfileService} from '../../services/profile.service';

@Component({
  selector: 'app-dashboard-side-menu',
  templateUrl: './dashboard-side-menu.component.html'
})
export class DashboardSideMenuComponent implements OnInit {
  isVerifier: boolean;
  showQRC: boolean;

  constructor(private keycloak: KeycloakService, private QRCService: QrcService, private profileService: ProfileService) {
    this.isVerifier = false;
    this.showQRC = false;
  }

  ngOnInit() {
    this.keycloak.getUserRoles(true).forEach((item) => {
      if (item === 'Verifier') {
        this.isVerifier = true;
      }
    });
    if (this.profileService.userId > 0) {
      this.showQRC = this.profileService.QRCCount > 0;
    }
    this.profileService.QRCCountUpdateSub().subscribe(() => {
      if (this.profileService.userId > 0) {
        this.showQRC = this.profileService.QRCCount > 0;
      }
    });
  }

}
