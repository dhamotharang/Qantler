import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { MemoListService } from 'src/app/memo/services/memolist.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { environment } from 'src/environments/environment';
//import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-reportmodal',
    templateUrl: './reportModal.component.html',
    styleUrls: ['./reportModal.component.scss'],
    providers: []
})
export class ReportModalComponent implements OnInit {
    bsConfig: Partial<BsDatepickerConfig>= {
        dateInputFormat:'DD/MM/YYYY'
      };
    environment = environment;
    statusOptions: any;
    language = 'en';
    status: any;
    draft: 'Draft';
    sourceouOptions: any;
    destinationOptions: any;
    priorityList: string[];
    privateOptions: string[];
    statusDisable: any;
    currentUser: any;
    user = {
        id: 0
    };

    constructor(private memolistservice: MemoListService, private common: CommonService, public arabicService:ArabicDataService, public bsModalRef: BsModalRef, public utill: UtilsService) {
        //     this.bsConfig = {
        //   dateInputFormat: 'DD/MM/YYYY'
        // }
        this.language = (this.common.language == 'English') ? 'en' : 'ar';
        // this.priorityList = this.common.priorityList;
        if(this.language == 'ar'){
            this.priorityList = [this.arabic('all'), this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
            this.privateOptions = [this.arabic('all'), this.arabic('yes'), this.arabic('no')];
        }else{
            this.priorityList = ['All','High', 'Medium', 'Low', 'VeryLow'];
            this.privateOptions = ['All', 'Yes', 'No'];
        }

        this.getComboList();
        this.user = JSON.parse(localStorage.getItem("User"));
    }

    report = {
        status: 'All',
        sourceOU: 'All',
        destinationOU: 'All',
        dateRangeForm: '',
        dateRangeTo: '',
        private: 'All',
        priority: 'All',
        smartSearch: '',
        userID: this.user.id
    }

    ngOnInit() {
        if (this.common.language == 'عربي') {
            this.report.status = this.arabic('all');
            this.report.sourceOU = this.arabic('all');
            this.report.destinationOU = this.arabic('all');
            this.report.private = this.arabic('all');
            this.report.priority = this.arabic('all');
        }
    }

    public getComboList() {
        let user = localStorage.getItem("User");
        let userdet = JSON.parse(user);
        let username = userdet.username;
        this.currentUser = username;
        let user_id = '1';
        let memo_id = '0';
        this.memolistservice.memoList("memos/",1,1,0,userdet.UserID).subscribe((res: any) => {
            // this.statusOptions = res.lookupsList;
            var AllStatusList = res.lookupsList;
            let statusids:any=[];
            if(this.language == 'ar'){
                statusids.push({DisplayName:this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()], LookupsID: 0, DisplayOrder: 0});
            }else if(this.language == 'en'){
                statusids.push({DisplayName: "All", LookupsID: 0, DisplayOrder: 0});
            }
            AllStatusList.forEach((item)=>{
                statusids.push({DisplayName:item.DisplayName, LookupsID: item.LookupsID, DisplayOrder: item.DisplayOrder});
            });
            this.statusOptions = statusids;
            this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 1);
            // this.statusOptions = this.statusOptions.filter(person => person.DisplayName != 'مسودة');
            this.sourceouOptions = res.organizationList;
            var calendar_id = environment.calendar_id;
            this.destinationOptions = res.organizationList.filter(resul => calendar_id != resul.OrganizationID);
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
        if (this.report.destinationOU == 'All' || this.report.destinationOU == this.arabic('all')) {
            this.report.destinationOU = '';
        };
        if (this.report.private == 'All' || this.report.private == this.arabic('all')) {
            this.report.private = '';
        };
        if (this.report.priority == 'All' || this.report.priority == this.arabic('all')) {
            this.report.priority = '';
        };
        this.memolistservice.downlaodExcel(this.report);
        this.bsModalRef.hide();
    }

    arabic(word) {
        return this.common.arabic.words[word];
    }
}