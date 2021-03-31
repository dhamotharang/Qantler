import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MediaRequestListComponent } from './container/media-request-list/media-request-list.component';
import { MediaTemplateComponent } from './components/media-template.component';
import { MediaRequestStaffListComponent } from './container/media-request-staff-list/media-request-staff-list.component';
import { MediaRequestDesignFormComponent } from './container/component/media-request-design-form/media-request-design-form.component';
import { MediaRequestDesignComponent } from './container/media-request-design/media-request-design.component';
import { MediaRequestPhotoComponent } from './container/component/media-request-photo/media-request-photo.component';
import { MediaRequestPhotoCreateComponent } from './container/media-request-photo-create/media-request-photo-create.component';
import { MediaProtocolRequestsComponent } from './container/media-protocol-requests/media-protocol-requests.component';
import { DocumentsPageComponent } from '../shared/component/documents-page/documents-page.component';
import { ProtocolHomepageComponent } from './container/protocol-homepage/protocol-homepage.component';

const routes: Routes = [{
  path: '', component:MediaTemplateComponent,
  children: [
      { path:'', redirectTo:'media-protocol-request', pathMatch:'full' },
      { path: 'media-request-list', component: MediaRequestListComponent },
      { path: 'media-request-staff-list', component: MediaRequestStaffListComponent },
      { path: 'media-request-design-form-creation', component: MediaRequestDesignComponent, data: { title: 'Create' } },
      { path: 'media-request-design/:id', component: MediaRequestDesignComponent, data: { title: 'View' } },
      { path: 'media-request-design/edit/:id', component: MediaRequestDesignComponent, data: { title: 'Edit' } },
      { path: 'media-request-photo', component: MediaRequestPhotoCreateComponent, data: { title: 'Create' } },
      { path: 'media-request-photo/view/:id', component: MediaRequestPhotoCreateComponent, data: { title: 'View' } },
      { path: 'media-request-photo/edit/:id', component: MediaRequestPhotoCreateComponent, data: { title: 'Edit' } },
      { path: 'campaign', loadChildren: './campaign/campaign.module#CampaignModule' },
      { path: 'photographer', loadChildren: './photographer/photographer.module#PhotographerModule'},
      { path: 'media-protocol-request', component: MediaProtocolRequestsComponent },
      { path: 'media-request-staff-list', component: MediaRequestStaffListComponent },
      { path: 'media-request-design-form-creation', component: MediaRequestDesignComponent },
      { path: 'media-request-photo', component: MediaRequestPhotoCreateComponent },
      { path: 'campaign', loadChildren: './campaign/campaign.module#CampaignModule' },
      { path: 'photographer', loadChildren: './photographer/photographer.module#PhotographerModule'},
      { path: 'protocol-home-page', component: MediaProtocolRequestsComponent },
      { path: 'diwan-identity', loadChildren: './diwan-identity/diwan-identity.module#DiwanIdentityModule'},
      { path: 'media-press-release', loadChildren: './media-press-release/media-press-release.module#MediaPressReleaseModule' },
      { path: 'documents', component:DocumentsPageComponent, data: { documentType: 'Media', department:'Protocol'}},
      { path: 'calendar-management', loadChildren: '../media/calendar-management/calendar-management.module#CalendarManagementModule'},
      { path: 'gifts-management', loadChildren:'../media/gifts-management/gifts-management.module#GiftsManagementModule' },
      { path: 'protocol-dashboard', component: ProtocolHomepageComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MediaRoutingModule { }
