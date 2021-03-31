import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VehicleMgmtRoutingModule } from './vehicle-mgmt-routing.module';
import { VehicleMgmtComponent } from './container/vehicle-mgmt/vehicle-mgmt.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProgressbarModule, BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { SharedModule } from '../shared/shared.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { VehicleRequestComponent } from './component/vehicle-request/vehicle-request.component';
import { VehicleRouterComponent } from './container/vehicle-router/vehicle-router.component';
import { VehicleMgmtListComponent } from './container/vehicle-mgmt-list/vehicle-mgmt-list.component';
import { VehicleMgmtAssignComponent } from './container/vehicle-mgmt-assign/vehicle-mgmt-assign.component';
import { VehicleRequestContainerComponent } from './container/vehicle-request-container/vehicle-request-container.component';
import { VehicleReleaseConfirmationComponent } from './component/vehicle-release-confirmation/vehicle-release-confirmation.component';
import { VehicleReleaseConfirmationContainerComponent } from './container/vehicle-release-confirmation-container/vehicle-release-confirmation-container.component';
import { VehicleReturnConfirmationContainerComponent } from './container/vehicle-return-confirmation-container/vehicle-return-confirmation-container.component';
import { VehicleReturnConfirmationComponent } from './component/vehicle-return-confirmation/vehicle-return-confirmation.component';
import { VehicleReturnFormComponent } from './component/vehicle-return-form/vehicle-return-form.component';
import { VehicleReturnFormContainerComponent } from './container/vehicle-return-form-container/vehicle-return-form-container.component';
import { VehicleMgmtRentCarComponent } from './container/vehicle-mgmt-rent-car/vehicle-mgmt-rent-car.component';
import { VehicleMgmtServiceService } from './service/vehicle-mgmt-service.service';
import { VehicleListComponent } from './container/vehicle-list/vehicle-list.component';
import { VehicleDetailsFormComponent } from './component/vehicle-details-form/vehicle-details-form.component';
import { VehicleDetailsCreateComponent } from './container/vehicle-details-create/vehicle-details-create.component';
import { VehicleDetailsViewComponent } from './container/vehicle-details-view/vehicle-details-view.component';
import { VehicleReleaseFormComponent } from './component/vehicle-release-form/vehicle-release-form.component';
import { VehicleReleaseFormContainerComponent } from './container/vehicle-release-form-container/vehicle-release-form-container.component';
import { PopupModule } from 'src/app/modal/popup.module';
import { VehicleRequestViewComponent } from './container/vehicle-request-view/vehicle-request-view.component';
import { VehicleServiceLogComponent } from './container/vehicle-service-log/vehicle-service-log/vehicle-service-log.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
@NgModule({
  declarations: [VehicleMgmtListComponent, VehicleMgmtComponent, VehicleRequestComponent, VehicleRouterComponent, VehicleMgmtAssignComponent, VehicleRequestContainerComponent, VehicleReleaseConfirmationComponent, VehicleReleaseConfirmationContainerComponent, VehicleReturnConfirmationContainerComponent, VehicleReturnConfirmationComponent, VehicleReturnFormComponent, VehicleReturnFormComponent, VehicleReturnFormContainerComponent, VehicleMgmtRentCarComponent,VehicleListComponent, VehicleDetailsFormComponent, VehicleReleaseFormComponent, VehicleReleaseFormContainerComponent, VehicleDetailsFormComponent, VehicleDetailsCreateComponent, VehicleDetailsViewComponent, VehicleRequestViewComponent, VehicleServiceLogComponent],
  imports: [
    CommonModule,
    VehicleMgmtRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    ProgressbarModule,
    NgbPaginationModule,
    BsDatepickerModule,
    NgxDatatableModule,
    SharedModule,
    ModalModule,
    NgSelectModule,
    PopupModule,
    PdfViewerModule
  ],
  providers:[VehicleMgmtServiceService],
})
export class VehicleMgmtModule { }
