import {Component, OnInit} from '@angular/core';
import {ResourceService} from '../../services/resource.service';
import {ActivatedRoute, Router} from '@angular/router';
import {ElasticSearchService} from '../../services/elastic-search.service';
import {EncService} from '../../services/enc.service';
import {MessageService} from 'primeng/api';
import {NgxSpinnerService} from 'ngx-spinner';
import {Title} from '@angular/platform-browser';
import {ProfileService} from '../../services/profile.service';
import {TranslateService} from '@ngx-translate/core';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html'
})
export class SearchResultComponent implements OnInit {
  subCategories: any[];
  Allresults: any[];
  results: any[];
  categories: any[];
  educationalStandards: any;
  educationalUses: any;
  levels: any;
  materialTypes: any;
  copyrights: any;
  educations: any;
  professions: any;
  selectedEducationalStandards: any;
  selectedEducationalUses: any;
  selectedLevels: any;
  selectedCategories: any;
  selectedCopyrights: any;
  selectedSubCategories: any[];
  showReportAbuse: any;
  totalResultCount: number;
  searchFlag: boolean;
  showQueryErrorMsg: boolean;
  pageSize: number;
  pageStart: number;
  querySortBy: string;
  queryString: string;

  constructor(public router: Router, private titleService: Title, public encService: EncService, private translate: TranslateService, private spinner: NgxSpinnerService, private resourceService: ResourceService,
              private elasticSearchService: ElasticSearchService, private profileService: ProfileService, private route: ActivatedRoute, private messageService: MessageService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Advanced Search | UAE - Open Educational Resources');
    this.subCategories = [];
    this.categories = [];
    this.educationalStandards = [];
    this.educationalUses = [];
    this.levels = [];
    this.materialTypes = [];
    this.copyrights = [];
    this.educations = [];
    this.professions = [];
    this.showReportAbuse = [];
    this.pageSize = 12;
    this.pageStart = 0;
    this.querySortBy = 'createdOn';
    this.queryString = '';
    this.Allresults = [];
    this.results = [];
    this.selectedEducationalStandards = [];
    this.selectedEducationalUses = [];
    this.selectedLevels = [];
    this.selectedCategories = [];
    this.selectedCopyrights = [];
    this.selectedSubCategories = [];
    this.totalResultCount = 0;
    this.searchFlag = false;
    this.showQueryErrorMsg = false;
    this.getMasterData();
    this.route.params.subscribe((res) => {
      if (res.q || res.f) {
        if (res.q) {
          this.queryString = res.q;
        }
        if (res.f) {
          this.selectedCategories = JSON.parse(res.f).ca;
          this.selectedCopyrights = JSON.parse(res.f).co;
          this.selectedEducationalStandards = JSON.parse(res.f).es;
          this.selectedEducationalUses = JSON.parse(res.f).eu;
          this.selectedLevels = JSON.parse(res.f).le;
          this.selectedSubCategories = JSON.parse(res.f).sc;
          this.pageStart = JSON.parse(res.f).p;
          this.querySortBy = JSON.parse(res.f).s;
        }
        if (this.queryString.length > 0) {
          this.getSearchResults();
        } else {
          this.getCourses();
        }
      } else {
        this.queryString = '';
        this.selectedCategories = [];
        this.selectedCopyrights = [];
        this.selectedEducationalStandards = [];
        this.selectedEducationalUses = [];
        this.selectedLevels = [];
        this.selectedSubCategories = [];
        this.pageStart = 0;
        this.querySortBy = '';
        this.getCourses();
      }
    });
    $(document).ready(function () {
      $('.ref-filt-head-icon').click(function () {
        $('.filter-accordion').stop().slideToggle();
      });
    });
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

  getCourses() {
    const filter = {
      'categories': this.makeInt(this.selectedCategories),
      'subCategories': this.makeInt(this.selectedSubCategories),
      'educationalStandard': this.makeInt(this.selectedEducationalStandards),
      'educationalUse': this.makeInt(this.selectedEducationalUses),
      'level': this.makeInt(this.selectedLevels),
      'materialType': [],
      'educations': [],
      'professions': [],
      'copyright': this.makeInt(this.selectedCopyrights)
    };
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.elasticSearchService.advancedGetAllDocuments(this.pageSize, this.pageStart, this.querySortBy, filter).subscribe(
      (response) => {
        if (response.hasSucceeded) {
          this.results = [];
          if (response.returnedObject.hits && response.returnedObject.hits.hits.length > 0) {
            response.returnedObject.hits.hits.forEach((item) => {
              this.results.push(item);
            });
            if (response.returnedObject.hits.total.value) {
              this.totalResultCount = response.returnedObject.hits.total.value;
            } else {
              this.totalResultCount = response.returnedObject.hits.total;
            }
            this.getRatings(this.results);
          } else {
            this.results = [];
            this.Allresults = [];
            this.totalResultCount = 0;
            this.translate.get('No Results Found').subscribe((msg) => {
              this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
            });
          }
          this.Allresults = this.results;
          this.searchFlag = false;
        } else {
          this.results = [];
          this.Allresults = [];
          this.totalResultCount = 0;
          this.translate.get('Failed to retrieve results').subscribe((msg) => {
            this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.results = [];
        this.Allresults = [];
        this.totalResultCount = 0;
        this.translate.get('Failed to retrieve results').subscribe((msg) => {
          this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
        });
        this.spinner.hide();
      });
  }


  getMasterData() {
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
      'educationalStandard': this.makeInt(this.selectedEducationalStandards),
      'educationalUse': this.makeInt(this.selectedEducationalUses),
      'level': this.makeInt(this.selectedLevels),
      'materialType': [],
      'educations': [],
      'professions': [],
      'copyright': this.makeInt(this.selectedCopyrights)
    };
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.elasticSearchService.advanceSearch(this.pageSize, this.pageStart, this.queryString, this.querySortBy, filter).subscribe(
      (response) => {
        if (response.hasSucceeded) {
          this.results = [];
          if (response.returnedObject.hits && response.returnedObject.hits.hits.length > 0) {
            response.returnedObject.hits.hits.forEach((item) => {
              this.results.push(item);
            });
            if (response.returnedObject.hits.total.value) {
              this.totalResultCount = response.returnedObject.hits.total.value;
            } else {
              this.totalResultCount = response.returnedObject.hits.total;
            }
            this.getRatings(this.results);
          } else {
            this.results = [];
            this.Allresults = [];
            this.totalResultCount = 0;
            this.translate.get('No Results Found').subscribe((msg) => {
              this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
            });
          }
          this.Allresults = this.results;
          this.searchFlag = true;
        } else {
          this.results = [];
          this.Allresults = [];
          this.totalResultCount = 0;
          this.translate.get('Failed to retrieve results').subscribe((msg) => {
            this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.results = [];
        this.Allresults = [];
        this.totalResultCount = 0;
        this.translate.get('Failed to retrieve results').subscribe((msg) => {
          this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
        });
        this.spinner.hide();
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

  fliter() {
    this.pageStart = 0;
    const filter = {
      'ca': this.selectedCategories ? this.selectedCategories : [],
      'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
      'es': this.selectedEducationalStandards ? this.selectedEducationalStandards : [],
      'eu': this.selectedEducationalUses ? this.selectedEducationalUses : [],
      'le': this.selectedLevels ? this.selectedLevels : [],
      'mt': [],
      'co': this.selectedCopyrights ? this.selectedCopyrights : [],
      'pr': [],
      'ed': [],
      'p': 0,
      's': this.querySortBy
    };
    this.router.navigate(['/search', {q: this.queryString, f: JSON.stringify(filter)}]);
    // if (this.queryString.length > 0) {
    //   this.pageStart = 0;
    //   this.getSearchResults();
    // } else {
    //   this.pageStart = 0;
    //   this.getCourses();
    // }
  }

  clearSearch() {
    this.router.navigate(['/search']);
  }

  pageChange(event) {
    const filter = {
      'ca': this.selectedCategories ? this.selectedCategories : [],
      'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
      'es': this.selectedEducationalStandards ? this.selectedEducationalStandards : [],
      'eu': this.selectedEducationalUses ? this.selectedEducationalUses : [],
      'le': this.selectedLevels ? this.selectedLevels : [],
      'mt': [],
      'co': this.selectedCopyrights ? this.selectedCopyrights : [],
      'pr': [],
      'ed': [],
      'p': (this.pageSize * event.page),
      's': this.querySortBy
    };
    this.router.navigate(['/search', {q: this.queryString, f: JSON.stringify(filter)}]);
    // this.pageStart = (this.pageSize * event.page);
    // if (this.searchFlag) {
    //   this.getSearchResults();
    // } else {
    //   this.getCourses();
    // }
  }

  search() {
    if (this.queryString.trim().length > 0) {
      this.pageStart = 0;
      const filter = {
        'ca': this.selectedCategories ? this.selectedCategories : [],
        'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
        'es': this.selectedEducationalStandards ? this.selectedEducationalStandards : [],
        'eu': this.selectedEducationalUses ? this.selectedEducationalUses : [],
        'le': this.selectedLevels ? this.selectedLevels : [],
        'mt': [],
        'co': this.selectedCopyrights ? this.selectedCopyrights : [],
        'pr': [],
        'ed': [],
        'p': 0,
        's': this.querySortBy
      };
      this.router.navigate(['/search', {q: this.queryString, f: JSON.stringify(filter)}]);
    } else {
      this.showQueryErrorMsg = true;
      setTimeout(() => {
        this.showQueryErrorMsg = false;
      }, 5000);
      // this.getCourses();
    }
  }

  getRatings(data) {
    const idArray = [];
    data.forEach((item) => {
      idArray.push({
        contentId: item._source.id,
        contentType: item._type === 'resource' ? 2 : 1
      });
    });
    const resources = this.results;
    this.elasticSearchService.getRatings(idArray).subscribe((res: any) => {
      const list = res.returnedObject;
      list.forEach((item) => {
        resources.forEach((resource) => {
          if (item.contentId === resource._source.id) {
            resource._source.rating = item.rating;
            resource._source.allratings = item.allRatings;
          }
        });
      });
    });
    this.results = resources;
  }

  sort() {
    const filter = {
      'ca': this.selectedCategories ? this.selectedCategories : [],
      'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
      'es': this.selectedEducationalStandards ? this.selectedEducationalStandards : [],
      'eu': this.selectedEducationalUses ? this.selectedEducationalUses : [],
      'le': this.selectedLevels ? this.selectedLevels : [],
      'mt': [],
      'co': this.selectedCopyrights ? this.selectedCopyrights : [],
      'pr': [],
      'ed': [],
      'p': 0,
      's': this.querySortBy
    };
    this.router.navigate(['/search', {q: this.queryString, f: JSON.stringify(filter)}]);
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
    this.selectedEducationalStandards.splice(index, 1);
    this.fliter();
    const data = this.educationalStandards;
    this.educationalStandards = [];
    setTimeout(() => {
      this.educationalStandards = data;
    }, 200);
  }

  removeEducationalUse(id, index) {
    this.selectedEducationalUses.splice(index, 1);
    this.fliter();
    const data = this.educationalUses;
    this.educationalUses = [];
    setTimeout(() => {
      this.educationalUses = data;
    }, 200);
  }

  removeLevel(id, index) {
    this.selectedLevels.splice(index, 1);
    this.fliter();
    const data = this.levels;
    this.levels = [];
    setTimeout(() => {
      this.levels = data;
    }, 200);
  }

  removeCopyright(id, index) {
    this.selectedCopyrights.splice(index, 1);
    this.fliter();
    const data = this.copyrights;
    this.copyrights = [];
    setTimeout(() => {
      this.copyrights = data;
    }, 200);
  }
}
