import { NgModule } from '@angular/core';
// import { CommonPageModule } from '../commonpage.module';
import { IncomingLetterFormComponent } from './container/component/incoming-letter-form/incoming-letter-form.component';
import { OutgoingLetterFormComponent } from './container/component/outgoing-letter-form/outgoing-letter-form.component';
import { LetterListComponent } from './container/component/letter-list/letter-list.component';
import { LetterTemplateComponent } from './components/letter-template.component';
import { LetterRoutingModule } from './letter-routing.module';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { SharedModule } from '../shared/shared.module';
import { CreateComponent } from './container/create/create.component';
import { ListComponent } from './container/list/list.component';
import { IncomingLetterFormRtlComponent } from './container/component/incoming-letter-form-rtl/incoming-letter-form-rtl.component';
import { OutgoingLetterFormRtlComponent } from './container/component/outgoing-letter-form-rtl/outgoing-letter-form-rtl.component';
import { LetterListRtlComponent } from './container/component/letter-list-rtl/letter-list-rtl.component';
import { OutComponent } from './container/out/out.component';
import { LinkToModalComponent } from './link-to-modal/link-to-modal.component';

@NgModule({
  declarations: [
    LetterTemplateComponent,
    LetterListComponent,
    IncomingLetterFormComponent,
    OutgoingLetterFormComponent,
    CreateComponent,
    ListComponent,
    IncomingLetterFormRtlComponent,
    OutgoingLetterFormRtlComponent,
    LetterListRtlComponent,
    OutComponent,
    LinkToModalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SelectDropDownModule,
    BsDatepickerModule.forRoot(),
    NgbPaginationModule,
    NgxEditorModule,
    NgxDatatableModule,
    NgxPaginationModule,
    NgbPaginationModule,
    TagInputModule,
    EditorModule,
    ModalModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    NgxTinymceModule.forRoot({
      baseURL: 'assets/tinymce/',
    }),
    NgSelectModule,
    AccordionModule.forRoot(),
    PdfViewerModule,
    LetterRoutingModule, SharedModule
  ],
  entryComponents: [LinkToModalComponent]
})
export class LetterModule { }