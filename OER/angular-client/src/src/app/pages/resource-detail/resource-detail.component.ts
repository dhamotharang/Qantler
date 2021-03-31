import {Component, ElementRef, OnDestroy, OnInit, SecurityContext, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ResourceService} from '../../services/resource.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {ProfileService} from '../../services/profile.service';
import {Subscription} from 'rxjs';
import {EncService} from '../../services/enc.service';
import {DomSanitizer, Meta, Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-resource-detail',
  templateUrl: './resource-detail.component.html'
})
export class ResourceDetailComponent implements OnInit, OnDestroy {
  slug: string;
  resource: any;
  userId: number;
  showRemixResource: boolean;
  remixResource: any;
  showReportAbuse: boolean;
  showRemixReportAbuse: boolean;
  showCommentReportAbuse: boolean;
  showCategories: boolean;
  showShare: boolean;
  resourceLoaded: boolean;
  commentSubmitted: boolean;
  CommentResourceForm: FormGroup;
  resourceAlignmentRatingForm: FormGroup;
  updateCommentResourceForm: FormGroup[];
  reportCommentId: number;
  showCommentUpdateForm: boolean[];
  StorageUrl: any;
  showAlignmentRating: boolean;
  alignmentFormSubmitted: boolean;
  categories: any;
  showRights: boolean;
  favoutireStatus: boolean;
  levels: any;
  @ViewChild('content') content: ElementRef;
  public resolution: number;
  private sub: Subscription;
  readmore: boolean;
  objectiveReadmore: boolean;
  contentReadMore: boolean;

  constructor(private MetaService: Meta, private titleService: Title, private translate: TranslateService, private confirmationService: ConfirmationService, private sanitizer: DomSanitizer, private encService: EncService, private route: ActivatedRoute, public router: Router, private resourceService: ResourceService, private profileService: ProfileService, private spinner: NgxSpinnerService, private messageService: MessageService) {
    this.CommentResourceForm = new FormGroup({
      resourceId: new FormControl(null, Validators.required),
      comments: new FormControl(null, Validators.required),
      userId: new FormControl(null, Validators.required),
    });
    this.resourceAlignmentRatingForm = new FormGroup({
      resourceId: new FormControl(null, Validators.required),
      categoryId: new FormControl(null),
      levelId: new FormControl(null),
      rating: new FormControl(null, Validators.required),
      ratedBy: new FormControl(null, Validators.required),
    });
    this.reportCommentId = null;
    this.showCommentUpdateForm = [];
    this.updateCommentResourceForm = [];
    this.showAlignmentRating = false;
    this.alignmentFormSubmitted = false;
    this.showRemixResource = false;
    this.favoutireStatus = false;
    this.remixResource = null;
    this.levels = [];
  }

  ngOnInit() {
    this.titleService.setTitle('UAE - Open Educational Resources');
    this.userId = this.profileService.userId;
    this.CommentResourceForm.patchValue({
      userId: this.userId
    });
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.userId = this.profileService.userId;
      this.getFavouriteStatus();
      this.CommentResourceForm.patchValue({
        userId: this.userId
      });
    });
    this.showRights = false;
    this.setResolution();
    this.readmore = false;
    this.objectiveReadmore = false;
    this.commentSubmitted = false;
    this.resourceLoaded = false;
    this.route.params.subscribe(params => {
      this.slug = this.encService.get(params['slug']);
      this.getResourceBySlug(this.slug);
    });
    this.getCategories();
    this.showReportAbuse = false;
    this.showCategories = false;
    this.showShare = false;
    this.resource = null;

  }

  getFavouriteStatus() {
    if (this.slug && this.userId > 0) {
      this.profileService.getFavouriteStatus(this.userId, this.slug, 2).subscribe((res) => {
        if (res.hasSucceeded) {
          this.favoutireStatus = true;
        } else {
          this.favoutireStatus = false;
        }
      });
    }
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

  removeFromFavourites() {
    this.translate.get('Are you sure that you want to perform this action?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.profileService.DeleteUserBookmarkedContent({
            'contentType': 2,
            'contentId': this.slug,
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
              this.getResourceBySlug(this.slug);
            } else {
              this.translate.get(res.message).subscribe((translation) => {
                this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
              });
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

  addToFavoutites() {
    if (this.userId > 0) {
      this.translate.get('Are you sure that you want to add this resource to favourites?').subscribe((trans) => {
        this.confirmationService.confirm({
          message: trans,
          accept: () => {
            this.spinner.show(undefined, {color: this.profileService.themeColor});
            this.profileService.AddUserBookmarkedContent({
              'contentType': 2,
              'contentId': this.resource.id,
              'userId': this.userId
            }).subscribe((res: any) => {
              if (res.hasSucceeded) {
                this.translate.get('Successfully Added Resource to Favourites').subscribe((msg) => {
                  this.messageService.add({
                    severity: 'success',
                    summary: msg,
                    key: 'toast',
                    life: 5000
                  });
                });
                this.getResourceBySlug(this.slug);
              } else {
                if (res.message === 'Record exists in database') {
                  this.translate.get('Already Added Resource to Favourites').subscribe((msg) => {
                    this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
                  });
                } else {
                  this.translate.get(res.message).subscribe((translation) => {
                    this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
                  });
                }
              }
              this.spinner.hide();
            }, (error) => {
              this.translate.get('Failed to Added Resource to Favourites').subscribe((msg) => {
                this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
              });
              this.spinner.hide();
            });
          }
        });
      });
    } else {
      this.translate.get('Please sign in to Continue').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  filterCategory(item) {
    this.router.navigate(['/resources', {q: item}]);
  }

  public sanitize(url) {
    return this.sanitizer.sanitize(SecurityContext.URL, url);
  }

  public setResolution(value?: number): void {
    this.resolution = value;
  }

  getCategories() {
    this.resourceService.getResourceMasterData().subscribe((res) => {
      if (res.hasSucceeded) {
        this.categories = res.returnedObject.categoryMasterData;
        this.levels = res.returnedObject.level;
      }
    });
  }

  getRating() {
    return this.resource.rating;
  }

  getAlignmentRating() {
    return this.resource.alignmentRating;
  }

  getResourceBySlug(slug) {
    this.resource = null;
    this.readmore = false;
    this.contentReadMore = false;
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.getFavouriteStatus();
    this.resourceService.getResourceBySlug(slug).subscribe((res) => {
      this.resourceLoaded = true;
      if (res.hasSucceeded) {
        this.titleService.setTitle(res.returnedObject[0].title + ' | UAE - Open Educational Resources');
        this.MetaService.updateTag({name: 'keywords', content: res.returnedObject[0].keywords});
        this.MetaService.updateTag({
          name: 'description',
          content: res.returnedObject[0].resourceDescription
        });
        this.showCommentUpdateForm = [];
        this.updateCommentResourceForm = [];
        this.resource = res.returnedObject[0];
        this.CommentResourceForm.patchValue({
          resourceId: this.resource.id
        });
        if (this.resource.resourceComments) {
          this.resource.resourceComments.forEach(() => {
            this.showCommentUpdateForm.push(false);
            this.updateCommentResourceForm.push(new FormGroup({
              id: new FormControl(null, Validators.required),
              resourceId: new FormControl(null, Validators.required),
              comments: new FormControl(null, Validators.required),
              userId: new FormControl(null, Validators.required),
            }));
          });
        }
      }
      this.spinner.hide();
    }, (error) => {
      this.resourceLoaded = true;
      this.translate.get('Failed to load resource').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  editComment(i) {
    this.showCommentUpdateForm[i] = true;
    this.updateCommentResourceForm[i].patchValue({
      id: this.resource.resourceComments[i].id,
      resourceId: this.resource.id,
      comments: this.resource.resourceComments[i].comments,
      userId: this.userId
    });
  }

  hideComment(comment) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.resourceService.hideResourceComment(comment.id, comment.resourceId, this.userId).subscribe((response) => {
      if (response.hasSucceeded) {
        this.translate.get('Resource Comment Hid Successfully').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.getResourceBySlug(this.slug);
      } else {
        this.translate.get(response.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Failed to hide Resource Comment').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  deleteComment(comment) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.resourceService.deleteResourceComment(comment.id, this.userId).subscribe((response) => {
      if (response.hasSucceeded) {
        this.translate.get('Resource Comment Deleted Successfully').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.getResourceBySlug(this.slug);
      } else {
        this.translate.get(response.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Failed to delete Resource Comment').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  submitUpdatedComment(i) {
    this.commentSubmitted = true;
    if (this.updateCommentResourceForm[i].valid) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.resourceService.updateCommentResource(this.updateCommentResourceForm[i].value).subscribe((response) => {
        if (response.hasSucceeded) {
          this.translate.get('Successfully commented on resource').subscribe((msg) => {
            this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
          });
          this.commentSubmitted = false;
          this.getResourceBySlug(this.slug);
        } else {
          this.translate.get(response.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.translate.get('Failed to comment on resource').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      });
    }
  }

  cancelUpdatedComment(i) {
    this.showCommentUpdateForm[i] = false;
    this.reportCommentId = null;
  }

  showReportAbuseModal() {
    this.showReportAbuse = true;
  }

  showCommentReportAbuseModal(id) {
    this.showCommentReportAbuse = true;
    this.reportCommentId = id;
  }

  getReportCommentId() {
    return this.reportCommentId;
  }

  showCategoriesModal() {
    this.showCategories = true;
  }

  showShareModal() {
    this.showShare = true;
  }

  getCurrentUrl() {
    return environment.clientUrl + this.router.url;
  }

  submitComment() {
    this.commentSubmitted = true;
    if (this.CommentResourceForm.valid) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.resourceService.commentResource(this.CommentResourceForm.value).subscribe((response) => {
        if (response.hasSucceeded) {
          this.translate.get('Successfully commented on resource').subscribe((msg) => {
            this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
          });
          this.CommentResourceForm.reset();
          this.CommentResourceForm.patchValue({
            resourceId: this.resource.id,
            userId: this.userId,
            comments: ''
          });
          this.commentSubmitted = false;
          this.getResourceBySlug(this.slug);
        } else {
          this.translate.get(response.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.translate.get('Failed to comment on resource').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      });
    }
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

  rateResource() {
    this.getResourceBySlug(this.resource.id);
  }

  rateAlignment(event) {
    if (this.userId) {
      this.resourceAlignmentRatingForm.patchValue({
        resourceId: this.resource.id,
        rating: event,
        ratedBy: this.userId,
        categoryId: 0,
        levelId: 0
      });
      if (event <= 3) {
        this.showAlignmentRating = true;
      } else {
        this.PostAlignmentRating(this.resourceAlignmentRatingForm.value);
      }
    } else {
      this.translate.get('Please sign in to Continue').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  SubmitAlignmentRating(data) {
    if (this.userId) {
      this.alignmentFormSubmitted = true;
      if (this.resourceAlignmentRatingForm.valid) {
        if (data.rating > 3) {
          this.PostAlignmentRating(data);
        } else {
          if (data.categoryId > 0 && data.levelId > 0) {
            this.PostAlignmentRating(data);
          }
        }
      }
    } else {
      this.translate.get('Please sign in to Continue').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  PostAlignmentRating(data) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.resourceService.rateResourceAlignment(data).subscribe((res) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully rated resource').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
      this.alignmentFormSubmitted = false;
      this.showAlignmentRating = false;
      this.resourceAlignmentRatingForm.reset();
      this.getResourceBySlug(this.slug);
    }, (error) => {
      this.translate.get('Failed to rate resource').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
      this.getResourceBySlug(this.slug);
    });
  }

  downloadResource() {
    if (this.userId) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.resourceService.downloadContentForResource({
        contentId: this.resource.id,
        downloadedBy: this.userId
      }).subscribe((response) => {
        if (response.hasSucceeded) {
          this.translate.get('Starting Download').subscribe((msg) => {
            this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
          });
          const link = document.createElement('a');
          link.href = environment.apiUrl + 'ContentMedia/DownloadResourceFiles/' + this.resource.id;
          link.download = this.resource.title + '.zip';
          // this is necessary as link.click() does not work on the latest firefox
          link.dispatchEvent(new MouseEvent('click', {bubbles: true, cancelable: true, view: window}));

          setTimeout(function () {
            // For Firefox it is necessary to delay revoking the ObjectURL
            window.URL.revokeObjectURL(link.href);
            link.remove();
          }, 100);
          // this.resourceService.downloadContent(this.resource.id).subscribe((resultBlob: Blob) => {
          //     const newBlob = new Blob([resultBlob], {type: 'application/zip'});
          //
          //     // IE doesn't allow using a blob object directly as link href
          //     // instead it is necessary to use msSaveOrOpenBlob
          //     if (window.navigator && window.navigator.msSaveOrOpenBlob) {
          //       window.navigator.msSaveOrOpenBlob(newBlob, this.resource.title + '.zip');
          //       return;
          //     }
          //     const data = window.URL.createObjectURL(newBlob);
          //
          //     const link = document.createElement('a');
          //     link.href = data;
          //     link.download = this.resource.title + '.zip';
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

  makeRemixResource() {
    if (!this.resource.copyRight.protected) {
      this.router.navigate(['/resources/create'], {queryParams: {remix: this.encService.set(this.resource.id)}});
    } else {
      this.translate.get('Resource is Copyright Protected').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  showRemixedResource() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.resourceService.getRemixVersion(this.resource.id).subscribe((res) => {
      if (res.hasSucceeded) {
        this.remixResource = res.returnedObject[0];
        this.showRemixResource = true;
      } else {
        this.translate.get('Failed to load remix resource').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Failed to load remix resource').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  showcopyrightDetails() {
    this.showRights = true;
  }

  clickreadmore() {
    this.readmore = !this.readmore;
  }

  clickObjectivereadmore() {
    this.objectiveReadmore = !this.objectiveReadmore;
  }

  clickContentReadMore() {
    this.contentReadMore = !this.contentReadMore;
    if (!this.contentReadMore) {
      window.scrollTo(0, 300);
    }
  }

  openInNewTab(link: string) {
    window.open(link, '_blank');
  }
}
