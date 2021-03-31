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
import { ContactsService } from 'src/app/contacts/service/contacts.service';
//import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-externalcontact-report-modal',
    templateUrl: './externalcontact-report-modal.component.html',
    styleUrls: ['./externalcontact-report-modal.component.scss'],
    providers: []
})
export class ExternalContactReportModalComponent implements OnInit {
    bsConfig: Partial<BsDatepickerConfig>;
    statusOptions: any;
    status: any;
    sourceouOptions: any;
    destinationOptions: any;
    priorityList: string[];
    statusDisable : any;
    departmentList: any=[];
    user = {
        id: 0
    };
    language:any;
    GovernmentEntityList=[];
    UserID: any;
    Department: any;
    UserName: any;
    EntityName: any;
    GovernmentEntity:any;
    Designation: any;
    EmailID: any;
    PhoneNumber: any;
    smartSearch: any;
    currentUser: any = JSON.parse(localStorage.getItem('User'));
    constructor(private memolistservice: MemoListService, private common: CommonService, public arabicService:ArabicDataService, public bsModalRef: BsModalRef,public contactService : ContactsService,public utill:UtilsService ) {
        this.language = (this.common.language == 'English') ? 'en' : 'ar';
       
    if(this.language == 'en'){
     
      this.GovernmentEntityList=[{OrganizationUnitsID:0,OrganizationUnits:'All'},{OrganizationUnitsID:1,OrganizationUnits:'Yes'},{OrganizationUnitsID:2,OrganizationUnits:'No'}];
    }else {
     
      this.GovernmentEntityList=[{OrganizationUnitsID:0,OrganizationUnits:this.arabic('all')},{OrganizationUnitsID:1,OrganizationUnits:this.arabic('yes')},{OrganizationUnitsID:2,OrganizationUnits:this.arabic('no')}];
    }
        // this.priorityList = this.common.priorityList;
       // this.getComboList();
       this.GovernmentEntity=0;
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

    
    Download() {
        var Report={
           
            EntityName:this.EntityName,
            ContactName :this.UserName,
            
            EmailId:this.EmailID,
            
            IsGovernmentEntity:this.GovernmentEntity,
            PhoneNumber:this.PhoneNumber,
            
            SmartSearch :null,
             
            UserID :this.currentUser.id
         };
        this.contactService.downlaodExternalExcel(Report);
        this.bsModalRef.hide();
    }
    arabic(word) {
        return this.common.arabic.words[word];
    }
    numberOnly(event): boolean {
        const charCode = (event.which) ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
          return false;
        }
        var target = event.target ? event.target : event.srcElement;
        if(target.value.length == 0 && charCode == 48) {
            return false;
        }
        return true;
      }

    

}