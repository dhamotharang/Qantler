import {Component, OnInit} from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import {NgxSpinnerService} from 'ngx-spinner';
import {WCMService} from '../../services/wcm.service';
import {GeneralService} from '../../services/general.service';
import {ActivatedRoute, Router} from '@angular/router';
import {EncService} from '../../services/enc.service';
import {DatePipe} from '@angular/common';
import {ResourceService} from '../../services/resource.service';
import {MessageService} from 'primeng/api';
import {Title} from '@angular/platform-browser';
import {ProfileService} from '../../services/profile.service';
import {TranslateService} from '@ngx-translate/core';
import { Page } from './PageContent';

declare var jQuery: any;
declare var $: any;


@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html'
})
export class LandingPageComponent implements OnInit {
  loggedIn: boolean;
  showSearchBox: boolean;
  initData: any;
  StorageUrl: string;
  advSearch: boolean;
  query: string;
  categories: any;
  subCategories: any;
  educationalStandards: any;
  educationalUses: any;
  levels: any;
  copyrights: any;
  selectedCategories: any;
  selectedSubCategories: any;
  selectedEducationalStandards: any;
  selectedEducationalUses: any;
  selectedLevels: any;
  selectedCopyrights: any;
  filteredSubCategories: any;
  advancedQuery: string;
  homePageContent:any = [];
  banner:any=new Page();
  courseCarousel:any=new Page();
  resourceCarousel:any=new Page();
  glanceDescription:any=new Page();
  featuresDescription:any=new Page();
  videoDescription:any=new Page();

  constructor(private titleService: Title, protected keycloakAngular: KeycloakService, private profileSerice: ProfileService, private translate: TranslateService, private messageService: MessageService, private router: Router, private resourceService: ResourceService, private datePipe: DatePipe, private spinner: NgxSpinnerService, private generalService: GeneralService, public encService: EncService,private WcmService: WCMService,) {
     }

  ngOnInit() {
    this.titleService.setTitle('UAE - Open Educational Resources');
    this.loggedIn = false;
    this.initData = null;
    this.showSearchBox = false;
    this.advSearch = false;
    this.query = '';
    this.advancedQuery = '';
    this.keycloakAngular.isLoggedIn().then((res) => {
      this.loggedIn = res;
    });
    this.categories = [];
    this.subCategories = [];
    this.educationalStandards = [];
    this.filteredSubCategories = [];
    this.educationalUses = [];
    this.levels = [];
    this.copyrights = [];
    this.selectedCategories = '';
    this.selectedSubCategories = '';
    this.selectedEducationalStandards = '';
    this.selectedEducationalUses = '';
    this.selectedLevels = '';
    this.selectedCopyrights = '';
    this.getData();
   this.getHomeContentData();
    
    
    window.scrollTo(0, 0);
    $(document).ready(function () {
      $('#create-slider').slick({
        autoplay: true,
        arrows: false,
        draggable: true
      });

      $('.create-slider-controls .slick-next').click(function () {
        $('#create-slider').slick('slickNext');
      });
      $('.create-slider-controls .slick-prev').click(function () {
        $('#create-slider').slick('slickPrev');
      });
    });
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

  getMasterData() {
    this.resourceService.getResourceMasterData().subscribe((res) => {
      if (res.hasSucceeded) {
        this.categories = res.returnedObject.categoryMasterData;
        this.subCategories = res.returnedObject.subCategoryMasterData;
        this.educationalStandards = res.returnedObject.educationalStandard;
        this.educationalUses = res.returnedObject.educationalUse;
        this.levels = res.returnedObject.level;
        this.copyrights = res.returnedObject.copyrightMasterData;
      }
    });
  }

  handleCategoryChange(id) {
    this.filteredSubCategories = this.subCategories.filter(x => x.categoryId.toString() === id.toString());
  }

  sortArray(array, string) {
    return array.sort((n1, n2) => {
      if (n1[string] > n2[string]) {
        return 1;
      }

      if (n1[string] < n2[string]) {
        return -1;
      }

      return 0;
    });
  }

  getData() {
    this.spinner.show(undefined, {color: this.profileSerice.themeColor});
    this.generalService.getLandingPageData().subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.initData = res.returnedObject;
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
    });
    this.getMasterData();
  }


  getHomeContentData() {
    this.WcmService.getAllPageContents().subscribe((res: any) => {
      this.homePageContent = res.returnedObject;
      this.banner = this.homePageContent.filter(text => text.webPage === 'Banner Section')[0];
      this.courseCarousel = this.homePageContent.filter(text => text.webPage === 'Course Carousel')[0];
      this.resourceCarousel = this.homePageContent.filter(text => text.webPage === 'Resource Carousel')[0];
      this.glanceDescription = this.homePageContent.filter(text => text.webPage === 'Glance Description')[0];
      this.videoDescription = this.homePageContent.filter(text => text.webPage === 'Video Section')[0];
      this.featuresDescription = this.homePageContent.filter(text => text.webPage === 'Features Description')[0];
      console.log(this.videoDescription); 
    });
  }

  signUp() {
    this.keycloakAngular.register();
  }

  getContributorCount() {
    if (this.initData) {
      return this.initData.contributors;
    } else {
      return 0;
    }
  }

  getCoursesCount() {
    if (this.initData) {
      return this.initData.courses;
    } else {
      return 0;
    }
  }

  getResourcesCount() {
    if (this.initData) {
      return this.initData.resources;
    } else {
      return 0;
    }
  }

  getVisitorsCount() {
    if (this.initData) {
      return this.initData.totalVisit;
    } else {
      return 0;
    }
  }

  showAdvSearch() {
    this.advSearch = true;
  }

  search() {
    if (this.query && this.query.trim().length > 0) {
      this.router.navigate(['/search', {q: this.query}]);
    } else {
      this.translate.get('Please enter a valid query').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  advanceSearch() {
    // if (this.advancedQuery && this.advancedQuery.trim().length > 0) {
      const filter = {
        'ca': this.selectedCategories ? [this.selectedCategories] : [],
        'sc': this.selectedSubCategories ? [this.selectedSubCategories] : [],
        'es': this.selectedEducationalStandards ? [this.selectedEducationalStandards] : [],
        'eu': this.selectedEducationalUses ? [this.selectedEducationalUses] : [],
        'le': this.selectedLevels ? [this.selectedLevels] : [],
        'mt': [],
        'co': this.selectedCopyrights ? [this.selectedCopyrights] : [],
        'pr': [],
        'ed': [],
        'p': 0,
        's': ''
      };
      this.router.navigate(['/search', {q: this.advancedQuery, f: JSON.stringify(filter)}]);
    // } else {
    //   this.translate.get('Please enter a valid query').subscribe((msg) => {
    //     this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
    //   });
    // }
  }

  clearAdvancedSearch() {
    this.advSearch = false;
    this.selectedCategories = '';
    this.selectedSubCategories = '';
    this.selectedEducationalStandards = '';
    this.selectedEducationalUses = '';
    this.selectedLevels = '';
    this.selectedCopyrights = '';
    this.advancedQuery = '';
  }
}
