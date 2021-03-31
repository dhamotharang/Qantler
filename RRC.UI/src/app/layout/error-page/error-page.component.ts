import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss']
})
export class ErrorPageComponent implements OnInit {
  lang: any;
  
  constructor(public common: CommonService) { 
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
  }

}
