import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContactsComponent } from './components/contacts/contacts.component';
import { ContactsContainerComponent } from './container/contacts-container/contacts-container.component';
import { ContactsFormComponent } from './components/contacts-form/contacts-form.component';
import { ContactViewComponent } from './container/contact-view/contact-view.component';
import { ContactEditComponent } from './container/contact-edit/contact-edit.component';
import { ContactCreateComponent } from './container/contact-create/contact-create.component';


const routes: Routes = [{
  path: '',
  component: ContactsContainerComponent,
  children: [
    { path: 'contacts', component: ContactsComponent, },
    { path: 'contact-form', component: ContactCreateComponent, },
    { path: 'contact-edit', component: ContactEditComponent, },
    { path: 'contact-view', component: ContactViewComponent, },
  ]
}];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContactsRoutingModule { }
