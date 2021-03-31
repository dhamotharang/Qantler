import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HrRoutingModule } from './hr-routing.module';
import { HrComponent } from './container/hr/hr.component';
import { SharedModule } from '../shared/shared.module';
import { HomePageComponent } from './container/home-page/home-page.component';
import { HrService } from './service/hr.service';
import { ProgressbarModule, BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { HrDocumentsComponent } from './container/hr-documents/hr-documents.component';
import { UploadService } from '../shared/service/upload.service';
import { RaiseComplaintsSuggestionsComponent } from './container/raise-complaints-suggestions/raise-complaints-suggestions.component';
import { RaiseComplaintsSuggestionsRtlComponent } from './container/raise-complaints-suggestions-rtl/raise-complaints-suggestions-rtl.component';
import { RaiseComplaintsSuggestionsCreateComponent } from './raise-complaints-suggestions-create/raise-complaints-suggestions-create.component';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [HrComponent, HomePageComponent, HrDocumentsComponent, RaiseComplaintsSuggestionsComponent, RaiseComplaintsSuggestionsRtlComponent, RaiseComplaintsSuggestionsCreateComponent ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ProgressbarModule,
    NgbPaginationModule,
    BsDatepickerModule,
    NgxDatatableModule,
    SharedModule,
    HrRoutingModule,
    ModalModule,
    NgSelectModule
  ],
  providers:[HrService],
  entryComponents:[]
})
export class HrModule { }
