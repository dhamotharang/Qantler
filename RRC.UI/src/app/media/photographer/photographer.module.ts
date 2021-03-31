import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { CommonModule } from '@angular/common';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { PhotographerRoutingModule } from './photographer-routing.module';
import { PhotographerRequestComponent } from './component/photographer-request/PhotographerRequestComponent';
import { PhotographerRequestCreateComponent } from './container/photographer-request-create/photographer-request-create.component';
import { PhotographerRequestViewComponent } from './container/photographer-request-view/photographer-request-view.component';
import { PhotographerComponent } from './container/photographer/photographer.component';
import { PhotographerService } from './service/photographer.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { PhotographerRequestEditComponent } from './container/photographer-request-edit/photographer-request-edit.component';
import { PhotographerRequestComponentRTL } from './component/photographer-request-rtl/PhotographerRequestComponent-rtl';

@NgModule({
  declarations: [
    PhotographerRequestComponent,
    PhotographerRequestCreateComponent,
    PhotographerRequestViewComponent,
    PhotographerComponent,
    PhotographerRequestEditComponent,
    PhotographerRequestComponentRTL
  ],
  imports: [
    CommonModule,
    PhotographerRoutingModule,
    BsDatepickerModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    NgSelectModule
  ],
  providers: [
    PhotographerService
  ]
})
export class PhotographerModule { }
