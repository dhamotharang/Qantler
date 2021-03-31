import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import {KeycloakService} from 'keycloak-angular';
import {CookieService} from 'ngx-cookie-service';
import {MessageService} from 'primeng/api';
import {EncService} from '../services/enc.service';
import {ProfileService} from '../services/profile.service';

/**
 * check if user profile is updated
 */
@Injectable({
  providedIn: 'root'
})
export class ProfileUpdateGuard implements CanActivate {
  /**
   *
   * @param router contain router details
   * @param cookieService contain cookieService details
   * @param messageService contain messageService details
   * @param keycloakAngular contain keycloak details
   * @param encService contain encService details
   * @param profileService contain profileService details
   */
  constructor(private router: Router, private cookieService: CookieService,
    private messageService: MessageService, private keycloakAngular: KeycloakService,
    private encService: EncService, private profileService: ProfileService) {

  }

  /**
   * checks keycloak & profile service for login status,
   *
   * if logged in, calls checkUser functions
   *
   * if not, redirects to 401 page
   *
   * @param next contain next details
   *
   * @param state contain state details
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
      this.router.navigateByUrl('error/401');
      return Promise.resolve(false);
    });
  }

  /**
   * checks if user status is active & admin, continues if both are true
   *
   * else return respective error pages
   *
   * @param res contain res details
   */
  checkUser(res): Promise<boolean> {
    if (res && this.profileService.user && this.profileService.user.active && this.profileService.user.isAdmin) {
      return Promise.resolve(true);
    } else {
      if (this.profileService.user) {
        if (this.profileService.user.active) {
          if (!this.profileService.user.isAdmin) {
            this.router.navigateByUrl('error/403');
            this.messageService.add({severity: 'error', summary: 'You need to be a Admin to perform this action.', key: 'toast'});
          }
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Your Account is Inactive. Please Contact site Administrator.',
            key: 'toast'
          });
        }
      }
      return Promise.resolve(false);
    }
  }
}

