import { ManagePhotoService } from './../../manage-photo/service/manage-photo.service';
import { CommonService } from './../../common.service';
import { Component, OnInit } from '@angular/core';
import { Lightbox } from 'ngx-lightbox';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photogallery',
  templateUrl: './photogallery.component.html',
  styleUrls: ['./photogallery.component.scss']
})
export class PhotogalleryComponent implements OnInit {
  lang: any;
  public page: number = 1;
  public maxSize: number = 12;
  public itemsPerPage: number = 12;
  public totalItems: number;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'modal-lg',
    paging: true,
    // sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  albums: Array<any> = [];
  currentUser: any;
  photoList: Array<any> = [];
  imageURL = environment.AttachmentDownloadUrl;

  constructor(
    public common: CommonService,
    public managePhotoService: ManagePhotoService,
    public lightBox: Lightbox
  ) { }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    if (this.lang == 'en') {
      this.common.breadscrumChange('Photo Gallery', '', '');
    } else {
      this.common.breadscrumChange(this.arabic('photogallery'), '', '');
    }
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.loadPhotoList();
  }

  loadPhotoList() {
    this.managePhotoService.loadAllPhotos(this.page, this.maxSize).subscribe(
      (resList: any) => {
        this.photoList = resList.Collection;
        // this.totalItems = resList.Count;
        this.photoList.forEach((obj: any) => {
          if (obj.AttachmentGuid != null && obj.AttachmentName != null) {
            this.albums.push(
              {
                src: this.imageURL + '?filename=' + obj.AttachmentName + '&guid=' + obj.AttachmentGuid,
                caption: obj.AttachmentName,
                thumb: this.imageURL + '?filename=' + obj.AttachmentName + '&guid=' + obj.AttachmentGuid
              }
            );
          }
        })
        this.totalItems = resList.Count;
      }
    );
  }

  public onChangePage(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
    this.page = page;
    this.albums = [];
    this.loadPhotoList();
  }

  open(index: number): void {
    this.lightBox.open(this.albums, index);
  }

  close(): void {
    this.lightBox.close();
  }
  
  arabic(word) {
    return this.common.arabic.words[word];
  }
}
