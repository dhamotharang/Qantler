import { NgModule } from '@angular/core';
import { CommonPageModule } from '../commonpage.module';
import { ModalComponent } from './modalcomponent/modal.component';
import { SuccessComponent } from './success-popup/success.component';
import { ReportModalComponent } from './reportModal/reportModal.component';
import { CommonModalComponent } from './commonmodal/commonmodal.component';
import { AssignModalComponent } from './assignmodal/assignmodal.component';
import { LetterReportModalComponent } from './letter-report-modal/letter-report-modal.component';
import { SharedModule } from '../shared/shared.module';
import { CircularReportModalComponent } from './circular-report-modal/circular-report-modal.component';
import { LegalReportModalComponent } from './legal-report-modal/legal-report-modal.component';
import { LegalDocumentsModalComponent } from './legal-documents-modal/legal-documents-modal.component';
import { ItDocumentsModalComponent } from './it-documents-modal/it-documents-modal.component';
import { ItReportModalComponent } from './it-report-modal/it-report-modal.component';
import { CitizenReportModalComponent } from './citizen-report-modal/citizen-report-modal.component';
import { MaintenanceReportModalComponent } from './maintenance-report-modal/maintenance-report-modal.component';
import { ApologiesModalComponent } from './apologies-modal/apologies-modal.component';
import { TaskReportModalComponent } from './task-report-modal/task-report-modal.component';
import { GiftReportModalComponent } from './gift-report-modal/gift-report-modal.component';
import { CalendarManagementReportModalComponent } from './calendar-management-report-modal/calendar-management-report-modal.component';
import { CommentModalComponent } from './comment-modal/comment-modal.component';
import { ManageNewsComponent } from '../manage-news/component/manage-news/manage-news.component';
import { ManagePhotoComponent } from '../manage-photo/component/manage-photo/manage-photo.component';
import { InternalContactReportModalComponent } from './internalcontact-report-modal/internalcontact-report-modal.component';
import { ExternalContactReportModalComponent } from './externalcontact-report-modal/externalcontact-report-modal.component';
import { ErrorModalComponent } from './error-modal/error-modal.component';
import { NotificationModalComponent } from './notification-modal/notification-modal.component';
import { VehicleRentCarCreateModalComponent } from './vehicle-rent-car-create-modal/vehicle-rent-car-create-modal.component';
import { VehicleManagementComponent } from './vehicle-management/vehicle-management.component';
import { VehcileListReportComponent } from './vehcile-list-report/vehcile-list-report.component';
import { DriverManagementModalComponent } from './driver-management-modal/driver-management-modal.component';


@NgModule({
  declarations: [
    ModalComponent,
    SuccessComponent,
    ReportModalComponent,
    CommonModalComponent,
    AssignModalComponent,
    LetterReportModalComponent,
    CircularReportModalComponent,
    LegalReportModalComponent,
    LegalDocumentsModalComponent,
    ItDocumentsModalComponent,
    ItReportModalComponent,
    CitizenReportModalComponent,
    MaintenanceReportModalComponent,
    ApologiesModalComponent,
    TaskReportModalComponent,
    GiftReportModalComponent,
    CalendarManagementReportModalComponent,
    ApologiesModalComponent,
    CommentModalComponent,
    ManageNewsComponent,
    ManagePhotoComponent,
    InternalContactReportModalComponent,
    ExternalContactReportModalComponent,
    ErrorModalComponent,
    NotificationModalComponent,
    VehicleRentCarCreateModalComponent,
    VehicleManagementComponent,
    VehcileListReportComponent,
    DriverManagementModalComponent

  ],
  entryComponents: [
    ModalComponent,
    SuccessComponent,
    ReportModalComponent,
    LetterReportModalComponent,
    CommonModalComponent,
    AssignModalComponent,
    CircularReportModalComponent,
    LegalReportModalComponent,
    LegalDocumentsModalComponent,
    ItDocumentsModalComponent,
    ItReportModalComponent,
    CitizenReportModalComponent,
    MaintenanceReportModalComponent,
    TaskReportModalComponent,
    GiftReportModalComponent,
    CalendarManagementReportModalComponent,
    ApologiesModalComponent,
    CommentModalComponent,
    ManageNewsComponent,
    ManagePhotoComponent,
    InternalContactReportModalComponent,
    ExternalContactReportModalComponent,
    ErrorModalComponent,
    NotificationModalComponent,
    VehicleRentCarCreateModalComponent,
    VehicleManagementComponent,
    VehcileListReportComponent,
    DriverManagementModalComponent
  ],
  exports: [
    ModalComponent,
    SuccessComponent,
    CitizenReportModalComponent,
    ReportModalComponent,
    LetterReportModalComponent,
    CommonModalComponent,
    AssignModalComponent,
    CircularReportModalComponent,
    LegalReportModalComponent,
    LegalDocumentsModalComponent,
    TaskReportModalComponent,
    GiftReportModalComponent,
    CalendarManagementReportModalComponent,
    ApologiesModalComponent,
    CommentModalComponent,
    InternalContactReportModalComponent,
    ExternalContactReportModalComponent,
    ErrorModalComponent,
    NotificationModalComponent,
    VehicleManagementComponent,
    VehcileListReportComponent,
    DriverManagementModalComponent
  ],
  imports: [
    CommonPageModule,
    SharedModule
  ]
})
export class PopupModule { }
