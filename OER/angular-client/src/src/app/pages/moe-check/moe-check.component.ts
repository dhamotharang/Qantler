import {Component, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {QrcService} from '../../services/qrc.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {EncService} from '../../services/enc.service';
import {environment} from '../../../environments/environment';
import {TranslateService} from '@ngx-translate/core';
import {MessageService} from 'primeng/api';
import {ActivatedRoute, Router} from '@angular/router';
import {ResourceService} from '../../services/resource.service';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-moe-check',
  templateUrl: './moe-check.component.html'
})
export class MoeCheckComponent implements OnInit {
  categories: any;
  category: any;
  translations: any;
  allContent: any[];
  requests: any;
  tempItem: any;
  rejectReason: string;
  showRejectModal: boolean;
  submitRejectForm: boolean;
  selection: string;
  initial: boolean;
  pageSize: number;
  pageNumber: number;
  pageStart: number;
  totalRows: number;

  constructor(private profileService: ProfileService, private messageService: MessageService, private generalService: ResourceService,
              private QRCService: QrcService, private translate: TranslateService, private spinner: NgxSpinnerService,
              private router: Router, public encService: EncService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.pageSize = 10;
    this.pageNumber = 1;
    this.pageStart = 0;
    this.totalRows = 0;
    this.initial = false;
    this.categories = [];
    this.allContent = [];
    this.translations = [];
    this.requests = [];
    this.showRejectModal = false;
    this.submitRejectForm = false;
    if (this.profileService.userId > 0) {
      this.getCategories();
    }
    this.profileService.getUserDataUpdate().subscribe(() => {
      if (this.profileService.userId > 0) {
        this.getCategories();
      }
    });
    this.route.params.subscribe((params) => {
      if (params['c'] && this.encService.get(params['c']) !== 'NA') {
        this.category = +this.encService.get(params['c']);
        this.selection = this.category;
        if (params['ps'] && params['pn']) {
          this.initial = true;
          this.pageStart = +params['ps'];
          this.pageNumber = +params['pn'];
        }
      }
    });
    $(document).ready(function () {
      $('.user-side-panel-head-btn').click(function () {
        $('.user-side-panel ul').stop().slideToggle();
      });
    });
  }

  checkActive(item) {
    if (this.categories.find(x => x.id === item)) {
      return true;
    }
    return false;
  }

  getTCName(item) {
    return this.translate.currentLang === 'en' ? item.name : item.name_Ar;
  }

  pageChange(event) {
    this.pageNumber = event.page + 1;
    this.pageStart = event.first;
    if (this.selection === 'approved') {
      this.getApprovedContent();
    } else {
      this.getContent();
    }
  }

  getCategories() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.GetExpertCategories(this.profileService.userId).subscribe((res: any) => {
      if (res.hasSucceeded) {
        if (res.returnedObject.length > 0) {
          this.categories = res.returnedObject;
          if (!this.initial) {
            if (!(this.category && this.categories.find(x => x.id === this.category))) {
              this.pageNumber = 1;
              this.pageStart = 0;
              this.category = (this.categories && this.categories.length) > 0 ? this.categories[0].id : null;
              this.selection = this.category;
            } else {
              if (this.totalRows - 1 === this.pageSize * (this.pageNumber - 1)) {
                this.pageNumber--;
                this.pageStart = this.pageSize * (this.pageNumber - 1);
              }
            }
          } else {
            this.initial = false;
          }
        } else {
          this.categories = [];
        }
        this.getContent();
      } else {
        this.categories = [];
      }
      this.spinner.hide();
    });
  }

  getContent() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.MOEGetContent(this.profileService.userId, this.pageNumber, this.pageSize, this.category).subscribe((res: any) => {
      if (res.hasSucceeded) {
        if (res.returnedObject.length > 0) {
          this.requests = res.returnedObject;
          this.totalRows = this.requests[0].totalrows;
        } else {
          this.requests = [];
          this.totalRows = 0;
        }
      } else {
        this.requests = [];
        this.totalRows = 0;
      }
      this.spinner.hide();
    });
  }

  getCategory(category) {
    this.category = category;
    this.selection = category;
    this.pageSize = 10;
    this.pageNumber = 1;
    this.pageStart = 0;
    this.totalRows = 0;
    this.getContent();
  }

  getApproved() {
    this.selection = 'approved';
    this.pageNumber = 1;
    this.pageStart = 0;
    this.getApprovedContent();
  }

  getApprovedContent() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.MOEGetApprovedListByUser(this.profileService.userId, this.pageNumber, this.pageSize).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.requests = res.returnedObject;
        this.totalRows = this.requests[0].totalrows
        ;
      } else {
        this.requests = [];
        this.totalRows = 0;
      }
      this.spinner.hide();
    });
  }

  ApproveResource(item) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.MOEUpdateContentStatus({
      contentType: item.contentType,
      status: 1,
      comments: '',
      userId: this.profileService.user.id,
      contentId: item.contentId,
      emailUrl: environment.clientUrl + (item.contentType === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully Approved Resource').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.getCategories();
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

  ApproveCourse(item) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.MOEUpdateContentStatus({
      contentType: item.contentType,
      status: 1,
      comments: '',
      userId: this.profileService.user.id,
      contentId: item.contentId,
      emailUrl: environment.clientUrl + (item.contentType === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully Approved Course').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.getCategories();
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

  rejectCourse(item) {
    this.tempItem = item;
    this.rejectReason = null;
    this.showRejectModal = true;
  }

  rejectResource(item) {
    this.tempItem = item;
    this.rejectReason = null;
    this.showRejectModal = true;
  }

  submitRejectionReport() {
    this.submitRejectForm = true;
    if (this.rejectReason && this.rejectReason.length > 0) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.QRCService.MOEUpdateContentStatus({
        contentType: this.tempItem.contentType,
        status: 0,
        comments: this.rejectReason,
        userId: this.profileService.user.id,
        contentId: this.tempItem.contentId,
        emailUrl: environment.clientUrl + (this.tempItem.contentType === 1 ? '/dashboard/courses' : '/dashboard/resources')
      }).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.translate.get('Successfully Rejected item').subscribe((msg) => {
            this.messageService.add({
              severity: 'success',
              summary: msg,
              key: 'toast',
              life: 5000
            });
          });
          this.closeRejectionReason();
          this.getCategories();
        } else {
          this.translate.get(res.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.translate.get('Failed to Reject item').subscribe((msg) => {
          this.messageService.add({
            severity: 'error',
            summary: msg,
            key: 'toast',
            life: 5000
          });
        });
      });
    }
  }

  closeRejectionReason() {
    this.tempItem = null;
    this.rejectReason = null;
    this.showRejectModal = false;
  }


  navigateToVerify(contentType, contentId) {
    this.router.navigate(['/verify-content/' + contentType + '/' + contentId + '/' + this.encService.set(0) + '/3/' + this.encService.set(this.category), {
      pn: this.pageNumber,
      ps: this.pageStart
    }]);
  }

}

