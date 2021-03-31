import { CommonService } from './../../../../common.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cv-bank-form-view',
  templateUrl: './cv-bank-form-view.component.html',
  styleUrls: ['./cv-bank-form-view.component.scss']
})
export class CvBankFormViewComponent implements OnInit {
  lang: string;
  constructor(private common: CommonService) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
  }

}
