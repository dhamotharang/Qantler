import {Component, OnInit} from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import {CookieService} from 'ngx-cookie-service';
import {environment} from '../../../environments/environment';
import {EncService} from '../../services/enc.service';
import {ProfileService} from '../../services/profile.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  loggedIn: boolean;
  user: any;

  constructor(private keycloakAngular: KeycloakService, private profileService: ProfileService) {
    this.loggedIn = false;
  }

  ngOnInit() {
    this.loggedIn = false;
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.keycloakAngular.isLoggedIn().then((res) => {
        this.loggedIn = res;
      }, (error) => {
        this.loggedIn = false;
      });
      this.user = this.profileService.user;
    });
    this.user = this.profileService.user;
  }

  signout() {
    this.keycloakAngular.logout(environment.clientUrl);
  }

}
