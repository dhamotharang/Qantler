import { Component, OnInit, OnDestroy } from '@angular/core';
import { PageHeaderComponent } from './components/english/page-header/page-header.component';
import { Router, ActivatedRoute, RouterStateSnapshot } from '@angular/router';
import { CommonService } from '../common.service';
import { AuthService } from '../auth/auth.service';
@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy {

  screenStatus: any;
  currentUser: any;
  lang: any;
  languageSubscribe: any;

  constructor(public router: Router, public authService: AuthService, route: ActivatedRoute, public common: CommonService) {
    route.url.subscribe(() => {
      this.screenStatus = route.snapshot.data.title;
    });
    this.common.currentScreen = this.screenStatus;
    if (this.screenStatus != 'login')
      this.currentUser = JSON.parse(localStorage.getItem('User'));
    if (this.common.getCookie() == 'English')
      this.lang = 'en';
    else
      this.lang = 'ar';
  }

  ngOnInit() {
    this.languageSubscribe = this.common.langChange$.subscribe(res => {
      this.lang = res;
      this.common.currentLang = res;
    });
    if (this.router.url === '/' + this.lang + '/home') {
      this.common.homeScrollTop();
    }
  }

  ngOnDestroy() {
    if (this.languageSubscribe)
      this.languageSubscribe.unsubscribe();
  }

}
