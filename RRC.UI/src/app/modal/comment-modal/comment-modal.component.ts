import { CommonService } from './../../common.service';
import { Component, OnInit, Input } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CalendarManagementService } from 'src/app/media/calendar-management/service/calendar-management.service';
import { SuccessComponent } from '../success-popup/success.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-comment-modal',
  templateUrl: './comment-modal.component.html',
  styleUrls: ['./comment-modal.component.scss']
})
export class CommentModalComponent implements OnInit {
  comment: string;
  @Input() identity: string; // we can set the default value also
  @Input() ApiString: string;
  @Input() ApiTitleString: string;
  @Input() ApproverID: string;
  @Input() id: number;
  @Input() redirectUrl: string;
  @Input() data:any;
  @Input() action:any;
  @Input() Comments: string = '';
  lang: string;
  approverList: Array<any> = [];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  constructor(
    public bsModalRef: BsModalRef,
    public common: CommonService,
    public calendarManagementService: CalendarManagementService,
    public modalService: BsModalService,
    public router: Router
  ) { }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.loadUserList();
  }

  loadUserList() {
    const params = [{
      'OrganizationID': 10,
      'OrganizationUnits': ''
    }];
    this.common.getmemoUserList(params, this.currentUser.id).subscribe(
      (response: any) => {
        this.approverList = response;
      }
    );
  }

  submitData() {
    this.bsModalRef.hide();
    if (this.action == 'return') {
      var toSendRequestData:any = {
        Action: 'ReturnForInfo',
        ApproverID: this.ApproverID,
        ActionBy: this.currentUser.id,
        ActionDateTime: new Date(),

      }
      toSendRequestData.CalendarID = [];
      this.data.forEach((selObj) => {
        toSendRequestData.CalendarID.push({CalendarID:selObj.CalendarID});
        toSendRequestData.Comments = this.comment;
      });

      this.calendarManagementService.bulkEventUpdate(toSendRequestData).subscribe(
        (response: any) => {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = 'Event Requests Returned For Info Successfully';
          if(this.common.currentLang !='en'){
            this.bsModalRef.content.message = this.common.arabic.words['calendarreqreturnmsg'];
          }
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/app/media/calendar-management/list']);
          });
        }
      );
    } else {
      var toSendRequestData:any = {
        Action: 'Escalate',
        ApproverID: this.ApproverID,
        ActionBy: this.currentUser.id,
        ActionDateTime: new Date(),
        Comments: this.comment
      }
      toSendRequestData.CalendarID = [];
      this.data.forEach((selObj) => {
        toSendRequestData.CalendarID.push({CalendarID:selObj.CalendarID});
        toSendRequestData.Comments = this.comment;
      });

      this.calendarManagementService.bulkEventUpdate(toSendRequestData).subscribe(
        (response: any) => {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = 'Event Requests Escalated Successfully';
          if(this.common.currentLang !='en'){
            this.bsModalRef.content.message = this.common.arabic.words['calendarreqescalatedmsg'];
          }
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/app/media/calendar-management/list']);
          });
        }
      );
    }
  }

  closeCommentModal(){
    this.bsModalRef.hide();
    this.common.setCommentModalClose('close');
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }
}


