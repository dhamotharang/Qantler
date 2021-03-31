import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommentSectionService } from '../../service/comment-section.service';
import { CommonService } from 'src/app/common.service';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-comment-section',
  templateUrl: './comment-section.component.html',
  styleUrls: ['./comment-section.component.scss']
})
export class CommentSectionComponent implements OnInit,OnChanges {

  @Input() comments:any;
  @Input() commentType:any;
  @Input() moduleId:any;
  @Input() moduleNameID:any;
  @Input() readOnly:boolean = false;
  currentUser: any;
  commentSectionArr:any = [];
  sevenDayDuration = 604800000;
  oneDayDuration = 86400000;
  twoDayDuration = 172800000;
  lang: string = 'ar';
  constructor(
    private commentSectionService:CommentSectionService,
    private commonService:CommonService,
    private endpoint: EndPointService) {
      this.lang = this.commonService.currentLang;
    }
  InProgress: boolean;
  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
  }

  ngOnChanges(changes:SimpleChanges){
    if(changes['comments']){
      if(this.comments){
        this.comments.forEach(element => {
          if(element.Photo == null){
            element.Photo = 'assets/home/user_male.png';
          }else{
            let imageDetails = element.Photo.split('|');
            element.Photo = this.endpoint.fileDownloadUrl + '?filename=' + imageDetails[1] + '&guid=' + imageDetails[0];
          }
        });
      }
      this.commentSectionArr = this.comments;
    }
  }

  hisLog(status:string) {
    let sts = status.toLowerCase();
    if(this.lang != 'ar'){
      let statusStr = status.charAt(0).toUpperCase() + status.slice(1);
      if (sts == 'reject' || sts == 'redirect') {
        return statusStr + 'ed by';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        return 'Assigned by';
      } else if(sts == 'submit' || sts == 'resubmit'){
        return statusStr +'ted by';
      } else if(sts == 'returnforinfo'){
        return 'Returned for Info by';
      } else if(sts == 'cancel'){
        return 'Cancelled by';
      } else if (sts == 'markascomplete') {
        return 'Raised Compensation by';
      } else if (sts == 'markascompleted') {
        return 'Mark As Completed by';
      } else if (sts == 'reopen') {
        return 'Reopened by';
      } else if (sts == 'trainingattendance') {
        return 'Proof of attendance submitted by';
      }else {
        return statusStr + 'd by';
      }
    } else if(this.lang == 'ar') {
      let arabicStatusStr = '';
      if (sts == 'reject' || sts == 'redirect') {
        arabicStatusStr = sts+'edby';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        arabicStatusStr = 'assignedby';
      } else if(sts == 'submit' || sts == 'resubmit'){
        arabicStatusStr = sts+'tedby';
      } else if(sts == 'returnforinfo'){
        arabicStatusStr = 'returnedforinfoby';
      } else if(sts == 'cancel'){
        arabicStatusStr = 'cancelledby';
      } else if (sts == 'markascomplete') {
        arabicStatusStr = 'closeandraisecompensationby';
      } else if (sts == 'markascompleted') {
        arabicStatusStr = 'markascompletedby';
      } else if (sts == 'reopen') {
        arabicStatusStr = 'reopenedby';
      }else if (sts == 'trainingattendance') {
        arabicStatusStr= 'ProofOfAttendanceSubmittedBy';
      } else {
        arabicStatusStr = sts+'dby';
      }
      return this.commonService.arabic.words[arabicStatusStr];
    }
  }

  sendReply (commentData:any) {
    if (commentData.replyContent && (commentData.replyContent.trim() != '')) {
      this.InProgress = true;
      let chatData:any = {
        Message:commentData.replyContent,
        ParentCommunicationID:commentData.CommunicationID,
        CreatedBy:this.currentUser.id,
        CreatedDateTime:new Date()
      };
      chatData[this.moduleNameID] = commentData[this.moduleNameID];
      console.log('chatData', chatData, 'commentType', this.commentType);
      this.commentSectionService.sendComment(this.commentType,chatData).subscribe((chatRes:any) => {
        this.commentSectionService.newCommentCreated(true);
        this.InProgress = false;
      });
    }
  }

  clearReplySection (commentData:any) {
    commentData.showReplyContent = false;
    commentData.replyContent = '';
  }

  parseDate(datVal?) {
    if(datVal) {
      return new Date(datVal);
    } else {
      return new Date();
    }
  }
}
