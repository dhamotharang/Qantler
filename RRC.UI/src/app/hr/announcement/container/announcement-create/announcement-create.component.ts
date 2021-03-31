import { CommonService } from './../../../../common.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-announcement-create',
  templateUrl: './announcement-create.component.html',
  styleUrls: ['./announcement-create.component.scss']
})
export class AnnouncementCreateComponent implements OnInit {
  lang: string;
  constructor( private common: CommonService) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
  }

}
