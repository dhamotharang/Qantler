import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SalaryCertificateRoutingModule } from './salary-certificate-routing.module';
import { SalaryComponent } from './container/salary/salary.component';
import { SalaryCertificateCreateComponent } from './container/salary-certificate-create/salary-certificate-create.component';
import { SalaryCertificateViewComponent } from './container/salary-certificate-view/salary-certificate-view.component';
import { SalaryCertificateComponent } from './component/salary-certificate/salary-certificate.component';
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
import { SalarycertificateService } from './service/salarycertificate.service';

@NgModule({
  declarations: [SalaryComponent, SalaryCertificateCreateComponent, SalaryCertificateViewComponent, SalaryCertificateComponent],
  imports: [
    CommonModule,
    SalaryCertificateRoutingModule,
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
    SalarycertificateService
  ]
})
export class SalaryCertificateModule { }
