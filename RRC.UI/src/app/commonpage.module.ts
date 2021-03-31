import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxEditorModule } from 'ngx-editor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { Ng2TableModule } from 'ngx-datatable/ng2-table';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { TagInputModule } from 'ngx-chips';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // this is needed!
import { DatePipe } from '@angular/common';
import { NgxTinymceModule } from 'ngx-tinymce';
import { EditorModule } from '@tinymce/tinymce-angular';
import { BsModalService, BsModalRef, ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { TypeaheadModule } from 'ngx-bootstrap';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { BrowserModule } from '@angular/platform-browser';
import { PopupModule } from './modal/popup.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
    // HttpClientModule,
    // HttpModule,
    // BrowserModule,
    SelectDropDownModule,
    BsDatepickerModule.forRoot(),
    NgxEditorModule,
    Ng2TableModule,
    NgxDatatableModule,
    NgxPaginationModule,
    NgbPaginationModule,
    ReactiveFormsModule,
    TagInputModule,
    // BrowserAnimationsModule,
    EditorModule,
    ModalModule.forRoot(),
    NgxTinymceModule.forRoot({
      baseURL: 'assets/tinymce/',
    }),
    NgSelectModule,
    AccordionModule.forRoot(),
    TypeaheadModule.forRoot(),
    PdfViewerModule
  ],
  exports:[
    CommonModule,
    FormsModule,
    // BrowserModule,
    BsDatepickerModule,
    NgbPaginationModule,
    NgxEditorModule,
    Ng2TableModule,
    NgxDatatableModule,
    NgxPaginationModule,
    ReactiveFormsModule,
    TagInputModule,
    // BrowserAnimationsModule,
    EditorModule,
    ModalModule,
    NgxTinymceModule,
    NgSelectModule,
    AccordionModule,
    // HttpClientModule,
    // HttpModule,
    SelectDropDownModule,
    TypeaheadModule,
    PdfViewerModule
  ],
  providers: [DatePipe,BsModalRef,BsModalService],
})
export class CommonPageModule { }
