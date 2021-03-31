import {Component, OnInit} from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import {ProfileService} from './services/profile.service';
import {CookieService} from 'ngx-cookie-service';
import {EncService} from './services/enc.service';
import {ActivatedRoute, Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import {Meta} from '@angular/platform-browser';
import {NgxSpinnerService} from 'ngx-spinner';
import {DeviceDetectorService} from 'ngx-device-detector';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  langClass: string;
  theme: string;
  GoodToGo: boolean;

  constructor(private MetaService: Meta, private translate: TranslateService, private route: ActivatedRoute, private spinner: NgxSpinnerService,
              protected keycloakAngular: KeycloakService, private BrowserCheck: DeviceDetectorService, private router: Router, private profileService: ProfileService) {
    this.GoodToGo = false;
  }

  ngOnInit() {
    if (
      (this.BrowserCheck.browser.toLowerCase().includes('chrome') && this.BrowserCheck.browser_version.split('.')[0] > '55') ||
      (this.BrowserCheck.browser.toLowerCase().includes('firefox') && this.BrowserCheck.browser_version.split('.')[0] > '51') ||
      (this.BrowserCheck.browser.toLowerCase().includes('edge') && this.BrowserCheck.browser_version.split('.')[0] > '15') ||
      (this.BrowserCheck.browser.toLowerCase().includes('safari') && this.BrowserCheck.browser_version.split('.')[0] > '10')) {
      this.initial();
    } else {
      this.router.navigateByUrl('not-supported');
    }
  }

  initial() {
    this.GoodToGo = true;
    this.checkLogin();
    this.theme = '';
    this.setTheme();
    this.profileService.getClientThemeUpdate().subscribe(() => {
      this.setTheme();
    });
    this.langClass = '';
    this.translate.onLangChange.subscribe((res) => {
      if (res.lang === 'ar') {
        this.langClass = 'ar';
      } else {
        this.langClass = '';
      }
    });
    this.router.events.subscribe((res: any) => {
      if (res && res.url) {
        this.MetaService.updateTag({name: 'keywords', content: 'Manara, Resources, Keywords'});
        this.MetaService.updateTag({
          name: 'description',
          content: 'Manara is a public digital library of open educational resources. Explore, create, and collaborate with people around the world to improve knowledge gathering'
        });
      }
    });
    this.timer();
  }

  timer() {
    setTimeout(() => {
      this.autoCheck();
    }, 300000);
  }

  autoCheck() {
    this.keycloakAngular.isLoggedIn().then((res) => {
      if (res) {
      } else {
        if (this.profileService.user != null) {
          this.profileService.setUser(null);
          this.router.navigate(['/']);
        }
      }
    }, (error) => {
      this.profileService.setUser(null);
    });
    this.timer();
  }

  setTheme() {
    // let color = '';
    if (this.profileService.theme === 'blue') {
      this.theme = 'blue';
      // color = '#1a3464';
    } else {
      this.theme = '';
      // color = '#ba9a3a';
    }
    // setTimeout(() => {
    //   this.spinner.show(undefined, {color: color});
    //   setTimeout(() => {
    //     this.spinner.hide();
    //   }, 500);
    // }, 1000);
  }

  getThemeClass() {
    return this.theme === 'blue' ? 'blue' : '';
  }

  getLangClass() {
    return this.langClass;
  }

  getLangDir() {
    let dir = '';
    if (this.langClass === 'ar') {
      dir = 'rtl';
    }
    return dir;
  }

  checkLogin() {
    this.keycloakAngular.isLoggedIn().then((res) => {
      if (res) {
        this.checkUserStatus();
      } else {
        this.logVisitor(0);
      }
    }, (error) => {
    });
  }

  logVisitor(id) {
    this.profileService.logVisitor(id).subscribe(() => {
    });
  }

  checkUserStatus() {
    let isAdmin = false;
    let isContributor = false;
    this.keycloakAngular.getUserRoles(true).forEach((item) => {
      if (item.toLowerCase().trim() === 'Portal Admin'.toLowerCase().trim()) {
        isAdmin = true;
      }
      if (item.toLowerCase().trim() === 'Contributor'.toLowerCase().trim()) {
        isContributor = true;
      }
    });
    this.keycloakAngular.loadUserProfile().then((response) => {
      this.profileService.checkProfileStatus(response.email).subscribe((profileRes) => {
        if (profileRes.hasSucceeded) {
          this.logVisitor(profileRes.returnedObject.id);
          this.profileService.setUser(profileRes.returnedObject);
          if (!isAdmin === profileRes.returnedObject.isAdmin && (isContributor === true && profileRes.returnedObject.isContributor === false)) {
            this.profileService.postUpdateRole('userId=' + profileRes.returnedObject.id + '&isAdmin' + isAdmin + '&isContributor=true').subscribe(() => {
              this.checkUserStatus();
            });
          } else if (!isAdmin === profileRes.returnedObject.isAdmin) {
            this.profileService.postUpdateRole('userId=' + profileRes.returnedObject.id + '&isAdmin=' + isAdmin).subscribe(() => {
              this.checkUserStatus();
            });
          } else if (isContributor === true && profileRes.returnedObject.isContributor === false) {
            this.profileService.postUpdateRole('userId=' + profileRes.returnedObject.id + '&isContributor=true').subscribe(() => {
              this.checkUserStatus();
            });
          }
          if (profileRes.returnedObject.active === false) {
            this.router.navigateByUrl('error/401');
          }
        } else {
          this.postInitialProfile(response, isAdmin, isContributor);
        }
      }, (error) => {
      });
    });
  }

  postInitialProfile(response, isAdmin, isContributor) {
    this.profileService.postInitalProfile({
      firstName: response.firstName,
      lastName: response.lastName,
      email: response.email,
      isContributor: isContributor,
      isAdmin: isAdmin
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.profileService.setUser(res.returnedObject);
      }
    });
  }

}
