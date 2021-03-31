import {Component, OnInit} from '@angular/core';
import {ResourceService} from '../../services/resource.service';
import {environment} from '../../../environments/environment';
import {StorageUploadService} from '../../services/storage-upload.service';
import {ProfileService} from '../../services/profile.service';
import {MessageService} from 'primeng/api';
import {FormArray, FormControl, FormGroup, Validators} from '@angular/forms';
import {NgxSpinnerService} from 'ngx-spinner';
import {ElasticSearchService} from '../../services/elastic-search.service';
import {CourseService} from '../../services/course.service';
import {ActivatedRoute, Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import {EncService} from '../../services/enc.service';
import {HttpEventType, HttpRequest, HttpResponse} from '@angular/common/http';
import {Title} from '@angular/platform-browser';
import {WCMService} from '../../services/wcm.service';
import {Observable} from 'rxjs';
import {TagModel} from 'ngx-chips/core/accessor';
import { Options } from 'ng5-slider';
@Component({
  selector: 'app-create-course',
  templateUrl: './create-course.component.html'
})
export class CreateCourseComponent implements OnInit {
  categories: any;
  materials: any;
  subCategories: any;
  filteredSubCategories: any;
  tempSection: any;
  imgURL: any;
  defaultImage: String = 'assets/images/default-thumb.png';
  courseImageUploadPercentage: number;
  thumbnailImageUploadStatus: string;
  keyWordSuggestions: any;
  filteredkeyWordSuggestions: any;
  showPreviewAddTest: boolean;
  init: any;
  CourseForm: FormGroup;
  TestForm: FormGroup;
  courseFormSubmitted: boolean;
  showWhitelistUrlRequest: boolean;
  tempFile: any;
  Requests: any[];
  courseFiles: any;
  copyrights: any;
  queryResults: any;
  queryResource: any;
  educations: any;
  professions: any;
  showAddResource: boolean;
  showAddSection: boolean;
  editSectionIndex: number;
  attachResourceSectionIndex: number;
  courseFormDrafted: boolean;
  view: any;
  whatTime: any;
  showAddTest: boolean;
  showCopyRightList: boolean;
  AddTestData: boolean;
  addSectionSubmitted: boolean;
  slug: any;
  clientUrl: string;
  testTempFile: any[];
  testFileUploads: any[];
  testFileUploadRequests: any[];
  storageUrl: string;
  educationalUses: any[];
  educationalStandards: any[];
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
    showSelectionBarEnd : false ,
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
  constructor(private titleService: Title, private WcmService: WCMService, public encService: EncService, private translate: TranslateService, private router: Router, private route: ActivatedRoute, private eS: ElasticSearchService, private courseService: CourseService, private resourceService: ResourceService, protected uploadService: StorageUploadService, private profileService: ProfileService, private messageService: MessageService, private spinner: NgxSpinnerService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Create Course | UAE - Open Educational Resources');
    window.scrollTo(0, 0);
    this.clientUrl = environment.clientUrl;
    this.termsContent = '';
    this.termsContent_Ar = '';
    this.materials = [];
    this.educations = [];
    this.copyrights = [];
    this.professions = [];
    this.subCategories = [];
    this.categories = [];
    this.removedFilesUrl='';
    this.getMasterData();
    this.initial();
    this.subCategories = [];
    this.TestForm = new FormGroup({
      testName: new FormControl(null, Validators.compose([Validators.required])),
      createdBy: new FormControl(null),
      updatedBy: new FormControl(null),
      questions: new FormArray([]),
    });
    this.CourseForm = new FormGroup({
      title: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(250)])),
      categoryId: new FormControl(null, Validators.compose([Validators.required])),
      subCategoryId: new FormControl(null, Validators.compose([Validators.required])),
      // materialTypeId: new FormControl(null, Validators.compose([Validators.required])),
      courseDescription: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(2000)])),
      thumbnail: new FormControl(null, Validators.compose([Validators.required])),
      tempThumbnail: new FormControl(null, Validators.compose([])),
      readingTime: new FormControl(null, Validators.compose([Validators.required, Validators.min(4)])),
      keywords: new FormControl(false, Validators.compose([Validators.maxLength(10)])),
      courseContent: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(1000000000)])),
      educationId: new FormControl(null, Validators.compose([Validators.required])),
      professionId: new FormControl(null, Validators.compose([Validators.required])),
      copyRightId: new FormControl(null, Validators.compose([Validators.required])),
      courseFiles: new FormControl([]),
      isDraft: new FormControl(true),
      references: new FormControl([]),
      sections: new FormControl([]),
      educationalStandardId: new FormControl(null, Validators.compose([Validators.required])),
      educationalUseId: new FormControl(null, Validators.compose([Validators.required])),
      levelId: new FormControl(null, Validators.compose([Validators.required])),
      objective: new FormControl(null, Validators.compose([Validators.maxLength(2000)])),
      resources: new FormControl([]),
      tests: new FormControl(null),
      agreed: new FormControl(false, Validators.compose([Validators.required])),
      tempFile: new FormControl(null),
      tempUrl: new FormControl(null),
      createdBy: new FormControl(null),
      emailUrl: new FormControl(environment.clientUrl + '/dashboard/qrc')
    });
    this.route.params.subscribe(params => {
      if (params['slug']) {
        this.slug = +this.encService.get(params['slug']);
      }
    });
    this.educationalUses = [];
    this.educationalStandards = [];
    this.levels = [];
    this.init = {
      skin_url: '/tinymce/skins/ui/oxide',
      skin: 'oxide',
      theme: 'silver',
      plugins: 'print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern help',
      toolbar: 'formatselect | bold italic strikethrough forecolor backcolor permanentpen formatpainter  | alignleft aligncenter alignright alignjustify ltr rtl  | numlist bullist outdent indent | removeformat | addcomment',
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

  initial() {
    this.whatTime = new Date().toLocaleString();
    this.terms = false;
    this.showAddTest = false;
    this.view = 'Create Course';
    this.imgURL = this.defaultImage;
    this.courseFormSubmitted = false;
    this.showWhitelistUrlRequest = false;
    this.showAddResource = false;
    this.showAddSection = false;
    this.showCopyRightList = false;
    this.courseFormDrafted = false;
    this.addSectionSubmitted = false;
    this.AddTestData = false;
    this.tempFile = null;
    this.editSectionIndex = null;
    this.attachResourceSectionIndex = null;
    this.tempSection = null;
    this.queryResource = [];
    this.queryResults = [];
    this.courseFiles = [];
    this.Requests = [];
    this.filteredSubCategories = [];
    this.keyWordSuggestions = [];
    this.filteredkeyWordSuggestions = [];
    this.courseImageUploadPercentage = 0;
    this.thumbnailImageUploadStatus = 'false';
    this.testFileUploads = [];
    this.testFileUploadRequests = [];
    this.testTempFile = [];
    this.showPreviewAddTest = false;
  }

  clearForm() {
    this.CourseForm.reset();
    this.initial();
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
        this.professions = res.returnedObject.professionMasterData;
        this.educations = res.returnedObject.educationMasterData;
        if (this.slug) {
          this.getCourseBySlug(this.slug);
        }
      }
    });
  }

  getCourseBySlug(slug) {
    if (this.subCategories.length > 0 && this.categories.length > 0 && this.copyrights.length > 0 && this.educations.length > 0 && this.professions.length > 0) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.courseService.getCourseBySlug(slug).subscribe((res) => {
        if (res.hasSucceeded) {
          const course = res.returnedObject;
          if (!course.isDraft || this.profileService.userId !== course.createdById) {
            this.view = 'Blocked Resource';
          } else {
            this.CourseForm.patchValue({
              categoryId: course.category && this.categories.find(x => x.id === course.category.id) ? course.category.id : null,
            });
            this.filteredSubCategories = this.subCategories.filter(x => x.categoryId.toString() === course.category.id.toString());
            let keywordsArray = [];
            const keywords = [];
            if (course.keywords !== 'false' && course.keywords !== '') {
              keywordsArray = course.keywords.split(',');
              if (keywordsArray && keywordsArray.length > 0) {
                keywordsArray.forEach((item) => {
                  keywords.push({
                    display: item,
                    value: item
                  });
                });
              }
            }
            const resourceFiles = course.associatedFiles;
            const resourceFilesArray = [];
            if (resourceFiles && resourceFiles.length > 0) {
              resourceFiles.forEach((item, index) => {
                this.courseFiles.push({
                  name: 'Course File ' + (index + 1),
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
            const references = course.references;
            const referencesArray = [];
            if (references && references.length > 0) {
              references.forEach((item) => {
                referencesArray.push({
                  urlReferenceId: item.id,
                  url: item.urlReference,
                  isApproved: item.isApproved
                });
              });
            }
            this.CourseForm.patchValue({
              title: course.title,
              subCategoryId: course.subCategory && this.filteredSubCategories.find(x => x.id === course.subCategory.id) ? course.subCategory.id : null,
              courseDescription: course.courseDescription,
              thumbnail: course.thumbnail,
              courseContent: course.courseContent,
              keywords: keywords,
              courseFiles: resourceFilesArray,
              sections: course.courseSection,
              isDraft: false,
              references: referencesArray,
              copyRightId: course.copyRight && this.copyrights.find(x => x.id === course.copyRight.id) ? course.copyRight.id : null,
              readingTime: course.readingTime,
              educationId: course.education && this.educations.find(x => x.id === course.education.id) ? course.education.id : null,
              professionId: course.profession && this.professions.find(x => x.id === course.profession.id) ? course.profession.id : null,
              // objective: course.objective,
              educationalUseId: course.educationalUse && this.educationalUses.find(x => x.id === course.educationalUse.id) ? course.educationalUse.id : null,
              levelId: course.educationLevel && this.levels.find(x => x.id === course.educationLevel.id) ? course.educationLevel.id : null,
              educationalStandardId: course.educationalStandard && this.educationalStandards.find(x => x.id === course.educationalStandard.id) ? course.educationalStandard.id : null
            });
            if (course.thumbnail) {
              this.imgURL = course.thumbnail;
            }
          }
          this.courseService.getCourseTest(course.id).subscribe((response) => {
            if (response.hasSucceeded) {
              const testData = response.returnedObject;
              if (testData.tests && testData.tests.length > 0) {
                if (this.TestForm.value.questions && this.TestForm.value.questions.length > 0) {
                  let length = this.TestForm.value.questions.length;
                  const control = <FormArray>this.TestForm.controls.questions;
                  for (let i = 0; i < length; i++) {
                    control.removeAt(i);
                    length = this.TestForm.value.questions.length;
                    i = 0;
                  }
                }
                this.TestForm.patchValue({
                  testName: testData.tests[0].testName,
                  createdBy: this.profileService.user.id,
                  updatedBy: this.profileService.user.id,
                  questions: []
                });
                const array = this.TestForm.controls.questions as FormArray;
                if (testData.questions && testData.questions.length > 0) {
                  testData.questions.forEach((question, index) => {
                    array.push(new FormGroup({
                      questionText: new FormControl(question.questionText, Validators.compose([Validators.required, Validators.maxLength(2000)])),
                      answerOptions: new FormArray([]),
                      media: new FormControl(question.media),
                      fileName: new FormControl(question.fileName),
                      tempFile: new FormControl(null)
                    }));
                    if (question.media) {
                      const tempfilename = 'Question File ' + (index + 1);
                      this.testFileUploads.push({
                        name: tempfilename,
                        originalName: question.fileName,
                        index: index,
                        status: 'Uploaded',
                        progress: 0
                      });
                    }
                    if (testData.answers && testData.answers.length > 0) {
                      testData.answers.forEach((answer) => {
                        if (question.id === answer.questionId) {
                          // @ts-ignore
                          const options = this.TestForm.controls.questions.controls[array.length - 1].controls.answerOptions as FormArray;
                          options.push(new FormGroup({
                            optionText: new FormControl(answer.optionText, Validators.compose([Validators.required, Validators.maxLength(200)])),
                            correctAnswer: new FormControl(answer.correctAnswer, Validators.compose([Validators.required]))
                          }));
                        }
                      });
                    }
                  });
                }
                const data = this.TestForm.value;
                this.CourseForm.patchValue({
                  tests: data
                });
              }
              if (this.TestForm.value.questions && this.TestForm.value.questions.length > 0) {
                let length = this.TestForm.value.questions.length;
                const control = <FormArray>this.TestForm.controls.questions;
                for (let i = 0; i < length; i++) {
                  control.removeAt(i);
                  length = this.TestForm.value.questions.length;
                  i = 0;
                }
              }
            } else {
              this.translate.get(res.message).subscribe((translation) => {
                this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
              });
            }
          });
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
        this.getCourseBySlug(this.slug);
      }, 500);
    }
  }

  handleCategoryChange(id) {
    this.filteredSubCategories = this.subCategories.filter(x => x.categoryId.toString() === id.toString());
  }

  handleFileInput(files: FileList) {
    if (files.length > 0) {
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
            this.courseImageUploadPercentage = 0;
            this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFiles', data,
              {
                reportProgress: true
              })).subscribe(event => {
              if (event.type === HttpEventType.UploadProgress) {
                this.courseImageUploadPercentage = Math.round(100 * event.loaded / event.total);
              } else if (event instanceof HttpResponse) {
                if (event.body.hasSucceed) {
                  this.thumbnailImageUploadStatus = 'Uploaded';
                  this.translate.get('Course Thumbnail Uploaded').subscribe((msg) => {
                    this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                  });
                  this.CourseForm.patchValue({
                    thumbnail: event.body.fileUrl
                  });
                  this.courseImageUploadPercentage = 0;
                  this.CourseForm.patchValue({
                    tempThumbnail: null
                  });
                } else {
                  this.translate.get('Failed to upload Course Thumbnail').subscribe((msg) => {
                    this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
                  });
                  this.thumbnailImageUploadStatus = 'Failed';
                  this.courseImageUploadPercentage = 0;
                  this.imgURL = this.defaultImage;
                  this.CourseForm.patchValue({
                    tempThumbnail: null
                  });
                }
              }
            }, (error) => {
              this.translate.get('Failed to upload Course Thumbnail').subscribe((msg) => {
                this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
              });
              this.thumbnailImageUploadStatus = 'Failed';
              this.courseImageUploadPercentage = 0;
              this.imgURL = this.defaultImage;
              this.CourseForm.patchValue({
                tempThumbnail: null
              });
            });
          }
        };
      } else {
        this.CourseForm.patchValue({
          tempThumbnail: null
        });
        this.translate.get('Make_Profile_message_10').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      }
    }
  }

  removePhoto() {    
        
    var thumbnailUrl = this.CourseForm.value.thumbnail;     
    if(thumbnailUrl != null && thumbnailUrl.search(this.uploadService.tempFolder) == -1)
    {       
      this.removedFilesUrl = (this.removedFilesUrl == '' || this.removedFilesUrl == null) ? this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length) : (this.removedFilesUrl + "," + this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length));
    }  
    this.imgURL = this.defaultImage;
    this.CourseForm.patchValue({
      thumbnail: null
    });
  }

  addNewKeyWord($event) {
    if (!this.keyWordSuggestions.find(x => x === $event.query)) {
      this.keyWordSuggestions.push($event.query);
    }
    this.filteredkeyWordSuggestions = this.filterKeyword($event.query, this.keyWordSuggestions);
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

  handlecourseFileChange(files: FileList) {
    const extArray = files[0].name.split('.');
    if ((this.encService.acceptedImageExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.imageSizeLimit)
      || this.encService.acceptedVideoExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit
      || this.encService.acceptedInvalidExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit
      || (this.encService.acceptedDocumentExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.docSizeLimit)
      || this.encService.acceptedAudioExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit) {
      this.tempFile = files[0];
      this.UploadNewFile();
    } else {
      this.CourseForm.patchValue({
        tempFile: null
      });
      this.translate.get('Invalid File Format/Size').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  UploadNewFile() {
    this.CourseForm.patchValue({
      tempFile: null
    });
    const file = this.tempFile;
    if (file) {
      const splitFile = file.name.split('.');
      const formdata = new FormData();
      formdata.append(this.uploadService.tempFolder + Date.now() + this.profileService.makeid(10) + '.' + splitFile[splitFile.length - 1], file);

      const tempfilename = 'Course File ' + (this.courseFiles.length + 1);
      this.courseFiles.push({
        name: tempfilename,
        originalName: file.name,
        status: 'Uploading',
        progress: 0
      });
      const index = this.courseFiles.length - 1;
      this.Requests[index] = this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFilesAsFormData', formdata,
        {
          reportProgress: true
        })).subscribe(event => {
          if (event.type === HttpEventType.UploadProgress) {
            this.courseFiles.find(x => x.name === tempfilename).progress = Math.round(100 * event.loaded / event.total);
          } else if (event instanceof HttpResponse) {
            if (event.body.hasSucceed) {
              const list = this.CourseForm.value['courseFiles'] ? this.CourseForm.value['courseFiles'] : [];
              list.push({
                url: event.body.fileUrl,
                fileName: file.name
              });
              if (this.courseFiles.find(x => x.name === tempfilename)) {
                this.courseFiles.find(x => x.name === tempfilename).status = 'Uploaded';
              }
              this.CourseForm.patchValue({
                courseFiles: list
              });
              this.translate.get('Course File Uploaded').subscribe((msg) => {
                this.messageService.add({ severity: 'success', summary: msg, key: 'toast', life: 5000 });
              });
            } else {
              if (this.courseFiles.find(x => x.name === tempfilename)) {
                this.courseFiles.find(x => x.name === tempfilename).status = 'Upload Failed';
                const item = this.courseFiles.find(x => x.name === tempfilename);
                this.courseFiles.splice(this.courseFiles.indexOf(item), 1);
              }
              this.translate.get('Failed to upload course File').subscribe((msg) => {
                this.messageService.add({ severity: 'error', summary: msg, key: 'toast', life: 5000 });
              });
            }
          }
        }, (error) => {
          if (this.courseFiles.find(x => x.name === tempfilename)) {
            this.courseFiles.find(x => x.name === tempfilename).status = 'Upload Failed';
            const item = this.courseFiles.find(x => x.name === tempfilename);
            this.courseFiles.splice(this.courseFiles.indexOf(item), 1);
          }
          this.translate.get('Failed to upload course File').subscribe((msg) => {
            this.messageService.add({ severity: 'error', summary: msg, key: 'toast', life: 5000 });
          });
        });
    }
  }

  addNewFile() {
    this.CourseForm.patchValue({
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
          const tempfilename = 'Course File ' + (this.courseFiles.length + 1);
          this.courseFiles.push({
            name: tempfilename,
            originalName: file.name,
            status: 'Uploading',
            progress: 0
          });
          const index = this.courseFiles.length - 1;
          this.Requests[index] = this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFiles', data,
            {
              reportProgress: true
            })).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
              this.courseFiles.find(x => x.name === tempfilename).progress = Math.round(100 * event.loaded / event.total);
            } else if (event instanceof HttpResponse) {
              if (event.body.hasSucceed) {
                const list = this.CourseForm.value['courseFiles'] ? this.CourseForm.value['courseFiles'] : [];
                list.push({
                  url: event.body.fileUrl,
                  fileName: file.name
                });
                if (this.courseFiles.find(x => x.name === tempfilename)) {
                  this.courseFiles.find(x => x.name === tempfilename).status = 'Uploaded';
                }
                this.CourseForm.patchValue({
                  courseFiles: list
                });
                this.translate.get('Course File Uploaded').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                });
              } else {
                if (this.courseFiles.find(x => x.name === tempfilename)) {
                  this.courseFiles.find(x => x.name === tempfilename).status = 'Upload Failed';
                  const item = this.courseFiles.find(x => x.name === tempfilename);
                  this.courseFiles.splice(this.courseFiles.indexOf(item), 1);
                }
                this.translate.get('Failed to upload course File').subscribe((msg) => {
                  this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
                });
              }
            }
          }, (error) => {
            if (this.courseFiles.find(x => x.name === tempfilename)) {
              this.courseFiles.find(x => x.name === tempfilename).status = 'Upload Failed';
              const item = this.courseFiles.find(x => x.name === tempfilename);
              this.courseFiles.splice(this.courseFiles.indexOf(item), 1);
            }
            this.translate.get('Failed to upload course File').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
          });
        }
      };
    }
  }

  removeCourseFile(index) {
        
    var courseFileUrl = this.CourseForm.value.courseFiles[index].url;     
    if(courseFileUrl != null && courseFileUrl.search(this.uploadService.tempFolder) == -1)
    { 
      this.removedFilesUrl = (this.removedFilesUrl == '' || this.removedFilesUrl == null) ? this.uploadService.courseFolder + courseFileUrl.substring(courseFileUrl.lastIndexOf("/") + 1, courseFileUrl.length) : (this.removedFilesUrl+","+this.uploadService.courseFolder + courseFileUrl.substring(courseFileUrl.lastIndexOf("/") + 1, courseFileUrl.length));
    }
    if (this.Requests[index]) {
      this.Requests[index].unsubscribe();
    }
    this.courseFiles.splice(index, 1);
    const list = this.CourseForm.value.courseFiles ? this.CourseForm.value.courseFiles : [];
    list.splice(index, 1);
    this.CourseForm.patchValue({
      courseFiles: list
    });
  }

  addNewUrl() {
    const url: string = this.CourseForm.value.tempUrl;
    if (url && url.trim().length > 0 && !url.trim().includes(' ') && url.trim().split('.').length > 1) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.resourceService.checkWhiteListUrl(this.CourseForm.value.tempUrl, this.profileService.userId).subscribe((res) => {
        this.spinner.hide();
        if (res.hasSucceeded) {
          const urls = this.CourseForm.value.references ? this.CourseForm.value.references : [];
          urls.push({
            urlReferenceId: res.returnedObject.id,
            url: this.CourseForm.value.tempUrl,
            isApproved: res.returnedObject.isApproved
          });
          this.CourseForm.patchValue({
            references: urls,
            tempUrl: null
          });
        } else {
          this.showWhitelistUrlRequest = true;
        }
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

  removeCourseUrl(index) {
    const urls = this.CourseForm.value.references ? this.CourseForm.value.references : [];
    urls.splice(index, 1);
    this.CourseForm.patchValue({
      references: urls
    });

  }

  RequestWhitelistUrl() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.resourceService.postWhiteListUrl({
      requestedBy: this.profileService.userId,
      url: this.CourseForm.value.tempUrl
    }).subscribe((res) => {
      this.spinner.hide();
      this.showWhitelistUrlRequest = false;
      if (res.hasSucceeded) {
        const urls = this.CourseForm.value.references ? this.CourseForm.value.references : [];
        if (res.returnedObject && res.returnedObject[0].id) {
          urls.push({
            urlReferenceId: res.returnedObject[0].id,
            url: this.CourseForm.value.tempUrl,
            isApproved: res.returnedObject[0].isApproved
          });
        }
        this.CourseForm.patchValue({
          references: urls,
          tempUrl: null
        });
        this.translate.get('URL Submitted for whitelisting').subscribe((msg) => {
          this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
        });
      } else if (res.message === 'Record exists in database') {
        const urls = this.CourseForm.value.references ? this.CourseForm.value.references : [];
        if (res.returnedObject && res.returnedObject[0].id) {
          urls.push({
            urlReferenceId: res.returnedObject[0].id,
            url: this.CourseForm.value.tempUrl,
            isApproved: res.returnedObject[0].isApproved
          });
        }
        this.CourseForm.patchValue({
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

  closeShowWhitelistUrlRequest() {
    this.showWhitelistUrlRequest = false;
  }

  searchResource(event) {
    this.eS.search(event.query, 'Resource', 1, 10).subscribe((res: any) => {
      if (res.hasSucceeded) {
        const resources = res.returnedObject;
        if (resources.length > 0) {
          const list = [];
          resources.forEach((resource) => {
            if (resource.copyRight.isResourceProtect === false) {
              list.push(resource);
            }
          });
          this.queryResults = list;
        }
      }
    });
  }

  previewCourse() {
    window.scrollTo(0, 0);
    this.view = 'View Course';
  }

  closePreviewCourse() {
    window.scrollTo(0, 0);
    this.view = 'Create Course';
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
    this.courseFormSubmitted = true;
    this.courseFormDrafted = false;
    if (this.thumbnailImageUploadStatus !== 'Uploading' && !this.courseFiles.find(x => x.status === 'Uploading')) {
      if (Form.valid && Form.value.agreed) {
      
      if(Form.value.thumbnail != null)
       { 
        Form.patchValue({
          thumbnail: Form.value.thumbnail.replace(this.uploadService.tempFolder,this.uploadService.thumbnailPicFolder)
        });
       }
       if(Form.value.courseFiles != null){       
       const filesArr=[];
          for(let i=0;i<Form.value.courseFiles.length;i++){
            filesArr.push({            
              fileName: Form.value.courseFiles[i].fileName,
              url: Form.value.courseFiles[i].url == null ? null : Form.value.courseFiles[i].url.replace(this.uploadService.tempFolder,this.uploadService.courseFolder)
            });
          }
          Form.patchValue({
            courseFiles:filesArr
          });
        }
        if(Form.value.tests != null){ 
          var mediaUrl = [];          
          var questionsArr = Form.value.tests.questions;
             for(let i=0;i<questionsArr.length;i++){
              Form.value.tests.questions[i].media = questionsArr[i].media == null ? null : questionsArr[i].media.replace(this.uploadService.tempFolder,this.uploadService.testFolder);
               if(questionsArr[i].media != null)
                {mediaUrl.push(questionsArr[i].media);}
             }
           }
        if (this.slug) {
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          const data = Form.value;
          this.courseService.patchCourse(data, this.slug).subscribe((res) => {
            if (res.hasSucceeded) {
              this.CourseForm.reset();
              this.courseFormSubmitted = false;
              this.removePhoto();
              this.courseFiles = [];
              this.initial();
              this.router.navigateByUrl('dashboard/courses/submitted');
              if (res.returnedObject.isDraft) {
                this.translate.get('No QRC available for assignment. Course saved as Draft.').subscribe((msg) => {
                  this.messageService.add({
                    severity: 'warn',
                    summary: msg,
                    key: 'toast',
                    life: 5000
                  });
                });
              } else {
                this.translate.get('Successfully submitted for approval').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                  
                  var associatedFiles = res.returnedObject.associatedFiles;
                  var thumbnailUrl =  res.returnedObject.thumbnail;
                  const data = [];
                  if(thumbnailUrl != null){
                  data.push({
                    tempObjectName: this.uploadService.tempFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length),
                    distObjectName: this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length)
                  });
                }
                if(associatedFiles != null){
                  for(let i=0;i<associatedFiles.length;i++){
                    data.push({
                      tempObjectName: this.uploadService.tempFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length),
                      distObjectName: this.uploadService.courseFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length)
                    });
                  }
                }
                if(mediaUrl != null){
                  for(let i=0;i<mediaUrl.length;i++){
                    data.push({
                      tempObjectName: this.uploadService.tempFolder + mediaUrl[i].substring(mediaUrl[i].lastIndexOf("/") + 1, mediaUrl[i].length),
                      distObjectName: this.uploadService.testFolder + mediaUrl[i].substring(mediaUrl[i].lastIndexOf("/") + 1, mediaUrl[i].length)
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
          this.courseService.postCourse(data).subscribe((res) => {
            if (res.hasSucceeded) {
              this.CourseForm.reset();
              this.courseFormSubmitted = false;
              this.removePhoto();
              this.courseFiles = [];
              this.initial();
              this.router.navigateByUrl('dashboard/courses/submitted');
              if (res.returnedObject.isDraft) {
                this.translate.get('No QRC available for assignment. Course saved as Draft.').subscribe((msg) => {
                  this.messageService.add({
                    severity: 'warn',
                    summary: msg,
                    key: 'toast',
                    life: 5000
                  });
                });
              } else {
                this.translate.get('Successfully submitted for approval').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                  var associatedFiles = res.returnedObject.associatedFiles;
                  var thumbnailUrl =  res.returnedObject.thumbnail;
                  const data = [];
                  if(thumbnailUrl != null){
                  data.push({
                    tempObjectName: this.uploadService.tempFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length),
                    distObjectName: this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length)
                  });
                }
                if(associatedFiles != null){
                  for(let i=0;i<associatedFiles.length;i++){
                    data.push({
                      tempObjectName: this.uploadService.tempFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length),
                      distObjectName: this.uploadService.courseFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length)
                    });
                  }
                }
                if(mediaUrl != null){
                  for(let i=0;i<mediaUrl.length;i++){
                    data.push({
                      tempObjectName: this.uploadService.tempFolder + mediaUrl[i].substring(mediaUrl[i].lastIndexOf("/") + 1, mediaUrl[i].length),
                      distObjectName: this.uploadService.testFolder + mediaUrl[i].substring(mediaUrl[i].lastIndexOf("/") + 1, mediaUrl[i].length)
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
      this.translate.get('Please wait until the file upload is complete.').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  saveAsDraft(Form) {
    this.courseFormDrafted = true;
    this.courseFormSubmitted = false;
    if (this.profileService.userId) {
      Form.patchValue({
        createdBy: this.profileService.userId
      });
    }
    if (Form.value.title != null && Form.value.categoryId != null) {
      if (this.thumbnailImageUploadStatus !== 'Uploading' && !this.courseFiles.find(x => x.status === 'Uploading')) {
        Form.patchValue({
          isDraft: true
        });
        
        if(Form.value.thumbnail != null)
         { 
          Form.patchValue({
            thumbnail: Form.value.thumbnail.replace(this.uploadService.tempFolder,this.uploadService.thumbnailPicFolder)
          });
         }
         if(Form.value.courseFiles != null){       
         const filesArr=[];
            for(let i=0;i<Form.value.courseFiles.length;i++){
              filesArr.push({            
                fileName: Form.value.courseFiles[i].fileName,
                url: Form.value.courseFiles[i].url == null ? null : Form.value.courseFiles[i].url.replace(this.uploadService.tempFolder,this.uploadService.courseFolder)
              });
            }
            Form.patchValue({
              courseFiles:filesArr
            });
          }          
          if(Form.value.tests != null){      
            
            var mediaUrl = [];          
            var questionsArr = Form.value.tests.questions;
               for(let i=0;i<questionsArr.length;i++){
                Form.value.tests.questions[i].media = questionsArr[i].media == null ? null : questionsArr[i].media.replace(this.uploadService.tempFolder,this.uploadService.testFolder);
                 if(questionsArr[i].media != null)
                  {mediaUrl.push(questionsArr[i].media);}
               }
             }
        if (this.slug) {

          this.spinner.show(undefined, {color: this.profileService.themeColor});
          const data = Form.value;
          this.courseService.patchCourse(data, this.slug).subscribe((res) => {
            if (res.hasSucceeded) {
              this.CourseForm.reset();
              this.courseFormDrafted = false;
              this.removePhoto();
              this.courseFiles = [];
              this.initial();
              this.router.navigateByUrl('dashboard/courses');
              this.translate.get('Successfully saved Draft').subscribe((msg) => {
                this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                var associatedFiles = res.returnedObject.associatedFiles;
                var thumbnailUrl =  res.returnedObject.thumbnail;
                const data = [];
                if(thumbnailUrl != null){
                data.push({
                  tempObjectName: this.uploadService.tempFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length),
                  distObjectName: this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length)
                });
              }
              if(associatedFiles != null){
                for(let i=0;i<associatedFiles.length;i++){
                  data.push({
                    tempObjectName: this.uploadService.tempFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length),
                    distObjectName: this.uploadService.courseFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length)
                  });
                }
              }
              if(mediaUrl != null){
                for(let i=0;i<mediaUrl.length;i++){
                  data.push({
                    tempObjectName: this.uploadService.tempFolder + mediaUrl[i].substring(mediaUrl[i].lastIndexOf("/") + 1, mediaUrl[i].length),
                    distObjectName: this.uploadService.testFolder + mediaUrl[i].substring(mediaUrl[i].lastIndexOf("/") + 1, mediaUrl[i].length)
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
          this.courseService.postCourse(data).subscribe((res) => {
            if (res.hasSucceeded) {
              this.CourseForm.reset();
              this.courseFormDrafted = false;
              this.removePhoto();
              this.courseFiles = [];
              this.initial();
              this.router.navigateByUrl('dashboard/courses');
              this.translate.get('Successfully saved Draft').subscribe((msg) => {
                this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});                
                var associatedFiles = res.returnedObject.associatedFiles;
                var thumbnailUrl =  res.returnedObject.thumbnail;
                const data = [];
                if(thumbnailUrl != null){
                data.push({
                  tempObjectName: this.uploadService.tempFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1, thumbnailUrl.length),
                  distObjectName: this.uploadService.thumbnailPicFolder + thumbnailUrl.substring(thumbnailUrl.lastIndexOf("/") + 1,thumbnailUrl.length)
                });
              }
              if(associatedFiles != null){
                for(let i=0;i<associatedFiles.length;i++){
                  data.push({
                    tempObjectName: this.uploadService.tempFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length),
                    distObjectName: this.uploadService.courseFolder + associatedFiles[i].associatedFile.substring(associatedFiles[i].associatedFile.lastIndexOf("/") + 1, associatedFiles[i].associatedFile.length)
                  });
                }
              }
              debugger
              if(mediaUrl != null){
                for(let i=0;i<mediaUrl.length;i++){
                  data.push({
                    tempObjectName: this.uploadService.tempFolder + mediaUrl[i].substring(mediaUrl[i].lastIndexOf("/") + 1, mediaUrl[i].length),
                    distObjectName: this.uploadService.testFolder + mediaUrl[i].substring(mediaUrl[i].lastIndexOf("/") + 1, mediaUrl[i].length)
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

  getCategory(id) {
    if (id) {
      return this.categories.find(x => +x.id === +id) ? this.categories.find(x => +x.id === +id).name : '';
    }
    return '';
  }

  getSubCat(id) {
    if (id) {
      return this.subCategories.find(x => +x.id === +id) ? this.getCurrentLang() === 'en' ? this.subCategories.find(x => +x.id === +id).name : this.subCategories.find(x => +x.id === +id).name_Ar : '';
    }
    return '';

  }

  getEduSta(id) {
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

  getEduLevel(id) {
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

  showAddTestModal() {
    if (this.profileService.user) {
      this.showAddTest = true;
      this.TestForm.reset();
      this.TestForm.patchValue({
        testName: (Date.now() + this.profileService.user.id).toString(),
        createdBy: this.profileService.user.id,
        updatedBy: this.profileService.user.id,
        questions: []
      });
      const array = this.TestForm.controls.questions as FormArray;
      array.push(new FormGroup({
        questionText: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(2000)])),
        answerOptions: new FormArray([]),
        media: new FormControl(null),
        fileName: new FormControl(null),
        tempFile: new FormControl(null)
      }));
      // @ts-ignore
      const options = this.TestForm.controls.questions.controls[array.length - 1].controls.answerOptions as FormArray;
      for (let i = 0; i < 4; i++) {
        options.push(new FormGroup({
          optionText: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(200)])),
          correctAnswer: new FormControl(false, Validators.compose([Validators.required]))
        }));
      }
    } else {
      setTimeout(() => {
        this.showAddTestModal();
      }, 2000);
    }
  }

  showEditTestModal() {
    if (this.profileService.user) {
      this.showAddTest = true;
      if (this.TestForm.value.questions && this.TestForm.value.questions.length > 0) {
        let length = this.TestForm.value.questions.length;
        const control = <FormArray>this.TestForm.controls.questions;
        for (let i = 0; i < length; i++) {
          control.removeAt(i);
          length = this.TestForm.value.questions.length;
          i = 0;
        }
      }

      this.TestForm.reset();
      const temp = this.TestForm.controls.questions as FormArray;
      this.CourseForm.value.tests.questions.forEach((question) => {
        temp.push(new FormGroup({
          questionText: new FormControl(question.questionText, Validators.compose([Validators.required, Validators.maxLength(2000)])),
          answerOptions: new FormArray([]),
          media: new FormControl(question.media),
          fileName: new FormControl(question.fileName),
          tempFile: new FormControl(null)
        }));
        question.answerOptions.forEach((answerOption) => {
          // @ts-ignore
          const tempOptions = this.TestForm.controls.questions.controls[temp.length - 1].controls.answerOptions as FormArray;
          tempOptions.push(new FormGroup({
            optionText: new FormControl(answerOption.optionText, Validators.compose([Validators.required, Validators.maxLength(200)])),
            correctAnswer: new FormControl(answerOption.correctAnswer, Validators.compose([Validators.required]))
          }));
        });
      });
      let testValue = this.TestForm.value.questions;
      if (this.TestForm.value.questions && this.TestForm.value.questions.length > 0) {
        let length = this.TestForm.value.questions.length;
        const control = <FormArray>this.TestForm.controls.questions;
        for (let i = 0; i < length; i++) {
          if (testValue[i].questionText === null) {
            control.removeAt(i);
            i = 0;
            length = this.TestForm.value.questions.length;
            testValue = this.TestForm.value.questions;
          }
        }
      }
      this.TestForm.patchValue({
        testName: this.CourseForm.value.tests.testName,
        createdBy: this.profileService.user.id,
        updatedBy: this.profileService.user.id
      });
    } else {
      setTimeout(() => {
        this.showAddTestModal();
      }, 2000);
    }
  }

  addMoreQuestion() {
    const array = this.TestForm.controls.questions as FormArray;
    array.push(new FormGroup({
      questionText: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(2000)])),
      answerOptions: new FormArray([]),
      media: new FormControl(null),
      tempFile: new FormControl(null)
    }));
    // @ts-ignore
    const options = this.TestForm.controls.questions.controls[array.length - 1].controls.answerOptions as FormArray;
    for (let i = 0; i < 4; i++) {
      options.push(new FormGroup({
        optionText: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(200)])),
        correctAnswer: new FormControl(false, Validators.compose([Validators.required]))
      }));
    }
    this.testFileUploads.push(null);
  }

  RightAnswerSelection(event, questionIndex, answerIndex) {
    const questions = this.TestForm.value.questions;
    questions[questionIndex].answerOptions.forEach((item) => {
      item.correctAnswer = false;
    });
    questions[questionIndex].answerOptions[answerIndex].correctAnswer = true;
    this.TestForm.patchValue({
      questions: questions
    });
  }

  checkAnswer(questionIndex, answerIndex) {
    const questions = this.TestForm.value.questions;
    return questions[questionIndex].answerOptions[answerIndex].correctAnswer;
  }

  AddTest() {
    this.AddTestData = true;
    const data = this.TestForm.value;
    const checked = [];
    data.questions.forEach((item, i) => {
      checked[i] = false;
      item.answerOptions.forEach((answer) => {
        if (answer.correctAnswer) {
          checked[i] = true;
        }
      });
    });
    if (this.TestForm.valid && checked.filter(x => x === false).length === 0) {
      if (data.questions.length === 0) {
        this.AddTestData = false;
        this.CourseForm.patchValue({
          tests: null
        });
      } else {
        this.CourseForm.patchValue({
          tests: this.TestForm.value
        });
      }
      this.showAddTest = false;
    } else {
      this.translate.get('Make_Profile_message_7').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  cancelTest() {
    this.showAddTest = false;
    this.TestForm.reset();
    this.TestForm.patchValue({
      testName: null,
      createdBy: null,
      updatedBy: null
    });
    if (this.TestForm.value.questions && this.TestForm.value.questions.length > 0) {
      const length = this.TestForm.value.questions.length;
      const control = <FormArray>this.TestForm.controls.questions;
      for (let i = 0; i < length; i++) {
        control.removeAt(i);
      }
    }
  }

  removeQuestion(index) {
    if (index > -1) {
      const control = <FormArray>this.TestForm.controls.questions;
      control.removeAt(index);
      if (this.testFileUploadRequests[index]) {
        this.testFileUploadRequests[index].unsubscribe();
      }
      this.testFileUploads.splice(index, 1);
    }
  }

  submitCopyRight() {
    if (this.CourseForm.value['copyRightId'] != null) {
      this.showCopyRightList = false;
    }
  }

  cancelCopyRight() {
    this.showCopyRightList = false;
  }

  getSelectedCopyRight() {
    if (this.CourseForm.value['copyRightId'] != null && this.copyrights.length > 0 && this.copyrights.find(x => x.id.toString() === this.CourseForm.value['copyRightId'].toString())) {
      return this.getCurrentLang() === 'en' ? this.copyrights.find(x => x.id.toString() === this.CourseForm.value['copyRightId'].toString()).title.toString() : this.copyrights.find(x => x.id.toString() === this.CourseForm.value['copyRightId'].toString()).title_Ar.toString();
    } else {
      return this.getCurrentLang() === 'en' ? 'Select' : 'اختر';
    }
  }

  addKeyword(event) {
    if (event.value.length > 100) {
      const keyword = this.CourseForm.value.keywords;
      keyword.splice(keyword.length - 1, 1);
      this.CourseForm.patchValue({
        keywords: keyword
      });
      this.translate.get('Max_Length_Keyword').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  addResources(index) {
    this.showAddResource = true;
    this.attachResourceSectionIndex = index;
    this.queryResource = this.CourseForm.value.sections[index].courseResources ? this.CourseForm.value.sections[index].courseResources : [];
  }

  closeAttachResource() {
    this.showAddResource = false;
    this.attachResourceSectionIndex = null;
    this.queryResource = [];
  }

  AttachResources() {
    const sections = this.CourseForm.value.sections;
    sections[this.attachResourceSectionIndex].courseResources = this.queryResource;
    this.CourseForm.patchValue({
      sections: sections
    });
    this.queryResource = [];
    this.showAddResource = false;
    this.attachResourceSectionIndex = null;
  }

  removeAttachedResource(i, index) {
    const sections = this.CourseForm.value.sections;
    sections[i].courseResources.splice(index, 1);
    this.CourseForm.patchValue({
      sections: sections
    });
  }

  addSection() {
    this.addSectionSubmitted = false;
    this.showAddSection = true;
    this.tempSection = {
      name: null,
      courseResources: []
    };
  }

  cancelAddSection() {
    this.addSectionSubmitted = false;
    this.showAddSection = false;
    this.tempSection = null;
    this.editSectionIndex = null;
  }

  submitSection(section) {
    if (section.name.length <= 100) {
      this.addSectionSubmitted = true;
      if (this.editSectionIndex != null) {
        if (section.name && section.name.length > 0) {
          const array = this.CourseForm.value.sections ? this.CourseForm.value.sections : [];
          array[this.editSectionIndex] = section;
          this.CourseForm.patchValue({
            sections: array
          });
          this.showAddSection = false;
          this.tempSection = null;
          this.addSectionSubmitted = false;
        }
      } else {
        if (section.name && section.name.length > 0) {
          const array = this.CourseForm.value.sections ? this.CourseForm.value.sections : [];
          array.push(section);
          this.CourseForm.patchValue({
            sections: array
          });
          this.showAddSection = false;
          this.tempSection = null;
          this.addSectionSubmitted = false;
        }
      }
    }
  }

  editSection(index) {
    this.editSectionIndex = index;
    const section = this.CourseForm.value.sections[index];
    this.addSectionSubmitted = false;
    this.showAddSection = true;
    this.tempSection = {
      name: section.name,
      resourceIds: section.resourceIds
    };
  }

  removeSection(index) {
    const array = this.CourseForm.value.sections ? this.CourseForm.value.sections : [];
    array.splice(index, 1);
    this.CourseForm.patchValue({
      sections: array
    });
  }

  handlequestionFileChange(files: FileList, i) {
    const extArray = files[0].name.split('.');
    if ((this.encService.acceptedImageExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.imageSizeLimit)
      || this.encService.acceptedVideoExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit
      || this.encService.acceptedInvalidExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit
      || (this.encService.acceptedDocumentExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.docSizeLimit)
      || this.encService.acceptedAudioExtensions.filter(x => x === extArray[extArray.length - 1].toLowerCase()).length > 0 && files[0].size < this.encService.mediaSizeLimit) {
      this.testTempFile[i] = files[0];
      this.addQuestionFile(i);
    } else {
      const value = this.TestForm.value;
      value.questions[i].tempFile = null;
      this.TestForm.patchValue(value);
      this.translate.get('Invalid File Format/Size').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
      });
    }
  }

  addQuestionFile(i) {
    const list = this.TestForm.value['questions'] ? this.TestForm.value['questions'] : [];
    list.forEach((item, index) => {
      if (i === index) {
        item.tempFile = null;
      }
    });
    this.TestForm.patchValue({
      questions: list
    });
    const file = this.testTempFile[i];
    this.testTempFile[i] = null;
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
          const tempfilename = 'Question File ' + (i + 1);
          this.testFileUploads[i] = {
            name: tempfilename,
            originalName: file.name,
            index: i,
            status: 'Uploading',
            progress: 0
          };
          this.testFileUploadRequests[i] = this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFiles', data,
            {
              reportProgress: true
            })).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
              this.testFileUploads.find(x => (x && x.index === i)).progress = Math.round(100 * event.loaded / event.total);
            } else if (event instanceof HttpResponse) {
              if (event.body.hasSucceed) {
                if (!this.showAddTest) {
                  if (this.TestForm.value.questions && this.TestForm.value.questions.length > 0) {
                    let length = this.TestForm.value.questions.length;
                    const control = <FormArray>this.TestForm.controls.questions;
                    for (let ii = 0; ii < length; ii++) {
                      control.removeAt(ii);
                      length = this.TestForm.value.questions.length;
                      ii = 0;
                    }
                  }

                  this.TestForm.reset();
                  const temp = this.TestForm.controls.questions as FormArray;
                  this.CourseForm.value.tests.questions.forEach((question) => {
                    temp.push(new FormGroup({
                      questionText: new FormControl(question.questionText, Validators.compose([Validators.required, Validators.maxLength(2000)])),
                      answerOptions: new FormArray([]),
                      media: new FormControl(question.media),
                      fileName: new FormControl(question.fileName),
                      tempFile: new FormControl(null)
                    }));
                    question.answerOptions.forEach((answerOption) => {
                      // @ts-ignore
                      const tempOptions = this.TestForm.controls.questions.controls[temp.length - 1].controls.answerOptions as FormArray;
                      tempOptions.push(new FormGroup({
                        optionText: new FormControl(answerOption.optionText, Validators.compose([Validators.required, Validators.maxLength(200)])),
                        correctAnswer: new FormControl(answerOption.correctAnswer, Validators.compose([Validators.required]))
                      }));
                    });
                  });
                  let testValue = this.TestForm.value.questions;
                  if (this.TestForm.value.questions && this.TestForm.value.questions.length > 0) {
                    let length = this.TestForm.value.questions.length;
                    const control = <FormArray>this.TestForm.controls.questions;
                    for (let ii = 0; ii < length; ii++) {
                      if (testValue[ii].questionText === null) {
                        control.removeAt(ii);
                        ii = 0;
                        length = this.TestForm.value.questions.length;
                        testValue = this.TestForm.value.questions;
                      }
                    }
                  }
                  this.TestForm.patchValue({
                    testName: this.CourseForm.value.tests.testName,
                    createdBy: this.profileService.user.id,
                    updatedBy: this.profileService.user.id
                  });

                  const lists = this.TestForm.value['questions'] ? this.TestForm.value['questions'] : [];
                  lists.forEach((item, j) => {
                    if (j === i) {
                      item.media = event.body.fileUrl;
                      item.fileName = file.name;
                    }
                  });
                  this.TestForm.patchValue({
                    questions: lists
                  });
                  const dataT = this.TestForm.value;
                  const checked = [];
                  dataT.questions.forEach((item, j) => {
                    checked[j] = false;
                    item.answerOptions.forEach((answer) => {
                      if (answer.correctAnswer) {
                        checked[j] = true;
                      }
                    });
                  });
                  if (dataT.questions.length === 0) {
                    this.AddTestData = false;
                    this.CourseForm.patchValue({
                      tests: null
                    });
                  } else {
                    this.CourseForm.patchValue({
                      tests: this.TestForm.value
                    });
                  }
                } else {
                  const lists = this.TestForm.value['questions'] ? this.TestForm.value['questions'] : [];
                  lists.forEach((item, j) => {
                    if (j === i) {
                      item.media = event.body.fileUrl;
                      item.fileName = file.name;
                    }
                  });
                  this.TestForm.patchValue({
                    questions: lists
                  });
                }
                if (this.testFileUploads.find((x, index) => (x && index === i))) {
                  this.testFileUploads.find((x, index) => (x && index === i)).status = 'Uploaded';
                }
                this.translate.get('Test File Uploaded').subscribe((msg) => {
                  this.messageService.add({severity: 'success', summary: msg, key: 'toast', life: 5000});
                });
              } else {
                if (this.testFileUploads.find((x, index) => (x && index === i))) {
                  this.testFileUploads.find((x, index) => (x && index === i)).status = 'Upload Failed';
                  const item = this.testFileUploads.find((x, index) => (x && index === i));
                  this.testFileUploads.splice(this.testFileUploads.indexOf(item), 1);
                }
                this.translate.get('Failed to upload test File').subscribe((msg) => {
                  this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
                });
              }
            }
          }, (error) => {
            if (this.testFileUploads.find((x, index) => (x && index === i))) {
              this.testFileUploads.find((x, index) => (x && index === i)).status = 'Upload Failed';
              const item = this.testFileUploads.find((x, index) => (x && index === i));
              this.testFileUploads.splice(this.testFileUploads.indexOf(item), 1);
            }
            this.translate.get('Failed to upload test File').subscribe((msg) => {
              this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
            });
          });
        }
      };
    }
  }

  checkAddedFile(i) {
    return this.testFileUploads.find((x, index) => (x && index === i)) ? this.testFileUploads.find((x, index) => (x && index === i)) : false;
  }

  removeTestFile(index) {    
    var fileUrl = this.TestForm.value.questions[index].media;     
    if(fileUrl != null && fileUrl.search(this.uploadService.tempFolder) == -1)
    { 
      this.removedFilesUrl = (this.removedFilesUrl == '' || this.removedFilesUrl == null) ? this.uploadService.testFolder + fileUrl.substring(fileUrl.lastIndexOf("/") + 1, fileUrl.length) : (this.removedFilesUrl+","+this.uploadService.testFolder + fileUrl.substring(fileUrl.lastIndexOf("/") + 1, fileUrl.length));
    }
    if (this.testFileUploadRequests[index]) {
      this.testFileUploadRequests[index].unsubscribe();
    }
    this.testFileUploads.splice(index, 1);
    const list = this.TestForm.value.questions ? this.TestForm.value.questions : [];
    list.forEach((item, i) => {
      if (i === index) {
        item.media = null;
      }
    });
    this.TestForm.patchValue({
      questions: list
    });
  }

  checkRightAnswer(form) {
    let checked = false;
    form.value.answerOptions.forEach((item) => {
      if (item.correctAnswer) {
        checked = true;
      }
    });
    return checked;
  }

  previewTest() {
    this.showPreviewAddTest = true;
  }

  showTerms() {
    if (!this.CourseForm.value.agreed) {
      this.terms = true;
    }
  }

  acceptAgreement(bool) {
    this.CourseForm.patchValue({
      agreed: bool
    });
    this.terms = false;
  }
}
