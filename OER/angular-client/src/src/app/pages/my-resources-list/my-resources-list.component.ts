import {Component, OnDestroy, OnInit} from '@angular/core';
import {ResourceService} from '../../services/resource.service';
import {ActivatedRoute} from '@angular/router';
import {Subscription} from 'rxjs';
import {NgxSpinnerService} from 'ngx-spinner';
import {ProfileService} from '../../services/profile.service';
import {ConfirmationService, MessageService} from 'primeng/api';
import {CourseService} from '../../services/course.service';
import {EncService} from '../../services/enc.service';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';
import {StorageUploadService} from '../../services/storage-upload.service';
import {environment} from '../../../environments/environment';
import {HttpRequest} from '@angular/common/http';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-my-resources-list',
  templateUrl: './my-resources-list.component.html'
})
export class MyResourcesListComponent implements OnInit, OnDestroy {
  resources: any;
  private sub: Subscription;
  type: string;

  constructor(private titleService: Title, public encService: EncService, private translate: TranslateService, private profileService: ProfileService, private messageService: MessageService, private route: ActivatedRoute, private spinner: NgxSpinnerService, private confirmationService: ConfirmationService, private resourceService: ResourceService, protected uploadService: StorageUploadService) {
  }

  ngOnInit() {
    this.titleService.setTitle('My Resources | UAE - Open Educational Resources');
    this.type = 'draft';
    this.getResources();
    this.route.params.subscribe((params) => {
      if (params['type'] === 'draft' || params['type'] === 'submitted' || params['type'] === 'published') {
        this.type = params['type'];
      }
    });
    this.sub = this.profileService.getUserDataUpdate().subscribe((res) => {
      this.getResources();
    });
    $(document).ready(function () {
      $('.user-side-panel-head-btn').click(function () {
        $('.user-side-panel ul').stop().slideToggle();
      });
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getResources() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.profileService.getResources().subscribe((res) => {
      if (res.hasSucceeded) {
        this.spinner.hide();
        this.resources = res.returnedObject;
      }
    }, (error) => {
      this.spinner.hide();
    });
  }

  deleteResource(id) {    
    const objectNames = [];
    this.resourceService.getResourceBySlug(id).subscribe((res) => {
      if (res.hasSucceeded) {        
        var thumbnailUrl = res.returnedObject[0].thumbnail;
        if(thumbnailUrl != null)
        {
           objectNames.push(this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length));
        }
        var associatedFiles = res.returnedObject[0].resourceFiles;
        if( associatedFiles !=null)
        {
          for(let i=0;i<associatedFiles.length;i++){
            objectNames.push( this.uploadService.resourceFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length));
          }
        }
      }
      });
    this.translate.get('Are you sure that you want to perform this action?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.resourceService.deleteResourceBySlug(id).subscribe((res) => {
            if (res.hasSucceeded) {
              this.translate.get('Successfully deleted resource').subscribe((msg) => {
                this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});                   
                this.uploadService.upload(new HttpRequest('DELETE', environment.apiUrl + 'ContentMedia/FilesDelete', objectNames,
                {
                  reportProgress: true
                })).subscribe();
              });
            } else {
              this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
            }
            this.getResources();
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to delete resource').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
          });
        }
      });
    });
  }

  withDrawResource(id) {
    this.translate.get('Are you sure that you want to perform this action?').subscribe((trans) => {
      this.confirmationService.confirm({
        message: trans,
        accept: () => {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.resourceService.contentWithdrawal(id).subscribe((res) => {
            if (res.hasSucceeded) {
              this.getResources();
              this.translate.get('Successfully withdrawn resource, moved to Drafts').subscribe((msg) => {
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
            this.translate.get('Failed to withdraw resource').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
          });
        }
      });
    });
  }

  getDraftCount() {
    if (this.resources) {
      return this.resources.filter(x => x.isDraft && !x.isApproved).length;
    }
    return 0;
  }

  getSubmittedCount() {
    if (this.resources) {
      return this.resources.filter(x => !x.isDraft && !x.isApproved).length;
    }
    return 0;
  }

  getApprovedCount() {
    if (this.resources) {
      return this.resources.filter(x => !x.isDraft && x.isApproved).length;
    }
    return 0;
  }


}
