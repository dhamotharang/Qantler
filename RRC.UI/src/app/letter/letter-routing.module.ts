import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { IncomingLetterFormComponent } from './container/component/incoming-letter-form/incoming-letter-form.component';
import { CreateComponent } from './container/create/create.component';
import { OutComponent } from './container/out/out.component';
import { ListComponent } from './container/list/list.component';
import { OutgoingLetterFormComponent } from './container/component/outgoing-letter-form/outgoing-letter-form.component';
import { LetterListComponent } from './container/component/letter-list/letter-list.component';
import { LetterTemplateComponent } from './components/letter-template.component';
const routes: Routes = [{
  path: '', component: LetterTemplateComponent,
  children: [
    { path: '', redirectTo:'letter-list'},
    { path: 'incomingletter-create', component: CreateComponent, data: { title: 'Create' } },
    { path: 'letter-list', component: ListComponent },
    { path: 'incomingletter-view/:id', component: CreateComponent, data: { title: 'View' } },
    { path: 'incomingletter-edit/:id', component: CreateComponent, data: { title: 'Edit' } },
    { path: 'outgoingletter-view/:id', component: OutComponent, data: { title: 'View' } },
    { path: 'outgoingletter-edit/:id', component: OutComponent, data: { title: 'Edit' } },
    { path: 'outgoingletter-create', component: OutComponent, data: { title: 'Create' } },
    { path: 'mypendingactionsincoming-list', component: ListComponent },
    { path: 'mypendingactionsoutcoming-list', component: ListComponent },
    { path: 'draftletter-list', component: ListComponent },
    { path: 'historicalletter-list', component: ListComponent },
    { path: 'draftletter-list', component: ListComponent },
    { path: 'historicalletter-list', component: ListComponent },
    { path: 'mypendingactionsincoming-list', component: ListComponent },
    { path: 'mypendingactionsoutcoming-list', component: ListComponent },
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LetterRoutingModule { }
