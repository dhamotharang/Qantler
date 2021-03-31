import {Component, OnInit} from '@angular/core';
import {EncService} from '../../services/enc.service';
import {ConfirmationService, MessageService} from 'primeng/api';
import {NgxSpinnerService} from 'ngx-spinner';
import {ProfileService} from '../../services/profile.service';
import {Title} from '@angular/platform-browser';
import {Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-favourites',
  templateUrl: './favourites.component.html'
})
export class FavouritesComponent implements OnInit {
  courses: any;
  resources: any;
  userId: any;

  constructor(public encService: EncService, public router: Router, private titleService: Title, private messageService: MessageService,
              private confirmationService: ConfirmationService, private spinner: NgxSpinnerService,
              private profileService: ProfileService, private translate: TranslateService) {
  }

  ngOnInit() {
    this.titleService.setTitle('My Favourites | UAE - Open Educational Resources');
    this.courses = [];
    this.resources = [];
    this.getList();
    this.userId = this.profileService.userId;
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.getList();
      this.userId = this.profileService.userId;
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

  getList() {
    if (this.profileService.userId) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.profileService.ListUserBookmarkedContent(this.profileService.userId).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.manageList(res.returnedObject);
        } else {
          this.manageList([]);
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
      });
    }
  }

  manageList(list) {
    this.courses = [];
    this.resources = [];
    list.forEach((item) => {
      if (item.contentType === 1) {
        this.courses.push(item);
      } else {
        this.resources.push(item);
      }
    });
  }

  removeSavedCourse(item) {
    this.translate.get('Are you sure that you want to remove this course from favourites?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.profileService.DeleteUserBookmarkedContent({
            'contentType': item.contentType,
            'contentId': item.contentId,
            'userId': this.userId
          }).subscribe((res: any) => {
            if (res.hasSucceeded) {
              this.translate.get('Successfully Removed Course from Favourites').subscribe((msg) => {
                this.messageService.add({
                  severity: 'success',
                  summary: msg,
                  key: 'toast',
                  life: 5000
                });
              });
              this.getList();
            } else {
              this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
            }
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to Remove Course from Favourites').subscribe((msg) => {
              this.messageService.add({
                severity: 'error',
                summary: msg,
                key: 'toast',
                life: 5000
              });
            });
            this.spinner.hide();
          });
        }
      });
    });
  }

  removeSavedResource(item) {
    this.translate.get('Are you sure that you want to remove this resource from favourites?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.profileService.DeleteUserBookmarkedContent({
            'contentType': item.contentType,
            'contentId': item.contentId,
            'userId': this.userId
          }).subscribe((res: any) => {
            if (res.hasSucceeded) {
              this.translate.get('Successfully Removed Resource from Favourites').subscribe((msg) => {
                this.messageService.add({
                  severity: 'success',
                  summary: msg,
                  key: 'toast',
                  life: 5000
                });
              });
              this.getList();
            } else {
              this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
            }
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to Remove Resource from Favourites').subscribe((msg) => {
              this.messageService.add({
                severity: 'error',
                summary: msg,
                key: 'toast',
                life: 5000
              });
            });
            this.spinner.hide();
          });
        }
      });
    });
  }

  removeSavedItem(item) {
    if (item.contentType === 1) {
      this.removeSavedCourse(item);
    } else {
      this.removeSavedResource(item);
    }
  }

}
