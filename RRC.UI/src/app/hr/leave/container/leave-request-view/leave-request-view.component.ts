import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-leave-request-view',
  templateUrl: './leave-request-view.component.html',
  styleUrls: ['./leave-request-view.component.scss']
})
export class LeaveRequestViewComponent implements OnInit {
  lang: string;
  requestId:Number;
  constructor(private route: ActivatedRoute, private commonService:CommonService) {
    this.route.params.subscribe(params => this.requestId = +params.id);
    route.url.subscribe(() => {
      this.lang = route.snapshot.data && route.snapshot.data.dir;
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
