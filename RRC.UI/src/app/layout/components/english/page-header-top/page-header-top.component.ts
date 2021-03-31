import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';
import { PageHeaderComponent } from '../page-header/page-header.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ManageNewsComponent } from 'src/app/manage-news/component/manage-news/manage-news.component';
import { OwlOptions } from 'ngx-owl-carousel-o';
import { ManagenewsService } from 'src/app/manage-news/service/managenews.service';
import { ManagePhotoComponent } from 'src/app/manage-photo/component/manage-photo/manage-photo.component';
import { ManagePhotoService } from 'src/app/manage-photo/service/manage-photo.service';
import { environment } from 'src/environments/environment';
import { EndPointService } from 'src/app/api/endpoint.service';
import { ArabicDataService } from 'src/app/arabic-data.service';

@Component({
  selector: 'app-page-header-top',
  templateUrl: './page-header-top.component.html',
  styleUrls: ['./page-header-top.component.scss']
})
export class PageHeaderTopComponent implements OnInit {
  home_banner: boolean = true;
  down = false;
  downLang = false;
  departments = [
    { name: 'CITIZEN AFFAIR SERVICES', link: '/app/citizen-affair/citizen-affair-list' },
    { name: 'MAINTENANCE SERVICES', link: '/app/maintenance/home' },
    { name: 'LEGAL SERVICES', link: '/app/legal/dashboard' },
    { name: 'H.R SERVICES', link: '/app/hr/dashboard' },
    { name: 'PROTOCOL SERVICES', link: '/app/media/protocol-home-page' },
    { name: 'I.T SERVICES', link: '/app/it/' },
    { name: 'VEHICLE MANAGEMENT', link: '/app/vehicle-management/' }
  ];
  languageList = ['English', this.arabic('arabic')];
  department: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  protocolServicePageLink: string;
  language: any = this.common.language;
  @ViewChild('mobileMenu') mobileMenu: ElementRef;
  bsModalRef: BsModalRef;
  CanManageNews = false;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'modal-lg'
  };
  newsList: any=[]
  
  carouselOptions: OwlOptions = {
    loop: true,
    mouseDrag: false,
    touchDrag: false,
    pullDrag: false,
    dots: false,
    navSpeed: 100,
    autoplay: true,
    rtl: false,
    responsive: {
      0: {
        items: 1
      },
      400: {
        items: 1
      },
      740: {
        items: 1
      },
      940: {
        items: 1
      }
    },
    nav: false
  }
  newsData: any=[];
  bannerData: any=[];
  bgImage: string;

  constructor(private router: Router, route: ActivatedRoute, public common: CommonService, public modalService: BsModalService, public newsService: ManagenewsService, public photoService: ManagePhotoService, public endpoint: EndPointService, public arabicService: ArabicDataService) {
    this.language = this.common.language;
    
    if (this.router.url) {
      this.home_banner = (route.snapshot.data.title === 'home') ? true : false;
      if(this.home_banner) {
        this.getBanner()
      } else {
        this.bgImage = "assets/home/inner-banner.png";
      }
    }
    
  }
  changePage(event) {
    this.mobileMenu.nativeElement.click();
    //this.common.mobileSideClick(event);
  }
  ngOnInit() {
    this.getNews();
    this.CanManageNews = this.currentUser.CanManageNews;
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    if(this.common.currentLang == 'en'){
      this.departments = [
        { name: 'CITIZEN AFFAIR SERVICES', link: '/en/app/citizen-affair/citizen-affair-list' },
        { name: 'MAINTENANCE SERVICES', link: '/en/app/maintenance/home' },
        { name: 'LEGAL SERVICES', link: '/en/app/legal/dashboard' },
        { name: 'H.R SERVICES', link: '/en/app/hr/dashboard' },
        { name: 'PROTOCOL SERVICES', link: '/en/app/media/protocol-home-page' },
        { name: 'I.T SERVICES', link: '/en/app/it/' },
        { name: 'VEHICLE MANAGEMENT', link: '/en/app/vehicle-management/' }
      ];
    }else{
      this.departments = [
        { name: this.arabic('citizenaffairservices'), link: '/ar/app/citizen-affair/citizen-affair-list' },
        { name: this.arabic('maintenanceservices'), link: '/ar/app/maintenance/home' },
        { name: this.arabic('legalservices'), link: '/ar/app/legal/dashboard' },
        { name: this.arabic('hrservices'), link: '/ar/app/hr/dashboard' },
        { name: this.arabic('protocolservices'), link: '/ar/app/media/protocol-home-page' },
        { name: this.arabic('itservices'), link: '/ar/app/it/' },
        { name: this.arabic('vehiclemgmt'), link: '/ar/app/vehicle-management/' }
      ];
    }
    if (this.currentUser.OrgUnitID == 4) {
      // if (this.currentUser.department == "Protocol") {
      // if (this.currentUser.department == "Ruler's Representative Office") {
        if(this.common.currentLang == 'en'){
          this.protocolServicePageLink = '/en/app/media/protocol-dashboard';
          this.departments[4].link = '/en/app/media/protocol-dashboard';
        }else{
          this.protocolServicePageLink = '/ar/app/media/protocol-dashboard';
          this.departments[4].link = '/ar/app/media/protocol-dashboard';
        }
        
      } else {
        if(this.common.currentLang == 'en'){
          this.protocolServicePageLink = '/en/app/media/protocol-home-page';
          this.departments[4].link = '/en/app/media/protocol-home-page';
        }else{
          this.protocolServicePageLink = '/ar/app/media/protocol-home-page';
          this.departments[4].link = '/ar/app/media/protocol-home-page';
        }
      }
      
  }

  departmentSelect(dept) {
    this.department = dept.name;
    this.router.navigate([dept.link]);
  }

  languageChange(language) {
    this.common.setCookie('language', language);
    var urls = this.router.url.split('/'),
      url = '';
    url = this.routingProcess(urls, language);
    this.router.navigate([url]);
    if (language == 'English')
      this.common.languageChange('en');
    else
      this.common.languageChange('ar');
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

  showNews() {
    this.bsModalRef = this.modalService.show(ManageNewsComponent, this.config);
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.getNews()
    })
  }

  showBanner() {
    const initialState = {
      reqType: "banner"
    };
    this.bsModalRef = this.modalService.show(ManagePhotoComponent, Object.assign({}, this.config, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.getBanner()
    })
  }
  
  getNews() {
    this.newsList=[]
    this.newsService.getNews(1, 50, this.currentUser.id,'').subscribe(data => {
      this.newsData = data;
      // this.newsList = this.newsData.collection;
      this.newsData.collection.forEach((obj: any) => {
        if (obj.News != null) {
          this.newsList.push(
            {
              // News: (obj.News.replace(/&nbsp;/g, ' ').replace(/&amp;/g, '&')).replace(/<[^>]*>/g, '')
              News: obj.News
            }
          );
        }
      })
    })
  }

  getBanner() {
    this.bannerData=[];
    this.photoService.getBanner("Banner").subscribe((data:any) => {
      this.bannerData = data;
      // this.newsList = this.newsData.collection;
      if(data) {
        this.bgImage = this.endpoint.fileDownloadUrl+"?filename="+ this.bannerData.AttachmentName + '&guid=' + this.bannerData.AttachmentGuid;
      }
      // console.log("this.bannerData", this.bgImage)
    })
  }
  arabic(word) {
    return this.common.arabic.words[word];
  }
}
