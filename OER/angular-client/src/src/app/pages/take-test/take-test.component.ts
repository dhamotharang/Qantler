import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {CourseService} from '../../services/course.service';
import {ProfileService} from '../../services/profile.service';
import {MessageService} from 'primeng/api';
import {EncService} from '../../services/enc.service';
import {Title} from '@angular/platform-browser';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-take-test',
  templateUrl: './take-test.component.html'
})
export class TakeTestComponent implements OnInit {
  id: any;
  dataLoadFailed: boolean;
  courseEnrolled: boolean;
  courseTest: any;
  StorageUrl: any;
  submittedAnswers: any[];

  constructor(private titleService: Title, public encService: EncService, private translate: TranslateService, private route: ActivatedRoute, private router: Router, private spinner: NgxSpinnerService, private courseService: CourseService, private profileService: ProfileService, private messageService: MessageService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Take Test | UAE - Open Educational Resources');
    this.id = null;
    this.submittedAnswers = [];
    this.dataLoadFailed = false;
    this.courseEnrolled = false;
    this.courseTest = null;
    this.route.params.subscribe(params => {
      this.id = params['id'];
      if (this.id && this.profileService.userId) {
        this.getTest(this.id);
      }
    });
    this.profileService.getUserDataUpdate().subscribe(() => {
      if (this.id && this.profileService.userId) {
        this.getTest(this.id);
      }
    });
  }

  getTest(id) {
    if (this.profileService.userId) {
      this.courseService.postCourseEnrolledStatus({
        courseId: +id,
        userId: this.profileService.userId
      }).subscribe((response) => {
        if (response.hasSucceeded) {
          this.courseEnrolled = true;
          this.spinner.show(undefined, {color: this.profileService.themeColor});
          this.courseService.getCourseTest(+id).subscribe((res) => {
            if (res.hasSucceeded) {
              this.dataLoadFailed = false;
              this.courseTest = res.returnedObject;
            } else {
              this.dataLoadFailed = true;
            }
            this.spinner.hide();
          }, (error) => {
            this.dataLoadFailed = true;
            this.spinner.hide();
          });
        } else {
          this.dataLoadFailed = true;
          this.courseEnrolled = false;
        }
      });
    }
  }

  submitAnswer(answers) {
    if (answers.length === this.courseTest.questions.length) {
      const data = {
        answers: [],
        courseId: +this.id,
        userId: +this.profileService.user.id
      };
      this.courseTest.questions.forEach((question, index) => {
        data.answers.push({
          answerId: +answers[index],
          questionId: +question.id
        });
      });
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.courseService.postCourseTest(data).subscribe((res) => {
        if (res.hasSucceeded) {
          this.translate.get('Test Result sent to your registered email').subscribe((msg) => {
            this.messageService.add({severity: 'success', summary: msg, key: 'toast'});
          });
          this.router.navigateByUrl('course/' + this.encService.set(+this.id));
        } else {
          this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
          this.dataLoadFailed = true;
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.translate.get('Failed to submit test').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast'});
        });
      });
    } else {
      this.translate.get('Please answer all questions').subscribe((msg) => {
        this.messageService.add({severity: 'error', summary: msg, key: 'toast'});
      });
    }
  }
}
