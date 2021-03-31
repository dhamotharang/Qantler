import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-hr',
  templateUrl: './hr.component.html',
  styleUrls: ['./hr.component.scss']
})
export class HrComponent implements OnInit {
  screenStatus: any;

  constructor(public router: Router,route: ActivatedRoute) {
    route.url.subscribe(() => {
      this.screenStatus = route.snapshot.data.title;
    });
   }

  ngOnInit() {
  }

}
