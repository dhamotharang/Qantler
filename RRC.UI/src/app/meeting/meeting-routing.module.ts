import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MeetingComponent } from './container/meeting/meeting.component';
import { ListPageComponent } from './container/list-page/list-page.component';
import { MyCalendarComponent } from './container/my-calendar/my-calendar.component';
import { CreateMeetingComponent } from './container/create-meeting/create-meeting.component';
import { EditMeetingComponent } from './container/edit-meeting/edit-meeting.component';
import { ViewMeetingComponent } from './container/view-meeting/view-meeting.component';
import { CreateMomComponent } from './container/create-mom/create-mom.component';
import { ViewMomComponent } from './container/view-mom/view-mom.component';

const routes: Routes = [{
  path: '', component: MeetingComponent, children: [
    { path: '' , redirectTo: 'list', component: ListPageComponent },
    { path: 'list', component: ListPageComponent},
    { path: 'my-calendar', component: MyCalendarComponent},
    { path: 'create', component: CreateMeetingComponent},
    { path: 'edit', component: EditMeetingComponent},
    { path: 'view/:id', component: ViewMeetingComponent},
    { path: 'create-mom/:id', component: CreateMomComponent},
    { path: 'view-mom', component: ViewMomComponent}

  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MeetingRoutingModule { }
