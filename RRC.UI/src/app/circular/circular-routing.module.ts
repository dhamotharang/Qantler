import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import{ListComponent} from './container/list/list.component';
import{CreateComponent} from './container/create/create.component';
import { CircularTemplateComponent } from './components/circular-template.component';
const routes: Routes = [{
  path: '', component:CircularTemplateComponent,
  children: [
    { path: '', redirectTo: 'circular-list'},
    { path: 'circular-list', component: ListComponent },
    { path: 'circular-create', component: CreateComponent, data: { title: 'Create' } },
    { path: 'Circular-view/:id', component: CreateComponent, data: { title: 'View' } },
    { path: 'Circular-edit/:id', component: CreateComponent, data: { title: 'Edit' } },
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CircularRoutingModule { }
