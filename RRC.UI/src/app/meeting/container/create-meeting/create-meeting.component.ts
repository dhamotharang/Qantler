import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-create-meeting',
  templateUrl: './create-meeting.component.html',
  styleUrls: ['./create-meeting.component.scss']
})
export class CreateMeetingComponent implements OnInit {
  lang: any;

  constructor(public common:CommonService) { 
    this.lang= this.common.currentLang;
  }

  ngOnInit() {
  }

}
