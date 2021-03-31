import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Injectable()
export class AdminGuard implements CanActivate  {
  constructor( private router: Router,public authService:AuthService) {

  }

 canActivate(
   next: ActivatedRouteSnapshot,
   state: RouterStateSnapshot): | Promise<boolean> | boolean {
     let currentUser:any = JSON.parse(localStorage.getItem('User'));
   if (!!currentUser.IsAdmin) {
     return true;
   } else {
     this.router.navigate(['/error']);
     return false;
   }
 }
}
