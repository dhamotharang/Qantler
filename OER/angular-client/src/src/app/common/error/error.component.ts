import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {Title} from '@angular/platform-browser';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html'
})
export class ErrorComponent implements OnInit {
  code: number;

  constructor(private titleService: Title, private route: ActivatedRoute, private spinner: NgxSpinnerService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Error | UAE - Open Educational Resources');
    this.route.params.subscribe((params) => {
      if (params['code']) {
        this.code = +params['code'];
      }
      this.spinner.hide();
    });
  }

  getTitle() {
    if (this.code === 401) {
      return 'Inactive Account';
    } else if (this.code === 403) {
      return 'Forbidden';
    } else if (this.code === 404) {
      return 'Not Found';
    } else {
      return 'Error';
    }
  }

  getDescription() {
    if (this.code === 401) {
      return 'Please contact site Administrator';
    } else if (this.code === 403) {
      return 'Access is forbidden to the requested page.';
    } else if (this.code === 404) {
      return 'The page you requested is not Found';
    } else {
      return 'Error Unknown';
    }
  }

}
