import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-it',
  templateUrl: './it.component.html',
  styleUrls: ['./it.component.scss']
})
export class ItComponent implements OnInit {

  constructor(public router: Router, public route: ActivatedRoute) { }

  ngOnInit() {
  }

}
