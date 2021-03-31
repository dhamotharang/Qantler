import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GiftsManagementComponent } from './container/gifts-management/gifts-management.component';
import { GiftsManagementHomeComponent } from './container/gifts-management-home/gifts-management-home.component';
import { GiftRequestCreateComponent } from './container/gift-request-create/gift-request-create.component';
import { GiftRequestViewComponent } from './container/gift-request-view/gift-request-view.component';

const routes: Routes = [
  {
    path:'', component:GiftsManagementComponent,
    children:[
      { path:'', redirectTo:'dashboard', pathMatch:'full' },
      { path: 'dashboard', component: GiftsManagementHomeComponent },
      { path: 'request-create', component: GiftRequestCreateComponent },
      { path: 'request-view/:id', component: GiftRequestViewComponent},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GiftsManagementRoutingModule { }
