import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PhotographerComponent } from './container/photographer/photographer.component';
import { PhotographerRequestCreateComponent } from './container/photographer-request-create/photographer-request-create.component';
import { PhotographerRequestViewComponent } from './container/photographer-request-view/photographer-request-view.component';
import { PhotographerRequestEditComponent } from './container/photographer-request-edit/photographer-request-edit.component';

const routes: Routes = [{
  path: '', component: PhotographerComponent, children: [
    { path: '', redirectTo: 'create', pathMatch: 'full' },
    { path: 'create', component: PhotographerRequestCreateComponent },
    { path: 'view/:id', component: PhotographerRequestViewComponent },
    { path: 'edit/:id', component: PhotographerRequestEditComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PhotographerRoutingModule { }
