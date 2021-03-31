import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MediaService } from '../../service/media.service';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';

@Component({
  selector: 'app-protocol-homepage',
  templateUrl: './protocol-homepage.component.html',
  styleUrls: ['./protocol-homepage.component.scss']
})
export class ProtocolHomepageComponent implements OnInit {
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  isHRDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 4;
  isHRDepartmentTeamUserID = this.currentUser.OrgUnitID == 4 && !this.currentUser.IsOrgHead;
  protocolProgresscardDetails: any;
  lang:string;
  constructor(private router: Router, private mediaService:MediaService, private common: CommonService,private arabicService:ArabicDataService) {
      if(this.common.currentLang == 'en'){
        this.common.breadscrumChange('Protocol Services','List Page','');
        this.common.topBanner(true, 'Dashboard', '', '');
      } else if(this.common.currentLang == 'ar'){
        this.common.breadscrumChange(this.arabicfn('protocolservices'),this.arabicfn('listpage'),'');
        this.common.topBanner(true, this.arabicfn('dashboard'), '', '');
      }
    }

  ngOnInit() {
    this.loadRequestCounts();
    this.lang = this.common.currentLang;
    if (this.lang == 'en') {
      this.protocolProgresscardDetails = [
        {
          'image': 'assets/hr-dashboard/file.png',
          'count': 0,
          'progress': 0,
          countType:'Calendar',
          requestType:'Calendar Management',
          pageLink:'/'+this.common.currentLang+'/app/media/calendar-management/list'
        },
        {
          'image': 'assets/hr-dashboard/agreement.png',
          'count': 0,
          'progress': 0,
          countType:'Gift',
          requestType:'Gift Management',
          pageLink:'/'+this.common.currentLang+'/app/media/gifts-management/dashboard'
        },
        {
          'image': 'assets/hr-dashboard/concept.png',
          'count': 0,
          'progress': 0,
          countType:'Media',
          requestType:'Media Requests',
          pageLink:'/'+this.common.currentLang+'/app/media/media-protocol-request'
        }
      ];
    } else {
      this.protocolProgresscardDetails = [
        {
          'image': 'assets/hr-dashboard/file.png',
          'count': 0,
          'progress': 0,
          countType:'Calendar',
          requestType: this.arabicfn('calendarmanagement'),
          pageLink:'/'+this.common.currentLang+'/app/media/calendar-management/list'
        },
        {
          'image': 'assets/hr-dashboard/agreement.png',
          'count': 0,
          'progress': 0,
          countType:'Gift',
          requestType: this.arabicfn('giftmanagement'),
          pageLink:'/'+this.common.currentLang+'/app/media/gifts-management/dashboard'
        },
        {
          'image': 'assets/hr-dashboard/concept.png',
          'count': 0,
          'progress': 0,
          countType:'Media',
          requestType: this.arabicfn('mediarequests'),
          pageLink:'/'+this.common.currentLang+'/app/media/media-protocol-request'
        }
      ];
    }
    // this.protocolProgresscardDetails.forEach((hpdObj) => {
    //   if(this.lang == 'ar'){
    //     hpdObj.requestType = this.arabicService.words[hpdObj.requestType];
    //   }
    //   if(this.lang == 'en'){
    //     hpdObj.requestType = hpdObj.requestType;
    //   }
    // });
  }

  async loadRequestCounts(){
    await this.mediaService.getProtocolListCount(this.currentUser.id).subscribe((modCountRes:any)=>{
      if(modCountRes){
        this.protocolProgresscardDetails.forEach((cardObj)=>{
          if(modCountRes[cardObj.countType] && modCountRes[cardObj.countType] > 0){
              cardObj.count = modCountRes[cardObj.countType];
          }else{
            cardObj.count = 0;
          }
        });
      }
    });
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }

}
