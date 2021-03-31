import { Component, OnInit, ViewChild, TemplateRef, ChangeDetectorRef, ElementRef } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { FormGroup, FormControl } from '@angular/forms';
import { CreateMemoModal } from '../create-memo/create-memo.modal';
import { MemoService } from '../../../services/memo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MasterData } from '../create-memo/masterdata';
import { SampleData } from '../create-memo/sampleDB';
import 'tinymce';
import { async } from 'q';
import { DatePipe } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../../../../modal/modalcomponent/modal.component';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { CommonService } from '../../../../common.service';
import { HttpEventType } from '@angular/common/http';


declare var tinymce: any;
import { CreateMemoComponent } from '../create-memo/create-memo.component';

@Component({
  selector: 'app-create-memo-rtl',
  templateUrl: './create-memo.component-rtl.html',
  styleUrls: ['./create-memo.component-rtl.scss']
})
export class CreateMemoComponentRTL extends CreateMemoComponent {
  // switch(this.screenStatus) {
  //         case 'Create':
  //   break;
  //         case 'View':
  //   this.screenTitle = 'رأي';
  //   break;
  //         case 'Edit':
  //   this.screenTitle = 'تصحيح';
  //   break;
  // }
}
// implements OnInit {
//   @ViewChild('variable') myInputVariable: ElementRef;
//   @ViewChild('template') template: TemplateRef<any>;
//   @ViewChild('printContent') printContent: ElementRef<any>;
//   @ViewChild('tinyDetail') tinyDetail: ElementRef;
//   bsModalRef: BsModalRef;
//   createMemo: CreateMemoModal = new CreateMemoModal();
//   screenStatus = 'Create';
//   masterData: MasterData = new MasterData();
//   memoData: any = {
//     HistoryLog: []
//   };
//   config = {
//     backdrop: true,
//     ignoreBackdropClick: true
//   };
//   status = this.masterData.data.status;
//   user = [];//this.masterData.data.user;
//   department = [];//this.masterData.data.department;
//   dropdownOptions = ['one', 'two', 'three', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'];
//   privateOptions = ['نعم ', 'لا'];
//   priorityList = ['فائق الأهمية', 'متوسط الأهمية', 'قليل الأهمية', 'غير مهم'];
//   attachmentFiles = [];
//   createdTime: string;
//   createdDate: string;
//   dropdownSettings: any;
//   bsConfig: Partial<BsDatepickerConfig>;
//   colorTheme = 'theme-green';
//   sendDraftBtnLoad = false;
//   sendBtnLoad = false;
//   closeBtnLoad = false;
//   shareBtnLoad = false;
//   printBtnLoad = false;
//   cloneBtnLoad = false;
//   downloadBtnLoad = false;
//   approveBtnLoad = false;
//   deleteBtnLoad = false;
//   returnInfoBtnLoad = false;
//   rejectBtnLoad = false;
//   autocompleteItems = ['s'];
//   btnLoad = false;
//   memoModel = {
//     MemoID: 0,
//     ReferenceNumber: '',
//     Title: '',
//     SourceOU: '',
//     SourceName: '',
//     DestinationOU: [],
//     DestinationUsername: [],
//     ApproverName: 0,
//     ApproverDepartment: 0,
//     Details: '',
//     Private: '',
//     Priority: '',
//     Keywords: [],
//     Comment: '',
//     Attachments: [],
//     AttachmentName: '',
//     DeleteFlag: '',
//     CreatedBy: '',
//     UpdatedBy: '',
//     CreatedDateTime: new Date(),
//     UpdatedDateTime: '',
//     Status: 0,
//   }
//   img_file: any;
//   message: any;
//   currentUser: any = JSON.parse(localStorage.getItem('User'));
//   attachments: any = [];
//   userDestination: any;
//   userReceiver: any;
//   commonMes: any;
//   DestinationDepartmentId: any[];
//   pdfSrc: string;
//   showPdf: boolean = false;
//   uploadProcess: boolean = false;
//   uploadPercentage: number;
//   currentId: any;
//   screenTitle: string;
//   constructor(private changeDetector: ChangeDetectorRef, private memoservice: MemoService, public common: CommonService, public router: Router, route: ActivatedRoute, public datepipe: DatePipe,
//     private modalService: BsModalService) {
//     route.url.subscribe(() => {
//       console.log(route.snapshot.data);
//       this.screenStatus = route.snapshot.data.title;
//     });
//     route.params.subscribe(param => {
//       var id = +param.id;
//       if (id > 0)
//         this.loadData(id, this.currentUser.id);
//       this.memoservice.breadscrumChange(1, this.screenStatus, id, 'ar');
//     });
//     this.memoservice.getMemo('memo', 0, 0).subscribe((data: any) => {
//       this.department = data.OrganizationList;
//     });
//     switch (this.screenStatus) {
//       case 'Create':
//         this.screenTitle = 'خلق';
//         break;
//       case 'View':
//         this.screenTitle = 'رأي';
//         break;
//       case 'Edit':
//         this.screenTitle = 'تصحيح';
//         break;
//     }
//   }

//   async loadData(id, userid) {
//     this.currentId = id;
//     await this.memoservice.getMemo('memo', id, userid).subscribe((data: any) => {
//       this.memoData = data;
//       if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
//         let that = this;
//         this.status = this.memoData.M_LookupsList;
//         var date = this.memoModel.CreatedDateTime;
//         this.memoModel.CreatedDateTime = new Date(date);
//         this.setData(this.memoData);
//         this.bottonControll();
//       } else {
//         this.initPage();
//         this.bottonControll();
//       }
//     });
//   }

//   setData(data) {
//     this.getDestUserList(+data.ApproverDepartmentID);
//     this.getRecvPrepareUserList(data.DestinationDepartmentID);
//     this.memoModel.MemoID = data.MemoID;
//     this.memoModel.ReferenceNumber = data.ReferenceNumber
//     this.memoModel.Title = data.Title;
//     this.memoModel.SourceOU = data.SourceOU;
//     this.memoModel.SourceName = data.SourceName;
//     const DestinationOU = [];
//     const DestinationUsername = [];
//     data.DestinationDepartmentID.forEach((department, index) => {
//       DestinationOU.push(department.MemoDestinationDepartmentID);
//     });
//     this.DestinationDepartmentId = DestinationOU;
//     this.memoModel.DestinationOU = DestinationOU;
//     data.DestinationUsernameID.forEach((user, index) => {
//       DestinationUsername.push(user.MemoDestinationUsersID);
//     });
//     this.memoModel.DestinationUsername = DestinationUsername;
//     if (data.CurrentApprover.length > 0) {
//       this.memoModel.ApproverName = data.CurrentApprover[0].ApproverId;
//     }
//     this.memoModel.ApproverDepartment = +data.ApproverDepartmentID;
//     this.memoModel.Details = data.Details;
//     this.tinyDetail.nativeElement.insertAdjacentHTML('beforeend', this.memoModel.Details);
//     this.memoModel.Private = data.Private;
//     this.memoModel.Priority = data.Priority;
//     this.memoModel.CreatedBy = data.CreatedBy;
//     this.attachments = data.Attachments;
//     data.Keywords.forEach(key => {
//       this.memoModel.Keywords.push({ display: key.keywords, value: key.keywords });
//     });

//     this.memoModel.Attachments = data.Attachments;
//     this.memoModel.Status = data.Status;
//     this.memoservice.breadscrumChange(this.memoModel.Status, this.screenStatus, this.currentId, 'ar');
//   }

//   async ngOnInit() {
//     this.bottonControll();
//     this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
//   }

//   ngAfterViewInit() {

//   }

//   closemodal() {
//     this.modalService.hide(1);
//     setTimeout(function () { location.reload(); }, 1000);
//   }


//   initPage() {
//     this.memoModel.MemoID = 0
//     this.memoModel.ReferenceNumber = '';
//     this.memoModel.Title = '';
//     this.memoModel.SourceOU = this.currentUser.department;
//     this.memoModel.SourceName = this.currentUser.username;
//     this.memoModel.DestinationOU = [];
//     this.memoModel.DestinationUsername = [];
//     this.memoModel.ApproverName = 0;
//     this.memoModel.ApproverDepartment = 0;
//     this.memoModel.Details = '';
//     this.memoModel.Private = '';
//     this.memoModel.Priority = '';
//     this.memoModel.Keywords = [];
//     this.memoModel.Comment = '';
//     this.memoModel.Attachments = [];
//     this.memoModel.AttachmentName = '';
//     this.memoModel.DeleteFlag = '';
//     this.memoModel.CreatedBy = this.currentUser.id;
//     this.memoModel.UpdatedBy = '';
//     this.memoModel.CreatedDateTime = new Date();
//     this.memoModel.UpdatedDateTime = '';
//     this.memoModel.Status = 0;
//   }

//   onTextChange(event) {
//     if (event != '')
//       this.memoModel.Keywords.push({ display: event, value: event });
//   }

//   Attachments(event) {
//     var files = event.target.files;
//     if (files.length > 0) {
//       let that = this;
//       this.uploadProcess = true;
//       this.common.postAttachment(files).subscribe((event: any) => {
//         if (event.type === HttpEventType.UploadProgress) {
//           this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
//         } else if (event.type === HttpEventType.Response) {
//           this.myInputVariable.nativeElement.value = "";
//           this.uploadProcess = false;
//           this.uploadPercentage = 0;
//           for (var i = 0; i < event.body.FileName.length; i++) {
//             this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'MemoID': '' });
//           }
//           this.memoModel.Attachments = this.attachments;
//         }
//       });
//     }
//   }
//   selectChange(data) {
//     this.memoModel.DestinationOU;
//   }

//   deleteAttachment(index) {
//     this.attachments.splice(index, 1);
//     // if (this.attachments.length == 0) {
//     this.myInputVariable.nativeElement.value = "";
//     //}
//   }


//   prepareData() {
//     this.memoModel.Keywords.forEach((data, index) => {
//       this.createMemo.Keywords.push({ 'keywords': data.value });
//     });
//     if (typeof this.DestinationDepartmentId !== 'undefined' && this.DestinationDepartmentId.length) {
//       this.DestinationDepartmentId.forEach(data => {
//         this.createMemo.DestinationDepartmentId.push({
//           "MemoDestinationDepartmentID": data,
//           'MemoDestinationDepartmentName': ''
//         });
//       });
//     }
//     this.userName();
//     this.createMemo.Title = this.memoModel.Title;
//     this.createMemo.SourceOU = this.currentUser.department;
//     this.createMemo.SourceName = this.currentUser.username;
//     this.createMemo.ApproverId = this.memoModel.ApproverName;
//     this.createMemo.ApproverDepartmentId = this.memoModel.ApproverDepartment;
//     this.createMemo.Details = this.memoModel.Details;
//     this.createMemo.Private = this.memoModel.Private;
//     this.createMemo.Priority = this.memoModel.Priority;
//     this.createMemo.Attachments = this.memoModel.Attachments;
//     //this.createMemo.Action = this.memoModel.Status+'';
//     this.createMemo.Comments = '';
//     return this.createMemo;
//   }

//   validateForm() {
//     var flag = true;
//     var destination = (this.memoModel.DestinationOU) ? (this.memoModel.DestinationOU.length > 0) : false;
//     var Keywords = (this.memoModel.Keywords) ? (this.memoModel.Keywords.length > 0) : false;
//     var username = (this.memoModel.DestinationUsername) ? (this.memoModel.DestinationUsername.length > 0) : false;

//     if (destination && username && this.memoModel.Title && this.memoModel.ApproverName
//       && this.memoModel.ApproverDepartment && this.memoModel.Details && this.memoModel.Private
//       && this.memoModel.Priority && this.attachments.length > 0 && !this.sendBtnLoad) {
//       flag = false;
//     }
//     return flag;
//   }


//   createBtnShow = false;
//   editBtnShow = false;
//   viewBtnShow = false;
//   approverBtn = false;
//   receiverBtn = false;
//   deleteBtn = false;
//   creatorBtn = false;
//   draftBtn = false;
//   cloneBtn = false;
//   id = '';
//   printbtn = false;

//   bottonControll() {
//     if (this.screenStatus == 'Create') {
//       this.createBtnShow = true;
//       this.printbtn = true;
//     } else if (this.screenStatus == 'Edit') {
//       this.editBtnShow = true;
//     } else if (this.screenStatus == 'View' && this.memoModel.CreatedBy == this.currentUser.id) {
//       this.viewBtnShow = true;
//     }
//     if (this.memoModel.CreatedBy == this.currentUser.id && this.memoModel.Status == 5) {
//       this.creatorBtn = true;
//     }
//     if (this.memoModel.CreatedBy == this.currentUser.id && this.memoModel.Status == 1) {
//       this.draftBtn = true;
//     }
//     if (this.memoModel.CreatedBy == this.currentUser.id && this.memoModel.Status != 1) {
//       this.cloneBtn = true;
//     }
//     if (this.screenStatus == 'View' && this.memoModel.ApproverName == this.currentUser.id && this.memoModel.Status == 2) {
//       this.approverBtn = true;
//     }
//     this.memoModel.DestinationUsername.forEach(element => {
//       if (element == this.currentUser.id && this.memoModel.Status == 3) {
//         this.receiverBtn = true;
//         this.editBtnShow = false;
//       }
//     });
//     if (this.memoModel.CreatedBy == this.currentUser.id && this.memoModel.Status == 1) {
//       this.deleteBtn = true;
//     }
//   }

//   async Destination(event) {
//     this.memoModel.DestinationUsername = [];
//     this.DestinationDepartmentId = this.memoModel.DestinationOU;
//     await this.getRecvUserList(this.DestinationDepartmentId);
//   }

//   userName(event = '') {
//     this.createMemo.DestinationUserId = [];
//     if (this.memoModel.DestinationUsername.length) {
//       this.memoModel.DestinationUsername.forEach(data => {
//         this.createMemo.DestinationUserId.push({
//           'MemoDestinationUsersID': data,
//           'MemoDestinationUsersName': ''
//         });
//       });
//     }
//   }

//   onChangeDepartment() {
//     this.memoModel.ApproverName = 0;
//     this.getDestUserList(+this.memoModel.ApproverDepartment);
//   }

//   async getRecvUserList(departments) {
//     let params = [];
//     let userId = (this.screenStatus == 'Create') ? this.currentUser.id : 0
//     if (departments.length) {
//       departments.forEach(element => {
//         params.push({ "OrganizationID": element, "OrganizationUnits": "string" });
//       });
//       this.common.getUserList(params, userId).subscribe((data: any) => {
//         this.userReceiver = data;
//       });
//     } else {
//       this.userReceiver = [];
//     }
//   }

//   async getRecvPrepareUserList(departments) {
//     let params = [];
//     let userId = (this.screenStatus == 'Create') ? this.currentUser.id : 0
//     if (departments.length) {
//       departments.forEach(element => {
//         params.push({ "OrganizationID": element.MemoDestinationDepartmentID, "OrganizationUnits": "string" });
//       });
//     }
//     this.common.getUserList(params, userId).subscribe((data: any) => {
//       this.userReceiver = data;
//     });
//   }

//   async getDestUserList(id) {
//     if (id) {
//       this.userDestination = [];
//       let userId = (this.screenStatus == 'Create') ? this.currentUser.id : 0
//       let params = [{
//         "OrganizationID": id,
//         "OrganizationUnits": "string"
//       }];
//       this.common.getUserList(params, userId).subscribe((data: any) => {
//         this.userDestination = data;
//       });
//     } else {
//       this.userDestination = [];
//     }
//   }

//   saveMemo(data = '') {
//     if (data == 'draft' && this.memoModel.MemoID == 0) {
//       var requestData = this.prepareData();
//       requestData.CreatedBy = this.currentUser.id;
//       requestData.CreatedDateTime = this.memoModel.CreatedDateTime;
//       requestData.Action = 'Save';
//       this.memoservice.saveMemo('memo', requestData).subscribe(data => {
//         this.sendDraftBtnLoad = false;
//         this.sendBtnLoad = false;
//         this.message = "تم حفظ مسودة المذكرة";
//         this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
//         this.bsModalRef.content.language = 'ar';
//         this.bsModalRef.content.message = this.message;
//         this.bsModalRef.content.pagename = 'memo';
//       });
//     } else {
//       if (this.memoModel.Status == 1 || this.memoModel.Status == 5) {
//         requestData = this.prepareData();
//         requestData.Action = 'Submit';
//         if (data == 'draft') {
//           requestData.Action = 'Save';
//         }
//         if (this.memoModel.Status == 5) {
//           requestData.Action = 'Resubmit';
//         }
//         requestData.MemoID = this.memoModel.MemoID
//         requestData.UpdatedBy = this.currentUser.id;
//         requestData.UpdatedDateTime = this.memoModel.CreatedDateTime;
//         this.memoservice.updateMemo('memo', this.memoModel.MemoID, requestData).subscribe(res => {
//           this.sendDraftBtnLoad = false;
//           this.sendBtnLoad = false;
//           if (this.screenStatus == 'Create' || data != 'draft') {
//             this.message = "تم إرسال المذكرة بنجاح";
//             this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
//             this.bsModalRef.content.language = 'ar';
//             this.bsModalRef.content.message = this.message;
//             this.bsModalRef.content.screenStatus = this.screenStatus;
//             this.bsModalRef.content.pagename = 'memo';

//           } else {
//             this.message = "تم حفظ مسودة المذكرة";
//             this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
//             this.bsModalRef.content.language = 'ar';
//             this.bsModalRef.content.message = this.message;
//             this.bsModalRef.content.pagename = 'memo';
//           }
//         });
//       } else {
//         requestData = this.prepareData();
//         requestData.Action = 'Submit';
//         requestData.CreatedBy = this.currentUser.id;
//         requestData.CreatedDateTime = this.memoModel.CreatedDateTime;
//         this.memoservice.saveMemo('memo', requestData).subscribe(data => {
//           this.sendDraftBtnLoad = false;
//           this.sendBtnLoad = false;
//           if (this.screenStatus == 'Create') {
//             this.message = "تم إرسال المذكرة بنجاح";
//             this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
//             this.bsModalRef.content.language = 'ar';
//             this.bsModalRef.content.message = this.message;
//             this.bsModalRef.content.screenStatus = this.screenStatus;
//             this.bsModalRef.content.pagename = 'memo';

//           } else {
//             this.message = "تم حفظ مسودة المذكرة";
//             this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
//             this.bsModalRef.content.language = 'ar';
//             this.bsModalRef.content.message = this.message;
//             this.bsModalRef.content.pagename = 'memo';
//           }
//         });
//       }
//     }
//   }


//   statusChange(status: any, dialog) {
//     var data = this.formatPatch(status, 'Action')
//     this.commonMes = status;
//     this.memoservice.statusChange('Memo', this.memoModel.MemoID, data).subscribe(data => {
//       this.closeBtnLoad = false;
//       this.shareBtnLoad = false;
//       this.approveBtnLoad = false;
//       this.rejectBtnLoad = false;
//       this.returnInfoBtnLoad = false;
//       this.btnLoad = false;
//       if (status == 'ReturnForInfo') {
//         this.message = "تم إرجاع المذكرة للحصول على معلومات بنجاح";
//       } else if (status == 'Approve') {
//         this.message = "تم اعتماد المذكرة بنجاح";
//       } else if (status == 'Reject') {
//         this.message = "تم رفض المذكرة بنجاح";
//       } else if (status == 'Close') {
//         this.message = "تم إغلاق المذكرة بنجاح";
//       } else {
//         this.message = 'تم اعتماد المذكرة بنجاح';
//       }
//       this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
//       this.bsModalRef.content.language = 'ar';
//       this.bsModalRef.content.message = this.message;
//       this.bsModalRef.content.pagename = 'memo';
//     });
//   }

//   popup(status: any) {
//     this.bsModalRef = this.modalService.show(ModalComponent, this.config);
//     this.bsModalRef.content.language = 'ar';
//     this.bsModalRef.content.status = status;
//     this.bsModalRef.content.button = (status == 'Redirect') ? 'إعادة توجيه' : 'يتصعد';
//     this.bsModalRef.content.screenStatus = this.screenStatus;
//     this.bsModalRef.content.fromScreen = 'Memo';
//     this.bsModalRef.content.Comments = this.memoModel.Comment;
//     this.bsModalRef.content.memoid = this.memoModel.MemoID;
//     this.bsModalRef.content.onClose.subscribe(result => {
//       this.btnLoad = result;
//     });
//   }

//   // messageGenarate(data){
//   //   var msg = '';
//   //   switch(data){
//   //     case 'Approve':
//   //       msg = 'تم اعتماد المذكرة بنجاح';
//   //       break;
//   //   }
//   //   return
//   // }

//   returnForInfo() {

//   }
//   clone() {
//     this.memoservice.cloneMemo('Memo/Clone', this.memoModel.MemoID, this.currentUser.id).subscribe(res => {
//       this.cloneBtnLoad = false;
//       this.btnLoad = false;
//       this.message = "مذكرة المستنسخة بنجاح";
//       this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
//       this.bsModalRef.content.language = 'ar';
//       this.bsModalRef.content.message = this.message;
//       this.bsModalRef.content.page_url = 'memo/memo-edit/' + res;
//       this.bsModalRef.content.pagename = 'Memo Clone';
//       //this.loadData(res, this.currentUser.id);
//     });
//   }
//   shareMemo() {

//     this.bsModalRef = this.modalService.show(ModalComponent, this.config);
//     this.bsModalRef.content.status = 'Share Memo';
//     this.bsModalRef.content.button = 'مشاركة المذكرة';
//     this.bsModalRef.content.memoid = this.memoModel.MemoID;
//     this.bsModalRef.content.onClose.subscribe(result => {
//       this.shareBtnLoad = result;
//       this.btnLoad = result;
//     });

//   }

//   print(template: TemplateRef<any>) {
//     this.memoservice.printPreview('Memo/preview', this.memoModel.MemoID, this.currentUser.id).subscribe(res => {
//       if (res) {
//         this.memoservice.pdfToJson(this.memoModel.ReferenceNumber).subscribe((data: any) => {
//           this.showPdf = true;
//           this.pdfSrc = data;
//           this.bsModalRef = this.modalService.show(template, { class: 'modal-xl' });
//         });
//       }
//     });
//   }


//   downloadPrint() {
//     this.common.previewPdf(this.memoModel.ReferenceNumber)
//       .subscribe((data: Blob) => {
//         this.downloadBtnLoad = false;
//         this.btnLoad = false;
//         this.printBtnLoad = false;
//         var url = window.URL.createObjectURL(data);
//         var a = document.createElement('a');
//         document.body.appendChild(a);
//         a.setAttribute('style', 'display: none');
//         a.href = url;
//         a.download = this.memoModel.ReferenceNumber + '.pdf';
//         a.click();
//         window.URL.revokeObjectURL(url);
//         a.remove();
//       });
//   }

//   printPdf(html: ElementRef<any>) {
//     this.btnLoad = false;
//     this.printBtnLoad = false;
//     this.common.printPdf(this.memoModel.ReferenceNumber);
//   }

//   delete(id) {
//     this.memoservice.deleteMemo('memo', this.memoModel.MemoID).subscribe(data => {
//       this.deleteBtnLoad = false;
//       this.btnLoad = false;
//       console.log('deleted' + data);
//       this.message = "تم حذف المذكرة بنجاح";
//       this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
//       this.bsModalRef.content.language = 'ar';
//       this.bsModalRef.content.message = this.message;
//       this.bsModalRef.content.pagename = 'memo';
//     });

//   }

//   closePrintPop() {
//     this.btnLoad = false;
//     this.printBtnLoad = false;
//     this.bsModalRef.hide();
//   }



//   formatPatch(val, path) {
//     var data = [{
//       "value": val,
//       "path": path,
//       "op": "replace"
//     }, {
//       "value": this.currentUser.id,
//       "path": "UpdatedBy",
//       "op": "replace"
//     }, {
//       "value": new Date(),
//       "path": "UpdatedDateTime",
//       "op": "replace"
//     }, {
//       "value": this.memoModel.Comment,
//       "path": "Comments",
//       "op": "replace"
//     }];
//     return data;
//   }

//   hisLog(status) {
//     if (status == 'Reject') {
//       return 'مرفوض';
//     } else if (status == 'Redirect') {
//       return 'إعادة توجيه';
//     } else if (status == 'Submit') {
//       return 'قدمت';
//     } else if (status == 'Resubmit') {
//       return 'أعاد';
//     } else if (status == 'ReturnForInfo') {
//       return 'عودة للحصول على معلومات';
//     } else if (status == 'submit') {
//       return 'قدمت';
//     } else if (status == 'approve') {
//       return 'وافق';
//     }
//   }

// }
