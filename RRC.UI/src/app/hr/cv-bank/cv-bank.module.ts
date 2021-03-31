import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BsDatepickerModule, ModalModule, AccordionModule } from 'ngx-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { CvBankRoutingModule } from './cv-bank-routing.module';
import { CvbankComponent } from './container/cvbank/cvbank.component';
import { CvbankFormComponent } from './component/cvbank-form/cvbank-form.component';
import { CvbankListComponent } from './component/cvbank-list/cvbank-list.component';
import { CvbankFormRtlComponent } from './component/cvbank-form-rtl/cvbank-form-rtl.component';
import { CvBankFormCreateComponent } from './container/cv-bank-form-create/cv-bank-form-create.component';
import { CvBankFormViewComponent } from './container/cv-bank-form-view/cv-bank-form-view.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { CvbankReportModalComponent } from './modal/cvbank-report-modal/cvbank-report-modal.component';

@NgModule({
  declarations: [
    CvbankComponent,
    CvbankFormComponent,
    CvbankListComponent,
    CvbankFormRtlComponent,
    CvBankFormCreateComponent,
    CvBankFormViewComponent,
    CvbankReportModalComponent
  ],
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
    CvBankRoutingModule
  ],
  entryComponents: [
    CvbankReportModalComponent
  ]
})
export class CvBankModule { }
