import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { LayoutComponent } from '../../layout/layout.component';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { id } from '@swimlane/ngx-datatable/release/utils';
import { MemoService } from 'src/app/memo/services/memo.service';
import { CircularService } from 'src/app/circular.service';
//import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'common-modal',
  templateUrl: './commonmodal.component.html',
  styleUrls: ['./commonmodal.component.scss'],
  providers: []
})
export class CommonModalComponent implements OnInit {
  @ViewChild('template') template: TemplateRef<any>;


  data: any;

  fromScreen: any;
  language: any = 'English';


  constructor(public router: Router, public modalService: BsModalService, public bsModalRef: BsModalRef,
    public commonService: CommonService, public memoservice: MemoService, public circularService: CircularService,
    private route: ActivatedRoute) {

      this.language = this.commonService.language;

  }

  ngOnInit() {

  }




  closemodal() {
    this.bsModalRef.hide();
    // setTimeout(function(){ location.reload(); }, 1000);
  }

  routepage(name) {
    if (name.lang) {
      this.router.navigate([name.lang + '/app/' + name.pagename + '/' + name.url], { relativeTo: this.route });
    } else {
      this.router.navigate(['app/' + name.pagename + '/' + name.url], { relativeTo: this.route });
    }
    this.bsModalRef.hide();
  }


  arabic(word){
    return this.commonService.arabic.words[word];
  }




}
