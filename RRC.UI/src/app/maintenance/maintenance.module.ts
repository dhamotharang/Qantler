import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { SelectDropDownModule } from 'ngx-select-dropdown';

import { MaintenanceRoutingModule } from './maintenance-routing.module';
import { MaintenanceHomeComponent } from './components/maintenance-home/maintenance-home.component';
import { MaintenanceFormComponent } from './components/maintenance-form/maintenance-form.component';
import { MaintenanceContainerComponent } from './containers/maintenance-container/maintenance-container.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [MaintenanceHomeComponent, MaintenanceFormComponent, MaintenanceContainerComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    AccordionModule.forRoot(),
    NgSelectModule,
    NgbPaginationModule,
    NgxDatatableModule,
    SelectDropDownModule,
    SharedModule,
    MaintenanceRoutingModule
  ]
})
export class MaintenanceModule { }
