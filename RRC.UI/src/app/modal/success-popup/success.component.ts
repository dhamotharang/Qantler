import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { LayoutComponent } from '../../layout/layout.component';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { id } from '@swimlane/ngx-datatable/release/utils';
import { MemoService } from 'src/app/memo/services/memo.service';
import { Subject } from 'rxjs';
//import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'bs-modal-backdrop',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.scss'],
  providers: []
})
export class SuccessComponent implements OnInit {

  status: any;
  list: any = [];
  language: any;
  message: any;
  pagename: any;
  params: any = [];
  page_url: any;
  memoid: any;
  submitted: boolean = false;
  screenStatus: any;
  incoming: boolean = false;
  public trigger = new Subject<any>();
  public closeEvent = new Subject<any>();

  paramsarray = [];
  department: any;
  lang: string;
  msg: string;
  constructor(
    public router: Router,
    public modalService: BsModalService,
    public bsModalRef: BsModalRef,
    public commonService:
      CommonService,
    public memoservice:
      MemoService,
    public route: ActivatedRoute
  ) {
  }

  ngOnInit() {
    this.lang = this.commonService.currentLang;
    setTimeout(() => {
      this.msg = this.message;
    }, 200);
  }


  close() {
    // location.reload();
    this.params = [];


    this.bsModalRef.hide();
  }

  async closemodal() {
    this.bsModalRef.hide();
    this.closeEvent.next();
    let param: any;
    let name = '';
    let btnname = '';
    let list = '';
    let create = '',
      lang = (this.commonService.language == 'English') ? 'en' : 'ar';
    let pageurl = '/' + lang + '/' + this.page_url;
    if (this.pagename == 'memo') {
      //await this.router.navigate(['/app/memo/memo-list'], { relativeTo: this.route });
      this.commonService.pageReLoad('memo-list');
      return;
    }
    else if (this.pagename == 'Circular') {
      this.commonService.pageReLoad('circular-list');
      return;

    } else if (this.pagename == 'Letter') {
      if (!this.incoming) {
        if (lang == 'ar')
          this.router.navigate(['/ar/app/letter/letter-list']);
        else
          this.commonService.pageReLoad('letter-list');
      }else{
        this.trigger.next();
      }
      return;
    } else if (this.pagename == 'Task') {
      this.commonService.pageReLoad('task-dashboard');
      return;
    }
    else if (this.pagename == 'Citizen Affair' || this.pagename == 'CitizenAffair') {
      if (this.language == 'ar')
        this.router.navigate(['/ar/app/citizen-affair/citizen-affair-list']);
      else
        this.router.navigate(['/en/app/citizen-affair/citizen-affair-list']);
      return;
    } else if (this.pagename == 'HR') {
      //this.commonService.pageReLoad('citizen-affair-list');
      this.router.navigate(['app/hr/dashboard']);
      return;
    }
    else if (this.pagename == 'Letter Clone') {
      //await this.router.navigate([this.page_url], { relativeTo: this.route });
      this.router.navigate([pageurl]);
      // window.open(pageurl);
      //await this.commonService.action(param);
      //await this.commonService.sideNavClick(param);
    } else if (this.pagename == 'Circular Clone') {
      //await this.router.navigate([pageurl], { relativeTo: this.route });
      this.router.navigate([pageurl]);
      //window.open(pageurl);
      //await this.commonService.action(param);
      //await this.commonService.sideNavClick(param);
    } else if (this.pagename == 'Memo Clone') {
      this.router.navigate([pageurl]);
      //window.open(pageurl);
    } else if (this.pagename == 'Media') {
      this.router.navigate(['app/media/protocol-home-page'], { relativeTo: this.route });
      //window.open(this.page_url);
    }
  }
}
