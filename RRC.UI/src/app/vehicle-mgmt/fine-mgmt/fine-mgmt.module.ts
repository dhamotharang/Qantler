import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';

import { FineMgmtRoutingModule } from './fine-mgmt-routing.module';
import { FineMgmtListComponent } from './container/fine-mgmt-list/fine-mgmt-list.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from 'src/app/shared/shared.module';
import { LogFineComponent } from './component/log-fine/log-fine.component';
import { FineManagementComponent } from './container/fine-management/fine-management.component';

@NgModule({
  declarations: [
    FineMgmtListComponent,
    LogFineComponent,
    FineManagementComponent
  ],
  imports: [
    CommonModule,
    FineMgmtRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    NgbPaginationModule,
    NgxDatatableModule,
    NgxPaginationModule,
    ModalModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    NgSelectModule,
    SharedModule
  ]
})
export class FineMgmtModule { }
