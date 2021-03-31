import { Component, OnInit, ViewChild, TemplateRef, ElementRef, Renderer2, Inject } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/common.service';
import { UtilsService } from '../../service/utils.service';
import { DOCUMENT } from '@angular/platform-browser';
import { HttpEventType } from '@angular/common/http';
import { UploadService } from '../../service/upload.service';

@Component({
  selector: 'app-media-documents-modal',
  templateUrl: './media-documents-modal.component.html',
  styleUrls: ['./media-documents-modal.component.scss']
})
export class MediaDocumentsModalComponent implements OnInit {
  user = {
    id: 0
  };
  @ViewChild('template') template : TemplateRef<any>;
  @ViewChild('modalFileInput') fileInput :ElementRef;
  requestTypeList:Array<any> = [];
  isApiLoading:boolean = false;
  uploadPercentage:number = 0;
  img_file:any[] = [];
  uploadProcess:boolean = false;
  attachments:any[] = [];
  requestType:any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  message = '';
  constructor(public router : Router, 
    public modalService : BsModalService, 
    public bsModalRef: BsModalRef,     
    public commonService : CommonService,
    private utilsService:UtilsService, 
    private uploadService:UploadService, 
    private renderer: Renderer2, 
    @Inject(DOCUMENT) private document: Document,) {
      this.user = JSON.parse(localStorage.getItem("User"));
     }

  ngOnInit() {}

  HrAttachments(event) {
    this.img_file = event.target.files;
    if(this.img_file.length > 0){
      this.isApiLoading = true;
      this.uploadProcess = true;
      let toSendFileData = {
        data:this.img_file[0],
        // RequestType: this.requestType
      };
      this.uploadService.uploadModuleAttachment(toSendFileData).subscribe((attachementRes:any) => {  
        this.isApiLoading = false;        
        if (attachementRes.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(attachementRes.loaded / attachementRes.total) * 100;
        } else if (attachementRes.type === HttpEventType.Response) {
          this.isApiLoading = false;
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          // for (var i = 0; i < attachementRes.body.FileName.length; i++) {            
            this.attachments[0] = {'AttachmentGuid':attachementRes.body.Guid,'AttachmentsName':attachementRes.body.FileName[0]};
          // }         
          this.fileInput.nativeElement.value = "";
        }
      });
    }
  }


  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.fileInput.nativeElement.value = "";
  }

  close(){
    this.bsModalRef.hide();
    this.fileInput.nativeElement.value = "";
  }

  closemodal(){
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    this.fileInput.nativeElement.value = "";
  }

  saveHrAttachments(){
    let toSendFileData ={
      AttachmentGuid:this.attachments[0].AttachmentGuid,
      AttachmentsName:this.attachments[0].AttachmentsName,
      Type:'HR',
      CreatedBy:this.currentUser.id,
      CreatedDateTime:new Date()
    };
    this.uploadService.uploadModuleAttachment(toSendFileData).subscribe((uploadRes)=>{
      if(uploadRes){
        this.bsModalRef.hide();
        this.attachments = [];
        this.message = 'File uploaded successfully';
        this.modalService.show(this.template);
        this.fileInput.nativeElement.value = "";
        this.uploadService.hrDocumentUploadDetector.next(true);
      }
    });
  }

}
