import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-media-press-release-request',
  templateUrl: './media-press-release-request.component.html',
  styleUrls: ['./media-press-release-request.component.scss']
})
export class MediaPressReleaseRequestComponent implements OnInit {
  lang: any;
  screenStatus: any;

  constructor(public route: ActivatedRoute, public common: CommonService) {
    route.url.subscribe(() => {
      this.lang = route.snapshot.parent.parent.parent.parent.parent.params.lang;
      this.screenStatus = route.snapshot.data.title;
      // if (this.lang == 'en') {
      //   this.common.language = 'English';
      // } else if (this.lang == 'ar') {
      //   this.common.language = 'Arabic';
      // }
    });
  }

  ngOnInit() {
  }

}
