import {Component, Input, OnInit} from '@angular/core';
import {EncService} from '../../services/enc.service';
import { FileMimeType } from '@taldor-ltd/angular-file-viewer';

@Component({
  selector: 'app-file-viewer',
  templateUrl: './file-viewer.component.html'
})
export class FileViewerComponent implements OnInit {

  @Input() url: string;
  @Input() fileName: string;
  @Input() title: string;
  type: string;
  showModal: boolean;
  filetype = FileMimeType.PDF;
  constructor(private encService: EncService) {
  }

  ngOnInit() {
    this.showModal = false;
    this.type = this.url.split('.')[this.url.split('.').length - 1];
  }

  showViewer() {
    this.showModal = true;
  }

  checkFileType() {
    if (this.encService.acceptedInvalidExtensions.filter(x => x === this.type.toLowerCase()).length > 0) {
      return 'accepted';
    } else if (this.encService.acceptedVideoExtensions.filter(x => x === this.type.toLowerCase()).length > 0) {
      return 'video';
    } else if (this.encService.acceptedDocumentExtensions.filter(x => x === this.type.toLowerCase()).length > 0) {
      if (this.type.toLowerCase() === 'pdf') {
        return 'pdf';
      }
      return 'document';
    } else if (this.encService.acceptedImageExtensions.filter(x => x === this.type.toLowerCase()).length > 0 && this.type.toLowerCase() !== 'tiff') {
      return 'image';
    } else if (this.encService.acceptedAudioExtensions.filter(x => x === this.type.toLowerCase()).length > 0) {
      return 'audio';
    } else {
      return 'invalid';
    }
  }
}
