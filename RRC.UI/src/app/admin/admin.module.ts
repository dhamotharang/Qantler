import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { SharedModule } from '../shared/shared.module';
import { AdminComponent } from './container/admin/admin.component';
import { ModalModule, ProgressbarModule, BsDatepickerModule, AccordionModule } from 'ngx-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { AdminSettingsComponent } from './container/admin-settings/admin-settings.component';
import { AdminService } from './service/admin/admin.service';
import { AdminSettingsRtlComponent } from './container/admin-settings-rtl/admin-settings-rtl.component';
import { MasterAdminSettingsComponent } from './container/master-admin-settings/master-admin-settings.component';
import { NgxTinymceModule } from 'ngx-tinymce';
import { EditorModule } from '@tinymce/tinymce-angular';

@NgModule({
  declarations: [AdminComponent, AdminSettingsComponent, AdminSettingsRtlComponent, MasterAdminSettingsComponent],
  imports: [
    CommonModule,
    SharedModule,
    AdminRoutingModule,
    ModalModule,
    NgSelectModule,
    FormsModule,
    ReactiveFormsModule,
    ProgressbarModule,
    NgbPaginationModule,
    BsDatepickerModule,
    NgxDatatableModule,
    NgMultiSelectDropDownModule,
    AccordionModule.forRoot(),
    NgxTinymceModule.forRoot({
      baseURL: 'assets/tinymce/',
    }),
    EditorModule
  ],
  providers:[AdminService]
})
export class AdminModule { }
