import {Component, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {FormArray, FormControl, FormGroup, Validators} from '@angular/forms';
import {KeycloakService} from 'keycloak-angular';
import {NgxSpinnerService} from 'ngx-spinner';
import {CookieService} from 'ngx-cookie-service';
import {Router} from '@angular/router';
import {environment} from '../../../environments/environment';
import {StorageUploadService} from '../../services/storage-upload.service';
import {ResourceService} from '../../services/resource.service';
import {MessageService} from 'primeng/api';
import {EncService} from '../../services/enc.service';
import {WCMService} from '../../services/wcm.service';
import {HttpEventType, HttpRequest, HttpResponse} from '@angular/common/http';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-make-profile',
  templateUrl: './make-profile.component.html'
})
export class MakeProfileComponent implements OnInit {
  from: Date;
  to: Date;
  display: boolean;
  years: any;
  defaultImage: String = 'assets/images/default-user.png';
  imagePath: FileList = null;
  imgURL: any;
  countries: any;
  genders: any;
  portalLanguages: any;
  socialMedias: any;
  states: any;
  filteredStates: any;
  titles: any;
  ProfileForm: FormGroup;
  contributorAcceptance: boolean;
  contributorDeclaration: boolean;
  now: Date;
  dobValidDate: Date;
  formSubmitted: boolean;
  email: string;
  firstName: string;
  lastName: string;
  terms: boolean;
  profileImageUploadStatus: string;
  profileImageUploadPercentage: number;
  categories: any;
  filteredCategories: any;
  msgs: any;
  termsContent: string;
  termsContent_Ar: string;
  educationDates: any[];
  experienceDates: any[];
  yearRange: string =  '1900:' + (new Date().getFullYear());
  
  constructor(private titleService: Title, private translate: TranslateService, private profileService: ProfileService, private encService: EncService, private WcmService: WCMService, private messageService: MessageService, private resourceService: ResourceService, protected keycloakAngular: KeycloakService, private spinner: NgxSpinnerService, private router: Router, private cookieService: CookieService, protected uploadService: StorageUploadService) {
    this.termsContent = '';
    this.termsContent_Ar = '';
    this.contributorAcceptance = false;
    this.contributorDeclaration = false;
    this.display = false;
    this.terms = false;
    this.countries = [];
    this.genders = [];
    this.portalLanguages = [];
    this.socialMedias = [];
    this.states = [];
    this.filteredStates = [];
    this.titles = [];
    this.years = [];
    this.categories = [];
    this.filteredCategories = [];
    this.profileImageUploadStatus = null;
    this.profileImageUploadPercentage = 0;
    this.now = new Date();
    this.formSubmitted = false;
    const year = this.now.getFullYear();
    this.dobValidDate = new Date();
    this.dobValidDate = new Date(this.dobValidDate.setFullYear(this.dobValidDate.getFullYear() - 1));
    for (let i = year; i >= 1900; i--) {
      this.years.push(i);
    }
    this.msgs = [];
  }

  ngOnInit() {
    this.titleService.setTitle('Make Profile | UAE - Open Educational Resources');
    this.educationDates = [];
    this.experienceDates = [];
    this.getInitialData();
    this.ProfileForm = new FormGroup({
      id: new FormControl(null),
      email: new FormControl(null),
      title: new FormControl(null, Validators.compose([Validators.required])),
      firstName: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(250)])),
      middleName: new FormControl('', Validators.compose([Validators.maxLength(250)])),
      lastName: new FormControl(null, Validators.compose([Validators.maxLength(250)])),
      country: new FormControl(null, Validators.compose([Validators.required])),
      state: new FormControl(null, Validators.compose([Validators.required])),
      gender: new FormControl(null, Validators.compose([Validators.required])),
      dob: new FormControl(null, Validators.compose([Validators.required])),
      portalLang: new FormControl(null, Validators.compose([Validators.required])),
      profileDesc: new FormControl(null, Validators.compose([Validators.maxLength(4000)])),
      photo: new FormControl(null, Validators.compose([])),
      profilePic: new FormControl(null, Validators.compose([])),
      contributor: new FormControl(false, Validators.compose([])),
      qualifications: new FormArray([]),
      certifications: new FormArray([]),
      experiences: new FormArray([]),
      interestedSubjects: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(150)])),
      languages: new FormArray([]),
      socialLinks: new FormArray([]),
      agreed: new FormControl(false, Validators.compose([Validators.required]))
    });
    this.keycloakAngular.loadUserProfile().then((res) => {
      this.email = res.email;
      this.firstName = res.firstName;
      this.lastName = res.lastName;
      this.ProfileForm.patchValue({
        firstName: this.firstName,
        lastName: this.lastName,
        email: this.email
      });
      if (this.profileService.user && this.profileService.user.title) {
        this.router.navigateByUrl('dashboard');
      } else if (this.profileService.user) {
        this.patchForm(this.profileService.user);
      } else {
        this.spinner.show(undefined, {color: this.profileService.themeColor});
        this.profileService.checkProfileStatus(this.email).subscribe((response) => {
          if (response.hasSucceeded) {
            if (!response.returnedObject.title) {
              this.patchForm(response.returnedObject);
            } else {
              this.router.navigateByUrl('dashboard');
            }
          }
          this.spinner.hide();
        }, (error) => {
          this.spinner.hide();
        });
      }
    });
    this.getCategories();
    this.translate.onLangChange.subscribe(() => {
      this.sortOnLangChange();
    });
    this.imgURL = this.defaultImage;
    window.scrollTo(0, 0);
    $(document).ready(function () {
      $('#file').fileinput({
        overwriteInitial: true,
        maxFileSize: 2000,
        minImageWidth: 200,
        minImageHeight: 200,
        maxImageWidth: 200,
        maxImageHeight: 200,
        showClose: false,
        showCaption: false,
        showBrowse: false,
        browseOnZoneClick: true,
        removeClass: 'btn btn-sm btn-danger',
        removeLabel: '',
        removeIcon: '<i class="fa fa-times"></i>',
        removeTitle: 'Cancel or reset changes',
        elErrorContainer: '#kv-avatar-errors-2',
        msgErrorClass: 'alert alert-danger',
        defaultPreviewContent: '<img src="assets/images/default-user.png" alt="Your Avatar" style="width:156px"><h6 class="text-muted">Click to select </h6>',
        layoutTemplates: {
          main2: '{preview}' + '{remove}',
          actions: '',
          footer: ''
        },
        allowedFileExtensions: ['jpg', 'jpeg', 'png', 'gif']
      });
      $('#file').on('fileclear', function (event) {
        // console.log('remove');
      });
    });
    this.translate.get('Please complete profile to continue').subscribe((res) => {
      this.msgs.push({
        severity: 'warn',
        summary: res,
        key: 'message'
      });
    });
    this.hideMessages(10000);
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

  getCurrentLang() {
    return this.translate.currentLang;
  }


  EduFromSelect(index, event) {
    this.educationDates[index].from = new Date(event);
  }

  EduFromChange(index, event) {
    if (event === this.ProfileForm.value.qualifications[index].fromDate) {
      this.educationDates[index].from = new Date(event);
    }
  }

  EduToSelect(index, event) {
    this.educationDates[index].to = new Date(event);
  }

  EduToChange(index, event) {
    if (event === this.ProfileForm.value.qualifications[index].toDate) {
      this.educationDates[index].to = new Date(event);
    }
  }

  ExpFromSelect(index, event) {
    this.experienceDates[index].from = new Date(event);
  }

  ExpFromChange(index, event) {
    if (event === this.ProfileForm.value.experiences[index].fromDate) {
      this.experienceDates[index].from = new Date(event);
    }
  }

  ExpToSelect(index, event) {
    this.experienceDates[index].to = new Date(event);
  }

  ExpToChange(index, event) {
    if (event === this.ProfileForm.value.experiences[index].toDate) {
      this.experienceDates[index].to = new Date(event);
    }
  }

  getInitialData() {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.profileService.getMakeProfileData().subscribe((res) => {
      this.spinner.hide();
      this.countries = this.sortArray(res.returnedObject.countries, this.translate.currentLang === 'en' ? 'name' : 'name_Ar');
      this.states = this.sortArray(res.returnedObject.states, this.translate.currentLang === 'en' ? 'name' : 'name_Ar');
      this.genders = res.returnedObject.genders;
      this.portalLanguages = res.returnedObject.portalLanguages;
      this.socialMedias = res.returnedObject.socialMedias;
      this.titles = res.returnedObject.titles;
    }, (error) => {
      this.spinner.hide();
    });
  }

  sortOnLangChange() {
    this.countries = this.sortArray(this.countries, this.translate.currentLang === 'en' ? 'name' : 'name_Ar');
    this.states = this.sortArray(this.states, this.translate.currentLang === 'en' ? 'name' : 'name_Ar');
  }


  sortArray(array, string) {
    return array.sort((n1, n2) => {
      if (n1[string] > n2[string]) {
        return 1;
      }

      if (n1[string] < n2[string]) {
        return -1;
      }

      return 0;
    });
  }

  setinitialCountry(id) {
    if (this.states.length > 0) {
      this.countryChange(id);
    } else {
      setTimeout(() => {
        this.setinitialCountry(id);
      }, 1000);
    }
  }


  patchForm(user) {
    if (user) {
      if (user.country) {
        this.setinitialCountry(user.country.id);
      }
      const interestedSubjects = [];
      const subjects = user.subjectsInterested ? user.subjectsInterested.split(',') : [];
      subjects.forEach((item) => {
        interestedSubjects.push(this.categories.filter(x => x.id == item)[0]);
      });
      this.ProfileForm.reset();
      if (user.userEducations) {
        user.userEducations.forEach((item, index) => {
          this.educationDates.push({
            to: new Date(),
            from: new Date()
          });
          this.educationDates[index].to = new Date(item.toDate);
          this.educationDates[index].from = new Date(item.fromDate);
          const userEducationsArray = this.ProfileForm.controls.qualifications as FormArray;
          userEducationsArray.push(new FormGroup({
            universitySchool: new FormControl(item.universitySchool, Validators.compose([Validators.required, Validators.maxLength(250)])),
            major: new FormControl(item.major, Validators.compose([Validators.required, Validators.maxLength(100)])),
            grade: new FormControl(item.grade, Validators.compose([Validators.maxLength(10)])),
            fromDate: new FormControl(new Date(item.fromDate), Validators.compose([Validators.required])),
            toDate: new FormControl(new Date(item.toDate), Validators.compose([Validators.required])),
            createdOn: new FormControl(new Date(item.createdOn).toISOString(), Validators.compose([Validators.required])),
          }));
        });
      }
      if (user.userCertifications) {
        user.userCertifications.forEach((item) => {
          const array = this.ProfileForm.controls.certifications as FormArray;
          array.push(new FormGroup({
            certificationName: new FormControl(item.certificationName, Validators.compose([Validators.required, Validators.maxLength(250)])),
            year: new FormControl(item.year, Validators.compose([Validators.required])),
            createdOn: new FormControl(new Date(item.createdOn).toISOString(), Validators.compose([Validators.required])),
          }));
        });
      }
      if (user.userExperiences) {
        user.userExperiences.forEach((item, index) => {
          this.experienceDates.push({
            to: new Date(),
            from: new Date()
          });
          this.experienceDates[index].to = item.toDate ? new Date(item.toDate) : new Date();
          this.experienceDates[index].from = new Date(item.fromDate);
          const array = this.ProfileForm.controls.experiences as FormArray;
          array.push(new FormGroup({
            organizationName: new FormControl(item.organizationName, Validators.compose([Validators.required, Validators.maxLength(250)])),
            designation: new FormControl(item.designation, Validators.compose([Validators.required, Validators.maxLength(250)])),
            fromDate: new FormControl(new Date(item.fromDate), Validators.compose([Validators.required])),
            toDate: new FormControl(item.toDate ? new Date(item.toDate) : null, Validators.compose([Validators.required])),
            current: new FormControl(!item.toDate, Validators.compose([Validators.required])),
          }));
        });
      }
      if (user.userLanguages) {
        user.userLanguages.forEach((item) => {
          const array = this.ProfileForm.controls.languages as FormArray;
          array.push(new FormGroup({
            language: new FormControl(item.language.name, Validators.compose([Validators.required, Validators.maxLength(100)])),
            isSpeak: new FormControl(item.isSpeak, Validators.compose([Validators.required])),
            isRead: new FormControl(item.isRead, Validators.compose([Validators.required])),
            isWrite: new FormControl(item.isWrite, Validators.compose([Validators.required])),
          }));
        });
      }
      if (user.userSocialMedias) {
        user.userSocialMedias.forEach((item) => {
          const array = this.ProfileForm.controls.socialLinks as FormArray;
          array.push(new FormGroup({
            socialMediaId: new FormControl(item.socialMedia.id, Validators.compose([Validators.required])),
            url: new FormControl(item.url, Validators.compose([Validators.required, Validators.maxLength(100)])),
            createdOn: new FormControl(new Date(item.createdOn).toISOString(), Validators.compose([Validators.required])),
          }));
        });
      }
      this.ProfileForm.patchValue({
        id: user.id,
        email: user.email,
        title: user.title ? user.title.id : null,
        firstName: user.firstName,
        middleName: user.middleName,
        lastName: user.lastName,
        country: user.country ? user.country.id : null,
        state: user.state ? user.state.id : null,
        gender: user.gender ? user.gender.id : null,
        dob: user.dateOfBirth ? new Date(user.dateOfBirth).getDate() + '/' + (new Date(user.dateOfBirth).getMonth() + 1) + '/' + new Date(user.dateOfBirth).getFullYear() : null,
        portalLang: user.portalLanguage ? user.portalLanguage.id : null,
        profileDesc: user.profileDescription,
        photo: user.photo,
        interestedSubjects: interestedSubjects
      });
      if (user.photo) {
        this.imgURL = user.photo;
      }
    }
  }

  hideMessages(timeout: number) {
    setTimeout(() => {
      this.msgs = [];
    }, timeout);
  }

  getCategories() {
    this.resourceService.getCategories(1).subscribe((res) => {
      if (res.hasSucceeded) {
        this.categories = res.returnedObject;
      }
    });
  }

  filterSubject(query, subjects: any[]): any[] {
    const filtered: any[] = [];
    for (let i = 0; i < subjects.length; i++) {
      const subject = subjects[i];
      if (subject.name.toLowerCase().indexOf(query.toLowerCase()) === 0) {
        filtered.push(subject);
      }
    }
    return filtered;
  }


  filterSelectedSubject(subjects: any[]): any[] {
    const filtered: any[] = [];
    const array = this.ProfileForm.value.interestedSubjects ? this.ProfileForm.value.interestedSubjects : [];
    for (let i = 0; i < subjects.length; i++) {
      const subject = subjects[i];
      let found = false;
      array.forEach((item) => {
        if (subject.name.toLowerCase().indexOf(item.name.toLowerCase()) === 0) {
          found = true;
        }
      });
      if (!found) {
        filtered.push(subject);
      }
    }
    return filtered;
  }

  filterSubjectMultiple(event) {
    const query = event.query;
    this.filteredCategories = this.filterSubject(query, this.filterSelectedSubject(this.categories));
  }

  showDialog() {
    if (!this.ProfileForm.controls.contributor.value) {
      this.display = true;
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
          this.profileImageUploadStatus = 'Uploading';
          const splitFile = files[0].name.split('.');
          const data = [{
            fileName: this.uploadService.profilePicFolder + Date.now() + this.profileService.makeid(10) + '.' + splitFile[splitFile.length - 1],
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
                this.profileImageUploadStatus = 'Uploaded';
                this.translate.get('Make_Profile_message_12').subscribe((trans) => {
                  this.messageService.add({severity: 'success', summary: trans, key: 'toast', life: 5000});
                });
                this.ProfileForm.patchValue({
                  photo: event.body.fileUrl
                });
                this.ProfileForm.patchValue({
                  profilePic: null
                });
              } else {
                this.translate.get('Make_Profile_message_11').subscribe((trans) => {
                  this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
                });
                this.profileImageUploadStatus = 'Failed';
                this.profileImageUploadPercentage = 0;
                this.imgURL = this.defaultImage;
                this.ProfileForm.patchValue({
                  profilePic: null
                });
              }
            }
          }, (error) => {
            this.translate.get('Make_Profile_message_11').subscribe((trans) => {
              this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
            });
            this.profileImageUploadStatus = 'Failed';
            this.profileImageUploadPercentage = 0;
            this.imgURL = this.defaultImage;
            this.ProfileForm.patchValue({
              profilePic: null
            });
          });
        }
      };
    } else {
      this.ProfileForm.patchValue({
        profilePic: null
      });
      this.translate.get('Make_Profile_message_10').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
    }
  }

  removePhoto() {
    this.imgURL = this.defaultImage;
    this.ProfileForm.patchValue({
      photo: null
    });
  }


  countryChange(event) {
    this.filteredStates = this.states.filter((state) => state.countryId === +event);
  }


  patchName() {
    this.ProfileForm.patchValue({
      firstName: this.ProfileForm.value.firstName ? this.ProfileForm.value.firstName.trim() : null,
      middleName: this.ProfileForm.value.middleName ? this.ProfileForm.value.middleName.trim() : null,
      lastName: this.ProfileForm.value.lastName ? this.ProfileForm.value.lastName.trim() : null
    });
  }

  submitProfile(data: FormGroup) {
    this.formSubmitted = true;
    this.ProfileForm.patchValue({
      firstName: data.value.firstName ? data.value.firstName.trim() : null,
      middleName: data.value.middleName ? data.value.middleName.trim() : null,
      lastName: data.value.lastName ? data.value.lastName.trim() : null
    });
    data = this.ProfileForm;
    if (data.valid && data.value.agreed === true) {
      if (data.value.id) {
        this.patchProfile(data);
      } else {
        this.postProfile(data);
      }
    } else {
      this.translate.get('Make_Profile_message_7').subscribe((trans) => {
        this.messageService.add({
          severity: 'warn',
          summary: trans,
          key: 'toast',
          life: 5000
        });
      });
    }
  }

  postProfile(data) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.profileService.postProfileData(data.value, this.email).subscribe((res) => {
      if (res.hasSucceeded) {
        this.profileService.setUser(res.returnedObject);
        this.translate.get('Make_Profile_message_8').subscribe((trans) => {
          this.messageService.add({severity: 'success', summary: trans, key: 'toast', life: 5000});
        });
        this.router.navigateByUrl('dashboard');
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Make_Profile_message_9').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  patchProfile(data) {
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.profileService.updateProfileData(data.value, this.email).subscribe((res) => {
      if (res.hasSucceeded) {
        this.profileService.setUser(res.returnedObject);
        this.translate.get('Make_Profile_message_8').subscribe((trans) => {
          this.messageService.add({severity: 'success', summary: trans, key: 'toast', life: 5000});
        });
        this.router.navigateByUrl('dashboard');
      } else {
        this.translate.get(res.message).subscribe((translation) => {
          this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.translate.get('Make_Profile_message_9').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
      this.spinner.hide();
    });
  }

  addEducation() {
    this.educationDates.push({
      to: new Date(),
      from: new Date()
    });
    const array = this.ProfileForm.controls.qualifications as FormArray;
    array.push(new FormGroup({
      universitySchool: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(250)])),
      major: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(100)])),
      grade: new FormControl(null, Validators.compose([Validators.maxLength(10)])),
      fromDate: new FormControl(null, Validators.compose([Validators.required])),
      toDate: new FormControl(null, Validators.compose([Validators.required])),
      createdOn: new FormControl(new Date().toISOString(), Validators.compose([Validators.required])),
    }));
  }

  addCertification() {
    const array = this.ProfileForm.controls.certifications as FormArray;
    array.push(new FormGroup({
      certificationName: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(250)])),
      year: new FormControl(null, Validators.compose([Validators.required])),
      createdOn: new FormControl(new Date().toISOString(), Validators.compose([Validators.required])),
    }));
  }

  addExperience() {
    this.experienceDates.push({
      to: new Date(),
      from: new Date()
    });
    const array = this.ProfileForm.controls.experiences as FormArray;
    array.push(new FormGroup({
      organizationName: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(250)])),
      designation: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(250)])),
      fromDate: new FormControl(null, Validators.compose([Validators.required])),
      toDate: new FormControl(null, Validators.compose([Validators.required])),
      current: new FormControl(false, Validators.compose([Validators.required])),
    }));
  }

  addLanguage() {
    const array = this.ProfileForm.controls.languages as FormArray;
    array.push(new FormGroup({
      language: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(100)])),
      isSpeak: new FormControl(false, Validators.compose([Validators.required])),
      isRead: new FormControl(false, Validators.compose([Validators.required])),
      isWrite: new FormControl(false, Validators.compose([Validators.required])),
    }));
  }

  addSocialMedia() {
    const array = this.ProfileForm.controls.socialLinks as FormArray;
    array.push(new FormGroup({
      socialMediaId: new FormControl(null, Validators.compose([Validators.required])),
      url: new FormControl(null, Validators.compose([Validators.required, Validators.maxLength(100)])),
      createdOn: new FormControl(new Date().toISOString(), Validators.compose([Validators.required])),
    }));
  }

  removeQualification(index) {
    if (index > -1) {
      const control = <FormArray>this.ProfileForm.controls.qualifications;
      control.removeAt(index);
      this.educationDates.splice(index, 1);
    }
  }

  removeCertification(index) {
    if (index > -1) {
      const control = <FormArray>this.ProfileForm.controls.certifications;
      control.removeAt(index);
    }
  }

  removeExperience(index) {
    if (index > -1) {
      const control = <FormArray>this.ProfileForm.controls.experiences;
      control.removeAt(index);
      this.experienceDates.splice(index, 1);
    }
  }

  removeLanguage(index) {
    if (index > -1) {
      const control = <FormArray>this.ProfileForm.controls.languages;
      control.removeAt(index);
    }
  }

  removeSocialLink(index) {
    if (index > -1) {
      const control = <FormArray>this.ProfileForm.controls.socialLinks;
      control.removeAt(index);
    }
  }

  checkContributorAcceptance() {
    if (this.contributorDeclaration && this.contributorAcceptance) {
      this.ProfileForm.patchValue({
        contributor: true
      });
    } else {
      this.ProfileForm.patchValue({
        contributor: false
      });
    }
    this.contributorDeclaration = false;
    this.contributorAcceptance = false;
  }

  acceptAgreement(bool) {
    this.ProfileForm.patchValue({
      agreed: bool
    });
    this.terms = false;
  }

  showTerms() {
    if (!this.ProfileForm.value.agreed) {
      this.terms = true;
    }
  }

  getProgress() {
    const data = this.ProfileForm.value;
    let score = 0;
    if (data.certifications && data.certifications.length > 0) {
      score += 1;
    }
    if (data.title && data.title > 0) {
      score += 1;
    }
    if (data.firstName && data.firstName.length > 0 || data.middleName && data.middleName.length > 0 || data.lastName && data.lastName.length > 0) {
      score += 1;
    }
    if (data.country && data.country > 0) {
      score += 1;
    }
    if (data.state && data.state > 0) {
      score += 1;
    }
    if (data.dob) {
      score += 1;
    }
    if (data.experiences && data.experiences.length > 0) {
      score += 1;
    }
    if (data.interestedSubjects && data.interestedSubjects.length > 0) {
      score += 1;
    }
    if (data.languages && data.languages.length > 0) {
      score += 1;
    }
    if (data.photo) {
      score += 1;
    }
    if (data.portalLang && data.portalLang > 0) {
      score += 1;
    }
    if (data.profileDesc && data.profileDesc.length > 0) {
      score += 1;
    }
    if (data.qualifications && data.qualifications.length > 0) {
      score += 1;
    }
    if (data.socialLinks && data.socialLinks.length > 0) {
      score += 1;
    }
    return Number(((score / 14) * 100).toString());
  }

  setCurrent(index) {
    if (index > -1) {
      const experiences = this.ProfileForm.value.experiences;
      if (experiences[index].current) {
        this.experienceDates[index].to = new Date();
        if (experiences[index].toDate === null) {
          experiences[index].toDate = 0;
        }
      } else {
        this.experienceDates[index].to = experiences[index].toDate ? new Date(experiences[index].toDate) : new Date();
        if (experiences[index].toDate === 0) {
          experiences[index].toDate = null;
        }
      }
      this.ProfileForm.patchValue({
        experiences: experiences
      });
    }
  }


  getDate(string) {
    if (string) {
      return new Date(string);
    } else {
      return new Date();
    }
  }
}
