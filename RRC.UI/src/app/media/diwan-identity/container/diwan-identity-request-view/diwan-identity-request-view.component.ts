import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';


@Component({
  selector: 'app-diwan-identity-request-view',
  templateUrl: './diwan-identity-request-view.component.html',
  styleUrls: ['./diwan-identity-request-view.component.scss']
})
export class DiwanIdentityRequestViewComponent implements OnInit {

  requestId:number;
  lang: any;
  screenStatus: any;
  constructor(public route: ActivatedRoute, public common: CommonService) {
    route.url.subscribe(() => {
      this.route.params.subscribe(params => this.requestId = +params.id);
      this.lang = route.snapshot.parent.parent.parent.parent.parent.params.lang;
      this.screenStatus = route.snapshot.data.title;
      // if (this.lang == 'en')
      //   this.common.language = 'English';
      // else
      //   this.common.language = 'Arabic';
    });

  }
  ngOnInit() {
  }

}
