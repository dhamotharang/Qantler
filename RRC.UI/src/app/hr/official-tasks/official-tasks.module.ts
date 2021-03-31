import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OfficialTasksRoutingModule } from './official-tasks-routing.module';
import { OfficialTaskFormComponent } from './components/official-task-form/official-task-form.component';
import { OfficialTaskComponent } from './containers/official-task/official-task.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from 'src/app/shared/shared.module';
import { OfficialTaskService } from './services/official-task.service';
import { CompensationModalComponent } from './modal/compensation-modal/compensation-modal.component';

@NgModule({
  declarations: [OfficialTaskFormComponent, OfficialTaskComponent, CompensationModalComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    SharedModule,
    OfficialTasksRoutingModule
  ],
  entryComponents: [
    CompensationModalComponent
  ],
  providers: [OfficialTaskService]
})
export class OfficialTasksModule { }
