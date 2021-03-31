import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Headers, RequestOptions, URLSearchParams, Http, HttpModule } from '@angular/http';
import { EndPointService } from './api/endpoint.service';
import { ArabicDataService } from './arabic-data.service';
import { environment } from 'src/environments/environment';
import { ScrollToConfigOptions, ScrollToService } from '@nicky-lenaers/ngx-scroll-to';


@Injectable({
  providedIn: 'root'
})
export class CommonService {
  private headers = new Headers({ 'Content-Type': 'application/json', 'Accept': 'q=0.8;application/json;q=0.9' });
  language: any = this.arabic.words['arabic'];
  currentLang: any = 'ar';
  public priorityList = [];
  public formPriorityList = []; // for priority dropdown in form level 
  public documentList = [];
  public ReportpriorityList = [];
  public assetBase = '../ass'
  public currentUser: any = '';
  public userList: any;
  public currentScreen: any;
  isNavigateTrigger = false;
  public actionLetterChanged$;
  
  public IsShowSearch = new BehaviorSubject<boolean>(false);
  public IsContactSearch = new BehaviorSubject<boolean>(false);
  public IsShowSearchrtl = new BehaviorSubject<boolean>(false);
  public IsContactSearchrtl = new BehaviorSubject<boolean>(false);

  private actionChanged = new Subject<any>();
  public actionChanged$ = this.actionChanged.asObservable();

  private contactChanged = new Subject<any>();
  public contactChanged$ = this.contactChanged.asObservable();

  private memoViewChanged = new Subject<any>();
  public memoViewChanged$ = this.memoViewChanged.asObservable();

  private circularViewChanged = new Subject<any>();
  public circularViewChanged$ = this.circularViewChanged.asObservable();

  public sideNavChanged = new Subject<any>();
  public sideNavChanged$ = this.sideNavChanged.asObservable();

  public sideNavMemoChanged = new Subject<any>();
  public sideNavMemoChanged$ = this.sideNavMemoChanged.asObservable();

  private createComponenetBtn = new Subject<any>();
  public createComponenetBtn$ = this.createComponenetBtn.asObservable();

  private isShowContact = new Subject<any>();
  public isShowContact$ = this.isShowContact.asObservable();


  private showTopBanner = new Subject<any>();
  public showTopBanner$ = this.showTopBanner.asObservable();

  public pageLoad = new Subject<any>();
  public pageLoad$ = this.pageLoad.asObservable();

  private mobileSideChanged = new Subject<any>();
  public mobileSideChanged$ = this.mobileSideChanged.asObservable();

  private sideNav = new Subject<any>();
  public sideNav$ = this.sideNav.asObservable();

  private langChange = new Subject<any>();
  public langChange$ = this.langChange.asObservable();

  private showTopBannerSearch = new Subject<any>();
  public showTopBannerSearch$ = this.showTopBannerSearch.asObservable();

  private searchByEvent = new Subject<any>();
  public searchByEvent$ = this.searchByEvent.asObservable();

  private escalateModalClose = new Subject<any>();
  public escalateModalClose$ = this.escalateModalClose.asObservable();

  private apologiesModalClose = new Subject<any>();
  public apologiesModalClose$ = this.apologiesModalClose.asObservable();

  private commentModalClose = new Subject<any>();
  public commentModalClose$ = this.commentModalClose.asObservable();

  public createCarPopup = new Subject<any>();
  public createCarPopup$ = this.createCarPopup.asObservable();

  



  constructor(public endpoint: EndPointService,
    public httpClient: HttpClient,
    private http: Http,
    public arabic: ArabicDataService,
    public _ScrollToService: ScrollToService
  ) {
    var user = localStorage.getItem('User');
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.language = this.getCookie();
    this.currentLang = (this.language == 'English') ? 'en' : 'ar';
    //setTimeout(() => {
      this.callEvent();
    //}, 500);
  }

  callEvent() {
    if (this.language != 'English') {
      this.ReportpriorityList = [this.arabic['all'], this.arabic.words['high'], this.arabic.words['medium'], this.arabic.words['low'], this.arabic.words['verylow']];
      this.priorityList = [this.arabic.words['all'], this.arabic.words['high'], this.arabic.words['medium'], this.arabic.words['low'], this.arabic.words['verylow']];
      this.documentList = [this.arabic.words['public'], this.arabic.words['restricted'], this.arabic.words['confidential'], this.arabic.words['secret']];
      this.formPriorityList = [this.arabic.words['high'], this.arabic.words['medium'], this.arabic.words['low'], this.arabic.words['verylow']];
    } else {
      this.ReportpriorityList = ['All', 'High', 'Medium', 'Low', 'VeryLow'];
      this.priorityList = ['All', 'High', 'Medium', 'Low', 'VeryLow'];
      this.documentList = ['Public', 'Restricted', 'Confidential', 'Secret'];
      this.formPriorityList = ['High', 'Medium', 'Low', 'Verylow'];
    }
  }



  private extractData(res: any) {
    let body = res.json();
    return body || [];
  }

  sideNavResponse(data, title) {
    var param = {
      type: data,
      title: title
    }
    this.sideNav.next(param);
  }

  public sideNavClick(data) {
    //console.log('memo');
    if (data.type == 'memo')
      this.sideNavMemoChanged.next(data);
    else
      this.sideNavChanged.next(data);
  }
  public action(data) {
    this.actionChanged.next(data);
  }

  public viewMemo(type) {
    this.memoViewChanged.next(type);
  }

  public viewCircular(type) {
    this.circularViewChanged.next(type);
  }

  pageReLoad(data) {
    console.log(data);
    localStorage.setItem("sideNavTigger", 'true');
    this.pageLoad.next(data);
  }

  mobileSideClick(data) {
    this.mobileSideChanged.next(data);
  }

  public breadscrumChange(firstURL, secondURL, thirdURL) {
    var param = {
      first: firstURL,
      second: secondURL,
      third: thirdURL
    }
    this.action(param);
  }

  public postAttachment(data) {
    var formData = new FormData();
    var headers = new HttpHeaders();
    for (var i = 0; i < data.length; i++) {
      formData.append('files', data[i]);
    }
    headers.append("Accept", 'application/json');
    headers.delete("Content-Type");
    const requestOptions = {
      headers: headers,
      reportProgress: true,
      observe: 'events'
    };
    //formData.append('files', data);
    return this.httpClient.post(this.endpoint.apiHostingURL + '/attachment/upload', formData, {
      reportProgress: true,
      observe: 'events'
    });
  }
  downloadattachment(filename, guid) {
    this.httpClient.post(this.endpoint.apiHostingURL + '/attachment/download?filename=' + filename + '&guid=' + guid, '');
  }

  public topBanner(isShow, page_title, btn_name, url) {
    var param = {
      isShow: isShow,
      btn_name: btn_name,
      url: url,
      title: page_title
    }
    this.showTopBanner.next(param);
  }

  public topSearchBanner(isShow, page_title, btn_name, url) {
    var param = {
      isShow: isShow,
      btn_name: btn_name,
      url: url,
      title: page_title
    }
    this.showTopBannerSearch.next(param);
  }

  setSearchByEvent(data) {
    this.searchByEvent.next(data);
  }

  public memoClick(param) {

  }

  public letterClick(param) {

  }

  public getUserList(data: any, id) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/User?UserID=' + id, data);
  }
  public getAssigneeUserList(data: any, id) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/User?UserID=' + id + '&type=2', data);
  }
  public getmemoUserList(data: any, id) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/User?UserID=' + id + '&type=1', data);
  }


  savememo(APIString: string, id, data: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, data);
  }

  public getSearchDetail(pageno, pagesize, userId, searchstring, type) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/Home/GlobalSearchList/' + pageno + ',' + pagesize +
      '?UserID=' + userId + '&SmartSearch=' + searchstring + '&Type=' + type);
  }
  shareparticipant(APIString: string, id, userid: any, comments, data: any) {
    //  let options = new RequestOptions({ headers: this.headers });

    // const url = this.endpoint.apiHostingURL+'/'+APIString+'/'+id;
    //     return this.http
    //         .post(url,data, options)
    //         .toPromise()
    //         .then(res => res.json() as any)
    //         .catch(this.handleError);

    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?Type=memo&UserID=' + userid + '&Comments=' + comments, data);

  }

  createComponentBtn(show) {
    this.createComponenetBtn.next(show);
  }

  previewPdf(refno) {
    return this.httpClient.get(environment.DownloadUrl + refno + '.pdf', { responseType: 'blob' });
  }

  printPdf(refno, type?: any) {
    this.httpClient.get(environment.DownloadUrl + refno + '.pdf', { responseType: 'blob' })
      .subscribe((data: Blob) => {
        var url = window.URL.createObjectURL(data);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        //a.download = refno + '.pdf';
        a.target = '_blank';
        a.click();
        // window.URL.revokeObjectURL(url);
        // a.remove();
        this.deleteGeneratedFiles('files/delete', refno + '.pdf').subscribe(result => {
          console.log(result);
        });
      });
  }

  patch(APIString, data, id) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, data);
  }

  delete(APIString, id) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + APIString + '/' + id);
  }

  formatPatchData(value, path, comment = '') {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    var data = [
      {
        "value": value,
        "path": path,
        "op": "Replace",
      },
      {
        "value": comment,
        "path": 'Comments',
        "op": "Replace"
      },
      {
        "value": this.currentUser.id,
        "path": 'UpdatedBy',
        "op": "Replace"
      },
      {
        "value": new Date(),
        "path": 'UpdatedDateTime',
        "op": "Replace"
      }
    ];
    return data;
  }

  getEmpDetails(empId: any, userId: any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/UserProfile/${empId}?UserId=${userId}`);
  }

  getTaskCount(userId: any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/DutyTasks/Home/Count/' + userId);
  }

  languageChange(lang) {
    if (lang == 'ar') {
      this.ReportpriorityList = [this.arabic.words['all'], this.arabic.words['high'], this.arabic.words['medium'], this.arabic.words['low'], this.arabic.words['verylow']];
      this.priorityList = [this.arabic.words['all'],this.arabic.words['high'], this.arabic.words['medium'], this.arabic.words['low'], this.arabic.words['verylow']];
      this.documentList = [this.arabic.words['public'], this.arabic.words['restricted'], this.arabic.words['confidential'], this.arabic.words['secret']];
    } else {
      this.ReportpriorityList = ['All', 'High', 'Medium', 'Low', 'VeryLow'];
      this.priorityList = ['All','High', 'Medium', 'Low', 'Verylow'];
      this.documentList = ['Public', 'Restricted', 'Confidential', 'Secret'];
    }
    this.langChange.next(lang);
  }

  validateEmail(data) {
    return (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(data));
  }

  validatePhone(number) {

  }

  setCookie(cName, cValue) {
    document.cookie = cName + ' = ' + cValue;
    this.language = cValue;
    this.currentLang = (this.language == 'English') ? 'en' : 'ar';
  }

  getCookie() {
    //debugger;
    var cookies = document.cookie.split(';'),
      cookie = '',
      lang = this.arabic.words['arabic'],
      cName = ' language=';
    cookies.map(res => {
      cookie = res.charAt(0) + res.charAt(1);
      if (cookie == ' l') {
        lang = res.substring(res.length, cName.length);
      } else if (cookie == 'la') {
        lang = res.split('=')[1];
        //lang = 'Arabic';
      }
    });
    return lang;
  }

  sideNaveTypeArabic(type) {
    var data = this.arabic.sideNavArabic[type];
    if (!data || typeof (data) == 'undefined' || data == null) {
      data = type;
    }
    return data;
  }


  private handleError(error: any): Promise<any> {
    //console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }

  setEscalateModalClose(data) {
    this.escalateModalClose.next(data);
  }

  setAplogiesModalClose(data) {
    this.apologiesModalClose.next(data);
  }

  setCommentModalClose(data) {
    this.commentModalClose.next(data);
  }

  showContact() {
    this.isShowContact.next();
  }

  triggerScrollTo(position) {
    const config: ScrollToConfigOptions = {
      target: position,
      offset: 200
    };
    this._ScrollToService.scrollTo(config);
  }
  homeScrollTop() {
    //var body = document.getElementById('body_temp');
    setTimeout(() => {
      document.body.scrollTop = 0;
      // document.documentElement.scrollTop = 0;
      for (let i = document.documentElement.scrollTop; i > 0; i--) {
        document.documentElement.scrollTop = i;
      }
    }, 100);
  }

  routingProcess(urls, lang) {
    var url = '';
    if (lang == 'English') {
      urls.map(res => {
        if (res != '' && res != 'en' && res != 'ar')
          url += '/' + res;
      });
      url = '/en' + url;
    } else {
      urls.map(res => {
        if (res != '' && res != 'en' && res != 'ar')
          url += '/' + res;
      });
      url = '/ar' + url;
    }
    return url;
  }

  checkLangUrl(url) {
    var urlArr = url.split('/'),
      urlLang = urlArr[1];
    if (urlLang != this.currentLang) {
      return this.routingProcess(urlArr, this.language);
    } else {
      return false;
    }
  }

  historyLog(status) {
    if (status == 'Reject' || status == 'Redirect') {
      return (this.language == 'English') ? status + 'ed' : this.arabic.words[status + 'ed'];
    } else if (status == 'Submit') {
      return (this.language == 'English') ? 'Submitted' : this.arabic.words['submitted'];
    } else if (status == 'Resubmit') {
      return (this.language == 'English') ? status + 'ted' : this.arabic.words[status + 'ted'];
    } else if (status == 'ReturnForInfo') {
      return (this.language == 'English') ? 'ReturnForInfo' : this.arabic.words['returnforinfo'];
    } else if (status == 'AssignTo' || status == 'AssignToMe') {
      return (this.language == 'English') ? 'Assigned' : this.arabic.words['assign'];
    } else {
      return (this.language == 'English') ? status + 'd' : this.arabic.words[status + 'd'];
    }
  }

  contactChangeTrigger(){
    this.contactChanged.next();
  }

  // Notification
  getNotification(pageNo,pageSize,userId){
    return this.httpClient.get(this.endpoint.apiHostingURL + '/Home/Notification/' + pageNo + ',' + pageSize + '?UserID=' + userId);
  }
  getEachNotification(APIString: string, serviceId, userId){
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/' + serviceId + '?UserID=' + userId,'');
  }
  readNotification(APIString: string, allRead: any, serviceID: any, userId){
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/' + serviceID + '?MarkAllAsRead=' + allRead + '&UserID=' + userId,'');
  }
  readAllNotification(APIString: string, allRead: any, userId){
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/0' + '?MarkAllAsRead=' + allRead + '&UserID=' + userId,'');
  }

  deleteGeneratedFiles(APIString, refNo){
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '?filename=' + refNo);
  }

}
