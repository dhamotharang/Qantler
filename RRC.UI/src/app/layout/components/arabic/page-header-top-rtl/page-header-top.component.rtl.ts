import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';
import { PageHeaderTopComponent } from '../../english/page-header-top/page-header-top.component';
import { OwlOptions } from 'ngx-owl-carousel-o';

@Component({
  selector: 'app-page-header-top-rtl',
  templateUrl: './page-header-top.component.rtl.html',
  styleUrls: ['./page-header-top.component.rtl.scss']
})

export class PageHeaderTopComponentRTL extends PageHeaderTopComponent {
  carouselOptions: OwlOptions = {
    loop: true,
    rtl: true,
    // mouseDrag: false,
    // touchDrag: false,
    // pullDrag: false,
    dots: false,
    navSpeed: 100,
    autoplay: true,
    responsive: {
      0: {
        items: 1
      },
      400: {
        items: 1
      },
      740: {
        items: 1
      },
      940: {
        items: 1
      }
    },
    nav: false
  }

}
