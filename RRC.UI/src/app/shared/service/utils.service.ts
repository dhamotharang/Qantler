import { Injectable } from '@angular/core';
import * as _ from 'lodash';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/common.service';

@Injectable()
export class UtilsService {
  currentRedirectUrl: string;
  emailRegex = /^\w.+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
  validateStartEndDate:{
    msg:any
  };


  constructor(private http: HttpClient, public common: CommonService, private endpointService: EndPointService, public datepipe: DatePipe, ) { }

  isEmptyString(value: string) {
    return _.isEmpty(_.trim(value));
  }

  isEmptyObject(value: any) {
    return _.isEmpty(value);
  }

  isEmail(email: string) {
    if (email) {
      return this.emailRegex.test(email);
    }
    return true;
  }

  isValidFromToDates(dateFrom: Date, dateTo: Date) {
    if (dateFrom && dateTo) {
      if (new Date(dateFrom).getTime() <= new Date(dateTo).getTime()) {
        return true;
      } else {
        return false;
      }
    }
    return false;
  }

  isAlreadyExist(content, value) {
    var flag = false;
    //content.splice(content.length - 1, 1);
    // content.map(res => {
    //   if (res == value) {
    //     flag = true;
    //   }
    // });
    for (let i = 0; i < content.length - 1; i++) {
      if (content[i] == value) {
        flag = true;
      }
    }
    return flag;
  }

  minDate(days, start) {
    if (start) {
      var today = new Date(start);
      var month: any = today.getMonth();
      if (today.getMonth() < 10) {
        month = '0' + (today.getMonth() + 1);
      }
      var dateLimit = (today.getFullYear()) + '/' + month + '/' + (today.getDate() + days);
      var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  maxDate(days, end) {
    if (end) {
      var endDate = new Date(end);
      var month: any = endDate.getMonth();
      if (month < 10) {
        month = '0' + (endDate.getMonth() + 1);
      }
      var dateLimit = (endDate.getFullYear()) + '/' + month + '/' + (endDate.getDate() + days);
      var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  isValidDate(date) {
    if (date) {
      let isDate: any = new Date(date);
      if (isNaN(isDate.getTime())) {
        return false;
      } else {
        return true;
      }
    }
    return false;
  }
  isDate(date) {
    if (date) {
      let isDate: any = new Date(date);
      if (isNaN(isDate.getTime())) {
        return false;
      } else {
        return true;
      }
    } else {
      return true;
    }
  }

  genarateLinkUrl(component, data, type?: number) {
    var lang = (this.common.language == 'English') ? 'en' : 'ar',
      lastLink = '',
      id = 0;
    switch (component) {
      case 'memo':
        lastLink = 'memo-view';
        id = data.MemoID;
        break;
      case 'letter':
        lastLink = (data.LetterType == 0 || type == 0) ? 'incomingletter-view' : 'outgoingletter-view';
        id = data.LetterID;
        break;
      case 'meeting':
        lastLink = 'view';
        id = data.MeetingID;
        break;
    }
    var url = '#/' + lang + '/app/' + component + '/' + lastLink + '/' + id;
    return url;
  }

  maxDateCheck(date) {
    if (date) {
      var endDate = new Date(date);
      var month: any = endDate.getMonth() + 1;
      // if (month < 10) {
      //   month = '0' + (endDate.getMonth() + 1);
      // }
      var dateLimit = (endDate.getFullYear()) + '/' + month + '/' + (endDate.getDate());
      var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  minDateCheck(date) {
    if (date) {
      var endDate = new Date(date);
      var month: any = endDate.getMonth() + 1;
      // if (month < 10) {
      //   month = '0' + (endDate.getMonth() + 1);
      // }
      var dateLimit = (endDate.getFullYear()) + '/' + month + '/' + (endDate.getDate());
      var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }


  dateValidation(datefrom, dateto) {
    let errorMsg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
    let showDateValidationMsg = false;
    if (!datefrom && dateto) {
      showDateValidationMsg = false;
    } else if (datefrom && dateto) {
      let startDate = new Date(datefrom).getTime();
      let endDate = new Date(dateto).getTime();
      if (endDate < startDate) {
        showDateValidationMsg = true;
      } else {
        showDateValidationMsg = false;
      }
    } else {
      showDateValidationMsg = false;
    }
    var param = {
      'flag': showDateValidationMsg,
      'msg': errorMsg
    }
    return param;
  }

  concatDateAndTime(StartDate, StartTime){
    if(StartDate && StartTime){
      let StartDateVal = this.datepipe.transform(StartDate, 'yyyy-MM-dd');
      let startTime = StartTime.split(" ");
      startTime = startTime[0];
      let hh = new Date (StartDateVal +" "+startTime).toJSON();
      return hh;
    }
  }

  formatAMPM(date) {
    let time;
    let mins;
    let hours;
    mins = date.getMinutes();
    hours = date.getHours();
    mins = (parseInt(mins) % 60) < 10 ? '0' + (parseInt(mins) % 60) : (parseInt(mins) % 60);
    hours = (parseInt(hours) % 60) < 10 ? '0' + (parseInt(hours) % 60) : (parseInt(hours) % 60);
    time = hours+":"+ mins;
    return time;
  }

  encryptionData(data){
    return btoa(JSON.stringify(data));
  }

  decryption(data){
    return JSON.parse(atob(data));
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
  // minDate(days) {
  //   var minDate = new Date();
  //   //this.maxDate = new Date();
  //   //return minDate.setDate(minDate.getDate() - 1);
  //   // this.minDate.setDate(this.minDate.getDate() - 1);
  //   // this.maxDate.setDate(this.maxDate.getDate() + 7);
  //   // if (this.createTask.StartDate)
  //   //   return this.createTask.StartDate.getDate();
  //   if (this.createTask.StartDate) {
  //     var today = new Date(this.createTask.StartDate);
  //     var month: any = today.getMonth() + 1;
  //     // if (today.getMonth() < 10) {
  //     //   month = '0' + (today.getMonth() + 1);
  //     // }
  //     var dateLimit = (today.getFullYear()) + '/' + month + '/' + (today.getDate() + days);
  //     var dates = this.datepipe.transform(dateLimit, 'yyyy-MM-dd');
  //     return new Date(dates);
  //   }
  // }
}
