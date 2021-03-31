import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HrComponent } from './container/hr/hr.component';
import { HomePageComponent } from './container/home-page/home-page.component';
import { HrDocumentsComponent } from './container/hr-documents/hr-documents.component';
import { RaiseComplaintsSuggestionsComponent } from './container/raise-complaints-suggestions/raise-complaints-suggestions.component';
import { RaiseComplaintsSuggestionsCreateComponent } from './raise-complaints-suggestions-create/raise-complaints-suggestions-create.component';
import { DocumentsPageComponent } from '../shared/component/documents-page/documents-page.component';

const routes: Routes = [
  {
    path:'',component:HrComponent,
    children:[
      { path:'', redirectTo:'dashboard', pathMatch:'full' },
      { path:'dashboard',component:HomePageComponent },
      { path:'leave',loadChildren:'./leave/leave.module#LeaveModule' },
      { path:'new-baby-addition',loadChildren:'./new-baby-addition/new-baby-addition.module#NewBabyAdditionModule' },
      { path: 'announcement', loadChildren: './announcement/announcement.module#AnnouncementModule' },
      { path:'salary-certificate',loadChildren:'./salary-certificate/salary-certificate.module#SalaryCertificateModule' },
      { path:'experience-certificate',loadChildren:'./experience-certificate/experience-certificate.module#ExperienceCertificateModule' },
      { path: 'cv-bank', loadChildren: './cv-bank/cv-bank.module#CvBankModule' },
      { path:'training-request',loadChildren:'./training-request/training-request.module#TrainingRequestModule'},
      { path:'employee',loadChildren:'./employee-profile/employee-profile.module#EmployeeProfileModule' },
      { path: 'official-tasks', loadChildren: './official-tasks/official-tasks.module#OfficialTasksModule' },
      { path:'documents',component:DocumentsPageComponent, data: { documentType: 'HR', department:'Human Resources'}},
      { path: 'raise-complaint-suggestion', component: RaiseComplaintsSuggestionsCreateComponent, data: {title: 'Create'}},
      { path: 'raise-complaint-suggestion-view/:id', component: RaiseComplaintsSuggestionsCreateComponent, data: {title: 'View'}},
      // { path:'**', redirectTo:'/home', pathMatch:'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HrRoutingModule { }
