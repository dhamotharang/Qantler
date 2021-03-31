import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TrainingRequestRoutingModule } from './training-request-routing.module';
import { TrainingRequestComponent } from './container/training-request/training-request.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxEditorModule } from 'ngx-editor';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxPaginationModule } from 'ngx-pagination';
import { TagInputModule } from 'ngx-chips';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from 'src/app/shared/shared.module';
import { TrainingRequestFormComponent } from './component/training-request-form/training-request-form.component';
import { TrainingRequestCreateComponent } from './container/training-request-create/training-request-create.component';
import { TrainingRequestFormRtlComponent } from './component/training-request-form-rtl/training-request-form-rtl.component';

@NgModule({
  declarations: [TrainingRequestComponent, TrainingRequestFormComponent, TrainingRequestCreateComponent, TrainingRequestFormRtlComponent],
  imports: [
    CommonModule,
    TrainingRequestRoutingModule,
    FormsModule,
    SelectDropDownModule,
    BsDatepickerModule.forRoot(),
    NgbPaginationModule,
    NgxEditorModule,
    NgxDatatableModule,
    NgxPaginationModule,
    NgbPaginationModule,
    ReactiveFormsModule,
    TagInputModule,
    ModalModule.forRoot(),
    NgSelectModule,
    SharedModule
  ]
})
export class TrainingRequestModule { }
