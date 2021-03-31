import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DiwanIdentityComponent } from './container/diwan-identity/diwan-identity.component';
import { DiwanIdentityRequestCreateComponent } from './container/diwan-identity-request-create/diwan-identity-request-create.component';
import { DiwanIdentityRequestViewComponent } from './container/diwan-identity-request-view/diwan-identity-request-view.component';

const routes: Routes = [{
  path: '',
  component: DiwanIdentityComponent,
  children: [
    { path: 'request-create', component: DiwanIdentityRequestCreateComponent, data:{title:'Creation'} },
    { path: 'request-view/:id', component: DiwanIdentityRequestViewComponent, data:{title:'View'} }
  ]
}];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DiwanIdentityRoutingModule { }
