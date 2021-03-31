import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import { MeetingService } from '../../service/meeting.service';
import { DatePipe } from '@angular/common';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-calendar',
  templateUrl: './my-calendar.component.html',
  styleUrls: ['./my-calendar.component.scss']
})
export class MyCalendarComponent implements OnInit {
  calendarPlugins = [dayGridPlugin,timeGridPlugin];
  meetingId: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  meetingList:any=[];
  public page: number = 1;
  public pageSize: number = 10;
  public maxSize: number = 10;
  @ViewChild(FullCalendarComponent) ucCalendar: FullCalendarComponent;
  smartSearch = '';
  currentSetMonth:any;
  currentSetYear:any;
  lang: any;
  constructor(
    private common: CommonService,
    public service: MeetingService,
    public datepipe: DatePipe,
    public router: Router,
  ) { }

  calendarEvents: Array<any> = [];
  calenderViewOptions = {
    dayGridMonth:{
      eventLimit:2,
      eventLimitClick: 'timeGridDay'
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
    this.lang = this.common.currentLang;
    if (this.lang == 'en') {
      // this.common.topBanner(false, '', '', '');
      this.common.breadscrumChange('Meetings', 'My Calendar', '');
      this.calendarLocale = 'en';
      this.buttonTextOptions['month'] = 'Month';
      this.buttonTextOptions['day'] = 'Day';
      this.common.sideNavResponse('meeting', 'My Calendar');
      this.common.topSearchBanner(true, 'Meeting', '+ CREATE MEETING', '/en/app/meeting/create');
      this.allDayText = 'all-day';
      this.calendarHeaderOptions = {
        left: 'dayGridMonth,timeGridDay',
        center:'prev,next',
        right:  'title'
      };
    } else {
      // this.common.topBanner(false, '', '', '');
      this.calendarHeaderOptions = {
        left: 'title',
        center:'prev,next',
        right:  'dayGridMonth,timeGridDay'
      };
      this.common.breadscrumChange(this.arabic('meeting'), this.arabic('mycalendar'), '');
      this.calendarLocale = 'ar';
      this.buttonTextOptions['month'] = this.common.arabic.words.month;
      this.buttonTextOptions['day'] = this.common.arabic.words.day;
      this.common.sideNavResponse('meeting', this.common.arabic.sideNavArabic['My Calendar']);
      this.common.topSearchBanner(true, this.arabic('meeting'), '+ '+this.arabic('createmeeting'), '/ar/app/meeting/create');
      this.allDayText = this.arabic('allday');
    }
    // this.common.topBanner(false, '', '', '');
    this.loadList(this.smartSearch);
    this.common.searchByEvent$.subscribe(data => {
      this.loadList(data);
    });
  }

  loadList(smartSearch) {
    let MeetingID = '';
    let Subject = '';
    let StartDatetime = '';
    let EndDatetime = '';
    let MeetingType = '';
    let Location = '';
    let invitees = '';
    this.calendarEvents=[];
    this.service.getMeetingList(this.page, this.maxSize, smartSearch, this.currentUser.id, MeetingID, Subject, StartDatetime, EndDatetime, MeetingType, Location, invitees).subscribe((data:any) => {
      this.meetingList = data.Collection;
      this.meetingList.forEach((item, index) => {
        var StartDateTime = new Date(item.StartDateTime);
        this.calendarEvents.push({
          "title": item.Subject,
          "date": StartDateTime,
          start:item.StartDateTime,
          end:item.EndDateTime,
          "id": item.MeetingID,
          backgroundColor:'#A39161',
          textColor:'#FFF',
          borderColor:"#A39161"
        });
      });
      this.ucCalendar.getApi().removeAllEvents();
      this.ucCalendar.getApi().addEventSource(this.calendarEvents);
    });
  }

  handleEventClick(options) { // handler method
    console.log("options", options);
    // return false;
    this.router.navigate(['/app/meeting/view/'+ options.event.id]);
  }

  eventRender(e) {
    // const html = `<img class="clock" src="assets/meeting/contact.png">`;
    // e.el.innerHTML = html + e.el.innerHTML;
  }

  dayRender(e){
    // console.log("dayRender******", e);
    // const html = `<span style="float: right !important;"><i class="fa fa-chevron-up" ></i></span>`;
    // e.el.innerHTML = html;
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
