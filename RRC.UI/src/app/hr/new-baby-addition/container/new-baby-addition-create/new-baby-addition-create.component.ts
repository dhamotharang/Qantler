import { CommonService } from 'src/app/common.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-new-baby-addition-create',
  templateUrl: './new-baby-addition-create.component.html',
  styleUrls: ['./new-baby-addition-create.component.scss']
})
export class NewBabyAdditionCreateComponent implements OnInit {
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
