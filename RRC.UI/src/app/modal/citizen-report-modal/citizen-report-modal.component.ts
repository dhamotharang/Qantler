import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { MemoListService } from 'src/app/memo/services/memolist.service';
import { CitizenAffairService } from 'src/app/citizen-affair/service/citizen-affair.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
//import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-citizenreportmodal',
    templateUrl: './citizen-report-modal.component.html',
    styleUrls: ['./citizen-report-modal.component.scss'],
    providers: []
})
export class CitizenReportModalComponent implements OnInit {
    bsConfig: Partial<BsDatepickerConfig>= {
      dateInputFormat: 'DD/MM/YYYY'
    };
    statusOptions: any;
    status: any;
    sourceouOptions: any;
    destinationOptions: any;
    priorityList: string[];
    language = 'en';
    statusDisable: any;
    user = {
        id: 0
    };
    requestList = ['All','FieldVisit', 'Personal Report', 'Complaints/Suggestion'];
    userList: any;

    validateStartEndDate: any = {
        isValid: true,
        msg: ''
      };

    constructor(private service: CitizenAffairService,public util:UtilsService, private common: CommonService, public bsModalRef: BsModalRef, ) {
        //     this.bsConfig = {
        //   dateInputFormat: 'DD/MM/YYYY'
        // }
        this.priorityList = this.common.priorityList;
        this.getComboList();
        this.user = JSON.parse(localStorage.getItem("User"));
        if (this.common.language != 'English') {
            this.language = 'ar';
            this.requestList = [this.arabic('all'),this.arabic('fieldvisit'), this.arabic('personal'), this.arabic('complaint')];
        }
    }

    report = {
        status: (this.common.language=='English')?'All':this.arabic('all'),
        requestType: (this.common.language=='English')?'All':this.arabic('all'),
        referenceNumber: '',
        requestDateRangeFrom: '',
        requestDateRangeTo: '',
        personalLocationName: '',
        sourcename:(this.common.language=='English')?'All':this.arabic('all'),
        phoneNumber: '',
        smartSearch: '',
        userID: this.user.id
    }

    ngOnInit() { 
        this.report.sourcename = (this.common.language == 'English') ? 'All' : this.arabic('all');
    }

    public getComboList() {
      var list = [];
    list.push({ 'LookupsID': 0, 'DisplayName': (this.common.language == 'English') ? 'All': this.arabic('all') });
        this.service.getList(1, 10, 1, 1, '').subscribe((res: any) => {
          res.M_LookupsList.map(r => list.push(r));
            this.statusOptions = list;
        });
    }

    // async getUserList(data) {
    //     this.userList = this.common.getUserList(data, this.currentUser.id).subscribe(list => {
    //       this.userList = list;
    //     });
    //   }

    Download() {
        if(!this.util.isValidDate(this.report.requestDateRangeFrom)){
            this.report.requestDateRangeFrom = '';
        }
        if(!this.util.isValidDate(this.report.requestDateRangeTo)){
            this.report.requestDateRangeTo = '';
        }
        if (this.report.status == 'All' || this.report.status == this.arabic('all')) {
            this.report.status ='';
        }
        if (this.report.requestType == 'All' || this.report.requestType == this.arabic('all')) {
            this.report.requestType ='';
        }
         if (this.report.sourcename == 'All' || this.report.sourcename == this.arabic('all')) {
            this.report.sourcename ='';
        }
        this.report.userID = this.user.id;
        this.service.downlaodExcel(this.report);
        this.bsModalRef.hide();
    }

    arabic(word) {
        return this.common.arabic.words[word];
    }

    minDate(date){
        return this.util.minDateCheck(date);
      }
      maxDate(date){
        return this.util.maxDateCheck(date);
      }

      dateValidation() {
        this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
        let showDateValidationMsg = false;
        if (!this.report.requestDateRangeFrom && this.report.requestDateRangeTo) {
          showDateValidationMsg = false;
        } else if (this.report.requestDateRangeFrom && this.report.requestDateRangeTo) {
          let startDate = new Date(this.report.requestDateRangeFrom).getTime();
          let endDate = new Date(this.report.requestDateRangeTo).getTime();
          if (endDate < startDate) {
            showDateValidationMsg = true;
          } else {
            showDateValidationMsg = false;
          }
        } else {
          showDateValidationMsg = false;
        }
        return showDateValidationMsg;
      }
}