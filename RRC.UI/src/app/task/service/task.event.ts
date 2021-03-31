import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from '../../api/endpoint.service';
import { Subject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class TaskEvent {
    private taskClicked = new Subject<any>();
    public taskClicked$ = this.taskClicked.asObservable();

    private FormOpen = new Subject<any>();
    public FormOpen$ = this.FormOpen.asObservable();

    private msgStatus = new Subject<any>();
    public msgStatus$ = this.msgStatus.asObservable();

    private taskStatus = new Subject<any>();
    public taskStatus$ = this.taskStatus.asObservable();

    private changeSelectType = new Subject<any>();
    public changeSelectType$ = this.changeSelectType.asObservable();

    private chatLoad = new Subject<any>();
    public chatLoad$ = this.chatLoad.asObservable();

    private chatData = new Subject<any>();
    public chatData$ = this.chatData.asObservable();

    private chatUpdate = new Subject<any>();
    public chatUpdate$ = this.chatUpdate.asObservable();


    taskClick(){
        this.taskClicked.next('task');
    }

    backToForm(){
      this.FormOpen.next();
    }

    changeMsgStatus(status){
      this.msgStatus.next(status);
    }

    changeTaskStatus(data){
      this.taskStatus.next(data);
    }

    selectType(data){
      this.changeSelectType.next(data);
    }

    chatReload(data){
      this.chatLoad.next(data);
    }

    chatdata(data){
      this.chatData.next(data);
    }

    chatUpdateCall(data){
      this.chatUpdate.next(data);
    }

}