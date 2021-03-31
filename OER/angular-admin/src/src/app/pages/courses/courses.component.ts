import {Component, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {KeycloakService} from 'keycloak-angular';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {CoursesService} from '../../services/courses/courses.service';
import {EncService} from '../../services/enc.service';
import {ProfileService} from '../../services/profile.service';
import { Paginator } from 'primeng/components/paginator/paginator';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.css']
})
export class CoursesComponent implements OnInit {

  courses: any[];
  users: any;
  courseList: any;
  courseEnd: any;
  pageNo: number;
  pageSize: number;
  totalLength: number;
  length: any;
  courseStart: number;
  userportalUrl: string;
  searchKeyword  : string = "";
  Clickfield :string = '';
  sortType: string = 'desc';
  sortField : number = 0;
  spanColNo : any;

  @ViewChild('pp') paginator: Paginator;

  constructor(private courseService: CoursesService,
              private profileService: ProfileService,
              private coolDialogs: ConfirmationService,
              private spinner: NgxSpinnerService,
              private messageService: MessageService,
              public encService: EncService) {
  }

  ngOnInit() {
    this.userportalUrl = environment.userClientUrl;
    this.courses = [];
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.pageNo = 1;
    this.pageSize = 10;
    this.getCourses(this.pageNo, this.pageSize, this.searchKeyword , 'desc' , 0);
  }

  getCourses(pageNo, pageSize , keyword , sortType , sortField) {
    this.courses = [];
    this.spinner.show();
    this.courseService.getAllCourses(pageNo, pageSize, keyword, sortType, sortField).subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.courses = response.returnedObject;
        this.length = this.courses.length;
        if (this.length > 0) {
          this.totalLength = this.courses[0].totalRows;
        }
        this.spinner.hide();
      } else {
        this.messageService.add({severity: 'error', summary: response.message});
        this.spinner.hide();
      }
    });


  }

  paginate(event) {
    this.pageNo = event.page + 1;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getCourses(this.pageNo, this.pageSize, this.searchKeyword, this.sortType, this.sortField);
  }

  search(){
    this.spinner.show();
    this.paginator.changePage(0);
    this.pageNo = 1;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.pageSize = 10;
    this.totalLength = 10;
    this.courseService.getAllCourses(this.pageNo, this.pageSize,this.searchKeyword,'desc',0).subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.courses = response.returnedObject;
        this.length = this.courses.length;
        if (this.length > 0) {
          this.totalLength = this.courses[0].totalRows;
        }
        this.spinner.hide();
      } else {
        this.messageService.add({severity: 'error', summary: response.message});
        this.spinner.hide();
      }
    });
  }

  SortingAscDesc(flag,sortField)
  {
    this.paginator.changePage(0);
    this.spanColNo = flag+"-"+sortField;
    this.sortType = flag;
    this.pageNo = 1;
    this.pageSize = 10;
    this.sortField = sortField;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getCourses(this.pageNo, this.pageSize, this.searchKeyword, this.sortType, this.sortField);
  }

  Clickingevent(event,sortField) {
    debugger;
    if(this.Clickfield == event.target.id) {
      this.sortType = this.sortType == 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.sortType = 'asc'
    }
    this.sortField = sortField;
    this.paginator.changePage(0);
    this.spanColNo = this.sortType+"-"+sortField;
    debugger;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getCourses( 1,10, this.searchKeyword ,this.sortType, this.sortField);
  }

  clearSearch(){
    this.paginator.changePage(0);
    this.pageNo = 1;
    this.searchKeyword = "";
    this.Clickfield = "";
    this.pageSize = 10;
    this.totalLength = 10;
    this.getCourses(this.pageNo, this.pageSize,this.searchKeyword,'desc',0);
  }

  deleteCourse(item) {
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.coolDialogs.confirm({
      message: 'Are you sure that you want to remove this?',
      accept: () => {
        this.spinner.show();
        this.courseService.deleteCourse(item.id).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
            if (this.totalLength - 1 === this.pageSize * (this.pageNo - 1)) {
              this.pageNo--;
            }
            this.getCourses(this.pageNo, this.pageSize, this.searchKeyword, this.sortType, this.sortField);
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.spinner.hide();
          }
        });
      }
    });
  }

}
