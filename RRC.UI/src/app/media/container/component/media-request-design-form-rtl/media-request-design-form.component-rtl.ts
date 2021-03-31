import { Component, OnInit,TemplateRef,ViewChild,ElementRef,ChangeDetectorRef } from '@angular/core';
import { MediaRequestDesignFormComponent } from '../media-request-design-form/media-request-design-form.component';


@Component({
  selector: 'app-media-request-design-form-rtl',
  templateUrl: './media-request-design-form.component-rtl.html',
  styleUrls: ['./media-request-design-form.component-rtl.scss']
})
export class MediaRequestDesignFormComponentRTL extends MediaRequestDesignFormComponent {
  requestfordesign:any;
  title: any;
  initiative: any;
  project: any;
  activity: any;
  diwansrole: any;
  otherparties: any;

  // above will be temporory Fix
}
