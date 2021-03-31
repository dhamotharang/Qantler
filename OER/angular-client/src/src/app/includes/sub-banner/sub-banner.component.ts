import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-sub-banner',
  templateUrl: './sub-banner.component.html'
})
export class SubBannerComponent implements OnInit {
  @Input() title: string;
  @Input() description: string;

  constructor() {
  }

  ngOnInit() {
  }

}
