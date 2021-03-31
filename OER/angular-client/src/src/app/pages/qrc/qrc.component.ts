import {Component, OnDestroy, OnInit} from '@angular/core';
import {QrcService} from '../../services/qrc.service';
import {ProfileService} from '../../services/profile.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {Subscription} from 'rxjs';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';
import {EncService} from '../../services/enc.service';
import {ActivatedRoute, Router} from '@angular/router';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-qrc',
  templateUrl: './qrc.component.html'
})
export class QRCComponent implements OnInit, OnDestroy {
  qrcs: any[];
  categories: any[];
  qrc: number;
  category: number;
  requests: any[];
  showRejectModal: boolean;
  tempItem: any;
  rejectReason: string;
  initial: boolean;
  submitRejectForm: boolean;
  pageSize: number;
  pageNumber: number;
  pageStart: number;
  totalRows: number;
  private sub: Subscription;

  constructor(private titleService: Title, public encService: EncService, private QRCService: QrcService,
              private translate: TranslateService, private profileService: ProfileService,
              private spinner: NgxSpinnerService, private messageService: MessageService
    , private router: Router, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.titleService.setTitle('QRC Management | UAE - Open Educational Resources');
    this.pageSize = 10;
    this.pageNumber = 1;
    this.pageStart = 0;
    this.totalRows = 0;
    this.qrcs = [];
    this.categories = [];
    this.requests = [];
    this.rejectReason = null;
    this.submitRejectForm = false;
    this.tempItem = null;
    this.qrc = null;
    this.category = null;
    this.showRejectModal = false;
    this.initial = false;
    if (this.profileService.user) {
      this.getQrcList(this.profileService.user.id);
    }
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      if (this.profileService.user) {
        this.getQrcList(this.profileService.user.id);
      }
    });
    $(document).ready(function () {
      $('.user-side-panel-head-btn').click(function () {
        $('.user-side-panel ul').stop().slideToggle();
      });
    });

    this.route.params.subscribe((params) => {      
      if (params['c'] && this.encService.get(params['c']) !== 'NA') {
        this.qrc = +this.encService.get(params['c']);
        if (params['ps'] && params['pn']) {
          this.initial = true;
          this.pageStart = +params['ps'];
          this.pageNumber = +params['pn'];
        }
      }
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getQrcList(id) {
    this.QRCService.QrcByUser(id).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.qrcs = res.returnedObject;
        if (!this.qrc) {
          this.qrc = this.qrcs.length > 0 ? this.qrcs[0].id : null;
        }
        this.getCategoryByQRC(this.qrc);
        this.category = null;
      }
    });
  }

  getCategoryByQRC(qrcId) {
    this.qrc = qrcId;
    if (this.qrc) {
      this.QRCService.getCategoryByQRC(this.qrc).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.categories = res.returnedObject;
          this.category = this.categories[0].id;
          if (!this.initial) {
            if (!(this.qrc && this.qrcs.find(x => x.id === this.qrc))) {
              this.pageNumber = 1;
              this.pageStart = 0;
            } else {
              if (this.totalRows - 1 === this.pageSize * (this.pageNumber - 1)) {
                this.pageNumber--;
                this.pageStart = this.pageSize * (this.pageNumber - 1);
              }
            }
          } else {
            this.initial = false;
          }
          this.totalRows = 0;
          this.getQRCContent();
        }
      });
    }
  }

  getCategory(category) {
    this.category = category;
    if(this.category==0)
    this.qrc = 0;
    else
    this.qrc = this.qrcs.find(x => x.categoryId === category).id;
    this.pageSize = 10;
    this.pageNumber = 1;
    this.pageStart = 0;
    this.totalRows = 0;
    this.getQRCContent();
  }

  pageChange(event) {
    this.pageNumber = event.page + 1;
    this.pageStart = event.first;
    this.getQRCContent();
  }

  getQRCContent() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    if (this.qrc && this.category || this.qrc==0 && this.category==0) {
      this.QRCService.GetContent({
        qrcId: +this.qrc,
        categoryId: +this.category,
        userId: this.profileService.user.id,
        pageNo: this.pageNumber,
        pageSize: this.pageSize
      }).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.requests = res.returnedObject;
          if (this.requests.length > 0) {
            this.totalRows = this.requests[0].totalRows;
          } else {
            this.totalRows = 0;
          }
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
      });
    }
  }

  ApproveResource(item) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.UpdateContentStatus({
      contentApprovalId: item.contentApprovalId,
      contentType: item.contentType,
      status: 1,
      comment: '',
      createdBy: this.profileService.user.id,
      contentId: item.contentId,
      emailUrl: environment.clientUrl + (item.contentType === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully Approved Resource').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.getQrcList(this.profileService.user.id);
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
    this.QRCService.UpdateContentStatus({
      contentApprovalId: item.contentApprovalId,
      contentType: item.contentType,
      status: 1,
      comment: '',
      createdBy: this.profileService.user.id,
      contentId: item.contentId,
      emailUrl: environment.clientUrl + (item.contentType === 1 ? '/dashboard/courses' : '/dashboard/resources')
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.translate.get('Successfully Approved Resource').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
        this.getQrcList(this.profileService.user.id);
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
      this.translate.get('Failed to Approve Resource').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    });
  }

  rejectResource(item) {
    this.tempItem = item;
    this.rejectReason = null;
    this.showRejectModal = true;
  }

  rejectCourse(item) {
    this.tempItem = item;
    this.rejectReason = null;
    this.showRejectModal = true;
  }

  closeRejectionReason() {
    this.tempItem = null;
    this.rejectReason = null;
    this.showRejectModal = false;
  }

  submitRejectionReport() {
    this.submitRejectForm = true;
    if (this.rejectReason && this.rejectReason.length > 0) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.QRCService.UpdateContentStatus({
        contentApprovalId: this.tempItem.contentApprovalId,
        contentType: this.tempItem.contentType,
        status: 0,
        comment: this.rejectReason,
        createdBy: this.profileService.user.id,
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
          this.getQrcList(this.profileService.user.id);
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


  navigateToVerify(contentType, contentId, contentApprovalId) {
    if(this.category !=0)
    this.router.navigate(['/verify-content/' + contentType + '/' + contentId + '/' + this.encService.set(contentApprovalId) + '/0/' + this.encService.set(this.qrc), {
      pn: this.pageNumber,
      ps: this.pageStart
    }]);
    else{
      if(contentType==1){
        this.router.navigate(['/course/'  + this.encService.set(contentId) , {
        }]);
      }
      else{
        this.router.navigate(['/resource/'  + this.encService.set(contentId) , {
        }]);
      }
      
    }
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

}
