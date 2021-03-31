import { id } from '@swimlane/ngx-datatable/release/utils';
import { ManagePhotoService } from './../../service/manage-photo.service';
import { Component, OnInit, TemplateRef, ViewChild, Renderer2, Inject, ElementRef, Input } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DOCUMENT } from '@angular/common';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { Router } from '@angular/router';
import { UploadService } from 'src/app/shared/service/upload.service';
import { HttpEventType } from '@angular/common/http';
import { Attachment } from 'src/app/shared/model/attachment/attachment.model';
import { environment } from 'src/environments/environment';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-manage-photo',
  templateUrl: './manage-photo.component.html',
  styleUrls: ['./manage-photo.component.scss']
})
export class ManagePhotoComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild('deletetemplate') deletetemplate: TemplateRef<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  rows: Array<any> = [];
  columns: Array<any> = [];
  config: any = {
    paging: true,
    page: 1,
    maxSize: 10,
    itemsPerPage: 10,
    totalItems:0
  };
  filterBy:any = {
    Type:null,
    Value:null
  };
  ManagePhotosModel: any = {
    Attachments: []
  };
  attachments: Array<Attachment> = [];
  newsColumns: Array<any> = [];
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('creationImageTemplate') creationImageTemplate: TemplateRef<any>;
  
  lang:string;
  isShowNewsAdd=true;
  bsModalRef:BsModalRef;
  message: string;
  inProgress: boolean = false;
  photoModel:any = {};
  photoList: any = [];
  imageURL = environment.AttachmentDownloadUrl;
  @Input() reqType: string = '';
  isBanner: boolean;
  ImageUrl: string;
  uploadProcess: boolean = false;
  uploadPercentage: number;
  PhotoID: any;
  
  constructor(
    private managePhotoService: ManagePhotoService,
    private common: CommonService,
    public router: Router,
    public arabicService: ArabicDataService,
    public modalService: BsModalService,
    public upload: UploadService,
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2,
    private endpoint: EndPointService) { }

  ngOnInit() {
    this.getPhotos();

    if(this.reqType === 'banner') {
      this.isBanner = true;
      this.getBanner();
    } else {
      this.isBanner = false;
      this.getPhotos();
    }
    this.lang = this.common.currentLang;
    if (this.lang == 'ar') {
      this.newsColumns = [
        { name: this.arabicfn('filename'), cellTemplate: this.creationImageTemplate },
        { name: this.arabicfn('action'), cellTemplate: this.actionTemplate },
      ];
    } else if (this.lang == 'en') {
      this.newsColumns = [
        { name: 'Image', cellTemplate: this.creationImageTemplate },
        { name: 'Action', cellTemplate: this.actionTemplate },
      ];
    }
  }

  removeWordSpaces(words:string){
    return  words.replace(/\s+/g, '');
  }

  handleFileUpload(event: any) {
    if (event.target.files.length > 0) {
      const files = event.target.files;
      this.uploadProcess = true;

      this.upload.uploadAttachment(files)
        .subscribe((response: any) => {
          if (event.type === HttpEventType.UploadProgress) {
            this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
          } else if (response.type === HttpEventType.Response) {
            this.uploadProcess = false;
            this.uploadPercentage = 0;
            const res = response.body;
            for (let i = 0; i < res.FileName.length; i++) {
              this.photoModel.AttachmentGuid =  res.Guid;
              this.photoModel.AttachmentName = res.FileName ? res.FileName[0] : res.Guid;
            }
            this.ImageUrl =  this.endpoint.fileDownloadUrl+"?filename="+this.photoModel.AttachmentName+"&guid="+this.photoModel.AttachmentGuid;

            // this.attachments.push(attachment);
          }
        }
      );
    }
  }
  
  saveBanner() {
    this.inProgress = true;
    this.photoModel.CreatedBy = this.currentUser.id;
    this.photoModel.CreatedDateTime = new Date();
    this.managePhotoService.createBanner(this.photoModel, 'Banner').subscribe(
      (response: any) => {
        // this.closemodal();
        if (response) {
          if (this.lang === 'en') {
            this.message = 'Banner Added Successfully';
          } else {
            this.message = this.common.arabic.words['banneraddedsuccessfully'];
          }
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.closeDialog();
          });
        }
        this.photoModel = {};
        this.ImageUrl = '';
        this.fileInput.nativeElement.value = "";
      }
    );
    this.inProgress = false;
  }

  savePhoto() {
    this.inProgress = true;
    // this.ManagePhotosModel.Attachments =  this.attachments;
    this.photoModel.ExpiryDate = new Date();
    this.photoModel.CreatedBy = this.currentUser.id;
    this.photoModel.CreatedDateTime = new Date();
    this.managePhotoService.create(this.photoModel).subscribe(
      (response: any) => {
        // this.closemodal();
        if (response.PhotoID) {
          if (this.lang === 'en') {
            this.message = 'Photo Added Successfully';
          } else {
            this.message = this.common.arabic.words['photoaddedsuccessfully'];
          }
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.closeDialog();
            this.photoModel = {};
            this.ImageUrl = '';
            this.fileInput.nativeElement.value = "";
          });
        }
        this.getPhotos();
      }
    );
    this.inProgress = false;
  }

  getPhotos() {
    this.rows = [];
    this.managePhotoService.getPhotos(this.config.page, this.config.maxSize, this.currentUser.id).subscribe(data => {
      this.photoList = data;
      this.rows = this.photoList.Collection;
      this.config.totalItems = this.photoList.Count;
    })
  }
  
  getBanner() {
    this.rows = []
    this.managePhotoService.getBanner("Banner").subscribe((data:any) => {
          this.rows.push(
            {
              AttachmentGuid: data.AttachmentGuid,
              AttachmentName: data.AttachmentName,
              BannerID: data.BannerID,
              CreatedBy: data.CreatedBy,
              CreatedDateTime: data.CreatedDateTime
            }
          );
      // this.rows = this.photoList.Collection;
      // console.log("this.bannerData", this.bgImage)
    })
  }

  openDeleteDialog(value) {
    this.PhotoID = value.PhotoID;
    this.bsModalRef = this.modalService.show(this.deletetemplate);
  }

  delateNews(value) {
    this.managePhotoService.deletePhotoById(this.PhotoID).subscribe(data => {
      if(data == "True") {
        this.getPhotos();
        this.closeDialog();
      }
    })
  }

  closeDialog() {
    this.bsModalRef.hide();
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }

  showNewsAdd() {
    this.isShowNewsAdd = true;
  }

  showFilter() {
    this.isShowNewsAdd = false;
  }

  closemodal(){
    this.photoModel = {};
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
  }
  public onChangePage(config: any, page: any = { page: this.config.page, itemsPerPage: this.config.itemsPerPage }): any {
    this.config.page = page;
    this.getPhotos();
  }
}
