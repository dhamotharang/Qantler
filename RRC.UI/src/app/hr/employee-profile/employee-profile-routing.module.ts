import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmployeeComponent } from './container/employee/employee.component';
import { EmployeeProfileCreateComponent } from './container/employee-profile-create/employee-profile-create.component';
import { EmployeeProfileViewComponent } from './container/employee-profile-view/employee-profile-view.component';
import { EmployeeListComponent } from './component/employee-list/employee-list.component';
import { EmployeeProfileEditComponent } from './container/employee-profile-edit/employee-profile-edit.component';
import { EmployeeProfileGuard } from '../guard/employee-profile/employee-profile.guard';

const routes: Routes = [
  {
    path:'', component:EmployeeComponent,
    children:[
      { path: '', redirectTo: 'list'},
      { path:'create', component:EmployeeProfileCreateComponent, data:{mode:'create'}, canActivate: [EmployeeProfileGuard] },
      { path:'view/:id', component:EmployeeProfileViewComponent, data:{mode:'view'}, canActivate: [EmployeeProfileGuard] },
      { path:'list', component:EmployeeListComponent, data:{mode:'list'}, canActivate: [EmployeeProfileGuard] },
      { path:'edit/:id/:from', component:EmployeeProfileEditComponent, data:{mode:'edit'}, canActivate: [EmployeeProfileGuard] }
    ]
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeProfileRoutingModule { }
