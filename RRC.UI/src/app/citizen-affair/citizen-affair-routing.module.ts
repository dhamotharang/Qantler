import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CitizenAffairTemplateComponent } from './components/citizen-affair-template.component';
import { ComplaintSuggestionComponent } from './container/component/complaints-suggestions/complaints-suggestions.component';
import { CitizenDocumentsComponent } from './container/citizen-documents/citizen-documents.component';
import { CreateComponent } from './container/create/create.component';
import { ListComponent } from './container/list/list.component';
import { ComplaintsSuggestionsCreateComponent } from './container/complaints-suggestions-create/complaints-suggestions-create.component';
const routes: Routes = [{
  path: '', component: CitizenAffairTemplateComponent,
  children: [
    { path: '', redirectTo: 'citizen-affair-list' },
    { path: 'citizen-affair-list', component: ListComponent },
    { path: 'citizen-affair-create', component: CreateComponent, data: { title: 'Create' } },
    { path: 'complaint-suggestion', component: ComplaintsSuggestionsCreateComponent, data: { title: 'Create' } },
    { path: 'complaint-suggestion-view/:id', component: ComplaintsSuggestionsCreateComponent, data: { title: 'View' } },
    { path: 'citizen-affair-view/:id', component: CreateComponent, data: { title: 'View' } },
    { path: 'citizen-affair-edit/:id', component: CreateComponent, data: { title: 'Edit' } },
    { path: 'citizen-affair-documents', component: CitizenDocumentsComponent },
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CitizenAffairRoutingModule { }
