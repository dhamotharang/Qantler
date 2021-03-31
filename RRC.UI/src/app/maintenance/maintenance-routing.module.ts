import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MaintenanceContainerComponent } from './containers/maintenance-container/maintenance-container.component';
import { MaintenanceHomeComponent } from './components/maintenance-home/maintenance-home.component';
import { MaintenanceFormComponent } from './components/maintenance-form/maintenance-form.component';
import { DocumentsPageComponent } from '../shared/component/documents-page/documents-page.component';

const routes: Routes = [{
  path: '',
  component: MaintenanceContainerComponent,
  children: [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: MaintenanceHomeComponent },
    { path: 'create', component: MaintenanceFormComponent },
    { path: 'view/:id', component: MaintenanceFormComponent },
    { path: 'documents', component: DocumentsPageComponent, data: { documentType: 'Maintenance', department:'Maintenance'}},
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MaintenanceRoutingModule { }
