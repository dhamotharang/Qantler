import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { CampaignRoutingModule } from './campaign-routing.module';
import { CampaignComponent } from './containers/campaign/campaign.component';
import { CampaignFormComponent } from './components/campaign-form/campaign-form.component';
import { CampaignService } from './services/campaign.service';
import { SharedModule } from 'src/app/shared/shared.module';
import { CampaignEditComponent } from './containers/campaign-edit/campaign-edit.component';
import { CampaignFormComponentRTL } from './components/campaign-form-rtl/campaign-form.component-rtl';
import { CampaignRequestComponent } from './containers/campaign-request/campaign-request.component';

@NgModule({
  declarations: [CampaignComponent, CampaignFormComponent,CampaignFormComponentRTL,CampaignRequestComponent, CampaignEditComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    AccordionModule.forRoot(),
    NgSelectModule,
    NgbPaginationModule,
    NgxDatatableModule,
    SharedModule,
    CampaignRoutingModule
  ],
  providers: [CampaignService]
})
export class CampaignModule { }
