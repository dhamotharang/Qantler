import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateComponent } from './container/create/create.component';
import { ListComponent } from './container/list/list.component';
import { TaskTemplateComponent } from './components/task-template.component';

const routes: Routes = [
  {
    path: '', component: TaskTemplateComponent,
    children: [
      { path: '', redirectTo: 'task-dashboard' },
      { path: 'task-dashboard', component: ListComponent, data: { title: 'Dashboard' } },
      { path: 'task-create', component: CreateComponent, data: { title: 'Task Create' } },
      { path: 'task-create/:letterNo', component: CreateComponent, data: { title: 'Task Create' } },
      { path: 'task-edit/:id', component: CreateComponent, data: { title: 'Task Edit' } },
      { path: 'task-view/:id', component: CreateComponent, data: { title: 'Task View' } },
    ]
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TaskRoutingModule { }