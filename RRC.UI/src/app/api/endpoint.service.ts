import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class EndPointService {

  apiHostingURL: string = null;
  imageHostingURL: string;
  pdfDownloads: string;
  fileDownloadUrl:string;

  constructor() {
    this.apiHostingURL = environment.apiHostingURL;
    this.imageHostingURL = environment.imageUrl;
    this.pdfDownloads = environment.DownloadUrl;
    this.fileDownloadUrl = environment.AttachmentDownloadUrl;
  }
}