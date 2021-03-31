import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DiwanIdentityRoutingModule } from './diwan-identity-routing.module';
import { DiwanIdentityRequestFormComponent } from './component/diwan-identity-request-form/diwan-identity-request-form.component';
import { DiwanIdentityComponent } from './container/diwan-identity/diwan-identity.component';
import { DiwanIdentityRequestCreateComponent } from './container/diwan-identity-request-create/diwan-identity-request-create.component';
import { DiwanIdentityRequestViewComponent } from './container/diwan-identity-request-view/diwan-identity-request-view.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { SharedModule } from 'src/app/shared/shared.module';
import { PopupModule } from 'src/app/modal/popup.module';
import { DiwanIdentityService } from './service/diwan-identity.service';
import { DiwanIdentityRequestFormRtlComponent } from './component/diwan-identity-request-form-rtl/diwan-identity-request-form-rtl.component';

@NgModule({
  declarations: [DiwanIdentityRequestFormComponent, DiwanIdentityComponent, DiwanIdentityRequestCreateComponent, DiwanIdentityRequestViewComponent, DiwanIdentityRequestFormRtlComponent],
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
    PopupModule,
    DiwanIdentityRoutingModule
  ],
  providers:[DiwanIdentityService]
})
export class DiwanIdentityModule { }
