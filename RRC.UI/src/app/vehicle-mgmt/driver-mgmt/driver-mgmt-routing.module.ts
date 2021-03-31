import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DriverMgmtContainerComponent } from './container/driver-mgmt-container/driver-mgmt-container.component';
import { DriverMgmtRouterComponent } from './container/driver-mgmt-router/driver-mgmt-router.component';
import { ManageDriveComponent } from './component/manage-drive/manage-drive.component';

const routes: Routes = [{
  path: '', component: DriverMgmtRouterComponent, children: [
    { path: '', redirectTo: 'driver-management', pathMatch:'full' },
    { path: 'driver-management', component: DriverMgmtContainerComponent },
    { path: 'manage-driver', component: ManageDriveComponent }
    
  ]
}];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DriverMgmtRoutingModule { }
