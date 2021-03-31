import {ChangeDetectorRef, Component, OnDestroy, OnInit} from '@angular/core';
import {ResourceService} from '../../services/resource.service';
import {ProfileService} from '../../services/profile.service';
import {ActivatedRoute, ParamMap, Router} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {CourseService} from '../../services/course.service';
import {environment} from '../../../environments/environment';
import {ElasticSearchService} from '../../services/elastic-search.service';
import {Subscription} from 'rxjs';
import {EncService} from '../../services/enc.service';
import {switchMap} from 'rxjs-compat/operator/switchMap';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html'
})
export class CoursesComponent implements OnInit, OnDestroy {
  courses: any;
  Allcourses: any;
  categories: any;
  subCategories: any;
  educationalStandards: any;
  educationalUses: any;
  levels: any;
  materialTypes: any;
  copyrights: any;
  educations: any;
  professions: any;
  selectedCategories: any;
  selectedEducationalStandard: any;
  selectedEducationalUse: any;
  selectedLevel: any;
  selectedMaterialType: any;
  selectedCopyright: any;
  selectedEducations: any;
  selectedProfessions: any;
  showReportAbuse: any[];
  userId: any;
  queryString: string;
  querySubCategories: any[];
  selectedSubCategories: any[];
  querySortBy: any;
  showQueryErrorMsg: boolean;
  private sub: Subscription;
  pageNumber: number;
  pageSize: number;
  totalResultCount: number;
  searchFlag: boolean;
  pasred: boolean;
  readmore: any;
  status: any;
  pageStart: number;
  isConnected: any;

  constructor(private titleService: Title,
              public encService: EncService,
              private route: ActivatedRoute,
              private cd: ChangeDetectorRef,
              private courseService: CourseService,
              private elasticSearchService: ElasticSearchService,
              private resourceService: ResourceService,
              private profileService: ProfileService,
              private translate: TranslateService,
              public router: Router,
              private spinner: NgxSpinnerService,
              private messageService: MessageService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Search Courses | UAE - Open Educational Resources');
    this.categories = [];
    this.subCategories = [];
    this.educationalStandards = [];
    this.educationalUses = [];
    this.levels = [];
    this.materialTypes = [];
    this.copyrights = [];
    this.educations = [];
    this.professions = [];
    this.userId = this.profileService.userId;
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.userId = this.profileService.userId;
    });
    this.isConnected = false;
    this.pasred = false;
    this.initial();
    this.getCategories();
    $(document).ready(function () {
      $('.ref-filt-head-icon').click(function () {
        $('.filter-accordion').stop().slideToggle();
      });
    });
    this.route.params.subscribe((res) => {
      if (res.f || res.q) {
        if (res.f) {
          this.selectedCategories = JSON.parse(res.f).ca;
          this.selectedCopyright = JSON.parse(res.f).co;
          this.selectedEducationalStandard = JSON.parse(res.f).es;
          this.selectedEducationalUse = JSON.parse(res.f).eu;
          this.selectedLevel = JSON.parse(res.f).le;
          this.selectedSubCategories = JSON.parse(res.f).sc;
          this.selectedEducations = JSON.parse(res.f).ed;
          this.selectedProfessions = JSON.parse(res.f).pr;
          this.pageStart = JSON.parse(res.f).p;
          this.querySortBy = JSON.parse(res.f).s;
        }
        if (res.q) {
          this.queryString = res.q;
        }
        if (this.queryString.length > 0) {
          this.getSearchResults();
        } else {
          this.getCourses();
        }
      } else {
        this.selectedCategories = [];
        this.selectedCopyright = [];
        this.selectedEducationalStandard = [];
        this.selectedEducationalUse = [];
        this.selectedLevel = [];
        this.selectedSubCategories = [];
        this.selectedEducations = [];
        this.selectedProfessions = [];
        this.pageStart = 0;
        this.querySortBy = '';
        this.queryString = '';
        this.getCourses();
      }
    });
  }

  initial() {
    this.selectedSubCategories = [];
    this.selectedCategories = [];
    this.showReportAbuse = [];
    this.searchFlag = false;
    this.courses = [];
    this.readmore = [];
    this.pageStart = 0;
    this.Allcourses = [];
    this.selectedEducationalStandard = [];
    this.selectedEducationalUse = [];
    this.selectedLevel = [];
    this.selectedMaterialType = [];
    this.selectedEducations = [];
    this.selectedCopyright = [];
    this.selectedProfessions = [];
    this.queryString = '';
    this.querySortBy = 'createdOn';
    this.querySubCategories = [];
    this.showQueryErrorMsg = false;
    this.pageNumber = 1;
    this.pageSize = 10;
    this.totalResultCount = 0;
    // this.elasticSearchService.isAvailable().then(() => {
    //   this.status = 'OK';
    //   this.isConnected = true;
    // }, error => {
    //   this.status = 'ERROR';
    //   this.isConnected = false;
    //   console.error('Server is down', error);
    // }).then(() => {
    //   this.cd.detectChanges();
    // });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  filterCategory(category) {
    this.selectedCategories = [];
    this.selectedSubCategories = [];
    this.selectedEducationalStandard = [];
    this.selectedEducationalUse = [];
    this.selectedLevel = [];
    this.selectedEducations = [];
    this.selectedProfessions = [];
    this.selectedCopyright = [];
    this.queryString = category;
    const filter = {
      'ca': this.selectedCategories ? this.selectedCategories : [],
      'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
      'es': this.selectedEducationalStandard ? this.selectedEducationalStandard : [],
      'eu': this.selectedEducationalUse ? this.selectedEducationalUse : [],
      'le': this.selectedLevel ? this.selectedLevel : [],
      'mt': [],
      'co': this.selectedCopyright ? this.selectedCopyright : [],
      'pr': this.selectedProfessions ? this.selectedProfessions : [],
      'ed': this.selectedEducations ? this.selectedEducations : [],
      'p': 0,
      's': ''
    };
    this.router.navigate(['/courses', {q: this.queryString, f: JSON.stringify(filter)}]);
  }

  getCourses() {
    const filter = {
      'categories': this.makeInt(this.selectedCategories),
      'subCategories': this.makeInt(this.selectedSubCategories),
      'educationalStandard': this.makeInt(this.selectedEducationalStandard),
      'educationalUse': this.makeInt(this.selectedEducationalUse),
      'level': this.makeInt(this.selectedLevel),
      'materialType': [],
      'educations': this.makeInt(this.selectedEducations),
      'professions': this.makeInt(this.selectedProfessions),
      'copyright': this.makeInt(this.selectedCopyright)
    };
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.elasticSearchService.getAllDocuments('courses', 'course', this.pageSize, this.pageStart, this.querySortBy, filter).subscribe(
      (response) => {
        if (response.hasSucceeded) {
          this.pasred = false;
          this.courses = [];
          this.readmore = [];
          this.showReportAbuse = [];
          if (response.returnedObject.hits && response.returnedObject.hits.hits.length > 0) {
            response.returnedObject.hits.hits.forEach((item) => {
              this.courses.push(item._source);
              this.showReportAbuse.push(false);
              this.readmore.push(false);
            });
            if (response.returnedObject.hits.total.value) {
              this.totalResultCount = response.returnedObject.hits.total.value;
            } else {
              this.totalResultCount = response.returnedObject.hits.total;
            }
            this.getRatings(this.courses);
          } else {
            this.courses = [];
            this.Allcourses = [];
            this.totalResultCount = 0;
            this.translate.get('No Results Found').subscribe((msg) => {
              this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
            });
          }
          this.Allcourses = this.courses;
          this.searchFlag = false;
        } else {
          this.Allcourses = [];
          this.courses = [];
          this.totalResultCount = 0;
          this.translate.get('Failed to retrieve results').subscribe((msg) => {
            this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.courses = [];
        this.Allcourses = [];
        this.totalResultCount = 0;
        this.translate.get('Failed to retrieve results').subscribe((msg) => {
          this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
        });
        this.spinner.hide();
      });
  }

  getRatings(data) {
    const idArray = [];
    data.forEach((item) => {
      idArray.push({
        contentId: item.id,
        contentType: 1
      });
    });
    this.elasticSearchService.getRatings(idArray).subscribe((res: any) => {
      const list = res.returnedObject;
      const courses = this.courses;
      list.forEach((item) => {
        courses.forEach((course) => {
          if (item.contentId === course.id) {
            course.rating = item.rating;
            course.allratings = item.allRatings;
          }
        });
      });
      this.pasred = true;
      this.courses = courses;
      this.Allcourses = this.courses;
    });
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

  getCategories() {
    this.resourceService.getResourceMasterData().subscribe((res) => {
      if (res.hasSucceeded) {
        this.categories = res.returnedObject.categoryMasterData;
        this.subCategories = res.returnedObject.subCategoryMasterData;
        this.educationalStandards = res.returnedObject.educationalStandard;
        this.educationalUses = res.returnedObject.educationalUse;
        this.levels = res.returnedObject.level;
        this.materialTypes = res.returnedObject.materialTypeMasterData;
        this.copyrights = res.returnedObject.copyrightMasterData;
        this.educations = res.returnedObject.educationMasterData;
        this.professions = res.returnedObject.professionMasterData;
      }
    });
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

  getCategory(id) {
    if (this.categories && this.categories.length > 0) {
      return this.categories.find(x => x.id === id) ? this.categories.find(x => x.id === id).name : ' ';
    }
  }

  getCourseLength() {
    const array = this.courses.filter(x => x.isDraft === false);
    return array.filter(x => x.isApproved === true).length;
  }

  rateCourse(event, item) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.courseService.rateCourse({
      'courseId': item.id,
      'rating': event,
      'comments': '',
      'ratedBy': this.userId
    }).subscribe((res) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully rated Course').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
      this.getCourses();
    }, (error) => {
      this.translate.get('Failed to rate Course').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
      this.getCourses();
    });
  }

  search() {
    if (this.queryString.trim().length > 0) {
      this.pageStart = 0;
      this.pageNumber = 1;
      const filter = {
        'ca': this.selectedCategories ? this.selectedCategories : [],
        'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
        'es': this.selectedEducationalStandard ? this.selectedEducationalStandard : [],
        'eu': this.selectedEducationalUse ? this.selectedEducationalUse : [],
        'le': this.selectedLevel ? this.selectedLevel : [],
        'mt': [],
        'co': this.selectedCopyright ? this.selectedCopyright : [],
        'pr': this.selectedProfessions ? this.selectedProfessions : [],
        'ed': this.selectedEducations ? this.selectedEducations : [],
        'p': this.pageStart,
        's': this.querySortBy
      };
      this.router.navigate(['/courses', {q: this.queryString, f: JSON.stringify(filter)}]);
    } else {
      this.showQueryErrorMsg = true;
      setTimeout(() => {
        this.showQueryErrorMsg = false;
      }, 5000);
      // this.getCourses();
    }
  }

  makeInt(array) {
    const result = [];
    array.forEach((item) => {
      result.push(+item);
    });
    return result;
  }

  getSearchResults() {
    const filter = {
      'categories': this.makeInt(this.selectedCategories),
      'subCategories': this.makeInt(this.selectedSubCategories),
      'educationalStandard': this.makeInt(this.selectedEducationalStandard),
      'educationalUse': this.makeInt(this.selectedEducationalUse),
      'level': this.makeInt(this.selectedLevel),
      'materialType': [],
      'educations': this.makeInt(this.selectedEducations),
      'professions': this.makeInt(this.selectedProfessions),
      'copyright': this.makeInt(this.selectedCopyright)
    };
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.elasticSearchService.searchItem('courses', 'course', this.pageSize, this.pageStart, this.queryString, this.querySortBy, filter).subscribe(
      (response) => {
        if (response.hasSucceeded) {
          this.pasred = false;
          this.courses = [];
          this.readmore = [];
          this.showReportAbuse = [];
          if (response.returnedObject.hits && response.returnedObject.hits.hits.length > 0) {
            response.returnedObject.hits.hits.forEach((item) => {
              this.courses.push(item._source);
              this.readmore.push(false);
              this.showReportAbuse.push(false);
            });
            if (response.returnedObject.hits.total.value) {
              this.totalResultCount = response.returnedObject.hits.total.value;
            } else {
              this.totalResultCount = response.returnedObject.hits.total;
            }
            this.getRatings(this.courses);
          } else {
            this.Allcourses = [];
            this.courses = [];
            this.totalResultCount = 0;
            this.translate.get('No Results Found').subscribe((msg) => {
              this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
            });
          }
          this.Allcourses = this.courses;
          this.searchFlag = true;
        } else {
          this.Allcourses = [];
          this.courses = [];
          this.totalResultCount = 0;
          this.translate.get('Failed to retrieve results').subscribe((msg) => {
            this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.Allcourses = [];
        this.courses = [];
        this.totalResultCount = 0;
        this.translate.get('Failed to retrieve results').subscribe((msg) => {
          this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
        });
        this.spinner.hide();
      });
  }

  sort() {
    const filter = {
      'ca': this.selectedCategories ? this.selectedCategories : [],
      'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
      'es': this.selectedEducationalStandard ? this.selectedEducationalStandard : [],
      'eu': this.selectedEducationalUse ? this.selectedEducationalUse : [],
      'le': this.selectedLevel ? this.selectedLevel : [],
      'mt': [],
      'co': this.selectedCopyright ? this.selectedCopyright : [],
      'pr': this.selectedProfessions ? this.selectedProfessions : [],
      'ed': this.selectedEducations ? this.selectedEducations : [],
      'p': this.pageStart,
      's': this.querySortBy
    };
    this.router.navigate(['/courses', {q: this.queryString, f: JSON.stringify(filter)}]);
  }

  fliter() {
    this.pageStart = 0;
    this.pageNumber = 1;
    const filter = {
      'ca': this.selectedCategories ? this.selectedCategories : [],
      'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
      'es': this.selectedEducationalStandard ? this.selectedEducationalStandard : [],
      'eu': this.selectedEducationalUse ? this.selectedEducationalUse : [],
      'le': this.selectedLevel ? this.selectedLevel : [],
      'mt': [],
      'co': this.selectedCopyright ? this.selectedCopyright : [],
      'pr': this.selectedProfessions ? this.selectedProfessions : [],
      'ed': this.selectedEducations ? this.selectedEducations : [],
      'p': this.pageStart,
      's': this.querySortBy
    };
    this.router.navigate(['/courses', {q: this.queryString, f: JSON.stringify(filter)}]);
  }

  getParsed(string) {
    if (string) {
      const parsed = JSON.parse(string);
      if (parsed.length > 0) {
        const array = [];
        parsed.forEach((item) => {
          array.push({
            'star': item.Star,
            'userCount': item.NoOfUsers
          });
        });
        return array;
      } else {
        return null;
      }
    } else {
      return null;
    }
  }

  pageChange(event) {
    this.pageNumber = event.page + 1;
    this.pageStart = (this.pageSize * event.page);
    const filter = {
      'ca': this.selectedCategories ? this.selectedCategories : [],
      'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
      'es': this.selectedEducationalStandard ? this.selectedEducationalStandard : [],
      'eu': this.selectedEducationalUse ? this.selectedEducationalUse : [],
      'le': this.selectedLevel ? this.selectedLevel : [],
      'mt': [],
      'co': this.selectedCopyright ? this.selectedCopyright : [],
      'pr': this.selectedProfessions ? this.selectedProfessions : [],
      'ed': this.selectedEducations ? this.selectedEducations : [],
      'p': this.pageStart,
      's': this.querySortBy
    };
    this.router.navigate(['/courses', {q: this.queryString, f: JSON.stringify(filter)}]);
  }

  clearSearch() {
    this.router.navigate(['/courses']);
  }

  clickreadmore(i) {
    this.readmore[i] = !this.readmore[i];
  }

  getSubCategoryName(id) {
    if (this.subCategories.length > 0) {
      return this.getCurrentLang() === 'en' ? this.subCategories.find(x => x.id === +id).name : this.subCategories.find(x => x.id === +id).name_Ar;
    } else {
      return '';
    }
  }

  getEducationalStandardName(id) {
    if (this.educationalStandards.length > 0) {
      return this.getCurrentLang() === 'en' ? this.educationalStandards.find(x => x.id === +id).standard : this.educationalStandards.find(x => x.id === +id).standard_Ar;
    } else {
      return '';
    }
  }

  getEducationalUseName(id) {
    if (this.educationalUses.length > 0) {
      return this.getCurrentLang() === 'en' ? this.educationalUses.find(x => x.id === +id).text : this.educationalUses.find(x => x.id === +id).text_Ar;
    } else {
      return '';
    }
  }

  getLevelName(id) {
    if (this.levels.length > 0) {
      return this.getCurrentLang() === 'en' ? this.levels.find(x => x.id === +id).levelText : this.levels.find(x => x.id === +id).levelText_Ar;
    } else {
      return '';
    }
  }

  getEducationName(id) {
    if (this.educations.length > 0) {
      return this.getCurrentLang() === 'en' ? this.educations.find(x => x.id === +id).name : this.educations.find(x => x.id === +id).name_Ar;
    } else {
      return '';
    }
  }

  getProfessionName(id) {
    if (this.professions.length > 0) {
      return this.getCurrentLang() === 'en' ? this.professions.find(x => x.id === +id).name : this.professions.find(x => x.id === +id).name_Ar;
    } else {
      return '';
    }
  }

  getCopyrightName(id) {
    if (this.copyrights.length > 0) {
      return this.getCurrentLang() === 'en' ? this.copyrights.find(x => x.id === +id).title : this.copyrights.find(x => x.id === +id).title_Ar;
    } else {
      return '';
    }
  }

  removeSubCategory(id, index) {
    this.selectedSubCategories.splice(index, 1);
    this.fliter();
    const data = this.subCategories;
    this.subCategories = [];
    setTimeout(() => {
      this.subCategories = data;
    }, 200);
  }

  removeEducationalStandard(id, index) {
    this.selectedEducationalStandard.splice(index, 1);
    this.fliter();
    const data = this.educationalStandards;
    this.educationalStandards = [];
    setTimeout(() => {
      this.educationalStandards = data;
    }, 200);
  }

  removeEducationalUse(id, index) {
    this.selectedEducationalUse.splice(index, 1);
    this.fliter();
    const data = this.educationalUses;
    this.educationalUses = [];
    setTimeout(() => {
      this.educationalUses = data;
    }, 200);
  }

  removeLevel(id, index) {
    this.selectedLevel.splice(index, 1);
    this.fliter();
    const data = this.levels;
    this.levels = [];
    setTimeout(() => {
      this.levels = data;
    }, 200);
  }

  removeEducation(id, index) {
    this.selectedEducations.splice(index, 1);
    this.fliter();
    const data = this.educations;
    this.educations = [];
    setTimeout(() => {
      this.educations = data;
    }, 200);
  }

  removeProfession(id, index) {
    this.selectedProfessions.splice(index, 1);
    this.fliter();
    const data = this.professions;
    this.professions = [];
    setTimeout(() => {
      this.professions = data;
    }, 200);
  }

  removeCopyright(id, index) {
    this.selectedCopyright.splice(index, 1);
    this.fliter();
    const data = this.copyrights;
    this.copyrights = [];
    setTimeout(() => {
      this.copyrights = data;
    }, 200);
  }

}
