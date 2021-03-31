import {Component, OnDestroy, OnInit} from '@angular/core';
import {ResourceService} from '../../services/resource.service';
import {environment} from '../../../environments/environment';
import {ProfileService} from '../../services/profile.service';
import {Subscription} from 'rxjs';
import {EncService} from '../../services/enc.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-recomended-articles',
  templateUrl: './recomended-articles.component.html'
})
export class RecomendedArticlesComponent implements OnInit, OnDestroy {
  resources: any;
  storageUrl: string;
  private sub: Subscription;

  constructor(public encService: EncService, private profileService: ProfileService, public router: Router) {
  }

  ngOnInit() {
    this.resources = [];
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.getUserRecommendedContent();
    });
    this.getUserRecommendedContent();
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getUserRecommendedContent() {
    this.profileService.getUserRecommendedContent(1, 8).subscribe((res) => {
      if (res.hasSucceeded) {
        this.resources = res.returnedObject;
      }
    });
  }

}
