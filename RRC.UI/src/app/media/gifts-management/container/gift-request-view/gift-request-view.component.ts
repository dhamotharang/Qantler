import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-gift-request-view',
  templateUrl: './gift-request-view.component.html',
  styleUrls: ['./gift-request-view.component.scss']
})
export class GiftRequestViewComponent implements OnInit {

  requestId:number;
  lang:string;
  constructor(private route:ActivatedRoute,private common: CommonService) { 
    this.route.params.subscribe(params => this.requestId = +params.id); 
    route.url.subscribe(() => {
      this.lang = route.snapshot.parent.parent.parent.parent.parent.params.lang;
      // if (this.lang == 'en')
      //   this.common.language = 'English';
      // else
      //   this.common.language = 'Arabic';
    });
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
  }

}
