import {Component, OnInit} from '@angular/core';
import {DeviceDetectorService} from 'ngx-device-detector';
import {Router} from '@angular/router';

@Component({
  selector: 'app-not-supported',
  templateUrl: './not-supported.component.html',
  styleUrls: ['./not-supported.component.css']
})
export class NotSupportedComponent implements OnInit {

  constructor(private BrowserCheck: DeviceDetectorService, private router: Router) {
  }

  ngOnInit() {
    if (
      (this.BrowserCheck.browser.toLowerCase().includes('chrome') && this.BrowserCheck.browser_version.split('.')[0] > '55') ||
      (this.BrowserCheck.browser.toLowerCase().includes('firefox') && this.BrowserCheck.browser_version.split('.')[0] > '51') ||
      (this.BrowserCheck.browser.toLowerCase().includes('edge') && this.BrowserCheck.browser_version.split('.')[0] > '15') ||
      (this.BrowserCheck.browser.toLowerCase().includes('safari') && this.BrowserCheck.browser_version.split('.')[0] > '10')) {
      this.router.navigateByUrl('/');
    }
  }

}
