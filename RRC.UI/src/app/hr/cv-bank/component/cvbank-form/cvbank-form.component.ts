import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
import { CvbankService } from '../../service/cvbank.service';
import { Cvbank } from '../../model/cvbank.model';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { DatePipe } from '@angular/common';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { UploadService } from 'src/app/shared/service/upload.service';
import { Attachment } from '../../model/attachment.model';
import { CommonService } from 'src/app/common.service';
import { HttpEventType } from '@angular/common/http';
import { ArabicDataService } from 'src/app/arabic-data.service';

@Component({
  selector: 'app-cv-bank-form',
  templateUrl: './cvbank-form.component.html',
  styleUrls: ['./cvbank-form.component.scss']
})

export class CvbankFormComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  @Input() mode: string;
  id: string;
  editMode: boolean = true;
  refId: string = '';
  certificateDate: Date;
  candidateName: string = '';
  emailID: string = '';
  jobTitle: string = '';
  specializationsList: any = [];
  specializations: any;
  qualificationList: any = [];
  qualification: any;
  gender: string = '';
  experienceList: any = [];
  experience: any;
  expertice: string = '';
  countryList: any = [];
  country: any;
  cityList: any = [];
  city: any;
  address: string = '';
  cvbank: Cvbank = new Cvbank();
  action: string = '';
  comments: string = '';
  bsModalRef: BsModalRef;
  message: string = '';
  createdBy: string = '';
  updatedBy: string = '';
  attachments: Array<Attachment> = [];
  downloadUrl: string = '';
  historyLog: any = null;
  bsConfig: Partial<BsDatepickerConfig>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  valid: boolean = false;
  inProgress: boolean = false;
  emailErr: string = '';
  lang: string;
  arWords: any;

  constructor(private cvbankService: CvbankService,
    public arabic: ArabicDataService,
    public modalService: BsModalService,
    private route: ActivatedRoute,
    public datePipe: DatePipe,
    public utils: UtilsService,
    public upload: UploadService,
    public router: Router,
    public dropdownService: DropdownsService,
    public common: CommonService) {
    this.lang = this.common.currentLang;
    this.arWords = this.arabic.words;
  }

  ngOnInit() {
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.id = this.route.snapshot.paramMap.get("id");
    if (this.id) {
      if (this.lang === 'en') {
        this.common.breadscrumChange('HR', 'CV Bank', 'View CV');// cvbank
      } else {
        this.common.breadscrumChange(this.arWords.humanresource, this.arWords.cvbank, this.arWords.viewcv);
      }
      this.editMode = false;
      this.getCVDetail();
    } else {
      if (this.lang === 'en') {
        this.common.breadscrumChange('HR', 'CV Bank', 'Create CV');
      } else {
        this.common.breadscrumChange(this.arWords.humanresource, this.arWords.cvbank, this.arWords.createcv);
      }
      this.editMode = true;
      this.initiateForm();
    }
    this.loadCountries();
    this.loadSpecializations();
    this.loadEducations();
    this.loadExperience();
  }

  initiateForm() {
    this.action = 'Submit';
  }

  loadCountries() {
    this.dropdownService.getCountries(this.currentUser.id)
      .subscribe((countries: any) => {
        if (countries != null) {
          this.countryList = countries;
        }
      });
  }

  onCountrySelect() {
    this.city = null;
    this.validate();
    this.loadCities();
  }

  onCountryClear() {
    this.cityList = [];
    this.city = null;
    this.validate();
  }

  loadCities() {
    this.dropdownService.getCities(this.currentUser.id, this.country)
      .subscribe((cities: any) => {
        if (cities != null) {
          this.cityList = cities;
        }
      });
  }

  loadExperience() {
    this.dropdownService.getExperience(this.currentUser.id)
      .subscribe((experiences: any) => {
        if (experiences != null) {
          this.experienceList = experiences;
        }
      });
  }

  loadEducations() {
    this.dropdownService.getEducations(this.currentUser.id)
      .subscribe((educations: any) => {
        if (educations != null) {
          this.qualificationList = educations;
        }
      });
  }

  loadSpecializations() {
    this.dropdownService.getSpecializations(this.currentUser.id)
      .subscribe((specializations: any) => {
        if (specializations != null) {
          this.specializationsList = specializations;
        }
      });
  }

  handleFileUpload(event: any) {
    if (event.target.files.length > 0) {
      const files = event.target.files;
      this.upload.uploadAttachment(files)
        .subscribe((response: any) => {
          if (response.type === HttpEventType.Response) {
            for (let i = 0; i < response.body.FileName.length; i++) {
              const attachment = {
                AttachmentGuid: response.body.Guid,
                AttachmentsName: response.body.FileName[i],
                CVBankId: 0
              };
              this.attachments.push(attachment);
            }
          }
          this.validate();
          this.fileInput.nativeElement.value = '';
        });
    }
  }

  handleFileDownload(file: any) {
    this.upload.downloadAttachment(file)
      .subscribe((data) => {
        var url = window.URL.createObjectURL(data);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = file.AttachmentsName;
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
      });
  }

  deleteAttachment(index: any) {
    this.attachments.splice(index, 1);
    this.validate();
  }

  validate() {
    this.valid = true;
    this.emailErr = '';
    if (this.utils.isEmptyString(this.candidateName)
      || this.utils.isEmptyString(this.emailID)
      || this.utils.isEmptyString(this.jobTitle)
      || this.utils.isEmptyString(this.specializations)
      || this.utils.isEmptyString(this.qualification)
      || this.utils.isEmptyString(this.gender)
      || this.utils.isEmptyString(this.experience)
      || this.utils.isEmptyString(this.expertice)
      || this.utils.isEmptyString(this.country)
      || this.utils.isEmptyString(this.city)
      || this.utils.isEmptyString(this.address)
      || this.utils.isEmptyObject(this.attachments)
    ) {
      this.valid = false;
    }
    if (!this.utils.isEmptyString(this.emailID) && !this.utils.isEmail(this.emailID)) {
      this.valid = false;
      this.emailErr = this.lang === 'ar' ? this.arWords.errormsgvalidmailid : 'Please Enter Valid Email!';
    }
  }

  onSubmit() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      this.cvbank.Date = new Date();
      this.cvbank.CandidateName = this.candidateName;
      this.cvbank.EmailId = this.emailID;
      this.cvbank.JobTitle = this.jobTitle;
      this.cvbank.Specialization = this.specializations;
      this.cvbank.EducationalQualification = this.qualification;
      this.cvbank.Gender = this.gender;
      this.cvbank.YearsofExperience = this.experience;
      this.cvbank.AreaofExpertise = this.expertice;
      this.cvbank.CountryofResidence = this.country;
      this.cvbank.CityofResidence = this.city;
      this.cvbank.Address = this.address;
      this.cvbank.Action = this.action;
      this.cvbank.CreatedBy = this.currentUser.id;
      this.cvbank.CreatedDateTime = new Date();
      this.cvbank.Comments = this.comments;
      this.cvbank.Attachments = this.attachments;
      this.cvbankService.create(this.cvbank)
        .subscribe((response: any) => {
          this.inProgress = false;
          if (response.CVBankId) {
            this.message = this.lang === 'ar' ? this.arWords.cvrecordsubmittedsuccess : "CV Record Submitted Successfully";
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['app/hr/cv-bank/cv-bank-list']);
            });
          }
        });
    }
  }

  resetForm() {
    this.candidateName = '';
    this.emailID = '';
    this.jobTitle = '';
    this.specializations = null;
    this.qualification = null;
    this.gender = '';
    this.experience = null;
    this.expertice = '';
    this.country = null;
    this.city = null;
    this.address = '';
    this.comments = '';
    this.attachments = [];
  }

  getCVDetail() {
    this.cvbankService.getCv(this.id, this.currentUser.id)
      .subscribe((cvData: any) => {
        if (cvData.CVBankId) {
          this.refId = cvData.ReferenceNumber;
          this.candidateName = cvData.CandidateName;
          this.certificateDate = new Date(cvData.CreatedDateTime);
          this.emailID = cvData.EmailId;
          this.jobTitle = cvData.JobTitle;
          this.specializations = cvData.Specialization;
          this.qualification = cvData.EducationalQualification;
          this.gender = cvData.Gender;
          this.experience = parseInt(cvData.YearsofExperience);
          this.expertice = cvData.AreaofExpertise;
          this.country = cvData.CountryofResidence;
          this.city = cvData.CityofResidence;
          this.address = cvData.Address;
          this.action = cvData.Action;
          this.createdBy = cvData.CreatedBy;
          this.updatedBy = cvData.UpdatedBy;
          this.comments = cvData.Comments;
          this.attachments = cvData.Attachments;
          this.historyLog = cvData.HistoryLog;
          this.loadCities();
        }
      });
  }

  arabicfn(word: string) {
    word = word.replace(/ +/g, "").toLowerCase();
    return this.common.arabic.words[word];
  }
}
