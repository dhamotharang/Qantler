import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {CookieService} from 'ngx-cookie-service';
import {KeycloakService} from 'keycloak-angular';
import {EncService} from '../services/enc.service';
import {ProfileService} from '../services/profile.service';
import {MessageService} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';

/**
 * check if user profile is updated
 */
@Injectable({
  providedIn: 'root'
})
export class ProfileUpdateGuard implements CanActivate {
  /**
   *
   * @param router
   * @param cookieService
   * @param translate
   * @param messageService
   * @param keycloakAngular
   * @param encService
   * @param profileService
   */
  constructor(private router: Router, private cookieService: CookieService, private translate: TranslateService, private messageService: MessageService, private keycloakAngular: KeycloakService, private encService: EncService, private profileService: ProfileService) {

  }

  /**
   *
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
   *
   * checks if user status is active & title, continues if both are true
   *
   * else return respective error pages
   *
   * @param res
   */
  checkUser(res): Promise<boolean> {
    if (res && this.profileService.user && this.profileService.user.active && this.profileService.user.title) {
      return Promise.resolve(true);
    } else {
      if (this.profileService.user) {
        if (!this.profileService.user.title) {
          this.router.navigateByUrl('make-profile');
          // this.messageService.add({severity: 'error', summary: 'Please complete your profile to perform this action.', key: 'toast'});
        } else {
          this.translate.get('Your Account is Inactive. Please Contact site Administrator.').subscribe((trans) => {
            this.messageService.add({
              severity: 'error',
              summary: trans,
              key: 'toast'
            });
          });
        }
      }
      return Promise.resolve(false);
    }
  }
}
