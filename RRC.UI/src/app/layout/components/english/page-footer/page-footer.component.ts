import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-page-footer',
  templateUrl: './page-footer.component.html',
  styleUrls: ['./page-footer.component.scss']
})
export class PageFooterComponent implements OnInit {
 
  constructor(public common: CommonService) { }

  ngOnInit() {
  }
  showContact() {
    debugger
      this.common.IsContactSearch.next(true)
  }

  toggleSmartSearch() {
    debugger
     this.common.IsShowSearch.next(true)
  }
  ngOnDestroy() {
    debugger
    this.common.IsShowSearch.next(false)
    this.common.IsContactSearch.next(false)
  }

}
