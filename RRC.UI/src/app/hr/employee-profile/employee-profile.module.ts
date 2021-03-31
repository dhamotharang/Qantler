import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeeProfileRoutingModule } from './employee-profile-routing.module';
import { BsDatepickerModule, AccordionModule, ModalModule } from 'ngx-bootstrap';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { TagInputModule } from 'ngx-chips';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { EditorModule } from '@tinymce/tinymce-angular';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxEditorModule } from 'ngx-editor';
import { NgxPaginationModule } from 'ngx-pagination';
import { EmployeeComponent } from './container/employee/employee.component';
import { EmployeeProfileCreateComponent } from './container/employee-profile-create/employee-profile-create.component';
import { EmployeeProfileViewComponent } from './container/employee-profile-view/employee-profile-view.component';
import { EmployeeProfileComponent } from './component/employee-profile/employee-profile.component';
import { EmployeeService } from './service/employee.service';
import { EmployeeListComponent } from './component/employee-list/employee-list.component';
import { EmployeeProfileEditComponent } from './container/employee-profile-edit/employee-profile-edit.component';
import { EmployeeProfileGuard } from '../guard/employee-profile/employee-profile.guard';

@NgModule({
  declarations: [EmployeeComponent, EmployeeProfileCreateComponent, EmployeeProfileViewComponent, EmployeeProfileComponent, EmployeeListComponent, EmployeeProfileEditComponent],
  imports: [
    CommonModule,
    EmployeeProfileRoutingModule,
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
    EditorModule,
    ModalModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    NgSelectModule,
    AccordionModule.forRoot(),
    SharedModule
  ],
  providers:[
    EmployeeService,
    EmployeeProfileGuard
  ]
})
export class EmployeeProfileModule { }
