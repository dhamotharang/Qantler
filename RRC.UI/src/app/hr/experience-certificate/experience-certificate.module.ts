import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
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

import { ExperienceCertificateRoutingModule } from './experience-certificate-routing.module';
import { SalarycertificateService } from '../salary-certificate/service/salarycertificate.service';
import { ExperienceCertificateComponent } from './component/experience-certificate/experience-certificate.component';
import { ExperienceViewComponent } from './container/experience-view/experience-view.component';
import { ExperienceCreateComponent } from './container/experience-create/experience-create.component';
import { ExperienceComponent } from './container/experience/experience.component';

@NgModule({
  declarations: [ExperienceComponent, ExperienceCreateComponent, ExperienceViewComponent, ExperienceCertificateComponent],
  imports: [
    CommonModule,
    ExperienceCertificateRoutingModule,
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
export class ExperienceCertificateModule { }
