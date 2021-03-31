import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from '../../api/endpoint.service';
import { CommonService } from 'src/app/common.service';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';
@Injectable({
  providedIn: 'root'
})
export class TaskService {
  public user = JSON.parse(localStorage.getItem("User"));
  public visibleChat = false;

  constructor(private httpClient: HttpClient, private endpoint: EndPointService, public common: CommonService, public _ScrollToService: ScrollToService) { }
  triggerScrollTo() {
    const config: ScrollToConfigOptions = {
      target: 'destination',
      offset: 300
    };
    this._ScrollToService.scrollTo(config);
  }

  saveTask(APIString, data) {
    if (data.TaskID && data.TaskID > 0) {
      return this.httpClient.put(this.endpoint.apiHostingURL + '/' + APIString, data);
    } else {
      return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString, data);
    }
  }

  viewTask(APIString, id, userId, clone = '') {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?userid=' + userId);
  }

  patchTask(APIString, data, id) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, data);
  }

  deleteTask(APIString, id, userId) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userId);
  }

  getTaskCount(userId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/DutyTasks/Home/Count/' + userId);
  }

  getTaskList(pageno, pagesize, userId, type, filderData) {
    var req = Object.keys(filderData);
    var param = '';
    req.map(res => {
      if (filderData[res] != '')
        param += '&' + res + '=' + filderData[res];
    });
    return this.httpClient.get(this.endpoint.apiHostingURL + '/DutyTasks/Home/List/' + pageno + ',' + pagesize + '?UserID=' + userId + '&Type=' + type + param);
  }

  getDatabyId(APIString, id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id);
  }

  sendChat(APIString, data) {
    return this.httpClient.post(this.endpoint.apiHostingURL + APIString, data);
  }

  getLetters(APIString: string, userid, refno) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + userid + '?ReferenceNumber=' + refno);
  }
  getMemos(APIString: string, userid, refno) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + userid + '?ReferenceNumber=' + refno);
  }

  breadscrumChange(status, type, id, lang = 'en') {
    switch (type) {
      case 'Task Create':
        if (lang == 'en')
          this.common.breadscrumChange('Duty Tasks', 'Task Creation', '');
        else
          this.common.breadscrumChange(this.arabic('dutytask'), this.arabic('taskcreate'), '');
        break;
      // case 'List':
      //     if (lang == 'en')
      //         this.common.breadscrumChange('Internal Memos', status, 'List');
      //     else
      //         this.common.breadscrumChange(this.arabic('internalmemos'), this.arabicNav(status), this.arabic('list'));
      //     break;
      case 'Task View':
        switch (status) {
          case 1:
            if (lang == 'en')
              this.common.breadscrumChange('Duty Tasks', 'Task View', id);
            else
              this.common.breadscrumChange(this.arabic('dutytask'), this.arabic('taskview'), + id);
            break;
          case 2:
            if (lang == 'en')
              this.common.breadscrumChange('Duty Tasks', 'Task View', id);
            else
              this.common.breadscrumChange(this.arabic('dutytask'), this.arabic('taskview'), + id);
            break;
          case 3:
            if (lang == 'en')
              this.common.breadscrumChange('Duty Tasks', 'Task View', id);
            else
              this.common.breadscrumChange(this.arabic('dutytask'), this.arabic('taskview'), + id);

            break;
          case 4:
            if (lang == 'en')
              this.common.breadscrumChange('Duty Tasks', 'Task View', id);
            else
              this.common.breadscrumChange(this.arabic('dutytask'), this.arabic('taskview'), + id);
            break;
          // case 5:
          //     if (lang == 'en')
          //         this.common.breadscrumChange('Duty Tasks', 'Task View', 'Memo ' + id);
          //     else
          //         this.common.breadscrumChange(this.arabic('dutytask'), 'سجلات المذكرات الواردة السابقة', 'المذكرات ' + id);
          //     break;
          // case 6:
          //     if (lang == 'en')
          //         this.common.breadscrumChange('Duty Tasks', 'Task View', 'Memo ' + id);
          //     else
          //         this.common.breadscrumChange(this.arabic('dutytask'), 'سجلات المذكرات الصادرة السابقة', 'المذكرات ' + id);

          //  break;
        }
        break;
    }
  }

  downlaodExcel(data) {
    var date = new Date,
      cur_date = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
    this.httpClient.post(this.endpoint.apiHostingURL + '/DutyTasks/Report?type=Excel', data, { responseType: 'blob' })
      .subscribe((resultBlob: Blob) => {
        var url = window.URL.createObjectURL(resultBlob);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = 'DutyTaskReport-' + cur_date + '.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
      });
  }

  arabicNav(word) {
    return this.common.arabic.sideNavArabic[word];
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  getCountries(userId: any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Country?UserID=${userId}&Module=DutyTask`);
  }

  getEmiratesList(userID) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/Emirates?UserID=' + userID);
  }

  getCities(userId: any, emiratesId: any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/City?UserID=${userId}&EmiratesID=${emiratesId}`);
  }

  memoList(APIString: string, pageNo, pageSize, type, userName) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/'
      + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userName);
  }

  letterFilterList(
    APIString: string,
    pageNo,
    pageSize,
    type,
    UserID,
    Status,
    SourceOU,
    DestinationOU,
    UserName,
    DateFrom,
    DateTo,
    Priority,
    SenderName,
    SmartSearch) {
    return this.httpClient.get(this.endpoint.apiHostingURL
      + '/' + APIString + pageNo + ',' + pageSize
      + '?Type=' + type + '&UserID=' + UserID + '&Status=' + Status
      + '&Source=' + SourceOU
      + '&Destination=' + DestinationOU + '&UserName=' + UserName
      + '&DateRangeFrom=' + DateFrom + '&DateRangeTo=' + DateTo + '&Priority=' + Priority
      + '&SenderName=' + SenderName + '&SmartSearch=' + SmartSearch);
  }

  getMeetingList(pageNumber: number, pageSize: number, smartSearch, UserID, MeetingID, Subject, StartDatetime, EndDatetime, MeetingType, location, invitees) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/Meeting/ListView' + pageNumber + ',' + pageSize + "?ReferenceNumber=" + MeetingID + "&Subject=" + Subject +
      "&UserId=" + UserID + "&StartDatetime=" + StartDatetime +
      "&EndDatetime=" + EndDatetime + "&MeetingType=" + MeetingType + "&Invitees=" + invitees + "&location=" + location + "&SmartSearch=" + smartSearch);
  }

  dateValidate(from, to) {
    if (this.dateCalc(from) > this.dateCalc(to))
      return false;
    else
      return true;
  }

  dateCalc(date) {
    return date.getFullYear() + date.getMonth() + date.getDate();
  }
}