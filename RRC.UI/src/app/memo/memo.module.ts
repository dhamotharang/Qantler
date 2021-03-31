import { NgModule } from '@angular/core';
import { MemoListComponent } from './container/component/memo-list/memo-list.component';
import { CreateMemoComponent } from './container/component/create-memo/create-memo.component';
import { MemoTemplateComponent } from './components/memo-template.component';
import { MemoRoutingModule } from './memo-routing.module';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxEditorModule } from 'ngx-editor';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { TagInputModule } from 'ngx-chips';
// import { EditorModule } from '@tinymce/tinymce-angular';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgxTinymceModule } from 'ngx-tinymce';
import { NgSelectModule } from '@ng-select/ng-select';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { SharedModule } from '../shared/shared.module';
import { CreateComponent } from './container/create/create.component';
import { ListComponent } from './container/list/list.component';
import { CreateMemoComponentRTL } from "./container/component/create-memo-rtl/create-memo.component-rtl";
import { MemoListComponentRTL } from './container/component/memo-list-rtl/memo-list.component-rtl';
@NgModule({
  declarations: [
    MemoTemplateComponent,
    CreateMemoComponent,
    MemoListComponent,
    CreateComponent,
    ListComponent,
    CreateMemoComponentRTL,
    MemoListComponentRTL
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
    // EditorModule,
    ModalModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    NgxTinymceModule.forRoot({
      baseURL: 'assets/tinymce/',
    }),
    NgSelectModule,
    AccordionModule.forRoot(),
    PdfViewerModule,
    MemoRoutingModule,
    SharedModule
  ]
})
export class MemoModule { }