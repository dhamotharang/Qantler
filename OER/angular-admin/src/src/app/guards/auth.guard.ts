import {Injectable} from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot} from '@angular/router';
import {KeycloakAuthGuard, KeycloakService} from 'keycloak-angular';
import {MessageService} from 'primeng/api';
import {ProfileService} from '../services/profile.service';

/**
 * allows authenticated user to continue
 *
 *  extends KeycloakAuthGuard
 */
@Injectable()
export class AuthGuard extends KeycloakAuthGuard {
  /**
   *
   * @param router contain router details
   * @param keycloakAngular contain keycloakAngular details
   * @param messageService contain messageService details
   * @param profileService contain profileService details
   */
  constructor(protected router: Router, protected keycloakAngular: KeycloakService,
    private messageService: MessageService, private profileService: ProfileService) {
    super(router, keycloakAngular);
  }

  /**
   * if not authenticated sets user data in profileService to null and show error message
   *
   * @param route contain route details
   *
   * @param state contain state details
   */
  isAccessAllowed(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve, reject) => {
      if (!this.authenticated) {
        this.profileService.setUser(null);
        this.messageService.add({severity: 'error', summary: 'Please Sign in to Continue', key: 'toast'});
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
