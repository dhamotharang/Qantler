import {Component, OnInit} from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {ActivatedRoute, Router} from '@angular/router';
import {ResourceService} from '../../services/resource.service';
import {StorageUploadService} from '../../services/storage-upload.service';
import {environment} from '../../../environments/environment';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {MessageService} from 'primeng/api';
import {ProfileService} from '../../services/profile.service';
import {TranslateService} from '@ngx-translate/core';
import {EncService} from '../../services/enc.service';
import {HttpEventType, HttpRequest, HttpResponse} from '@angular/common/http';
import {Title} from '@angular/platform-browser';
import {WCMService} from '../../services/wcm.service';
import { Options } from 'ng5-slider';

@Component({
  selector: 'app-create-resource',
  templateUrl: './create-resource.component.html'
})
export class CreateResourceComponent implements OnInit {

  view: string;
  imgURL: any;
  categories: any;
  subCategories: any;
  copyrights: any;
  filteredSubCategories: any;
  ResourceForm: FormGroup;
  defaultImage = 'assets/images/default-thumb.png';
  thumbnailImageUploadStatus: string;
  filteredSubjectsMultiple: any[];
  keyWordSuggestions: any[];
  filteredkeyWordSuggestions: any[];
  profileImageUploadPercentage: number;
  tempFile: any;
  resourceFiles: {
    name: string,
    originalName: string,
    status: string,
    progress: number
  }[];
  Requests: any[];
  showWhitelistUrlRequest: boolean;
  showCopyRightList: boolean;
  selectedCopyRight: any;
  resourceFormSubmitted: boolean;
  resourceFormDrafted: boolean;
  init: any;
  materials: any;
  slug: any;
  whatTime: any;
  readtime: Date;
  storageUrl: string;
  educationalStandards: any[];
  educationalUses: any[];
  levels: any[];
  terms: boolean;
  termsContent: string;
  termsContent_Ar: string;
  removedFilesUrl: string;
  optionsEn: Options = {
    floor: 0,
    ceil: 600 ,
    step: 1,
    showSelectionBar:true,
    showSelectionBarEnd : false ,
    getSelectionBarColor:function() {
      return '#1a3464';
},
    getPointerColor:function() {
          return '#1a3464';
  }
  };
  optionsAr: Options = {
    floor: 0,
    ceil: 600,
    step: 1,
    showSelectionBar:true,
    showSelectionBarEnd : false ,
    rightToLeft:true,
    getSelectionBarColor:function() {
         return '#1a3464';
      },
    getPointerColor:function() {
      return '#1a3464';   
  }
  };
  optionsThemeEn: Options = {
    floor: 0,
    ceil: 600,
    step: 1,
    showSelectionBar:true,
    showSelectionBarEnd : false,
    getSelectionBarColor:function() {
      return '#ba9a3a';
},
    getPointerColor:function() {
          return '#ba9a3a';
  }
  };
  optionsThemeAr: Options = {
    floor: 0,
    ceil: 600,
    step: 1,
    showSelectionBar:true,
    showSelectionBarEnd : false ,
    rightToLeft:true,
    getSelectionBarColor:function() {
      return '#ba9a3a';
},
    getPointerColor:function() {
          return '#ba9a3a';
  }
  };
  constructor(private titleService: Title, private WcmService: WCMService, private encService: EncService, private translate: TranslateService, private route: ActivatedRoute, protected uploadService: StorageUploadService, private profileService: ProfileService, private messageService: MessageService, private spinner: NgxSpinnerService, private router: Router, private resourceService: ResourceService) {

    this.ResourceForm = new FormGroup({
      title: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(250)])),
      categoryId: new FormControl(null, Validators.compose([Validators.required])),
      subCategoryId: new FormControl(null, Validators.compose([Validators.required])),
      materialTypeId: new FormControl(null, Validators.compose([Validators.required])),
      resourceDescription: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(2000)])),
      thumbnail: new FormControl(null, Validators.compose([Validators.required])),
      tempThumbnail: new FormControl(null, Validators.compose([])),
      keywords: new FormControl(false, Validators.compose([Validators.maxLength(10)])),
      resourceContent: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(1000000000)])),
      readingTime: new FormControl(null, Validators.compose([Validators.required, Validators.min(4)])),
      resourceFiles: new FormControl([]),
      isDraft: new FormControl(true),
      references: new FormControl([]),
      copyRightId: new FormControl(null, Validators.compose([Validators.required])),
      educationalStandardId: new FormControl(null, Validators.compose([Validators.required])),
      educationalUseId: new FormControl(null, Validators.compose([Validators.required])),
      levelId: new FormControl(null, Validators.compose([Validators.required])),
      objective: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(2000)])),
      agreed: new FormControl(false, Validators.compose([Validators.required])),
      tempFile: new FormControl(null),
      tempUrl: new FormControl(null),
      createdBy: new FormControl(null),
      resourceSourceId: new FormControl(0),
      emailUrl: new FormControl(environment.clientUrl + '/dashboard/qrc')
    });
  }

  ngOnInit() {
    this.titleService.setTitle('Create Resource | UAE - Open Educational Resources');
    window.scrollTo(0, 0);
    this.termsContent = '';
    this.termsContent_Ar = '';
    this.educationalStandards = [];
    this.educationalUses = [];
    this.levels = [];
    this.subCategories = [];
    this.categories = [];
    this.copyrights = [];
    this.materials = [];
    this.removedFilesUrl='';
    this.initial();
    this.getMasterData();
    this.route.params.subscribe(params => {
      if (params['slug']) {
        this.slug = +this.encService.get(params['slug']);
        this.getResourceBySlug(this.slug);
      }
    });
    this.route.queryParams.subscribe(queryParams => {
      if (queryParams['remix']) {
        this.ResourceForm.patchValue({
          resourceSourceId: +this.encService.get(queryParams['remix'])
        });
        this.getResourceBySlug(+this.encService.get(queryParams['remix']));
      }
    });
    this.init = {
      skin_url: '/tinymce/skins/ui/oxide',
      skin: 'oxide',
      theme: 'silver',
      plugins: 'print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern help',
      toolbar: 'formatselect | bold  italic strikethrough forecolor backcolor permanentpen formatpainter  | alignleft aligncenter alignright alignjustify ltr rtl | numlist bullist outdent indent | removeformat | addcomment',
      image_advtab: true,
      height: 400,
      template_cdate_format: '[CDATE: %m/%d/%Y : %H:%M:%S]',
      template_mdate_format: '[MDATE: %m/%d/%Y : %H:%M:%S]',
      image_caption: true,
      formats: {
        alignright:{selector : 'ol,ul', styles : {'padding-inline-end': '40px','text-align':'right'}}
      }
    };
    // @ts-ignore
    window.tinyMCE.overrideDefaults({
      base_url: '/tinymce/',  // Base for assets such as skins, themes and plugins
      suffix: '.min'          // This will make Tiny load minified versions of all its assets
    });
    const categoryId: number =1;
    this.WcmService.getAllList(categoryId).subscribe((res: any) => {
      if (res.hasSucceeded) {
        const WCMs = res.returnedObject;
        let found = false;
        WCMs.forEach((item) => {
          if (item.pageName.toLowerCase() === 'Terms of Service'.toLowerCase()) {
            found = true;
            this.WcmService.getContent(item.id).subscribe((response: any) => {
              if (response.hasSucceeded && response.returnedObject.length > 0) {
                this.termsContent = response.returnedObject[0].pageContent;
                this.termsContent_Ar = response.returnedObject[0].pageContent_Ar;
              }
            });
          }
        });
      }
    });
  }

  clearForm() {
    this.ResourceForm.reset();
    this.initial();
  }

  initial() {
    this.whatTime = new Date().toLocaleString();
    this.view = 'Create Resource';
    this.terms = false;
    this.imgURL = this.defaultImage;
    window.scrollTo(0, 0);
    this.profileImageUploadPercentage = 0;
    this.thumbnailImageUploadStatus = 'false';
    this.filteredSubjectsMultiple = [];
    this.keyWordSuggestions = [];
    this.filteredkeyWordSuggestions = [];
    this.resourceFiles = [];
    this.Requests = [];
    this.showWhitelistUrlRequest = false;
    this.showCopyRightList = false;
    this.selectedCopyRight = null;
    this.resourceFormSubmitted = false;
    this.resourceFormDrafted = false;
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

  getMasterData() {
    this.resourceService.getResourceMasterData().subscribe((res) => {
      if (res.hasSucceeded) {
        this.educationalUses = res.returnedObject.educationalUse;
        this.educationalStandards = res.returnedObject.educationalStandard;
        this.levels = res.returnedObject.level;
        this.categories = res.returnedObject.categoryMasterData;
        this.materials = res.returnedObject.materialTypeMasterData;
        this.copyrights = res.returnedObject.copyrightMasterData;
        this.subCategories = res.returnedObject.subCategoryMasterData;
      }
    });
  }

  getResourceBySlug(slug) {
    if (this.subCategories.length > 0 && this.categories.length > 0 && this.copyrights.length > 0 && this.materials.length > 0) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.resourceService.getResourceBySlug(slug).subscribe((res) => {
        if (res.hasSucceeded) {
          const resource = res.returnedObject[0];
          if (this.slug && (!resource.isDraft || this.profileService.userId !== resource.createdById)) {
            this.view = 'Blocked Resource';
          } else {
            this.ResourceForm.patchValue({
              categoryId: resource.category && this.categories.find(x => x.id === resource.category.id) ? resource.category.id : null,
            });
            this.filteredSubCategories = this.subCategories.filter(x => x.categoryId.toString() === resource.category.id.toString());
            let keywordsArray = [];
            const keywords = [];
            if (resource.keywords !== 'false' && resource.keywords !== null && resource.keywords !== '') {
              keywordsArray = resource.keywords.split(',');
              if (keywordsArray && keywordsArray.length > 0) {
                keywordsArray.forEach((item) => {
                  keywords.push({
                    display: item,
                    value: item
                  });
                });
              }
            }
            const resourceFiles = resource.resourceFiles;
            const resourceFilesArray = [];
            if (resourceFiles && resourceFiles.length > 0) {
              resourceFiles.forEach((item, index) => {
                this.resourceFiles.push({
                  name: 'Resource File ' + (index + 1),
                  originalName: item.fileName,
                  status: 'Uploaded',
                  progress: 0
                });
                resourceFilesArray.push({
                  url: item.associatedFile,
                  fileName: item.fileName
                });
              });
            }
            const references = resource.references;
            const referencesArray = [];
            if (references && references.length > 0) {
              references.forEach((item) => {
                referencesArray.push({
                  urlReferenceId: item.id,
                  url: item.urlReference,
                  isAllowed: item.isAllowed
                });
              });
            }
            this.ResourceForm.patchValue({
              title: resource.title,
              subCategoryId: resource.subCategory && this.filteredSubCategories.find(x => x.id === resource.subCategory.id) ? resource.subCategory.id : null,
              materialTypeId: resource.materialType && this.materials.find(x => x.id === resource.materialType.id) ? resource.materialType.id : null,
              resourceDescription: resource.resourceDescription,
              thumbnail: resource.thumbnail,
              resourceContent: resource.resourceContent,
              keywords: keywords,
              readingTime: resource.readingTime,
              resourceFiles: resourceFilesArray,
              isDraft: false,
              references: referencesArray,
              copyRightId: resource.copyRight && this.copyrights.find(x => x.id === resource.copyRight.id) ? resource.copyRight.id : null,
              objective: resource.objective,
              educationalUseId: resource.educationalUse && this.educationalUses.find(x => x.id === resource.educationalUse.id) ? resource.educationalUse.id : null,
              levelId: resource.educationLevel && this.levels.find(x => x.id === resource.educationLevel.id) ? resource.educationLevel.id : null,
              educationalStandardId: resource.educationalStandard && this.educationalStandards.find(x => x.id === resource.educationalStandard.id) ? resource.educationalStandard.id : null
            });
            if (resource.thumbnail) {
              this.imgURL = resource.thumbnail;
            }
          }
        } else {
          this.translate.get(res.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.translate.get('Failed to load resource').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
        this.spinner.hide();
      });
    } else {
      setTimeout(() => {
        this.getResourceBySlug(slug);
      }, 200);
    }
  }

  handleFileInput(files: FileList) {
    const extArray = files[0].name.split('.');
    if (this.encService.acceptedImageExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.imageSizeLimit) {
      const reader = new FileReader();
      reader.readAsDataURL(files[0]);
      reader.onload = () => {
        this.imgURL = reader.result;
        if (typeof reader.result === 'string') {
          this.thumbnailImageUploadStatus = 'Uploading';
          const splitFile = files[0].name.split('.');
          const data = [{
            fileName: this.uploadService.tempFolder + Date.now() + this.profileService.makeid(10) + '.' + splitFile[splitFile.length - 1],
            fileBase64: reader.result.split(',')[1]
          }];
          this.profileImageUploadPercentage = 0;
          this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFiles', data,
            {
              reportProgress: true
            })).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
              this.profileImageUploadPercentage = Math.round(100 * event.loaded / event.total);
            } else if (event instanceof HttpResponse) {
              if (event.body.hasSucceed) {
                this.thumbnailImageUploadStatus = 'Uploaded';
                this.translate.get('Resource Thumbnail Uploaded').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                });
                this.ResourceForm.patchValue({
                  thumbnail: event.body.fileUrl
                });
                this.profileImageUploadPercentage = 0;
                this.ResourceForm.patchValue({
                  tempThumbnail: null
                });
              } else {
                this.translate.get('Failed to upload Resource Thumbnail').subscribe((msg) => {
                  this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
                });
                this.thumbnailImageUploadStatus = 'Failed';
                this.profileImageUploadPercentage = 0;
                this.imgURL = this.defaultImage;
                this.ResourceForm.patchValue({
                  tempThumbnail: null
                });
              }
            }
          }, (error) => {
            this.translate.get('Make_Profile_message_11').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
            this.thumbnailImageUploadStatus = 'Failed';
            this.profileImageUploadPercentage = 0;
            this.imgURL = this.defaultImage;
            this.ResourceForm.patchValue({
              tempThumbnail: null
            });
          });
        }
      };
    } else {
      this.ResourceForm.patchValue({
        tempThumbnail: null
      });
      this.translate.get('Make_Profile_message_10').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  removePhoto() {
        
    var thumbnailUrl = this.ResourceForm.value.thumbnail;     
    if(thumbnailUrl != null && thumbnailUrl.search(this.uploadService.tempFolder) == -1)
    {       
      this.removedFilesUrl = (this.removedFilesUrl == '' || this.removedFilesUrl == null) ? this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length) : (this.removedFilesUrl + "," + this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length));
    }
    this.imgURL = this.defaultImage;
    this.ResourceForm.patchValue({
      thumbnail: null
    });
  }

  handleCategoryChange(id) {
    this.filteredSubCategories = this.subCategories.filter(x => x.categoryId.toString() === id);
  }

  filterSubject(query, subjects: any[]): any[] {
    const filtered: any[] = [];
    for (let i = 0; i < subjects.length; i++) {
      const subject = subjects[i];
      if (subject.name.toLowerCase().indexOf(query.toLowerCase()) === 0) {
        filtered.push(subject.name);
      }
    }
    return filtered;
  }

  filterSubjectMultiple(event) {
    const query = event.query;
    this.filteredSubjectsMultiple = this.filterSubject(query, this.subCategories);
  }

  filterKeyword(query, items: any[]): any[] {
    const filtered: any[] = [];
    for (let i = 0; i < items.length; i++) {
      const item = items[i];
      if (item.toLowerCase().indexOf(query.toLowerCase()) === 0) {
        filtered.push(item);
      }
    }
    return filtered;
  }

  addNewKeyWord($event) {
    if (!this.keyWordSuggestions.find(x => x === $event.query)) {
      this.keyWordSuggestions.push($event.query);
    }
    this.filteredkeyWordSuggestions = this.filterKeyword($event.query, this.keyWordSuggestions);
  }

  submitForApproval(Form) {
    Form.patchValue({
      isDraft: false
    });
    if (this.profileService.userId) {
      Form.patchValue({
        createdBy: this.profileService.userId
      });
    }
    this.resourceFormSubmitted = true;
    this.resourceFormDrafted = false;
    if (this.thumbnailImageUploadStatus !== 'Uploading' && !this.resourceFiles.find(x => x.status === 'Uploading')) {
      if (Form.valid && Form.value.agreed) {        
         
         if(Form.value.thumbnail != null)
         { 
          Form.patchValue({
            thumbnail: Form.value.thumbnail.replace(this.uploadService.tempFolder,this.uploadService.thumbnailPicFolder)
          });
         }
         if(Form.value.resourceFiles != null){       
         const filesArr=[];
            for(let i=0;i<Form.value.resourceFiles.length;i++){
              filesArr.push({            
                fileName: Form.value.resourceFiles[i].fileName,
                url: Form.value.resourceFiles[i].url == null ? null : Form.value.resourceFiles[i].url.replace(this.uploadService.tempFolder,this.uploadService.resourceFolder)
              });
            }
            Form.patchValue({
              resourceFiles:filesArr
            });
          }             
        if (this.slug) {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          const data = Form.value;
          this.resourceService.patchResource(data, this.slug).subscribe((res) => {
            if (res.hasSucceeded) {
              this.ResourceForm.reset();
              this.resourceFormSubmitted = false;
              this.removePhoto();
              this.resourceFiles = [];
              this.initial();
              this.router.navigateByUrl('/resources/create');
              if (res.returnedObject[0].isDraft) {
                this.translate.get('No QRC available for assignment. Resource saved as Draft.').subscribe((msg) => {
                  this.messageService.add({
                    severity: 'warn',
                    summary: msg,
                    key: 'toast',
                    life: 5000
                  });
                });
                this.router.navigateByUrl('dashboard/resources');
              } else {
                this.router.navigateByUrl('dashboard/resources/submitted');
                this.translate.get('Successfully submitted for approval').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                  
                  var associatedFiles = res.returnedObject[0].resourceFiles;
                  var thumbnailUrl =  res.returnedObject[0].thumbnail;
                const data = [];
                if(thumbnailUrl != null){
                  data.push({
                    tempObjectName: this.uploadService.tempFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length),
                    distObjectName: this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length)
                  });
                  }
                  if(associatedFiles !=null) {                                 
                  for(let i=0;i<associatedFiles.length;i++){
                      data.push({
                        tempObjectName: this.uploadService.tempFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length),
                        distObjectName: this.uploadService.resourceFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length)
                      });
                    }
                  }
                  this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFilesTempToDestination', data,
                  {
                    reportProgress: true
                  })).subscribe();
                  if(this.removedFilesUrl != '' && this.removedFilesUrl != null)
                  { 
                    const removeArr = this.removedFilesUrl.split(',');               
                   this.removedFilesUrl = '';                  
                    this.uploadService.upload(new HttpRequest('DELETE', environment.apiUrl + 'ContentMedia/FilesDelete', removeArr,
                    {
                      reportProgress: true
                    })).subscribe();
                    }
                });
              }
            } else {
              this.translate.get(res.message).subscribe((translation) => {
                this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
              });
            }
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to submit for approval').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
            this.spinner.hide();
          });
        } else {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          const data = Form.value;
          this.resourceService.postResource(data).subscribe((res) => {
            if (res.hasSucceeded) {
              this.ResourceForm.reset();
              this.resourceFormSubmitted = false;
              this.removePhoto();
              this.resourceFiles = [];
              this.initial();
              if (res.returnedObject.isDraft) {
                this.translate.get('No QRC available for assignment. Resource saved as Draft.').subscribe((msg) => {
                  this.messageService.add({
                    severity: 'warn',
                    summary: msg,
                    key: 'toast',
                    life: 5000
                  });
                });
                this.router.navigateByUrl('dashboard/resources');
              } else {
                this.router.navigateByUrl('dashboard/resources/submitted');
                this.translate.get('Successfully submitted for approval').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                  
                  var associatedFiles = res.returnedObject.resourceFiles;
                  var thumbnailUrl =  res.returnedObject.thumbnail;
                const data = [];
                if(thumbnailUrl != null){
                  data.push({
                    tempObjectName: this.uploadService.tempFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length),
                    distObjectName: this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length)
                  });
                  }
                  if(associatedFiles !=null) {                                 
                  for(let i=0;i<associatedFiles.length;i++){
                      data.push({
                        tempObjectName: this.uploadService.tempFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length),
                        distObjectName: this.uploadService.resourceFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length)
                      });
                    }
                  }
                  this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFilesTempToDestination', data,
                  {
                    reportProgress: true
                  })).subscribe();
                  if(this.removedFilesUrl != '' && this.removedFilesUrl != null)
                  { 
                    const removeArr = this.removedFilesUrl.split(',');               
                   this.removedFilesUrl = '';                  
                    this.uploadService.upload(new HttpRequest('DELETE', environment.apiUrl + 'ContentMedia/FilesDelete', removeArr,
                    {
                      reportProgress: true
                    })).subscribe();
                    }
                });
              }
            } else {
              this.translate.get(res.message).subscribe((translation) => {
                this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
              });
            }
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to submit for approval').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
            this.spinner.hide();
          });
        }
      } else {
        this.translate.get('Make_Profile_message_7').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      }
    } else {
      this.translate.get('Please wait until the file upload is complete').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  saveAsDraft(Form) {
    this.resourceFormSubmitted = false;
    this.resourceFormDrafted = true;
    if (this.profileService.userId) {
      Form.patchValue({
        createdBy: this.profileService.userId
      });
    }
    if (Form.value.title != null && Form.value.categoryId != null) {
      if (this.thumbnailImageUploadStatus !== 'Uploading' && !this.resourceFiles.find(x => x.status === 'Uploading')) {
        Form.patchValue({
          isDraft: true
        });
        
        if(Form.value.thumbnail != null)
        { 
         Form.patchValue({
           thumbnail: Form.value.thumbnail.replace(this.uploadService.tempFolder,this.uploadService.thumbnailPicFolder)
         });
        }
        if(Form.value.resourceFiles != null){       
        const filesArr=[];
           for(let i=0;i<Form.value.resourceFiles.length;i++){
             filesArr.push({            
               fileName: Form.value.resourceFiles[i].fileName,
               url: Form.value.resourceFiles[i].url == null ? null : Form.value.resourceFiles[i].url.replace(this.uploadService.tempFolder,this.uploadService.resourceFolder)
             });
           }
           Form.patchValue({
             resourceFiles:filesArr
           });
         }
       

        if (this.slug) {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          const data = Form.value;
          this.resourceService.patchResource(data, this.slug).subscribe((res) => {
            if (res.hasSucceeded) {
              this.ResourceForm.reset();
              this.resourceFormDrafted = false;
              this.removePhoto();
              this.resourceFiles = [];
              this.initial();
              this.router.navigateByUrl('dashboard/resources');
              this.translate.get('Successfully saved Draft').subscribe((msg) => {
                this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                
                var associatedFiles = res.returnedObject[0].resourceFiles;
                var thumbnailUrl =  res.returnedObject[0].thumbnail;
              const data = [];
              if(thumbnailUrl != null){
                data.push({
                  tempObjectName: this.uploadService.tempFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length),
                  distObjectName: this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length)
                });
                }
                if(associatedFiles !=null) {                                 
                for(let i=0;i<associatedFiles.length;i++){
                    data.push({
                      tempObjectName: this.uploadService.tempFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length),
                      distObjectName: this.uploadService.resourceFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length)
                    });
                  }
                }
                this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFilesTempToDestination', data,
                {
                  reportProgress: true
                })).subscribe();
                if(this.removedFilesUrl != '' && this.removedFilesUrl != null)
            { 
              const removeArr = this.removedFilesUrl.split(',');               
             this.removedFilesUrl = '';                  
              this.uploadService.upload(new HttpRequest('DELETE', environment.apiUrl + 'ContentMedia/FilesDelete', removeArr,
              {
                reportProgress: true
              })).subscribe();
              }
              });
            } else {
              this.translate.get(res.message).subscribe((translation) => {
                this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
              });
            }
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to save Draft').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
            this.spinner.hide();
          });
        } else {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          const data = Form.value;
          this.resourceService.postResource(data).subscribe((res) => {
            if (res.hasSucceeded) {
              this.ResourceForm.reset();
              this.resourceFormDrafted = false;
              this.removePhoto();
              this.resourceFiles = [];
              this.initial();
              this.router.navigateByUrl('dashboard/resources');
              this.translate.get('Successfully saved Draft').subscribe((msg) => {
                this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                
                var associatedFiles = res.returnedObject.resourceFiles;
                var thumbnailUrl =  res.returnedObject.thumbnail;
              const data = [];
              if(thumbnailUrl != null){
                data.push({
                  tempObjectName: this.uploadService.tempFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length),
                  distObjectName: this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length)
                });
                }
                if(associatedFiles !=null) {                                 
                for(let i=0;i<associatedFiles.length;i++){
                    data.push({
                      tempObjectName: this.uploadService.tempFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length),
                      distObjectName: this.uploadService.resourceFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length)
                    });
                  }
                }
                this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFilesTempToDestination', data,
                {
                  reportProgress: true
                })).subscribe();
                if(this.removedFilesUrl != '' && this.removedFilesUrl != null)
                { 
                  const removeArr = this.removedFilesUrl.split(',');               
                 this.removedFilesUrl = '';                  
                  this.uploadService.upload(new HttpRequest('DELETE', environment.apiUrl + 'ContentMedia/FilesDelete', removeArr,
                  {
                    reportProgress: true
                  })).subscribe();
                  }
              });
            } else {
              this.translate.get(res.message).subscribe((translation) => {
                this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
              });
            }
            this.spinner.hide();
          }, (error) => {
            this.translate.get('Failed to save Draft').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
            this.spinner.hide();
          });
        }
      } else {
        this.translate.get('Please wait until the file upload is complete').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      }
    } else {
      this.translate.get('Make_Profile_message_7').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  previewResource() {
    window.scrollTo(0, 0);
    this.view = 'View Resource';
  }

  closePreviewResource() {
    window.scrollTo(0, 0);
    this.view = 'Create Resource';
  }

  handleResourceFileChange(files: FileList) {
    const extArray = files[0].name.split('.');
    if ((this.encService.acceptedImageExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.imageSizeLimit)
      || this.encService.acceptedVideoExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit
      || this.encService.acceptedInvalidExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit
      || (this.encService.acceptedDocumentExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.docSizeLimit)
      || this.encService.acceptedAudioExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit) {
      this.tempFile = files[0];
      this.UploadNewFile();
    } else {
      this.ResourceForm.patchValue({
        tempFile: null
      });
      this.translate.get('Invalid File Format/Size').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  UploadNewFile() {
    this.ResourceForm.patchValue({
      tempFile: null
    });
    const file = this.tempFile;
    if (file) {
      const splitFile = file.name.split('.');
      const formdata = new FormData();
      formdata.append(this.uploadService.tempFolder + Date.now() + this.profileService.makeid(10) + '.' + splitFile[splitFile.length - 1], file);

      const tempfilename = 'Resource File ' + (this.resourceFiles.length + 1);
      this.resourceFiles.push({
        name: tempfilename,
        originalName: file.name,
        status: 'Uploading',
        progress: 0
      });
      const index = this.resourceFiles.length - 1;
      this.Requests[index] = this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFilesAsFormData', formdata,
        {
          reportProgress: true
        })).subscribe(event => {
          if (event.type === HttpEventType.UploadProgress) {
            this.resourceFiles.find(x => x.name === tempfilename).progress = Math.round(100 * event.loaded / event.total);
          } else if (event instanceof HttpResponse) {
            if (event.body.hasSucceed) {
              const list = this.ResourceForm.value['resourceFiles'] ? this.ResourceForm.value['resourceFiles'] : [];
              list.push({
                url: event.body.fileUrl,
                fileName: file.name
              });
              if (this.resourceFiles.find(x => x.name === tempfilename)) {
                this.resourceFiles.find(x => x.name === tempfilename).status = 'Uploaded';
              }
              this.ResourceForm.patchValue({
                courseFiles: list
              });
              this.translate.get('Resource File Uploaded').subscribe((msg) => {
                this.messageService.add({ severity: 'success', summary: msg, key: 'toast', life: 5000 });
              });
            } else {
              if (this.resourceFiles.find(x => x.name === tempfilename)) {
                this.resourceFiles.find(x => x.name === tempfilename).status = 'Upload Failed';
                const item = this.resourceFiles.find(x => x.name === tempfilename);
                this.resourceFiles.splice(this.resourceFiles.indexOf(item), 1);
              }
              this.translate.get('Failed to upload Resource File').subscribe((msg) => {
                this.messageService.add({ severity: 'error', summary: msg, key: 'toast', life: 5000 });
              });
            }
          }
        }, (error) => {
          if (this.resourceFiles.find(x => x.name === tempfilename)) {
            this.resourceFiles.find(x => x.name === tempfilename).status = 'Upload Failed';
            const item = this.resourceFiles.find(x => x.name === tempfilename);
            this.resourceFiles.splice(this.resourceFiles.indexOf(item), 1);
          }
          this.translate.get('Failed to upload Resource File').subscribe((msg) => {
            this.messageService.add({ severity: 'error', summary: msg, key: 'toast', life: 5000 });
          });
        });
    }
  }

  addNewFile() {
    this.ResourceForm.patchValue({
      tempFile: null
    });
    const file = this.tempFile;
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        if (typeof reader.result === 'string') {
          const splitFile = file.name.split('.');
          const data = [{
            fileName: this.uploadService.tempFolder + Date.now() + this.profileService.makeid(10) + '.' + splitFile[splitFile.length - 1],
            fileBase64: reader.result.split(',')[1]
          }];
          const tempfilename = 'Resource File ' + (this.resourceFiles.length + 1);
          this.resourceFiles.push({
            name: tempfilename,
            originalName: file.name,
            status: 'Uploading',
            progress: 0
          });
          const index = this.resourceFiles.length - 1;
          this.Requests[index] = this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFiles', data,
            {
              reportProgress: true
            })).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
              this.resourceFiles.find(x => x.name === tempfilename).progress = Math.round(100 * event.loaded / event.total);
            } else if (event instanceof HttpResponse) {
              if (event.body.hasSucceed) {
                const list = this.ResourceForm.value['resourceFiles'] ? this.ResourceForm.value['resourceFiles'] : [];
                list.push({
                  url: event.body.fileUrl,
                  fileName: file.name
                });
                if (this.resourceFiles.find(x => x.name === tempfilename)) {
                  this.resourceFiles.find(x => x.name === tempfilename).status = 'Uploaded';
                }
                this.ResourceForm.patchValue({
                  courseFiles: list
                });
                this.translate.get('Resource File Uploaded').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                });
              } else {
                if (this.resourceFiles.find(x => x.name === tempfilename)) {
                  this.resourceFiles.find(x => x.name === tempfilename).status = 'Upload Failed';
                  const item = this.resourceFiles.find(x => x.name === tempfilename);
                  this.resourceFiles.splice(this.resourceFiles.indexOf(item), 1);
                }
                this.translate.get('Failed to upload Resource File').subscribe((msg) => {
                  this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
                });
              }
            }
          }, (error) => {
            if (this.resourceFiles.find(x => x.name === tempfilename)) {
              this.resourceFiles.find(x => x.name === tempfilename).status = 'Upload Failed';
              const item = this.resourceFiles.find(x => x.name === tempfilename);
              this.resourceFiles.splice(this.resourceFiles.indexOf(item), 1);
            }
            this.translate.get('Failed to upload Resource File').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
          });
        }
      };
    }
  }

  removeResourceFile(index) {
        
    var resourceFileUrl = this.ResourceForm.value.resourceFiles[index].url;     
    if(resourceFileUrl != null && resourceFileUrl.search(this.uploadService.tempFolder) == -1)
    { 
      this.removedFilesUrl = (this.removedFilesUrl == '' || this.removedFilesUrl == null) ? this.uploadService.resourceFolder + resourceFileUrl.substring(resourceFileUrl.lastIndexOf("/") + 1, resourceFileUrl.length) : (this.removedFilesUrl+","+this.uploadService.resourceFolder + resourceFileUrl.substring(resourceFileUrl.lastIndexOf("/") + 1, resourceFileUrl.length));
    }
    if (this.Requests[index]) {
      this.Requests[index].unsubscribe();
    }
    this.resourceFiles.splice(index, 1);
    const list = this.ResourceForm.value.resourceFiles ? this.ResourceForm.value.resourceFiles : [];
    list.splice(index, 1);
    this.ResourceForm.patchValue({
      resourceFiles: list
    });
  }

  addNewUrl() {
    const url: string = this.ResourceForm.value.tempUrl;
    if (url && url.trim().length > 0 && !url.trim().includes(' ') && url.trim().split('.').length > 1) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.resourceService.checkWhiteListUrl(this.ResourceForm.value.tempUrl, this.profileService.userId).subscribe((res) => {
        if (res.hasSucceeded) {
          const urls = this.ResourceForm.value.references ? this.ResourceForm.value.references : [];
          urls.push({
            urlReferenceId: res.returnedObject.id,
            url: this.ResourceForm.value.tempUrl,
            isApproved: res.returnedObject.isApproved
          });
          this.ResourceForm.patchValue({
            references: urls,
            tempUrl: null
          });
        } else {
          this.showWhitelistUrlRequest = true;
        }
        this.spinner.hide();
      }, (error) => {
        this.showWhitelistUrlRequest = true;
        this.spinner.hide();
      });
    } else {
      this.translate.get('Invalid Url format').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }


  removeResourceUrl(index) {
    const urls = this.ResourceForm.value.references ? this.ResourceForm.value.references : [];
    urls.splice(index, 1);
    this.ResourceForm.patchValue({
      references: urls
    });

  }

  submitCopyRight() {
    if (this.ResourceForm.value['copyRightId'] != null) {
      this.showCopyRightList = false;
    }
  }

  cancelCopyRight() {
    this.showCopyRightList = false;
  }

  getSelectedCopyRight() {
    if (this.ResourceForm.value['copyRightId'] != null && this.copyrights.length > 0 && this.copyrights.find(x => x.id.toString() === this.ResourceForm.value['copyRightId'].toString())) {
      return this.getCurrentLang() === 'en' ? this.copyrights.find(x => x.id.toString() === this.ResourceForm.value['copyRightId'].toString()).title.toString() : this.getCurrentLang() === 'ar' ? this.copyrights.find(x => x.id.toString() === this.ResourceForm.value['copyRightId'].toString()).title_Ar.toString() : this.copyrights.find(x => x.id.toString() === this.ResourceForm.value['copyRightId'].toString()).title.toString();
    } else {
      return this.getCurrentLang() === 'en' ? 'Select' : 'اختر';
    }

  }


  addKeyword(event) {
    if (event.value.length > 100) {
      const keyword = this.ResourceForm.value.keywords;
      keyword.splice(keyword.length - 1, 1);
      this.ResourceForm.patchValue({
        keywords: keyword
      });
      this.translate.get('Max_Length_Keyword').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }
  RequestWhitelistUrl() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.resourceService.postWhiteListUrl({
      requestedBy: this.profileService.userId,
      url: this.ResourceForm.value.tempUrl
    }).subscribe((res) => {
      this.spinner.hide();
      this.showWhitelistUrlRequest = false;
      if (res.hasSucceeded) {
        const urls = this.ResourceForm.value.references ? this.ResourceForm.value.references : [];
        if (res.returnedObject && res.returnedObject[0].id) {
          urls.push({
            urlReferenceId: res.returnedObject[0].id,
            url: this.ResourceForm.value.tempUrl,
            isApproved: res.returnedObject[0].isApproved
          });
        }
        this.ResourceForm.patchValue({
          references: urls,
          tempUrl: null
        });
        this.translate.get('URL Submitted for whitelisting').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
      } else if (res.message === 'Record exists in database') {
        const urls = this.ResourceForm.value.references ? this.ResourceForm.value.references : [];
        if (res.returnedObject && res.returnedObject[0].id) {
          urls.push({
            urlReferenceId: res.returnedObject[0].id,
            url: this.ResourceForm.value.tempUrl,
            isApproved: res.returnedObject[0].isApproved
          });
        }
        this.ResourceForm.patchValue({
          references: urls,
          tempUrl: null
        });
        this.translate.get('URL already Submitted for whitelisting').subscribe((msg) => {
          this.messageService.add({severity: 'warn', summary: msg, key: 'toast', life: 5000});
        });
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
    }, (error) => {
      this.spinner.hide();
      this.translate.get('Failed to submit url for whitelisting').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    });
  }

  getCategory(id) {
    if (id) {
      return this.categories.find(x => +x.id === +id) ? this.getCurrentLang() === 'en' ? this.categories.find(x => +x.id === +id).name : this.categories.find(x => +x.id === +id).name_Ar : '';
    }
    return '';
  }

  getSubCategory(id) {
    if (id) {
      return this.subCategories.find(x => +x.id === +id) ? this.getCurrentLang() === 'en' ? this.subCategories.find(x => +x.id === +id).name : this.subCategories.find(x => +x.id === +id).name_Ar : '';
    }
    return '';
  }

  getCopyrightTitle(id) {
    if (id) {
      return this.copyrights.find(x => +x.id === +id) ? this.getCurrentLang() === 'en' ? this.copyrights.find(x => +x.id === +id).title : this.copyrights.find(x => +x.id === +id).title_Ar : '';
    }
    return '';
  }

  getMaterialType(id) {
    if (id) {
      return this.materials.find(x => +x.id === +id) ? this.getCurrentLang() === 'en' ? this.materials.find(x => +x.id === +id).name : this.materials.find(x => +x.id === +id).name_Ar : '';
    }
    return '';
  }

  getEduStanard(id) {
    if (id) {
      return this.educationalStandards.find(x => +x.id === +id) ? this.getCurrentLang() === 'en' ? this.educationalStandards.find(x => +x.id === +id).standard : this.educationalStandards.find(x => +x.id === +id).standard_Ar : '';
    }
    return '';
  }

  getEduUse(id) {
    if (id) {
      return this.educationalUses.find(x => +x.id === +id) ? this.getCurrentLang() === 'en' ? this.educationalUses.find(x => +x.id === +id).text : this.educationalUses.find(x => +x.id === +id).text_Ar : '';
    }
    return '';
  }

  getLevel(id) {
    if (id) {
      return this.levels.find(x => +x.id === +id) ? this.getCurrentLang() === 'en' ? this.levels.find(x => +x.id === +id).levelText : this.levels.find(x => +x.id === +id).levelText_Ar : '';
    }
    return '';
  }

  getKeywords(data) {
    if (data.length > 0) {
      let keywords = '';
      data.forEach((item, index) => {
        keywords += (data.length - 1 <= index) ? item.display + ' .' : item.display + ' ,';
      });
      return keywords;
    } else {
      return '';
    }
  }

  showTerms() {
    if (!this.ResourceForm.value.agreed) {
      this.terms = true;
    }
  }

  acceptAgreement(bool) {
    this.ResourceForm.patchValue({
      agreed: bool
    });
    this.terms = false;
  }

}
