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
    selector: 'app-internalcontact-report-modal',
    templateUrl: './internalcontact-report-modal.component.html',
    styleUrls: ['./internalcontact-report-modal.component.scss'],
    providers: []
})
export class InternalContactReportModalComponent implements OnInit {
    bsConfig: Partial<BsDatepickerConfig>;
    statusOptions: any;
    status: any;
    sourceouOptions: any;
    destinationOptions: any;
    priorityList: string[];
    statusDisable : any;
    departmentList: any=[];
    Type = 1; // default internal
    user = {
        id: 0
    };
    language:any;
    UserID: any;
    Department: any;
    UserName: any;
    EntityName: any;
    GovernmentEntity:any;
    Designation: any;
    EmailID: any;
    PhoneNumber: any;
    smartSearch: any;
    departmentids:any=[];
    currentUser: any = JSON.parse(localStorage.getItem('User'));
    constructor(private memolistservice: MemoListService, private common: CommonService, public arabicService:ArabicDataService, public bsModalRef: BsModalRef,public contactService : ContactsService,public utill:UtilsService ) {
        this.language = (this.common.language == 'English') ? 'en' : 'ar';
       
        if(this.language=='ar'){
          this.GovernmentEntity=0
        }else {
            this.GovernmentEntity=0
        }
        // this.priorityList = this.common.priorityList;
       // this.getComboList();
        this.user = JSON.parse(localStorage.getItem("User"));
        this.loadContactsList();
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
        this.GovernmentEntity =0;
    }

   
    Download() {
        var Report={
           
           Department:this.Department,
           UserName :this.UserName,
           
          Designation:this.Designation,
           
           EmailId:this.EmailID,
          PhoneNumber:this.PhoneNumber,
           
          SmartSearch :null,
            
          UserID :this.currentUser.id
        };
        this.contactService.downlaodinternalExcel(Report);
        this.bsModalRef.hide();
    }
    arabic(word) {
        return this.common.arabic.words[word];
    }
    async loadContactsList() {
        let Type = 0;
        let UserID = this.currentUser.id;
        let Department = '';
        let UserName = '';
        let Designation = '';
        let EmailID = '';
        let PhoneNumber = '';
        let smartsearch = '';
        let GovernmentEntity=0;
        let EntityName =''
    
        if (this.Type) {
          Type = this.Type;
        }
        if (this.UserID) {
          UserID = this.UserID;
        }
        if (this.Department) {
          if(this.Department=='All')
          Department="";
          else
          Department = this.Department.trim();
        }
        if (this.UserName) {
          UserName = this.UserName.trim();
        }
        if (this.Designation) {
          Designation = this.Designation.trim();
        }
        if (this.EmailID) {
          EmailID = this.EmailID.trim();
        }
        if (this.PhoneNumber) {
          PhoneNumber = this.PhoneNumber;
        }
        if (this.smartSearch) {
          smartsearch = this.smartSearch.trim();
        }
        if(this.GovernmentEntity)
        {
          GovernmentEntity = this.GovernmentEntity;
        }
        if(this.EntityName)
        {
          EntityName =this.EntityName;
        }

        await this.contactService.getContactList(1, 5, Type, UserID, Department, UserName, Designation, EmailID, PhoneNumber, smartsearch,EntityName,GovernmentEntity).subscribe((data:any) => {
         
          // this.rows = this.userList.Collection;
           var departments =data.M_OrganizationList;
           for(var i=0;i<=departments.length;i++)
          {
            if(i==0)
            {
                var param={};
                if(this.language!='ar'){
                 param ={OrganizationID: "", OrganizationUnits: "All"};}
                else {
                 param ={OrganizationID: "", OrganizationUnits: this.arabic("all")};}
              this.departmentids.push(param);
            }
            else{
              this.departmentids.push(departments[i-1]);
            }
          }
          this.departmentList=this.departmentids;
          this.Department="";
         
    
        });
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