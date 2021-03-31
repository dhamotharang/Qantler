import {Injectable} from '@angular/core';
import {Router, ActivatedRouteSnapshot, RouterStateSnapshot, ActivatedRoute} from '@angular/router';
import {KeycloakService, KeycloakAuthGuard} from 'keycloak-angular';
import {ProfileService} from '../services/profile.service';
import {MessageService} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import {CookieService} from 'ngx-cookie-service';
import {environment} from '../../environments/environment';

/**
 * Authentication Guard
 */
@Injectable()
export class AuthGuard extends KeycloakAuthGuard {
  /**
   *
   * @param router
   * @param keycloakAngular
   * @param messageService
   * @param translate
   * @param profileService
   * @param cookieService
   */
  constructor(protected router: Router, protected keycloakAngular: KeycloakService,
              private messageService: MessageService, private translate: TranslateService,
              private profileService: ProfileService, private cookieService: CookieService) {
    super(router, keycloakAngular);
  }

  /**
   * Check authentication status , returns error message if not
   *
   * @param route
   * @param state
   */
  isAccessAllowed(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve, reject) => {
      if (!this.authenticated) {
        this.profileService.setUser(null);
        if (route && route.url[0] && (route.url[0].path === 'resources' || route.url[0].path === 'courses') && route.url[1] && route.url[1].path === 'create') {
          const url = '/' + route.routeConfig.path;
          this.router.navigate(['/']);
          this.keycloakAngular.login({
            kcLocale: this.cookieService.get('manaraLang') ? this.cookieService.get('manaraLang') : 'en',
            redirectUri: url
          });
        } else {
          this.translate.get('Please sign in to Continue').subscribe((trans) => {
            this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
          });
        }
        return;
      }

      const requiredRoles = route.data.roles;
      if (!requiredRoles || requiredRoles.length === 0) {
        return resolve(true);
      } else {
        if (!this.roles || this.roles.length === 0) {
          resolve(false);
        }
        let granted = false;
        for (const requiredRole of requiredRoles) {
          if (this.roles.indexOf(requiredRole) > -1) {
            granted = true;
            break;
          }
        }
        resolve(granted);
      }
    });
  }
}
