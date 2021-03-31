import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { FullCalendarModule } from '@fullcalendar/angular';

import { CalendarManagementRoutingModule } from './calendar-management-routing.module';
import { ListPageComponent } from './container/list-page/list-page.component';
import { CreateEventComponent } from './container/create-event/create-event.component';
import { ViewEventComponent } from './container/view-event/view-event.component';
import { CalendarComponent } from './container/calendar/calendar.component';
import { CalendarManagementFormComponent } from './component/calendar-management-form/calendar-management-form.component';
import { EventListPageComponent } from './container/event-list-page/event-list-page.component';
import { HomePageComponent } from './container/home-page/home-page.component';
import { CalendarManagementService } from './service/calendar-management.service';
import { CalendarManagementFormRtlComponent } from './component/calendar-management-form-rtl/calendar-management-form-rtl.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    ListPageComponent,
    CreateEventComponent,
    ViewEventComponent,
    CalendarComponent,
    CalendarManagementFormComponent,
    EventListPageComponent,
    HomePageComponent,
    CalendarManagementFormRtlComponent],
  imports: [
    CommonModule,
    CalendarManagementRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    BsDatepickerModule,
    ModalModule,
    AccordionModule,
    NgSelectModule,
    NgbPaginationModule,
    NgxDatatableModule,
    SelectDropDownModule,
    FullCalendarModule,
    SharedModule
  ],
  providers: [
    CalendarManagementService
  ]
})
export class CalendarManagementModule { }
