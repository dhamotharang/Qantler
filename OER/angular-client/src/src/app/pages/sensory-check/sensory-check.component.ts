import {Component, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {MessageService} from 'primeng/api';
import {QrcService} from '../../services/qrc.service';
import {TranslateService} from '@ngx-translate/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {EncService} from '../../services/enc.service';
import {environment} from '../../../environments/environment';
import {GeneralService} from '../../services/general.service';
import {ResourceService} from '../../services/resource.service';
import {ActivatedRoute, Router} from '@angular/router';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-sensory-check',
  templateUrl: './sensory-check.component.html'
})
export class SensoryCheckComponent implements OnInit {
  categories: any;
  category: any;
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

  constructor(private generalService: ResourceService, private router: Router, private route: ActivatedRoute,
              private profileService: ProfileService, private messageService: MessageService, private QRCService: QrcService,
              private translate: TranslateService, private spinner: NgxSpinnerService, public encService: EncService) {
  }

  ngOnInit() {
    this.pageSize = 10;
    this.pageNumber = 1;
    this.pageStart = 0;
    this.totalRows = 0;
    this.initial = false;
    this.categories = [];
    this.allContent = [];
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

  getLang() {
    return this.translate.currentLang;
  }

  getCategories() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.GetSensitivityCategories(this.profileService.userId).subscribe((res: any) => {
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

  pageChange(event) {
    this.pageNumber = event.page + 1;
    this.pageStart = event.first;
    this.getContent();
  }

  getContent() {
    if (this.category!=null) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.QRCService.SensoryCheckGetContent(this.profileService.userId, this.pageNumber, this.pageSize, this.category).subscribe((res: any) => {
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

  ApproveCourse(item) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.SensoryCheckUpdateContentStatus({
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

  ApproveResource(item) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.QRCService.SensoryCheckUpdateContentStatus({
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

  rejectResource(item) {
    this.tempItem = item;
    this.rejectReason = null;
    this.showRejectModal = true;
  }

  submitRejectionReport() {
    this.submitRejectForm = true;
    if (this.rejectReason && this.rejectReason.length > 0) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.QRCService.SensoryCheckUpdateContentStatus({
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
    if(this.category !=0)
    {
    this.router.navigate(['/verify-content/' + contentType + '/' + contentId + '/' + this.encService.set(0) + '/2/' + this.encService.set(this.category), {
      pn: this.pageNumber,
      ps: this.pageStart
    }]);
    }
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
}

