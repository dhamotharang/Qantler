import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-complaints-suggestions-create',
  templateUrl: './complaints-suggestions-create.component.html',
  styleUrls: ['./complaints-suggestions-create.component.scss']
})
export class ComplaintsSuggestionsCreateComponent implements OnInit {

 lang: any;
  screenStatus: any;

  constructor(public route: ActivatedRoute, public common: CommonService) {
    route.url.subscribe(() => {
      this.lang = route.snapshot.parent.parent.parent.params.lang;
      this.screenStatus = route.snapshot.data.title;
      // if (this.lang == 'en')
      //   this.common.language = 'English';
      // else
      //   this.common.language = this.common.arabic.words['arabic'];
    });

  }
  ngOnInit() {
  }

}