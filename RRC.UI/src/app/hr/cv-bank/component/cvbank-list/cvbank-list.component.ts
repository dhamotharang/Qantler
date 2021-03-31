import { ArabicDataService } from 'src/app/arabic-data.service';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { CvbankService } from '../../service/cvbank.service';
import * as _ from 'lodash';
import { Router } from '@angular/router';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { CvbankReportModalComponent } from '../../modal/cvbank-report-modal/cvbank-report-modal.component';

@Component({
  selector: 'app-cvbank-list',
  templateUrl: './cvbank-list.component.html',
  styleUrls: ['./cvbank-list.component.scss']
})
export class CvbankListComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('dateColumn') dateColumn: TemplateRef<any>;
  editMode:boolean = true;
  experience:string = '';
  specialization:string = '';
  country:string = '';
  dateFrom:Date;
  dateTo:Date;
  dateFromErr:string = '';
  dateToErr:string = '';
  SmartSearch:string = '';
  candidateName:string = 'All';
  candidateList: Array<any> = [];
  public columns: Array<any> = [];
  public rows: Array<any> = [];
  public page: Number = 1;
  public pageSize: Number = 10;
  public pageCount: Number;
  public maxSize: Number = 10;
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  valid:boolean = true;
  inProgress:boolean = false;
  lang: string;
  arWords: any;
  bsModalRef: BsModalRef;
  tableMessages:any = {
    emptyMessage:''
  };
  constructor(public datePipe: DatePipe,
    private cvbankService: CvbankService,
    public arabic: ArabicDataService,
    private modalService: BsModalService,
    public router: Router,
    public utils: UtilsService,
    public common: CommonService) {
      this.lang = this.common.currentLang;
      this.arWords = this.arabic.words;
    }

  ngOnInit() {
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    if (this.lang === 'en') {
      this.common.breadscrumChange('HR', 'CV Bank List', '');
    } else {
      this.common.breadscrumChange(this.arWords.humanresource, this.arWords.cvbanklist, '');
      this.candidateName = this.common.arabic.words.all;
    }
    this.loadCVList();
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.common.arabic.words.nodatatodisplay
    };
  }

  loadCVList() {
    let fromDate = this.dateFrom ? new Date(this.dateFrom).toJSON() : '';
    let toDate = this.dateTo ? new Date(this.dateTo).toJSON() : '';
    let canName = '';
    if (!this.candidateName || this.candidateName == 'All' || this.candidateName == this.common.arabic.words.all) {
      canName = '';
    } else {
      canName = this.candidateName;
    }
    let qryName = `?CandidateName=${canName}`;
    let qryExp = `${qryName}&YearsofExperience=${this.experience}`;
    let qrySpl = `${qryExp}&Specialization=${this.specialization}`;
    let qryCountry = `${qrySpl}&CountryofResidence=${this.country}`;
    let qryDateFrom = `${qryCountry}&DateFrom=${fromDate}`;
    let qryDateTo = `${qryDateFrom}&DateTo=${toDate}`;
    let finalQuery = `${qryDateTo}&SmartSearch=${this.SmartSearch}`;

    this.columns = [
      { name: this.lang === 'ar' ? this.arWords.refid :'Ref ID', prop: 'ReferenceNumber' },
      { name: this.lang === 'ar' ? this.arWords.candidatename : 'Candidate Name', prop: 'CandidateName' },
      { name: this.lang === 'ar' ? this.arWords.position : 'Position', prop: 'Position' },
      { name: this.lang === 'ar' ? this.arWords.yeasofexperience : 'Years of Experience', prop: 'YearsofExperience' },
      { name: this.lang === 'ar' ? this.arWords.specialization : 'Specialization', prop: 'Specialization' },
      { name: this.lang === 'ar' ? this.arWords.country : 'Country', prop: 'CountryofResidence' },
      { name: this.lang === 'ar' ? this.arWords.date : 'Date', prop: 'Date', cellTemplate: this.dateColumn },
      { name: this.lang === 'ar' ? this.arWords.action : 'Action', prop: '', cellTemplate: this.actionTemplate },
    ];

    this.cvbankService.getCVList(this.page, this.pageSize, finalQuery)
      .subscribe((res:any) => {
        this.rows = res.Collection;
        if(this.common.currentLang == 'en'){
          this.candidateList = [{CandidateName: 'All'}].concat(res.CandidateName);
        }else{
          this.candidateList = [{CandidateName: this.common.arabic.words.all}].concat(res.CandidateName);
        }

        this.pageCount = res.Count;
      });
  }

  viewData(value:any) {
    const path = this.lang + '/app/hr/cv-bank/cv-bank-view/' + value.CVBankId;
    this.router.navigate([path]);
  }

  onChangePage(page:any) {
    this.page = page;
    this.loadCVList();
  }

  validateDates() {
    this.dateFromErr = '';
    this.dateToErr = '';
    if (this.dateFrom && !this.dateTo) {
      this.dateToErr = this.lang ==='ar' ? this.arWords.errormsgvalidenddate : 'Please select a valid End Date';
      this.valid = false;
    } else if (this.dateTo && !this.dateFrom) {
      this.dateFromErr = this.lang ==='ar' ? this.arWords.errormsgvalidstartdate : 'Please select a valid Start Date';
      this.valid = false;
    } else if (this.dateFrom && this.dateTo && !this.utils.isValidFromToDates(this.dateFrom, this.dateTo)) {
      this.dateToErr = this.lang ==='ar' ? this.arWords.errormsgvaliddaterange : 'Please Select Valid Date Range!';
      this.valid = false;
    } else {
      this.valid = true;
    }
  }

  openReport() {
    this.bsModalRef = this.modalService.show(CvbankReportModalComponent,{class:'modal-lg'});
  }
}
