import { Component, OnInit, ViewChild, TemplateRef, Input, Renderer2, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { AssignService } from '../../service/assign.service';
import { UtilsService } from '../../service/utils.service';
import { DOCUMENT } from '@angular/common';
import { ReportsService } from '../../service/reports.service';

@Component({
  selector: 'app-escalate-modal',
  templateUrl: './escalate-modal.component.html',
  styleUrls: ['./escalate-modal.component.scss']
})
export class EscalateModalComponent implements OnInit {
  @ViewChild('template') template: TemplateRef<any>;
  list: any = [];
  // message : any;
  submitted: boolean = false;
  commonMes: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  status: string;
  ApproverId = null;
  @Input() identity: string; // we can set the default value also
  @Input() ApiString: string;
  @Input() ApiTitleString: string;
  @Input() id: number;
  @Input() module: string;
  @Input() Title: string;
  @Input() comments: string;
  @Input() message: any;
  @Input() redirectPath: string;
  @Input() approverNode: string = 'ApproverId';
  @Input() isDepartmentNotNeeded:boolean = false;
  @Input() departmentOrgID:any = null;
  @Input() isFirstApprover:boolean = false;
  userList: any[] = [];
  department: any[] = [];
  departmentSel: any;
  inProgress: boolean = false;
  lang: string;

  constructor(public router: Router,
    public modalService: BsModalService,
    public bsModalRef: BsModalRef,
    public assignService: AssignService,
    public commonService: CommonService,
    private reportsService: ReportsService,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document, ) {
    this.reportsService.getLeaveRequestById(0, 0).subscribe((data: any) => {
      if(this.isFirstApprover){
        this.department = data.M_ApproverDepartmentList;
      }else{
        this.department = data.OrganizationList;
      }
    });

  }

  ngOnInit() {
    this.lang = this.commonService.currentLang;
    if(this.isDepartmentNotNeeded){
      this.getUserList();
    }
   }

  formatPatch(val, path) {
    var data = [{
      "value": val,
      "path": path,
      "op": "replace"
    },
    {
      "value": this.ApproverId,
      "path": this.approverNode,
      "op": "Replace"
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
      // this.message = 'Leave Request Escalated';
      // this.router.navigate(['app/hr/dashboard']);
      this.modalService.show(this.template);
      let newSubscriber = this.modalService.onHide.subscribe((data: any) => {
        if (data == "backdrop-click") {
          newSubscriber.unsubscribe();
          this.closemodal();
        }
      });
    });
  }

  getUserList(data?: any) {
    let that = this;
    // let toSenDep = data;
    // let params = [{
    //   "OrganizationID": this.departmentSel, //localStorage.getItem("UserDepartment"),
    //   "OrganizationUnits": ""
    // }];
    let toSenDep = this.departmentSel;
    // this.commonService.getUserList(params, this.currentUser.id).subscribe((res: any) => {
    //     this.userList = res;
    // });
    if(this.isDepartmentNotNeeded){
      toSenDep = this.departmentOrgID;
      this.isFirstApprover = true;
    }
    let params = [{
      "OrganizationID": toSenDep, //localStorage.getItem("UserDepartment"),
      "OrganizationUnits": ""
    }];
    this.userList = [];
    this.ApproverId = '';
    if(!this.isFirstApprover){
      this.commonService.getUserList(params, this.currentUser.id).subscribe(data => {
        if (data) {
          that.list = data;
          that.userList = [];
          for (var i = 0; i < that.list.length; i++) {
            // if(!that.list[i].IsOrgHead) {
            that.userList.push(that.list[i]);
          }
        }
      });
    } else{
      this.commonService.getmemoUserList(params, this.currentUser.id).subscribe(data => {
        if (data) {
          that.list = data;
          that.userList = [];
          for (var i = 0; i < that.list.length; i++) {
            // if(!that.list[i].IsOrgHead) {
            that.userList.push(that.list[i]);
          }
        }
      });
    }
  }

  close() {
    this.bsModalRef.hide();
    this.inProgress = false;
    this.commonService.setEscalateModalClose('close');
  }

  closemodal() {
    this.inProgress = false;
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    var lang = (this.commonService.language == 'English') ? 'en' : 'ar',
      redirectURL = '/' + lang + '/' + this.redirectPath;
    this.router.navigate([redirectURL]);
    this.commonService.setEscalateModalClose('close');
  }

  onChangeDepartment(data) {
    this.getUserList(+data);
  }

  arabicfn(word) {
    return this.commonService.arabic.words[word];
  }
}
