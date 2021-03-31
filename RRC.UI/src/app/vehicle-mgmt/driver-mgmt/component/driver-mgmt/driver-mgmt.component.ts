
import { Component, OnInit, ViewChild, Inject, Renderer2, TemplateRef, ElementRef } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { Router, ActivatedRoute } from '@angular/router';
import timeGridPlugin from '@fullcalendar/timegrid';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { MaintenanceService } from 'src/app/maintenance/service/maintenance.service';
import { FullCalendarComponent } from '@fullcalendar/angular';
import resourceTimelinePlugin from '@fullcalendar/resource-timeline';
import { environment } from 'src/environments/environment';
import resourceTimeline from '@fullcalendar/resource-timeline';
import { HttpResponse } from '@angular/common/http';
import { VehicleMgmtServiceService } from 'src/app/vehicle-mgmt/service/vehicle-mgmt-service.service';
import { CalenderFormat } from 'src/app/vehicle-mgmt/model/vehicle-details/CalenderFormat';
import { resourcesArr } from 'src/app/vehicle-mgmt/model/resourcesArr';
import { Calendar } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import momentPlugin from '@fullcalendar/core/CalendarComponent';
import { Collection } from 'src/app/vehicle-mgmt/model/vehicle-details/Collection';
import { DatePipe, DOCUMENT } from '@angular/common';
import resourceTimeGridPlugin from '@fullcalendar/resource-timegrid';
import resourceDayGridPlugin from '@fullcalendar/resource-daygrid';
import Tooltip from 'Tooltip.js';
import $ from 'jquery';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { EventInput } from '@fullcalendar/core' 
import timeGrigPlugin from '@fullcalendar/timegrid'; 
  
import * as moment from 'moment'; 

@Component({
  selector: 'app-driver-mgmt',
  templateUrl: './driver-mgmt.component.html',
  styleUrls: ['./driver-mgmt.component.scss',]
})
export class DriverMgmtComponent implements OnInit {
  tooltip: Tooltip;
  title = "";
  lang: any;
  _Collections = new Collection();
  // testArray:any[]=[];
  test: any = "To:Abudhabi" + ", With:Ahmed  "
  test1: any = "To:Madinat Zayed" + ",  With:Ali  "
  test2: any = "To:Abudhabi  " + ",  With:Muhammad  "
  isLoading: boolean;
  @ViewChild('ucCalendar') ucCalendar: FullCalendarComponent;
  calendarPlugins = [resourceTimeGridPlugin, resourceDayGridPlugin, resourceTimelinePlugin, resourceTimeline];
  // defaultView:string;
  schedulerLicenseKey = environment.schedulerLicenseKey;
  calendarEvents: CalenderFormat[] = [];
  bsModalRef: BsModalRef;
  html:string="";
  bsConfig: Partial<BsDatepickerConfig>;
  @ViewChild('template') template: TemplateRef<any>;
  // calendarEvents:any[]=[

  // ];



  calenderViewOptions = {
    resourceTimelineDay: {
      eventLimit: true
    },
  };
  calendarHeaderOptions = {
    left: '',
    center: 'prev,next',
    right: 'title'
  };
  columnHeaderOptions = {
    weekday: 'long'
  };
  eventTimeOptions = {
    hour: 'numeric',
    minute: '2-digit',
    meridiem: 'short'
  };
  buttonTextOptions = {
    month: 'Month',
    day: 'Day'
  }
  calendarLocale = 'en';
  allDayText = 'all-day';
  resourcesArr1: any[] = [];
  resourcesArr: any[] = [];
  resourceLabel = 'Driver Name';
  selectedDate: any = "";
  calendarConfig: Calendar;
  calendarEl: HTMLElement;
  flag: boolean = false;
  scrollTime: '00:00:00';
  calendarViewDate: any;
  pdfSrc: string;
  inProgress = false;
  isHidden = true;
  isEventClick=false;
 // title : any;
  constructor(private Datepipe: DatePipe, private renderer: Renderer2, @Inject(DOCUMENT) private document: Document, private _VchlMgmtService: VehicleMgmtServiceService, public common: CommonService, public router: Router, private maintenanceService: MaintenanceService, public modalService: BsModalService) {

    this.lang = this.common.currentLang;
    if (this.lang == 'en') {
      this.common.breadscrumChange('Vehicle Management', 'Driver Management', '');
      this.common.topBanner(true, 'Driver List', 'Manage Drivers', '/en/app/vehicle-management/driver-management/manage-driver');
      this.calendarHeaderOptions = {
        left: '',
        center: 'prev,next',
        right: 'title'
      };
      this.calendarLocale = 'en';
    } else {
      this.resourceLabel = this.common.arabic.words.drivername;
      this.common.breadscrumChange(this.common.arabic.words.vehiclemgmt, this.common.arabic.words.vehiclereturnconfirmation, '');
      this.common.topBanner(true, this.common.arabic.words.driverlist, this.common.arabic.words.managedrivers, '/en/app/vehicle-management/driver-management/manage-driver');
      this.calendarHeaderOptions = {
        left: 'title',
        center: 'prev,next',
        right: ''
      };
      this.calendarLocale = 'ar';
    }
  }
  ngOnInit() {
    debugger
    this.title = "";
    this.isHidden=true;
    this.isEventClick=false;
    this.isLoading = true;
    let currentDate = this.Datepipe.transform(new Date(), 'yyyy-MM-dd');
    this.getCalendarEvents(currentDate);
    // this.calendarEvents = [];
    // let currentDate = this.Datepipe.transform(new Date(), 'yyyy-MM-dd');
    // const response = <HttpResponse<any>>await this._VchlMgmtService.getDriversList(1, 15, currentDate);
    // this._Collections = response.body;
    // this.isLoading = false;
    // for (let i = 0; i < response.body.Collection.length; i++) {
    //   this.resourcesArr1.push({ "id": response.body.Collection[i].DriverID, "title": response.body.Collection[i].DriverName, "eventColor": 'skyblue' })
    //   this.resourcesArr = this.resourcesArr1;

    //   if (response.body.Collection[i].DriverTrips.length > 0) {
    //     let temlength = response.body.Collection[i].DriverTrips
    //     for (let j = 0; j < temlength.length; j++) {
    //       // let tempArray = new CalenderFormat()
    //       let tempArray = { resourceId: null, start: '', end: '', title: '' }
    //       tempArray.resourceId = response.body.Collection[i].DriverID;
    //       tempArray.start = temlength[j].TripPeriodFrom;
    //       tempArray.end = temlength[j].TripPeriodTo;
    //       tempArray.title = " With :" + temlength[j].With + " , To: " + temlength[j].TO;
    //       this.calendarEvents.push(tempArray);
    //     }
    //   }
    // }

    // var _this = this;
    // _this.calendarEl = document.getElementById("calendar");
    // _this.calendarConfig = new Calendar(_this.calendarEl, {
    //   plugins: [  dayGridPlugin, timeGridPlugin, resourceTimelinePlugin],
    //   now: new Date(),
    //   schedulerLicenseKey: environment.schedulerLicenseKey,
    //   editable: true, // enable draggable events
    //   aspectRatio: 1.8,
    //   // eventDurationEditable: true,
    //   // eventResourceEditable: true,
    //   timeGridEventMinHeight: 200,
    //   locale: _this.calendarLocale,
    //   timeZone: 'UTC',
    //   scrollTime: '00:00:00', // undo default 6am scrollTime
    //   // slotDuration: '02:00:00',
    //   dragScroll: true,
    //   header: _this.calendarHeaderOptions,
    //   defaultView: 'resourceTimeline',
    //   views: {
    //     resourceTimelineThreeDays: {
    //       type: 'resourceTimeline',
    //       duration: { days: 2 },
    //       buttonText: '3 day',
    //     }
    //   },
    //   eventTextColor: 'black',
    //   resourceLabelText: _this.resourceLabel,
    //   resources: _this.resourcesArr,
    //   events: _this.calendarEvents,
    //   filterResourcesWithEvents: true,
    //   progressiveEventRendering: true,
    //   datesRender: function (info) {
    //      let gettingDate = info.view.activeStart;
    //      let dateselected = _this.Datepipe.transform(gettingDate, 'yyyy-MM-dd');
    //      _this.resourcesArr = []; _this.calendarEvents = []; _this.resourcesArr1 = [];
    //      if(dateselected != currentDate){
    //      _this.getCalendarEvents(dateselected);
    //      }

    //   },
    // });
    // _this.calendarConfig.render();

  }

  handleEventRender(info, element) {
    $(element).tooltip({ title: info.title });
    // var tooltip = new Tooltip(info.el, {
    //   title: info.event.extendedProps.title,
    //   placement: 'top',
    //   trigger: 'hover',
    //   container: 'body'
    // });
  }
  // eventMouseover(info) {
  //   debugger;
  //   this.title='';
  //   var tis = info.el;
  //   var popup = info.event.extendedProps.popup;
  //   var tooltip = '<div class="tooltipevent" style="top:' + ($(tis).offset().top - 5) + 'px; left:' + ($(tis).offset().left + ($(tis).width()) / 2) + 'px"><div>' + popup.title + '</div><div>' + popup.descri + '</div></div>';
  //   var $tooltip = $(tooltip).appendTo('body');
  // }

//   eventMouseEnter(info,template: TemplateRef<any>) {
//    debugger
//    this.title = info.event.title;
//    //this.bsModalRef = this.modalService.show(template, { class: 'modal' });
//    //const initialState = { message: 'popup message', title:'popup title'};
// //let bsModalRef = this.modalService.show(template, Object.assign({}, th, { class: 'modal-sm', initialState })
//    // alert(mouseEnterInfo.type)
//   }

  
  // eventMouseout(info) {
  //   this.title = "";
  //   console.log('eventMouseLeave');
  //   $(info.el).css('z-index', 8);
  //   $('.tooltipevent').remove();
  // }

  handleDatesRender(info) {
    var _this = this;
    let gettingDate = info.view.activeStart;
    let dateselected = _this.Datepipe.transform(gettingDate, 'yyyy-MM-dd');
    _this.resourcesArr = []; _this.calendarEvents = []; _this.resourcesArr1 = [];
    _this.getCalendarEvents(dateselected);
  }
  // eventRender(eventObj) {
  //   debugger
  //   console.log(eventObj);
  //   // eventObj.source.popover({
  //   //   title: eventObj.event.title,
  //   //   trigger: 'hover',
  //   //   placement: 'top',
  //   //   container: 'body'
  //   // });
 // }

 eventrenderFunction(event) {
   debugger
   this.isHidden=false;
  //  debugger
   // event.el.getElementsByClassName('fc-title-wrap')[0].setAttribute("onBlur", "clickCalender()"); 
    // event.el.getElementsByClassName('fc-title-wrap')[0].setAttribute("triggers", "mouseenter:mouseleave");   
//event.el.getElementsByClassName('fc-title-wrap')[0].setAttribute('title', event.event.title);   
  this.title=event.event.title;
this.isEventClick=true;
}
clickCalender(){
  debugger
  if(!this.isEventClick)
  {
    this.isHidden=true;
  }
  this.isEventClick=false;  
}
  getCalendarEvents(date) {
    this.calendarViewDate = date;
    var _this = this;
    _this._VchlMgmtService.getDriversCalendar(0, 0, date).subscribe((result: any) => {
      _this.isLoading = false;
      _this._Collections = result;

      for (let i = 0; i < result.Collection.length; i++) {
        _this.resourcesArr1.push({ "id": result.Collection[i].DriverID, "title": result.Collection[i].DriverName, "eventColor": 'skyblue' })
        _this.resourcesArr = _this.resourcesArr1;
        if (result.Collection[i].DriverTrips.length > 0) {
          let temlength = result.Collection[i].DriverTrips
          for (let j = 0; j < temlength.length; j++) {
            let tempArray = { resourceId: null, start: '', end: '', title: '' }
            tempArray.resourceId = result.Collection[i].DriverID;
            tempArray.start = temlength[j].TripPeriodFrom;
            tempArray.end = temlength[j].TripPeriodTo;
            tempArray.title = " With : " + temlength[j].With + " , To : " + temlength[j].TO;
            _this.calendarEvents.push(tempArray);
          }
        }
        console.log("Current resources:" + _this.resourcesArr[i].title);
      }
      console.log("Current Events:" + _this.calendarEvents);
      // _this.calendarConfig.removeAllEvents();
      // _this.calendarConfig.addResource(_this.resourcesArr);
      // _this.calendarConfig.addEvent(_this.calendarEvents); 
      // _this.calendarConfig.refetchEvents();
      // _this.calendarConfig.refetchResources();
      // _this.calendarConfig.rerenderEvents();
      // _this.calendarConfig.rerenderResources(); 
      _this.ucCalendar.getApi().removeAllEvents();
      _this.ucCalendar.getApi().addResource(_this.resourcesArr);
      _this.ucCalendar.getApi().addEventSource(_this.calendarEvents);
    });
  }

  async CalenderAction() {



    // this.defaultView = "timelineDay"
    // this.lang = this.common.currentLang;
    // this.common.breadscrumChange('Vehicle Management', 'Driver Management', '');
    // this.common.topBanner(true, '', 'Manage Drivers', '/en/app/vehicle-management/driver-management/manage-driver');
    // this.calendarHeaderOptions = {
    //   left: '',
    //   center: 'prev,next',
    //   right: 'title',
    // };
    // this.calendarEvents = [{ title: 'Event Now', start: new Date() }];
    // this.calendarLocale = 'en';
    // this.buttonTextOptions['month'] = 'Month';
    // this.buttonTextOptions['day'] = 'Day';
    // this.allDayText = 'all-day';
    // this.resourceLabel = 'Driver Name';

    // if (this.lang == 'ar') {
    //   this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('vehiclereturnconfirmation'), '');
    //   this.common.topBanner(true, '', 'Manage Drivers', '/en/app/vehicle-management/driver-management/manage-driver');
    //   this.calendarHeaderOptions = {
    //     left: 'title',
    //     center: 'prev,next',
    //     right: ''
    //   };
    //   this.calendarLocale = 'ar';
    //   this.resourceLabel['drivername'] = this.common.arabic.words.drivername;
    //   this.buttonTextOptions['month'] = this.common.arabic.words.month;
    //   this.buttonTextOptions['day'] = this.common.arabic.words.day;
    //   this.allDayText = this.common.arabic.words['allday'];

    // }
  }

  print(template: TemplateRef<any>) {
    this.inProgress = true;
    this._VchlMgmtService.PreviewCalender('VehicleDriver/Preview',this.calendarViewDate).subscribe(res => {
      if (res) {
        this._VchlMgmtService.pdfToJson('PreviewCalender-' + this.Datepipe.transform(this.calendarViewDate, "dd-MM-yyyy")).subscribe((data: any) => {
          this.pdfSrc = data;
          this.bsModalRef = this.modalService.show(template, { class: 'modal-xl' });
          this.inProgress = false;
          this.common.deleteGeneratedFiles('files/delete', 'PreviewCalender-'+this.Datepipe.transform(this.calendarViewDate, "dd-MM-yyyy")+'.pdf').subscribe(result => {
            console.log(result);
          });
        });
      }
    });
  }

  printPdf(html: ElementRef<any>) {
    debugger
    this.inProgress = false;
    // let isReturnForm = false;
    // if(this.screenStatus == 'return'){
    //   isReturnForm = true;
    // }
    this._VchlMgmtService.PreviewCalender('VehicleDriver/Preview', this.calendarViewDate).subscribe(res => {
      if (res) {
        this.common.printPdf('PreviewCalender-' + this.Datepipe.transform(this.calendarViewDate, "dd-MM-yyyy"));
      }
    });
  }

  downloadPrint() {
    this.inProgress = true;
    // let isReturnForm = false;
    // if(this.screenStatus == 'return'){
    //   isReturnForm = true;
    // }
    this._VchlMgmtService.PreviewCalender('VehicleDriver/Preview', this.calendarViewDate).subscribe(res => {
      if (res) {
        this.common.previewPdf('PreviewCalender-' +this.Datepipe.transform(this.calendarViewDate, "dd-MM-yyyy"))
          .subscribe((data: Blob) => {
            this.inProgress = false;
            var url = window.URL.createObjectURL(data);
            var a = document.createElement('a');
            document.body.appendChild(a);
            a.setAttribute('style', 'display: none');
            a.href = url;
            a.download = 'PreviewCalender-' + this.Datepipe.transform(this.calendarViewDate, "dd-MM-yyyy") + '.pdf';
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
            this.common.deleteGeneratedFiles('files/delete', 'PreviewCalender-' + this.Datepipe.transform(this.calendarViewDate, "dd-MM-yyyy") + '.pdf').subscribe(result => {
              console.log(result);
            });
          });
      }
    });
  }

  closePrintPop() {
    this.inProgress = false;
    this.bsModalRef.hide();
  }
  arabic(word) {
    return this.common.arabic.words[word];
  }
  summadata(info :any ) {
    debugger;
   this.title = "";
          }

eventRenderFn(info) {
 // var tooltip = new Tooltip(info.el, {
 //   title: info.event.extendedProps,
 //   placement: 'top',
 //   trigger: 'hover'
 // });
 debugger;
 //info.event.element.querySelectorAll(".fc-title fc-sticky").setAttribute("data-tooltip", info.event.title);
this.title = info.event.title;
this
 this.isHidden=true;   
// }
// eventMouseover(calEvent){
//   var tooltip ='<div class="tooltipevent" style="width:100px;height:100px;background:#ccc;position:absolute;z-index:10001;">'+ calEvent.title +'</div>';var $tooltip = $(tooltip).appendTo('body');
//     $(this).mouseover(function(e){
//         $(this).css('z-index',10000);
//         $tooltip.fadeIn('500');
//         $tooltip.fadeTo('10',1.9);}).mousemove(function(e){
//         $tooltip.css('top', e.pageY +10);
//         $tooltip.css('left', e.pageX +20);});}

// eventMouseout(calEvent){
//     $(this).css('z-index',8);
//     $('.tooltipevent').remove();}

}
// eventrender(event, element)
// {
//   debugger
//    event.element[0].querySelectorAll(".fc-timeline-event fc-h-event fc-event fc-not-start fc-end")[0].setAttribute("data-tooltip", event.event.title);
// }

}
