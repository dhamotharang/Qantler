import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { LeaveRoutingModule } from './leave-routing.module';
import { LeaveComponent } from './container/leave/leave.component';
import { LeaveRequestCreateComponent } from './container/leave-request-create/leave-request-create.component';
import { LeaveRequestViewComponent } from './container/leave-request-view/leave-request-view.component';
import { LeaveRequestFormComponent } from './component/leave-request-form/leave-request-form.component';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxEditorModule } from 'ngx-editor';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxPaginationModule } from 'ngx-pagination';

import { TagInputModule } from 'ngx-chips';
import { EditorModule } from '@tinymce/tinymce-angular';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgxTinymceModule } from 'ngx-tinymce';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from 'src/app/shared/shared.module';
import { LeaveService } from './service/leave.service';
import { LeaveRequestFormRtlComponent } from './component/leave-request-form-rtl/leave-request-form-rtl.component';

@NgModule({
  declarations: [LeaveComponent, LeaveRequestCreateComponent, LeaveRequestViewComponent, LeaveRequestFormComponent, LeaveRequestFormRtlComponent],
  imports: [
    CommonModule,
    LeaveRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SelectDropDownModule,
    BsDatepickerModule,
    NgxDatatableModule,
    NgbPaginationModule,
    ModalModule,
    NgMultiSelectDropDownModule,
    NgSelectModule,
    SharedModule
  ],
  providers:[LeaveService]
})
export class LeaveModule { }
