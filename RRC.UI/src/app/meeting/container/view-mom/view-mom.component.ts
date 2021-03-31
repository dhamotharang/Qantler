import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-view-mom',
  templateUrl: './view-mom.component.html',
  styleUrls: ['./view-mom.component.scss']
})
export class ViewMomComponent implements OnInit {
  lang: any;

  constructor(public common:CommonService) { 
    this.lang= this.common.currentLang;
  }

  ngOnInit() {
  }

}
