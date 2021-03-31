import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExperienceCreateComponent } from './container/experience-create/experience-create.component';
import { ExperienceViewComponent } from './container/experience-view/experience-view.component';
import { ExperienceComponent } from './container/experience/experience.component';

const routes: Routes = [
  {
    path:'', component:ExperienceComponent,
    children:[
      { path:'create',component:ExperienceCreateComponent },
      { path:'view/:id',component:ExperienceViewComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExperienceCertificateRoutingModule { }
