import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-media-request-photo-create',
  templateUrl: './media-request-photo-create.component.html',
  styleUrls: ['./media-request-photo-create.component.scss']
})
export class MediaRequestPhotoCreateComponent implements OnInit {

  lang: any;
  screenStatus: any;

  constructor(public route: ActivatedRoute, public common: CommonService) {
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
