import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { LetterListService } from 'src/app/letter/container/component/letter-list/letterlist.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
//import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-letter-report-modal',
    templateUrl: './letter-report-modal.component.html',
    styleUrls: ['./letter-report-modal.component.scss'],
    providers: []
})
export class LetterReportModalComponent implements OnInit {
    bsConfig: Partial<BsDatepickerConfig>= {
        dateInputFormat:'DD/MM/YYYY'
      };
    statusOptions: any;
    status: any;
    sourceouOptions: any;
    destinationOptions: any;
    priorityList: string[];
    userList = [];
    statusDisable : any;
    currentUser: any;
    user = {
        id: 0
    };
    language: any;

    validateDueStartEndDate: any = {
        isValid: true,
        msg: ''
      };
    

    constructor(private letterlistservice: LetterListService,public util:UtilsService, private common: CommonService, public arabicService:ArabicDataService, public bsModalRef: BsModalRef, ) {
        this.bsConfig = {
      dateInputFormat: 'DD/MM/YYYY'
    }
    if(this.language == 'English'){
        this.priorityList=['All','High', 'Medium', 'Low', 'VeryLow'];
    }else{
        this.priorityList=[this.arabic('all'),this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
    }
    // this.priorityList = this.common.ReportpriorityList;
        this.getComboList();
        this.user = JSON.parse(localStorage.getItem("User"));
        this.language = this.common.language;
    }
    report = {
        status: 'All',
        sourceOU: 'All',
        destination: 'All',
        username: 'All',
        dateRangeForm: '',
        dateRangeTo: '',
        priority: 'All',
        smartSearch: '',
        userID: this.user.id
    }

    ngOnInit() {
        if(this.language!='English')
         {
            this.report = {
                status: this.arabic('all'),
                sourceOU: this.arabic('all'),
                destination: this.arabic('all'),
                username: this.arabic('all'),
                dateRangeForm: '',
                dateRangeTo: '',
                priority: this.arabic('all'),
                smartSearch: '',
                userID: this.user.id
            }
         }

        if(this.language == 'English'){
            this.priorityList=['All','High', 'Medium', 'Low', 'VeryLow'];
        }else{
            this.priorityList=[this.arabic('all'),this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
        }
    

     }

    public getComboList() {
        let user_id = '1';
        let memo_id = '0';
        let requestData = [];
        let user = localStorage.getItem("User");
        let userdet = JSON.parse(user);
        let username = userdet.username;
        this.currentUser = username;
        this.letterlistservice.memoList("memos/",1,1,0,userdet.UserID).subscribe((res: any) => {
            // this.statusOptions = res.M_LookupsList;
            var AllStatusList = res.lookupsList;
            let statusids:any=[];
            if(this.language != 'English'){
                statusids.push({DisplayName:this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()], LookupsID: 0, DisplayOrder: 0});
            }else if(this.language == 'English'){
                statusids.push({DisplayName: "All", LookupsID: 0, DisplayOrder: 0});
            }
            AllStatusList.forEach((item)=>{
                statusids.push({DisplayName:item.DisplayName, LookupsID: item.LookupsID, DisplayOrder: item.DisplayOrder});
            });
            this.statusOptions = statusids;
            this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 1);
            this.sourceouOptions = res.organizationList;
            this.destinationOptions = res.organizationList;
        });
        this.letterlistservice.userList('User/', requestData).subscribe((res: any) => {
            // this.userList = res;
            var AllUsersList = res;
            let userids:any=[];
            if(this.language != 'English'){
                userids.push({EmployeeName:this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()], userID: 0, OrgUnitID: 0});
            }else if(this.language == 'English'){
                userids.push({EmployeeName: "All", userID: 0, OrgUnitID: 0});
            }
            AllUsersList.forEach((item)=>{
                userids.push({EmployeeName:item.EmployeeName, userID: item.UserID, OrgUnitID: item.OrgUnitID});
            });
            this.userList = userids;
          });
    }

    Download() {
        this.report.userID = this.user.id;
        if (this.report.status == 'All' || this.report.status == this.arabic('all')) {
            this.report.status = '';
        };
        if (this.report.sourceOU == 'All' || this.report.sourceOU == this.arabic('all')) {
            this.report.sourceOU = '';
        };
        if (this.report.destination == 'All' || this.report.destination == this.arabic('all')) {
            this.report.destination = '';
        };
        if (this.report.username == 'All' || this.report.username == this.arabic('all')) {
            this.report.username = '';
        };
        if (this.report.priority == 'All' || this.report.priority == this.arabic('all')) {
            this.report.priority = '';
        };
        this.letterlistservice.downlaodExcel(this.report);
        this.bsModalRef.hide();
    }

    maxDate(date) {
        return this.util.maxDateCheck(date);
      }

      dateValidation() {
        this.validateDueStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
        let showDueDateValidationMsg = false;
        if (!this.report.dateRangeForm && this.report.dateRangeTo) {
          showDueDateValidationMsg = false;
        } else if (this.report.dateRangeForm && this.report.dateRangeTo) {
          let startDate = new Date(this.report.dateRangeForm).getTime();
          let endDate = new Date(this.report.dateRangeTo).getTime();
          if (endDate < startDate) {
            showDueDateValidationMsg = true;
          } else {
            showDueDateValidationMsg = false;
          }
        } else {
          showDueDateValidationMsg = false;
        }
        return showDueDateValidationMsg;
      }

    arabic(word) {
        return this.common.arabic.words[word];
    }
}
