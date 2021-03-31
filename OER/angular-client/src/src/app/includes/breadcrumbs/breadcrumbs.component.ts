import {Component, Input, OnInit} from '@angular/core';

interface List {
  text: string;
  link: string | null;
  active: boolean;
}

@Component({
  selector: 'app-breadcrumbs',
  templateUrl: './breadcrumbs.component.html'
})
export class BreadcrumbsComponent implements OnInit {
  @Input() items: List[];

  constructor() {
  }

  ngOnInit() {
  }

}
