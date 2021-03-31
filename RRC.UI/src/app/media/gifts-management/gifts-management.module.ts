import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GiftsManagementRoutingModule } from './gifts-management-routing.module';
import { GiftsManagementHomeComponent } from './container/gifts-management-home/gifts-management-home.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxPaginationModule } from 'ngx-pagination';
import { TagInputModule } from 'ngx-chips';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { GiftRequestFormComponent } from './component/gift-request-form/gift-request-form.component';
import { GiftRequestCreateComponent } from './container/gift-request-create/gift-request-create.component';
import { GiftRequestViewComponent } from './container/gift-request-view/gift-request-view.component';
import { GiftsManagementService } from './service/gifts-management.service';
import { GiftsManagementComponent } from './container/gifts-management/gifts-management.component';
import { GiftRequestFormRtlComponent } from './component/gift-request-form-rtl/gift-request-form-rtl.component';

@NgModule({
  declarations: [GiftsManagementHomeComponent, GiftRequestFormComponent, GiftRequestCreateComponent, GiftRequestViewComponent, GiftsManagementComponent, GiftRequestFormRtlComponent],
  imports: [
    CommonModule,
    SharedModule,
    GiftsManagementRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SelectDropDownModule,
    BsDatepickerModule.forRoot(),
    NgbPaginationModule,    
    NgxDatatableModule,
    NgxPaginationModule,    
    TagInputModule,    
    ModalModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),    
    NgSelectModule
  ],
  providers:[GiftsManagementService]
})
export class GiftsManagementModule { }
