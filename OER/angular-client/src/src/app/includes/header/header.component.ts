import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import {environment} from '../../../environments/environment';
import {ProfileService} from '../../services/profile.service';
import {Subscription} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';
import {Router} from '@angular/router';
import {MessageService} from 'primeng/api';
import {NotificationService} from '../../services/notification.service';
import {CookieService} from 'ngx-cookie-service';
import {NgxSpinnerService} from 'ngx-spinner';

declare var jQuery: any;
declare var $: any;
declare var EnjoyHint: any;

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})

export class HeaderComponent implements OnInit, OnDestroy {
  loggedIn: boolean;
  showSpinner: boolean;
  themeBox: boolean;
  tempSpinner: boolean;
  notificationBox: boolean;
  showSearchBox: boolean;
  newAnnouncements: boolean;
  checkAnnouncement: boolean;
  keyCloakResponse: any;
  Announcements: any;
  langClass: string;
  query: string;
  userName: string;
  private sub: Subscription;
  portalLanguages: any;
  notifications: any;
  unReadNotifications: number = 0;
  notificationCount: number;
  theme: string;
  @ViewChild('input') input;
  showMore : boolean = false;
  notifyNo = null;
  constructor(private notificationService: NotificationService, private spinner: NgxSpinnerService, private cookieService: CookieService, private translate: TranslateService, private messageService: MessageService, protected keycloakAngular: KeycloakService, private profileService: ProfileService, private router: Router) {
  }

  ngOnInit() {
    this.portalLanguages = null;
    this.loggedIn = false;
    this.themeBox = false;
    this.showSpinner = false;
    this.tempSpinner = false;
    this.notificationBox = false;
    this.keyCloakResponse = null;
    this.Announcements = [];
    this.notifications = this.notificationService.notificationList;
    this.notificationCount = this.notificationService.notificationCount;
    this.notificationService.notificationUpdate.subscribe(() => {
      this.notifications = this.notificationService.notificationList;
      this.notificationCount = this.notificationService.notificationCount;
      if(this.notificationService.notificationList)
      this.unReadNotifications = this.notificationService.notificationList[0].totalUnRead;//.filter(x=> x.isRead===false).length;
      else
      this.unReadNotifications = 0;
    });
    this.showSearchBox = false;
    this.newAnnouncements = false;
    this.langClass = '';
    this.query = '';
    this.userName = '';
    if (this.cookieService.get('showHint') === '') {
      this.tempSpinner = true;
    }
    // this.profileService.getMakeProfileData().subscribe((res) => {
    //   this.portalLanguages = res.returnedObject.portalLanguages;
    // });
    this.checkAnnouncements();
    setTimeout(() => {
      this.checkAnnouncements();
    }, 900000);
    this.theme = this.profileService.theme;
    this.profileService.getClientThemeUpdate().subscribe(() => {
      this.theme = this.profileService.theme;
    });
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.checkAnnouncements();
      this.keycloakAngular.isLoggedIn().then((res) => {
        this.loggedIn = res;
        if (res) {
          if (this.profileService.user && this.profileService.user.firstName && this.profileService.user.lastName) {
            this.userName = this.profileService.user.firstName + ' ' + this.profileService.user.lastName;
          } else {
            this.keycloakAngular.loadUserProfile().then((user) => {
              this.userName = user.firstName + ' ' + user.lastName;
            });
          }
        }
      }, (error) => {
        this.loggedIn = false;
      });
    });
    this.router.events.subscribe(() => {
      if (this.router.url.split('/')[this.router.url.split('/').length - 1] === 'announcements' && !this.checkAnnouncement) {
        this.checkAnnouncement = true;
        setTimeout(() => {
          this.checkAnnouncements();
        }, 4000);
      }
    });
    this.translate.onLangChange.subscribe((res) => {
      if (res.lang === 'ar') {
        this.langClass = 'ar';
      } else {
        this.langClass = '';
      }
    });
  }

  showHint() {
    if (this.translate.currentLang === 'en') {
      if (this.loggedIn) {
        $(document).ready(function () {
          function formatDate(date) {
            const d = new Date(date);
            let month = '' + (d.getMonth() + 1);
            let day = '' + d.getDate();
            const year = d.getFullYear();

            if (month.length < 2) {
              month = '0' + month;
            }
            if (day.length < 2) {
              day = '0' + day;
            }

            return [year, month, day].join('-');
          }

          const enjoyhint_instance = new EnjoyHint({
            onSkip: function () {
              document.cookie = 'showHint=false';
            },
            onEnd: function () {
              document.cookie = 'showHint=false';
            }
          });
          const enjoyhint_script_steps = [
            {
              'next .discover': 'Discover more about this portal',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .courses': 'Check all the courses',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .resources': 'Check all the resources',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .create': 'Create courses/resources',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .search': 'Search all details in this portal',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .lang': 'Change Language(English or Arabic)',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            }
            // ,{
            //   'next .dashboard': 'View all the information in your account',
            // }

          ];

          // set script config
          enjoyhint_instance.set(enjoyhint_script_steps);

          // run Enjoyhint script
          enjoyhint_instance.run();
        });
      } else {
        $(document).ready(function () {
          function formatDate(date) {
            const d = new Date(date);
            let month = '' + (d.getMonth() + 1);
            let day = '' + d.getDate();
            const year = d.getFullYear();

            if (month.length < 2) {
              month = '0' + month;
            }
            if (day.length < 2) {
              day = '0' + day;
            }

            return [year, month, day].join('-');
          }

          const enjoyhint_instance = new EnjoyHint({
            onSkip: function () {
              document.cookie = 'showHint=false';
            },
            onEnd: function () {
              document.cookie = 'showHint=false';
            }
          });
          const enjoyhint_script_steps = [
            {
              'next .discover': 'Discover more about this portal',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .courses': 'Check all the courses',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .resources': 'Check all the resources',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .create': 'Create courses/resources',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .search': 'Search all details in this portal',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .signup': 'Sign up on this portal',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .signin': 'Sign in this portal',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            },
            {
              'next .lang': 'Change Language(English or Arabic)',
              'skipButton': {text: 'Skip'},
              'nextButton': {text: 'Next'}
            }
            // ,{
            //   'next .dashboard': 'View all the information in your account',
            // }

          ];

          // set script config
          enjoyhint_instance.set(enjoyhint_script_steps);

          // run Enjoyhint script
          enjoyhint_instance.run();
        });
      }
    } else {
      if (this.loggedIn) {
        $(document).ready(function () {
          function formatDate(date) {
            const d = new Date(date);
            let month = '' + (d.getMonth() + 1);
            let day = '' + d.getDate();
            const year = d.getFullYear();

            if (month.length < 2) {
              month = '0' + month;
            }
            if (day.length < 2) {
              day = '0' + day;
            }

            return [year, month, day].join('-');
          }

          const enjoyhint_instance = new EnjoyHint({
            onSkip: function () {
              document.cookie = 'showHint=false';
            },
            onEnd: function () {
              document.cookie = 'showHint=false';
            }
          });
          const enjoyhint_script_steps = [
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .discover': 'هنا يمكنك استعراض أحدث المساقات والمصادر التعليمية المنشورة من قبل مختلف المساهمين حول العالم'

            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .courses': 'بالضغط هنا، يمكنك البحث والتسجيل في المساقات المتنوعة والمتوفرة على منصة منارة' // add arabic translations

            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .resources': 'استكشف وابحث في المصادر التعليمية المتوفرة على منصة منارة' // add arabic translations

            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .create': 'من هنا يمكنك المساهمة في انشاء المصادر التعليمية ومساقاتها' // add arabic translations
            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .search': 'ابحث في كل التفاصيل في هذه البوابة' // add arabic translations
            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .lang': 'تغيير اللغة (الإنجليزية أو العربية)' // add arabic translations
            }
            // ,{
            //   'next .dashboard': 'View all the information in your account',
            // }

          ];

          // set script config
          enjoyhint_instance.set(enjoyhint_script_steps);

          // run Enjoyhint script
          enjoyhint_instance.run();
        });
      } else {
        $(document).ready(function () {
          function formatDate(date) {
            const d = new Date(date);
            let month = '' + (d.getMonth() + 1);
            let day = '' + d.getDate();
            const year = d.getFullYear();

            if (month.length < 2) {
              month = '0' + month;
            }
            if (day.length < 2) {
              day = '0' + day;
            }

            return [year, month, day].join('-');
          }

          const enjoyhint_instance = new EnjoyHint({
            onSkip: function () {
              document.cookie = 'showHint=false';
            },
            onEnd: function () {
              document.cookie = 'showHint=false';
            }
          });
          const enjoyhint_script_steps = [
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .discover': 'هنا يمكنك استعراض أحدث المساقات والمصادر التعليمية المنشورة من قبل مختلف المساهمين حول العالم'

            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .courses': 'بالضغط هنا، يمكنك البحث والتسجيل في المساقات المتنوعة والمتوفرة على منصة منارة' // add arabic translations

            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .resources': 'استكشف وابحث في المصادر التعليمية المتوفرة على منصة منارة' // add arabic translations
            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .create': 'من هنا يمكنك المساهمة في انشاء المصادر التعليمية ومساقاتها' // add arabic translations
            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .search': 'ابحث في كل التفاصيل في هذه البوابة' // add arabic translations
            },
            {
              'skipButton': {text: 'تخطي'},
              'nextButton': {text: 'التالي'},
              'next .lang': 'تغيير اللغة (الإنجليزية أو العربية)' // add arabic translations
            }
            // ,{
            //   'next .dashboard': 'View all the information in your account',
            // }

          ];

          // set script config
          enjoyhint_instance.set(enjoyhint_script_steps);

          // run Enjoyhint script
          enjoyhint_instance.run();
        });
      }
    }
  }

  updateHelpStatus() {
    this.showHint();
  }

  checkLandingPage() {
    return (this.router.url === '/');
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  handleThemeBox(value: boolean) {
    this.themeBox = value;
    this.notificationBox = false;
    this.showSearchBox = false;
  }

  handleNotificationBox(value: boolean) {
    this.notificationBox = value;
    this.themeBox = false;
    this.showSearchBox = false;
  }

  getThemeClass() {
    return this.profileService.theme === 'blue' ? 'blue' : '';
  }

  switchLaguage(language) {
    this.cookieService.set('manaraLang', language);
    if (this.profileService.userId) {
      if (language === 'ar') {
        this.profileService.updateProtalLanguage(1);
      } else if (language === 'en') {
        this.profileService.updateProtalLanguage(2);
      }
    } else {
      this.translate.use(language);
    }
  }

  updateTheme(event) {
    this.themeBox = false;
    // this.profileService.setTheme(event.target.value);
    this.profileService.setTheme(event);
  }

  getLangClass() {
    return this.langClass;
  }

  getLanguage() {
    return this.translate.currentLang;
  }

  logout() {
    this.keycloakAngular.logout(environment.clientUrl);
  }

  showSearch() {
    this.themeBox = false;
    this.notificationBox = false;
    this.showSearchBox = true;
    setTimeout(() => {
      this.input.nativeElement.focus();
    }, 500);
  }

  closeSearch() {
    this.showSearchBox = false;
  }

  signIn() {
    const url = this.router.url;
    this.router.navigate(['/']);
    this.keycloakAngular.login({
      kcLocale: this.cookieService.get('manaraLang') ? this.cookieService.get('manaraLang') : 'en',
      redirectUri: url
    });
  }

  signUp() {
    const url = this.router.url;
    this.router.navigate(['/']);
    this.keycloakAngular.register({
      kcLocale: this.cookieService.get('manaraLang') ? this.cookieService.get('manaraLang') : 'en',
      redirectUri: url
    });
  }

  checkAnnouncements() {
    if (this.profileService.userId) {
      this.profileService.getAnnouncementsByUser(this.profileService.userId, 1, 25)
        .subscribe((res: any) => {
          if (res.hasSucceeded) {
            this.Announcements = res.returnedObject;
            let found = false;
            this.Announcements.forEach((item) => {
              if (Date.parse(item.lastLogin) < Date.parse(item.updatedOn)) {
                found = true;
              }
            });
            this.newAnnouncements = found;
          } else {
            this.newAnnouncements = true;
          }
          this.checkAnnouncement = false;
        }, (error) => {
          this.newAnnouncements = true;
          this.checkAnnouncement = false;
        });
    }
  }

  checkNewAnnouncements() {
    return this.newAnnouncements;
  }

  search() {
    if (this.query && this.query.trim().length > 0) {
      this.router.navigate(['/search', {q: this.query}]);
      this.showSearchBox = false;
      this.query = '';
    } else {
      this.translate.get('Please enter a valid query').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
    }
  }

  clickAction(notification) {
    if (this.profileService.userId > 0 && !notification.isRead) {
      this.notificationService.updateNotifications(this.profileService.userId, notification.id).subscribe(() => {
        this.notificationService.getMyList();
      });
    }
    if (notification.emailUrl) {
      this.router.navigateByUrl(notification.emailUrl.replace(environment.clientUrl, ''));
      this.notificationBox = false;
    }
  }

  lessMoreItem(notify,lessMore){
    if(lessMore == 1){
      this.showMore = true;
      this.notifyNo = notify.id;
    }
    else if(lessMore == 2) {
      this.showMore = false;    
      this.notifyNo = notify.id;
    }
  }

}
