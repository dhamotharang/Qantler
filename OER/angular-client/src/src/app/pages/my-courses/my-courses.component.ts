import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs';
import {ResourceService} from '../../services/resource.service';
import {ActivatedRoute} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {ProfileService} from '../../services/profile.service';
import {CourseService} from '../../services/course.service';
import {ConfirmationService, MessageService} from 'primeng/api';
import {EncService} from '../../services/enc.service';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';
import {StorageUploadService} from '../../services/storage-upload.service';
import {environment} from '../../../environments/environment';
import {HttpRequest} from '@angular/common/http';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-my-courses',
  templateUrl: './my-courses.component.html'
})
export class MyCoursesComponent implements OnInit, OnDestroy {

  courses: any;
  private sub: Subscription;
  type: string;

  constructor(private titleService: Title, public encService: EncService, private translate: TranslateService, private profileService: ProfileService, private messageService: MessageService, private courseService: CourseService, private route: ActivatedRoute, private spinner: NgxSpinnerService, private confirmationService: ConfirmationService,protected uploadService: StorageUploadService) {
  }

  ngOnInit() {
    this.titleService.setTitle('My Courses | UAE - Open Educational Resources');
    this.type = 'draft';
    this.getCourses();
    this.route.params.subscribe((params) => {
      if (params['type'] === 'draft' || params['type'] === 'submitted' || params['type'] === 'published') {
        this.type = params['type'];
      }
    });
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.getCourses();
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

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getCourses() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.profileService.getCourses().subscribe((res) => {
      if (res.hasSucceeded) {
        this.spinner.hide();
        this.courses = res.returnedObject;
      }
    }, (error) => {
      this.spinner.hide();
    });
  }

  deleteCourse(id) {
    const objectNames = [];
    this.courseService.getCourseBySlug(id).subscribe((res) => {
      if (res.hasSucceeded) {        
        var thumbnailUrl = res.returnedObject.thumbnail;
        if(thumbnailUrl != null)
        {
           objectNames.push(this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length));
        }
        var associatedFiles = res.returnedObject.associatedFiles;
        if( associatedFiles !=null)
        {
          for(let i=0;i<associatedFiles.length;i++){
            objectNames.push(this.uploadService.courseFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length));
          }
        }
      }
    });
    this.translate.get('Are you sure that you want to perform this action?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.courseService.deleteCourseBySlug(id).subscribe((res) => {
            if (res.hasSucceeded) {
              this.translate.get('Successfully deleted course').subscribe((msg) => {
                this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});                
                this.uploadService.upload(new HttpRequest('DELETE', environment.apiUrl + 'ContentMedia/FilesDelete', objectNames,
                {
                  reportProgress: true
                })).subscribe();
              });
            } else {
              this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
            }
            this.getCourses();
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to delete course').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
          });
        }
      });
    });
  }

  getDraftCount() {
    if (this.courses) {
      return this.courses.filter(x => x.isDraft && !x.isApproved).length;
    }
    return 0;
  }

  getSubmittedCount() {
    if (this.courses) {
      return this.courses.filter(x => !x.isDraft && !x.isApproved).length;
    }
    return 0;
  }

  getApprovedCount() {
    if (this.courses) {
      return this.courses.filter(x => !x.isDraft && x.isApproved).length;
    }
    return 0;
  }

  withDrawCourse(id) {
    this.translate.get('Are you sure that you want to perform this action?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.courseService.contentWithdrawal(id).subscribe((res) => {
            if (res.hasSucceeded) {
              this.getCourses();
              this.translate.get('Successfully withdrawn course, moved to Drafts').subscribe((msg) => {
                this.messageService.add({
                  severity: 'success',
                  summary: msg,
                  key: 'toast',
                  life: 5000
                });
              });
            } else {
              this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
            }
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to withdraw course').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
          });
        }
      });
    });
  }

}
