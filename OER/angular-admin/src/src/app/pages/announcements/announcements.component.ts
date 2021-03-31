import {Component, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {StorageUploadService} from '../../services/storage-upload.service'; 
import {AnnouncementsService} from '../../services/announcements.service';
import {ProfileService} from '../../services/profile.service';
import {HttpEventType, HttpRequest, HttpResponse} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import { Paginator } from 'primeng/components/paginator/paginator';
import _ from 'lodash'
/**
 * component used to create and update announcements
 */
@Component({
  selector: 'app-announcements',
  templateUrl: './announcements.component.html',
  styleUrls: ['./announcements.component.css']
})
export class AnnouncementsComponent implements OnInit {
  user: any;
  AnnouncementForm: FormGroup;
  Announcements: any;
  showAnnouncementForm: boolean;
  AnnouncementFormSubmitted: boolean;
  patchId: number;
  init: any;
  init1: any;
  searchKeyword  : string = "";
  sortType: string = 'desc';
  sortField : number = 0;
  Clickfield :string = '';
  spanColNo : any;
  searchbackup : any;

  @ViewChild('pp') paginator: Paginator;
  /**
   * initialize tinyMCE
   *
   * @param confirmationService contains the confirmationService pattern
   * @param announcementService contains the announcementService pattern
   * @param profileService contains the profileService pattern
   * @param spinner contains the spinner pattern
   * @param messageService contains the messageService pattern
   */
  constructor(private confirmationService: ConfirmationService, private announcementService: AnnouncementsService,protected uploadService: StorageUploadService,
    private profileService: ProfileService, private spinner: NgxSpinnerService,
    private messageService: MessageService) {
    this.init = {
      skin_url: '/tinymce/skins/ui/oxide',
      skin: 'oxide',
      theme: 'silver',
      plugins:
      'print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen image link media template\
      codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern help image code',
      toolbar:
      'formatselect | bold italic strikethrough forecolor backcolor permanentpen formatpainter | link image media pageembed | alignleft\
      aligncenter alignright alignjustify  | numlist bullist outdent indent | removeformat | addcomment undo redo | image code',
      image_advtab: false,
      height: 400, 
      template_cdate_format: '[CDATE: %m/%d/%Y : %H:%M:%S]',
      template_mdate_format: '[MDATE: %m/%d/%Y : %H:%M:%S]',
      image_caption: true,
      directionality: 'rtl',
      images_upload_url: 'postAcceptor.php',
      images_upload_handler: (blobInfo, success, failure)=> {
        console.log(blobInfo.blob());
        console.log(blobInfo.filename());
        if (blobInfo) {
          const formdata = new FormData();
          formdata.append(this.uploadService.Announcements + Date.now() + this.profileService.makeid(10) + '.' +blobInfo.filename(), blobInfo.blob());
          this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFilesAsFormData', formdata,
            {
              reportProgress: true
            })).subscribe(event => {
              debugger;
              console.log(event.body);
              if(event.body)
              {
              if(event.body.hasSucceed)
              {
                success(event.body.fileUrl);
              }
              else if(!event.body.hasSucceed)
              {
                success('');
              }
            }
            },(error) => {
              console.log(error);
        });
        }
      }
    };
    this.init1 = {
      skin_url: '/tinymce/skins/ui/oxide',
      skin: 'oxide',
      theme: 'silver',
      plugins:
      'print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen image link media template\
      codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern help image code',
      toolbar:
      'formatselect | bold italic strikethrough forecolor backcolor permanentpen formatpainter | link image media pageembed | alignleft\
      aligncenter alignright alignjustify ltr rtl | numlist bullist outdent indent | removeformat | addcomment undo redo | image code',
      image_advtab: false,
      height: 400,
      template_cdate_format: '[CDATE: %m/%d/%Y : %H:%M:%S]',
      template_mdate_format: '[MDATE: %m/%d/%Y : %H:%M:%S]',
      image_caption: true,
      directionality: 'rtl',
      formats: {
        alignright:{selector : 'ol,ul', styles : {'padding-inline-end': '40px','text-align':'right'}}
      },
      images_upload_url: 'postAcceptor.php',
      images_upload_handler: (blobInfo, success, failure)=> {
        debugger;
        console.log(blobInfo.blob());
        console.log(blobInfo.filename());
        if (blobInfo) {
          const formdata = new FormData();
          formdata.append(this.uploadService.Announcements + Date.now() + this.profileService.makeid(10) + '.' +blobInfo.filename(), blobInfo.blob());
          this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFilesAsFormData', formdata,
            {
              reportProgress: true
            })).subscribe(event => {
              debugger;
              console.log(event.body);
              if(event.body)
              {
              if(event.body.hasSucceed)
              {
                success(event.body.fileUrl);
              }
              else if(!event.body.hasSucceed)
              {
                success('');
              }
            }
            },(error) => {
              console.log(error);
        });
        }
      }
    };

    // @ts-ignore
    window.tinyMCE.overrideDefaults({
      base_url: '/tinymce/',  // Base for assets such as skins, themes and plugins
      suffix: '.min'          // This will make Tiny load minified versions of all its assets
    });
  }

  ngOnInit() {
    this.Announcements = [];
    this.showAnnouncementForm = false;
    this.AnnouncementFormSubmitted = false;
    this.patchId = null;
    this.AnnouncementForm = new FormGroup({
      id: new FormControl(null),
      text: new FormControl(null, [Validators.required, Validators.maxLength(10000)]),
      text_Ar: new FormControl(null, [Validators.required, Validators.maxLength(10000)]),
      updatedBy: new FormControl(0, Validators.required),
      createdBy: new FormControl(0, Validators.required),
      active: new FormControl(false, Validators.required)
    });
    this.user = this.profileService.user;
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.user = this.profileService.user;
    });
    this.getAnnouncements();
  }

  getAnnouncements() {
    this.spinner.show();
    this.announcementService.getAnnouncements()
      .subscribe((response: any) => {
        if (response.hasSucceeded) {
          if (response.returnedObject && response.returnedObject.length > 0) {
            this.Announcements = response.returnedObject.sort((a, b): number => {
              if (Date.parse(a.createdOn) > Date.parse(b.createdOn)) { return -1; }
              if (Date.parse(a.createdOn) < Date.parse(b.createdOn)) { return 1; }
              return 0;
            });
            this.searchbackup = this.Announcements
          } else if (response.returnedObject.length === 0) {
            this.Announcements = [];
            this.searchbackup = [];
          }
          this.spinner.hide();
        } else {
          this.messageService.add({severity: 'error', summary: response.message, key: 'toast', life: 5000});
          this.spinner.hide();
        }
      }, (error) => {
        this.messageService.add({severity: 'error', summary: 'Failed to load Data', key: 'toast', life: 5000});
        this.spinner.hide();
      });
  }

  editAnnouncement(Announcement) {
    this.AnnouncementForm.reset();
    if (this.user) {
      this.patchId = Announcement.id;
      this.AnnouncementForm.patchValue({
        id: this.patchId,
        text: Announcement.text,
        text_Ar: Announcement.text_Ar,
        createdBy: this.user.id,
        updatedBy: this.user.id,
        active: Announcement.active
      });
      window.scrollTo(0, 0);
      this.showAnnouncementForm = true;
    } else {
      this.messageService.add({severity: 'warning', summary: 'Please Wait... Loading data', key: 'toast', life: 5000});
    }
  }

  AddAnnouncement() {
    this.AnnouncementForm.reset();
    if (this.user) {
      this.AnnouncementForm.patchValue({
        updatedBy: this.user.id,
        createdBy: this.user.id,
      });
      window.scrollTo(0, 0);
      this.showAnnouncementForm = true;
    } else {
      this.messageService.add({severity: 'warning', summary: 'Please Wait... Loading data', key: 'toast', life: 5000});
    }
  }

  cancelForm() {
    this.showAnnouncementForm = false;
    this.AnnouncementFormSubmitted = false;
    this.patchId = null;
    this.AnnouncementForm.reset();
    window.scrollTo(0, 0);
  }

  submitForm(Form) {
    this.AnnouncementFormSubmitted = true;
    if (Form.valid) {
      this.spinner.show();
      this.announcementService.postAnnouncements({
        text: Form.value.text,
        text_Ar: Form.value.text_Ar,
        createdBy: Form.value.createdBy,
        active: Form.value.active
      }).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.showAnnouncementForm = false;
          this.AnnouncementFormSubmitted = false;
          this.AnnouncementForm.reset();
          this.messageService.add({
            severity: 'success',
            summary: 'Announcement Added Successfully',
            key: 'toast',
            life: 5000
          });
          this.getAnnouncements();
        } else {
          this.messageService.add({severity: 'error', summary: res.message, key: 'toast', life: 5000});
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.messageService.add({severity: 'warning', summary: 'Failed to Add Announcement', key: 'toast', life: 5000});
      });
    } else {
      this.messageService.add({severity: 'error', summary: 'Please fill all fields', key: 'toast', life: 5000});
    }
  }

  updateForm(Form) {
    this.AnnouncementFormSubmitted = true;
    if (Form.valid) {
      this.spinner.show();
      this.announcementService.putAnnouncements({
        id: this.patchId,
        text: Form.value.text,
        text_Ar: Form.value.text_Ar,
        updatedBy: Form.value.updatedBy,
        active: Form.value.active
      }).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.showAnnouncementForm = false;
          this.AnnouncementFormSubmitted = false;
          this.AnnouncementForm.reset();
          this.patchId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'Announcement Updated Successfully',
            key: 'toast',
            life: 5000
          });
          this.getAnnouncements();
        } else {
          this.messageService.add({severity: 'error', summary: res.message, key: 'toast', life: 5000});
        }
        this.spinner.hide();
      }, (error) => {
        this.messageService.add({
          severity: 'warning',
          summary: 'Failed to Update Announcement',
          key: 'toast',
          life: 5000
        });
        this.spinner.hide();
      });
    } else {
      this.messageService.add({severity: 'error', summary: 'Please fill all fields', key: 'toast', life: 5000});
    }
  }

  search(){
    debugger;
    const versiondata = [];
    this.Announcements = this.searchbackup;
    var userdata = this.Announcements;
    var s = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    if(s == "" ) {
      this.searchKeyword = "";
      this.Clickfield = "";
      this.getAnnouncements();
    } else {
      userdata &&  userdata.length
      ?  userdata.map(function mm(i) {
        if (i.text.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
        else if (i.text_Ar.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.createdBy.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.createdBy.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.createdOn.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.updatedBy.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.updatedOn.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if ("Active".toLowerCase().includes(s.toLowerCase())) {
            if(i.active) {
              versiondata.push(i);
            }
           }
           else if ("Inactive".toLowerCase().includes(s.toLowerCase())) {
            if(!i.active) {
              versiondata.push(i);
            }
           }
        }) : [];
        userdata = versiondata;
        this.Announcements = userdata;
    }
  }

  Clickingevent(event,columnNo) {
    debugger;
    var userdata = this.Announcements;
    if(this.Clickfield == event.target.id) {
      this.sortType = this.sortType == 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.sortType = 'asc'
    }
    this.sortField = columnNo;
  
    this.spanColNo = this.sortType+"-"+columnNo;
    var sortedArray = _.sortBy(userdata, function(patient) {
      if(event.target.id == 'announcement'){
        return patient.text;
      }
      else if(event.target.id == 'announcementarabic'){
        return patient.text_Ar;
      }
      else if(event.target.id == 'createdby'){
        return patient.createdBy;
      }
      else if(event.target.id == 'createdon'){
        return patient.createdOn;
      }
      else if(event.target.id == 'updatedby'){
        return patient.updatedBy;
      }
      else if(event.target.id == 'updatedon'){
        return patient.updatedOn;
      }
      else if(event.target.id == 'status'){
        return patient.active;
      }
  });
  if(this.sortType == 'desc') {
    sortedArray =   sortedArray.reverse();
  }
    this.Announcements = sortedArray;
    }

  clearSearch(){
  
    this.searchKeyword = "";
    this.Clickfield = "";
    this.getAnnouncements();
  }

  deleteAnnouncement(Announcement) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this Announcement?',
      accept: () => {
        this.spinner.show();
        this.announcementService.deleteAnnouncements(Announcement.id).subscribe((res: any) => {
          if (res.hasSucceeded) {
            this.messageService.add({
              severity: 'success',
              summary: 'Announcement Deleted Successfully',
              key: 'toast',
              life: 5000
            });
            this.getAnnouncements();
          } else {
            this.messageService.add({severity: 'error', summary: res.message, key: 'toast', life: 5000});
          }
          this.spinner.hide();
        }, (error) => {
          this.messageService.add({
            severity: 'warning',
            summary: 'Failed to Delete Announcement',
            key: 'toast',
            life: 5000
          });
          this.spinner.hide();
        });
      }
    });
  }
}
