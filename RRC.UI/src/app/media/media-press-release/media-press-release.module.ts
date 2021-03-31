import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
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

import { MediaPressReleaseRoutingModule } from './media-press-release-routing.module';
import { MediaPressReleaseComponent } from './components/media-press-release/media-press-release.component';
import { MediaPressReleaseRequestComponent } from './container/media-press-release-request/media-press-release-request.component';
import { MediaPressReleaseViewComponent } from './container/media-press-release-view/media-press-release-view.component';
import { MediaPressReleaseContainerComponent } from './container/media-press-release-container/media-press-release-container.component';
import { MediapressService } from './service/mediapress.service';
import { MediaPressReleaseEditComponent } from './container/media-press-release-edit/media-press-release-edit.component';
import { MediaPressReleaseComponentRTL } from './components/media-press-release-rtl/media-press-release.component-rtl';


@NgModule({
  declarations: [MediaPressReleaseComponent,MediaPressReleaseComponentRTL, MediaPressReleaseRequestComponent, MediaPressReleaseViewComponent, MediaPressReleaseContainerComponent, MediaPressReleaseEditComponent],
  imports: [
    CommonModule,
    MediaPressReleaseRoutingModule,
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
    MediapressService
  ]
})
export class MediaPressReleaseModule { }
