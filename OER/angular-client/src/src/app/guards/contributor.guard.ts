import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {CookieService} from 'ngx-cookie-service';
import {KeycloakService} from 'keycloak-angular';
import {EncService} from '../services/enc.service';
import {ProfileService} from '../services/profile.service';
import {MessageService} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';

/**
 * Contributor Guard Check
 */
@Injectable({
  providedIn: 'root'
})
export class ContributorGuard implements CanActivate {
  /**
   *
   * @param router
   * @param cookieService
   * @param messageService
   * @param translate
   * @param keycloakAngular
   * @param encService
   * @param profileService
   */
  constructor(private router: Router, private cookieService: CookieService, private messageService: MessageService, private translate: TranslateService, private keycloakAngular: KeycloakService, private encService: EncService, private profileService: ProfileService) {

  }

  /**
   * checks keycloak & profile service for login status,
   *
   * if logged in, calls checkUser functions
   *
   * if not, redirects to 401 page
   *
   * @param next
   * @param state
   */
  async canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Promise<boolean> {
    return this.keycloakAngular.loadUserProfile().then(async (res) => {
      if (this.profileService.user) {
        return this.checkUser(res);
      } else {
        const user = await this.profileService.checkProfileStatus(res.email).toPromise();
        if (user.hasSucceeded) {
          this.profileService.setUser(user.returnedObject);
        }
        return this.checkUser(res);
      }
    }).catch((error) => {
      return Promise.resolve(false);
    });
  }

  /**
   * checks if user contributor status , continues if true
   *
   *  else return respective error pages
   *
   * @param res
   */
  checkUser(res): Promise<boolean> {
    if (res && this.profileService.user && this.profileService.user.isContributor) {
      return Promise.resolve(true);
    } else {
      if (this.profileService.user && !this.profileService.user.isContributor) {
        this.translate.get('You need to be a contributor to perform this action. Please update the profile from the dashboard').subscribe((trans) => {
          this.messageService.add({
            severity: 'error',
            summary: trans,
            key: 'toast'
          });
        });
      }
      return Promise.resolve(false);
    }

  }
}
