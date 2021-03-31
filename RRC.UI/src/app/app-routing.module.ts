import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { MemoComponent } from './pages/pages.component';
import { LoginComponent } from './auth/login/login.component';
import { TaskChatComponent } from './task/container/task-chat/task-chat.component';
import { AppComponent } from './app.component';
import { AuthGuard } from './auth/auth.guard';
import { ErrorPageComponent } from './layout/error-page/error-page.component';
import { LoginNewComponent } from './layout/components/login/login.component';


const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  // { path: ':lang/home', component: LayoutComponent, canActivate: [AuthGuard], data: { title: 'home' } },
  { path: ':lang/home', component: LayoutComponent, data: { title: 'home' } },
  { path: 'login', component: LayoutComponent, data: { title: 'login' } },
  { path: ':lang/login', component: LayoutComponent, data: { title: 'login' } },
  //{ path: 'login', component: LoginComponent},
  { path: ':lang/app', loadChildren: './layout/layout.module#LayoutModule' },
   { path: 'app', loadChildren: './layout/layout.module#LayoutModule' },
  // { path: ':type/:reference', component: AppComponent },
  { path: ':lang/:type/:mode/:reference', component: AppComponent },
  { path: ':lang/:type/:mode/:reference/:finePlateNum', component: AppComponent },
  { path: 'chat', component: TaskChatComponent },
  //{ path: ':lang/chat', component: TaskChatComponent }
  { path: 'error', component: ErrorPageComponent },

  // { path: 'pages', component: MemoComponent, },
  // { path: 'task-chat', component: TaskChatComponent },
  // {
  //   path: 'pages', component: MemoComponent,
  //   children: [
  //     { path: 'memo', loadChildren: './memo/memo.module#MemoModule' },
  //     { path: 'letter', loadChildren: './letter/letter.module#LetterModule' },
  //     { path: 'task', loadChildren: './task/task.module#TaskModule' },
  //     { path: 'circular', loadChildren: './circular/circular.module#CircularModule' },
  //     { path: 'citizen-affair', loadChildren: './citizen-affair/citizen-affair.module#CitizenAffairModule' },
  //     { path: 'media', loadChildren: './media/media.module#MediaModule' },
  //     { path: '**', redirectTo: '/pages', pathMatch: 'full' },
  //     // { path: 'memo', loadChildren: './memo/memo.module#MemoModule' },
  //     // { path: 'letter', loadChildren: './letter/letter.module#LetterModule' },
  //     // { path: 'task', loadChildren: './task/task.module#TaskModule' },
  //     // { path: 'circular', loadChildren: './circular/circular.module#CircularModule' },
  //     // { path: 'citizen-affair', loadChildren: './citizen-affair/citizen-affair.module#CitizenAffairModule' },
  //     // { path: 'media-request-list', component: MediaRequestListComponent },
  //     // { path: 'media-request-staff-list', component: MediaRequestStaffListComponent },
  //     // { path: 'media-request-design-form-creation', component: MediaRequestDesignFormComponent },
  //     // { path: '**', redirectTo: '/pages', pathMatch: 'full' },
  //   ]
  // },
  // {
  //   path: '',
  //   component: LayoutComponent,
  //   children: [
  //     { path: 'contacts', loadChildren: './contacts/contacts.module#ContactsModule' },
  //     { path: 'hr', loadChildren: './hr/hr.module#HrModule' },
  //     { path: 'it', loadChildren: './it/it.module#ItModule' },
  //     { path: 'maintenance', loadChildren: './maintenance/maintenance.module#MaintenanceModule' },
  //     { path: 'contacts', loadChildren: './contacts/contacts.module#ContactsModule' },
  //     { path: 'legal', loadChildren: './legal/legal.module#LegalModule' },
  //   ]
  // },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
