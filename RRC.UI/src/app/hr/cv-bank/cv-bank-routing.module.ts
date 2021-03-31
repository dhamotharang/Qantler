import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CvbankComponent } from './container/cvbank/cvbank.component';
import { CvbankFormComponent } from './component/cvbank-form/cvbank-form.component';
import { CvbankListComponent } from './component/cvbank-list/cvbank-list.component';
import { CvBankFormCreateComponent } from './container/cv-bank-form-create/cv-bank-form-create.component';
import { CvBankFormViewComponent } from './container/cv-bank-form-view/cv-bank-form-view.component';

const routes: Routes = [
  {
    path: '',
    component: CvbankComponent,
    children: [
      { path: "", redirectTo:'cv-bank-list' },
      { path: 'cv-bank-create', component: CvBankFormCreateComponent },
      { path: 'cv-bank-view/:id', component: CvBankFormViewComponent },
      { path: 'cv-bank-list', component: CvbankListComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CvBankRoutingModule { }
