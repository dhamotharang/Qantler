import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OfficialTaskComponent } from './containers/official-task/official-task.component';
import { OfficialTaskFormComponent } from './components/official-task-form/official-task-form.component';

const routes: Routes = [
  {
    path: '',
    component: OfficialTaskComponent,
    children: [
      { path: 'request-create', component: OfficialTaskFormComponent },
      { path: 'request-view/:type/:id', component: OfficialTaskFormComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OfficialTasksRoutingModule { }
