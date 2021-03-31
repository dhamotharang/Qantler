import {Component, OnInit, ViewChild , Pipe} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {KeycloakService} from 'keycloak-angular';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {EncService} from '../../services/enc.service';
import {ProfileService} from '../../services/profile.service';
import {UsersService} from '../../services/users/users.service';
import { Paginator } from 'primeng/components/paginator/paginator';
import _ from 'lodash'

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})

export class UsersComponent implements OnInit {

  courses: any[];
  users: any;
  userList: any;
  userEnd: any;
  length: any;
  userStart: number;
  userportalUrl: string;
  searchKeyword  : string = "";
  sortType: string = 'desc';
  sortField : number = 0;
  Clickfield :string = '';
  spanColNo : any;
  searchbackup : any;

  @ViewChild('pp') paginator: Paginator;
 

  constructor(private userService: UsersService,
              private profileService: ProfileService,
              private coolDialogs: ConfirmationService,
              private spinner: NgxSpinnerService,
              private messageService: MessageService,
              public encService: EncService) {
  }

  ngOnInit() {
    this.userportalUrl = environment.userClientUrl;
    this.users = [];
    this.userEnd = 0;
    this.userStart = 0;
    this.length = 0;
    this.userList = [];
    this.getUsers();
  }

  getUsers() {
    this.users = [];
    this.userEnd = 0;
    this.userStart = 0;
    this.length = 0;
    this.userList = [];
    this.spinner.show();
    this.userService.getAllUsers().subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.users = response.returnedObject;
        this.searchbackup = response.returnedObject;
        this.length = this.users.length;
        this.userEnd = this.users.length;
        this.showCat(this.users);
        this.spinner.hide();
      } else {
        this.messageService.add({severity: 'error', summary: response.message});
        this.spinner.hide();
      }
    });
  }

  showCat(catgory, start = 0) {
    const end = (this.userEnd - start) > 10 ? 10 : (this.userEnd - start);
    this.userList = [];
    for (let i = 0; i < end; i++) {
      this.userList[i] = catgory[start + i];
    }
  }

  search(){
    this.users = this.searchbackup;
    ((this.users &&  this.users.length) && (this.userList &&  this.userList.length))  ? this.paginator.changePage(0) : null;
    debugger;
    const versiondata = [];
    var userdata = this.users;
    var s = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    if(s == "" ) {
      ((this.users &&  this.users.length) && (this.userList &&  this.userList.length)) ? this.paginator.changePage(0) : null;
      this.searchKeyword = "";
      this.Clickfield = "";
      this.getUsers();
    } else {
      userdata &&  userdata.length
      ?  userdata.map(function mm(i, index) {
        if (i.firstName.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
        else if (i.lastName.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.email.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
        }) : [];
        userdata = versiondata;
        this.length = userdata.length;
        this.userEnd = userdata.length;
        this.users = userdata;
        this.showCat(userdata);
    }
  }

  Clickingevent(event,columnNo) {
    debugger;
    var userdata = this.users;
  
    if(this.Clickfield == event.target.id) {
      this.sortType = this.sortType == 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.sortType = 'asc'
    }
    this.sortField = columnNo;
    ((this.users &&  this.users.length) && (this.userList &&  this.userList.length)) ? this.paginator.changePage(0) : null;
    this.spanColNo = this.sortType+"-"+columnNo;
    var sortedArray = _.sortBy(userdata, function(patient) {
      if(event.target.id == 'firstname'){
        return patient.firstName.toLowerCase();
      }
      else if(event.target.id == 'lastname'){
        return patient.lastName.toLowerCase();
      }
      else if(event.target.id == 'email'){
        return patient.email.toLowerCase();
      }
  });
  if(this.sortType == 'desc') {
    sortedArray =   sortedArray.reverse();
  }
    this.length = sortedArray.length;
    this.userEnd = sortedArray.length;
    this.users = sortedArray;
    this.showCat(this.users);
    debugger;
    }

  clearSearch(){
    ((this.users &&  this.users.length) && (this.userList &&  this.userList.length)) ? this.paginator.changePage(0) : null;
    this.searchKeyword = "";
    this.Clickfield = "";
    this.getUsers();
  }

  paginateCat(event) {
    this.showCat(this.users, event.first);
  }
}
