import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-gift-request-create',
  templateUrl: './gift-request-create.component.html',
  styleUrls: ['./gift-request-create.component.scss']
})
export class GiftRequestCreateComponent implements OnInit {
  lang: string;
  constructor(
    private activeRoute: ActivatedRoute,
    private common: CommonService
  ) {
    activeRoute.url.subscribe(() => {
      this.lang = activeRoute.snapshot.parent.parent.parent.parent.parent.params.lang;
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
