import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AnnouncementRoutingModule } from './announcement-routing.module';
import { AnnouncementComponent } from './container/announcement/announcement.component';
import { AnnouncementCreateComponent } from './container/announcement-create/announcement-create.component';
import { AnnouncementViewComponent } from './container/announcement-view/announcement-view.component';
import { AnnouncementFormComponent } from './component/announcement-form/announcement-form.component';

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
import { AnnouncementFormRtlComponent } from './component/announcement-form-rtl/announcement-form-rtl.component';

@NgModule({
  declarations: [
    AnnouncementComponent,
    AnnouncementCreateComponent,
    AnnouncementViewComponent,
    AnnouncementFormComponent,
    AnnouncementFormRtlComponent
  ],
  imports: [
    CommonModule,
    AnnouncementRoutingModule,
    SelectDropDownModule,
    FormsModule,
    BsDatepickerModule.forRoot(),
    NgbPaginationModule,
    NgxEditorModule,
    NgxDatatableModule,
    NgxPaginationModule,
    NgbPaginationModule,
    TagInputModule,
    ReactiveFormsModule,
    EditorModule,
    ModalModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    NgxTinymceModule.forRoot({
      baseURL: 'assets/tinymce/',
    }),
    NgSelectModule,
    AccordionModule.forRoot(),
    SharedModule
  ]
})
export class AnnouncementModule { }
