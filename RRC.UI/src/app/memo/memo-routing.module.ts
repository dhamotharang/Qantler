import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
// import { CreateMemoComponent } from './container/create-memo/create-memo.component';
import { MemoTemplateComponent } from './components/memo-template.component';
// import { MemoListComponent } from './container/memo-list/memo-list.component';
import { CreateComponent } from './container/create/create.component';
import { ListComponent } from './container/list/list.component';
const routes: Routes = [{
  path: '', component: MemoTemplateComponent,
  children: [
    { path: '', redirectTo: 'memo-list'},
    { path: 'memo-create', component: CreateComponent, data: { title: 'Create' } },
    { path: 'memo-view/:id', component: CreateComponent, data: { title: 'View' } },
    { path: 'memo-edit/:id', component: CreateComponent, data: { title: 'Edit' } },
    { path: 'memo-list', component: ListComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MemoRoutingModule { }
