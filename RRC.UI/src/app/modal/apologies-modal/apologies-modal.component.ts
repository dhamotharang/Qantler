import { CommonService } from './../../common.service';
import { Component, OnInit, Input } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CalendarManagementService } from 'src/app/media/calendar-management/service/calendar-management.service';
import { Router } from '@angular/router';
import { SuccessComponent } from '../success-popup/success.component';

@Component({
  selector: 'app-apologies-modal',
  templateUrl: './apologies-modal.component.html',
  styleUrls: ['./apologies-modal.component.scss']
})
export class ApologiesModalComponent implements OnInit {
  @Input() identity: string; // we can set the default value also
  @Input() ApiString: string;
  @Input() ApiTitleString: string;
  @Input() ApproverID: string;
  @Input() id: number;
  @Input() redirectUrl: string;
  @Input() data:any;
  @Input() action:any;
  @Input() Comments: string = '';
  @Input() isBulkEvent:boolean;
  lang: string;
  inProgress: boolean;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  popupMsg: any;
  constructor(
    public bsModalRef: BsModalRef,
    public common: CommonService,
    public calendarManagementService: CalendarManagementService,
    public modalService: BsModalService,
    public router: Router
  ) { this.lang = this.common.currentLang; }

  ngOnInit() {
    this.lang = this.common.currentLang;
  }

  Apology(string: any) {
    this.bsModalRef.hide();
    this.inProgress = true;
    var toSendRequestData:any = {};
    if(this.isBulkEvent){
      toSendRequestData = {
        Action: 'Apology',
        ApproverID: this.ApproverID,
        ActionBy: this.currentUser.id,
        ActionDateTime: new Date(),
      };
      if (string == 'Yes') {
        toSendRequestData.CalendarID = [];
          this.data.forEach((selObj) => {
            toSendRequestData.CalendarID.push({CalendarID:selObj.CalendarID});
            toSendRequestData.IsApologySent = true;
          });
      } else {
          toSendRequestData.CalendarID = [];
          this.data.forEach((selObj) => {
            toSendRequestData.CalendarID.push({CalendarID:selObj.CalendarID});
            toSendRequestData.IsApologySent = false;
          });
      }
      if (this.lang == 'en') {
        this.popupMsg = 'Event Request Rejected Successfully';
      } else {
        this.popupMsg = this.arabicfn('calendarreqrejectmsg');
      }
      this.calendarManagementService.bulkApology(toSendRequestData).subscribe(
        (response: any) => {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.popupMsg;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/app/media/calendar-management/list']);
          });
        }
      );
    }else{
      if (string == 'Yes') {
        toSendRequestData = [{
          "value": 'Apology',
          "path": "Action",
          "op": "Replace"
        },{
          "value": true,
          "path": "IsApologySent",
          "op": "Replace"
        }, {
          "value": this.Comments,
          "path": "Comments",
          "op": "Replace",
        }, {
          "value": this.currentUser.id,
          "path": "UpdatedBy",
          "op": "Replace"
        }, {
          "value": new Date(),
          "path": "UpdatedDateTime",
          "op": "Replace"
        }];
      }else{
        toSendRequestData = [{
          "value": 'Apology',
          "path": "Action",
          "op": "Replace"
        },{
          "value": false,
          "path": "IsApologySent",
          "op": "Replace"
        }, {
          "value": this.Comments,
          "path": "Comments",
          "op": "Replace",
        }, {
          "value": this.currentUser.id,
          "path": "UpdatedBy",
          "op": "Replace"
        }, {
          "value": new Date(),
          "path": "UpdatedDateTime",
          "op": "Replace"
        }];
      }
      if (this.lang == 'en') {
        this.popupMsg = 'Event Request Rejected Successfully';
      } else {
        this.popupMsg = this.arabicfn('calendarreqrejectmsg');
      }
      this.calendarManagementService.update(this.id, toSendRequestData)
      .subscribe((response: any) => {
        if (response.CalendarID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.popupMsg;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/app/media/calendar-management/list']);
          });
        }
      });
    }


  }

  closemodal() {
    this.bsModalRef.hide();
    this.common.setAplogiesModalClose('close');
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }
}
