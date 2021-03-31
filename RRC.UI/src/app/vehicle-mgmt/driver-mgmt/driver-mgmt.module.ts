import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DriverMgmtRoutingModule } from './driver-mgmt-routing.module';
import { DriverMgmtComponent } from './component/driver-mgmt/driver-mgmt.component';
import { DriverMgmtContainerComponent } from './container/driver-mgmt-container/driver-mgmt-container.component';
import { DriverMgmtRouterComponent } from './container/driver-mgmt-router/driver-mgmt-router.component';
import { ManageDriveComponent } from './component/manage-drive/manage-drive.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProgressbarModule, BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { FullCalendarModule } from '@fullcalendar/angular';
import { PdfViewerModule } from 'ng2-pdf-viewer';

@NgModule({
  declarations: [DriverMgmtComponent, DriverMgmtContainerComponent, DriverMgmtRouterComponent, ManageDriveComponent],
  imports: [
    CommonModule,
    DriverMgmtRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    ProgressbarModule,
    NgbPaginationModule,
    BsDatepickerModule,
    NgxDatatableModule,
    SharedModule,
    ModalModule,
    NgSelectModule,
    FullCalendarModule,
    PdfViewerModule    
  ]
})
export class DriverMgmtModule { }
