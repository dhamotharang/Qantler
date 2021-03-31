import { Component, OnInit, Input, Inject, Renderer2 } from '@angular/core';
import { MediaPressReleaseComponent } from '../media-press-release/media-press-release.component';

@Component({
  selector: 'app-media-press-release-rtl',
  templateUrl: './media-press-release.component-rtl.html',
  styleUrls: ['./media-press-release.component-rtl.scss']
})
export class MediaPressReleaseComponentRTL extends MediaPressReleaseComponent {
  typeList = [
    {"value": '1', "label": "اجتماعي"},
    {"value": '2', "label": "رسمي"}
  ];
}