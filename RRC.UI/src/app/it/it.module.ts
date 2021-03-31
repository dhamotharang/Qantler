import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ItRoutingModule } from './it-routing.module';
import { HomePageComponent } from './container/home-page/home-page.component';
import { ItComponent } from './container/it/it.component';
import { SharedModule } from '../shared/shared.module';
import { ProgressbarModule, BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { ItRequestComponent } from './component/it-request/it-request.component';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { ItRequestCreateComponent } from './container/it-request-create/it-request-create.component';
import { ItRequestViewComponent } from './container/it-request-view/it-request-view.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ItService } from './service/it.service';
import { ItDocumentsComponent } from './container/it-documents/it-documents.component';
import { ItRequestRtlComponent } from './component/it-request-rtl/it-request-rtl.component';

@NgModule({
  declarations: [HomePageComponent, ItComponent, ItRequestComponent, ItRequestCreateComponent, ItRequestViewComponent,ItDocumentsComponent, ItRequestRtlComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ItRoutingModule,
    SharedModule,
    ProgressbarModule,
    BsDatepickerModule,
    ModalModule,
    SelectDropDownModule,
    NgSelectModule,
    NgxDatatableModule,
    NgbPaginationModule
  ],
  providers:[ItService]
})
export class ItModule { }
