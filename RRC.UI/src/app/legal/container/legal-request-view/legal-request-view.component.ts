import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-legal-request-view',
  templateUrl: './legal-request-view.component.html',
  styleUrls: ['./legal-request-view.component.scss']
})
export class LegalRequestViewComponent implements OnInit {
  lang: any;
  screenStatus: any;
  requestId: Number;
  constructor(private route: ActivatedRoute, public common: CommonService) {
    this.route.params.subscribe(params => this.requestId = +params.id);
    route.url.subscribe(() => {
      this.lang = route.snapshot.parent.parent.parent.params.lang;
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
