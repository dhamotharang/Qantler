import {ChangeDetectorRef, Component, OnDestroy, OnInit} from '@angular/core';
import {ResourceService} from '../../services/resource.service';
import {ActivatedRoute, Router} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {ProfileService} from '../../services/profile.service';
import {environment} from '../../../environments/environment';
import {ElasticSearchService} from '../../services/elastic-search.service';
import {Subscription} from 'rxjs';
import {EncService} from '../../services/enc.service';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-resources',
  templateUrl: './resources.component.html'
})
export class ResourcesComponent implements OnInit, OnDestroy {
  resources: any;
  Allresources: any;
  educationalStandards: any;
  selectedEducationalStandard: any;
  educationalUses: any;
  selectedEducationalUse: any;
  levels: any;
  selectedLevel: any;
  materialTypes: any;
  selectedMaterialType: any;
  copyrights: any;
  selectedCopyright: any;
  selectedCategories: any;
  categories: any;
  subCategories: any;
  selectedSubCategories: any;
  showReportAbuse: any[];
  userId: any;
  queryString: string;
  querySubCategories: any[];
  querySortBy: any;
  showQueryErrorMsg: boolean;
  parsed: boolean;
  searchFlag: boolean;
  private sub: Subscription;
  pageNumber: number;
  pageStart: number;
  pageSize: number;
  totalResultCount: number;
  status: any;
  isConnected: any;
  readmore: any;

  constructor(private titleService: Title, public encService: EncService, private route: ActivatedRoute, private translate: TranslateService, private cd: ChangeDetectorRef, private elasticSearchService: ElasticSearchService,
              private resourceService: ResourceService, private profileService: ProfileService, public router: Router, private spinner: NgxSpinnerService, private messageService: MessageService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Search Resources | UAE - Open Educational Resources');
    this.categories = [];
    this.subCategories = [];
    this.educationalStandards = [];
    this.educationalUses = [];
    this.levels = [];
    this.materialTypes = [];
    this.copyrights = [];
    this.parsed = false;
    this.userId = this.profileService.userId;
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.userId = this.profileService.userId;
    });
    this.getCategories();
    this.initial();
    $(document).ready(function () {
      $('.ref-filt-head-icon').click(function () {
        $('.filter-accordion').stop().slideToggle();
      });
    });
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
    this.route.params.subscribe((res) => {
      if (res.f || res.q) {
        if (res.f) {
          this.selectedCategories = JSON.parse(res.f).ca;
          this.selectedCopyright = JSON.parse(res.f).co;
          this.selectedEducationalStandard = JSON.parse(res.f).es;
          this.selectedEducationalUse = JSON.parse(res.f).eu;
          this.selectedLevel = JSON.parse(res.f).le;
          this.selectedSubCategories = JSON.parse(res.f).sc;
          this.selectedMaterialType = JSON.parse(res.f).mt;
          this.pageStart = JSON.parse(res.f).p;
          this.querySortBy = JSON.parse(res.f).s;
        }
        if (res.q) {
          this.queryString = res.q;
        }
        if (this.queryString.length > 0) {
          this.getSearchResults();
        } else {
          this.getResources();
        }
      } else {
        this.queryString = '';
        this.selectedCategories = [];
        this.selectedCopyright = [];
        this.selectedEducationalStandard = [];
        this.selectedEducationalUse = [];
        this.selectedLevel = [];
        this.selectedSubCategories = [];
        this.selectedMaterialType = [];
        this.pageStart = 0;
        this.querySortBy = '';
        this.getResources();
      }
    });
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }


  initial() {
    this.selectedSubCategories = [];
    this.resources = [];
    this.readmore = [];
    this.Allresources = [];
    this.selectedCategories = [];
    this.selectedEducationalStandard = [];
    this.selectedEducationalUse = [];
    this.selectedLevel = [];
    this.selectedMaterialType = [];
    this.selectedCopyright = [];
    this.queryString = '';
    this.querySortBy = 'createdOn';
    this.querySubCategories = [];
    this.showQueryErrorMsg = false;
    this.searchFlag = false;
    this.pageNumber = 1;
    this.pageStart = 0;
    this.pageSize = 10;
    this.totalResultCount = 0;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getCategory(id) {
    if (this.categories && this.categories.length > 0) {
      return this.categories.find(x => x.id === id) ? this.categories.find(x => x.id === id).name : ' ';
    }
  }

  makeInt(array) {
    const result = [];
    array.forEach((item) => {
      result.push(+item);
    });
    return result;
  }

  filterCategory(category) {
    this.selectedCategories = [];
    this.selectedSubCategories = [];
    this.selectedEducationalStandard = [];
    this.selectedEducationalUse = [];
    this.selectedLevel = [];
    this.selectedMaterialType = [];
    this.selectedCopyright = [];
    this.queryString = category;
    const filter = {
      'ca': this.selectedCategories ? this.selectedCategories : [],
      'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
      'es': this.selectedEducationalStandard ? this.selectedEducationalStandard : [],
      'eu': this.selectedEducationalUse ? this.selectedEducationalUse : [],
      'le': this.selectedLevel ? this.selectedLevel : [],
      'mt': this.selectedMaterialType ? this.selectedMaterialType : [],
      'co': this.selectedCopyright ? this.selectedCopyright : [],
      'pr': [],
      'ed': [],
      'p': 0,
      's': ''
    };
    this.router.navigate(['/resources', {q: this.queryString, f: JSON.stringify(filter)}]);
  }


  getResources() {
    const filter = {
      'categories': this.makeInt(this.selectedCategories),
      'subCategories': this.makeInt(this.selectedSubCategories),
      'educationalStandard': this.makeInt(this.selectedEducationalStandard),
      'educationalUse': this.makeInt(this.selectedEducationalUse),
      'level': this.makeInt(this.selectedLevel),
      'materialType': this.makeInt(this.selectedMaterialType),
      'educations': [],
      'professions': [],
      'copyright': this.makeInt(this.selectedCopyright)
    };
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.elasticSearchService.getAllDocuments('resources', 'resource', this.pageSize, this.pageStart, this.querySortBy, filter).subscribe(
      (response) => {
        if (response.hasSucceeded) {
          this.parsed = false;
          this.resources = [];
          this.readmore = [];
          this.showReportAbuse = [];
          if (response.returnedObject.hits && response.returnedObject.hits.hits.length > 0) {
            response.returnedObject.hits.hits.forEach((item) => {
              this.resources.push(item._source);
              this.readmore.push(false);
              this.showReportAbuse.push(false);
            });
            if (response.returnedObject.hits.total.value) {
              this.totalResultCount = response.returnedObject.hits.total.value;
            } else {
              this.totalResultCount = response.returnedObject.hits.total;
            }
            this.getRatings(this.resources);
          } else {
            this.resources = [];
            this.Allresources = [];
            this.totalResultCount = 0;
            this.translate.get('No Results Found').subscribe((msg) => {
              this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
            });
          }
          this.Allresources = this.resources;
          this.searchFlag = false;
        } else {
          this.resources = [];
          this.Allresources = [];
          this.totalResultCount = 0;
          this.translate.get('Failed to retrieve results').subscribe((msg) => {
            this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.resources = [];
        this.Allresources = [];
        this.totalResultCount = 0;
        this.translate.get('No Results Found').subscribe((msg) => {
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
        contentType: 2
      });
    });
    const resources = this.resources;
    this.elasticSearchService.getRatings(idArray).subscribe((res: any) => {
      const list = res.returnedObject;
      if (list && list.length > 0) {
        list.forEach((item) => {
          resources.forEach((resource) => {
            if (item.contentId === resource.id) {
              resource.rating = item.rating;
              resource.allratings = item.allRatings;
            }
          });
        });
      }
    });
    this.parsed = true;
    this.resources = resources;
    this.Allresources = this.resources;
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

  getResourceLength() {
    const array = this.resources.filter(x => x.isDraft === false);
    return array.filter(x => x.isApproved === true).length;
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
        'mt': this.selectedMaterialType ? this.selectedMaterialType : [],
        'co': this.selectedCopyright ? this.selectedCopyright : [],
        'pr': [],
        'ed': [],
        'p': this.pageStart,
        's': this.querySortBy
      };
      this.router.navigate(['/resources', {q: this.queryString, f: JSON.stringify(filter)}]);
    } else {
      this.showQueryErrorMsg = true;
      setTimeout(() => {
        this.showQueryErrorMsg = false;
      }, 5000);
    }
  }

  getSearchResults() {
    const filter = {
      'categories': this.makeInt(this.selectedCategories),
      'subCategories': this.makeInt(this.selectedSubCategories),
      'educationalStandard': this.makeInt(this.selectedEducationalStandard),
      'educationalUse': this.makeInt(this.selectedEducationalUse),
      'level': this.makeInt(this.selectedLevel),
      'materialType': this.makeInt(this.selectedMaterialType),
      'educations': [],
      'professions': [],
      'copyright': this.makeInt(this.selectedCopyright)
    };
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.elasticSearchService.searchItem('resources', 'resource', this.pageSize, this.pageStart, this.queryString, this.querySortBy, filter).subscribe(
      (response) => {
        if (response.hasSucceeded) {
          this.parsed = false;
          this.resources = [];
          this.readmore = [];
          this.showReportAbuse = [];
          if (response.returnedObject.hits && response.returnedObject.hits.hits.length > 0) {
            response.returnedObject.hits.hits.forEach((item) => {
              this.resources.push(item._source);
              this.readmore.push(false);
              this.showReportAbuse.push(false);
            });
            if (response.returnedObject.hits.total.value) {
              this.totalResultCount = response.returnedObject.hits.total.value;
            } else {
              this.totalResultCount = response.returnedObject.hits.total;
            }
            this.getRatings(this.resources);
          } else {
            this.resources = [];
            this.Allresources = [];
            this.totalResultCount = 0;
            this.translate.get('No Results Found').subscribe((msg) => {
              this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
            });
          }
          this.Allresources = this.resources;
          this.searchFlag = true;
        } else {
          this.resources = [];
          this.Allresources = [];
          this.totalResultCount = 0;
          this.translate.get('Failed to retrieve results').subscribe((msg) => {
            this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.resources = [];
        this.Allresources = [];
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
      'mt': this.selectedMaterialType ? this.selectedMaterialType : [],
      'co': this.selectedCopyright ? this.selectedCopyright : [],
      'pr': [],
      'ed': [],
      'p': this.pageStart,
      's': this.querySortBy
    };
    this.router.navigate(['/resources', {q: this.queryString, f: JSON.stringify(filter)}]);
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
      'mt': this.selectedMaterialType ? this.selectedMaterialType : [],
      'co': this.selectedCopyright ? this.selectedCopyright : [],
      'pr': [],
      'ed': [],
      'p': this.pageStart,
      's': this.querySortBy
    };
    this.router.navigate(['/resources', {q: this.queryString, f: JSON.stringify(filter)}]);
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
      'mt': this.selectedMaterialType ? this.selectedMaterialType : [],
      'co': this.selectedCopyright ? this.selectedCopyright : [],
      'pr': [],
      'ed': [],
      'p': this.pageStart,
      's': this.querySortBy
    };
    this.router.navigate(['/resources', {q: this.queryString, f: JSON.stringify(filter)}]);
  }

  clearSearch() {
    this.router.navigate(['/resources']);
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

  getMaterialTypeName(id) {
    if (this.materialTypes.length > 0) {
      return this.getCurrentLang() === 'en' ? this.materialTypes.find(x => x.id === +id).name : this.materialTypes.find(x => x.id === +id).name_Ar;
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

  removeMaterialType(id, index) {
    this.selectedMaterialType.splice(index, 1);
    this.fliter();
    const data = this.materialTypes;
    this.materialTypes = [];
    setTimeout(() => {
      this.materialTypes = data;
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
