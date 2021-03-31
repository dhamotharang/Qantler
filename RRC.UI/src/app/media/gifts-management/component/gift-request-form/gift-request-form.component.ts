import { Component, OnInit, ChangeDetectorRef, Input, ViewChild, TemplateRef, ElementRef, Inject } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { BsModalService, BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { FormBuilder, FormGroup, Validators, ValidatorFn, AbstractControl, FormControl } from '@angular/forms';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { GiftsManagementService } from '../../service/gifts-management.service';
import { GiftRequest } from '../../model/gift-request/gift-request.model';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { ModalComponent } from 'src/app/modal/modalcomponent/modal.component';
import { HttpEventType } from '@angular/common/http';
import { Attachment } from 'src/app/shared/model/attachment/attachment.model';
import { Subject, Observable, concat, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap, catchError, switchMap } from 'rxjs/operators';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { EndPointService } from 'src/app/api/endpoint.service';
import { DOCUMENT } from '@angular/platform-browser';

@Component({
  selector: 'app-gift-request-form',
  templateUrl: './gift-request-form.component.html',
  styleUrls: ['./gift-request-form.component.scss']
})
export class GiftRequestFormComponent implements OnInit {
  @Input() mode: string;
  @Input() requestId: number;
  @ViewChild('template') template: TemplateRef<any>;
  @ViewChild('templateConfirm') templateConfirm: TemplateRef<any>;
  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild(BsModalRef) modalComp: BsModalRef;
  @ViewChild('confirmationFileInput') confirmationFileInput: ElementRef;
  bsModalRef: BsModalRef;
  bsModalRefConfirm: BsModalRef;
  screenStatus = 'Create Gifts';
  department = [];
  lang: string;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  giftRequestModel: GiftRequest = {
    ReferenceNumber: null,
    GiftID: 0,
    SourceOU: '',
    SourceName: '',
    CreatedBy: 0,
    UpdatedBy: null,
    CreatedDateTime: new Date(),
    UpdatedDateTime: null,
    Status: 0,
    Action: '',
    Attachments: [],
    GiftType: '1',
    HistoryLog: []
  };
  attachmentFiles = [];
  giftFormGroup: FormGroup = this._formBuilder.group({
    ReferenceNumber: [this.giftRequestModel && this.giftRequestModel.ReferenceNumber || null],
    GiftID: [this.giftRequestModel && this.giftRequestModel.GiftID || 0],
    SourceOU: [this.giftRequestModel && this.giftRequestModel.SourceOU || ''],
    SourceName: [this.giftRequestModel && this.giftRequestModel.SourceName || ''],
    CreatedDateTime: [this.giftRequestModel && this.giftRequestModel.CreatedDateTime || new Date()],
    CreatedBy: [this.giftRequestModel && this.giftRequestModel.CreatedBy || ''],
    GiftType: [this.giftRequestModel && this.giftRequestModel.GiftType || '1', Validators.required],
    PurchasedBy: [this.giftRequestModel && this.giftRequestModel.PurchasedBy || ''],
    PurchasedToName: [this.giftRequestModel && this.giftRequestModel.PurchasedToName || ''],
    PurchasedToOrganization: [this.giftRequestModel && this.giftRequestModel.PurchasedToOrganisation || ''],
    RecievedDate: [this.giftRequestModel && this.giftRequestModel.RecievedDate || ''],
    RecievedFromName: [this.giftRequestModel && this.giftRequestModel.RecievedFromName || ''],
    RecievedFromOrganization: [this.giftRequestModel && this.giftRequestModel.RecievedFromOrganization || ''],
    Attachments: [[], Validators.required]
  });

  confirmModalFormGroup: FormGroup = this._formBuilder.group({
    HandedOverTo: ['', [Validators.required, this.emptyValidator]],
    HandedOverDate: ['', [Validators.required]],
    Attachment: ['', [Validators.required]]
  });
  message: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  submitBtn = false;
  sendBtn = false;
  confirmBtn = false;
  isApiLoading: boolean = false;
  uploadProcess: boolean = false;
  uploadPercentage: number;
  confirmationUploadProcess: boolean = false;
  confirmationUploadPercentage: number;
  isStartEndDiff: boolean = false;
  giftReqModal = {
    modalTitle: '',
    message: ''
  };
  isProtocolDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 4;
  isProtocolDepartmentTeamUserID = this.currentUser.OrgUnitID == 4 && !this.currentUser.IsOrgHead;
  attachments: any = [];
  confirmationAttachments: any = [];
  img_file: any[] = [];
  confirm_img_file: any[] = [];
  userFilterInput$ = new Subject<string>();
  allUserList$: Observable<any>;
  userListLoading: boolean = false;

  constructor(private changeDetector: ChangeDetectorRef,
    public common: CommonService,
    public router: Router,
    private route: ActivatedRoute,
    public datepipe: DatePipe,
    private modalService: BsModalService,
    private modalServiceConfirm: BsModalService,
    private _formBuilder: FormBuilder,
    private giftsManagementService: GiftsManagementService,
    private utilsService: UtilsService,
    private dropDownService: DropdownsService,
    @Inject(DOCUMENT) private docs: Document) { }

  ngOnInit() {
    debugger
    this.lang = this.common.currentLang;
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.route.params.subscribe(params => {
      if (params['id']) {
        if (this.lang === 'en') {
          this.common.breadscrumChange('Gift Request', 'View', '');
        } else {
          this.common.breadscrumChange('ترك طلب', 'رأي', '');
        }
        this.isApiLoading = true;
        this.loadData(this.requestId);
      }
    });
    if (this.mode == 'create') {
      if (this.lang === 'en') {
        this.common.breadscrumChange('Gifts', 'Create Gifts', '');
      } else {
        this.common.breadscrumChange('الهدايا', this.arabic('creategift'), '');
      }
      this.isApiLoading = true;
      this.initPage();
    } else {
      this.giftFormGroup.disable();
    }
    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
    this.giftsManagementService.getGiftRequestById(0, 0).subscribe((data: any) => {
      this.department = data.OrganizationList;
    });
    // this.getUserList();
    this.initGiftFormValidations();
    this.setGiftFormValidations();
    this.allUserList$ = this.dropDownService.getUserList({ OrganizationID: null, OrganizationUnits: null }).pipe(
      catchError(() => of([])), // empty list on error
      tap());
  }

  async loadData(id) {
    this.giftsManagementService.getGiftRequestById(id, this.currentUser.id).subscribe((giftRequestRes: GiftRequest) => {
      this.giftRequestModel = giftRequestRes;
      this.setFormData(this.giftRequestModel);
      this.isApiLoading = false;
      if (this.giftRequestModel.Status == 117 && !this.giftRequestModel.IsSend && this.currentUser.OrgUnitID == 4) {
        if (this.lang === 'en') {
          this.common.breadscrumChange('Gifts', 'Send For Delivery', '');
        } else {
          this.common.breadscrumChange('الهدايا', this.arabic('sendfordelivery'), '');
        }
      }
      if (this.giftRequestModel.Status == 118 && this.giftRequestModel.CreatedBy == this.currentUser.id) {
        if (this.lang === 'en') {
          this.common.breadscrumChange('Gifts', 'Confirm Delivery', '');
        } else {
          this.common.breadscrumChange('الهدايا', this.arabic('confirmdelivery'), '');
        }
      }
    }, (err: any) => {
      this.initPage();
    });
  }

  setData(data) {
    this.giftRequestModel = data;
  }

  closemodal() {
    this.modalService.hide(1);
    setTimeout(function () { location.reload(); }, 1000);
  }


  initPage() {
    this.giftRequestModel.GiftID = 0;
    this.giftRequestModel.SourceOU = this.currentUser.department;
    this.giftRequestModel.SourceName = this.currentUser.username;
    this.giftRequestModel.Attachments = [];
    this.giftRequestModel.CreatedBy = this.currentUser.id;
    this.giftRequestModel.CreatedDateTime = new Date();
    this.giftRequestModel.UpdatedDateTime = null;
    this.isApiLoading = false;
    // this.giftRequestModel.Status = 0;
    this.buttonControl();
    this.setFormData(this.giftRequestModel);
  }

  giftAttachments(event) {
    this.img_file = event.target.files;
    if (this.img_file.length > 0) {
      this.isApiLoading = true;
      this.uploadProcess = true;
      this.giftsManagementService.uploadGiftRequestAttachment(this.img_file).subscribe((attachementRes: any) => {
        this.isApiLoading = false;
        if (attachementRes.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(attachementRes.loaded / attachementRes.total) * 100;
        } else if (attachementRes.type === HttpEventType.Response) {
          this.isApiLoading = false;
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          for (var i = 0; i < attachementRes.body.FileName.length; i++) {
            if (this.giftRequestModel && this.giftRequestModel.GiftID) {
              this.attachments.push({ 'AttachmentGuid': attachementRes.body.Guid, 'AttachmentsName': attachementRes.body.FileName[i], 'GiftID': this.giftRequestModel.GiftID, currentUpload: true });
            } else {
              this.attachments.push({ 'AttachmentGuid': attachementRes.body.Guid, 'AttachmentsName': attachementRes.body.FileName[i], 'GiftID': 0, currentUpload: true });
            }
          }
          this.giftRequestModel.Attachments = this.attachments;
          this.giftFormGroup.patchValue({
            Attachments: this.attachments
          });
          this.fileInput.nativeElement.value = '';
        }
      });
    }
  }


  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.giftFormGroup.patchValue({
      Attachments: this.attachments
    });
    this.fileInput.nativeElement.value = "";
  }



  buttonControl() {
    if (this.mode == 'create') {
      this.submitBtn = true;
      this.giftFormGroup.controls['CreatedBy'].disable();
      this.giftFormGroup.controls['ReferenceNumber'].disable();
    } else if (this.mode == 'view') {
      this.changeDetector.detectChanges();
      this.giftFormGroup.disable();
      this.submitBtn = false;
      this.sendBtn = false;
      this.confirmBtn = false;
      if (this.giftRequestModel && (this.giftRequestModel.GiftID > 0) || (this.isProtocolDepartmentHeadUserID || this.isProtocolDepartmentTeamUserID)) {
        this.changeDetector.detectChanges();
        this.giftFormGroup.disable();
      }
      if (this.giftRequestModel.Status == 117 && !this.giftRequestModel.IsSend && this.currentUser.OrgUnitID == 4) {
        this.screenStatus = 'Send For Delivery';
        this.sendBtn = true;
        this.changeDetector.detectChanges();
        this.giftFormGroup.disable();
      }
      if (this.giftRequestModel.Status == 118 && this.giftRequestModel.CreatedBy == this.currentUser.id) {
        this.screenStatus = 'Confirm Delivery';
        this.confirmBtn = true;
        this.changeDetector.detectChanges();
        this.giftFormGroup.disable();
      }
      if (this.giftRequestModel.Status != 117 && this.giftRequestModel.Status != 118) {
        this.screenStatus = 'View Gifts';
      }
    }
    if (this.giftFormGroup.get('GiftType').value == '1') {
      this.giftFormGroup.get('PurchasedBy').clearValidators();
      this.giftFormGroup.get('PurchasedBy').updateValueAndValidity();
      this.giftFormGroup.get('PurchasedToName').clearValidators();
      this.giftFormGroup.get('PurchasedToName').updateValueAndValidity();
      this.giftFormGroup.get('PurchasedToOrganization').clearValidators();
      this.giftFormGroup.get('PurchasedToOrganization').updateValueAndValidity();
      this.giftFormGroup.get('RecievedDate').setValidators([Validators.required]);
      this.giftFormGroup.get('RecievedDate').updateValueAndValidity();
      this.giftFormGroup.get('RecievedFromName').setValidators([Validators.required]);
      this.giftFormGroup.get('RecievedFromName').updateValueAndValidity();
      this.giftFormGroup.get('RecievedFromOrganization').setValidators([Validators.required]);
      this.giftFormGroup.get('RecievedFromOrganization').updateValueAndValidity();
    } else if (this.giftFormGroup.get('GiftType').value == '2') {
      this.giftFormGroup.get('PurchasedBy').setValidators([Validators.required]);
      this.giftFormGroup.get('PurchasedBy').updateValueAndValidity();
      this.giftFormGroup.get('PurchasedToName').setValidators([Validators.required]);
      this.giftFormGroup.get('PurchasedToName').updateValueAndValidity();
      this.giftFormGroup.get('PurchasedToOrganization').setValidators([Validators.required]);
      this.giftFormGroup.get('PurchasedToOrganization').updateValueAndValidity();
      this.giftFormGroup.get('RecievedDate').clearValidators();
      this.giftFormGroup.get('RecievedDate').updateValueAndValidity();
      this.giftFormGroup.get('RecievedFromName').clearValidators();
      this.giftFormGroup.get('RecievedFromName').updateValueAndValidity();
      this.giftFormGroup.get('RecievedFromOrganization').clearValidators();
      this.giftFormGroup.get('RecievedFromOrganization').updateValueAndValidity();
    }
    this.changeDetector.detectChanges();
    this.giftFormGroup.updateValueAndValidity();
  }



  popup(status: any) {
    this.bsModalRef = this.modalService.show(ModalComponent);
    this.bsModalRef.content.status = status;
  }

  formatPatch(val, comment, approver?: any) {
    var data = [{
      "value": val,
      "path": 'Action',
      "op": "Replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "Replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "Replace"
    },
    {
      "value": comment,
      "path": "Comments",
      "op": "Replace"
    }];
    if (val == 'Escalate') {
      data.push({
        "value": approver,
        "path": 'ApproverId',
        "op": "Replace"
      });
    }
    return data;
  }

  setFormData(giftRequestData?: GiftRequest) {

    this.giftFormGroup = this._formBuilder.group({
      ReferenceNumber: [giftRequestData && giftRequestData.ReferenceNumber || null],
      GiftID: [giftRequestData && giftRequestData.GiftID || 0],
      SourceOU: [giftRequestData && giftRequestData.SourceOU || this.currentUser.department],
      SourceName: [giftRequestData && giftRequestData.SourceName || this.currentUser.username],
      CreatedDateTime: [giftRequestData && giftRequestData.CreatedDateTime && new DatePipe(navigator.language).transform(new Date(giftRequestData.CreatedDateTime), 'dd/MM/yyyy') || new Date()],
      CreatedBy: [this.giftRequestModel && this.giftRequestModel.CreatedBy || ''],
      GiftType: [this.giftRequestModel && this.giftRequestModel.GiftType.toString() || '1', Validators.required],
      PurchasedBy: [this.giftRequestModel && this.giftRequestModel.PurchasedBy || ''],
      PurchasedToName: [this.giftRequestModel && this.giftRequestModel.PurchasedToName || ''],
      PurchasedToOrganization: [this.giftRequestModel && this.giftRequestModel.PurchasedToOrganisation || ''],
      RecievedDate: [this.giftRequestModel && this.giftRequestModel.RecievedDate && new DatePipe(navigator.language).transform(new Date(this.giftRequestModel.RecievedDate), 'dd/MM/yyyy') || ''],
      RecievedFromName: [this.giftRequestModel && this.giftRequestModel.RecievedFromName || ''],
      RecievedFromOrganization: [this.giftRequestModel && this.giftRequestModel.RecievedFromOrganization || ''],
      Attachments: [[], Validators.required]
    });
    this.giftFormGroup.patchValue({ Attachments: this._formBuilder.array(giftRequestData && giftRequestData.GiftPhotos || new Array<Attachment>()) });
    if (giftRequestData && giftRequestData.GiftID) {
      this.attachments = [];
      if (giftRequestData.GiftPhotos) {
        this.attachments = giftRequestData.GiftPhotos;
      }
      this.dropDownService.getUserList({ OrganizationID: null, OrganizationUnits: null }).subscribe((userList: any) => {
        userList.forEach((uObj) => {
          if (uObj.userID == this.giftRequestModel.CreatedBy) {
            this.giftFormGroup.patchValue({
              SourceName: uObj.EmployeeName
            });
          }
        });
      });
    }

    this.buttonControl();
  }

  userAction(actionType: string) {
    debugger
    this.isApiLoading = true;
    let giftRequestBody: GiftRequest = {
      // SourceOU:this.giftFormGroup.controls['SourceOU'].value,
      // SourceName:this.giftFormGroup.controls['SourceName'].value,
      GiftPhotos: this.attachments,
      GiftType: parseInt(this.giftFormGroup.controls['GiftType'].value),
      PurchasedBy: this.giftFormGroup.controls['PurchasedBy'].value,
      PurchasedToName: this.giftFormGroup.controls['PurchasedToName'].value,
      PurchasedToOrganisation: this.giftFormGroup.controls['PurchasedToOrganization'].value,
      RecievedDate: this.giftFormGroup.controls['RecievedDate'].value,
      RecievedFromName: this.giftFormGroup.controls['RecievedFromName'].value,
      RecievedFromOrganization: this.giftFormGroup.controls['RecievedFromOrganization'].value
    };
    if (giftRequestBody.GiftType == '1') {
      giftRequestBody.PurchasedBy = null;
      giftRequestBody.PurchasedToName = null;
      giftRequestBody.PurchasedToOrganisation = null;
    } else if (giftRequestBody.GiftType == '2') {
      giftRequestBody.RecievedDate = null;
      giftRequestBody.RecievedFromName = null;
      giftRequestBody.RecievedFromOrganization = null;
    }
    if (actionType != 'submit') {
      if (this.giftRequestModel.GiftID) {
        giftRequestBody.GiftID = this.giftRequestModel.GiftID;
        giftRequestBody.UpdatedBy = this.currentUser.id;
        giftRequestBody.UpdatedDateTime = new Date();
        if (this.giftRequestModel.Status == 117 && !this.giftRequestModel.IsSend) {
          if (this.common.currentLang != 'ar') {
            this.message = 'Gift Sent For Delivery Successfully';
          } else if (this.common.currentLang == 'ar') {
            this.message = this.arabic('giftsentfordeliverysuccessfully');
          }

          this.giftsManagementService.sendGiftForDelivery(this.giftRequestModel.GiftID, giftRequestBody).subscribe((giftRes: any) => {
            if (giftRes) {
              this.giftsManagementService.downloadDeliveryNote(this.giftRequestModel.ReferenceNumber).subscribe((pdfRes: Blob) => {
                let dateVal = new Date(), cur_date = dateVal.getDate() + '-' + (dateVal.getMonth() + 1) + '-' + dateVal.getFullYear();
                var url = window.URL.createObjectURL(pdfRes);
                var a = document.createElement('a');
                document.body.appendChild(a);
                a.setAttribute('style', 'display: none');
                a.href = url;
                a.download = 'Delivery Note PDF-' + this.giftRequestModel.ReferenceNumber + '.pdf';
                a.click();
                window.URL.revokeObjectURL(url);
                a.remove();
                this.common.deleteGeneratedFiles('files/delete', this.giftRequestModel.ReferenceNumber + '.pdf').subscribe(result => {
                  console.log(result);
                });
                this.bsModalRef = this.modalService.show(SuccessComponent);
                this.bsModalRef.content.message = this.message;
                let newSubscriber = this.modalService.onHide.subscribe(r => {
                  newSubscriber.unsubscribe();
                  this.router.navigate(['app/media/gifts-management/dashboard']);
                });
                this.loadData(this.requestId);
              });
            }
          });
        } else if (this.giftRequestModel.Status == 118) {
          let confirmationRequestBody: any = {
            GiftID: this.giftRequestModel.GiftID,
            HandedOverDate: this.confirmModalFormGroup.value.HandedOverDate,
            HandedOverTo: this.confirmModalFormGroup.value.HandedOverTo,
            Attachment: this.confirmModalFormGroup.value.Attachment,
            UpdatedBy: this.currentUser.id,
            UpdatedDateTime: new Date()
          };
          if (this.common.currentLang != 'ar') {
            this.message = 'Gift Delivery Confirmed Successfully';
          } else if (this.common.currentLang == 'ar') {
            this.message = this.arabic('giftdeliveryconfirmedsuccessfully');
          }
          this.giftsManagementService.confirmGiftDelivery(confirmationRequestBody).subscribe((giftRes: any) => {
            if (giftRes.GiftID) {
              console.log("navigation confirm submit successfully");
              // this.bsModalRef = this.modalServiceConfirm.show(SuccessComponent);
              // this.bsModalRef.content.message = this.message;
              // console.log(this.modalServiceConfirm);
              // console.log(this.bsModalRefConfirm);
              // let newSubscriber = this.modalServiceConfirm.onHide.subscribe(r => {
              //   newSubscriber.unsubscribe();
              //   console.log("navigation popup successfully");
              //   this.router.navigate(['app/media/gifts-management/dashboard']);
              // });
              this.bsModalRef = this.modalService.show(this.templateConfirm, { class: 'modal-md' });
              this.loadData(this.requestId);
            }
          });
        }
      }
    } else {
      giftRequestBody.Action = 'Submit';
      giftRequestBody.CreatedBy = this.currentUser.id;
      giftRequestBody.CreatedDateTime = new Date();
      // giftRequestBody.Action = 'Submit';
      if (this.common.currentLang != 'ar') {
        this.message = 'Gift Request Submitted Successfully';
      } else if (this.common.currentLang == 'ar') {
        this.message = this.arabic('giftrequestsuccessmsg');
      }
      this.giftsManagementService.addGiftRequest(giftRequestBody).subscribe((giftRes: any) => {
        if (giftRes.GiftID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(r => {
            newSubscriber.unsubscribe();
            this.router.navigate(['app/media/gifts-management/dashboard']);
          });
          this.giftFormGroup.reset();
        }
      });
    }
  }

  getUserList() {
    this.allUserList$ = concat(
      of([]), // default items
      this.userFilterInput$.pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(() => this.userListLoading = true),
        switchMap(term => this.dropDownService.getUserList({ OrganizationID: null, OrganizationUnits: null }).pipe(
          catchError(() => of([])), // empty list on error
          tap(() => this.userListLoading = false)
        ))
      )
    );
  }

  deliveryConfirmationDialog() {
    this.giftReqModal.modalTitle = (this.common.language == 'English') ? 'Confirm Delivery' : this.arabic('confirmdelivery');
    if (this.common.currentLang == 'ar') {
      this.message = this.arabic('confirmdelivery');
    }
    this.confirmationAttachments = [];
    this.confirmModalFormGroup.reset();
    this.confirmModalFormGroup.patchValue({
      HandedOverDate: ''
    });
    this.bsModalRef = this.modalService.show(this.template, { class: 'modal-lg' });
  }

  confirmDeliveryRequest() {
    debugger
    console.log("gift confirm inprogress");
    this.bsModalRef.hide();
    this.userAction('confirm');
  }

  closemodalconfirm(){
    this.bsModalRef.hide();    
    this.router.navigate(['app/media/gifts-management/dashboard']);
  }
  cancelConfirmation() {
    this.confirmationAttachments = [];
    this.confirmModalFormGroup.reset();
    this.confirmModalFormGroup.patchValue({
      HandedOverDate: ''
    });
    this.changeDetector.detectChanges();
    this.bsModalRef.hide();
  }

  addconfirmationAttachments(event) {
    this.confirm_img_file = event.target.files;
    if (this.confirm_img_file.length > 0) {
      this.isApiLoading = true;
      this.confirmationUploadProcess = true;
      this.giftsManagementService.uploadGiftRequestAttachment(this.confirm_img_file).subscribe((attachementRes: any) => {
        this.isApiLoading = false;
        if (attachementRes.type === HttpEventType.UploadProgress) {
          this.confirmationUploadPercentage = Math.round(attachementRes.loaded / attachementRes.total) * 100;
        } else if (attachementRes.type === HttpEventType.Response) {
          this.isApiLoading = false;
          this.confirmationUploadProcess = false;
          this.confirmationUploadPercentage = 0;
          for (var i = 0; i < attachementRes.body.FileName.length; i++) {
            if (this.giftRequestModel && this.giftRequestModel.GiftID) {
              this.confirmationAttachments.push({ 'AttachmentGuid': attachementRes.body.Guid, 'AttachmentsName': attachementRes.body.FileName[i], 'GiftID': this.giftRequestModel.GiftID, currentUpload: true });
            } else {
              this.confirmationAttachments.push({ 'AttachmentGuid': attachementRes.body.Guid, 'AttachmentsName': attachementRes.body.FileName[i], 'GiftID': 0, currentUpload: true });
            }
          }
          this.confirmModalFormGroup.patchValue({
            Attachment: this.confirmationAttachments
          });
          console.log(this.docs.querySelectorAll('.gift-confirm-dialog-upload'));
          let confirmationFileInputVal: any = this.docs.querySelectorAll('.gift-confirm-dialog-upload')[0];
          confirmationFileInputVal.value = "";
        }
      });
    }
  }


  deleteConfirmationAttachment(index) {
    this.confirmationAttachments.splice(index, 1);
    this.confirmModalFormGroup.patchValue({
      Attachment: this.confirmationAttachments
    });
    let confirmationFileInputVal: any = this.docs.querySelectorAll('.gift-confirm-dialog-upload')[0];
    confirmationFileInputVal.value = "";
  }

  initGiftFormValidations() {
    if (this.giftFormGroup.get('GiftType').value == '1') {
      this.giftFormGroup.get('PurchasedBy').clearValidators();
      this.giftFormGroup.get('PurchasedBy').updateValueAndValidity();
      this.giftFormGroup.get('PurchasedToName').clearValidators();
      this.giftFormGroup.get('PurchasedToName').updateValueAndValidity();
      this.giftFormGroup.get('PurchasedToOrganization').clearValidators();
      this.giftFormGroup.get('PurchasedToOrganization').updateValueAndValidity();
      this.giftFormGroup.get('RecievedDate').setValidators([Validators.required]);
      this.giftFormGroup.get('RecievedDate').updateValueAndValidity();
      this.giftFormGroup.get('RecievedFromName').setValidators([Validators.required, this.emptyValidator]);
      this.giftFormGroup.get('RecievedFromName').updateValueAndValidity();
      this.giftFormGroup.get('RecievedFromOrganization').setValidators([Validators.required, this.emptyValidator]);
      this.giftFormGroup.get('RecievedFromOrganization').updateValueAndValidity();
    } else if (this.giftFormGroup.get('GiftType').value == '2') {
      this.giftFormGroup.get('PurchasedBy').setValidators([Validators.required]);
      this.giftFormGroup.get('PurchasedBy').updateValueAndValidity();
      this.giftFormGroup.get('PurchasedToName').setValidators([Validators.required]);
      this.giftFormGroup.get('PurchasedToName').setValidators([Validators.required, this.emptyValidator]);
      this.giftFormGroup.get('PurchasedToName').updateValueAndValidity();
      this.giftFormGroup.get('PurchasedToOrganization').setValidators([Validators.required, this.emptyValidator]);
      this.giftFormGroup.get('PurchasedToOrganization').updateValueAndValidity();
      this.giftFormGroup.get('RecievedDate').clearValidators();
      this.giftFormGroup.get('RecievedDate').updateValueAndValidity();
      this.giftFormGroup.get('RecievedFromName').clearValidators();
      this.giftFormGroup.get('RecievedFromName').updateValueAndValidity();
      this.giftFormGroup.get('RecievedFromOrganization').clearValidators();
      this.giftFormGroup.get('RecievedFromOrganization').updateValueAndValidity();
    }
    this.changeDetector.detectChanges();
    this.giftFormGroup.updateValueAndValidity();
  }

  setGiftFormValidations() {
    this.giftFormGroup.get('GiftType').valueChanges.subscribe((values: any) => {
      this.initGiftFormValidations();
    });
  }
  hisLog(status: any) {
    let sts = status.toLowerCase();
    if (this.common.currentLang != 'ar') {
      if (sts == 'submit') {
        return status + 'ted By';
      } else if (sts == 'sendfordelivery') {
        return 'Sent For Delivery By';
      } else if (sts == 'deliveryconfirmed') {
        return 'Delivery Confirmed By';
      }
    } else if (this.common.currentLang == 'ar') {
      let arabicStatusStr = '';
      if (sts == 'submit') {
        arabicStatusStr = sts + 'tedby';
      } else if (sts == 'sendfordelivery') {
        arabicStatusStr = 'sentfordeliveryby';
      } else if (sts == 'deliveryconfirmed') {
        arabicStatusStr = 'deliveryconfirmedby';
      }
      return this.common.arabic.words[arabicStatusStr];
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  private emptyValidator(control: FormControl) {
    let str: any = control.value;
    // return (control: AbstractControl): { [key: string]: boolean } | null => {
    if (str && str.trim() == '') {
      return { 'notEmptyString': true };
    }
    return null;
    // };
  }

}
