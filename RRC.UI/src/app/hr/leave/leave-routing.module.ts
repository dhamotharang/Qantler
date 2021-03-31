import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LeaveComponent } from './container/leave/leave.component';
import { LeaveRequestCreateComponent } from './container/leave-request-create/leave-request-create.component';
import { LeaveRequestViewComponent } from './container/leave-request-view/leave-request-view.component';

const routes: Routes = [
  {
    path:'', component:LeaveComponent,
    children:[
      // {path:"", redirectTo:'hr/dashboard', pathMatch:'full'},
      {path:'request-create',component:LeaveRequestCreateComponent, data : {title:'Creation'}},
      {path:'request-view/:id',component:LeaveRequestViewComponent, data : {title:'View'}},
      // {path:'request-create-rtl',component:LeaveRequestCreateComponent, data:{ dir: 'rtl' }},
      // {path:'request-view-rtl/:id',component:LeaveRequestViewComponent, data:{ dir: 'rtl' }},
      {path:"**", redirectTo:'/app/hr/dashboard', pathMatch:'full'}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LeaveRoutingModule { }
