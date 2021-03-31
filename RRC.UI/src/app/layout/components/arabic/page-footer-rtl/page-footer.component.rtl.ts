import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-page-footer-rtl',
  templateUrl: './page-footer.component.rtl.html',
  styleUrls: ['./page-footer.component.rtl.scss']
})
export class PageFooterComponentRTL implements OnInit {

  constructor(public common: CommonService) { }
  ngOnInit() {
  }
showContact() {
    this.common.IsContactSearchrtl.next(true)
}

toggleSmartSearch() {
   this.common.IsShowSearchrtl.next(true)
}

ngOnDestroy() {
  this.common.IsShowSearchrtl.next(false)
  this.common.IsContactSearchrtl.next(false)
}

}
