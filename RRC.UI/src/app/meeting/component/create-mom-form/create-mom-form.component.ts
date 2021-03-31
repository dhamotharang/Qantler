import { Component } from '@angular/core';
import { MeetingFormComponent } from '../meeting-form/meeting-form.component';

@Component({
  selector: 'app-mom-form',
  templateUrl: './create-mom-form.component.html',
  styleUrls: ['./create-mom-form.component.scss']
})
export class CreateMomFormComponent extends MeetingFormComponent {
  // @Input() mode: string;
  // bsConfig: Partial<BsDatepickerConfig> = {
  // };
  // constructor(
  //   public common: CommonService
  // ) { }

  // ngOnInit() {
  //   console.log("mode", this.mode);
  //   this.common.breadscrumChange('Meeting', 'Create MOM', '');
  // }

}
