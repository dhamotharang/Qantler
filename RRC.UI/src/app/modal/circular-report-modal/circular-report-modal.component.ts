import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../common.service';
import { CircularService } from '../../circular.service';
import { MemoListService } from 'src/app/memo/services/memolist.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
//import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-circular-report-modal',
    templateUrl: './circular-report-modal.component.html',
    styleUrls: ['./circular-report-modal.component.scss'],
    providers: []
})
export class CircularReportModalComponent implements OnInit {
    bsConfig: Partial<BsDatepickerConfig>= {
        dateInputFormat:'DD/MM/YYYY'
      };
    statusOptions: any;
    status: any;
    sourceouOptions: any;
    destinationOptions: any;
    priorityList: string[];
    statusDisable : any;
    user = {
        id: 0
    };
    language:any;
    constructor(private memolistservice: MemoListService, private common: CommonService, public arabicService:ArabicDataService, public bsModalRef: BsModalRef,public circularService : CircularService,public utill:UtilsService ) {
        this.language = (this.common.language == 'English') ? 'en' : 'ar';
        if(this.language == 'ar'){
            this.priorityList = [this.arabic('all'), this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
        }else{
            this.priorityList = ['All', 'High', 'Medium', 'Low', 'VeryLow'];
        }
        // this.priorityList = this.common.priorityList;
        this.getComboList();
        this.user = JSON.parse(localStorage.getItem("User"));
    }

    report = {
        status: 'All',
        sourceOU: 'All',
        destinationOU: 'All',
        dateRangeForm: '',
        dateRangeTo: '',
        priority: 'All',
        smartSearch: '',
        userID: this.user.id
    }

    ngOnInit() { 
        if(this.language == 'ar'){
            this.report.status = this.arabic('all');
            this.report.sourceOU = this.arabic('all');
            this.report.destinationOU = this.arabic('all');
            this.report.priority = this.arabic('all');
        }
    }

    public getComboList() {
        let user_id = '1';
        let memo_id = '0';
        let user = localStorage.getItem("User");
        let userdet = JSON.parse(user);
        let userid = userdet.id;
        let username = userdet.username;

        this.circularService.circularList("Circular/",1,1,0, userdet.UserID)
      .subscribe((res: any) => {
            // this.statusOptions = res.M_LookupsList;

            var AllStatusList = res.M_LookupsList;
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

            this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 12 && person.LookupsID != 17);
            this.sourceouOptions = res.M_OrganizationList;
            this.destinationOptions = res.M_OrganizationList;
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
        if (this.report.priority == 'All' || this.report.priority == this.arabic('all')) {
            this.report.priority = '';
        };
        this.circularService.downlaodExcel(this.report);
        this.bsModalRef.hide();
    }
    arabic(word) {
        return this.common.arabic.words[word];
    }

}