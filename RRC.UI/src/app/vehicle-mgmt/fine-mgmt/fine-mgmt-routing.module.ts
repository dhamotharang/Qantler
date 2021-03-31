import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FineMgmtListComponent } from './container/fine-mgmt-list/fine-mgmt-list.component';
import { from } from 'rxjs';
import { LogFineComponent } from './component/log-fine/log-fine.component';
import { FineManagementComponent } from './container/fine-management/fine-management.component';

const routes: Routes = [{
  path: '', component: FineManagementComponent, children: [
    { path: '', redirectTo: 'dashboard', pathMatch:'full' },
    { path: 'dashboard', component: FineMgmtListComponent },
    { path: 'log-fine', component: LogFineComponent },
    { path: 'log-fine/:PlateNumber', component: LogFineComponent },
    { path: 'log-fine-view/:ID/:PlateNumber', component: LogFineComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FineMgmtRoutingModule { }
