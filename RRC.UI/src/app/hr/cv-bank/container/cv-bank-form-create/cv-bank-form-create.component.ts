import { CommonService } from './../../../../common.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cv-bank-form-create',
  templateUrl: './cv-bank-form-create.component.html',
  styleUrls: ['./cv-bank-form-create.component.scss']
})
export class CvBankFormCreateComponent implements OnInit {
  lang: string;
  constructor(private common: CommonService) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
  }

}
