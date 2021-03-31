import {Component, OnDestroy, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {Subscription} from 'rxjs';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-notification-settings',
  templateUrl: './notification-settings.component.html'
})
export class NotificationSettingsComponent implements OnInit, OnDestroy {
  checked: boolean;
  private sub: Subscription;

  constructor(private titleService: Title, private profileService: ProfileService, private translate: TranslateService, private spinner: NgxSpinnerService, private messageService: MessageService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Notification Settings | UAE - Open Educational Resources');
    this.checked = false;
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.getNotificationStatus();
    });
    this.getNotificationStatus();
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

  getNotificationStatus() {
    if (this.profileService.userId) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.profileService.GetEmailNotificationStatus(this.profileService.userId).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.checked = res.returnedObject.isEmailNotification;
        } else {
          this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
        }
        this.spinner.hide();
      }, (error) => {
        this.translate.get('Failed to update Record').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast'});
        });
        this.spinner.hide();
      });
    }
  }

  updateNotificationStatus(event) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.profileService.UpdateEmailNotification({
      userId: this.profileService.userId,
      isEmailNotificaiton: event.checked
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.checked = event.checked;
        this.translate.get('Record Updated Successfully').subscribe((result) => {
          this.messageService.add({severity: 'success', summary: result, key: 'toast'});
        });
      } else {
        this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Failed to update Record').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast'});
      });
      this.spinner.hide();
    });
  }

}
