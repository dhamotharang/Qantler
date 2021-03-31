import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommentSectionComponent } from './component/comment-section/comment-section.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { AssignModalComponent } from './modal/assign-modal/assign-modal.component';
import { AssignService } from './service/assign.service';
import { CommentSectionService } from './service/comment-section.service';
import { LayoutModule } from '../layout/layout.module';
import { BtnLoaderComponent } from './btn-loader/btn-loader.component';
import { ProgressBarComponent } from './progress-bar/progress-bar.component';
import { DropdownsService } from './service/dropdowns.service';
import { UtilsService } from './service/utils.service';
import { UploadService } from './service/upload.service';
import { EscalateModalComponent } from './modal/escalate-modal/escalate-modal.component';
import { HrReportModalComponent } from './modal/hr-report-modal/hr-report-modal.component';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { HrDocumentsModalComponent } from './modal/hr-documents-modal/hr-documents-modal.component';
import { TaskChatComponent } from '../task/container/task-chat/task-chat.component';
import { SendMsgComponent } from '../task/container/task-chat/send-msg/send-msg.component';
import { ReceiveMsgComponent } from '../task/container/task-chat/receive-msg/receive-msg.component';
import { PickerModule } from '@ctrl/ngx-emoji-mart';
import { EmojiModule } from '@ctrl/ngx-emoji-mart/ngx-emoji';
import { ReportsService } from './service/reports.service';
import { CitizenDocumentsModalComponent } from './modal/citizen-documents-modal/citizen-documents-modal.component';
import { MediaDocumentsModalComponent } from './modal/media-documents-modal/media-documents-modal.component';
import { DocumentsPageComponent } from './component/documents-page/documents-page.component';
import { DocumentsPageService } from './service/documents-page.service';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { ItService } from '../it/service/it.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { NumbersOnlyDirective } from './numbers-only.directive';

@NgModule({
  declarations: [
    CommentSectionComponent,
    AssignModalComponent,
    TaskChatComponent,
    SendMsgComponent,
    ReceiveMsgComponent,
    BtnLoaderComponent,
    ProgressBarComponent,
    EscalateModalComponent,
    HrReportModalComponent,
    HrDocumentsModalComponent,
    CitizenDocumentsModalComponent,
    MediaDocumentsModalComponent,
    DocumentsPageComponent,
    NumbersOnlyDirective
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SelectDropDownModule,
    AccordionModule,
    BsDatepickerModule,
    PickerModule,
    EmojiModule,
    NgxDatatableModule,
    NgbPaginationModule,
    NgSelectModule
    // LayoutModule
  ],
  exports: [
    CommentSectionComponent,
    BtnLoaderComponent,
    ProgressBarComponent,
    TaskChatComponent, 
    SendMsgComponent, 
    ReceiveMsgComponent,
    NumbersOnlyDirective
  ],
  entryComponents: [
    AssignModalComponent,
    EscalateModalComponent,
    HrReportModalComponent,
    HrDocumentsModalComponent,
    CitizenDocumentsModalComponent,
    MediaDocumentsModalComponent
  ],
  providers: [
    AssignService,
    UtilsService,
    UploadService,
    DropdownsService,
    CommentSectionService,
    ReportsService,
    DocumentsPageService,
    ItService
  ]
})
export class SharedModule { }
