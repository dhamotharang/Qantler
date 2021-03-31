import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { CommonService } from '../../../../common.service';
import { TaskEvent } from 'src/app/task/service/task.event';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { CommonModalComponent } from '../../../../modal/commonmodal/commonmodal.component';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-page-body-rtl',
  templateUrl: './page-body.component.rtl.html',
  styleUrls: ['./page-body.component.rtl.scss']
})
export class PageBodyComponentRTL implements OnInit {
  // ItHomeUrl = "/app/it/home";
  bsModalRef: BsModalRef;
  pageInfo: any = {
    title: ''
  };
  buttonName = '';
  menuName: string;
  showTopLabel: boolean = false;
  showCreateTopBanner: boolean = false;
  btn_url: string = '';
  showBannerCalendar: boolean = false;
  currentUser = JSON.parse(localStorage.getItem('User'));
  searchForm: any= [];
  showSideNav = true;

  constructor(private route: ActivatedRoute, public taskEvent: TaskEvent, public router: Router, public commonservice: CommonService,
    private modalService: BsModalService, private _formBuilder:FormBuilder,) {
    // this.commonservice.actionChanged$.subscribe(data => {
    //   this.title = data;
    // });
    // this.commonservice.actionLetterChanged$.subscribe(data => {
    //   this.title = data;
    // });
    this.searchForm = this._formBuilder.group({
      smartSearch: [''],
    });
  }

  ngOnInit() {
    this.route.url.subscribe(() => {
      if(this.route.snapshot.firstChild && this.route.snapshot.firstChild.data.title === "photogallery") {
        this.showSideNav = false;
      } else {
        this.showSideNav = true;
      }
    });
    this.commonservice.showTopBanner$.subscribe(data => {
      this.showTopBanner(data);
    });
    this.commonservice.showTopBannerSearch$.subscribe(data => {
      this.showTopSearch(data);
    });
  }

  showTopSearch(data) {
    this.showTopLabel = false;
    this.showBannerCalendar = data.isShow;
    this.buttonName = (data.btn_name) ? data.btn_name : '';
    this.btn_url = (data.url) ? data.url : '';
    this.pageInfo.title = (data.title) ? data.title : '';
  }

  showTopBanner(data) {
    this.showTopLabel = data.isShow;
    this.showBannerCalendar = false;
    this.buttonName = (data.btn_name) ? data.btn_name : '';
    this.btn_url = (data.url) ? data.url : '';
    this.pageInfo.title = (data.title) ? data.title : '';
  }
  // async changePage(data) {
  //   if (typeof (data) == "object") {
  //     this.pageInfo = data;
  //     switch (data.type) {
  //       case 'memo':
  //         this.showTopLabel = true;
  //         this.buttonName = "+ CREATE MEMO";
  //         this.btn_url = data.type+'/memo-create';
  //         await this.pageList(data);
  //         break;
  //       case 'letter':
  //         this.buttonName = "";
  //         this.showTopLabel = true;
  //         let user = localStorage.getItem("User");
  //         let userdet = JSON.parse(user);
  //         let OrgUnitID = userdet.OrgUnitID;
  //         // if (data.title == 'Incoming Letters') {

  //         //   if(OrgUnitID==14){
  //         //     this.btn_url = data.type+'/incomingletter-create';
  //         //   //this.buttonName = "+ CREATE LETTER";
  //         // }
  //         // } else
  //         // if (data.title == 'Outgoing Letters') {
  //           this.btn_url = data.type+'/outgoingletter-create';
  //           this.buttonName = "+ CREATE LETTER";
  //           if(OrgUnitID==14){
  //             this.btn_url = data.type+'/incomingletter-create';
  //           }
  //        // }
  //         await this.pageList(data);
  //         break;
  //       case 'task':
  //         this.showTopLabel = true;
  //         if (data.menu == 'task-create')
  //           this.showTopLabel = false;
  //         this.btn_url = data.type+'/task-create';
  //         this.buttonName = "+ CREATE TASK";
  //         await this.pageList(data);
  //         break;
  //       case 'circular':
  //         this.showTopLabel = true;
  //         this.btn_url = data.type+'/circular-create';
  //         this.buttonName = "+ CREATE CIRCULAR";
  //         await this.pageList(data);
  //     }
  //   } else {
  //     if (data == 'Salary Certificate') {
  //       this.menuName = 'salary-certificate';
  //     }
  //     if (data == 'Experience Certificate') {
  //       this.menuName = 'experience-certificate';
  //     }
  //     if (data == 'Leave Request') {
  //       this.menuName = 'leave-request';
  //     }
  //     this.routePath(data);
  //   }
  // }

  // routePath(data) {
  //   if (data) {
  //     this.router.navigate([this.menuName + '-list'], { relativeTo: this.route })
  //     //this.router.navigate(['task-create'], { relativeTo: this.route });
  //   } else {
  //     this.router.navigate([this.menuName + '-create'], { relativeTo: this.route })
  //   }
  // }

  createForm() {
    if (this.btn_url == 'citizen-affair-create') {
      //this.btn_url = 'commonmodal';
      this.bsModalRef = this.modalService.show(CommonModalComponent);
      var data= [
        { 'name': this.arabic('citizenaffair'), 'img': 'assets/citizen-affair/citizen-affair.png', 'url': 'citizen-affair-create', 'pagename': 'citizen-affair', 'lang': 'ar' },
        { 'name':  this.arabic('complaintssuggestions'), 'img': 'assets/citizen-affair/complaints.png', 'url': 'complaint-suggestion', 'pagename': 'citizen-affair', 'lang': 'ar' }
      ];
      this.bsModalRef.content.data = data;
      return;
    }
    if (this.btn_url == 'media-request-list') {
      //this.btn_url = 'commonmodal';
      this.bsModalRef = this.modalService.show(CommonModalComponent);
      let data = [
        { 'name': 'Request For Photo', 'img': 'assets/media/image.png', 'url': 'media-request-photo', 'pagename': 'media', 'lang': 'en' },
        { 'name': 'Request For Design', 'img': 'assets/media/sketch.png', 'url': 'media-request-design-form-creation', 'pagename': 'media', 'lang': 'en' },
        { 'name': 'Request For Press Release', 'img': 'assets/media/newspaper-folded.png', 'url': 'media-press-release/request', 'pagename': 'media', 'lang': 'en' },
        { 'name': 'Request For Campaign', 'img': 'assets/media/professional-condenser-microphone-outline.png', 'url': 'campaign/request', 'pagename': 'media', 'lang': 'en' },
        { 'name': 'Request For Photographer', 'img': 'assets/media/photo-camera.png', 'url': 'photographer/create', 'pagename': 'media', 'lang': 'ar' },
        { 'name': 'Request For Diwan Identity', 'img': 'assets/media/name.png', 'url': 'diwan-identity/request-create', 'pagename': 'media', 'lang': 'en' }
      ];

      if(this.commonservice.currentLang == 'ar'){
        data = [
          { 'name': this.arabic(('Request For Photo').replace(/ +/g, "").toLowerCase()), 'img': 'assets/media/image.png', 'url': 'media-request-photo', 'pagename': 'media', 'lang': 'ar' },
          { 'name': this.arabic(('Request For Design').replace(/ +/g, "").toLowerCase()), 'img': 'assets/media/sketch.png', 'url': 'media-request-design-form-creation', 'pagename': 'media', 'lang': 'ar' },
          { 'name': this.arabic(('Request For Press Release').replace(/ +/g, "").toLowerCase()), 'img': 'assets/media/newspaper-folded.png', 'url': 'media-press-release/request', 'pagename': 'media', 'lang': 'ar' },
          { 'name': this.arabic(('Request For Campaign').replace(/ +/g, "").toLowerCase()), 'img': 'assets/media/professional-condenser-microphone-outline.png', 'url': 'campaign/request', 'pagename': 'media', 'lang': 'ar' },
          { 'name': this.arabic(('Request For Photographer').replace(/ +/g, "").toLowerCase()), 'img': 'assets/media/photo-camera.png', 'url': 'photographer/create', 'pagename': 'media', 'lang': 'ar' },
          { 'name': this.arabic(('Request For Diwan Identity').replace(/ +/g, "").toLowerCase()), 'img': 'assets/media/name.png', 'url': 'diwan-identity/request-create', 'pagename': 'media', 'lang': 'ar' }
        ];
      }
      this.bsModalRef.content.data = data;
      return;
    }
    if (this.btn_url == 'hr/dashboard') {
      this.bsModalRef = this.modalService.show(CommonModalComponent);
      let hrRequestsList = [
        {
          'img': 'assets/hr-dashboard/file.png',
          name: 'Leave Requests',
          url: 'leave/request-create',
          pagename: 'hr', 'lang': 'en',
          isAllowedAccess:true
        },
        {
          'img': 'assets/hr-dashboard/agreement.png',
          name: 'Salary Certificate',
          url: 'salary-certificate/create',
          pagename: 'hr', 'lang': 'en',
          isAllowedAccess:true
        },
        {
          'img': 'assets/hr-dashboard/concept.png',
          name: 'Experience Certificate',
          url: 'experience-certificate/create',
          pagename: 'hr', 'lang': 'en',
          isAllowedAccess:true
        },
        {
          'img': 'assets/hr-dashboard/notes.png',
          name: 'New Baby Addition',
          url: 'new-baby-addition/request-create',
          pagename: 'hr', 'lang': 'en',
          isAllowedAccess:true
        },
        {
          'img': 'assets/hr-dashboard/layout.png',
          name: 'Announcement Requests',
          url: 'announcement/announcement-create',
          pagename: 'hr', 'lang': 'en',
          isAllowedAccess:true
        },
        {
          'img': 'assets/hr-dashboard/concept-1.png',
          name: 'Training Requests',
          url: 'training-request/request-create',
          pagename: 'hr', 'lang': 'en',
          isAllowedAccess:true
        },
        {
          'img': 'assets/hr-dashboard/file.png',
          name: 'Official Requests',
          url: 'official-tasks/request-create',
          pagename: 'hr', 'lang': 'en',
          isAllowedAccess:!!this.currentUser.CanRaiseOfficalRequest
        },
        {
          'img': 'assets/hr-dashboard/document.png',
          name: 'Raise Complaints/Suggestions',
          url: 'raise-complaint-suggestion',
          pagename: 'hr', 'lang': 'en',
          isAllowedAccess:true
        }
      ];

      if(this.commonservice.currentLang == 'ar'){
        hrRequestsList = [
          {
            'img': 'assets/hr-dashboard/file.png',
            name: this.arabic('leaverequestspopuptitle'),
            url: 'leave/request-create',
            pagename: 'hr', 'lang': 'ar',
            isAllowedAccess:true
          },
          {
            'img': 'assets/hr-dashboard/agreement.png',
            name: this.arabic('salarycertificaterequestpopuptitle'),
            url: 'salary-certificate/create',
            pagename: 'hr', 'lang': 'ar',
            isAllowedAccess:true
          },
          {
            'img': 'assets/hr-dashboard/concept.png',
            name: this.arabic('experiencecertificatepopuptitle'),
            url: 'experience-certificate/create',
            pagename: 'hr', 'lang': 'ar',
            isAllowedAccess:true
          },
          {
            'img': 'assets/hr-dashboard/notes.png',
            name: this.arabic('newbabyadditionpopuptitle'),
            url: 'new-baby-addition/request-create',
            pagename: 'hr', 'lang': 'ar',
            isAllowedAccess:true
          },
          {
            'img': 'assets/hr-dashboard/layout.png',
            name: this.arabic('announcementrequestspopuptitle'),
            url: 'announcement/announcement-create',
            pagename: 'hr', 'lang': 'ar',
            isAllowedAccess:true
          },
          {
            'img': 'assets/hr-dashboard/concept-1.png',
            name: this.arabic('trainingrequest'),
            url: 'training-request/request-create',
            pagename: 'hr', 'lang': 'ar',
            isAllowedAccess:true
          },
          {
            'img': 'assets/hr-dashboard/file.png',
            name: this.arabic('officialrequests'),
            url: 'official-tasks/request-create',
            pagename: 'hr', 'lang': 'ar',
            isAllowedAccess:!!this.currentUser.CanRaiseOfficalRequest
          },
          {
            'img': 'assets/hr-dashboard/document.png',
            name: this.arabic('raisecomplaintssuggestionspopuptitle'),
            url: 'raise-complaint-suggestion',
            pagename: 'hr', 'lang': 'ar',
            isAllowedAccess:true
          }
        ];
      }


      let data = [];
      hrRequestsList.forEach((hrReqObj) => {
        if(hrReqObj.isAllowedAccess){
          data.push(hrReqObj);
        }
      });
      this.bsModalRef.content.data = data;
      return;
    }
    
    if (this.btn_url == 'rent-car-create') {
      this.commonservice.createCarPopup.next();
      return;
    }
    var param = {
      pageInfo: this.pageInfo,
      btn: this.btn_url,
      type: 'create'
    };
    this.commonservice.action(param);
    this.showTopLabel =  false;
    this.router.navigate([this.btn_url], { relativeTo: this.route });
  }

  filterCalendarEvent() {
    this.commonservice.setSearchByEvent(this.searchForm.value.smartSearch);
  }

  arabic(word) {
    return this.commonservice.arabic.words[word];
  }

  // async pageList(data) {
  //   var param = {
  //     pageInfo:this.pageInfo,
  //     btn:this.btn_url,
  //     type:'list'
  //   };
  //   await this.router.navigate([data.menu], { relativeTo: this.route });
  //   await this.commonservice.action(param);
  //   await this.commonservice.sideNavClick(param)
  // }

}
