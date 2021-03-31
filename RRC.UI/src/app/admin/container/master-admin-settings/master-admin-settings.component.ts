import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-master-admin-settings',
  templateUrl: './master-admin-settings.component.html',
  styleUrls: ['./master-admin-settings.component.scss']
})
export class MasterAdminSettingsComponent implements OnInit {
  lang: any;
  screenStatus: any;

  constructor(public route: ActivatedRoute, public common: CommonService) {
    route.url.subscribe(() => {
      this.lang = this.common.currentLang;
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
