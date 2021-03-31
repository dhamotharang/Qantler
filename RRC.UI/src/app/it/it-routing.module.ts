import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ItComponent } from './container/it/it.component';
import { HomePageComponent } from './container/home-page/home-page.component';
import { ItRequestCreateComponent } from './container/it-request-create/it-request-create.component';
import { ItRequestViewComponent } from './container/it-request-view/it-request-view.component';
import { ItDocumentsComponent } from './container/it-documents/it-documents.component';
import { DocumentsPageComponent } from '../shared/component/documents-page/documents-page.component';

const routes: Routes = [{
  path: '', component: ItComponent, children: [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomePageComponent },
    { path: 'it-request-create', component: ItRequestCreateComponent },
    { path: 'it-request-view/:id', component: ItRequestViewComponent },
    { path: 'document', component: DocumentsPageComponent, data:{documentType:'ITSupport', department:'Information Technology'} }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ItRoutingModule { }
