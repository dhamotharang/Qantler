import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { PageHeaderComponent } from 'src/app/layout/components/english/page-header/page-header.component';
import { PageHeaderComponentRTL } from '../../layout/components/arabic/page-header-rtl/page-header.component.rtl';

@Component({
  selector: 'app-notification-modal',
  templateUrl: './notification-modal.component.html',
  styleUrls: ['./notification-modal.component.scss'],
  providers: []
})
export class NotificationModalComponent implements OnInit {
  ReferenceNumber: any;
  ServiceID: any;
  Subject: any;
  Content: any;
  lang: string;
  modalTitle: any;
  constructor(public bsModalRef: BsModalRef,public modalService: BsModalService,public commonService:
    CommonService) { }

  ngOnInit() {
    this.lang = this.commonService.currentLang;
    this.modalTitle = (this.lang == 'ar') ? 'تفصيل الإشعار': 'Notification Detail';
  }

  async closemodal() {
    this.bsModalRef.hide();
  }

}
