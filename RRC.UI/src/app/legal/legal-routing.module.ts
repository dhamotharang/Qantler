import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LegalComponent } from './container/legal/legal.component';
import { LegalRequestViewComponent } from './container/legal-request-view/legal-request-view.component';
import { LegalRequestCreateComponent } from './container/legal-request-create/legal-request-create.component';
import { LegalDocumentsComponent } from './container/legal-documents/legal-documents.component';
import { DocumentsPageComponent } from '../shared/component/documents-page/documents-page.component';
import { HomeComponent } from './container/home/home.component';

const routes: Routes = [
  {
    path: '', component: LegalComponent,
    children: [
      { path: '', redirectTo: '/en/app/legal/dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: HomeComponent },
      { path: 'documents', component: DocumentsPageComponent, data: { documentType: 'Legal', department: 'Legal Services' } },
      { path: 'request-create', component: LegalRequestCreateComponent, data: {title:'Creation'} },
      { path: 'request-view/:id', component: LegalRequestViewComponent, data: {title:'View'} },
      { path: '**', redirectTo: '/home', pathMatch: 'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LegalRoutingModule { }
