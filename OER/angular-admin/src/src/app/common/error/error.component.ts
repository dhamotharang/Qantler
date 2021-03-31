import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {
  code: number;

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      if (params['code']) {
        this.code = +params['code'];
      }
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

}
