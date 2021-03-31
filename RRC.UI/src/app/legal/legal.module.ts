import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LegalRoutingModule } from './legal-routing.module';
import { LegalComponent } from './container/legal/legal.component';
import { LegalRequestFormComponent } from './component/legal-request-form/legal-request-form.component';
import { LegalRequestCreateComponent } from './container/legal-request-create/legal-request-create.component';
import { LegalRequestViewComponent } from './container/legal-request-view/legal-request-view.component';
import { HomePageComponent } from './component/home-page/home-page.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { LegalService } from './service/legal.service';
import { TagInputModule } from 'ngx-chips';
import { LegalDocumentsComponent } from './container/legal-documents/legal-documents.component';
import { PopupModule } from '../modal/popup.module';
import { HomeComponent } from './container/home/home.component';
import { HomePageComponentRTL } from './component/home-page-rtl/home-page.component-rtl';
import { LegalRequestFormComponentRTL } from './component/legal-request-form-rtl/Legal-request-form.component-rtl';


@NgModule({
  declarations: [LegalComponent, LegalRequestFormComponent, LegalRequestFormComponentRTL, LegalRequestCreateComponent, LegalRequestViewComponent, HomePageComponent, HomePageComponentRTL, LegalDocumentsComponent, HomeComponent],
  imports: [
    CommonModule,
    LegalRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    SelectDropDownModule,
    BsDatepickerModule,
    NgbPaginationModule,
    NgxDatatableModule,
    ModalModule,
    NgMultiSelectDropDownModule,
    NgSelectModule,
    AccordionModule,
    TagInputModule,
    PopupModule
  ],
  providers: [LegalService]
})
export class LegalModule { }
