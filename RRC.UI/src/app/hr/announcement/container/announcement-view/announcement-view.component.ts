import { CommonService } from './../../../../common.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-announcement-view',
  templateUrl: './announcement-view.component.html',
  styleUrls: ['./announcement-view.component.scss']
})
export class AnnouncementViewComponent implements OnInit {
  lang: string;
  constructor( private common: CommonService) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
  }

}
