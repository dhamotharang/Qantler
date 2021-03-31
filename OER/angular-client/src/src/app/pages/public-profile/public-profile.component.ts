import {Component, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {environment} from '../../../environments/environment';
import {ActivatedRoute} from '@angular/router';
import {EncService} from '../../services/enc.service';
import {TranslateService} from '@ngx-translate/core';
import {ResourceService} from '../../services/resource.service';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-public-profile',
  templateUrl: './public-profile.component.html'
})
export class PublicProfileComponent implements OnInit {
  user: any;
  slug: any;
  StorageBaseUrl: any;
  categories: any;
  subjectsInterested : string;
  subjectsInterestedAr : string;
  constructor(private profileService: ProfileService, private translate: TranslateService, private spinner: NgxSpinnerService, private resourceService: ResourceService, private route: ActivatedRoute, private encService: EncService) {
  }

  ngOnInit() {
    window.scrollTo(0, 0);
    this.route.params.subscribe(params => {
      this.slug = this.encService.get(params['slug']);
      this.getDashBoardData();      
    });

  }

  getDashBoardData() {
    if (this.slug) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.profileService.getPublicUserById(this.slug).subscribe((res) => {
        if (res.hasSucceeded) {
          this.user = res.returnedObject;
          this.getCategories(this.user);
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
      });
    }
  }

  getCurrentLang() {
    return this.translate.currentLang;
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

  getCategories(data:any) {
    this.resourceService.getResourceMasterData().subscribe((res) => {
      if (res.hasSucceeded) {
        this.categories = res.returnedObject.categoryMasterData;        
        this.getInterestSubject(data);
      }
    });
  }

  getInterestSubject(data:any){
    var splitData = data.subjectsInterested.split(',');    
    this.subjectsInterested ='';
    this.subjectsInterestedAr ='';
    for(let i=0;i<splitData.length;i++){
      if(i==0){
        this.subjectsInterested += this.categories.filter(x => x.id == splitData[i])[0].name;
        this.subjectsInterestedAr += this.categories.filter(x => x.id == splitData[i])[0].name_Ar;
      }
      else{
        this.subjectsInterested += ','+this.categories.filter(x => x.id == splitData[i])[0].name;
        this.subjectsInterestedAr += ','+this.categories.filter(x => x.id == splitData[i])[0].name_Ar;
      }
    }
  }
}
