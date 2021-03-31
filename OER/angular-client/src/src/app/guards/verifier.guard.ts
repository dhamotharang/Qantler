import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {KeycloakService} from 'keycloak-angular';
import {CookieService} from 'ngx-cookie-service';
import {MessageService} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';

/**
 * Verifier Guard Check
 */
@Injectable({
  providedIn: 'root'
})
export class VerifierGuard implements CanActivate {
  /**
   *
   * @param keycloak
   * @param messageService
   * @param translate
   */
  constructor(private keycloak: KeycloakService, private messageService: MessageService, private translate: TranslateService) {
  }

  /**
   *
   * @param next
   * @param state
   */
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.checkUser();
  }

  /**
   * check if user is a verifier
   */
  checkUser(): boolean {
    let isVerifier = false;
    this.keycloak.getUserRoles(true).forEach((item) => {
      if (item === 'Verifier') {
        isVerifier = true;
      }
    });
    return isVerifier;
  }
}
