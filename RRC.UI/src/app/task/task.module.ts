import { NgModule } from '@angular/core';
import { TaskEvent } from './service/task.event'
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
import { CommonModule } from '@angular/common';
import { TaskRoutingModule } from './task.routing.module';
import { CreateTaskComponent } from './container/component/create-task/create-task.component';
import { TaskDashboardComponent } from './container/component/task-dashboard/task-dashboard.component';
import { CreateTaskComponentRTL } from './container/component/create-task-rtl/create-task.component-rtl';
import { TaskDashboardComponentRTL } from './container/component/task-dashboard-rtl/task-dashboard.component-rtl';
import { TaskTemplateComponent } from './components/task-template.component';
import { SharedModule } from '../shared/shared.module';
import { CreateComponent } from './container/create/create.component';
import { ListComponent } from './container/list/list.component';
import { LinkToModalComponent } from './modal/link-to-modal/link-to-modal.component';


@NgModule({
  declarations: [
    TaskTemplateComponent,
    CreateTaskComponent,
    TaskDashboardComponent,
    CreateTaskComponentRTL,
    TaskDashboardComponentRTL,
    ListComponent,
    CreateComponent,
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
    TypeaheadModule,
    TaskRoutingModule,
    SharedModule,
    // PickerModule,
    // EmojiModule
  ],
  entryComponents: [
    LinkToModalComponent
  ],
  exports: [
  ]
})
export class TaskModule { }
