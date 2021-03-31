import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CampaignComponent } from './containers/campaign/campaign.component';
import { CampaignEditComponent } from './containers/campaign-edit/campaign-edit.component';
import { CampaignRequestComponent } from './containers/campaign-request/campaign-request.component';

const routes: Routes = [{
  path: '',
  component: CampaignComponent,
  children: [
    { path: 'request', component: CampaignRequestComponent },
    { path: 'view/:id', component: CampaignRequestComponent },
    { path: 'edit/:id', component: CampaignEditComponent }

  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CampaignRoutingModule { }
