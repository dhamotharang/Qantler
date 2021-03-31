import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AnnouncementComponent } from './container/announcement/announcement.component';
import { AnnouncementCreateComponent } from './container/announcement-create/announcement-create.component';
import { AnnouncementViewComponent } from './container/announcement-view/announcement-view.component';

const routes: Routes = [
  {
    path: '',
    component: AnnouncementComponent,
    children: [
      // { path:"**", redirectTo:'/app/hr/dashboard', pathMatch:'full' },
      { path: 'announcement-create', component: AnnouncementCreateComponent },
      { path: 'announcement-view/:id', component: AnnouncementViewComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AnnouncementRoutingModule { }
