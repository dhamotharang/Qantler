import { CalendarManagementService } from './../../service/calendar-management.service';
import { CommonService } from './../../../../common.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { Router } from '@angular/router';
import { ArabicDataService } from 'src/app/arabic-data.service';
// import * as _moment from 'moment-timezone';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {
  @ViewChild(FullCalendarComponent) ucCalendar: FullCalendarComponent;
  calendarPlugins = [dayGridPlugin,timeGridPlugin];
  lang: string;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  Month: any;
  Year: any;
  eventList: Array<any> = [];
  holidayEventList: Array<any> = [];
  public config: any = {
    paging: true,
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    pageSize: 10,
    page: 1
  };
  SmartSearch: any = '';
  currentSetMonth:any;
  currentSetYear:any;
  searchText: any = '';
  constructor(
    private common: CommonService,
    public arabic: ArabicDataService,
    private calendarManagementService: CalendarManagementService,
    private router: Router
  ) {
    this.lang = this.common.currentLang;
  }

  calendarEvents: Array<any> = [];
  calenderViewOptions = {
    dayGridMonth:{
      eventLimit:2,
      eventLimitClick: 'timeGridDay',
      eventLimitText:'more'
    },
    dayGridDay:{
      eventLimit:false
    },
    timeGridDay:{
      slotDuration:'00:30:00'
    }
  };
  calendarHeaderOptions = {
    left: 'dayGridMonth,timeGridDay',
    center:'prev,next',
    right:  'title'
  };
  columnHeaderOptions = {
    weekday:'long'
  };
  eventTimeOptions = {
    hour: 'numeric',
    minute: '2-digit',
    meridiem: 'short'
  };
  buttonTextOptions = {
    month:'Month',
    day:'Day'
  }
  calendarLocale='en';
  allDayText = 'all-day';
  ngOnInit() {
    if (this.lang == 'en') {
      this.common.topBanner(false, '', '', '');
      this.common.topSearchBanner(true, 'Protocol Calendar', '+ CREATE EVENTS', '/en/app/media/calendar-management/create-event');
        this.loadListOfRequests(this.SmartSearch);
        this.common.searchByEvent$.subscribe(data => {
          this.searchText = data;
          this.eventsForFullCalendar();
        });
      this.common.breadscrumChange('Protocol Calendar', 'Home Page', '');
      this.calendarHeaderOptions = {
        left: 'dayGridMonth,timeGridDay',
        center:'prev,next',
        right:  'title'
      };
      this.calendarLocale = 'en';
      this.buttonTextOptions['month'] = 'Month';
      this.buttonTextOptions['day'] = 'Day';
      this.allDayText = 'all-day';
      this.calenderViewOptions.dayGridMonth.eventLimitText = 'more';
    } else {
      this.common.topBanner(false, '', '', '');
      this.common.topSearchBanner(true, this.arabicfn('protocolcalendar'), '+'+this.arabicfn('createevent'), '/ar/app/media/calendar-management/create-event');
      this.loadListOfRequests(this.SmartSearch);
      this.common.searchByEvent$.subscribe(data => {
          this.searchText = data;
          this.eventsForFullCalendar();
      });
      this.common.breadscrumChange(this.arabicfn('protocolcalendar'), this.arabicfn('homepage'), '');
      this.calendarHeaderOptions = {
        left: 'title',
        center:'prev,next',
        right:  'dayGridMonth,timeGridDay'
      };
      this.calendarLocale = 'ar';
      this.buttonTextOptions['month'] = this.common.arabic.words.month;
      this.buttonTextOptions['day'] = this.common.arabic.words.day;
      this.allDayText = this.common.arabic.words['allday'];
      this.calenderViewOptions.dayGridMonth.eventLimitText = this.common.arabic.words['more'];
    }
    var today = new Date();
    var Months = today.getMonth() + 1;
    if (Months < 10) {
      var currentMonth = '0' + Months;
    }
    var currentYear = today.getFullYear();
    this.Month = currentMonth;
    this.Year = currentYear;
    // this.eventsForFullCalendar();
    // this.getHolidays();
    this.SmartSearch = '';
    this.searchText = '';
  }

  loadListOfRequests(value: any) {
    let ReferenceNumber = '';
    let EventType = '';
    let EventRequestor = '';
    let StartDate = '';
    let EndDate = '';
    let SmartSearch = '';
    let Type = '';
    if(this.SmartSearch != null && this.SmartSearch != undefined && this.SmartSearch != 'undefined'){
      SmartSearch = this.SmartSearch;
    }

    let toSendParams:any = {
      pageNumber : '',
      pageSize : '',
      UserID : '',
      Type: '',
      ReferenceNumber: '',
      EventType : '',
      EventRequestor : '',
      StartDate: '',
      EndDate : '',
      SmartSearch: SmartSearch
    };

    this.calendarManagementService.getList(toSendParams).subscribe((allOwnRes: any) => {
      if (allOwnRes) {
        this.eventList = allOwnRes.Collection;
        this.eventList.forEach((item, index) => {
          var StartDateTime = new Date(item.StartDateTime);
          this.calendarEvents.push({
            "title": item.Subject,
            "date": StartDateTime,
            "id": item.MeetingID,
          });
      });
    }
  });
}


  eventsForFullCalendar() {
    this.calendarEvents = [];
    this.calendarManagementService.fullCalendarData(this.currentUser.id, this.Month, this.Year, this.searchText).subscribe(
      (response: any) => {
        this.eventList = response.Collection;
        this.eventList.forEach(
          (item, index) => {
              var StartDate = item.DateFrom;
              if(item.Status == 120){
                item.backgroundColor = '#DA8237';
                item.textColor = '#FFF';
                item.borderColor = "#DA8237";
              } else if(item.Status == 124){
                item.backgroundColor = '#00BA58';
                item.textColor = '#FFF';
                item.borderColor = "#00BA58";
              } else if(item.Status == 121){
                item.backgroundColor = '#03ADE0';
                item.textColor = '#FFF';
                item.borderColor = "#03ADE0";
              } else if(item.Status == 123){
                item.backgroundColor = '#FF5D5A';
                item.textColor = '#FFF';
                item.borderColor = "#FF5D5A";
              }else {
                item.backgroundColor = '#A39160';
                item.textColor = '#FFF';
                item.borderColor = "#A39160";
              }
              if(item.AllDayEvents){
                this.calendarEvents.push({
                  title: item.EventRequestor,
                  date: StartDate,
                  start:item.DateFrom,
                  end: item.DateTo,
                  id: item.CalendarID,
                  backgroundColor:item.backgroundColor,
                  textColor: item.textColor,
                  borderColor:item.borderColor,
                  allDay:item.AllDayEvents,
                  startRecur:item.DateFrom,
                  endRecur:item.DateTo
                });
              } else {
                this.calendarEvents.push({
                  title: item.EventRequestor,
                  date: StartDate,
                  start:item.DateFrom,
                  end: item.DateTo,
                  id: item.CalendarID,
                  backgroundColor:item.backgroundColor,
                  textColor: item.textColor,
                  borderColor:item.borderColor,
                  allDay:item.AllDayEvents
                });
              }
          }
        );
        if (this.searchText) {
          this.holidayEventList.forEach(
            (item, index) => {
              var StartDate = new Date(item.DateFrom);
              item.backgroundColor = '#A39161';
              item.textColor = '#FFF';
              item.borderColor = "#A39161";
              this.calendarEvents.push({
                title: item.Message,
                date: item.Holiday,
                // id: item.HolidayID,
                backgroundColor:item.backgroundColor,
                textColor: item.textColor,
                borderColor:item.borderColor,
                classNames:['holiday-color-block-content'],
                allDay:true
              });
            }
          );
        }
        this.ucCalendar.getApi().removeAllEvents();
        this.ucCalendar.getApi().addEventSource(this.calendarEvents);
      }
    );
  }

  getHolidays() {
    this.calendarManagementService.holidays(this.currentUser.id, this.Month, this.Year, this.searchText).subscribe(
      (response: any) => {
        this.holidayEventList = response;
        this.holidayEventList.forEach(
          (item, index) => {
            var StartDate = new Date(item.DateFrom);
            item.backgroundColor = '#A39161';
            item.textColor = '#FFF';
            item.borderColor = "#A39161";
            this.calendarEvents.push({
              title: item.Message,
              date: item.Holiday,
              // id: item.HolidayID,
              backgroundColor:item.backgroundColor,
              textColor: item.textColor,
              borderColor:item.borderColor,
              classNames:['holiday-color-block-content'],
              allDay:true
            });
          }
        );
        this.ucCalendar.getApi().removeAllEvents();
        this.ucCalendar.getApi().addEventSource(this.calendarEvents);
      }
    );
  }

  handleEventClick(options) { // handler method
    if(options.event.id){
      this.router.navigate(['/app/media/calendar-management/view-event/'+ options.event.id]);
    }
  }

  eventRender(e) {
    // const html = `<img class="clock" src="assets/home/Meeting-clock.png">`;
    // e.el.innerHTML = html + e.el.innerHTML;
    if(e.event.title && !e.event.id){
      let holidayDate:any = new Date(e.event.start).getDate();
      let holidayMonth:any =(new Date(e.event.start).getMonth()+1);
      let holidayYear =  new Date(e.event.start).getFullYear();
      if(holidayMonth < 10){
        holidayMonth = '0'+ holidayMonth;
      }
      let dateString =holidayYear + '-' +  holidayMonth + '-' +  holidayDate ;
      e.event.source.calendar.el.querySelector('.fc-day[data-date="' + dateString + '"]').classList.add('holiday-color-block');
      e.event.source.calendar.el.querySelector('.fc-day-top[data-date="' + dateString + '"]').classList.add('holiday-color-block');
    }
  }

  dayRender(e){
    // console.log("dayRender******", e);
    // const html = `<span style="float: right !important;"><i class="fa fa-chevron-up" ></i></span>`;
    // e.el.innerHTML = html;
  }

  monthChange(event){
    let changedMonth:any = new Date(event.view.currentStart).getMonth()+1;
    if (changedMonth < 10) {
      changedMonth = '0' + changedMonth;
    }
    let currentYear = new Date(event.view.currentStart).getFullYear();
    if(this.currentSetMonth != changedMonth){
      this.Month = changedMonth;
      this.currentSetMonth = changedMonth;
      this.Year = currentYear;
      this.calendarEvents = [];
      this.eventsForFullCalendar();
      this.getHolidays();
    }
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }

}
