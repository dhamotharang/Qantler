import { Component, OnInit, Input } from '@angular/core';
import { TaskEvent } from '../../../service/task.event';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-receive-msg',
  templateUrl: './receive-msg.component.html',
  styleUrls: ['./receive-msg.component.scss']
})
export class ReceiveMsgComponent implements OnInit {
  @Input('message') message;
  history: boolean = false;
  lang: any;
  AttachmentDownloadUrl = environment.AttachmentDownloadUrl;
  constructor(
    public common: CommonService
  ) {
    this.lang = this.common.language;
  }
  ngOnInit() {
    if (this.message.Action) {
      this.history = true;
    } else {
      this.history = false;
    }
  }

}