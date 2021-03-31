import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {ResourceService} from '../../services/resource.service';
import {CourseService} from '../../services/course.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html'
})
export class RatingComponent implements OnInit, OnChanges {

  @Input() id: number;
  @Input() allRating: any;
  @Input() rating: number;
  @Input() type: string;
  @Output() updateData: EventEmitter<boolean> = new EventEmitter<boolean>();
  userId: number;
  allUsers: number;
  ratingCount: any;
  ratingValue: any;

  constructor(private resourceService: ResourceService, private courseService: CourseService, private profileService: ProfileService,
              private spinner: NgxSpinnerService, private messageService: MessageService, private translate: TranslateService) {
    this.ratingValue = 0;
  }

  ngOnInit() {
    this.ratingCount = [1, 2, 3, 4, 5];
    this.userId = this.profileService.userId;
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.userId = this.profileService.userId;
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if(changes.allRating != null)
    this.manageAllRating(changes.allRating.currentValue);
  }

  getRatingNumber(): number {
    const data: number = this.ratingValue ? +this.ratingValue.toFixed(2) : 0;
    return data;
  }

  checkRating(rating) {
    if (this.allRating) {
      return !!this.allRating.find((x) => x.star === rating);
    } else {
      return false;
    }
  }

  convertDigitIn(enDigit) { // PERSIAN, ARABIC, URDO
    if (this.translate.currentLang === 'en') {
      return enDigit;
    } else {
      const arabicNumbers = ['۰', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩'];
      const chars = enDigit.toString().split('');
      for (let i = 0; i < chars.length; i++) {
        if (/\d/.test(chars[i])) {
          chars[i] = arabicNumbers[chars[i]];
        }
      }
      return chars.join('');
    }
  }

  manageAllRating(data) {
    let allStars = 0;
    let allUsers = 0;
    if (data) {
      data.forEach((item) => {
        allUsers += item.userCount;
        allStars += item.star * item.userCount;
      });
    }
    this.allUsers = allUsers;
    this.ratingValue = allStars / allUsers;
  }

  getPercentage(item) {
    if (this.allUsers > 0) {
      return ((this.allRating.find((x) => x.star === item).userCount / this.allUsers) * 100).toFixed(0);
    } else {
      return 0;
    }
  }

  getUserCount(item) {
    if (this.allUsers > 0) {
      return this.allRating.find((x) => x.star === item).userCount;
    } else {
      return 0;
    }
  }

  rateData(event) {
    if (this.type === 'course') {
      this.rateCourse(event.rating);
    } else {
      this.rateResource(event.rating);
    }
  }

  rateCourse(event) {
    if (this.userId > 0) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.courseService.rateCourse({
        'courseId': this.id,
        'rating': event,
        'comments': '',
        'ratedBy': this.userId
      }).subscribe((res) => {
        if (res.hasSucceeded) {
          this.translate.get('Successfully rated Course').subscribe((trans) => {
            this.messageService.add({severity: 'success', summary: trans, key: 'toast', life: 5000});
          });
        } else {
          this.translate.get(res.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
        this.updateData.emit(true);
      }, (error) => {
        this.translate.get('Failed to rate Course').subscribe((trans) => {
          this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
        });
        this.spinner.hide();
        this.updateData.emit(true);
      });
    } else {
      this.translate.get('Please sign in to Continue').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
      this.updateData.emit(true);
    }
  }

  rateResource(event) {
    if (this.userId) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.resourceService.rateResource({
        'resourceId': this.id,
        'rating': event,
        'comments': '',
        'ratedBy': this.userId
      }).subscribe((res) => {
        if (res.hasSucceeded) {
          this.translate.get('Successfully rated resource').subscribe((trans) => {
            this.messageService.add({severity: 'success', summary: trans, key: 'toast', life: 5000});
          });
        } else {
          this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
        }
        this.spinner.hide();
        this.updateData.emit(true);
      }, (error) => {
        this.translate.get('Failed to rate resource').subscribe((trans) => {
          this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
        });
        this.spinner.hide();
        this.updateData.emit(true);
      });
    } else {
      this.translate.get('Please sign in to Continue').subscribe((trans) => {
        this.messageService.add({severity: 'error', summary: trans, key: 'toast', life: 5000});
      });
      this.updateData.emit(true);
    }
  }

}
