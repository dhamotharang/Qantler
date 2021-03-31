import {Component, OnInit} from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {EncService} from '../../services/enc.service';
import {ProfileService} from '../../services/profile.service';
import {ResourcesService} from '../../services/resources/resources.service';

@Component({
  selector: 'app-rejection-list',
  templateUrl: './rejection-list.component.html',
  styleUrls: ['./rejection-list.component.css']
})
export class RejectionListComponent implements OnInit {

  resources: any[];
  users: any;
  pageNo: number;
  pageSize: number;
  totalLength: number;
  userportalUrl: string;
  searchKeyword  : string = "";
  sortType: string = 'desc';
  sortField : number = 0;
  Clickfield :string = '';
  spanColNo : any;

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
    this.pageSize = 10;
    this.totalLength = 10;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getList(this.searchKeyword ,this.pageNo, this.pageSize, this.sortType,this.sortField);
  }

  getList(keyword , pageNo, pageSize , sortType , sortField) {
    this.spinner.show();
    this.resourseService.getRejectionList(keyword , pageNo, pageSize ,sortType ,sortField).subscribe((response: any) => {
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

  
  search(){
    this.spinner.show();
    //this.paginator.changePage(0);
    this.pageNo = 1;
    this.pageSize = 10;
    this.totalLength = 10;
    this.resourseService.getRejectionList(this.searchKeyword,this.pageNo, this.pageSize,'desc',0).subscribe((response: any) => {
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

  Clickingevent(event,sortField) {
    debugger;
    if(this.Clickfield == event.target.id) {
      this.sortType = this.sortType == 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.sortType = 'asc'
    }
    this.sortField = sortField;
   // this.paginator.changePage(0);
    this.pageNo = 1;
    this.spanColNo = this.sortType+"-"+sortField;
    debugger;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getList(this.searchKeyword , 1 ,10, this.sortType, this.sortField);
  }

  clearSearch(){
   // this.paginator.changePage(0);
    this.pageNo = 1;
    this.searchKeyword = "";
    this.Clickfield = "";
    this.pageSize = 10;
    this.totalLength = 10;
    this.getList(this.searchKeyword , this.pageNo, this.pageSize,'desc',0);
  }

  paginate(event) {
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getList(this.searchKeyword , event.page + 1, 10,this.sortType , this.sortField);
  }

}


