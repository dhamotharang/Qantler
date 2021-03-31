import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {CourseService} from '../../services/course.service';
import {environment} from '../../../environments/environment';
import {Title} from '@angular/platform-browser';
import {ProfileService} from '../../services/profile.service';

@Component({
  selector: 'app-take-test',
  templateUrl: './verify-test.component.html'
})
export class VerifyTestComponent implements OnInit {
  id: any;
  dataLoadFailed: boolean;
  courseTest: any;
  StorageUrl: any;

  constructor(private titleService: Title, private route: ActivatedRoute, private profileService: ProfileService, private spinner: NgxSpinnerService, private courseService: CourseService) {
  }

  ngOnInit() {
    this.titleService.setTitle('Verify Test | UAE - Open Educational Resources');
    this.id = null;
    this.dataLoadFailed = false;
    this.courseTest = null;
    this.route.params.subscribe(params => {
      this.id = params['id'];
      if (this.id) {
        this.getTest(this.id);
      }
    });
  }

  getTest(id) {
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
  }

}
