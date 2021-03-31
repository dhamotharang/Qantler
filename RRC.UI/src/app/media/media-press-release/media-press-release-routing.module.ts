import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MediaPressReleaseContainerComponent } from './container/media-press-release-container/media-press-release-container.component';
import { MediaPressReleaseViewComponent } from './container/media-press-release-view/media-press-release-view.component';
import { MediaPressReleaseRequestComponent } from './container/media-press-release-request/media-press-release-request.component';
import { MediaPressReleaseEditComponent } from './container/media-press-release-edit/media-press-release-edit.component';

const routes: Routes = [{
  path: '',
  component: MediaPressReleaseContainerComponent,
  children: [
    // { path: '', redirectTo: 'media/protocol-home-page' },
    { path: 'request', component: MediaPressReleaseRequestComponent },
    { path: 'view/:id', component: MediaPressReleaseViewComponent },
    { path: 'edit/:id', component: MediaPressReleaseEditComponent },
  ]
}];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MediaPressReleaseRoutingModule { }
