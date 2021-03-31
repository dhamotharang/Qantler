import {Component, OnDestroy, OnInit, SecurityContext} from '@angular/core';
import {environment} from '../../../environments/environment';
import {ProfileService} from '../../services/profile.service';
import {ConfirmationService, MessageService} from 'primeng/api';
import {NgxSpinnerService} from 'ngx-spinner';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {CourseService} from '../../services/course.service';
import {ResourceService} from '../../services/resource.service';
import {Subscription} from 'rxjs';
import {EncService} from '../../services/enc.service';
import {DomSanitizer, Meta, Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html'
})
export class CourseDetailComponent implements OnInit, OnDestroy {
  slug: string;
  course: any;
  courseResources: any;
  courseTest: any;
  userId: number;
  showReportAbuse: boolean;
  favoutireStatus: boolean;
  testDataLoaded: boolean;
  showResourceReportAbuse: boolean[];
  showCommentReportAbuse: boolean;
  showCategories: boolean;
  showShare: boolean;
  courseLoaded: boolean;
  commentSubmitted: boolean;
  CommentCourseForm: FormGroup;
  updateCommentCourseForm: FormGroup[];
  reportCommentId: number;
  showCommentUpdateForm: boolean[];
  StorageUrl: any;
  private sub: Subscription;
  courseEnrolled: boolean;
  enrollmentCheckStatus: boolean;
  readmore: boolean;
  contentReadMore: boolean;
  showRights: boolean;

  constructor(private MetaService: Meta, private titleService: Title, private sanitizer: DomSanitizer, private encService: EncService, private messageService: MessageService,
              private confirmationService: ConfirmationService, private spinner: NgxSpinnerService, private router: Router, private courseService: CourseService,
              private resourceService: ResourceService, private profileService: ProfileService, private route: ActivatedRoute, private translate: TranslateService) {
    this.CommentCourseForm = new FormGroup({
      courseId: new FormControl(null, Validators.required),
      comments: new FormControl(null, Validators.required),
      userId: new FormControl(null, Validators.required),
    });
    this.favoutireStatus = false;
    this.reportCommentId = null;
    this.showCommentUpdateForm = [];
    this.updateCommentCourseForm = [];
    this.showResourceReportAbuse = [];
  }

  ngOnInit() {
    this.titleService.setTitle('UAE - Open Educational Resources');
    this.userId = this.profileService.userId;
    this.CommentCourseForm.patchValue({
      userId: this.userId
    });
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.userId = this.profileService.userId;
      this.getFavouriteStatus();
      this.CommentCourseForm.patchValue({
        userId: this.userId
      });
      if (!this.enrollmentCheckStatus) {
        this.getCourseDetails();
      }
    });
    this.route.params.subscribe(params => {
      this.slug = this.encService.get(params['slug']);
      this.getCourseBySlug(this.slug);
    });
    this.showRights = false;
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }


  getFiltered(array) {
    if (array && array.length > 0) {
      if (array[0].hasOwnProperty('isApproved')) {
        return array.filter(x => x.isActive === x.isApproved === true);
      } else {
        return array.filter(x => x.isActive);
      }
    } else {
      return [];
    }
  }

  getFavouriteStatus() {
    if (this.slug && this.userId > 0) {
      this.profileService.getFavouriteStatus(this.userId, this.slug, 1).subscribe((res) => {
        if (res.hasSucceeded) {
          this.favoutireStatus = true;
        } else {
          this.favoutireStatus = false;
        }
      });
    }
  }

  showcopyrightDetails() {
    this.showRights = true;
  }

  removeFromFavourites() {
    this.translate.get('Are you sure that you want to remove this course from favourites?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.profileService.DeleteUserBookmarkedContent({
            'contentType': 1,
            'contentId': this.slug,
            'userId': this.userId
          }).subscribe((res: any) => {
            if (res.hasSucceeded) {
              this.translate.get('Successfully Removed Course from Favourites').subscribe((msg1) => {
                this.messageService.add({
                  severity: 'success',
                  summary: msg1,
                  key: 'toast',
                  life: 5000
                });
              });
              this.getCourseBySlug(this.slug);
            } else {
              this.translate.get(res.message).subscribe((translation) => {
                this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
              });
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

  addToFavoutites() {
    if (this.userId > 0) {
      this.translate.get('Are you sure that you want to add this course to favourites?').subscribe((msg) => {
        this.confirmationService.confirm({
          message: msg,
          accept: () => {
            this.spinner.show(undefined, {color: this.profileService.themeColor});
            this.profileService.AddUserBookmarkedContent({
              'contentType': 1,
              'contentId': this.course.id,
              'userId': this.userId
            }).subscribe((res: any) => {
              if (res.hasSucceeded) {
                this.translate.get('Successfully Added Course to Favourites').subscribe((trans) => {
                  this.messageService.add({
                    severity: 'success',
                    summary: trans,
                    key: 'toast',
                    life: 5000
                  });
                });
                this.getCourseBySlug(this.slug);
              } else {
                if (res.message === 'Record exists in database') {
                  this.translate.get('Already Added Course to Favourites?').subscribe((trans) => {
                    this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
                  });
                } else {
                  this.translate.get(res.message).subscribe((translation) => {
                    this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
                  });
                }
              }
              this.spinner.hide();
            }, (error) => {
              this.translate.get('Failed to Added Course to Favourites').subscribe((trans) => {
                this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
              });
              this.spinner.hide();
            });
          }
        });
      });
    } else {
      this.translate.get('Please sign in to Continue').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
    }
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  public sanitize(url) {
    return this.sanitizer.sanitize(SecurityContext.URL, url);
  }

  getCourseBySlug(slug) {
    this.getFavouriteStatus();
    this.course = null;
    this.readmore = false;
    this.contentReadMore = false;
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.testDataLoaded = false;
    this.enrollmentCheckStatus = false;
    this.commentSubmitted = false;
    this.courseEnrolled = false;
    this.courseLoaded = false;
    this.course = null;
    this.courseResources = [];
    this.courseTest = false;
    this.showReportAbuse = false;
    this.showCategories = false;
    this.showShare = false;
    this.courseService.getCourseBySlug(slug).subscribe((res) => {
      this.courseLoaded = true;
      if (res.hasSucceeded) {
        this.titleService.setTitle(res.returnedObject.title + ' | UAE - Open Educational Resources');
        this.MetaService.updateTag({name: 'keywords', content: res.returnedObject.keywords});
        this.MetaService.updateTag({
          name: 'description',
          content: res.returnedObject.courseDescription
        });

        this.showCommentUpdateForm = [];
        this.updateCommentCourseForm = [];
        this.showResourceReportAbuse = [];
        this.course = res.returnedObject;
        this.courseResources = this.course.courseSection;
        if (this.courseResources) {
          for (let i = 0; i < this.courseResources.length; i++) {
            this.showResourceReportAbuse.push(false);
          }
        }
        this.CommentCourseForm.patchValue({
          courseId: this.course.id
        });
        this.getCourseDetails();
        if (this.course.courseComments) {
          this.course.courseComments.forEach(() => {
            this.showCommentUpdateForm.push(false);
            this.updateCommentCourseForm.push(new FormGroup({
              id: new FormControl(null, Validators.required),
              courseId: new FormControl(null, Validators.required),
              comments: new FormControl(null, Validators.required),
              userId: new FormControl(null, Validators.required),
            }));
          });
        }
      }
      this.spinner.hide();
    }, (error) => {
      this.courseLoaded = true;
      this.translate.get('Failed to load course').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  filterCategory(item) {
    this.router.navigate(['/courses', {q: item}]);
  }

  getCourseDetails() {
    if (this.userId && this.userId > 0 && this.course && this.course.id) {
      this.enrollmentCheckStatus = true;
      this.getTest();
    } else {
      this.enrollmentCheckStatus = false;
    }
  }

  getTest() {
    this.courseService.postCourseEnrolledStatus({
      courseId: this.course.id,
      userId: this.userId
    }).subscribe((response) => {
      if (response.hasSucceeded) {
        this.courseEnrolled = true;
        this.courseService.getCourseTest(this.course.id).subscribe((res) => {
          this.testDataLoaded = true;
          if (res.hasSucceeded && res.returnedObject.tests.length > 0) {
            this.courseTest = res.returnedObject;
          }
        }, (error) => {
          this.testDataLoaded = true;
        });
      } else {
        this.testDataLoaded = true;
        this.courseEnrolled = false;
      }
    });
  }

  enrollCourse() {
    this.translate.get('Are you sure that you want to enroll into this course?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.courseService.postCourseEnrollment({
            'courseId': this.course.id,
            'userId': this.userId
          }).subscribe((res) => {
            if (res.hasSucceeded) {
              this.getTest();
              this.translate.get('Successfully Enrolled Course').subscribe((msg) => {
                this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
              });
            } else {
              this.translate.get(res.message).subscribe((translation) => {
                this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
              });
            }
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to Enroll course').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
            this.spinner.hide();
          });
        }
      });
    });
  }

  showShareModal() {
    this.showShare = true;
  }

  getCurrentUrl() {
    return environment.clientUrl + this.router.url;
  }

  showCategoriesModal() {
    this.showCategories = true;
  }

  rateCourse() {
    this.getCourseBySlug(this.course.id);
  }

  showReportAbuseModal() {
    this.showReportAbuse = true;
  }

  submitComment() {
    this.commentSubmitted = true;
    if (this.CommentCourseForm.valid) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.courseService.commentCourse(this.CommentCourseForm.value).subscribe((response) => {
        if (response.hasSucceeded) {
          this.translate.get('Successfully commented on course').subscribe((msg) => {
            this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
          });
          this.CommentCourseForm.reset();
          this.CommentCourseForm.patchValue({
            courseId: this.course.id,
            userId: this.userId,
            comments: ''
          });
          this.commentSubmitted = false;
          this.getCourseBySlug(this.slug);
        } else {
          this.translate.get(response.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.translate.get('Failed to comment on course').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      });
    }
  }

  showCommentReportAbuseModal(id) {
    this.showCommentReportAbuse = true;
    this.reportCommentId = id;
  }

  cancelUpdatedComment(i) {
    this.showCommentUpdateForm[i] = false;
    this.reportCommentId = null;
  }

  getReportCommentId() {
    return this.reportCommentId;
  }


  editComment(i) {
    this.showCommentUpdateForm[i] = true;
    this.updateCommentCourseForm[i].patchValue({
      id: this.course.courseComments[i].id,
      courseId: this.course.id,
      comments: this.course.courseComments[i].comments,
      userId: this.userId
    });
  }

  hideComment(comment) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.courseService.hideCourseComment(comment.id, comment.courseId, this.userId).subscribe((response) => {
      if (response.hasSucceeded) {
        this.translate.get('Course Comment Hid Successfully').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.getCourseBySlug(this.slug);
      } else {
        this.translate.get(response.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Failed to hide Course Comment').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }


  deleteComment(comment) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.courseService.deleteCourseComment(comment.id, this.userId).subscribe((response) => {
      if (response.hasSucceeded) {
        this.translate.get('Course Comment Deleted Successfully').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.getCourseBySlug(this.slug);
      } else {
        this.translate.get(response.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Failed to delete Course Comment').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  submitUpdatedComment(i) {
    this.commentSubmitted = true;
    if (this.updateCommentCourseForm[i].valid) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.courseService.updateCommentCourse(this.updateCommentCourseForm[i].value).subscribe((response) => {
        if (response.hasSucceeded) {
          this.translate.get('Successfully commented on course').subscribe((msg) => {
            this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
          });
          this.commentSubmitted = false;
          this.getCourseBySlug(this.slug);
        } else {
          this.translate.get(response.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.translate.get('Failed to comment on course').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      });
    }
  }

  downloadResource() {
    if (this.userId) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.courseService.downloadContentForCourse({
        contentId: this.course.id,
        downloadedBy: this.userId
      }).subscribe((response) => {
        if (response.hasSucceeded) {
          this.translate.get('Starting Download').subscribe((msg) => {
            this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
          });
          const link = document.createElement('a');
          link.href = environment.apiUrl + 'ContentMedia/DownloadCourseFiles/' + this.course.id;
          link.download = this.course.title + '.zip';
          // this is necessary as link.click() does not work on the latest firefox
          link.dispatchEvent(new MouseEvent('click', {bubbles: true, cancelable: true, view: window}));

          setTimeout(function () {
            // For Firefox it is necessary to delay revoking the ObjectURL
            window.URL.revokeObjectURL(link.href);
            link.remove();
          }, 100);
          // this.courseService.downloadContent(this.course.id).subscribe((resultBlob: Blob) => {
          //     const newBlob = new Blob([resultBlob], {type: 'application/zip'});
          //
          //     // IE doesn't allow using a blob object directly as link href
          //     // instead it is necessary to use msSaveOrOpenBlob
          //     if (window.navigator && window.navigator.msSaveOrOpenBlob) {
          //       window.navigator.msSaveOrOpenBlob(newBlob, this.course.title + '.zip');
          //       return;
          //     }
          //     const data = window.URL.createObjectURL(newBlob);
          //
          //     const link = document.createElement('a');
          //     link.href = data;
          //     link.download = this.course.title + '.zip';
          //     // this is necessary as link.click() does not work on the latest firefox
          //     link.dispatchEvent(new MouseEvent('click', {bubbles: true, cancelable: true, view: window}));
          //
          //     setTimeout(function () {
          //       // For Firefox it is necessary to delay revoking the ObjectURL
          //       window.URL.revokeObjectURL(data);
          //       link.remove();
          //     }, 100);
          //   },
          //   (error) => {
          //     console.log('Error downloading the file.');
          //   },
          //   () => {
          //     console.log('OK');
          //   });
        } else {
          this.translate.get(response.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.translate.get('Failed to start download').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      });
    } else {
      this.translate.get('Please sign in to Continue').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  clickreadmore() {
    this.readmore = !this.readmore;
  }

  clickContentReadMore() {
    this.contentReadMore = !this.contentReadMore;
    if (!this.contentReadMore) {
      window.scrollTo(0, 300);
    }
  }
}
