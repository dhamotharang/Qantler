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
import { FullCalendarModule } from '@fullcalendar/angular';

import { MeetingRoutingModule } from './meeting-routing.module';
import { ListPageComponent } from './container/list-page/list-page.component';
import { MeetingComponent } from './container/meeting/meeting.component';
import { MyCalendarComponent } from './container/my-calendar/my-calendar.component';
import { MeetingFormComponent } from './component/meeting-form/meeting-form.component';
import { CreateMeetingComponent } from './container/create-meeting/create-meeting.component';
import { ViewMeetingComponent } from './container/view-meeting/view-meeting.component';
import { EditMeetingComponent } from './container/edit-meeting/edit-meeting.component';
import { SharedModule } from '../shared/shared.module';
import { CreateMomComponent } from './container/create-mom/create-mom.component';
import { ViewMomComponent } from './container/view-mom/view-mom.component';
import { CreateMomFormComponent } from './component/create-mom-form/create-mom-form.component';
import { CreateMomFormRtlComponent } from './component/create-mom-form-rtl/create-mom-form-rtl.component';
import { MeetingFormRtlComponent } from './component/meeting-form-rtl/meeting-form-rtl.component';

@NgModule({
  declarations: [
    ListPageComponent,
    MeetingComponent,
    MyCalendarComponent,
    MeetingFormComponent,
    CreateMeetingComponent,
    ViewMeetingComponent,
    EditMeetingComponent,
    CreateMomComponent,
    ViewMomComponent,
    CreateMomFormComponent,
    CreateMomFormRtlComponent,
    MeetingFormRtlComponent
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
    MeetingRoutingModule,
    SharedModule,
    FullCalendarModule
  ]
})
export class MeetingModule { }
