import { Component, OnInit, Inject, Renderer2, TemplateRef, ViewChild, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ContactsService } from '../../service/contacts.service';
import { CommonService } from 'src/app/common.service';
import { DOCUMENT } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { EndPointService } from 'src/app/api/endpoint.service';
import { InternalContactReportModalComponent } from 'src/app/modal/internalcontact-report-modal/internalcontact-report-modal.component';
import { ExternalContactReportModalComponent } from 'src/app/modal/externalcontact-report-modal/externalcontact-report-modal.component';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.scss']
})
export class ContactsComponent implements OnInit {
  enableInternal = false;
  enableExternal = false;
  public page: number = 1;
  public itemsPerPage: number = 10;
  public maxSize: number = 10;
  public numPages: number = 1;
  public length: number = 0;
  bsModalRefin: BsModalRef;
  bsModalRefex: BsModalRef;
  public config: any = {
    paging: true,
    page: 1,
    maxSize: 10
    // sorting: { columns: this.columns },
    // filtering: { filterString: '' },
    // className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  Type =null; // default internal
  UserID: any;
  Department: any;
  UserName: any;
  EntityName: any;
  GovernmentEntity:any;
  Designation: any;
  EmailID: any;
  PhoneNumber: any;
  smartSearch: any;
  contactsList: any=[];
  departmentids:any=[];
  // contactType = "internal";
  departmentList: any=[];
  GovernmentEntityList :any=[];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  message: string;
  @ViewChild('template') template : TemplateRef<any>;
  contactId: any;
  ImageUrl: string;
  rows: any;
  formPage: boolean=false;
  screenStatus = "create";
  @Input() successMsg:any;
  @Input() contactType:any;
  lang: any;
  isEngLang: boolean = true;
  canEditContact: boolean = false;
  emptyData: boolean = false;

  constructor(private endpoint: EndPointService, public modalService: BsModalService, private renderer: Renderer2, @Inject(DOCUMENT) private document: Document, public router:Router, public service: ContactsService, public common: CommonService, private route: ActivatedRoute) {
    // this.common.breadscrumChange('Contacts', 'List Page', '');
    // this.loadContactsList();
    this.ImageUrl =  this.endpoint.imageHostingURL;
   }

  ngOnInit() {
    this.showEnableForm();
    this.canEditContact = this.currentUser.CanEditContact
    this.lang = this.common.currentLang;
    if(this.lang == 'en'){
      this.isEngLang = true;
      this.GovernmentEntityList=[{OrganizationUnitsID:0,OrganizationUnits:'All'},{OrganizationUnitsID:1,OrganizationUnits:'Yes'},{OrganizationUnitsID:2,OrganizationUnits:'No'}];
    }else {
      this.isEngLang = false;
      this.GovernmentEntityList=[{OrganizationUnitsID:0,OrganizationUnits:this.arabic('all')},{OrganizationUnitsID:1,OrganizationUnits:this.arabic('yes')},{OrganizationUnitsID:2,OrganizationUnits:this.arabic('no')}];
    }
    this.UserID = this.currentUser.id;
  }

  showEnableForm() {
    if(this.contactType == 'internal') {
      this.showInternal()
    }else if(this.contactType == 'external') {
      this.showExternal()
    }else {
      this.showInternal()    
    }
  }

  initpage(){
    this.Department = "";
    this.GovernmentEntity =0;
    this.UserName = "";
    this.Designation = "";
    this.EmailID = "";
    this.PhoneNumber = "";
    this.smartSearch = "";
    this.EntityName = "";
  }

  async loadContactsList() {
    this.contactsList = [];
    this.config.totalItems = 0;
    let Type = 1;
    let UserID = this.currentUser.id;
    let Department = '';
    let UserName = '';
    let Designation = '';
    let EmailID = '';
    let PhoneNumber = '';
    let smartsearch = '';
    let GovernmentEntity=0;
    let EntityName ='';
    if(this.contactType == 'internal') {
      this.Type = 1;
    }else if(this.contactType == 'external') {
      this.Type = 2;
    }
    if (this.UserID) {
      UserID = this.UserID;
    }
    if (this.Department) {
      if(this.Department=='All')
      Department="";
      else
      Department = this.Department;
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
    await this.service.getContactList(this.page, this.maxSize, this.Type, UserID, Department, UserName, Designation, EmailID, PhoneNumber, smartsearch,EntityName,GovernmentEntity).subscribe((data:any) => {
      this.contactsList = data.Collection;
        for(var i =0; i<this.contactsList.length; i++) {
          let profileURL='';
          if(this.contactsList[i].ProfilePhotoID && this.contactsList[i].ProfilePhotoName) {
            profileURL = this.endpoint.fileDownloadUrl+"?filename="+this.contactsList[i].ProfilePhotoName+"&guid="+ this.contactsList[i].ProfilePhotoID;
          }
          this.contactsList[i].profileURL = profileURL;
        }
        // this.rows = this.userList.Collection;
         var departments =data.M_OrganizationList;
         for(var i=0;i<=departments.length;i++)
        {
          if(i==0)
          {
            var param={};
            if(this.isEngLang){
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
        this.rows = data.Collection;
        this.config.totalItems = data.Count;
        this.emptyData = false;
      if(this.contactsList && this.contactsList.length == 0) {
        this.emptyData = true;
      }
    });
  }

  public onChangePage(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
    this.page = page;
    this.loadContactsList();
  }

  addContact(){
    // this.contactType = "internal";
    this.formPage = true;
    this.screenStatus = "create";
    // this.router.navigate(['app/contacts/contact-form'], { queryParams: { contactType: this.contactType } });
  }

  viewContact(type, value){
    this.formPage = true;
    this.contactId = value;
    var user = JSON.parse(localStorage.getItem("User"));
    if((user.CanEditContact==null||user.CanEditContact==false)&&type=='edit'){
      this.router.navigate(['/error']);
    }else {
      if(type=='edit'){
      this.screenStatus = "edit";
      // this.router.navigate(['app/contacts/contact-edit'], { queryParams: { contactType: this.contactType, contactId: value } });
      }else{
      this.screenStatus = "View";
      // this.router.navigate(['app/contacts/contact-view'], { queryParams: { contactType: this.contactType, contactId: value } });
      }
    }
  }

  deleteContact(contactId) {
    var user = JSON.parse(localStorage.getItem("User"));
    if(user.CanEditContact==null||user.CanEditContact==false)
    {
      this.router.navigate(['/error']);
    }
    else
    { 
    this.contactId = contactId;
    this.modalService.show(this.template);
    }
  }

  confirmDelete(contactId) {
    this.service.deleteById(contactId, this.UserID).subscribe((data:any) => {
      this.loadContactsList();
      this.closemodal();
    });
  }

  showInternal() {
    this.contactsList = [];
    this.initpage();
    this.Type = 1;
    this.contactType = "internal"
    this.enableInternal = true;
    this.enableExternal = false;
    this.loadContactsList()
  }
  
  showExternal() {
    this.contactsList = [];
    this.initpage();
    this.Type = 2;
    this.contactType = "external";
    this.enableExternal = true;
    this.enableInternal = false;
    this.loadContactsList()
  }

  closemodal(){
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
  }
  
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    var target = event.target ? event.target : event.srcElement;
    
    return true;
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  loadinternalreports(){
    this.bsModalRefin = this.modalService.show(InternalContactReportModalComponent, { class: 'modal-lg' });
  }

  loadexternalreports(){
    this.bsModalRefex = this.modalService.show(ExternalContactReportModalComponent, { class: 'modal-lg' });
  }
}
