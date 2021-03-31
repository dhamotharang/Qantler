import { NgModule } from '@angular/core';
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
import { CommonModule } from '@angular/common';
// import { CommonPageModule } from '../commonpage.module';
import { CircularListComponent } from './container/component/circular-list/circular-list.component';
import { IncomingCircularFormComponent } from './container/component/incoming-circular-form/incoming-circular-form.component';
import { CircularListComponentRTL } from './container/component/circular-list-rtl/circular-list.component-rtl';
import { IncomingCircularFormComponentRTL } from './container/component/incoming-circular-form-rtl/incoming-circular-form.component-rtl';
import { CircularTemplateComponent } from './components/circular-template.component';
import { CircularRoutingModule } from './circular-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CreateComponent } from './container/create/create.component';
import { ListComponent } from './container/list/list.component';

@NgModule({
  declarations: [
    CircularTemplateComponent,
    CircularListComponent,
    CircularListComponentRTL,
    IncomingCircularFormComponent,
    IncomingCircularFormComponentRTL,
    CreateComponent,
    ListComponent
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
    CircularRoutingModule, SharedModule
  ]
})
export class CircularModule { }