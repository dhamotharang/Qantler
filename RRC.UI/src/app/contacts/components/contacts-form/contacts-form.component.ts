import { Component, OnInit, ElementRef, ViewChild, Renderer2, Inject, TemplateRef, Input } from '@angular/core';
import { ContactsService } from '../../service/contacts.service';
import { CommonService } from 'src/app/common.service';
import { HttpEventType } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { DOCUMENT } from '@angular/common';
import { BsModalService } from 'ngx-bootstrap';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { EndPointService } from 'src/app/api/endpoint.service';
import { AdminService } from 'src/app/admin/service/admin/admin.service';

@Component({
  selector: 'app-contacts-form',
  templateUrl: './contacts-form.component.html',
  styleUrls: ['./contacts-form.component.scss']
})
export class ContactsFormComponent implements OnInit {
  @Input() screenStatus: string;
  @Input() contactType: any;
  @Input() contactId: any;
  formData = {
    ContactManagementID: 0,
    Action: "",
    Type: "internal",
    Department: null,
    UserName: "",
    EntityName: "",
    Designation: "",
    EmailId: "",
    PhoneNumber: "",
    OfficialEntity: null,
    CreatedBy: 0,
    CreatedDateTime: new Date(),
    Comments: "",
    Section: null,
    Unit: null,
    PhoneNumberExtension: "",
    Attachments: [
      {
        AttachmentGuid: "",
        AttachmentsName: "",
        ContactID: 0
      }
    ],
    ProfilePhotoID: "",
    ProfilePhotoName: ""
  }
  placeholderselect: string = "";
  attachments: any = [];
  uploadProcess: boolean = false;
  uploadPercentage: number;
  @ViewChild('variable') myInputVariable: ElementRef;
  @ViewChild('myScrollElem') myScrollElem: ElementRef;
  @ViewChild('template') template: TemplateRef<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  departmentList: any = [];
  message: string;
  // contactId: any;
  ImageUrl: string;
  photoattachments: any = [];
  screenMsg: string;
  successMsg: boolean = false;
  valid: boolean = false;
  lang: any;
  isEngLang: boolean = true;
  Headername: string = "";
  sectionList: any = [];
  unitList: any = [];
  placeholdersection: string;
  placeholderunit: string;
  windowScrolled: boolean;
  PhoneNumberValidation: string = "";
  ExtensionNumberValidation: boolean = false;
  constructor(private adminService: AdminService, private endpoint: EndPointService, public utils: UtilsService, public modalService: BsModalService, private renderer: Renderer2, @Inject(DOCUMENT) private document: Document, public router: Router, public service: ContactsService, public common: CommonService, private route: ActivatedRoute, ) {
  }

  ngOnInit() {
    setTimeout(() => {
      let viewHeight = this.myScrollElem.nativeElement.offsetHeight;
      this.myScrollElem.nativeElement.scrollIntoView({ behavior: 'smooth' });
    }, 100);
    this.lang = this.common.currentLang;
    if (this.lang == 'en') {
      this.isEngLang = true;
      this.placeholderselect = "Select Department";
      this.placeholdersection = "Select Section";
      this.placeholderunit = "Select Unit";
      if (this.contactType == 'external') {
        if (this.screenStatus == "create") {
          this.Headername = "EXTERNAL CONTACT CREATION"
        } else if (this.screenStatus == "edit") {
          this.Headername = "EXTERNAL CONTACT EDIT"
        } else {
          this.Headername = "EXTERNAL CONTACT VIEW"
        }
      } else {
        if (this.screenStatus == "create") {
          this.Headername = "INTERNAL CONTACT CREATION"
        } else if (this.screenStatus == "edit") {
          this.Headername = "INTERNAL CONTACT EDIT"
        } else {
          this.Headername = "INTERNAL CONTACT VIEW"
        }

      }

    } else {
      this.isEngLang = false;
      this.placeholderselect = this.arabic("selectrequesterdepartment");
      this.placeholdersection = this.arabic('section');
      this.placeholderunit = this.arabic('unit');
      if (this.contactType == 'external') {
        if (this.screenStatus == "create") {
          this.Headername = this.arabic("externalcontactcreation");
        } else if (this.screenStatus == "edit") {
          this.Headername = this.arabic("externalcontactedit");
        } else {
          this.Headername = this.arabic("externalcontactview");
        }

      } else {
        if (this.screenStatus == "create") {
          this.Headername = this.arabic("internalcontactcreation");
        } else if (this.screenStatus == "edit") {
          this.Headername = this.arabic("internalcontactedit");
        } else {
          this.Headername = this.arabic("internalcontactview");
        }

      }
    }
    this.getDepartments();
    if (this.screenStatus == "create") {
      this.screenMsg = "Creation";
    } else if (this.screenStatus == "edit") {
      this.screenMsg = "Edit";
    } else {
      this.screenMsg = "View";
    }
    if (this.contactId && this.screenStatus != "create") {
      if (this.contactId > 0) {
        this.loadContact();
      }
    }

    // this.route.queryParams.subscribe(params => {
    //   this.contactType = params["contactType"];
    //   this.contactId = params["contactId"];
    //   if(this.contactId > 0){
    //     this.loadContact();
    //   }
    // });
    // this.initPage();
    this.formData.Type = this.contactType;
  }

  initPage() {
    this.formData = {
      ContactManagementID: 0,
      Action: "",
      Type: "",
      Department: null,
      UserName: "",
      EntityName: "",
      Designation: "",
      EmailId: "",
      PhoneNumber: "",
      OfficialEntity: null,
      CreatedBy: 0,
      CreatedDateTime: new Date(),
      Section: 0,
      Unit: 0,
      PhoneNumberExtension: "",
      Comments: "",
      Attachments: [
        {
          AttachmentGuid: "",
          AttachmentsName: "",
          ContactID: 0
        }
      ],
      ProfilePhotoID: "",
      ProfilePhotoName: ""
    }
  }

  loadContact() {
    this.service.getContact(this.contactId, this.currentUser.id)
      .subscribe((response: any) => {
        if (response != null) {
          this.formData = response;
          if (this.contactType == "external") {
            if (this.formData.OfficialEntity == true) {
              this.formData.OfficialEntity = "true";
            } else {
              this.formData.OfficialEntity = "false";
            }
            this.validateExternalForm();
          } else {
            this.validateInternalForm();
          }
          if (this.formData.ProfilePhotoID && this.formData.ProfilePhotoName) {
            this.ImageUrl = this.endpoint.fileDownloadUrl + "?filename=" + this.formData.ProfilePhotoName + "&guid=" + this.formData.ProfilePhotoID;
          }
        }
      });
  }

  ngAfterViewInit() {
    var contactType = '';
    if (this.contactType == "external") {
      contactType = "External";
      this.validateExternalForm();
    } else {
      contactType = "Internal";
      this.validateInternalForm();
    }
    this.common.breadscrumChange('Contacts', contactType + " Creation", '');
  }

  validateInternalForm() {
    var flag = true;
    if (this.utils.isEmptyString(this.formData.UserName) ||
      this.utils.isEmptyString(this.formData.Department) ||
      this.utils.isEmptyString(this.formData.Designation) ||
      this.utils.isEmptyString(this.formData.EmailId) ||
      !this.utils.isEmail(this.formData.EmailId) ||
      this.utils.isEmptyString(this.formData.PhoneNumber)
    ) {
      flag = false
    }
    return flag;
  }

  checkMail() {
    if (!this.utils.isEmail(this.formData.EmailId.trim())) {
      return true;
    }
  }

  validMail() {
    this.valid = false;
    if (!this.utils.isEmail(this.formData.EmailId.trim())) {
      this.valid = true;
    }
  }

  emptyValid() {
    if (this.formData.EmailId.length <= 1) {
      this.valid = false;
    }
  }
  validateExternalForm() {
    var flag = true;
    if (this.utils.isEmptyString(this.formData.EntityName) ||
      this.utils.isEmptyString(this.formData.OfficialEntity)) {
      flag = false
    }
    return flag;
  }


  getDepartments() {
    this.service.getById(0, this.currentUser.id)
      .subscribe((response: any) => {
        if (response != null) {
          this.departmentList = response.OrganizationList;
        }
      });
    this.callSection();
  }

  callSection() {
    this.adminService.getSections(this.currentUser.id).subscribe((secList: any) => {
      this.sectionList = secList;
      this.callUnits();
    });
  }

  callUnits() {
    this.adminService.getUnits(this.currentUser.id).subscribe((unList: any) => {
      this.unitList = unList;
    });
  }

  onSelectFile(event) {
    return true;
  }

  uploadFiles(event) {
    var files = event.target.files;
    let that = this;
    this.uploadProcess = true;
    this.common.postAttachment(files).subscribe((event: any) => {
      if (event.type === HttpEventType.UploadProgress) {
        this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
      } else if (event.type === HttpEventType.Response) {
        this.uploadProcess = false;
        this.uploadPercentage = 0;
        this.photoattachments = [];
        for (var i = 0; i < event.body.FileName.length; i++) {
          this.photoattachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'ContactManagementID': 0 });
        }
        this.formData.ProfilePhotoID = this.photoattachments[0].AttachmentGuid;
        this.formData.ProfilePhotoName = this.photoattachments[0].AttachmentsName;
        this.ImageUrl = this.endpoint.fileDownloadUrl + "?filename=" + this.formData.ProfilePhotoName + "&guid=" + this.formData.ProfilePhotoID;
        this.formData.Attachments = this.attachments;
      }
    }, (error) => {
      this.uploadProcess = false;
    });
    this.myInputVariable.nativeElement.value = "";

  }

  saveContact() {
    if (this.contactType == "external") {
      this.formData.Type = "external";
      if (this.isEngLang) {
        this.message = "External Contact Submitted Successfully";
      } else {
        this.message = this.arabic('externalcontactsubmitsuccessfully');
      }
    } else {
      this.formData.Type = "internal";
      if (this.isEngLang) {
        this.message = "Internal Contact Submitted Successfully";
      } else {
        this.message = this.arabic('internalcontactsubmitsuccessfully');
      }
    }
    this.formData.Action = "Submit";
    if (this.formData.OfficialEntity == "true") {
      this.formData.OfficialEntity = true;
    } else {
      this.formData.OfficialEntity = false;
    }
    this.service.saveContact(this.formData, this.currentUser.id).subscribe((data: any) => {
      if (data) {
        this.modalService.show(this.template);
        let newSubscriber = this.modalService.onHide.subscribe((data: any) => {
          if (data == "backdrop-click") {
            newSubscriber.unsubscribe();
            this.closemodal();
          }
        });
        this.common.contactChangeTrigger();
      }
    });
  }

  updateContact() {
    if (this.contactType == "external") {
      if (this.isEngLang) {
        this.message = "External Contact Updated Successfully";
      } else {
        this.message = this.arabic('externalcontactupdatedsuccessfully');
      }

      this.formData.Type = "external";
    } else {
      this.formData.Type = "internal";
      if (this.isEngLang) {
        this.message = "Internal Contact Updated Successfully";
      } else {
        this.message = this.arabic('internalcontactupdatedsuccessfully');
      }

    }
    if (this.formData.OfficialEntity == "true") {
      this.formData.OfficialEntity = true;
    } else {
      this.formData.OfficialEntity = false;
    }
    this.formData.ContactManagementID = this.contactId;
    this.service.updateContact(this.formData, this.currentUser.id).subscribe((data: any) => {
      this.loadContact();
      if (data) {
        this.modalService.show(this.template);
        let newSubscriber = this.modalService.onHide.subscribe((data: any) => {
          if (data == "backdrop-click") {
            newSubscriber.unsubscribe();
            this.closemodal();
          }
        });
      }
    });
  }

  closepage() {
    this.renderer.removeClass(this.document.body, 'modal-open');
    this.successMsg = true;

  }
  closemodal() {
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    this.successMsg = true;
    // this.router.navigate(['app/contacts/contacts'],  { queryParams: { contactType: this.contactType} });
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

  numberOnlyPhoneNumber(event): boolean {
    debugger
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    var target = event.target ? event.target : event.srcElement;
    if (event.keyCode == 8 ? (event.target.value.length - 1 < 9) : (event.target.value.length != 0 && event.target.value.length < 8)) {
      this.PhoneNumberValidation = "1";
    }
    else if (event.keyCode == 8 ? (event.target.value.length - 1 > 14) : (event.target.value.length != 0 && event.target.value.length > 13)) {
      this.PhoneNumberValidation = "2";
    }
    else {
      this.PhoneNumberValidation = "0";
    }
    
    return true;
  }
}
