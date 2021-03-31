import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { LayoutComponent } from '../../layout/layout.component';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { id } from '@swimlane/ngx-datatable/release/utils';
import { MemoService } from 'src/app/memo/services/memo.service';
import { CitizenAffairService } from 'src/app/citizen-affair/service/citizen-affair.service';
import { IncomingLetterService } from 'src/app/letter/container/component/incoming-letter-form/incoming-letter-form.service';
import { MediaRequestPhotoService } from 'src/app/media/container/component/media-request-photo/media-request-photo.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { TaskService } from 'src/app/task/service/task.service';

//import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'assignmodal',
  templateUrl: './assignmodal.component.html',
  styleUrls: ['./assignmodal.component.scss'],
  providers: []
})
export class AssignModalComponent implements OnInit {
  @ViewChild('template') template: TemplateRef<any>;
  status: any;
  list: any = [];
  saveModalRef: BsModalRef;
  userarray: any = [];
  modal: any = {
    UserID: '',
    DepartmentID: ''
  };
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  link: any;
  ActionTaken: any;
  page: any;
  fromScreen: any;
  message: any;
  params: any = [];
  id = 0;
  HRComplaintSuggestionId = 0;
  CAComplaintSuggestionId = 0;
  TaskID = 0;
  submitted = true;
  submittedload = false;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  paramsarray = [];
  department: any;
  lang: string;
  arWords: any;

  constructor(
    public router: Router,
    public modalService: BsModalService,
    public bsModalRef: BsModalRef,
    public commonService: CommonService,
    public memoservice: MemoService,
    public citizenService: CitizenAffairService,
    public incomingletterservice: IncomingLetterService,
    public mediaphotoservice: MediaRequestPhotoService,
    public dutyTask: TaskService,
    public arabic: ArabicDataService) {
    this.lang = this.commonService.currentLang;
    this.arWords = this.arabic.words;
  }

  ngOnInit() {
    setTimeout(() => {
      this.getUserList();
    }, 200);
  }



  getUserList() {
    let that = this;

    let params = [{
      "OrganizationID": this.currentUser.OrgUnitID,//localStorage.getItem("UserDepartment"),
      "OrganizationUnits": "string"
    }];

    if (this.page == 'CitizenAffair') {
      params = [
        {
          "OrganizationID": 5,//localStorage.getItem("UserDepartment"),
          "OrganizationUnits": "string"
        },
        {
          "OrganizationID": 6,//localStorage.getItem("UserDepartment"),
          "OrganizationUnits": "string"
        },
        {
          "OrganizationID": 7,//localStorage.getItem("UserDepartment"),
          "OrganizationUnits": "string"
        },
        {
          "OrganizationID": 8,//localStorage.getItem("UserDepartment"),
          "OrganizationUnits": "string"
        }
      ];
    }
    //this.list = this.commonService.getUserList({{{}}});

    this.commonService.getAssigneeUserList(params, 0)
      .subscribe(data => {
        if (data) {
          that.list = data;
        }

      });
  }

  saveStatus() {
    this.submitted = true;
    this.submittedload = true;
    if (!this.modal.UserID || this.modal.UserID === null) {
      //alert("Please Select User");
      this.submitted = true;
      this.submittedload = false;
      return;
    }
    var data = this.formatPatch('AssignTo', 'Action');
    if (this.page == 'Dutytask') {
      var data = this.formatPatchTask('Assign', 'Action');
    }
    let that = this;
    let method: any;
    let url: any;
    if (this.page == 'Dutytask') {
      this.link = 'DutyTask';
      this.id = this.TaskID;
      url = this.dutyTask.patchTask(this.link, data, this.id);
      // this.citizenService.statusChange(this.link, this.id, data);
    }
    if (this.page == 'CitizenAffair') {
      this.link = 'CAComplaintSuggestion';
      this.id = this.CAComplaintSuggestionId;

      url = this.citizenService.statusChange(this.link, this.id, data);
    } else if (this.page == 'HR') {
      this.link = 'HRComplaintSuggestion';
      this.id = this.HRComplaintSuggestionId;

      url = this.citizenService.statusChange(this.link, this.id, data);
    } else if (this.page == 'Media') {
      if (this.fromScreen == 'PhotoMedia') {
        this.link = 'Photo';
        this.fromScreen = 'Photo Request';
      } else if (this.fromScreen == 'Media Request Design') {
        this.link = 'Design';
        this.fromScreen = 'Media Request for Design';
      }

      this.id = this.HRComplaintSuggestionId;
      url = this.mediaphotoservice.statusChange(this.link, this.id, data);
    }
    url.subscribe(data => {
      that.message = this.fromScreen + ' ' + ' Assigned' + ' Successfully';
      if (data) {
        if (that.lang == 'ar') {
          if (that.link == 'CAComplaintSuggestion' || that.link == 'HRComplaintSuggestion') {
            that.message = that.arWords.complaintsuggestionassigned;
          } else if (that.link == 'Design') {
            that.message = that.arWords.designassignedmsg;
          } else if (that.link == 'Photo') {
            that.message = that.arWords.photoassignsuccessmsg;
          } else if (this.page == 'Dutytask') {
            that.message = that.arWords.taskassignsuccessmsg;
          }
        }
        that.saveModalRef = that.modalService.show(that.template, this.config);
        that.submitted = true;
        that.params = [];
        that.userarray = [];
        that.modal = {};

        that.bsModalRef.hide();
      }

    });
    //that.submitted = false;
  }

  formatPatch(val, path) {
    var data = [{
      "value": val,
      "path": path,
      "op": "replace"
    }, {
      "value": this.modal.UserID,
      "path": 'AssigneeId',
      "op": 'replace'
    }, {
      "value": this.currentUser.id,
      "path": 'UpdatedBy',
      "op": 'replace'
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "replace"
    }, {
      "value": this.ActionTaken,
      "path": "Comments",
      "op": "replace"
    }];
    return data;
  }

  formatPatchTask(val, path) {
    var data = [{
      "value": val,
      "path": path,
      "op": "replace"
    }, {
      "value": this.currentUser.id,
      "path": 'UpdatedBy',
      "op": 'replace'
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "replace"
    }];

    // , {
    //   "value": this.ActionTaken,
    //   "path": "Comments",
    //   "op": "replace"
    // }
    return data;
  }

  close() {
    // location.reload();
    this.params = [];


    this.modal = {};
    this.bsModalRef.hide();
  }

  closemodal() {
    this.saveModalRef.hide();
    if (this.page == 'CitizenAffair') {
      this.router.navigate([this.commonService.currentLang + '/app/citizen-affair/citizen-affair-list']);
      return;
    } else if (this.page == 'HR') {
      this.router.navigate(['app/hr/dashboard']);
      return;
    } else if (this.page == 'Media') {
      this.router.navigate(['app/media/protocol-home-page']);
      return;
    } else if (this.page == 'Dutytask') {
      this.router.navigate([this.commonService.currentLang + '/app/task/task-dashboard']);
      return;
    }
    //setTimeout(function () { location.reload(); }, 1000);

  }



  selectionChanged(event) {
    let name = '';
    if (event != undefined) {
      name = event.EmployeeName;
    }
    if (name == 'Select a user' || name == '') {
      this.submitted = true;
    } else {
      let user = this.list.filter((value) => value.EmployeeName == name);
      this.modal.UserID = parseInt(user[0].UserID);
      this.modal.DepartmentID = parseInt(user[0].OrgUnitID);
      this.submitted = false;
    }
  }


  arabicLang(word) {
    return this.commonService.arabic.words[word];
  }


}
