import {Component, OnInit} from '@angular/core';
import {AbuseReportService} from '../../services/abuse-report/abuse-report.service';
import {CoursesService} from '../../services/courses/courses.service';
import {MdmServiceService} from '../../services/mdm/mdm-service.service';
import {ProfileService} from '../../services/profile.service';
import {QrcService} from '../../services/qrc.service';
import {ResourcesService} from '../../services/resources/resources.service';
import {URLService} from '../../services/url.service';
import {UsersService} from '../../services/users/users.service';
import {WCMService} from '../../services/wcm.service';


@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent implements OnInit {
  resource: any;
  course: any;
  user: any;
  users: any;
  abuses: any;
  qrcs: any;
  requests: any[];
  Pages: any[];
  datas: any;
  pageNo: number;
  pageSize: number;

  constructor(private courseService: CoursesService,
              private resourseService: ResourcesService,
              private profileService: ProfileService,
              private userService: UsersService,
              private abuseService: AbuseReportService,
              private QRCService: QrcService,
              private URLservice: URLService,
              private WCMservice: WCMService,
              private  mdmService: MdmServiceService,
  ) {
  }

  ngOnInit() {
    this.resource = 0;
    this.course = 0;
    this.user = null;
    this.users = [];
    this.abuses = [];
    this.qrcs = [];
    this.requests = [];
    this.Pages = [];
    this.pageNo = 1;
    this.pageSize = 10;
    this.datas = 0;
    this.user = this.profileService.user;
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.user = this.profileService.user;
    });
    this.getCourses(this.pageNo, this.pageSize);
    this.getResources(this.pageNo, this.pageSize);
    this.getUsers();
    this.getAbuses();
    this.getQrcList();
    this.getURLRequestList();
    this.getPages();
    this.getData();

  }

  getCourses(pageNo, pageSize) {
    this.courseService.getAllCourses(pageNo, pageSize, "", 'desc', 0).subscribe((response: any) => {
      if (response.hasSucceeded) {
        if (response.returnedObject.length > 0) {
          this.course = response.returnedObject[0].totalRows;
        }
      }
    });
  }

  getResources(pageNo, pageSize) {
    this.resourseService.getAllResources("" , pageNo, pageSize,'desc',0).subscribe((response: any) => {
      if (response.hasSucceeded) {
        if (response.returnedObject.length > 0) {
          this.resource = response.returnedObject[0].totalRows;
        }
      }
    });
  }

  getUsers() {
    this.userService.getAllUsers().subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.users = response.returnedObject;
      }
    });
  }

  getAbuses() {
    this.abuseService.getAllAbuses().subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.abuses = response.returnedObject;
      }
    });
  }

  getQrcList() {
    this.QRCService.getAllQRCs().subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.qrcs = res.returnedObject;
      }
    });
  }

  getURLRequestList() {
    this.URLservice.getAllRequests().subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.requests = res.returnedObject;
      }
    });
  }

  getPages() {
    const categoryId: number =0;
    this.WCMservice.getAllList(categoryId).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.Pages = res.returnedObject;
      }
    });
  }

  getData() {
    this.mdmService.getAllCategories(1).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.datas = this.datas + res.returnedObject.length;
      }
      this.mdmService.getAllCopyrights().subscribe((resu: any) => {
        if (resu.hasSucceeded) {
          this.datas = this.datas + resu.returnedObject.length;
        }
        this.mdmService.getAllEducation().subscribe((resul: any) => {
          if (resul.hasSucceeded) {
            this.datas = this.datas + resul.returnedObject.length;
          }
          this.mdmService.getAllGrades().subscribe((result: any) => {
            if (result.hasSucceeded) {
              this.datas = this.datas + result.returnedObject.length;
            }
            this.mdmService.getAllLanguages().subscribe((data: any) => {
              if (data.hasSucceeded) {
                this.datas = this.datas + data.returnedObject.length;
              }
              this.mdmService.getAllQRC().subscribe((resdata: any) => {
                if (resdata.hasSucceeded) {
                  this.datas = this.datas + resdata.returnedObject.length;
                }
                this.mdmService.getAllStreams().subscribe((resultdata: any) => {
                  if (resultdata.hasSucceeded) {
                    this.datas = this.datas + resultdata.returnedObject.length;
                  }
                  this.mdmService.getAllSubCategories().subscribe((resuldata: any) => {
                    if (resuldata.hasSucceeded) {
                      this.datas = this.datas + resuldata.returnedObject.length;
                    }
                  });
                });
              });
            });
          });
        });
      });

    });

  }
}
