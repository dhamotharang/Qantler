import {Component, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {Subscription} from 'rxjs';
import {MessageService} from 'primeng/api';
import {NgxSpinnerService} from 'ngx-spinner';
import {TranslateService} from '@ngx-translate/core';
import {Title} from '@angular/platform-browser';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-announcements',
  templateUrl: './announcements.component.html'
})
export class AnnouncementsComponent implements OnInit {
  private sub: Subscription;
  Announcements: any;
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  errorCount: number;

  constructor(private titleService: Title, private profileService: ProfileService, private translate: TranslateService, private messageService: MessageService, private spinner: NgxSpinnerService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Announcements | UAE - Open Educational Resources');
    this.errorCount = 0;
    this.Announcements = [];
    this.pageNumber = 1;
    this.pageSize = 10;
    this.totalCount = 0;
    this.getAnnouncements(this.pageNumber, this.pageSize);
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.getAnnouncements(this.pageNumber, this.pageSize);
    });
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

  getAnnouncements(pageNumber, pageSize) {
    if (this.profileService.userId && this.errorCount < 2) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.profileService.getAnnouncementsByUser(this.profileService.userId, pageNumber, pageSize)
        .subscribe((res: any) => {
          if (res.hasSucceeded) {
            this.errorCount = 0;
            this.spinner.hide();
            this.Announcements = res.returnedObject;
            if (this.Announcements.length > 0) {
              this.totalCount = this.Announcements[0].totalRows;
            }
            if (this.profileService.userId && this.profileService.userId > 0 && this.pageNumber === 1) {
              this.profileService.postLogin(this.profileService.userId).subscribe();
            }
          } else {
            this.errorCount++;
            this.spinner.hide();
            if (this.profileService.userId && this.profileService.userId > 0 && res.message === 'Server Error') {
              this.profileService.postLogin(this.profileService.userId).subscribe(() => {
                this.getAnnouncements(this.pageNumber, this.pageSize);
              });
            } else {
              this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
            }
          }
        }, (error) => {
          this.errorCount++;
          this.spinner.hide();
          this.translate.get('Failed to retrieve Announcements').subscribe((trans) => {
            this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
          });
        });
    }
  }

  getFilteredAnnouncements() {
    return this.Announcements.filter(x => x.active === true);
  }

  getStyle(item) {
    if (Date.parse(item.lastLogin) < Date.parse(item.updatedOn)) {
      return 'latest';
    } else {
      return '';
    }
  }

  // getStyle(item) {
  //   if (Date.parse(item.lastLogin) < Date.parse(item.updatedOn)) {
  //     return {'background-color': '#ba9a3a'};
  //   } else {
  //     return {};
  //   }
  // }

  paginate(event) {
    this.pageNumber = event.page + 1;
    this.getAnnouncements(this.pageNumber, this.pageSize);
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }


}
