import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { TaskEvent } from '../../service/task.event';
import { Router, ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { TaskChatModal } from './modal';
import { TaskService } from '../../service/task.service';
import { CommonService } from 'src/app/common.service';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-task-chat',
  templateUrl: './task-chat.component.html',
  styleUrls: ['./task-chat.component.scss']
})
export class TaskChatComponent implements OnInit {
  @ViewChild('taskChatHeight') taskChatHeight: ElementRef;
  @ViewChild('messageBox') messageBox: ElementRef;
  @ViewChild('attachFile') attchFile: ElementRef;
  @ViewChild('emoji') emoji: ElementRef;
  visibleChat: any = false;
  createDate: any = "Create Date";
  taskTitle: any = "My Task";
  @Input() set screen(status) {
    this.visibleChat = status;
  };

  chatDet: any;
  tagUserList: any;
  backClick = false;
  message = '';
  msg = {
    profile: 'assets/home/user_male.png',
    time: '',
    message: '',
    user: '',
    file: ''
  }
  messageList = [];
  chat: TaskChatModal = new TaskChatModal;
  isTags: boolean = false;
  uploadProcess: boolean;
  uploadPercentage: number;
  chatModal: any = {
    Attachments: []
  };
  url: any = [];
  currentUser: any;
  tagedUserList: any = [];
  messageSend: boolean = false;
  isEmoji = false;
  selectedEmoji: any;
  attachmentShow: boolean = false;
  attachementName: any = '';
  emojiStyle: any;
  screenHeight: any;
  language = 'English';
  title: string = 'COMMUNICATION BOARD/ TASK HISTORY';
  placeholder: any = "";
  constructor(private event: TaskEvent, public common: CommonService,
    private router: Router, public dp: DatePipe, public route: ActivatedRoute,
    public taskservice: TaskService) {
    this.language = this.common.language;
    if (screen.width < 770) {
      this.screenHeight = screen.height;
    }
    // if(this.router.getCurrentNavigation().extras.state){
    //   console.log(this.router.getCurrentNavigation().extras.state);
    // }
      
  }

  ngOnInit() {
    console.log(history.state);

    if(history.state.taskDetails){
      this.chatDet = history.state.taskDetails;
      this.tagUserList = history.state.tagUserList;
      this.messageList = history.state.taskDetails.communiationHistory;
    }

    if (!this.visibleChat) {
      this.visibleChat = this.taskservice.visibleChat;
    }
    if (this.common.language != 'English') {
      this.title = this.arabic('communicationboardtitle');
      this.createDate = this.arabic('createdate');
      this.taskTitle = this.arabic('mytask');

    }
    this.placeholder = (this.common.language == 'English') ? 'Type your message...' : this.arabic('typeyourmessage');
    this.currentUser = JSON.parse(localStorage.getItem("User"));
    this.event.chatData$.subscribe((res: any) => {
      this.chatDet = res.chatDet;
      this.tagUserList = res.tagUserList;
      this.messageList = (this.chatDet.communiationHistory) ? this.chatDet.communiationHistory : [];
      setTimeout(() => {
        this.scrollToBottom();
      }, 200);
    });
    this.event.chatLoad$.subscribe(res => {
      this.currentUser = JSON.parse(localStorage.getItem("User"));
      if (res) {
        this.messageList = res;
        this.messageList.forEach(res => {
          res['status'] = true;
        });
        setTimeout(() => {
          this.scrollToBottom();
        }, 200);
      }
    });

    this.event.chatUpdate$.subscribe(res => {
      if (this.messageList)
        if (this.messageList.length < res.length) {
          var length = (res.length - this.messageList.length);
          for (let i = 0; i < length; i++)
            this.messageList.push(res[this.messageList.length]);
        }
    });
  }

  backToForm() {
    var path = '';
    if (this.common.language == 'English')
      path = 'en/app/task/task-view/';
    else
      path = 'ar/app/task/task-view/';

    // this.router.navigateByUrl('en/app/task/task-dashboard');
    this.router.navigate([path + this.chatDet.TaskID]);
  }

  attachFileClick() {
    return;
  }

  enterSendMsg(e) {
    if (e.keyCode == 13) {
      this.sendMsg();
    }
  }

  tagUser() {
    let str = this.message;
    let data = str.split(" ");
    this.isTags = (data[data.length - 1].charAt(0) == '@') ? true : false;
  }

  tagUserSelect(user) {
    var tagList = [];
    this.message += user.name;
    var data = this.message.split("@");
    this.tagedUserList.push(user);
    this.message = '';
    data.map(msg => {
      this.message += msg;
    });
    var msg = this.message.split(" ");
    msg.map((res) => {
      this.tagedUserList.map(ures => {
        if (res == ures.name)
          tagList.push(ures);
      });
    });
    this.tagedUserList = tagList;
    this.isTags = false;
    this.messageBox.nativeElement.focus();
  }

  scrollBottom() {
    this.taskChatHeight.nativeElement.scrollTop = this.taskChatHeight.nativeElement.scrollHeight + 150;
  }

  prepareMsg() {

  }

  Attachments(event) {
    var files = event.target.files;
    let that = this;
    this.uploadProcess = true;
    var reader = new FileReader();
    for (var i = 0; i < event.target.files.length; i++) {
      reader.onload = (e: ProgressEvent) => {
        var path = (<FileReader>e.target).result;
        this.url.push(path);
      }
      reader.readAsDataURL(event.target.files[i]);
    }
    this.common.postAttachment(files).subscribe((event: any) => {
      if (event.type === HttpEventType.UploadProgress) {
        this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
      } else if (event.type === HttpEventType.Response) {
        this.uploadProcess = false;
        this.uploadPercentage = 0;
        this.attachmentShow = true;

        for (var i = 0; i < event.body.FileName.length; i++) {
          this.attachementName = event.body.FileName[i];
          that.chatModal.Attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'TaskID': '' });
        }
        that.chatModal.Attachments = that.chatModal.Attachments;
      }
    });
  }

  cancelAttachment() {
    this.attachementName = '';
    this.attchFile.nativeElement.value = "";
    this.chatModal.Attachments = [];
    this.attachmentShow = false;
  }

  sendMsg() {
    this.chat['TaggedUserID'] = [];
    if (this.message.trim() == '' && this.chatModal.Attachments.length == 0) {
      return;
    }
    if (this.isEmoji) {
      this.isEmoji = false;
    }
    this.scrollToBottom();
    this.tagedUserList.map(res => {
      this.chat.TaggedUserID.push({ 'TaggedUsersID': res.id, 'TaggedUsersName': res.name });
    })
    this.chat.TaskID = this.chatDet.TaskID;
    this.chat.Message = this.message;
    this.chat.CreatedDateTime = new Date;
    this.chat.CreatedBy = this.currentUser.id;
    this.chat.AttachmentGuid = (this.chatModal.Attachments.length > 0) ? this.chatModal.Attachments[0].AttachmentGuid : '';
    this.chat.AttachmentName = (this.chatModal.Attachments.length > 0) ? this.chatModal.Attachments[0].AttachmentsName : '';
    this.chat['Photo'] = 'https://picsum.photos/id/1074/200/200';
    this.chat['status'] = false;
    var chat = { ...this.chat };
    this.messageList.push(chat);
    this.scrollBottom();
    this.taskservice.sendChat('/DutyTask/CommunicationHistory', this.chat).subscribe(res => {
      if (res) {
        this.event.changeMsgStatus(true);
        this.attachementName = '';
        this.attchFile.nativeElement.value = "";
        this.chatModal.Attachments = [];
        this.attachmentShow = false;
      }
    });
    this.message = '';
  }

  addEmoji(event) {
    this.message += event.emoji.native;
    this.selectedEmoji = event.emoji;
  }

  emojiClick(emoji, event) {
    const styles = emoji.emojiSpriteStyles(event.emoji.sheet, 'apple');
    Object.assign(this.messageBox.nativeElement.styles, styles);
  }

  scrollToBottom() {
    const chatbox = document.querySelector('.communication-board');
    setTimeout(() => {
      if (chatbox) {
        chatbox.scrollTop = chatbox.scrollHeight;
      }
    }, 100);
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

}
