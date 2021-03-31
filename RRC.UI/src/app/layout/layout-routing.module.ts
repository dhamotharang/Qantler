import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { AuthGuard } from '../auth/auth.guard';
import { ErrorPageComponent } from './error-page/error-page.component';
import { AdminGuard } from './guard/admin.guard';
import { PhotogalleryComponent } from './photogallery/photogallery.component';
//import '../task/task.routing.module';


const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'hr', loadChildren: '../hr/hr.module#HrModule' },
      { path: 'it', loadChildren: '../it/it.module#ItModule' },
      { path: 'maintenance', loadChildren: '../maintenance/maintenance.module#MaintenanceModule' },
      { path: 'contacts', loadChildren: '../contacts/contacts.module#ContactsModule' },
      { path: 'legal', loadChildren: '../legal/legal.module#LegalModule' },
      { path: 'memo', loadChildren: '../memo/memo.module#MemoModule',canActivate:[AuthGuard] },
      { path: 'letter', loadChildren: '../letter/letter.module#LetterModule' },
      { path: 'task', loadChildren: '../task/task.module#TaskModule' },
      { path: 'circular', loadChildren: '../circular/circular.module#CircularModule' },
      { path: 'citizen-affair', loadChildren: '../citizen-affair/citizen-affair.module#CitizenAffairModule' },
      { path: 'media', loadChildren: '../media/media.module#MediaModule' },
      { path: 'meeting', loadChildren: '../meeting/meeting.module#MeetingModule' },
      { path: 'photogallery', component: PhotogalleryComponent, data:{ title: 'photogallery' } },
      { path: 'admin', loadChildren: '../admin/admin.module#AdminModule',canActivate:[AdminGuard] },
      { path: 'vehicle-management', loadChildren: '../vehicle-mgmt/vehicle-mgmt.module#VehicleMgmtModule' },

      { path: '**', redirectTo: '/app', pathMatch: 'full' },
    ]
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
