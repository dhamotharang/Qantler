import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-memo-template',
  templateUrl: './memo-template.component.html'
})
export class MemoTemplateComponent implements OnInit {
  lang: any;

  constructor(public route:ActivatedRoute) { 
    route.url.subscribe(() => {
      console.log(route.snapshot.data);
      this.lang = route.snapshot.params.lang;
     // this.screenStatus = route.snapshot.data.title;
    });
  }

  ngOnInit() {
  }

}
