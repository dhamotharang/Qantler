import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TrainingRequestComponent } from './container/training-request/training-request.component';
import { TrainingRequestCreateComponent } from './container/training-request-create/training-request-create.component';

const routes: Routes = [
  {
    path: '', component: TrainingRequestComponent,
    children: [
      { path: 'request-create', component: TrainingRequestCreateComponent },
      { path: 'request-view/:id', component: TrainingRequestCreateComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TrainingRequestRoutingModule { }
