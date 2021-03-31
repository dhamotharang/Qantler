import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit,OnDestroy {
  public type: any = '';
  public title: any = '';
  firstUrl = '';
  secondUrl = '';
  thirdUrl = '';
  currentLocation = '';
  secondLink = '';
  public texts: string[] = [];
  actionSubscribe: any;

  secondLinkType: any;

  constructor(public commonservice: CommonService, private router: Router) {
    this.actionSubscribe = this.commonservice.actionChanged$.subscribe(data => {
      if (!data.type) {
        this.secondLinkType = data.second;
        this.firstUrl = data.first;
        this.secondUrl = data.second;
        this.thirdUrl = data.third;
        let fullPath = this.router.url;
        let splitVal = fullPath.split("app")[1];
        if(!splitVal) {
          return false
        }
        let actualUrl = splitVal.split("/");
        let urlLen = actualUrl;
        let lastIndex = actualUrl.length - 1;
        let lastVal = actualUrl[lastIndex];
        let User = JSON.parse(localStorage.getItem('User'));
        if(parseInt(lastVal)) {
          let endPoint = actualUrl.length - 2;
          if (actualUrl.length >= 4) {
            if(!(actualUrl[2] == 'cv-bank' || actualUrl[2] == 'employee' || actualUrl[2] == 'raise-complaint-suggestion-view')){
              if (actualUrl.length == 4) {
                endPoint = actualUrl.length - 2;
              } else {
                endPoint = actualUrl.length - 3;
              }
            }
          }
          if (actualUrl[2] == 'official-tasks') {
            endPoint = actualUrl.length - 4;
          }
          actualUrl = actualUrl.slice(0, endPoint);
        } else {
          if (actualUrl.length >= 4) {
            let endPoint = actualUrl.length - 2;
            if(actualUrl[2] == 'cv-bank' || actualUrl[2] == 'employee'){
              endPoint = actualUrl.length - 1;
            }
            if(actualUrl[3] == 'log-fine-view'){
              endPoint = actualUrl.length - 4;
            }
            actualUrl = actualUrl.slice(0, endPoint);
          } else {
            actualUrl = actualUrl.slice(0,  actualUrl.length - 1);
          }
        }
        let url = '';
        for (var value of actualUrl) {
          if (value)
            url += '/' + value;
        }
        if (urlLen[2] == 'employee') {
          this.secondLink = '/hr/employee';
          url = '/hr'
        }
        if (urlLen[2] == 'cv-bank') {
          this.secondLink = '/hr/cv-bank';
          url = '/hr';
        }
        if (url == '/media') {
          if (User.UnitID == 4) {
            if(urlLen.length >= 4){
              url = '';
              if(urlLen[2] == 'calendar-management' || urlLen[2] == 'gifts-management'){
                if(parseInt(lastVal)) {
                  actualUrl = urlLen.slice(0,  urlLen.length - 2);
                } else {
                  actualUrl = urlLen.slice(0,  urlLen.length - 1);
                }
              } else {
                if(parseInt(lastVal)) {
                  if(urlLen.length == 4) {
                    actualUrl = urlLen.slice(0,  urlLen.length - 2);
                  } else {
                    actualUrl = urlLen.slice(0,  urlLen.length - 3);
                  }
                } else {
                  actualUrl = urlLen.slice(0,  urlLen.length - 2);
                }
              }
              for (var value of actualUrl) {
                if (value)
                  url += '/' + value;
              }
            }else{
              if(urlLen[2] == 'media-protocol-request'){
                url = '/media/protocol-dashboard';
              }
            }
          }
        }
        this.currentLocation = url;
      }
      else if (data.type == 'list') {
        this.type = data.title;
        switch (data.category) {
          // case 'memo':
          //   this.firstUrl = 'InternalMemos';
          //   this.secondUrl = this.type;
          //   this.thirdUrl = 'List';
          //   break;
          case 'letter':
            this.firstUrl = 'Letters';
            this.secondUrl = this.type;
            this.thirdUrl = 'List';
            break;
          // case 'task':
          //   this.firstUrl = 'Duty Tasks';
          //   this.secondUrl = 'Tasks List';
          //   this.thirdUrl = '';
          //   break;
          // case 'circular':
          //   this.firstUrl = 'InternalCirculars';
          //   this.secondUrl = this.type;
          //   this.thirdUrl = 'List';
          //   break;
        }
      } else if (data.type == 'create') {
        switch (data.pageInfo.type) {
          // case 'memo':
          //   this.firstUrl = 'InternalMemos';
          //   this.secondUrl = 'Memo Creation';
          //   this.thirdUrl = '';
          //   break;
          case 'letter':
            this.firstUrl = 'InternalLetters';
            this.secondUrl = 'Letter Creation';
            this.thirdUrl = '';
            break;
          // case 'task':
          //   this.firstUrl = 'Duty Tasks';
          //   this.secondUrl = 'Creation';
          //   this.thirdUrl = '';
          //   break;
          // case 'circular':
          //   this.firstUrl = 'InternalCirculars';
          //   this.secondUrl = 'Creation';
          //   this.thirdUrl = '';
          //   break;
        }
      } else if (data.type == 'view') {
        switch (data.pageInfo.type) {
          // case 'memo':
          //   this.firstUrl = 'InternalMemos';
          //   this.secondUrl = this.type;
          //   this.thirdUrl = 'Memo ' + data.pageInfo.id;
          //   break;
          case 'letter':
            this.firstUrl = 'InternalLetters';
            this.secondUrl = this.type;
            this.thirdUrl = 'Letter ' + data.pageInfo.id;
            break;
          // case 'task':
          //   this.firstUrl = 'Duty Tasks';
          //   this.secondUrl = 'Creation';
          //   this.thirdUrl = '';
          //   break;
          // case 'circular':
          //   this.firstUrl = 'InternalCirculars';
          //   this.secondUrl = this.type;
          //   this.thirdUrl = 'Circular ' + data.pageInfo.id;
          //   break;
        }
      }
    });
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.actionSubscribe.unsubscribe();
  }

  clickFirstLink(){
    if(this.firstUrl == "Incoming Letters" || this.firstUrl == "Outgoing Letters"){
      var param={
        menu: "en/app/letter/letter-list",
        title: this.firstUrl,
        type: "letter"
      }
      this.router.navigate([param.menu]);
      setTimeout(() => {
        this.commonservice.sideNavClick(param);
      }, 500);
    }else if(this.firstUrl == "Protocol Calendar"){
      this.router.navigate(['/en/app/media/calendar-management/list']);
    }else{
      this.router.navigate(['/en/app'+this.currentLocation]);
    }
  }

  clickSecondLink(){
    if(this.firstUrl == "Internal Memos" || this.firstUrl == "Internal Circulars"){
      if(this.firstUrl == "Internal Memos"){
        var param={
          menu: "en/app/memo/memo-list",
          title: this.secondUrl,
          type: "memo"
        }
      }else if(this.firstUrl == "Internal Circulars"){
        var param={
          menu: "en/app/circular/circular-list",
          title: this.secondUrl,
          type: "circular"
        }
      }
      this.router.navigate([param.menu]);
      setTimeout(() => {
        this.commonservice.sideNavClick(param);
      }, 500);
    }
    else{
      let link = (this.secondLink == '') ? this.currentLocation: this.secondLink;
      this.router.navigate(['/en/app'+link]);
    }
  }

}
