import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {WCMService} from '../../services/wcm.service';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html'
})
export class FooterComponent implements OnInit {

  WCMs: any[];

  constructor(private WcmService: WCMService, public router: Router) {
  }

  ngOnInit() {
    this.WCMs = [];
    this.getWCM();
    $(window).scroll(function () {
      if ($(this).scrollTop() > 100) {
        $('.goToTop').fadeIn();
      } else {
        $('.goToTop').fadeOut();
      }
    });
    $('.goToTop').click(function () {
      $('html, body').animate({scrollTop: 0}, 1000);
      return false;
    });
  }

  getWCM() {
    const categoryId: number =1;
    this.WcmService.getAllList(categoryId).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.WCMs = res.returnedObject;
      }
    });
  }

  getRouteLink(item) {
    if (item.pageName === 'About Organisation') {
      return 'about-manara-platform';
    } else {
      return item.pageName.replace(/ /g, '-').toLowerCase();
    }
  }
}
