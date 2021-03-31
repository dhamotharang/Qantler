import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router} from '@angular/router';
import {Observable} from 'rxjs';
import {DeviceDetectorService} from 'ngx-device-detector';

@Injectable({
  providedIn: 'root'
})
export class BrowserCheckGuard implements CanActivate {
  constructor(private BrowserCheck: DeviceDetectorService, private router: Router) {

  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (
      (this.BrowserCheck.browser.toLowerCase().includes('chrome') && this.BrowserCheck.browser_version.split('.')[0] > '55') ||
      (this.BrowserCheck.browser.toLowerCase().includes('firefox') && this.BrowserCheck.browser_version.split('.')[0] > '51') ||
      (this.BrowserCheck.browser.toLowerCase().includes('edge') && this.BrowserCheck.browser_version.split('.')[0] > '15') ||
      (this.BrowserCheck.browser.toLowerCase().includes('safari') && this.BrowserCheck.browser_version.split('.')[0] > '10')) {
      return true;
    } else {
      this.router.navigateByUrl('not-supported');
      return false;
    }
  }

}
