import { Component } from '@angular/core';
import { ActivatedRoute, Router, Event, NavigationStart, NavigationEnd, NavigationError } from '@angular/router';
import { CommonService } from './common.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  showSpinner = true;
  language = 'ar';
  constructor(public route: ActivatedRoute,
    public router: Router,
    public common: CommonService) {
    this.route.params.subscribe(param => {
      var link = this.router.url;
      // if (param.type != 'en' || param.type != 'ar') {
      var url = '';
      //this.router.navigate(['app/'+param.type + '/memo-view/' + param.reference], { relativeTo: this.route });
      let languageSet = this.getCookieData();
      let lang = 'ar';
      this.language = lang;
      lang = (languageSet == 'English') ? 'en' : 'ar';
      if (param.mode == 'Edit') {
        switch (param.type) {
          case 'Memo':
            url = '#/' + lang + '/app/memo/memo-edit/' + param.reference;
            break;
          case 'Outbound_Letter':
            url = '#/' + lang + '/app/letter/outgoingletter-edit/' + param.reference;
            break;
          case 'Inbound_Letter':
            url = '#/' + lang + '/app/letter/incomingletter-edit/' + param.reference;
            break;
          case 'Circular':
            url = '#/' + lang + '/app/circular/Circular-edit/' + param.reference;
            break;
          case 'Meeting':
            url = '#/' + lang + '/app/meeting/view/' + param.reference;
            break;
          case 'Calendar':
            url = '#/' + lang + '/app/media/calendar-management/view-event/' + param.reference;
            break;
          case 'CAComplaintSuggestions':
            url = '#/' + lang + '/app/citizen-affair/complaint-suggestion-view/' + param.reference;
            break;
          case 'Citizen_Affair':
            url = '#/' + lang + '/app/citizen-affair/citizen-affair-view/' + param.reference;
            break;
          case 'MediaNewPressReleaseRequest':
            url = '#/' + lang + '/app/media/media-press-release/view/' + param.reference;
            break;
          case 'MediaNewPhotoGrapherRequest':
            url = '#/' + lang + '/app/media/photographer/view/' + param.reference;
            break;
          case 'MediaPhotoRequest':
            url = '#/' + lang + '/app/media/media-request-photo/view/' + param.reference;
            break;
          case 'DiwanIdentity':
            url = '#/' + lang + '/app/media/diwan-identity/request-view/' + param.reference;
            break;
          case 'MediaDesignRequest':
            url = '#/' + lang + '/app/media/media-request-design/' + param.reference;
            break;
          case 'MediaNewCampaignRequest':
            url = '#/' + lang + '/app/media/campign/view/' + param.reference;
            break;
          case 'OfficialTask':
            url = '#/' + lang + '/app/hr/official-tasks/request-view/official/' + param.reference;
            break;
          case 'Compensation':
            url = '#/' + lang + '/app/hr/official-tasks/request-view/compensation/' + param.reference;
            break;
          case 'Announcement':
            url = '#/' + lang + '/app/hr/announcement/announcement-view/' + param.reference;
            break;
          case 'BabyAddition':
            url = '#/' + lang + '/app/hr/new-baby-addition/request-view/' + param.reference;
            break;
          case 'SalaryCertificate':
            url = '#/' + lang + '/app/hr/salary-certificate/view/' + param.reference;
            break;
          case 'ExperienceCertificate':
            url = '#/' + lang + '/app/hr/experience-certificate/view/' + param.reference;
            break;
          case 'HRComplaintSuggestions':
            url = '#/' + lang + '/app/hr/raise-complaint-suggestion-view/' + param.reference;
            break;
          case 'Leave':
            url = '#/' + lang + '/app/hr/leave/request-view/' + param.reference;
            break;
          case 'Training':
            url = '#/' + lang + '/app/hr/training-request/request-view/' + param.reference;
            break;
          case 'Maintenance':
            url = '#/' + lang + '/app/maintenance/view/' + param.reference;
            break;
          case 'Legal':
            url = '#/' + lang + '/app/legal/request-view/' + param.reference;
            break;
          case 'ITSupport':
            url = '#/' + lang + '/app/it/it-request-view/' + param.reference;
            break;
          case 'DutyTask':
            url = '#/' + lang + '/app/task/task-view/' + param.reference;
            break;
          case 'UserProfile':
            url = '#/' + lang + '/app/hr/employee/view/' + param.reference;
            break;
          case 'Vehicle':
            url = '#/' + lang + '/app/vehicle-management/vehicle-request-view/' + param.reference;
            break;
          case 'VehicleFine':
            url = '#/' + lang + '/app/vehicle-management/fine-management/log-fine-view/' + param.reference + '/' + param.finePlateNum;
            break;
        }
      }
      else {
        switch (param.type) {
          case 'Memo':
            url = '#/' + lang + '/app/memo/memo-view/' + param.reference;
            break;
          case 'Outbound_Letter':
            url = '#/' + lang + '/app/letter/outgoingletter-view/' + param.reference;
            break;
          case 'Inbound_Letter':
            url = '#/' + lang + '/app/letter/incomingletter-view/' + param.reference;
            break;
          case 'Circular':
            url = '#/' + lang + '/app/circular/Circular-view/' + param.reference;
            break;
          case 'Meeting':
            url = '#/' + lang + '/app/meeting/view/' + param.reference;
            break;
          case 'Calendar':
            url = '#/' + lang + '/app/media/calendar-management/view-event/' + param.reference;
            break;
          case 'CAComplaintSuggestions':
            url = '#/' + lang + '/app/citizen-affair/complaint-suggestion-view/' + param.reference;
            break;
          case 'Citizen_Affair':
            url = '#/' + lang + '/app/citizen-affair/citizen-affair-view/' + param.reference;
            break;
          case 'MediaNewPressReleaseRequest':
            url = '#/' + lang + '/app/media/media-press-release/view/' + param.reference;
            break;
          case 'MediaNewPhotoGrapherRequest':
            url = '#/' + lang + '/app/media/photographer/view/' + param.reference;
            break;
          case 'MediaPhotoRequest':
            url = '#/' + lang + '/app/media/media-request-photo/view/' + param.reference;
            break;
          case 'DiwanIdentity':
            url = '#/' + lang + '/app/media/diwan-identity/request-view/' + param.reference;
            break;
          case 'MediaDesignRequest':
            url = '#/' + lang + '/app/media/media-request-design/' + param.reference;
            break;
          case 'MediaNewCampaignRequest':
            url = '#/' + lang + '/app/media/campign/view/' + param.reference;
            break;
          case 'OfficialTask':
            url = '#/' + lang + '/app/hr/official-tasks/request-view/official/' + param.reference;
            break;
          case 'Compensation':
            url = '#/' + lang + '/app/hr/official-tasks/request-view/compensation/' + param.reference;
            break;
          case 'Announcement':
            url = '#/' + lang + '/app/hr/announcement/announcement-view/' + param.reference;
            break;
          case 'BabyAddition':
            url = '#/' + lang + '/app/hr/new-baby-addition/request-view/' + param.reference;
            break;
          case 'SalaryCertificate':
            url = '#/' + lang + '/app/hr/salary-certificate/view/' + param.reference;
            break;
          case 'ExperienceCertificate':
            url = '#/' + lang + '/app/hr/experience-certificate/view/' + param.reference;
            break;
          case 'HRComplaintSuggestions':
            url = '#/' + lang + '/app/hr/raise-complaint-suggestion-view/' + param.reference;
            break;
          case 'Leave':
            url = '#/' + lang + '/app/hr/leave/request-view/' + param.reference;
            break;
          case 'Training':
            url = '#/' + lang + '/app/hr/training-request/request-view/' + param.reference;
            break;
          case 'Maintenance':
            url = '#/' + lang + '/app/maintenance/view/' + param.reference;
            break;
          case 'Legal':
            url = '#/' + lang + '/app/legal/request-view/' + param.reference;
            break;
          case 'ITSupport':
            url = '#/' + lang + '/app/it/it-request-view/' + param.reference;
            break;
          case 'DutyTask':
            url = '#/' + lang + '/app/task/task-view/' + param.reference;
            break;
          case 'UserProfile':
            url = '#/' + lang + '/app/hr/employee/view/' + param.reference;
            break;
          case 'Vehicle':
            url = '#/' + lang + '/app/vehicle-management/vehicle-request-view/' + param.reference;
            break;
          case 'VehicleFine':
            url = '#/' + lang + '/app/vehicle-management/fine-management/log-fine-view/' + param.reference + '/' + param.finePlateNum;
            break;
        }
      }
      if (url != '') {

        //this.router.navigate([url]);
        // //var url = '#/app/' + param.type + '/' + param.type + '-view/' + param.reference;
        var createA = document.createElement('a');
        //createA.setAttribute('target', "_blank");
        createA.setAttribute('href', url);
        createA.click();
      }
      // createA.remove();
      // }
    });
    router.events.subscribe((event: Event) => {
      if (event instanceof NavigationStart) {
        this.showSpinner = true;
      }
      if (event instanceof NavigationEnd) {
        if (event.url != '/' && event.url != '/login' && event.url != '/chat' && event.url != '/error') {
          var url = this.common.checkLangUrl(event.url);
          if (url)
            this.router.navigate([url]);
        }
        setTimeout(() => {
          this.showSpinner = false;
        }, 200);
        this.common.isNavigateTrigger = true;
      }
      if (event instanceof NavigationError) {
        setTimeout(() => {
          this.showSpinner = false;
        }, 200);
      }
    });
  }
  title = 'rrc';

  ngOnInit() {

  }

  private getCookieData() {
    // let cookies = document.cookie.split(';'),
    //   cookie = '',
    //   lang = '',
    //   cName = ' language=';
    // cookies.map(res => {
    //   cookie = res.charAt(0) + res.charAt(1);
    //   if (cookie == ' l') {
    //     lang = res.substring(res.length, cName.length);
    //   } else if (cookie == 'la') {
    //     lang = res.split('=')[1];
    //     //lang = 'Arabic';
    //   } else {
    //     lang = 'English';
    //   }
    // });
    // return lang;
    return this.common.getCookie();
  }

  checkurl(url) {
    var urls = url.split('/'),
      //firstUrlLang = urls.find(res => res == 'en' || res == 'ar'),
      lang = this.common.currentLang,
      first = '/' + lang,
      second = '';
    urls.map(res => {
      if (res && res != 'en' && res != 'ar')
        second += '/' + res;
    });
    return first + second;
  }

}
