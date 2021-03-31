import {Component, OnDestroy, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {environment} from '../../../environments/environment';
import {Subscription} from 'rxjs';
import {EncService} from '../../services/enc.service';
import {Title} from '@angular/platform-browser';
import {Router} from '@angular/router';
import {ElasticSearchService} from '../../services/elastic-search.service';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-discover',
  templateUrl: './discover.component.html'
})
export class DiscoverComponent implements OnInit, OnDestroy {
  resources: any;
  storageUrl: string;
  private sub: Subscription;
  pageNumber: number;
  pageSize: number;
  totalRowCount: number;
  parsed: boolean;

  constructor(private titleService: Title, public router: Router, private elasticSearchService: ElasticSearchService, private profileService: ProfileService, private encService: EncService, private translate: TranslateService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Discover | UAE - Open Educational Resources');
    this.resources = [];
    this.pageNumber = 1;
    this.pageSize = 24;
    this.parsed = false;
    this.totalRowCount = 0;
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.getUserRecommendedContent();
    });
    this.getUserRecommendedContent();
  }

  getEncodeed(str) {
    return this.encService.set(str);
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

  getRatings(data) {
    const idArray = [];
    data.forEach((item) => {
      idArray.push({
        contentId: item.id,
        contentType: item.contentType
      });
    });
    const resources = this.resources;
    this.elasticSearchService.getRatings(idArray).subscribe((res: any) => {
      const list = res.returnedObject;
      list.forEach((item) => {
        resources.forEach((resource) => {
          if (item.contentId === resource.id) {
            resource.rating = item.rating;
            resource.allratings = item.allRatings;
          }
        });
      });
    });
    this.parsed = true;
    this.resources = resources;
  }


  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getUserRecommendedContent() {
    this.profileService.getUserRecommendedContent(this.pageNumber, this.pageSize).subscribe((res) => {
      if (res.hasSucceeded) {
        this.resources = res.returnedObject;
        this.totalRowCount = this.resources.length > 0 ? this.resources[0].totalRows : 0;
        this.getRatings(this.resources);
      } else {
        if (res.message === 'Server Error') {
          this.profileService.getDefaultRecommendedContent(this.pageNumber, this.pageSize).subscribe((result) => {
            if (result.hasSucceeded) {
              this.resources = result.returnedObject;
              this.totalRowCount = this.resources.length > 0 ? this.resources[0].totalRows : 0;
              this.getRatings(this.resources);
            }
          });
        }
      }
    });
  }

  truncate(value: string, limit = 100, completeWords = true, ellipsis = '…') {
    if (value && value.length > 10) {
      let lastindex = limit;
      if (completeWords) {
        lastindex = value.substr(0, limit).lastIndexOf(' ');
      }
      return `${value.substr(0, limit)}${ellipsis}`;
    } else {
      return value;
    }
  }

  pageChange(event) {
    this.pageNumber = event.page + 1;
    this.getUserRecommendedContent();
  }
}
