import {Component, OnDestroy, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {environment} from '../../../environments/environment';
import {Subscription, concat} from 'rxjs';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';
import {Router} from '@angular/router';
import {KeycloakService} from 'keycloak-angular';
import { ConstantPool } from '@angular/compiler';
import { NgxSpinnerService } from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {ResourceService} from '../../services/resource.service';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html'
})
export class UserProfileComponent implements OnInit, OnDestroy {
  user: any;
  StorageBaseUrl: any;
  private sub: Subscription;
  ChangePassword : string;
  categories: any;
  subjectsInterested : string;
  subjectsInterestedAr : string;
  constructor(private titleService: Title, private translate: TranslateService,private spinner: NgxSpinnerService,private messageService: MessageService, private profileService: ProfileService, private resourceService: ResourceService, private router: Router,protected keycloakAngular: KeycloakService) {
    this.categories = [];
  }

  ngOnInit() {    
    window.scrollTo(0, 0);
    this.titleService.setTitle('Profile Information | UAE - Open Educational Resources');
    this.user = this.profileService.user;
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.user = this.profileService.user;      
      this.getCategories(this.user);
    });
    if(this.user){
      this.getCategories(this.user);
    }
    
    $(document).ready(function () {
      $('.user-side-panel-head-btn').click(function () {
        $('.user-side-panel ul').stop().slideToggle();
      });
    });
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
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
