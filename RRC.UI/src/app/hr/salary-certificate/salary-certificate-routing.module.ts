import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SalaryComponent } from './container/salary/salary.component';
import { SalaryCertificateCreateComponent } from './container/salary-certificate-create/salary-certificate-create.component';
import { SalaryCertificateViewComponent } from './container/salary-certificate-view/salary-certificate-view.component';

const routes: Routes = [
  {
    path:'', component:SalaryComponent,
    children:[
      {path:'create',component:SalaryCertificateCreateComponent},
      {path:'view/:id',component:SalaryCertificateViewComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalaryCertificateRoutingModule { }
