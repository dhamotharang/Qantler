import { CommonService } from './../../../../common.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.component.html',
  styleUrls: ['./create-event.component.scss']
})
export class CreateEventComponent implements OnInit {
  lang: string;

  constructor(
    public common: CommonService
  ) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
  }

}
