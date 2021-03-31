import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NewBabyAdditionRoutingModule } from './new-baby-addition-routing.module';
import { NewBabyAdditionComponent } from './container/new-baby-addition/new-baby-addition.component';
import { NewBabyAdditionCreateComponent } from './container/new-baby-addition-create/new-baby-addition-create.component';
import { NewBabyAdditionViewComponent } from './container/new-baby-addition-view/new-baby-addition-view.component';
import { NewBabyAdditionFormComponent } from './component/new-baby-addition-form/new-baby-addition-form.component';
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
import { NewBabyAdditionFormRtlComponent } from './component/new-baby-addition-form-rtl/new-baby-addition-form-rtl.component';

@NgModule({
  declarations: [NewBabyAdditionComponent, NewBabyAdditionCreateComponent, NewBabyAdditionViewComponent, NewBabyAdditionFormComponent, NewBabyAdditionFormRtlComponent],
  imports: [
    CommonModule,
    NewBabyAdditionRoutingModule,
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
export class NewBabyAdditionModule { }
