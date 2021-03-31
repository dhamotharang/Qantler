import {Component, OnInit, ViewChild} from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {QrcService} from '../../services/qrc.service';
import { Paginator } from 'primeng/components/paginator/paginator';


@Component({
  selector: 'app-verifier-reports',
  templateUrl: './verifier-reports.component.html',
  styleUrls: ['./verifier-reports.component.css']
})
export class VerifierReportsComponent implements OnInit {
  reports: any;
  totalReportsCount: number;
  pageNumber: number;
  pageSize: number;
  reportsList: any;
  searchKeyword  : string = "";
  sortType: string = 'desc';
  sortField : number = 0;
  Clickfield :string = '';
  spanColNo : any;

  @ViewChild('pp') paginator: Paginator;

  constructor(private spinner: NgxSpinnerService, private QRCService: QrcService, private messageService: MessageService) {
  }

  ngOnInit() {
    this.totalReportsCount = 0;
    this.pageNumber = 1;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.pageSize = 10;
    this.reportsList = [];
    this.getVerifierReports(this.searchKeyword , this.pageNumber, this.pageSize , 'desc', 0);
  }

  getVerifierReports(keyword , pageNumber, pageSize , sortType , sortField) {
    this.spinner.show();
    debugger;
    this.QRCService.getVerifiersReport(keyword , pageNumber, pageSize , sortType , sortField).subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.reportsList = response.returnedObject;
        if (this.reportsList.length > 0) {
          this.totalReportsCount = this.reportsList[0].totalrows;
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
    this.pageNumber = 1;
    this.pageSize = 10;
    this.totalReportsCount = 10;
    this.QRCService.getVerifiersReport(this.searchKeyword,this.pageNumber, this.pageSize,'desc',0).subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.reportsList = response.returnedObject;
        if (this.reportsList.length > 0) {
          this.totalReportsCount = this.reportsList[0].totalrows;
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
    this.pageNumber = 1;
    this.spanColNo = this.sortType+"-"+sortField;
    debugger;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getVerifierReports(this.searchKeyword , 1 ,10, this.sortType, this.sortField);
  }

  clearSearch(){
   // this.paginator.changePage(0);
    this.pageNumber = 1;
    this.searchKeyword = "";
    this.Clickfield = "";
    this.pageSize = 10;
    this.totalReportsCount = 10;
    this.getVerifierReports(this.searchKeyword , this.pageNumber, this.pageSize,'desc',0);
  }

  paginate(event) {
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getVerifierReports(this.searchKeyword , event.page, this.pageSize , this.sortType , this.sortField);
  }
}
