import {Component, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {KeycloakService} from 'keycloak-angular';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {EncService} from '../../services/enc.service';
import {ProfileService} from '../../services/profile.service';
import {ResourcesService} from '../../services/resources/resources.service';
import { Paginator } from 'primeng/components/paginator/paginator';

@Component({
  selector: 'app-resources',
  templateUrl: './resources.component.html',
  styleUrls: ['./resources.component.css']
})
export class ResourcesComponent implements OnInit {

  resources: any[];
  users: any;
  pageNo: number;
  pageSize: number;
  totalLength: number;
  userportalUrl: string;
  queryString : "";
  listload = false;
  showQueryErrorMsg = false;
  Clickfield :string = ''
  ascDescNo: string = 'desc';
  columnNo : number = 0;
  spanColNo : any;

  @ViewChild('pp') paginator: Paginator;

  constructor(private resourseService: ResourcesService,
              private profileService: ProfileService,
              private coolDialogs: ConfirmationService,
              private spinner: NgxSpinnerService,
              private messageService: MessageService,
              public encService: EncService) {
  }

  ngOnInit() {
    this.userportalUrl = environment.userClientUrl;
    this.resources = [];
    this.pageNo = 1;
    this.queryString = this.queryString == undefined || this.queryString == null ? "" : this.queryString;
    this.pageSize = 10;
    this.totalLength = 10;
    this.getResources(this.queryString , this.pageNo, this.pageSize,'desc',0);
  }

  search(){
    this.spinner.show();
    this.paginator.changePage(0);
    this.pageNo = 1;
    this.pageSize = 10;
    this.totalLength = 10;
    this.resourseService.getAllResources(this.queryString,this.pageNo, this.pageSize,'desc',0).subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.resources = response.returnedObject;
        if (this.resources.length > 0) {
          this.totalLength = this.resources[0].totalRows;
        }
        this.spinner.hide();
      } else {
        this.messageService.add({severity: 'error', summary: response.message});
       this.spinner.hide();
      }
    });
  }

  clearSearch(){
    this.paginator.changePage(0);
    this.pageNo = 1;
    this.queryString = "";
    this.Clickfield = "";
    this.pageSize = 10;
    this.totalLength = 10;
    this.getResources(this.queryString , this.pageNo, this.pageSize,'desc',0);
  }

  getResources(queryString ,pageNo, pageSize,ascDescNo,columnNo) {
    this.spinner.show();
    this.resourseService.getAllResources(queryString , pageNo , pageSize,ascDescNo,columnNo).subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.resources = response.returnedObject;
        if (this.resources.length > 0) {
          this.totalLength = this.resources[0].totalRows;
        }
        this.spinner.hide();
      } else {
        this.messageService.add({severity: 'error', summary: response.message});
        this.spinner.hide();
      }
    });
  }
  // SortingAscDesc(flag,columnNo)
  // {
  //   this.paginator.changePage(0);
  //   this.spanColNo = flag+"-"+columnNo;
  //   this.ascDescNo = flag;
  //   this.columnNo = columnNo;
  //   this.queryString = this.queryString == undefined || this.queryString == null ? "" : this.queryString;
  //   this.getResources(this.queryString , 1,10, this.ascDescNo, this.columnNo);
  // }

  Clickingevent(event,columnNo) {
    debugger;
    if(this.Clickfield == event.target.id) {
      this.ascDescNo = this.ascDescNo == 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.ascDescNo = 'asc'
    }
    this.columnNo = columnNo;
    this.paginator.changePage(0);
    this.spanColNo = this.ascDescNo+"-"+columnNo;
    debugger;
    this.queryString = this.queryString == undefined || this.queryString == null ? "" : this.queryString;
    this.getResources(this.queryString , 1,10, this.ascDescNo, this.columnNo);
  }

  paginate(event) {
    this.pageNo = event.page + 1;
    this.queryString = this.queryString == undefined || this.queryString == null ? "" : this.queryString;
    this.getResources(this.queryString ,this.pageNo, 10,this.ascDescNo,this.columnNo);
  }

  deleteCourse(item) {
    this.queryString = this.queryString == undefined || this.queryString == null ? "" : this.queryString;
    this.coolDialogs.confirm({
      message: 'Are you sure that you want to remove this?',
      accept: () => {
        this.resourseService.deleteResource(item.id).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
            if (this.totalLength - 1 === this.pageSize * (this.pageNo - 1)) {
              this.pageNo--;
              // this.pageStart = this.pageSize * (this.pageNo - 1);
            }
            this.getResources(this.queryString ,this.pageNo, this.pageSize,'desc',0);
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
          }
        });
      }
    });
  }

}

