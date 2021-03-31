import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewBabyAdditionComponent } from './container/new-baby-addition/new-baby-addition.component';
import { NewBabyAdditionCreateComponent } from './container/new-baby-addition-create/new-baby-addition-create.component';
import { NewBabyAdditionViewComponent } from './container/new-baby-addition-view/new-baby-addition-view.component';

const routes: Routes = [
  {path: '', component: NewBabyAdditionComponent,
    children: [
      {path: 'request-create', component: NewBabyAdditionCreateComponent},
      {path: 'request-view/:id', component: NewBabyAdditionViewComponent},
      // {path: 'request-create-rtl', component: NewBabyAdditionCreateComponent, data: {dir : 'rtl'}},
      // {path: 'request-view-rtl/:id', component: NewBabyAdditionViewComponent, data: {dir : 'rtl'}}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NewBabyAdditionRoutingModule { }
