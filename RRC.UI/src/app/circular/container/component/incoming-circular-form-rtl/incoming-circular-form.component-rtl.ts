import { Component, OnInit, ViewChild, TemplateRef, ElementRef, ChangeDetectorRef } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { FormGroup, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CreateCircularModal } from './create-circular.modal';
//import { SampleData } from './sampleDB';
import 'tinymce';
import { async } from 'q';
import { DatePipe } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../../../../modal/modalcomponent/modal.component';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { CommonService } from '../../../../common.service';
import { CircularService } from '../../../service/circular.service';
import { HttpEventType } from '@angular/common/http';
import { IncomingCircularFormComponent } from '../incoming-circular-form/incoming-circular-form.component';

declare var tinymce: any;

@Component({
  selector: 'app-circular-form-rtl',
  templateUrl: './incoming-circular-form.component-rtl.html',
  styleUrls: ['./incoming-circular-form.component-rtl.scss']
})
export class IncomingCircularFormComponentRTL extends IncomingCircularFormComponent {
 
  // bsConfig: Partial<BsDatepickerConfig>;
  // @ViewChild('template') template: TemplateRef<any>;
  // @ViewChild('variable') myInputVariable: ElementRef;
  // @ViewChild('printContent') printContent: ElementRef<any>;
  // bsModalRef: BsModalRef;
  // createCircular: CreateCircularModal = new CreateCircularModal();
  // screenStatus = 'Create';
  // displayStatus: any = 'CREATION';
  // masterData: any;
  // circularData: any = {
  //   HistoryLog: []
  // };
  // config = {
  //   backdrop: true,
  //   ignoreBackdropClick: true
  // };
  // //circularData: any;
  // status = '';
  // user = [];//this.masterData.data.user;
  // department = [];//this.masterData.data.department;
  // priorityList = ['High', 'Medium', 'Low', 'Very low'];
  // attachmentFiles = [];
  // createdTime: string;
  // createdDate: string;
  // submitted = false;

  // dropdownSettings: any;

  // colorTheme = 'theme-green';
  // incomingcircular = {
  //   CircularID: 0,
  //   ReferenceNumber: '',
  //   Title: '',
  //   SourceOU: '',
  //   SourceName: '',
  //   DestinationOU: [],
  //   ApproverName: 0,
  //   ApproverDepartment: 0,
  //   Details: '',
  //   Priority: '',
  //   Comments: '',
  //   Attachments: [],
  //   AttachmentName: '',
  //   DeleteFlag: '',
  //   CreatedBy: '',
  //   UpdatedBy: '',
  //   HistoryLog: [],
  //   CreatedDateTime: new Date(),
  //   UpdatedDateTime: '',
  //   Status: 0,
  // }
  // img_file: any;
  // message: any;
  // currentUser: any = JSON.parse(localStorage.getItem('User'));
  // attachments: any = [];
  // userDestination: any;
  // userReceiver: any;
  // commonMes: any;
  // DestinationDepartmentId: any = [];
  // uploadProcess: boolean = false;
  // uploadPercentage: number;
  // pdfSrc: string;
  // showPdf: boolean = false;
  // sendDraftBtnLoad = false;
  // sendBtnLoad = false;
  // printBtnLoad = false;
  // cloneBtnLoad = false;
  // downloadBtnLoad = false;
  // approveBtnLoad = false;
  // deleteBtnLoad = false;
  // returnInfoBtnLoad = false;
  // btnLoad = false;
  // currentId: any;
  // constructor(private changeDetector: ChangeDetectorRef, public common: CommonService, public router: Router, public route: ActivatedRoute, public datepipe: DatePipe,
  //   private modalService: BsModalService, private circularService: CircularService) {
  //   // this.bsConfig = {
  //   //   dateInputFormat: 'DD/MM/YYYY'
  //   // }
  //   route.url.subscribe(() => {
  //     console.log(route.snapshot.data);
  //     this.screenStatus = route.snapshot.data.title;
  //   });
  //   route.params.subscribe(param => {
  //     var id = +param.id;
  //     if (id > 0)
  //       this.loadData(id, this.currentUser.id);
  //     this.circularService.breadscrumChange(1, this.screenStatus, id, 'ar');

  //   });
  //   this.circularService.getCircular('Circular', 0, 0).subscribe((data: any) => {
  //     this.department = data.OrganizationList;
  //   });
  //   // if (this.screenStatus == 'Create') {
  //   //   this.displayStatus = 'CREATION';
  //   // }
  //   // if (this.screenStatus == 'View') {
  //   //   this.displayStatus = 'VIEW';
  //   // }
  //   // if (this.screenStatus == 'Edit') {
  //   //   this.displayStatus = 'EDIT';
  //   // }
  //   switch (this.screenStatus) {
  //     case 'Create':
  //       this.displayStatus = 'خلق';
  //       break;
  //     case 'View':
  //       this.displayStatus = 'رأي';
  //       break;
  //     case 'Edit':
  //       this.displayStatus = 'تصحيح';
  //       break;
  //   }
  // }

  // async loadData(id, userid) {
  //   this.currentId = id;
  //   await this.circularService.getCircular('Circular', id, userid).subscribe((data: any) => {
  //     this.circularData = data;
  //     if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
  //       let that = this;
  //       this.status = this.circularData.M_LookupsList;
  //       var date = this.incomingcircular.CreatedDateTime;
  //       this.incomingcircular.CreatedDateTime = new Date(date);
  //       this.incomingcircular.CreatedBy = this.circularData.CreatedBy;
  //       this.setData(this.circularData);
  //       this.bottonControll();
  //     } else {
  //       this.initPage();
  //       this.bottonControll();
  //     }
  //   });
  // }

  // async  setData(data) {
  //   this.getDestUserList(+data.ApproverDepartmentId);
  //   //this.getRecvPrepareUserList(data.DestinationDepartmentID);
  //   this.incomingcircular.CircularID = data.CircularID;
  //   this.incomingcircular.ReferenceNumber = data.ReferenceNumber
  //   this.incomingcircular.Title = data.Title;
  //   this.incomingcircular.SourceOU = data.SourceOU;
  //   this.incomingcircular.SourceName = data.SourceName;

  //   const DestinationOU = [];
  //   const DestinationUsername = [];
  //   data.DestinationDepartmentID.forEach((department, index) => {
  //     DestinationOU.push(department.CircularDestinationDepartmentID);
  //   });
  //   this.incomingcircular.DestinationOU = DestinationOU;
  //   this.DestinationDepartmentId = this.incomingcircular.DestinationOU;
  //   // data.DestinationUsernameID.forEach((user,index)=>{
  //   //   DestinationUsername.push(user.MemoDestinationUsersID);
  //   // });
  //   //this.incomingcircular.DestinationUsername = DestinationUsername;
  //   if (data.CurrentApprover.length > 0) {
  //     this.incomingcircular.ApproverName = data.CurrentApprover[0].ApproverId;
  //   }
  //   //this.incomingcircular.ApproverName = data.CurrentApprover[0].ApproverId; //Check this set
  //   this.incomingcircular.ApproverDepartment = +data.ApproverDepartmentId; //check this set
  //   this.incomingcircular.Details = data.Details;
  //   this.incomingcircular.Priority = data.Priority;
  //   this.incomingcircular.CreatedBy = data.CreatedBy;
  //   this.attachments = data.Attachments;
  //   this.incomingcircular.HistoryLog = data.HistoryLog;

  //   this.incomingcircular.Attachments = data.Attachments;
  //   this.incomingcircular.Status = data.Status;
  //   this.circularService.breadscrumChange(this.incomingcircular.Status, this.screenStatus, this.currentId, 'ar');

  // }

  // async ngOnInit() {
  //   this.bottonControll();

  //   this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
  //   // tinymce.init({
  //   //   //skin_url: '/skins' // Or loaded from your environments config
  //   //   selector: 'textarea',
  //   //   directionality: 'rtl',
  //   //   language: 'ar'
  //   // });

  // }
  // public tinyMceSettings = {
  //   skin_url: 'assets/tinymce/skins/lightgray',
  //   inline: false,
  //   statusbar: false,
  //   browser_spellcheck: true,
  //   height: 320,
  //   plugins: 'fullscreen',
  // };


  // ngAfterViewInit() {

  // }

  // closemodal() {
  //   this.modalService.hide(1);
  //   //this.router.navigate(['app/circular/circular-list']);
  //   this.router.navigate(['app/circular/circular-list'], { relativeTo: this.route });
  // }


  // initPage() {
  //   this.incomingcircular.CircularID = 0
  //   this.incomingcircular.ReferenceNumber = '';
  //   this.incomingcircular.Title = '';
  //   this.incomingcircular.SourceOU = this.currentUser.department;
  //   this.incomingcircular.SourceName = this.currentUser.username;
  //   this.incomingcircular.DestinationOU = [];
  //   // this.incomingcircular.DestinationUsername = [];
  //   this.incomingcircular.ApproverName = 0;
  //   this.incomingcircular.ApproverDepartment = 0;
  //   this.incomingcircular.Details = '';
  //   this.incomingcircular.Priority = '';
  //   this.incomingcircular.Comments = '';
  //   this.incomingcircular.Attachments = [];
  //   this.incomingcircular.AttachmentName = '';
  //   this.incomingcircular.DeleteFlag = '';
  //   this.incomingcircular.CreatedBy = this.currentUser.id;
  //   this.incomingcircular.UpdatedBy = '';
  //   this.incomingcircular.CreatedDateTime = new Date();
  //   this.incomingcircular.UpdatedDateTime = '';
  //   this.incomingcircular.Status = 0;
  // }

  // Attachments(event) {
  //   this.img_file = event.target.files;
  //   for (var i = 0; i < this.img_file.length; i++) {
  //     this.attachmentFiles.push(this.img_file[i]);
  //     this.attachments.push({ 'AttachmentGuid': 0, 'AttachmentsName': this.img_file[i].name, 'CircularID': '' });
  //   }
  //   this.incomingcircular.Attachments = this.attachments;
  // }
  // selectChange(data) {
  //   this.incomingcircular.DestinationOU;
  // }

  // deleteAttachment(index) {
  //   this.attachments.splice(index, 1);
  //   this.myInputVariable.nativeElement.value = "";
  // }


  // prepareData() {
  //   this.createCircular.CreatedDateTime = this.incomingcircular.CreatedDateTime;
  //   if (this.DestinationDepartmentId.length) {
  //     this.DestinationDepartmentId.forEach(data => {
  //       this.createCircular.DestinationDepartmentID.push({
  //         "CircularDestinationDepartmentID": data,
  //         'CircularDestinationDepartmentName': ''
  //       });
  //     });
  //   } else {
  //     this.DestinationDepartmentId = [];
  //   }
  //   this.createCircular.Title = this.incomingcircular.Title;
  //   this.createCircular.SourceOU = this.currentUser.department;
  //   this.createCircular.SourceName = this.currentUser.username;
  //   this.createCircular.ApproverId = this.incomingcircular.ApproverName;
  //   this.createCircular.ApproverDepartmentId = this.incomingcircular.ApproverDepartment;
  //   this.createCircular.Details = this.incomingcircular.Details;
  //   this.createCircular.Priority = this.incomingcircular.Priority;
  //   this.createCircular.Attachments = this.incomingcircular.Attachments;
  //   this.createCircular.CreatedBy = this.currentUser.id;
  //   //this.createMemo.Action = this.memoModel.Status+'';
  //   this.createCircular.Comments = this.incomingcircular.Comments;



  //   return this.createCircular;
  // }

  // validateForm() {
  //   var flag = true;
  //   var destination = (this.incomingcircular.DestinationOU) ? (this.incomingcircular.DestinationOU.length > 0) : false;
  //   //var Keywords = (this.incomingcircular.Keywords) ? (this.incomingcircular.Keywords.length > 0) : false;
  //   //var username = (this.incomingcircular.DestinationUsername) ? (this.incomingcircular.DestinationUsername.length > 0) : false;

  //   if (destination && this.incomingcircular.Title && this.incomingcircular.ApproverName
  //     && this.incomingcircular.ApproverDepartment && this.incomingcircular.Details
  //     && this.incomingcircular.Priority && this.incomingcircular.Attachments.length > 0) {
  //     flag = false;
  //   }
  //   return flag;
  // }


  // createBtnShow = false;
  // editBtnShow = false;
  // viewBtnShow = false;
  // approverBtn = false;
  // receiverBtn = false;
  // deleteBtn = false;
  // creatorBtn = false;
  // draftBtn = false;
  // cloneBtn = false;
  // savedraftBtn = false;
  // id = '';
  // printbtn = false;



  // bottonControll() {
  //   if (this.screenStatus == 'Create') {
  //     this.createBtnShow = true;
  //     this.savedraftBtn = true;
  //     this.printbtn = true;
  //   } else if (this.screenStatus == 'Edit') {
  //     this.editBtnShow = true;
  //   } else if (this.screenStatus == 'View' && this.incomingcircular.CreatedBy == this.currentUser.id) {
  //     this.viewBtnShow = true;
  //   }
  //   if (this.incomingcircular.CreatedBy == this.currentUser.id) {
  //     this.creatorBtn = true;
  //   }
  //   if (this.incomingcircular.CreatedBy == this.currentUser.id && (this.incomingcircular.Status == 12 || this.incomingcircular.Status == 16) && this.screenStatus == 'Edit') {
  //     this.draftBtn = true;
  //   }
  //   if (this.screenStatus == 'View' && this.incomingcircular.ApproverName == this.currentUser.id && this.incomingcircular.Status == 13) {
  //     this.approverBtn = true;
  //   }
  //   // this.incomingcircular.DestinationUsername.forEach(element => {
  //   //   if (element == this.currentUser.id && this.incomingcircular.Status == 3) {
  //   //     this.receiverBtn = true;
  //   //     this.editBtnShow = false;
  //   //   }
  //   // });
  //   if (this.incomingcircular.CreatedBy == this.currentUser.id && this.incomingcircular.Status == 12 && this.screenStatus == 'Edit') {
  //     this.deleteBtn = true;
  //     this.savedraftBtn = true;
  //   }
  // }

  // async Destination(event) {
  //   this.DestinationDepartmentId = this.incomingcircular.DestinationOU;
  //   //await this.getRecvUserList(this.DestinationDepartmentId);
  // }


  // onChangeDepartment() {
  //   this.getDestUserList(+this.incomingcircular.ApproverDepartment);
  //   this.incomingcircular.ApproverName = 0;
  // }


  // async getDestUserList(id) {
  //   if (id) {
  //     let params = [{
  //       "OrganizationID": id,
  //       "OrganizationUnits": "string"
  //     }];
  //     let user_dept_id = this.currentUser.id;
  //     if (this.screenStatus == 'View') {
  //       user_dept_id = 0;
  //     }
  //     this.common.getUserList(params, user_dept_id).subscribe((data: any) => {
  //       this.userDestination = data;
  //     });
  //   } else {
  //     this.userDestination = [];
  //   }
  // }

  // saveCircular(data = '') {

  //   //this.submitted = true;
  //   if (data == 'draft' && this.incomingcircular.CircularID == 0) {
  //     var requestData = this.prepareData();
  //     requestData.Action = 'Save';
  //     //requestData['CircularID'] = this.incomingcircular.CircularID;
  //     this.circularService.saveCircular('Circular', requestData).subscribe(data => {

  //       this.sendDraftBtnLoad = false;
  //       this.sendBtnLoad = false;
  //       console.log(data);
  //       this.message = this.arabic('circulardraftsuccess');
  //       this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //       this.bsModalRef.content.message = this.message;
  //       this.bsModalRef.content.pagename = 'Circular';
  //       //location.reload();
  //     });
  //   } else {
  //     if (this.incomingcircular.Status == 12 || this.incomingcircular.Status == 16) {
  //       requestData = this.prepareData();
  //       requestData.Action = 'Submit';
  //       if (data == 'draft') {
  //         requestData.Action = 'Save';
  //       }
  //       if (this.incomingcircular.Status == 16) {
  //         requestData.Action = 'Resubmit';
  //       }
  //       requestData['DeleteFlag'] = false;
  //       requestData['UpdatedBy'] = this.incomingcircular.CreatedBy;
  //       requestData['UpdatedDateTime'] = new Date();
  //       requestData['CircularID'] = this.incomingcircular.CircularID;

  //       this.circularService.updateCircular('Circular', this.incomingcircular.CircularID, requestData).subscribe(data => {
  //         this.sendDraftBtnLoad = false;
  //         this.sendBtnLoad = false;
  //         if (this.screenStatus == 'Create') {

  //           this.message = this.arabic('circularsubmitsuccess');
  //           this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //           this.bsModalRef.content.language = 'ar';
  //           this.bsModalRef.content.message = this.message;
  //           this.bsModalRef.content.screenStatus = this.screenStatus;
  //           this.bsModalRef.content.pagename = 'Circular';
  //         } else {
  //           this.message = this.arabic('circularupdatesuccess');
  //           this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //           this.bsModalRef.content.language = 'ar';
  //           this.bsModalRef.content.message = this.message;
  //           this.bsModalRef.content.pagename = 'Circular';
  //           //location.reload();
  //         }
  //       });
  //     } else {
  //       requestData = this.prepareData();
  //       requestData.Action = 'Submit';
  //       console.log(requestData);
  //       this.circularService.saveCircular('Circular', requestData).subscribe(data => {
  //         console.log(data);
  //         this.sendDraftBtnLoad = false;
  //         this.sendBtnLoad = false;
  //         if (this.screenStatus == 'Create') {
  //           this.message = this.arabic('circularsubmitsuccess');
  //           this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //           this.bsModalRef.content.language = 'ar';
  //           this.bsModalRef.content.message = this.message;
  //           this.bsModalRef.content.screenStatus = this.screenStatus;
  //           this.bsModalRef.content.pagename = 'Circular';
  //         } else {
  //           this.message = this.arabic('circularupdatesuccess');
  //           this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //           this.bsModalRef.content.language = 'ar';
  //           this.bsModalRef.content.message = this.message;
  //           this.bsModalRef.content.pagename = 'Circular';
  //           //location.reload();
  //         }
  //       });
  //     }
  //   }
  //   // this.router.navigate(['app/circular/circular-list']);
  // }



  // statusChange(status: any, dialog) {
  //   this.submitted = true;
  //   var data = this.formatPatch(status, 'Action')
  //   this.commonMes = status;
  //   this.circularService.statusChange('Circular', this.incomingcircular.CircularID, data).subscribe(data => {
  //     //this.message = 'Memo '+status+'d';
  //     this.approveBtnLoad = false;
  //     this.returnInfoBtnLoad = false;
  //     this.btnLoad = false;
  //     if (status == 'ReturnForInfo') {
  //       this.message = this.arabic('circularresubmitsuccess');
  //     } else if (status == 'Approve') {
  //       this.message = this.arabic('circularapprovesuccess');
  //     } else if (status == 'Reject') {
  //       this.message = this.arabic('circularrejectsuccess');
  //     } else if (status == 'Close') {
  //       this.message = this.arabic('circularclosedsuccess');
  //     } else {
  //       this.message = this.arabic('circularapprovesuccess');
  //     }
  //     //that.modalService.show(that.template);
  //     //this.modalService.show(this.template);
  //     this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //     this.bsModalRef.content.language = 'ar';
  //     this.bsModalRef.content.message = this.message;
  //     this.bsModalRef.content.pagename = 'Circular';
  //     this.loadData(data['CircularId'], this.currentUser.id);
  //     //location.reload();

  //   });

  // }

  // popup(status: any) {
  //   this.bsModalRef = this.modalService.show(ModalComponent);
  //   this.bsModalRef.content.language = 'ar';
  //   this.bsModalRef.content.status = status;
  //   this.bsModalRef.content.button = 'رفع للاعتماد';
  //   this.bsModalRef.content.modalTitle = 'رفع للاعتماد';
  //   this.bsModalRef.content.fromScreen = 'Circular';
  //   this.bsModalRef.content.Comments = this.incomingcircular.Comments;
  //   this.bsModalRef.content.memoid = this.incomingcircular.CircularID;
  //   this.bsModalRef.content.onClose.subscribe(result => {
  //     this.btnLoad = result;
  //   });
  // }

  // // approve() {
  // //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  // //     console.log('approved' + data);
  // //   });
  // // }

  // // escalate() {
  // //   var data = this.formatPatch('escalate', 'Status')
  // //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  // //     console.log('escalated' + data);
  // //   });
  // // }

  // // reject() {
  // //   var data = this.formatPatch('reject', 'Status')
  // //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  // //     console.log('rejected' + data);
  // //   });
  // // }
  // returnForInfo() {

  // }
  // clone(data) {
  //   this.submitted = true;
  //   var requestData = this.prepareData();
  //   requestData.Action = data;
  //   this.circularService.saveClone('Circular/Clone', this.incomingcircular.CircularID, this.currentUser.id).subscribe(data => {
  //     console.log(data);

  //     this.cloneBtnLoad = false;
  //     this.btnLoad = false;
  //     this.message = this.arabic('circularclonesuccess');
  //     this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //     this.bsModalRef.content.language = 'ar';
  //     this.bsModalRef.content.message = this.message;
  //     this.bsModalRef.content.page_url = 'ar/app/circular/Circular-edit/' + data;
  //     this.bsModalRef.content.pagename = 'Circular Clone';
  //     //this.bsModalRef.content.pagename = 'Letter';
  //     //location.reload();
  //   });
  // }
  // print(template: TemplateRef<any>) {
  //   this.circularService.printPreview('Circular/Download', this.incomingcircular.CircularID, this.currentUser.id).subscribe(res => {
  //     if (res) {
  //       this.circularService.pdfToJson(this.incomingcircular.ReferenceNumber).subscribe((data: any) => {
  //         this.showPdf = true;
  //         this.pdfSrc = data;
  //         this.bsModalRef = this.modalService.show(template, { class: 'modal-xl' });
  //       });
  //     }
  //   });
  // }


  // downloadPrint() {
  //   this.common.previewPdf(this.incomingcircular.ReferenceNumber)
  //     .subscribe((data: Blob) => {
  //       this.downloadBtnLoad = false;
  //       this.btnLoad = false;
  //       this.printBtnLoad = false;
  //       var url = window.URL.createObjectURL(data);
  //       var a = document.createElement('a');
  //       document.body.appendChild(a);
  //       a.setAttribute('style', 'display: none');
  //       a.href = url;
  //       a.download = this.incomingcircular.ReferenceNumber + '.pdf';
  //       a.click();
  //       window.URL.revokeObjectURL(url);
  //       a.remove();
  //     });
  // }



  // printPdf(html: ElementRef<any>) {
  //   // var myWindow = window.open('', '');
  //   // myWindow.document.write(html.innerHTML);
  //   // myWindow.document.close();
  //   // myWindow.focus();
  //   // myWindow.print();
  //   // myWindow.close();
  //   this.btnLoad = false;
  //   this.printBtnLoad = false;
  //   this.common.printPdf(this.incomingcircular.ReferenceNumber);

  // }
  // delete(id) {
  //   this.submitted = true;
  //   this.circularService.deleteCircular('Circular', this.incomingcircular.CircularID).subscribe(data => {
  //     this.deleteBtnLoad = false;
  //     this.btnLoad = false;
  //     this.message = this.arabic('circulardeletesuccess');
  //     this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
  //     this.bsModalRef.content.language = 'ar';
  //     this.bsModalRef.content.message = this.message;
  //     this.bsModalRef.content.pagename = 'Circular';
  //   });

  // }

  // closePrintPop() {
  //   this.btnLoad = false;
  //   this.printBtnLoad = false;
  //   this.bsModalRef.hide();
  // }

  // uploadFiles(event) {
  //   var files = event.target.files;
  //   let that = this;
  //   this.uploadProcess = true;
  //   this.common.postAttachment(files).subscribe((event: any) => {
  //     if (event.type === HttpEventType.UploadProgress) {
  //       this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
  //     } else if (event.type === HttpEventType.Response) {
  //       this.uploadProcess = false;
  //       this.uploadPercentage = 0;
  //       for (var i = 0; i < event.body.FileName.length; i++) {
  //         this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'CircularID': '' });
  //       }
  //       this.incomingcircular.Attachments = this.attachments;
  //     }
  //   });
  //   this.myInputVariable.nativeElement.value = "";
  // }

  // // close() {
  // //   var data = this.formatPatch('close', 'Status')
  // //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  // //     console.log('closed' + data);
  // //   });
  // // }

  // // redirect() {
  // //   var data = this.formatPatch('redirect', 'Status')
  // //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  // //     console.log('redirected' + data);
  // //   });
  // // }

  // formatPatch(val, path) {
  //   var data = [{
  //     "value": val,
  //     "path": path,
  //     "op": "replace"
  //   }, {
  //     "value": this.currentUser.id,
  //     "path": "UpdatedBy",
  //     "op": "replace"
  //   }, {
  //     "value": new Date(),
  //     "path": "UpdatedDateTime",
  //     "op": "replace"
  //   }, {
  //     "value": this.incomingcircular.Comments,
  //     "path": "Comments",
  //     "op": "replace"
  //   }];
  //   return data;
  // }

  // hisLog(status) {
  //   if (status == 'Reject' || status == 'Redirect') {
  //     return this.arabic(status + 'ed');
  //   } else if (status == 'ReturnForInfo') {
  //     return this.arabic(status);
  //   } else if (status == 'Submit') {
  //     return this.arabic('Submitted');;
  //   } else if (status == 'Resubmit') {
  //     return this.arabic('Resubmitted');
  //   } else {
  //     return this.arabic(status + 'd');
  //   }
  // }
}


