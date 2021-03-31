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
import { MediaRoutingModule } from './media-routing.module';
import { MediaTemplateComponent } from './components/media-template.component';
import { MediaRequestListComponent } from './container/media-request-list/media-request-list.component';
import { MediaRequestStaffListComponent } from './container/media-request-staff-list/media-request-staff-list.component';
import { MediaRequestDesignFormComponent } from './container/component/media-request-design-form/media-request-design-form.component';
import { MediaRequestPhotoComponent } from './container/component/media-request-photo/media-request-photo.component';
import { MediaProtocolRequestsComponent } from './container/media-protocol-requests/media-protocol-requests.component';
import { SharedModule } from '../shared/shared.module';
import { MediaRequestDesignFormComponentRTL } from './container/component/media-request-design-form-rtl/media-request-design-form.component-rtl';
import { MediaRequestDesignComponent } from './container/media-request-design/media-request-design.component';
import { MediaRequestPhotoRtlComponent } from './container/component/media-request-photo-rtl/media-request-photo-rtl.component';
import { MediaRequestPhotoCreateComponent } from './container/media-request-photo-create/media-request-photo-create.component';
import { ProtocolHomepageComponent } from './container/protocol-homepage/protocol-homepage.component';

@NgModule({
  declarations: [
    MediaTemplateComponent,
    MediaRequestListComponent,
    MediaRequestStaffListComponent,
    MediaRequestDesignFormComponent,
    MediaRequestPhotoComponent,
    MediaProtocolRequestsComponent,
    MediaRequestDesignFormComponentRTL,
    MediaRequestDesignComponent,
    MediaRequestPhotoRtlComponent,
    MediaRequestPhotoCreateComponent,
    ProtocolHomepageComponent
  ],
  imports:[
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
    MediaRoutingModule,
    SharedModule
  ],
  providers: [],

})
export class MediaModule{}
