import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators, AbstractControl} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {ProfileService} from '../../services/profile.service';
import {WCMService} from '../../services/wcm.service';


@Component({
  selector: 'app-wcmmanagement',
  templateUrl: './wcmmanagement.component.html',
  styleUrls: ['./wcmmanagement.component.css']
})
export class WCMManagementComponent implements OnInit {
  Pages: any[];
  showUpdateContent: boolean;
  init: any;
  Page: any;
  pageName:string;
  ContentForm: FormGroup;
  type: any;
  init1: any;
  catList:any;
  categoryList: any;
  catEnd: any;
  length: number =0;
  catStart: number;

  constructor(private WCMservice: WCMService, private route: ActivatedRoute, private spinner: NgxSpinnerService,
    private profileService: ProfileService, private messageService: MessageService, private confirmationService: ConfirmationService) {
    this.init = {
      skin_url: '/tinymce/skins/ui/oxide',
      skin: 'oxide',
      theme: 'silver',
      plugins:
      'print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen image link media template\
      codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern help',
      toolbar:
      'formatselect | bold italic strikethrough forecolor backcolor permanentpen formatpainter | link image media pageembed | alignleft\
      aligncenter alignright alignjustify  | numlist bullist outdent indent | removeformat | addcomment',
      image_advtab: true,
      height: 400,
      template_cdate_format: '[CDATE: %m/%d/%Y : %H:%M:%S]',
      template_mdate_format: '[MDATE: %m/%d/%Y : %H:%M:%S]',
      image_caption: true,
      directionality: 'rtl'
    };
    this.init1 = {
      skin_url: '/tinymce/skins/ui/oxide',
      skin: 'oxide',
      theme: 'silver',
      plugins:
      'print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen image link media template\
      codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern help',
      toolbar:
      'formatselect | bold italic strikethrough forecolor backcolor permanentpen formatpainter | link image media pageembed | alignleft\
      aligncenter alignright alignjustify ltr rtl | numlist bullist outdent indent | removeformat | addcomment',
      image_advtab: true,
      height: 400,
      template_cdate_format: '[CDATE: %m/%d/%Y : %H:%M:%S]',
      template_mdate_format: '[MDATE: %m/%d/%Y : %H:%M:%S]',
      image_caption: true,
      directionality: 'rtl',
      formats: {
        alignright:{selector : 'ol,ul', styles : {'padding-inline-end': '40px','text-align':'right'}}
      }
    };
    // @ts-ignore
    window.tinyMCE.overrideDefaults({
      base_url: '/tinymce/',  // Base for assets such as skins, themes and plugins
      suffix: '.min'          // This will make Tiny load minified versions of all its assets
    });
  }

  ngOnInit() {
    this.Pages = [];
    this.showUpdateContent = false;
    this.getPages();
    this.Page = null;
    this.ContentForm = new FormGroup({
      content: new FormControl(null, [Validators.required, Validators.maxLength(100000)]),
      content_Ar: new FormControl(null, [Validators.required, Validators.maxLength(100000)]),
      videolink: new FormControl(null)
    });
  }

  getPages() {
    this.spinner.show();
    const categoryId: number =0;
    this.WCMservice.getAllList(0).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.Pages = res.returnedObject;
        this.length = this.Pages.length;
        this.catEnd = this.Pages.length;
          this.showCat(this.Pages);
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
    });
  }

  editContent(page) {
    this.pageName = page.pageName;
    this.Page = page;
    this.spinner.show();
    if(page.pageName == 'Video Section')
    {
      this.ContentForm.controls["videolink"].setValidators([Validators.required,Validators.pattern('^(http(s)?:\/\/)?((w){3}.)?(youtube|vimeo)?(\.com)?\/.+$')]);
      this.ContentForm.controls["content_Ar"].setValidators([Validators.required, Validators.maxLength(100000)]);
    }
    this.WCMservice.getContent(page.id).subscribe((res: any) => {
      if (res.hasSucceeded) {
        if (res.returnedObject.length > 0) {
          this.ContentForm.patchValue({
            content: res.returnedObject[0].pageContent,
            content_Ar: res.returnedObject[0].pageContent_Ar,
            videolink: res.returnedObject[0].videoLink
          });
          this.type = 'put';
        } else {
          this.type = 'post';
        }
        this.showUpdateContent = true;
      }
      this.spinner.hide();
    }, (error) => {
      this.spinner.hide();
    });
  }

  UpdateContent() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to Update?',
      accept: () => {
        if (this.ContentForm.valid) {
          this.spinner.show();
          if (this.type === 'put') {
            this.WCMservice.patchContent({
              pageID: this.Page.id,
              pageContent: this.ContentForm.value.content,
              pageContent_Ar: this.ContentForm.value.content_Ar,
              videolink: this.ContentForm.value.videolink,
              createdBy: this.profileService.user.id,
              updatedBy: this.profileService.user.id
            }).subscribe((res: any) => {
              if (res.hasSucceeded) {
                this.messageService.add({severity: 'success', summary: 'Successfully Updated Content', key: 'toast'});
                this.showUpdateContent = false;
                this.Page = null;
                this.ContentForm.reset();
              } else {
                this.messageService.add({severity: 'error', summary: 'failed to update Content', key: 'toast'});
              }
              this.spinner.hide();
            }, (error) => {
              this.messageService.add({severity: 'error', summary: 'failed to update Content', key: 'toast'});
              this.spinner.hide();
            });
          } else if (this.type === 'post') {
            this.WCMservice.postContent({
              pageID: this.Page.id,
              pageContent: this.ContentForm.value.content,
              pageContent_Ar: this.ContentForm.value.content_Ar,
              videolink: this.ContentForm.value.videolink,
              createdBy: this.profileService.user.id,
              updatedBy: this.profileService.user.id
            }).subscribe((res: any) => {
              if (res.hasSucceeded) {
                this.messageService.add({severity: 'success', summary: 'Successfully Updated Content', key: 'toast'});
                this.showUpdateContent = false;
                this.Page = null;
                this.ContentForm.reset();
              } else {
                this.messageService.add({severity: 'error', summary: 'failed to update Content', key: 'toast'});
              }
              this.spinner.hide();
            }, (error) => {
              this.messageService.add({severity: 'error', summary: 'failed to update Content', key: 'toast'});
              this.spinner.hide();
            });
          }
        }
      }
    });
  }

  cancelUpdate() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to cancel?',
      accept: () => {
        this.showUpdateContent = false;
        this.Page = null;
        this.ContentForm.reset();
      }
    });
  }

  showCat(catgory, start = 0) {
    const end = (this.catEnd - start) > 10 ? 10 : (this.catEnd - start);
    this.catList = [];
    for (let i = 0; i < end; i++) {
      this.catList[i] = catgory[start + i];
    }
  }

  paginateCat(event) {
    this.showCat(this.Pages, event.first);
  }
}


