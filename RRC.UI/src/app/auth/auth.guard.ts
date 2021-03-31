import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor( private router: Router,public authService:AuthService) {

   }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): | Promise<boolean> | boolean {
    if (!!localStorage.getItem('token')) {
      return true;
    } else {
      this.authService.returnURL = state.url;
      this.router.navigate(['/login']);
      return false;
    }
  }
}