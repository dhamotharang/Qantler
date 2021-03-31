import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './container/admin/admin.component';
import { AdminSettingsComponent } from './container/admin-settings/admin-settings.component';
import { MasterAdminSettingsComponent } from './container/master-admin-settings/master-admin-settings.component';

const routes: Routes = [
  {
    path:'', component:AdminComponent,
    children:[
      { path:'', redirectTo:'admin-settings', pathMatch:'full' },
      { path:'admin-settings', component:MasterAdminSettingsComponent},
      { path:'**', redirectTo:'/home', pathMatch:'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
