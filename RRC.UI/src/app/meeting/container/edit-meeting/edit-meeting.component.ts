import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-edit-meeting',
  templateUrl: './edit-meeting.component.html',
  styleUrls: ['./edit-meeting.component.scss']
})
export class EditMeetingComponent implements OnInit {
  lang: any;

  constructor(public common:CommonService) { 
    this.lang= this.common.currentLang;
  }
  ngOnInit() {
  }

}
