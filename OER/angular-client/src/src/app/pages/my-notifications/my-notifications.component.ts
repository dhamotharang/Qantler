import {Component, OnInit} from '@angular/core';
import {NotificationService} from '../../services/notification.service';
import {ProfileService} from '../../services/profile.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {Router} from '@angular/router';
import {environment} from '../../../environments/environment';
import {TranslateService} from '@ngx-translate/core';
import {Title} from '@angular/platform-browser';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-my-notifications',
  templateUrl: './my-notifications.component.html'
})
export class MyNotificationsComponent implements OnInit {

  notifications: any;
  totalrows: number;
  page: number;
  pageSize: number;
  showMore : boolean = false;
  notifyNo = null;
  constructor(private router: Router, private titleService: Title, private notificationService: NotificationService, private translate: TranslateService, private messageService: MessageService, private profileService: ProfileService, private spinner: NgxSpinnerService, private confirmationService: ConfirmationService) {
  }

  ngOnInit() {
    this.titleService.setTitle('My Notifications | UAE - Open Educational Resources');
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
    this.notifications = [];
    this.totalrows = 0;
    this.page = 1;
    this.pageSize = 10;
    if (this.profileService.userId > 0) {
      this.getNotifications();
    }
    this.profileService.getUserDataUpdate().subscribe(() => {
      if (this.profileService.userId > 0) {
        this.getNotifications();
      }
    });
  }

  getNotifications() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.notificationService.getMyNotifications(this.profileService.userId, this.page, this.pageSize).subscribe((res) => {
      if (res.hasSucceeded) {
        this.notifications = res.returnedObject;
        if (res.returnedObject.length > 0) {
          this.totalrows = res.returnedObject[0].totalrows;
        }
      } else if (res.message === 'No Records Found') {
        this.notifications = [];
        this.totalrows = 0;
      }
      this.spinner.hide();
    }, () => {
      this.translate.get('Failed to retrieve notification').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast'});
      });
      this.spinner.hide();
    });
  }

  paginate(event) {
    this.page = event.page + 1;
    this.getNotifications();
  }

  clickAction(notification) {
    if (this.profileService.userId > 0 && !notification.isRead) {
      this.notificationService.updateNotifications(this.profileService.userId, notification.id).subscribe(() => {
        this.notificationService.getMyList();
      });
    }
    if (notification.emailUrl) {
      this.router.navigateByUrl(notification.emailUrl.replace(environment.clientUrl, ''));
    } else {
      setTimeout(() => {
        this.getNotifications();
      }, 1000);
    }
  }

  markNotificationAsRead(id) {
    if (this.profileService.userId > 0) {
      this.translate.get('Are you sure that you want to mark this notification as Read?').subscribe((trans) => {
        this.confirmationService.confirm({
          message: trans,
          accept: () => {
            this.spinner.show(undefined, {color: this.profileService.themeColor});
            this.notificationService.updateNotifications(this.profileService.userId, id).subscribe((res) => {
              this.spinner.hide();
              if (res.hasSucceeded) {
                this.translate.get('Successfully marked notification as Read').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast'});
                });
                this.getNotifications();
                this.notificationService.getMyList();
              } else {
                this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
              }
            }, () => {
              this.translate.get('Failed to mark notification as Read').subscribe((msg) => {
                this.messageService.add({severity: 'error', summary: msg, key: 'toast'});
              });
              this.spinner.hide();
            });
          }
        });
      })
      ;
    }
  }

  deleteNotification(id) {
    if (this.profileService.userId > 0) {
      this.translate.get('Are you sure that you want to delete this notification?').subscribe((trans) => {
        this.confirmationService.confirm({
          message: trans,
          accept: () => {
            this.spinner.show(undefined, {color: this.profileService.themeColor});
            this.notificationService.deleteNotifications(this.profileService.userId, id).subscribe((res) => {
              this.spinner.hide();
              if (res.hasSucceeded) {
                this.translate.get('Successfully deleted notification').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast'});
                });
                this.getNotifications();
                this.notificationService.getMyList();
              } else {
                this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
              }
            }, () => {
              this.translate.get('Failed to delete notification').subscribe((msg) => {
                this.messageService.add({severity: 'error', summary: msg, key: 'toast'});
              });
              this.spinner.hide();
            });
          }
        });
      });
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
