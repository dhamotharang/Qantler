import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-training-request-create',
  templateUrl: './training-request-create.component.html',
  styleUrls: ['./training-request-create.component.scss']
})
export class TrainingRequestCreateComponent implements OnInit {
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
