import { CommonService } from './../../../../common.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-view-event',
  templateUrl: './view-event.component.html',
  styleUrls: ['./view-event.component.scss']
})
export class ViewEventComponent implements OnInit {
  lang: string;

  constructor(
    public common: CommonService
  ) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
  }

}
