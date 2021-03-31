import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-create-mom',
  templateUrl: './create-mom.component.html',
  styleUrls: ['./create-mom.component.scss']
})
export class CreateMomComponent implements OnInit {
  lang: any;

  constructor(public common:CommonService) { 
    this.lang= this.common.currentLang;
  }

  ngOnInit() {
  }

}
