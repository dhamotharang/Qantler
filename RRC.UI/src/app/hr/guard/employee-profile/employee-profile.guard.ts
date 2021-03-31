import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Injectable()
export class EmployeeProfileGuard implements  CanActivate{
  employeeId:any;  
  // currentUser:any  = JSON.parse(localStorage.getItem('User'));
  // isHRDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 9;
  constructor( private router: Router,public authService:AuthService) {

  }

 canActivate(
   next: ActivatedRouteSnapshot,
   state: RouterStateSnapshot): | Promise<boolean> | boolean {
     let User = JSON.parse(localStorage.getItem('User'));
     let isHRDepartmentUserID =  User.OrgUnitID == 9;
    if(next.data.mode == 'view' || next.data.mode == 'edit' ){
      if(!isHRDepartmentUserID && User.id != next.params.id ){
        this.router.navigate(['/error']);
        return false;
      }else{ 
        return true;
      }
    } else if(next.data.mode == 'create'){
      this.router.navigate(['/error']);
      return false;
    } else if( next.data.mode == 'list'){
      if(!isHRDepartmentUserID){
        this.router.navigate(['/error']);
        return false;
      }else{
        return true;
      }
    }
 }
}
