import {Component, Input, OnInit} from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-iframe-viewer',
  templateUrl: './iframe-viewer.component.html'
})
export class IFrameViewerComponent implements OnInit {

  @Input() url: string;
  showPopUp: boolean;
  tempurl: string;
  urlSafe: SafeResourceUrl;

  constructor(public sanitizer: DomSanitizer) {
  }

  ngOnInit() {
    debugger
    this.showPopUp = false;
    if (this.url.substring(0, 30).includes('youtube.com') && !this.url.includes('embed')) {
      this.tempurl = 'https://www.youtube.com/embed/' + this.url.substring(this.url.indexOf('youtube.com/watch?v=') + 20, this.url.length);
    }
    else {
      this.tempurl = this.url;
    }
    this.tempurl = 'https://www.youtube.com/embed/'+ this.getId(this.url);
    this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl(this.tempurl);
    // document.getElementById("youtubeIFrame").setAttribute('src',this.tempurl);
    
  }

   getId(url) {
    const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*/;
    const match = url.match(regExp);

    return (match && match[2].length === 11)
      ? match[2]
      : null;
}

  showPopUpFrame() {
    this.showPopUp = true;
  }
}
