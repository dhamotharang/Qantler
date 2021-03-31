import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {environment} from '../../../environments/environment';
import {ResourceService} from '../../services/resource.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {CourseService} from '../../services/course.service';
import {EncService} from '../../services/enc.service';
import {Title} from '@angular/platform-browser';
import {ProfileService} from '../../services/profile.service';
import {QrcService} from '../../services/qrc.service';
import {TranslateService} from '@ngx-translate/core';
import {MessageService} from 'primeng/api';

@Component({
  selector: 'app-verify-content',
  templateUrl: './verify-content.component.html'
})
export class VerifyContentComponent implements OnInit {
  type: number;
  id: number;
  contentApprovalId: number;
  checkType: number;
  submitRejectForm: boolean;
  rejectReason: string;
  category: string;
  resource: any;
  course: any;
  showRejectModal: boolean;
  dataLoadFailed: boolean;
  testAvaialble: boolean;
  showRights: boolean;
  courseResources: any;
  courseTest: any;
  pageStart: number;
  pageNumber: number;

  constructor(private titleService: Title, private messageService: MessageService, public encService: EncService, private translate: TranslateService, private QRCService: QrcService, private profileService: ProfileService, private router: Router, private route: ActivatedRoute, private courseService: CourseService, private resourceService: ResourceService, private spinner: NgxSpinnerService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Verify Content | UAE - Open Educational Resources');
    this.courseResources = [];
    this.courseTest = null;
    this.dataLoadFailed = false;
    this.showRejectModal = false;
    this.submitRejectForm = false;
    this.rejectReason = '';
    this.showRights = false;
    this.testAvaialble = false;
    this.resource = null;
    this.course = null;
    this.type = null;
    this.id = null;
    this.pageNumber = 1;
    this.pageStart = 0;
    this.category = null;
    this.route.params.subscribe(params => {
      this.type = +params['type'];
      this.id = +params['id'];
      this.category = params['cat'];
      this.checkType = params['checkType'] ? +params['checkType'] : 0;
      this.contentApprovalId = +this.encService.get(params['contentApprovalId']);
      if (params['ps']) {
        this.pageStart = params['ps'];
      }
      if (params['pn']) {
        this.pageNumber = params['pn'];
      }
      this.getContent(this.type, this.id);
    });
  }

  getContent(content, id) {
    if (+content === 1) {
      this.getCourseContent(id);
    } else if (+content === 2) {
      this.getResourceContent(id);
    }
  }

  showcopyrightDetails() {
    this.showRights = true;
  }


  getCourseTest(id) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.courseService.getCourseTest(+id).subscribe((res) => {
      if (res.hasSucceeded && res.returnedObject.tests.length > 0) {
        this.testAvaialble = true;
        this.courseTest = res.returnedObject;
      } else {
        this.testAvaialble = false;
      }
      this.spinner.hide();
    }, (error) => {
      this.testAvaialble = false;
      this.spinner.hide();
    });
  }

  getCourseResources(id) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.courseService.getResourceByCourseId(+id).subscribe((res) => {
      if (res.hasSucceeded) {
        this.courseResources = res.returnedObject;
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
    });
  }

  getCourseContent(id) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.courseService.getCourseBySlug(+id).subscribe((res) => {
      if (res.hasSucceeded) {
        this.dataLoadFailed = false;
        this.course = res.returnedObject;
        this.getCourseTest(this.course.id);
        this.getCourseResources(this.course.id);
      } else {
        this.dataLoadFailed = true;
      }
      this.spinner.hide();
    }, (error) => {
      this.dataLoadFailed = true;
      this.spinner.hide();
    });
  }

  getResourceContent(id) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.resourceService.getResourceBySlug(+id).subscribe((res) => {
      if (res.hasSucceeded) {
        this.dataLoadFailed = false;
        this.resource = res.returnedObject[0];
      } else {
        this.dataLoadFailed = true;
      }
      this.spinner.hide();
    }, (error) => {
      this.dataLoadFailed = true;
      this.spinner.hide();
    });
  }

  approveItem(item) {
    if (item === 'course') {
      if (this.contentApprovalId > 0) {
        this.ApproveCourse();
      } else {
        if (this.checkType === 1) {
          this.approveCommunityItem(1, '');
        } else if (this.checkType === 2) {
          this.approveSensitivityItem(1, '');
        } else if (this.checkType === 3) {
          this.approveExpertItem(1, '');
        }
      }
    } else {
      if (this.contentApprovalId > 0) {
        this.ApproveResource();
      } else {
        if (this.checkType === 1) {
          this.approveCommunityItem(1, '');
        } else if (this.checkType === 2) {
          this.approveSensitivityItem(1, '');
        } else if (this.checkType === 3) {
          this.approveExpertItem(1, '');
        }
      }
    }
  }

  getFiltered(array) {
    if (array && array.length > 0) {
      return array.filter(x => x.isActive);
    } else {
      return [];
    }
  }

  goToQRC() {
    this.router.navigate(['/dashboard/qrc', {c: this.category, ps: this.pageStart, pn: this.pageNumber}]);
  }

  gotoCommunity() {
    this.router.navigate(['/dashboard/community-check', {c: this.category, ps: this.pageStart, pn: this.pageNumber}]);
  }

  gotoSensitivity() {
    this.router.navigate(['/dashboard/sensitivity-check', {c: this.category, ps: this.pageStart, pn: this.pageNumber}]);
  }

  gotoExpert() {
    this.router.navigate(['/dashboard/expert-check', {c: this.category, ps: this.pageStart, pn: this.pageNumber}]);
  }

  approveCommunityItem(status, reason) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.CommunityUpdateContentStatus({
      contentType: this.type,
      status: status,
      comments: reason,
      userId: this.profileService.user.id,
      contentId: this.id,
      emailUrl: environment.clientUrl + (this.type === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        if (status) {
          if (this.type === 1) {
            this.translate.get('Successfully Approved Course').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          } else {
            this.translate.get('Successfully Approved Resource').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          }
        } else {
          if (this.type === 1) {
            this.translate.get('Successfully Rejected Course').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          } else {
            this.translate.get('Successfully Rejected Resource').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          }
        }
        this.gotoCommunity();
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
      if (status) {
        if (this.type === 1) {
          this.translate.get('Failed to Approve Course').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        } else {
          this.translate.get('Failed to Approve Resource').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        }
      } else {
        if (this.type === 1) {
          this.translate.get('Failed to Reject Course').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        } else {
          this.translate.get('Failed to Reject Resource').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        }
      }
    });
  }

  approveSensitivityItem(status, reason) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.SensoryCheckUpdateContentStatus({
      contentType: this.type,
      status: status,
      comments: reason,
      userId: this.profileService.user.id,
      contentId: this.id,
      emailUrl: environment.clientUrl + (this.type === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        if (status) {
          if (this.type === 1) {
            this.translate.get('Successfully Approved Course').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          } else {
            this.translate.get('Successfully Approved Resource').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          }
        } else {
          if (this.type === 1) {
            this.translate.get('Successfully Rejected Course').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          } else {
            this.translate.get('Successfully Rejected Resource').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          }
        }
        this.gotoSensitivity();
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
      if (status) {
        if (this.type === 1) {
          this.translate.get('Failed to Approve Course').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        } else {
          this.translate.get('Failed to Approve Resource').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        }
      } else {
        if (this.type === 1) {
          this.translate.get('Failed to Reject Course').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        } else {
          this.translate.get('Failed to Reject Resource').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        }
      }
    });
  }

  approveExpertItem(status, reason) {

    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.MOEUpdateContentStatus({
      contentType: this.type,
      status: status,
      comments: reason,
      userId: this.profileService.user.id,
      contentId: this.id,
      emailUrl: environment.clientUrl + (this.type === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        if (status) {
          if (this.type === 1) {
            this.translate.get('Successfully Approved Course').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          } else {
            this.translate.get('Successfully Approved Resource').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          }
        } else {
          if (this.type === 1) {
            this.translate.get('Successfully Rejected Course').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          } else {
            this.translate.get('Successfully Rejected Resource').subscribe((msg) => {
              this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
            });
          }
        }
        this.gotoExpert();
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
      if (status) {
        if (this.type === 1) {
          this.translate.get('Failed to Approve Course').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        } else {
          this.translate.get('Failed to Approve Resource').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        }
      } else {
        if (this.type === 1) {
          this.translate.get('Failed to Reject Course').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        } else {
          this.translate.get('Failed to Reject Resource').subscribe((msg) => {
            this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
          });
        }
      }
    });
  }

  ApproveResource() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.UpdateContentStatus({
      contentApprovalId: this.contentApprovalId,
      contentType: this.type,
      status: 1,
      comment: '',
      createdBy: this.profileService.user.id,
      contentId: this.id,
      emailUrl: environment.clientUrl + (this.type === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully Approved Resource').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.goToQRC();
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Failed to Approve Resource').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  ApproveCourse() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.UpdateContentStatus({
      contentApprovalId: this.contentApprovalId,
      contentType: this.type,
      status: 1,
      comment: '',
      createdBy: this.profileService.user.id,
      contentId: this.id,
      emailUrl: environment.clientUrl + (this.type === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully Approved Course').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.goToQRC();
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
      this.translate.get('Failed to Approve Course').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    });
  }

  rejectItem(item) {
    this.showRejectModal = true;
  }

  closeRejectionReason() {
    this.showRejectModal = false;
    this.submitRejectForm = false;
    this.rejectReason = '';
  }

  submitRejectionReport() {
    this.submitRejectForm = true;
    if (this.rejectReason && this.rejectReason.length > 0) {
      if (this.contentApprovalId > 0) {
        this.RejectQRC();
      } else {
        if (this.checkType === 1) {
          this.approveCommunityItem(0, this.rejectReason);
        } else if (this.checkType === 2) {
          this.approveSensitivityItem(0, this.rejectReason);
        } else if (this.checkType === 3) {
          this.approveExpertItem(0, this.rejectReason);
        }
      }
    }
  }

  RejectQRC() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.UpdateContentStatus({
      contentApprovalId: this.contentApprovalId,
      contentType: this.type,
      status: 0,
      comment: this.rejectReason,
      createdBy: this.profileService.user.id,
      contentId: this.id,
      emailUrl: environment.clientUrl + (this.type === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully Rejected ' + (this.type === 1 ? 'Course' : 'Resource')).subscribe((msg) => {
          this.messageService.add({
            severity: 'success',
            summary: msg,
            key: 'toast',
            life: 5000
          });
        });
        this.closeRejectionReason();
        this.goToQRC();
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
      this.translate.get('Failed to Reject ' + (this.type === 1 ? 'Course' : 'Resource')).subscribe((msg) => {
        this.messageService.add({
          severity: 'error',
          summary: msg,
          key: 'toast',
          life: 5000
        });
      });
    });
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }
}
