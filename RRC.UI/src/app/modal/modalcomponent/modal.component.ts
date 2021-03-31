import { Component, OnInit, TemplateRef, ViewChild, AfterViewInit } from '@angular/core';
import { LayoutComponent } from '../../layout/layout.component';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { id } from '@swimlane/ngx-datatable/release/utils';
import { MemoService } from 'src/app/memo/services/memo.service';
import { CircularService } from 'src/app/circular.service';
import { IncomingLetterService } from 'src/app/letter/container/component/incoming-letter-form/incoming-letter-form.service';
import { Subject, iif } from 'rxjs';
import { MediaRequestPhotoService } from 'src/app/media/container/component/media-request-photo/media-request-photo.service';
import { CitizenAffairService } from 'src/app/citizen-affair/service/citizen-affair.service';
import { environment } from 'src/environments/environment';
//import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
  providers: []
})
export class ModalComponent implements OnInit {
  @ViewChild('template') template: TemplateRef<any>;
  environment = environment;
  status: any;
  screenStatus = '';
  list: any = [];
  userarray: any = [];
  modal: any = {
    UserID: '',
    DepartmentID: ''

  };
  language = 'en';
  departmentSel = [];
  userSel = [];
  fromScreen: any;
  message: any;
  Comments: any;
  params: any = [];
  memoid: any;
  submitted = false;
  button = 'Save';
  multipledropdown: any = [1];
  dropdownOptions = ['one', 'two', 'three', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  config = {
    displayKey: "EmployeeName",
    height: 'auto',
    placeholder: 'Select',
    limitTo: this.dropdownOptions.length,
    noResultsFound: 'No results found!',
    searchPlaceholder: 'Search',
    searchOnKey: 'name',
  }
  paramsarray = [];
  department: any;
  saveModalRef: BsModalRef;
  public onClose: Subject<boolean>;
  flag: boolean;
  isSave: any = false;
  modalTitle: string = '';
  isCitizen = false;
  destination = false;

  constructor(public router: Router, public modalService: BsModalService, public bsModalRef: BsModalRef,
    public commonService: CommonService, public memoservice: MemoService, public route: ActivatedRoute,
    public circularService: CircularService, public incomingletterservice: IncomingLetterService, private citizenService: CitizenAffairService, public photoService: MediaRequestPhotoService) {
    this.memoservice.getMemo('memo', 0, 0).subscribe((data: any) => {
      this.department = data.M_ApproverDepartmentList;
      var calendar_id = environment.calendar_id;
      if(this.destination){
        this.department = data.OrganizationList.filter(res => calendar_id != res.OrganizationID);
      }
    });
    this.language = this.commonService.currentLang;
  }

  ngOnInit() {
    this.onClose = new Subject();
    setTimeout(() => {
      if (this.fromScreen == 'Citizen Affair') {
        this.isCitizen = true;
        this.departmentSel[0] = 2;
        this.onChangeDepartment(0);
      }
      if (this.language == 'ar') {
        switch (this.status) {
          case 'Memo Escalate':
          case 'Circular Escalate':
          case 'Letter Escalate':
          case 'Photo Escalate':
          case 'Design Escalate':
          case 'Citizen Affair Escalate':
            this.modalTitle = this.arabic('escalate');
            this.button = this.modalTitle;
            break;
          case 'Redirect':
          case 'Letter Redirect':
            this.modalTitle = this.arabic('redirect');
            this.button = this.modalTitle;
            break;
          case 'Share Memo':
            this.modalTitle = 'مشاركة المذكرة';
            this.button = this.modalTitle;
            break;
          case 'Circular Escalate':
            this.modalTitle = 'رفع للاعتماد';
            this.button = this.modalTitle;
            break;
        }
      } else {
        switch (this.status) {
          case 'Memo Escalate':
          case 'Circular Escalate':
          case 'Letter Escalate':
          case 'Photo Escalate':
          case 'Design Escalate':
          case 'Citizen Affair Escalate':
            this.modalTitle = 'Escalate';
            this.button = this.modalTitle;
            break;
          case 'Redirect':
          case 'Letter Redirect':
            this.modalTitle = 'Redirect';
            this.button = this.modalTitle;
            break;
          case 'Share Memo':
            this.modalTitle = 'Share Memo';
            this.button = this.modalTitle;
            break;
        }
      }
      this.modalTitle = (this.modalTitle == '') ? this.status : this.modalTitle;
      this.destination = (this.destination == true)?true:false;
    }, 200);
  }

  onChangeDepartment(index) {
    this.getUserList(index);
  }

  getUserList(index) {
    let that = this;
    let userId = this.currentUser.id;
    let params = [{
      "OrganizationID": this.departmentSel[index],
      "OrganizationUnits": "string"
    }];
    if (this.departmentSel[0]) {
      if (this.modalTitle == 'Redirect' || this.modalTitle == this.arabic('redirect')  || this.modalTitle == 'Share Memo' ||  this.modalTitle == 'مشاركة المذكرة' ) {
        this.commonService.getUserList(params, userId)
          .subscribe(data => {
            if (data) {
              that.list = data;
            }
          });
      } else {
        this.commonService.getmemoUserList(params, userId)
          .subscribe(data => {
            if (data) {
              that.list = data;
            }
          });
      }
    } else {
      that.list = [];
      that.userSel[0] = '';
    }
  }
  validate() {
    this.flag = true;
    var department = (this.departmentSel.length > 0 && this.departmentSel[0]) ? true : false,
      user = (this.userSel.length > 0 && this.userSel[0]) ? true : false;
    if (department && user) {
      this.flag = false;
    }
    if (this.isSave) {
      this.flag = true;
    }
    return this.flag;
  }

  saveStatus() {
    this.submitted = true;
    let that = this;
    this.params = [];
    if (this.status == 'Letter Escalate') {
      let value = {
        "value": 'Escalate',
        "path": 'Action',
        "op": 'Replace'
      };

      this.params.push(value);

      let value2 = {
        "value": this.Comments,
        "path": "Comments",
        "op": "Replace"
      };
      this.params.push(value2);


      let value3 = {
        "value": localStorage.getItem("UserID"),
        "path": 'UpdatedBy',
        "op": 'Replace'
      };
      this.params.push(value3);

      let value4 = {
        "value": new Date(),
        "path": 'UpdatedDateTime',
        "op": 'Replace'
      };
      this.params.push(value4);


      let value5 = {
        "value": this.modal.UserID,
        "path": 'ApproverId',
        "op": 'Replace'
      };
      this.params.push(value5);

    } else if (this.status == 'Letter Redirect') {
      let value = {
        "value": 'Redirect',
        "path": 'Action',
        "op": 'Replace'
      };

      this.params.push(value);

      let value2 = {
        "value": this.Comments,
        "path": "Comments",
        "op": "Replace"
      };
      this.params.push(value2);


      let value3 = {
        "value": localStorage.getItem("UserID"),
        "path": 'UpdatedBy',
        "op": 'Replace'
      };
      this.params.push(value3);

      let value4 = {
        "value": new Date(),
        "path": 'UpdatedDateTime',
        "op": 'Replace'
      };
      this.params.push(value4);


      let value5 = {
        "value": this.modal.UserID,
        "path": 'ApproverId',
        "op": 'Replace'
      };
      this.params.push(value5);
    }
    else if (this.status == 'Memo Escalate' || this.status == 'Citizen Affair Escalate' || this.status == 'Circular Escalate') {
      let value = {
        "value": 'Escalate',
        "path": 'Action',
        "op": 'replace'
      };

      this.params.push(value);

      let value2 = {
        "value": new Date(),
        "path": 'UpdatedDateTime',
        "op": 'replace'
      };
      this.params.push(value2);


      let value3 = {
        "value": localStorage.getItem("UserID"),
        "path": 'UpdatedBy',
        "op": 'replace'
      };
      this.params.push(value3);


      let value4 = {
        "value": this.modal.DepartmentID,
        "path": 'ApproverDepartmentId',
        "op": 'replace'
      };
      this.params.push(value4);

      let value5 = {
        "value": this.modal.UserID,
        "path": 'ApproverId',
        "op": 'replace'
      };
      this.params.push(value5);

      let value6 = {
        "value": this.Comments,
        "path": 'Comments',
        "op": 'replace'
      };

      this.params.push(value6);
    } else if (this.status == 'Redirect') {
      let value = {
        "value": 'Redirect',
        "path": 'Action',
        "op": 'replace'
      };

      this.params.push(value);

      let value2 = {
        "value": new Date(),
        "path": 'UpdatedDateTime',
        "op": 'replace'
      };
      this.params.push(value2);


      let value3 = {
        "value": localStorage.getItem("UserID"),
        "path": 'UpdatedBy',
        "op": 'replace'
      };
      this.params.push(value3);


      let value4 = {
        "value": this.modal.DepartmentID,
        "path": 'ApproverDepartmentId',
        "op": 'replace'
      };
      this.params.push(value4);

      let value5 = {
        "value": this.modal.UserID,
        "path": 'ApproverId',
        "op": 'replace'
      };
      this.params.push(value5);

      let value6 = {
        "value": this.Comments,
        "path": 'Comments',
        "op": 'replace'
      };
      this.params.push(value6);

    } else if (this.status == 'Share Memo') {
      this.params = this.userarray;
    } else if (this.status == 'Photo Escalate' || this.status == 'Design Escalate') {
      this.params = this.formatPatch('Escalate', 'Action');
    } 
    
    // destination department user change for letter
    if(this.destination == true && this.status != 'Share Memo'){
      this.params  = this.formatPatchDestination();
    }

    if (this.status == 'Share Memo') {
      this.commonService.shareparticipant('ShareParticipation', this.memoid, localStorage.getItem("UserID"), this.Comments, this.params)
        .subscribe(data => {

          if (data) {
            that.message = (this.language == 'ar') ? 'تمت مشاركة المذكرة بنجاح' : 'Memo Shared Successfully';
            that.saveModalRef = that.modalService.show(that.template);
            that.submitted = false;
            that.params = [];
            that.userarray = [];
            that.modal = {};
            that.bsModalRef.hide();

          }

        });
    } else {
      let method: any;
      if (this.fromScreen == 'Circular') {
        method = this.circularService.statusChange('Circular', this.memoid, this.params);
      } else if (this.fromScreen == 'Incoming Letter') {
        method = this.incomingletterservice.statusChange('InboundLetter', this.memoid, this.params);
      } else if (this.fromScreen == 'Outgoing Letter') {
        method = this.incomingletterservice.statusChange('OutboundLetter', this.memoid, this.params);
      } else if (this.fromScreen == 'Citizen Affair') {
        method = this.citizenService.statusChange('CitizenAffair', this.memoid, this.params);
      } else if (this.fromScreen == 'PhotoMedia') {
        method = this.photoService.statusChange('Photo', this.memoid, this.params);
      } else if (this.fromScreen == 'DesignMedia') {
        method = this.photoService.statusChange('Design', this.memoid, this.params);
      }
      else {
        this.fromScreen = (this.language == 'ar') ? 'مذكرة' : 'Memo';
        method = this.commonService.savememo('Memo', this.memoid, this.params);
      }
      method.subscribe(data => {
        if (data) {
          let status = (this.language == 'ar') ? 'أحالت' : 'Redirected';
          if (that.status == 'Memo Escalate') {
            status = (this.language == 'ar') ? 'تصاعدت' : 'Escalated';
          }
          if (that.status == 'Circular Escalate') {
            status = (this.language == 'ar') ? 'تصاعدت' : 'Escalated';
          }
          if (that.status == 'Letter Escalate') {
            status = (this.language == 'ar') ? 'تصاعدت' : 'Escalated';
          }
          if (that.status == 'Photo Escalate' || that.status == 'Design Escalate' || that.status == 'Citizen Affair Escalate') {
            status = (this.language == 'ar') ? 'تصاعدت' : 'Escalated';
          }
          if (this.fromScreen == 'PhotoMedia') {
            this.fromScreen = 'Photo Request';
          }
          if (this.fromScreen == 'DesignMedia') {
            this.fromScreen = (this.language == 'ar') ? this.arabic('mediarequestfordesign') : 'Media Request Design';
          }
          if ((this.fromScreen == 'Outgoing Letter' || this.fromScreen == 'Incoming Letter')) {
            if((that.status == 'Letter Escalate') || (that.status == 'Letter Redirect' && this.language != 'ar')){
              this.fromScreen = (this.language == 'ar') ? this.arabic('letters') : 'Letter';
            }
          }
          if (this.fromScreen == 'Citizen Affair') {
            this.fromScreen = (this.language == 'ar') ? this.arabic('citizenaffair') : 'Citizen Affair';
          }
          if(this.fromScreen == 'Circular'){
            this.fromScreen = (this.language == 'ar')? this.arabic('circular'):'Circular';
          }
          var success = (this.language == 'ar') ? 'بنجاح' : ' Successfully';
          if (that.status == 'Memo Escalate' && this.language == 'ar') {
            that.message = 'تم رفع المذكرة للموافقة بنجاح';
          } else if (this.fromScreen == 'مذكرة' && that.status != 'Memo Escalate' && this.language == 'ar') {
            that.message = this.arabic('memoredirectsuccessfully');
          } else if (this.fromScreen == 'Outgoing Letter' && that.status == 'Letter Redirect' && this.language == 'ar') {
            that.message = this.arabic('outgoingletterredirectsuccessfully');
            this.fromScreen = (this.language == 'ar') ? this.arabic('letters') : 'Letter';
          } else if (this.fromScreen == 'Incoming Letter' && that.status == 'Letter Redirect' && this.language == 'ar') {
            that.message = this.arabic('incomingletterredirectsuccessfully');
            this.fromScreen = (this.language == 'ar') ? this.arabic('letters') : 'Letter';
          } else {
            that.message = this.fromScreen + ' ' + status + success;
          }
          if (that.status == 'Design Escalate' ) {
            if (this.commonService.currentLang == 'en') {
              that.message = 'Media Request for Design Escalated Successfully';
            } else {
              that.message = this.arabic('designescalatemsg');
            }
          }
          if (this.fromScreen == 'Photo Request'){
            if (this.commonService.currentLang == 'en') {
              that.message = 'Photo Request Escalated Successfully';
            } else {
              that.message = this.arabic('photoescalatesuccessmsg');
            }
          }

          that.saveModalRef = that.modalService.show(that.template);
          that.submitted = false;
          that.params = [];
          that.userarray = [];
          that.modal = {};

          that.bsModalRef.hide();
        }

      });
      that.submitted = false;
    }
    this.onClose.next(false);
  }
  formatPatch(val, path) {
    var data = [{
      "value": val,
      "path": path,
      "op": "Replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "Replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "Replace"
    }, {
      "value": this.Comments,
      "path": "Comments",
      "op": "Replace"
    }, {
      "value": this.modal.UserID,
      "path": "ApproverId",
      "op": "Replace"
    }];
    return data;
  }

  close() {
    this.params = [];

    this.userarray = [];
    this.modal = {};
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

  closemodal() {
    this.saveModalRef.hide();

    var inletter = (this.commonService.language == 'English') ? 'Letter' : this.arabic('letters'),
      outletter = (this.commonService.language == 'English') ? 'Letter' : this.arabic('letters'),
      citizen = (this.commonService.language == 'English') ? 'Citizen Affair' : this.arabic('citizenaffair');

    if (this.fromScreen == 'Memo' || this.fromScreen == 'مذكرة') {
      this.commonService.pageReLoad('memo-list');
      return;
    }
    else if (this.fromScreen == inletter || this.fromScreen == outletter) {
      this.commonService.pageReLoad('letter-list');
      return;
    } else if (this.fromScreen == 'Circular' || this.fromScreen == this.arabic('circular')) {
      this.commonService.pageReLoad('circular-list');
    } else if (this.fromScreen == 'Photo Request' || this.fromScreen == 'Media Request Design') {
      this.router.navigate(['/'+this.commonService.currentLang+'/app/media/protocol-home-page']);
    } else if (this.status == 'Design Escalate') {
      this.router.navigate(['/'+this.commonService.currentLang+'/app/media/protocol-home-page']);
    } else if (this.fromScreen == citizen) {
      if (this.language == 'ar') {
        this.router.navigate(['ar/app/citizen-affair/citizen-affair-list']);
      } else {
        this.router.navigate(['en/app/citizen-affair/citizen-affair-list']);
      }
    }
  }

  addNew() {
    let index = this.multipledropdown[this.multipledropdown.length - 1];
    this.multipledropdown.push(index + 1);
  }

  selectionChanged(index) {
    let user = this.list.filter((value) => value.UserID == this.userSel[index]);
    this.modal.UserID = parseInt(user[0].UserID);
    this.modal.DepartmentID = parseInt(user[0].OrgUnitID);
    let value = {
      "UserID": user[0].UserID,
      "Type": 'Memo'
    }
    this.userarray.push(value);
  }

  arabic(data) {
    return this.commonService.arabic.words[data];
  }

  formatPatchDestination() {
    var data = [{
      "value": 'Redirect',
      "path": 'Action',
      "op": "Replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "Replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "Replace"
    }, {
      "value": this.Comments,
      "path": "Comments",
      "op": "Replace"
    }, {
      "value": this.modal.UserID,
      "path": "DestinationRedirectedBy",
      "op": "Replace"
    }];
    return data;
  }



}
