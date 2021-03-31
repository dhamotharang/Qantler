import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {KeycloakService} from 'keycloak-angular';
import {CookieService} from 'ngx-cookie-service';
import {EncService} from './services/enc.service';
import {ProfileService} from './services/profile.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  constructor(
    protected keycloakAngular: KeycloakService, private router: Router, private profileService: ProfileService,
    private cookieService: CookieService, private encService: EncService
    ) {

  }

  ngOnInit() {
    this.checkLogin();
  }

  checkLogin() {
    this.keycloakAngular.isLoggedIn().then((res) => {
      if (res) {
        this.checkUserStatus();
      }
    }, (error) => {
    });
  }

  checkUserStatus() {
    let isAdmin = false;
    let isContributor = false;
    this.keycloakAngular.getUserRoles(true).forEach((item) => {
      if (item === 'Portal Admin') {
        isAdmin = true;
      }
      if (item === 'Contributor') {
        isContributor = true;
      }
    });
    this.keycloakAngular.loadUserProfile().then((response) => {
      this.profileService.checkProfileStatus(response.email).subscribe((profileRes) => {
        if (profileRes.hasSucceeded) {
          this.profileService.setUser(profileRes.returnedObject);
          if (!isAdmin === profileRes.returnedObject.isAdmin) {
            this.profileService.postUpdateRole(profileRes.returnedObject.id, isAdmin, profileRes.returnedObject.isContributor)
            .subscribe(() => {
              this.checkUserStatus();
            });
          }
          if (profileRes.returnedObject.active === false) {
            this.router.navigateByUrl('error/401');
          }
        } else {
          this.postInitialProfile(response, isAdmin);
        }
      }, (error) => {
      });
    });
  }

  postInitialProfile(response, isAdmin) {
    this.profileService.postInitalProfile({
      firstName: response.firstName,
      lastName: response.lastName,
      email: response.email,
      isContributor: false,
      isAdmin: isAdmin
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.profileService.setUser(res.returnedObject);
      }
    });
  }


}
