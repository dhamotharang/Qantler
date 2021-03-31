import { NgModule } from '@angular/core';
import { CitizenAffairTemplateComponent } from './components/citizen-affair-template.component';
import { CitizenAffairListComponent } from './container/component/citizen-affair-list/citizen-affair-list.component';
// import { CommonPageModule } from '../commonpage.module';
import { CitizenAffairCreateComponent } from './container/component/citizen-affair-create/citizen-affair-create.component';
import { ComplaintSuggestionComponent } from './container/component/complaints-suggestions/complaints-suggestions.component';
import { CitizenAffairRoutingModule } from './citizen-affair-routing.module';
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
import { TypeaheadModule } from 'ngx-bootstrap';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';
import { CitizenDocumentsComponent } from './container/citizen-documents/citizen-documents.component';
import { CreateComponent } from './container/create/create.component';
import { ListComponent } from './container/list/list.component';
import { CitizenAffairListComponentRTL } from './container/component/citizen-affair-list-rtl/citizen-affair-list.component-rtl';
import { CitizenAffairCreateComponentRTL } from './container/component/citizen-affair-create-rtl/citizen-affair-create.component-rtl';
import { ComplaintsSuggestionsRtlComponent } from './container/component/complaints-suggestions-rtl/complaints-suggestions-rtl.component';
import { ComplaintsSuggestionsCreateComponent } from './container/complaints-suggestions-create/complaints-suggestions-create.component';
@NgModule({
  declarations: [CitizenAffairTemplateComponent, CitizenAffairListComponent, CitizenAffairCreateComponent, CitizenAffairListComponentRTL, CitizenAffairCreateComponentRTL, ComplaintSuggestionComponent, CitizenDocumentsComponent, CreateComponent, ListComponent, ComplaintsSuggestionsRtlComponent, ComplaintsSuggestionsCreateComponent],
  imports: [
    // CommonPageModule,
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
    TypeaheadModule,
    CitizenAffairRoutingModule,
    SharedModule
  ]
})
export class CitizenAffairModule { }
