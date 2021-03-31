import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-leave-request-create',
  templateUrl: './leave-request-create.component.html',
  styleUrls: ['./leave-request-create.component.scss']
})
export class LeaveRequestCreateComponent implements OnInit {
  lang: string;
  constructor(
    private activeRoute: ActivatedRoute,
    private commonService: CommonService
  ) {
    activeRoute.url.subscribe(() => {
      // this.lang = activeRoute.snapshot.data && activeRoute.snapshot.data.dir;
      // if (this.lang == 'en')
      //   this.common.language = 'English';
      // else
      //   this.common.language = 'Arabic';
    });
    this.lang = this.commonService.currentLang;
  }

  ngOnInit() {
  }

}
