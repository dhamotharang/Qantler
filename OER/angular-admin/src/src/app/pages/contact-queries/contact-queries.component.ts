import {Component, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {NgxSpinnerService} from 'ngx-spinner';
import {MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {EncService} from '../../services/enc.service';
import {GeneralService} from '../../services/general.service';
import {ProfileService} from '../../services/profile.service';
import { Paginator } from 'primeng/components/paginator/paginator';

@Component({
  selector: 'app-contact-queries',
  templateUrl: './contact-queries.component.html',
  styleUrls: ['./contact-queries.component.css']
})
export class ContactQueriesComponent implements OnInit {
  pageNo: number;
  pageSize: number;
  totalLength: number;
  Queries: any;
  showResponseBox: boolean;
  responseFormSubmitted: boolean;
  showResponse: boolean;
  response: any;
  ResponseForm: FormGroup;
  userportalUrl: string;
  searchKeyword: "";
  Clickfield :string = ''
  sortType: string = 'desc';
  sortField : number = 0;
  spanColNo : any;

  @ViewChild('pp') paginator: Paginator;


  constructor(private generalService: GeneralService, private profileService: ProfileService,
              public encService: EncService, private spinner: NgxSpinnerService, private messageService: MessageService) {
  }

  ngOnInit() {
    this.userportalUrl = environment.userClientUrl;
    this.Queries = [];
    this.response = null;
    this.pageNo = 1;
    this.pageSize = 25;
    this.totalLength = 25;
    this.showResponseBox = false;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.showResponse = false;
    this.responseFormSubmitted = false;
    this.ResponseForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      queryId: new FormControl(null, Validators.required),
      email: new FormControl(null, Validators.compose([Validators.required, Validators.email])),
      replyText: new FormControl(null, Validators.compose([Validators.required, Validators.minLength(3)])),
      repliedBy: new FormControl(null, Validators.required)
    });
    this.getQueries(this.pageNo, this.pageSize , this.searchKeyword , 'desc' , 0);
  }

  getQueries(pageNo, pageSize , search , sortType , sortField) {
    this.spinner.show();
    this.generalService.getContactUsQueries(pageNo, pageSize , search , sortType , sortField).subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.Queries = response.returnedObject;
        // this.totalLength = response.returnedObject.length;
        this.spinner.hide();
      } else {
        this.messageService.add({severity: 'error', summary: response.message, key: 'toast', life: 5000});
        this.spinner.hide();
      }
    }, (error) => {
      this.messageService.add({severity: 'error', summary: 'Failed to load Data', key: 'toast', life: 5000});
      this.spinner.hide();
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
    this.spanColNo = this.sortType+"-"+sortField;
    debugger;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getQueries( 1,10 , this.searchKeyword , this.sortType ,  this.sortField );
  }


  clearSearch(){
    this.Queries && this.Queries.length ?  this.paginator.changePage(0) : null;
    this.pageNo = 1;
    this.searchKeyword = "";
    this.Clickfield = "";
    this.pageSize = 10;
    this.totalLength = 10;
    this.getQueries(this.pageNo, this.pageSize , this.searchKeyword , 'desc' , 0);
  }

  search(){
    this.spinner.show();
    this.Queries && this.Queries.length ?  this.paginator.changePage(0) : null;
    this.pageNo = 1;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.pageSize = 10;
    this.totalLength = 10;
    this.generalService.getContactUsQueries(this.pageNo, this.pageSize , this.searchKeyword ,'desc',0 ).subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.Queries = response.returnedObject;
       // this.length = this.courses.length;
        // if (this.length > 0) {
        //   this.totalLength = this.courses[0].totalRows;
        // }
        this.spinner.hide();
      } else {
        this.messageService.add({severity: 'error', summary: response.message});
        this.spinner.hide();
      }
    });
  }

  paginate(event) {
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    this.getQueries(event.page + 1, event.rows , this.searchKeyword , this.sortType , this.sortField);
  }

  respond(contact) {
    this.showResponseBox = true;
    this.ResponseForm.patchValue({
      username: contact.firstName + ' ' + contact.lastName,
      queryId: contact.id,
      email: contact.email,
      repliedBy: this.profileService.user.id
    });
  }

  cancelResponseBox() {
    this.ResponseForm.reset();
    this.showResponseBox = false;
  }

  submitResponseBox(Form) {
    this.responseFormSubmitted = true;
    this.searchKeyword = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    if (Form.valid) {
      this.spinner.show();
      this.generalService.postContactUsQueryResponse(Form.value).subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.responseFormSubmitted = false;
          this.showResponseBox = false;
          this.ResponseForm.reset();
          this.getQueries(this.pageNo, this.pageSize , this.searchKeyword , this.sortType , this.sortField);
          this.messageService.add({severity: 'success', summary: response.message, key: 'toast', life: 5000});
        } else {
          this.messageService.add({severity: 'error', summary: response.message, key: 'toast', life: 5000});
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.messageService.add({severity: 'error', summary: 'Failed to respond to Enquiry', key: 'toast', life: 5000});
      });
    }
  }

  viewResponse(item) {
    this.showResponse = true;
    this.response = item;
  }

  closeResponse() {
    this.showResponse = false;
    this.response = null;
  }

}
