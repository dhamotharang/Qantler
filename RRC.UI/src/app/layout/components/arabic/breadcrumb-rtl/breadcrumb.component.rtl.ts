import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-breadcrumb-rtl',
  templateUrl: './breadcrumb.component.rtl.html',
  styleUrls: ['./breadcrumb.component.rtl.scss']
})
export class BreadcrumbComponentRTL implements OnInit, OnDestroy {

  public type: any = '';
  public title: any = '';
  firstUrl = '';
  secondUrl = '';
  thirdUrl = '';
  currentLocation = '';
  secondLink = '';
  public texts: string[] = [];
  actionSubscribe: any;

  breadcrumbttranslation = [];
  constructor(public commonservice: CommonService, private router: Router) {
    this.breadcrumbttranslation = [
      { enListType: "Incoming Memos", arListType: this.commonservice.sideNaveTypeArabic('Incoming Memos') },
      { enListType: "Outgoing Memos", arListType: this.commonservice.sideNaveTypeArabic('Outgoing Memos') },
      { enListType: "Draft Memos", arListType: this.commonservice.sideNaveTypeArabic('Draft Memos') },
      { enListType: "Historical Memos Incoming", arListType: this.commonservice.sideNaveTypeArabic('Historical Memos Incoming') },
      { enListType: "Historical Memos Outgoing", arListType: this.commonservice.sideNaveTypeArabic('Historical Memos Outgoing') },
      { enListType: "Pending Memo List", arListType: this.commonservice.sideNaveTypeArabic('Pending Memo List') },
      { enListType: "Incoming Letters", arListType: this.commonservice.sideNaveTypeArabic('Incoming Letters') },
      { enListType: "Outgoing Letters", arListType: this.commonservice.sideNaveTypeArabic('Outgoing Letters') },
      { enListType: "My Pending Circulars", arListType: this.commonservice.sideNaveTypeArabic('My Pending Circulars') },
      { enListType: "Incoming Circulars", arListType: this.commonservice.sideNaveTypeArabic('Incoming Circulars') },
      { enListType: "Outgoing Circulars", arListType: this.commonservice.sideNaveTypeArabic('Outgoing Circulars') },
      { enListType: "Draft Circulars", arListType: this.commonservice.sideNaveTypeArabic('Draft Circulars') },
    ]
    this.actionSubscribe = this.commonservice.actionChanged$.subscribe(data => {
      if (!data.type) {
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
        this.type = this.commonservice.sideNaveTypeArabic(data.title);
        switch (data.category) {
          // case 'memo':
          //   this.firstUrl = 'المذكرات الداخلية';
          //   this.secondUrl = this.type;
          //   this.thirdUrl = 'قائمة';
          //   break;
          case 'letter':
            this.firstUrl = this.arabic('letters');
            this.secondUrl = this.type;
            this.thirdUrl = this.arabic('list');
            break;
          // case 'task':
          //   this.firstUrl = this.arabic('dutytask');
          //   this.secondUrl = this.arabic('tasklist');
          //   this.thirdUrl = '';
          //   break;
          // case 'circular':
          //   this.firstUrl = 'التعاميم الداخلية';
          //   this.secondUrl = this.type;
          //   this.thirdUrl = 'قائمة';
          //   break;
        }
      } else if (data.type == 'create') {
        switch (data.pageInfo.type) {
          // case 'memo':
          //   this.firstUrl = 'المذكرات الداخلية';
          //   this.secondUrl = 'إنشاء مذكرة';
          //   this.thirdUrl = '';
          //   break;
          case 'letter':
            this.firstUrl = this.arabic('internalletters');
            this.secondUrl = this.arabic('lettercreation');
            this.thirdUrl = '';
            break;
          // case 'task':
          //   this.firstUrl = this.arabic('dutytask');
          //   this.secondUrl = this.arabic('create');
          //   this.thirdUrl = '';
          //   break;
          // case 'circular':
          //   this.firstUrl = 'التعاميم الداخلية';
          //   this.secondUrl = 'Creation';
          //   this.thirdUrl = '';
          //   break;
        }
      } else if (data.type == 'view') {
        switch (data.pageInfo.type) {
          // case 'memo':
          //   this.firstUrl = 'المذكرات الداخلية';
          //   this.secondUrl = 'عرض المذكرة';
          //   this.thirdUrl = 'المذكرات ' + data.pageInfo.id;
          //   break;
          case 'letter':
            this.firstUrl = this.arabic('internalletters');
            this.secondUrl = this.type;
            this.thirdUrl = this.arabic('letters') + data.pageInfo.id;
            break;
          // case 'task':
          //   this.firstUrl = this.arabic('dutytask');
          //   this.secondUrl = this.arabic('create');
          //   this.thirdUrl = '';
          //   break;
          // case 'circular':
          //   this.firstUrl = 'التعاميم الداخلية';
          //   this.secondUrl = this.type;
          //   this.thirdUrl = 'Circular ' + data.pageInfo.id;
          //   break;
        }
      }
    });
  }

  arabic(word) {
    return this.commonservice.arabic.words[word];
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.actionSubscribe.unsubscribe();
  }

  clickFirstLink(){
    if(this.firstUrl == this.arabic('incomingletters') || this.firstUrl == this.arabic('outgoingletters')){
      var param={
        menu: "ar/app/letter/letter-list",
        title: this.breadcrumbttranslation.find(x => x.arListType == this.firstUrl).enListType,
        type: "letter"
      }
      this.router.navigate([param.menu]);
      setTimeout(() => {
        this.commonservice.sideNavClick(param);
      }, 500);
    }else if(this.firstUrl == this.arabic('protocolcalendar') || this.firstUrl == 'بروتوكول التقويم'){
      this.router.navigate(['/ar/app/media/calendar-management/list']);
    }else{
      this.router.navigate(['/ar/app'+this.currentLocation]);
    }
  }

  clickSecondLink(){
    if(this.firstUrl == this.arabic('internalmemos') || this.firstUrl == this.arabic('internalcirculars')){
      let param;
      if(this.firstUrl == this.arabic('internalmemos')){
        param={
          menu: "ar/app/memo/memo-list",
          title: this.breadcrumbttranslation.find(x => x.arListType == this.secondUrl).enListType,
          type: "memo"
        }
      }else if(this.firstUrl == this.arabic('internalcirculars')){
        param={
          menu: "ar/app/circular/circular-list",
          title: this.breadcrumbttranslation.find(x => x.arListType == this.secondUrl).enListType,
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
      this.router.navigate(['/ar/app'+link]);
    }
  }
}
