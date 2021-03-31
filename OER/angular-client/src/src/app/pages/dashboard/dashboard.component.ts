import { Component, OnDestroy, OnInit } from '@angular/core';
import { ProfileService } from '../../services/profile.service';
import { environment } from '../../../environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription } from 'rxjs';
import { EncService } from '../../services/enc.service';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import {KeycloakService} from 'keycloak-angular';
import {MessageService} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit, OnDestroy {
  user: any;
  User: any;
  StorageBaseUrl: any;
  dashboardData: any;
  latestCourses: any;
  private sub: Subscription;
  ChangePassword : string;

  constructor(private titleService: Title,private translate: TranslateService,private messageService: MessageService, public encService: EncService, public router: Router, private profileService: ProfileService, private spinner: NgxSpinnerService,protected keycloakAngular: KeycloakService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Dashboard | UAE - Open Educational Resources');
    this.dashboardData = null;
    this.latestCourses = [];
    this.user = this.profileService.user;
    this.User = this.profileService.user;
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.user = this.profileService.user;
      this.User = this.profileService.user;
      this.getDashBoardData();
    });
    this.getDashBoardData();
    $(document).ready(function () {

      const topLimit = $('.user-side-panel').offset().top;
      $(window).scroll(function () {
        if (topLimit <= $(window).scrollTop()) {
          $('.user-side-panel').addClass('stickIt');
        } else {
          $('.user-side-panel').removeClass('stickIt');
        }
      });

      $('.user-side-panel-head-btn').click(function () {
        $('.user-side-panel ul').stop().slideToggle();
      });
    });

  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getDashBoardData() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.profileService.getUserDashboardReport().subscribe((res) => {
      if (res.hasSucceeded) {
        this.dashboardData = res.returnedObject;
        this.latestCourses = this.dashboardData.latestCourse;
        this.user = this.dashboardData;
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
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

  toChangePassword() {
    const keyCloakConfig = this.keycloakAngular.getKeycloakInstance();
    this.ChangePassword = keyCloakConfig.authServerUrl+'/realms/'+keyCloakConfig.realm+'/account/password';
    window.open(this.ChangePassword,"_self");
  }

ResetPassword(){
  this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.profileService.postResetPassword().subscribe((res) => {
      if (res.hasSucceeded) {
            this.translate.get('PleaseCheckYourMailForResettingPassword').subscribe((trans) => {
              this.messageService.add({severity: 'success', summary: trans, key: 'toast', life: 5000});
            });
          } 
          else{
            this.translate.get('Please Contact site Administrator').subscribe((trans) => {
              this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
            });
          }        
          this.spinner.hide();       
    }, (error) => {
      this.translate.get('Please Contact site Administrator').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
} 

}
