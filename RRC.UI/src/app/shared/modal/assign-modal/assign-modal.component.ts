import { Component, OnInit, TemplateRef, ViewChild, Input, Renderer2, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';

import { BsModalRef } from 'ngx-bootstrap/modal';
import { MemoService } from 'src/app/memo/services/memo.service';
import { CommonService } from 'src/app/common.service';
import { AssignService } from '../../service/assign.service';
import { DOCUMENT } from '@angular/common';
import { ArabicDataService } from 'src/app/arabic-data.service';

@Component({
  selector: 'app-assign-modal',
  templateUrl: './assign-modal.component.html',
  styleUrls: ['./assign-modal.component.scss']
})

export class AssignModalComponent implements OnInit {
  @ViewChild('template') template: TemplateRef<any>;
  list: any = [];
  message: any;
  submitted: boolean = false;
  commonMes: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  status: string;
  AssigneId = null;
  @Input() identity: string; // we can set the default value also
  @Input() ApiString: string;
  @Input() ApiTitleString: string;
  @Input() id: number;
  @Input() redirectUrl: string;
  @Input() comments: string = '';
  userList: any[];
  isAssigned = true;
  inProgress: boolean = false;
  lang:string;
  arWords:any;
  placeholderdep: string;

  constructor(
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document,
    public router: Router,
    public modalService: BsModalService,
    public bsModalRef: BsModalRef,
    public assignService: AssignService,
    public commonService: CommonService,
    public memoservice: MemoService,
    public arabic: ArabicDataService) {
    this.lang = this.commonService.currentLang;
    if(this.lang == 'en') {
      this.placeholderdep = "Select a user"
    }else {
      this.placeholderdep = "اختر مستخدم"
    }
    this.arWords = this.arabic.words;    
    this.getUserList();
  }

  ngOnInit() {
  }

  formatPatch(val, path) {
    var data = [{
      "value": val,
      "path": path,
      "op": "replace"
    },
    {
      "value": this.AssigneId,
      "path": "AssigneeId",
      "op": "replace"
    },
    {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "replace"
    }, {
      "value": this.comments,
      "path": "Comments",
      "op": "replace"
    }];
    return data;
  }

  saveStatus(status: any) {
    this.inProgress = true;
    var data = this.formatPatch(status, 'Action');
    this.commonMes = status;
    this.assignService.statusChange(this.ApiString, this.id, data).subscribe(data => {
      this.close();
      this.modalService.show(this.template);
      let newSubscriber = this.modalService.onHide.subscribe((data: any) => {
        if (data == "backdrop-click") {
          newSubscriber.unsubscribe();
          this.closemodal();
        }
      });

    });
  }

  onChangeUser() {
    if (this.AssigneId) {
      this.isAssigned = false;
    } else {
      this.isAssigned = true;
    }
  }

  getUserList() {
    let that = this;
    let params = [{
      "OrganizationID": this.currentUser.OrgUnitID,
      "OrganizationUnits": this.currentUser.department
    }];
    this.AssigneId = null;
    this.commonService.getAssigneeUserList(params, 0).subscribe(data => {
      if (data) {
        that.list = data;
        that.userList = []
        for (var i = 0; i < that.list.length; i++) {
          that.userList.push(that.list[i]);
        }
      }
    });
  }

  close() {
    this.bsModalRef.hide();
  }

  closemodal() {
    this.inProgress = false;
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    var lang = (this.commonService.language == 'English') ? 'en' : 'ar',
      redirectURL = '/' + lang + '/' + this.redirectUrl;
    this.router.navigate([redirectURL]);
  }
}
