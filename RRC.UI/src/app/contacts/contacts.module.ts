import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDatepickerModule, AccordionModule, ModalModule } from 'ngx-bootstrap';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { TagInputModule } from 'ngx-chips';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { EditorModule } from '@tinymce/tinymce-angular';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxEditorModule } from 'ngx-editor';
import { NgxPaginationModule } from 'ngx-pagination';

import { ContactsRoutingModule } from './contacts-routing.module';
import { ContactsComponent } from './components/contacts/contacts.component';
import { ContactsContainerComponent } from './container/contacts-container/contacts-container.component';
import { ContactsFormComponent } from './components/contacts-form/contacts-form.component';
import { ContactsService } from './service/contacts.service';
import { ContactEditComponent } from './container/contact-edit/contact-edit.component';
import { ContactViewComponent } from './container/contact-view/contact-view.component';
import { ContactCreateComponent } from './container/contact-create/contact-create.component';
import { AdminService } from '../admin/service/admin/admin.service';

@NgModule({
  declarations: [ContactsComponent, ContactsContainerComponent, ContactsFormComponent, ContactEditComponent, ContactViewComponent, ContactCreateComponent],
  imports: [
    CommonModule,
    ContactsRoutingModule,
    SharedModule,
    ContactsRoutingModule,
    FormsModule,
    SelectDropDownModule,
    BsDatepickerModule.forRoot(),
    NgbPaginationModule,
    NgxEditorModule,
    NgxDatatableModule,
    NgxPaginationModule,
    NgbPaginationModule,
    ReactiveFormsModule,
    TagInputModule,
    EditorModule,
    ModalModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    NgSelectModule,
    AccordionModule.forRoot(),
    SharedModule
  ],
  providers:[ContactsService, AdminService],
  exports:[
    ContactsComponent
  ]
})
export class ContactsModule { }
