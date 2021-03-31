import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsDatepickerConfig } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { CvbankService } from '../../service/cvbank.service';

@Component({
  selector: 'app-cvbank-report-modal',
  templateUrl: './cvbank-report-modal.component.html',
  styleUrls: ['./cvbank-report-modal.component.scss']
})
export class CvbankReportModalComponent implements OnInit {
  experience:string = '';
  specialization:string = '';
  country:string = '';
  dateFrom:Date;
  dateTo:Date;
  dateFromErr:string = '';
  dateToErr:string = '';
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  SmartSearch:string = '';
  candidateName:string = '';
  candidateList: Array<any> = [];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  public page: Number = 1;
  public pageSize: Number = 10;
  public pageCount: Number;
  public maxSize: Number = 10;
  valid:boolean = true;
  lang:string;
  arWords: any;

  constructor(
    public bsModalRef: BsModalRef,
    public arabic: ArabicDataService,
    public utils: UtilsService,
    private cvbankService: CvbankService,
    private common: CommonService) {
      this.lang = this.common.currentLang;
      this.arWords = this.arabic.words;
      this.candidateName = this.lang === 'ar' ? this.arWords.all : 'All';
  }

  ngOnInit() {
    this.loadCVList();
  }

  loadCVList() {
    let fromDate = this.dateFrom ? new Date(this.dateFrom).toJSON() : '';
    let toDate = this.dateTo ? new Date(this.dateTo).toJSON() : '';
    let canName = this.candidateName;
    if (!this.candidateName || this.candidateName == 'All' || this.candidateName == this.common.arabic.words.all) {
      canName = '';
    }
    let qryName = `?CandidateName=${canName}`;
    let qryExp = `${qryName}&YearsofExperience=${this.experience}`;
    let qrySpl = `${qryExp}&Specialization=${this.specialization}`;
    let qryCountry = `${qrySpl}&CountryofResidence=${this.country}`;
    let qryDateFrom = `${qryCountry}&DateFrom=${fromDate}`;
    let qryDateTo = `${qryDateFrom}&DateTo=${toDate}`;
    let finalQuery = `${qryDateTo}&SmartSearch=${this.SmartSearch}`;

    this.cvbankService.getCVList(this.page, this.pageSize, finalQuery)
      .subscribe((res:any) => {
        if(this.common.currentLang == 'en'){
          this.candidateList = [{CandidateName: 'All'}].concat(res.CandidateName);
        }else{
          this.candidateList = [{CandidateName: this.common.arabic.words.all}].concat(res.CandidateName);
        }        
        this.pageCount = res.Count;
      });
  }


  validateDates() {
    this.dateFromErr = '';
    this.dateToErr = '';
    if (this.dateFrom && !this.dateTo) {
      this.dateToErr = this.lang === 'ar' ? this.arWords.errormsgvalidenddate : 'Please select a valid End Date';
      this.valid = false;
    } else if (this.dateTo && !this.dateFrom) {
      this.dateFromErr = this.lang === 'ar' ? this.arWords.errormsgvalidstartdate : 'Please select a valid Start Date';
      this.valid = false;
    } else if (this.dateFrom && this.dateTo && !this.utils.isValidFromToDates(this.dateFrom, this.dateTo)) {
      this.dateToErr = this.lang === 'ar' ? this.arWords.errormsgvaliddaterange : 'Please Select Valid Date Range!';
      this.valid = false;
    } else {
      this.valid = true;
    }
  }

  downloadReport() {
    let fromDate = this.dateFrom ? new Date(this.dateFrom).toJSON() : '';
    let toDate = this.dateTo ? new Date(this.dateTo).toJSON() : '';
    const reportModel = {
      CandidateName: this.candidateName,
      EmailId: "",
      JobTitle: "",
      Specializations: this.specialization,
      EducationalQualification: 0,
      YearsofExperience: this.experience,
      CityofResidence: 0,
      SmartSearch: this.SmartSearch,
      UserID: this.currentUser.UserID,
      CountryofResidence: this.country,
      DateFrom: fromDate,
      DateTo: toDate
    };
    if (!this.candidateName || this.candidateName == 'All' || this.candidateName == this.common.arabic.words.all) {
      reportModel.CandidateName = '';
    }
    let date = new Date,
    cur_date = date.getDate() +'-'+(date.getMonth()+1)+'-'+date.getFullYear();
    this.cvbankService.getReport(reportModel)
    .subscribe((resultBlob: Blob)=>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = this.lang ==='ar' ? this.arWords.cvbank+'_'+cur_date+'.xlsx' : 'CVBank'+'_'+cur_date+'.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
      this.bsModalRef.hide();
    });
  }
}
