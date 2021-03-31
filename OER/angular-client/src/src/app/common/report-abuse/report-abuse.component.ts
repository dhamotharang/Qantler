import {Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {ResourceService} from '../../services/resource.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {ProfileService} from '../../services/profile.service';
import {CourseService} from '../../services/course.service';
import {Subscription} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-report-abuse',
  templateUrl: './report-abuse.component.html'
})
export class ReportAbuseComponent implements OnInit, OnChanges, OnDestroy {
  @Input() display: boolean;
  @Input() id: number;
  @Input() type: string;
  @Output() displayChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  ReportForm: FormGroup;
  userId: any;
  private sub: Subscription;

  constructor(private resourceService: ResourceService, private translate: TranslateService, private courseService: CourseService, private spinner: NgxSpinnerService, private messageService: MessageService, private profileService: ProfileService) {
  }

  ngOnInit() {
    this.userId = this.profileService.userId;
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.userId = this.profileService.userId;
    });
    this.display = false;
    this.ReportForm = new FormGroup({
      id: new FormControl(this.id, Validators.required),
      spam: new FormControl(null),
      offensive: new FormControl(null),
      misleading: new FormControl(null),
      other: new FormControl(null),
      comment: new FormControl(null)
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.ReportForm = new FormGroup({
      id: new FormControl(null, Validators.required),
      spam: new FormControl(null),
      offensive: new FormControl(null),
      misleading: new FormControl(null),
      other: new FormControl(null),
      comment: new FormControl(null)
    });
    this.ReportForm.patchValue({
      id: this.id ? this.id : null,
      spam: '',
      offensive: '',
      misleading: '',
      other: '',
      comment: '',
    });
  }

  closeModel() {
    this.display = false;
    this.displayChange.emit(false);
  }

  submitReport() {
    if (this.userId) {
      let reportReasons = '';
      if (this.ReportForm.value.spam) {
        reportReasons += '1';
      }
      if (this.ReportForm.value.offensive) {
        if (reportReasons !== '') {
          reportReasons += ',';
        }
        reportReasons += '2';
      }
      if (this.ReportForm.value.misleading) {
        if (reportReasons !== '') {
          reportReasons += ',';
        }
        reportReasons += '3';
      }
      if (this.ReportForm.value.other) {
        if (reportReasons !== '') {
          reportReasons += ',';
        }
        reportReasons += '4';
      }
      if (this.type === 'Resource') {
        const data = {
          'resourceId': this.ReportForm.value.id,
          'reportReasons': reportReasons,
          'comments': this.ReportForm.value.comment,
          'reportedBy': this.userId
        };
        this.spinner.show(undefined, {color: this.profileService.themeColor});
        this.resourceService.reportResource(data).subscribe((response) => {
          if (response.hasSucceeded) {
            this.translate.get('Resource Reported').subscribe((res) => {
              this.messageService.add({severity: 'success', summary: res, key: 'toast', life: 5000});
            });
            this.closeModel();
          } else {
            this.translate.get(response.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
          }
          this.spinner.hide();
        }, (error) => {
          this.translate.get('Failed to report Resource').subscribe((res) => {
            this.messageService.add({severity: 'error', summary: res, key: 'toast', life: 5000});
          });
          this.spinner.hide();
        });
      }
      if (this.type === 'ResourceComment') {
        const data = {
          'resourceCommentId': this.ReportForm.value.id,
          'reportReasons': reportReasons,
          'comments': this.ReportForm.value.comment,
          'reportedBy': this.userId
        };
        this.spinner.show(undefined, {color: this.profileService.themeColor});
        this.resourceService.reportResourceComment(data).subscribe((response) => {
          if (response.hasSucceeded) {
            this.translate.get('Resource Comment Reported').subscribe((res) => {
              this.messageService.add({
                severity: 'success',
                summary: res,
                key: 'toast',
                life: 5000
              });
            });
            this.closeModel();
          } else {
            this.translate.get(response.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
          }
          this.spinner.hide();
        }, (error) => {

          this.translate.get('Failed to report Resource Comment').subscribe((res) => {
            this.messageService.add({
              severity: 'error',
              summary: res,
              key: 'toast',
              life: 5000
            });
          });
          this.spinner.hide();
        });
      }
      if (this.type === 'Course') {
        const data = {
          'courseId': this.ReportForm.value.id,
          'reportReasons': reportReasons,
          'comments': this.ReportForm.value.comment,
          'reportedBy': this.userId
        };
        this.spinner.show(undefined, {color: this.profileService.themeColor});
        this.courseService.reportCourse(data).subscribe((response) => {
          if (response.hasSucceeded) {
            this.translate.get('Course Reported').subscribe((res) => {
              this.messageService.add({severity: 'success', summary: res, key: 'toast', life: 5000});
            });
            this.closeModel();
          } else {
            this.translate.get(response.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
          }
          this.spinner.hide();
        }, (error) => {
          this.translate.get('Failed to report Course').subscribe((res) => {
            this.messageService.add({severity: 'error', summary: res, key: 'toast', life: 5000});
          });
          this.spinner.hide();
        });
      }
      if (this.type === 'CourseComment') {
        const data = {
          'courseCommentId': this.ReportForm.value.id,
          'reportReasons': reportReasons,
          'comments': this.ReportForm.value.comment,
          'reportedBy': this.userId
        };
        this.spinner.show(undefined, {color: this.profileService.themeColor});
        this.courseService.reportCourseComment(data).subscribe((response) => {
          if (response.hasSucceeded) {
            this.translate.get('Course Comment Reported').subscribe((res) => {
              this.messageService.add({
                severity: 'success',
                summary: res,
                key: 'toast',
                life: 5000
              });
            });
            this.closeModel();
          } else {
            this.translate.get(response.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
          }
          this.spinner.hide();
        }, (error) => {
          this.translate.get('Failed to report Course Comment').subscribe((res) => {
            this.messageService.add({
              severity: 'error',
              summary: res,
              key: 'toast',
              life: 5000
            });
          });
          this.spinner.hide();
        });
      }
    } else {
      this.translate.get('Please sign in to report an article').subscribe((res) => {
        this.messageService.add({
          severity: 'error',
          summary: res,
          key: 'toast',
          life: 5000
        });
      });
    }
  }

}
