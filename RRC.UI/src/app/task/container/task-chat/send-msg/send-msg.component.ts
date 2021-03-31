import { Component, OnInit, Input } from '@angular/core';
import { TaskEvent } from '../../../service/task.event';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-send-msg',
  templateUrl: './send-msg.component.html',
  styleUrls: ['./send-msg.component.scss']
})
export class SendMsgComponent implements OnInit {
  chat: any;
  history: boolean = false;
  lang: any;
  @Input() set message(data) {
    this.chat = data;
  };
  AttachmentDownloadUrl = environment.AttachmentDownloadUrl;

  constructor(public event: TaskEvent,
    public common: CommonService) {
    this.event.msgStatus$.subscribe(status => {
      this.chat.status = status;
    });
    this.lang = this.common.language;
    //let messageArr = this.message.Message.split('$');
  }
  ngOnInit() {
    if (this.chat.Action) {
      this.history = true;
    } else {
      this.history = false;
    }
  }
}