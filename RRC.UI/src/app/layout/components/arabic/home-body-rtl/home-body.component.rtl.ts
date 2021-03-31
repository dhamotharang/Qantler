import { Component, OnInit } from '@angular/core';
import { LayoutServiceService } from '../../../layout-service.service';
import { CommonService } from 'src/app/common.service';
import { Router } from '@angular/router';
import * as moment from 'moment-timezone';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ManagePhotoComponent } from 'src/app/manage-photo/component/manage-photo/manage-photo.component';
import { Lightbox } from 'ngx-lightbox';
import { OwlOptions } from 'ngx-owl-carousel-o';
import { ManagePhotoService } from 'src/app/manage-photo/service/manage-photo.service';
import { environment } from 'src/environments/environment';
import $ from 'jquery';

@Component({
  selector: 'app-home-body-rtl',
  templateUrl: './home-body.component.rtl.html',
  styleUrls: ['./home-body.component.rtl.scss']
})
export class HomeBodyComponentRTL implements OnInit {
  type: any = 'photo';
  dutyTaskCount = 0;
  meetingCount = 0;
  memosCount = 0;
  circularCount = 0;
  letterCount = 0;
  galleryCardName = 'Lorem ipsum';
  currentUser: any;
  NextMeetingDateTime: any;
  MeetingID: any;
  nextEventDays: any;
  nextEventhours: any;
  nextMeeting: any;
  bsModalRef: BsModalRef;
  CanManageNews = false;
  public page: number = 1;
  public maxSize: number = 12;
  public itemsPerPage: number = 10;
  totalItems: number;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'modal-lg',
    paging: true,
    // sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };

  carouselOptions: OwlOptions = {
    loop: true,
    mouseDrag: false,
    touchDrag: false,
    pullDrag: false,
    dots: false,
    navSpeed: 500,
    autoplay: true,
    rtl:true,
    margin: 10,
    autoHeight: true,
    autoWidth: true,
    responsiveRefreshRate: 1,
    // responsive: {
    //   0: {
    //     items: 1
    //   },
    //   400: {
    //     items: 2
    //   },
    //   740: {
    //     items: 3
    //   },
    //   940: {
    //     items: 4
    //   }
    // },
    nav: false
  }

  albums: Array<any> = [];
  photoList: any;
  lang: any;
  imageURL = environment.AttachmentDownloadUrl;

  constructor(
    public managePhotoService: ManagePhotoService,
    public service: LayoutServiceService,
    public common: CommonService,
    public router: Router,
    public modalService: BsModalService,
    public lightBox: Lightbox
    ) {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.CanManageNews = this.currentUser.CanManageNews;
    this.service.moduleCount(this.currentUser.id).subscribe((res: any) => {
      this.dutyTaskCount = res.DutyTask;
      this.meetingCount = res.Meeting;
      this.memosCount = res.Memo;
      this.circularCount = res.Circular;
      this.letterCount = res.Letters;
      this.MeetingID = res.MeetingID;
      this.nextMeeting = res.NextMeetingDateTime;
      var NextMeetingDateTime = moment(new Date(res.NextMeetingDateTime));
      this.NextMeetingDateTime = NextMeetingDateTime.format('MMM D, YYYY hh:mm A');
      this.callTimer(NextMeetingDateTime);
    });
    this.loadPhotoList();
  }
  callTimer(NextMeetingDateTime) {
    var interval = 1000;
    // setInterval(function() {
      var eventTime = new Date(NextMeetingDateTime).getTime();
      var currentTime = new Date().getTime()
      var diffTime = eventTime - currentTime;
      var duration = moment.duration(diffTime, 'milliseconds');
      duration = moment.duration(duration - interval, 'milliseconds');
      this.nextEventDays = duration.days();
      this.nextEventhours = duration.hours();
    // }, interval);
  }

  meetingView(MeetingID){
    if(MeetingID){
      this.router.navigate(['/app/meeting/view/' + MeetingID]);
    }else{
      this.router.navigate(['/app/meeting/list']);
    }
  }

  photoType(type) {
    this.type = type;
  }

  showPhotoGallery () {
    this.router.navigate([this.lang + '/app/photogallery']);
  }

  showPhoto() {
    const initialState = {
      reqType: "photo"
    };
    this.bsModalRef = this.modalService.show(ManagePhotoComponent, Object.assign({}, this.config, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.loadPhotoList()
    })
  }

  open(index: number): void {
    this.lightBox.open(this.albums, index);
  }

  close(): void {
    this.lightBox.close();
  }

  loadPhotoList() {
    this.albums = [];
    this.managePhotoService.loadAllPhotos(this.page, this.maxSize).subscribe(
      (resList: any) => {
        this.photoList = resList.Collection;
        // this.totalItems = resList.Count;
        this.photoList.forEach((obj: any) => {
          if (obj.AttachmentGuid != null && obj.AttachmentName != null) {
            this.albums.push(
              {
                src: this.imageURL + '?filename=' + obj.AttachmentName + '&guid=' + obj.AttachmentGuid,
                caption: obj.AttachmentName,
                thumb: this.imageURL + '?filename=' + obj.AttachmentName + '&guid=' + obj.AttachmentGuid
              }
            );
          }
        })
        this.totalItems = this.albums.length;
      }
    );
  }
 

  arabicfn(word) {
    return this.common.arabic.words[word];
  }
  prevNext(value){
    if(value == 'prev')
      $(".owl-prev").click();
    else 
      $(".owl-next").click();
    }
}
