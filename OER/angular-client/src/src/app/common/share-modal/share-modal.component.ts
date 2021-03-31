import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GeneralService} from '../../services/general.service';
import {ProfileService} from '../../services/profile.service';

@Component({
  selector: 'app-share-modal',
  templateUrl: './share-modal.component.html'
})
export class ShareModalComponent implements OnInit {

  @Input() display: boolean;
  @Input() url: boolean;
  @Input() type: string;
  @Input() data: {
    id: number,
    title: string
    description: string
    image: string
    tags: string
  };
  @Output() displayChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private generalService: GeneralService, private profileService: ProfileService) {
  }

  ngOnInit() {
    this.display = false;
  }

  closeModel() {
    this.display = false;
    this.displayChange.emit(false);
  }

  logShareContent(service) {
    if (this.profileService.userId && this.profileService.userId > 0) {
      this.generalService.logSharedContent({
        'contentId': this.data.id,
        'contentTypeId': this.type === 'course' ? 1 : 2,
        'socialMediaName': service,
        'createdBy': this.profileService.userId
      }).subscribe(() => {
      });
    }
  }

}
