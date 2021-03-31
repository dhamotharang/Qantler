import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-view-meeting',
  templateUrl: './view-meeting.component.html',
  styleUrls: ['./view-meeting.component.scss']
})
export class ViewMeetingComponent implements OnInit {
  lang: any;

  constructor(public common:CommonService) { 
    this.lang= this.common.currentLang;
  }

  ngOnInit() {
  }

}
