import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListPageComponent } from './container/list-page/list-page.component';
import { CreateEventComponent } from './container/create-event/create-event.component';
import { ViewEventComponent } from './container/view-event/view-event.component';
import { CalendarComponent } from './container/calendar/calendar.component';
import { EventListPageComponent } from './container/event-list-page/event-list-page.component';
import { HomePageComponent } from 'src/app/media/calendar-management/container/home-page/home-page.component';

const routes: Routes = [{
  path: '', component: CalendarComponent, children: [
    // { path: '', redirectTo: 'list', component: ListPageComponent },
    { path: 'list', component: ListPageComponent },
    { path: 'homepage', component: HomePageComponent },
    { path: 'event-list/:id', component: EventListPageComponent },
    { path: 'create-event', component: CreateEventComponent },
    { path: 'view-event/:id', component: ViewEventComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CalendarManagementRoutingModule { }
