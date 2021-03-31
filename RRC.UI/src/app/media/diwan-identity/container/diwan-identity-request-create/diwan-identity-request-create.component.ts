import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-diwan-identity-request-create',
  templateUrl: './diwan-identity-request-create.component.html',
  styleUrls: ['./diwan-identity-request-create.component.scss']
})
export class DiwanIdentityRequestCreateComponent implements OnInit {

  lang: any;
  screenStatus: any;

  constructor(public route: ActivatedRoute, public common: CommonService) {
    route.url.subscribe(() => {
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
