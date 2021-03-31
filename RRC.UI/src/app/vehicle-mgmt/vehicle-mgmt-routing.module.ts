import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VehicleMgmtListComponent } from './container/vehicle-mgmt-list/vehicle-mgmt-list.component';
import { VehicleRouterComponent } from './container/vehicle-router/vehicle-router.component';
import { VehicleRequestContainerComponent } from './container/vehicle-request-container/vehicle-request-container.component';
import { VehicleReleaseConfirmationContainerComponent } from './container/vehicle-release-confirmation-container/vehicle-release-confirmation-container.component';
import { VehicleReturnConfirmationContainerComponent } from './container/vehicle-return-confirmation-container/vehicle-return-confirmation-container.component';
import { VehicleReturnFormContainerComponent } from './container/vehicle-return-form-container/vehicle-return-form-container.component';
import { VehicleMgmtAssignComponent } from './container/vehicle-mgmt-assign/vehicle-mgmt-assign.component';
import { VehicleMgmtRentCarComponent } from './container/vehicle-mgmt-rent-car/vehicle-mgmt-rent-car.component';
import { VehicleListComponent } from './container/vehicle-list/vehicle-list.component';
import { VehicleDetailsFormComponent } from './component/vehicle-details-form/vehicle-details-form.component';
import { VehicleDetailsViewComponent } from './container/vehicle-details-view/vehicle-details-view.component';
import { VehicleDetailsCreateComponent } from './container/vehicle-details-create/vehicle-details-create.component';
import { VehicleReleaseFormContainerComponent } from './container/vehicle-release-form-container/vehicle-release-form-container.component';
import { VehicleRequestViewComponent } from './container/vehicle-request-view/vehicle-request-view.component';
import { DocumentsPageComponent } from '../shared/component/documents-page/documents-page.component';
import { VehicleServiceLogComponent } from './container/vehicle-service-log/vehicle-service-log/vehicle-service-log.component';


const routes: Routes = [{
  path: '', component: VehicleRouterComponent, children: [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'dashboard', component: VehicleMgmtListComponent },
    { path: 'vehicle-request', component: VehicleRequestContainerComponent },
    { path: 'vehicle-release-confirmation/:id', component: VehicleReleaseConfirmationContainerComponent },
    { path: 'vehicle-return-confirmation/:id', component: VehicleReturnConfirmationContainerComponent },
    { path: 'vehicle-return-form/:id', component: VehicleReturnFormContainerComponent },
    { path: 'vehicle-release-form/:id', component: VehicleReleaseFormContainerComponent },    
    { path: 'vehicle-assign/:id', component: VehicleMgmtAssignComponent },
    { path: 'fine-management', loadChildren: './fine-mgmt/fine-mgmt.module#FineMgmtModule' },
    { path: 'rent-car', component: VehicleMgmtRentCarComponent },
    { path: 'vehicle-list', component: VehicleListComponent},
    { path: 'vehicle-create', component: VehicleDetailsFormComponent},
    { path: 'vehicle-view/:id', component: VehicleDetailsFormComponent},
    { path: 'driver-management', loadChildren: './driver-mgmt/driver-mgmt.module#DriverMgmtModule' },
    { path:'vehicle-details-create', component: VehicleDetailsCreateComponent},
    { path:'vehicle-details-view/:id', component: VehicleDetailsViewComponent},
    { path:'vehicle-request-view/:id', component: VehicleRequestViewComponent},
    { path:'vehicle-servicelog-view', component: VehicleServiceLogComponent},
    { path:'documents',component:DocumentsPageComponent, data: { documentType: 'Vehicle', department:'Vehicle Management'}},
  ]
}];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VehicleMgmtRoutingModule { }
