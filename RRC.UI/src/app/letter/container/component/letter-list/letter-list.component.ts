import { Component, OnInit, ViewChild, HostListener, TemplateRef, OnDestroy } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { CommonService } from '../../../../common.service';
import { LetterListService } from './letterlist.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { LetterReportModalComponent } from '../../../../modal/letter-report-modal/letter-report-modal.component';
import { environment } from 'src/environments/environment';
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: 'app-letter-list',
  templateUrl: './letter-list.component.html',
  styleUrls: ['./letter-list.component.scss']
})
export class LetterListComponent implements OnInit, OnDestroy {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('checkTemplate') checkTemplate: TemplateRef<any>;
  bsConfig: Partial<BsDatepickerConfig>;
  bsConfigs: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  bsModalRef: BsModalRef;
  public letter_type = 'My Pending Actions Incoming';
  public letter_type_id = 1;
  public user_name: any = 'TestUser 1';
  message: any;
  selected: any[];
  currentStatus: string = 'All';
  OrgUnitID = 0;
  bulk_data: any[];
  filter_data: any = [];
  filter = false;
  status = (this.commonservice.language == 'English') ? 'All' : this.arabic('all');
  source = 'All';
  destination = 'All';
  username = 'All';
  date_from = '';
  date_to = '';
  private = '';
  priority = 'All';
  smartSearch = '';
  statusOptions = [];
  StatusDropdown = [];
  userList = [];
  sourceouOptions = [];
  destinationOptions = [];
  privateOptions = ['Yes', 'No'];
  priorityOptions = ['All', 'High', 'Medium', 'Low', 'VeryLow'];
  dest_sourceSettings: { singleSelection: boolean; idField: string; textField: string; selectAllText: string; unSelectAllText: string; itemsShowLimit: number; allowSearchFilter: boolean; };
  statusDisable = false;
  public rows: Array<any> = [];
  public columns: Array<any> = [];
  public page: number = 1;
  public itemsPerPage: number = 10;
  public maxSize: number = 10;
  public numPages: number = 1;
  public length: number = 0;
  tableMessages: any;
  public alreadyExist: boolean = false;

  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };

  private data: any;
  letter_id: any;
  progress: boolean = false;
  sideSubscribe: any;

  validateDueStartEndDate: any = {
    isValid: true,
    msg: ''
  };
  bulkApproveEnable: boolean;
  destinationFieldShown: boolean = true;
  usernameFieldShown: boolean = true;
  sourceFieldShown: boolean = false;

  constructor(public commonservice: CommonService, public util: UtilsService, public router: Router, private letterlistservice: LetterListService, public datePipe: DatePipe, private modalService: BsModalService) {
    this.tableMessages = {
      emptyMessage: (this.commonservice.language === 'English') ? 'No Items Found' : this.arabic('noItemsFound')
    }
    this.sideSubscribe = this.commonservice.sideNavChanged$.subscribe(data => {
      this.page = 1;
      this.usernameFieldShown = true;
      let user = localStorage.getItem("User");
      let userdet = JSON.parse(user);
      let userID = userdet.id;
      this.OrgUnitID = userdet.OrgUnitID;
      if (data.type == 'letter') {
        this.rows = [];
        this.resetFilter();
        this.getComboList();
        this.letter_type = data.title;
        let letter = (this.commonservice.language == 'English') ? 'Letter' : this.arabic('letter'),
          letterType = (this.commonservice.language == 'English') ? this.letter_type : this.arabic(this.letter_type),
          list = (this.commonservice.language == 'English') ? 'List' : this.arabic('list');
        this.commonservice.breadscrumChange(letter, letterType, list);
        if (this.letter_type == 'My Pending Actions Outgoing') {
          this.destinationFieldShown = false;
          this.sourceFieldShown = true;
          this.columns = [
            {
              prop: 'selected',
              name: '',
              sortable: false,
              canAutoResize: false,
              draggable: false,
              resizable: false,
              headerCheckboxable: true,
              checkboxable: true,
              width: 30
            },
            { name: 'Ref ID', prop: 'ReferenceNumber' },
            { name: 'Title', prop: 'Title' },
            { name: 'Destination', prop: 'Destination' },
            { name: 'Source', prop: 'Source' },
            { name: 'Sender Name', prop: 'SenderName' },
            { name: 'Status', prop: 'Status' },
            { name: 'Date', prop: 'Date' },
            { name: 'Need Reply', prop: 'Replied' },
            { name: 'Priority', prop: 'NewPriority' },
            { name: 'Action', prop: '', cellTemplate: this.actionTemplate }

          ];
          if (this.commonservice.language != 'English') {
            this.columns = [
              {
                prop: 'selected',
                name: '',
                sortable: false,
                canAutoResize: false,
                draggable: false,
                resizable: false,
                headerCheckboxable: true,
                checkboxable: true,
                width: 30
              },
              { name: this.arabic('refid'), prop: 'ReferenceNumber' },
              { name: this.arabic('title'), prop: 'Title' },
              { name: this.arabic('destination'), prop: 'Destination' },
              { name: this.arabic('source'), prop: 'Source' },
              { name: this.arabic('sendername'), prop: 'SenderName' },
              { name: this.arabic('status'), prop: 'Status' },
              { name: this.arabic('date'), prop: 'Date' },
              { name: this.arabic('needreply'), prop: 'Replied' },
              { name: this.arabic('priority'), prop: 'NewPriority' },
              { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
            ];
          }
        } else if (this.letter_type == 'Incoming Letters') {
          this.destinationFieldShown = true;
          this.sourceFieldShown = false;
          this.columns = [
            // {
            //   prop: 'selected',
            //   name: '',
            //   sortable: false,
            //   canAutoResize: false,
            //   draggable: false,
            //   resizable: false,
            //   headerCheckboxable: true,
            //   checkboxable: true,
            //   width: 30
            // },
            { name: 'Ref ID', prop: 'ReferenceNumber' },
            { name: 'Title', prop: 'Title' },
            { name: 'Sender Entity', prop: 'SenderEntity' },
            { name: 'Destination', prop: 'Destination' },
            { name: 'Destination Name', prop: 'UserName' },
            { name: 'Status', prop: 'Status' },
            { name: 'Linked to other letter', prop: 'LinkedToOtherLetter' },
            { name: 'Date', prop: 'Date' },
            { name: 'Need Reply', prop: 'Replied' },
            { name: 'Priority', prop: 'NewPriority' },
            { name: 'Action', prop: '', cellTemplate: this.actionTemplate }
          ];
          if (this.commonservice.language != 'English') {
            this.columns = [
              // {
              //   prop: 'selected',
              //   name: '',
              //   sortable: false,
              //   canAutoResize: false,
              //   draggable: false,
              //   resizable: false,
              //   headerCheckboxable: true,
              //   checkboxable: true,
              //   width: 30
              // },
              { name: this.arabic('refid'), prop: 'ReferenceNumber' },
              { name: this.arabic('title'), prop: 'Title' },
              { name: this.arabic('senderentity'), prop: 'SenderEntity' },
              { name: this.arabic('destination'), prop: 'Destination' },
              { name: this.arabic('destinationname'), prop: 'UserName' },
              { name: this.arabic('status'), prop: 'Status' },
              { name: this.arabic('linkedtootherletter'), prop: 'LinkedToOtherLetter' },
              { name: this.arabic('date'), prop: 'Date' },
              { name: this.arabic('needreply'), prop: 'Replied' },
              { name: this.arabic('priority'), prop: 'NewPriority' },
              { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
            ];
          }
        }
        else if (this.letter_type == 'Outgoing Letters') {
          this.destinationFieldShown = false;
          this.sourceFieldShown = true;
          this.columns = [
            { name: 'Ref ID', prop: 'ReferenceNumber' },
            { name: 'Title', prop: 'Title' },
            { name: 'Destination', prop: 'Destination' },
            { name: 'Source', prop: 'Source' },
            { name: 'Sender Name', prop: 'SenderName' },
            { name: 'Status', prop: 'Status' },
            { name: 'Date', prop: 'Date' },
            { name: 'Need Reply', prop: 'Replied' },
            { name: 'Priority', prop: 'NewPriority' },
            { name: 'Action', prop: '', cellTemplate: this.actionTemplate }

          ];
          if (this.commonservice.language != 'English') {
            this.columns = [
              { name: this.arabic('refid'), prop: 'ReferenceNumber' },
              { name: this.arabic('title'), prop: 'Title' },
              { name: this.arabic('destination'), prop: 'Destination' },
              { name: this.arabic('source'), prop: 'Source' },
              { name: this.arabic('sendername'), prop: 'SenderName' },
              { name: this.arabic('status'), prop: 'Status' },
              { name: this.arabic('date'), prop: 'Date' },
              { name: this.arabic('needreply'), prop: 'Replied' },
              { name: this.arabic('priority'), prop: 'NewPriority' },
              { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
            ];
          }
        }
        else if (this.letter_type == 'My Pending Actions Incoming') {
          this.destinationFieldShown = true;
          this.sourceFieldShown = false;
          this.columns = [
            { name: 'Ref ID', prop: 'ReferenceNumber' },
            { name: 'Title', prop: 'Title' },
            { name: 'Sender Entity', prop: 'SenderEntity' },
            { name: 'Destination', prop: 'Destination' },
            { name: 'Destination Name', prop: 'UserName' },
            { name: 'Status', prop: 'Status' },
            { name: 'Linked to other letter', prop: 'LinkedToOtherLetter' },
            { name: 'Date', prop: 'Date' },
            { name: 'Need Reply', prop: 'Replied' },
            { name: 'Priority', prop: 'NewPriority' },
            { name: 'Action', prop: '', cellTemplate: this.actionTemplate }
          ];
          if (this.commonservice.language != 'English') {
            this.columns = [
              { name: this.arabic('refid'), prop: 'ReferenceNumber' },
              { name: this.arabic('title'), prop: 'Title' },
              { name: this.arabic('senderentity'), prop: 'SenderEntity' },
              { name: this.arabic('destination'), prop: 'Destination' },
              { name: this.arabic('destinationname'), prop: 'UserName' },
              { name: this.arabic('status'), prop: 'Status' },
              { name: this.arabic('linkedtootherletter'), prop: 'LinkedToOtherLetter' },
              { name: this.arabic('date'), prop: 'Date' },
              { name: this.arabic('needreply'), prop: 'Replied' },
              { name: this.arabic('priority'), prop: 'NewPriority' },
              { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
            ];
          }
        }
        else {
          if (this.letter_type == 'Draft Letters') {
            this.sourceFieldShown = false;
            this.destinationFieldShown = false;
            this.usernameFieldShown = false;
            if (this.OrgUnitID == 14) {
            this.columns = [
              { name: 'Ref ID', prop: 'ReferenceNumber' },
              { name: 'Title', prop: 'Title' },
              { name: 'Destination', prop: 'Destination' },
              { name: 'Sender Name', prop: 'SenderName' },
              { name: 'Status', prop: 'Status' },
              { name: 'Linked to other letter', prop: 'LinkedToOtherLetter' },
              { name: 'Date', prop: 'Date' },
              { name: 'Need Reply', prop: 'Replied' },
              { name: 'Priority', prop: 'NewPriority' },
              { name: 'Action', prop: '', cellTemplate: this.actionTemplate }
            ];
            if (this.commonservice.language != 'English') {
              this.columns = [
                { name: this.arabic('refid'), prop: 'ReferenceNumber' },
                { name: this.arabic('title'), prop: 'Title' },
                { name: this.arabic('destination'), prop: 'Destination' },
                { name: this.arabic('sendername'), prop: 'SenderName' },
                { name: this.arabic('status'), prop: 'Status' },
                { name: this.arabic('linkedtootherletter'), prop: 'LinkedToOtherLetter' },
                { name: this.arabic('date'), prop: 'Date' },
                { name: this.arabic('needreply'), prop: 'Replied' },
                { name: this.arabic('priority'), prop: 'NewPriority' },
                { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
              ];
            }
          }
          else{            
            this.columns = [
              { name: 'Ref ID', prop: 'ReferenceNumber' },
              { name: 'Title', prop: 'Title' },
              { name: 'Destination', prop: 'Destination' },
              { name: 'Status', prop: 'Status' },
              { name: 'Linked to other letter', prop: 'LinkedToOtherLetter' },
              { name: 'Date', prop: 'Date' },
              { name: 'Need Reply', prop: 'Replied' },
              { name: 'Priority', prop: 'NewPriority' },
              { name: 'Action', prop: '', cellTemplate: this.actionTemplate }
            ];
            if (this.commonservice.language != 'English') {
              this.columns = [
                { name: this.arabic('refid'), prop: 'ReferenceNumber' },
                { name: this.arabic('title'), prop: 'Title' },
                { name: this.arabic('destination'), prop: 'Destination' },
                { name: this.arabic('status'), prop: 'Status' },
                { name: this.arabic('linkedtootherletter'), prop: 'LinkedToOtherLetter' },
                { name: this.arabic('date'), prop: 'Date' },
                { name: this.arabic('needreply'), prop: 'Replied' },
                { name: this.arabic('priority'), prop: 'NewPriority' },
                { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
              ];
            }
          }
          }
          else {
            this.sourceFieldShown = true;
            this.destinationFieldShown = true;

            this.columns = [
              { name: 'Ref ID', prop: 'ReferenceNumber' },
              { name: 'Title', prop: 'Title' },
              { name: 'Source', prop: 'Source' },
              { name: 'Destination', prop: 'Destination' },
              { name: 'Sender Name', prop: 'UserName' },
              { name: 'Status', prop: 'Status' },
              { name: 'Linked to other letter', prop: 'LinkedToOtherLetter' },
              { name: 'Date', prop: 'Date' },
              { name: 'Need Reply', prop: 'Replied' },
              { name: 'Priority', prop: 'NewPriority' },
              { name: 'Action', prop: '', cellTemplate: this.actionTemplate }
            ];
            if (this.commonservice.language != 'English') {
              this.columns = [
                { name: this.arabic('refid'), prop: 'ReferenceNumber' },
                { name: this.arabic('title'), prop: 'Title' },
                { name: this.arabic('source'), prop: 'Source' },
                { name: this.arabic('destination'), prop: 'Destination' },
                { name: this.arabic('username'), prop: 'UserName' },
                { name: this.arabic('status'), prop: 'Status' },
                { name: this.arabic('linkedtootherletter'), prop: 'LinkedToOtherLetter' },
                { name: this.arabic('date'), prop: 'Date' },
                { name: this.arabic('needreply'), prop: 'Replied' },
                { name: this.arabic('priority'), prop: 'NewPriority' },
                { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
              ];
            }
          }
        }

        this.commonservice.sideNavResponse('letter', this.letter_type);
        let user = localStorage.getItem("User");
        let userdet = JSON.parse(user);
        let userID = userdet.id;
        this.OrgUnitID = userdet.OrgUnitID;
        //this.letter_type = 'My Pending Actions';
        if (this.letter_type == 'My Pending Actions Incoming') {
          this.letter_type_id = 1;
          if (this.commonservice.language == 'English')
            this.currentStatus = 'All';
          else
            this.currentStatus = this.arabic('all');
          this.statusDisable = false;
        } else if (this.letter_type == 'My Pending Actions Outgoing') {
          this.letter_type_id = 2;
          if (this.commonservice.language == 'English')
            this.currentStatus = 'All';
          else
            this.currentStatus = this.arabic('all');
          this.statusDisable = false;
        } else if (this.letter_type == 'Incoming Letters') {
          if (this.commonservice.language == 'English')
            this.currentStatus = 'All';
          else
            this.currentStatus = this.arabic('all');
          this.statusDisable = false;
          this.letter_type_id = 4;
        } else if (this.letter_type == 'Outgoing Letters') {
          if (this.commonservice.language == 'English')
            this.currentStatus = 'All';
          else
            this.currentStatus = this.arabic('all');
          this.statusDisable = false;
          this.letter_type_id = 3;
        } else if (this.letter_type == 'Draft Letters') {
          this.letter_type_id = 5;
          if (this.commonservice.language == 'English')
            this.currentStatus = 'Draft';
          else
            this.currentStatus = this.arabic('draft');
          this.statusDisable = true;
        } else if (this.letter_type == 'Historical Letters Incoming') {
          this.letter_type_id = 6;
          this.currentStatus = '';
          this.statusDisable = false;
        } else if (this.letter_type == 'Historical Letters Outgoing') {
          this.letter_type_id = 7;
          this.currentStatus = '';
          this.statusDisable = false;
        }
        if (this.letter_type_id) {
          this.status = this.currentStatus;
          this.getservice();
        }
        if (this.letter_type == 'Outgoing Letters')
          this.statusOptions = this.statusOptions.filter(person => person.DisplayName != 'Draft');
      }
      // let OrgUnitID = this.cu.OrgUnitID;
      // this.showTopPanelData.btn_url = type + '/outgoingletter-create';
      // this.showTopPanelData.buttonName = "+ CREATE LETTER";
      // if (this.OrgUnitID == 14) {
      //   this.commonservice.topBanner(true, this.letter_type, '+ CREATE LETTER', 'letter/incomingletter-create');
      // } else {
      //   this.commonservice.topBanner(true, this.letter_type, '+ CREATE LETTER', 'letter/outgoingletter-create');
      // }
    });
  }
  public getComboList() {
    let user_id = '1';
    let letter_id = '0';
    let requestData = [];
    let dropdown = [];
    this.StatusDropdown = [];
    this.sourceouOptions = [];
    let sourceou = [];
    this.destinationOptions = [];
    let currentUser = localStorage.getItem("User");
    let userdet = JSON.parse(currentUser);
    let userID = userdet.id;
    this.letterlistservice.memoCombos("InboundLetters/", 1, userID).subscribe((res: any) => {
      let allvar; let source;
      if (this.commonservice.language == 'English') {
        allvar = { DisplayName: "All" };
        source = { OrganizationUnits: "All" };
      }
      else {
        allvar = { DisplayName: this.arabic('all') };
        source = { OrganizationUnits: this.arabic('all') };
      }
      // sourceou.push(source);
      dropdown.push(allvar);
      this.statusOptions = res.M_LookupsList;
      //this.status = this.currentStatus;
      res.M_OrganizationList.forEach(element => {
        sourceou.push(element);
        this.destinationOptions
      });
      this.sourceouOptions = sourceou;
      var calendar_id = environment.calendar_id;
      this.destinationOptions = sourceou.filter(ret => calendar_id != ret.OrganizationID);
      if (this.letter_type == 'Incoming Letters') {
        this.statusOptions = this.statusOptions.filter(person => (person.LookupsID != 24) && (person.LookupsID != 28));
        this.statusOptions.forEach(element => {
          dropdown.push(element);
        });
      }
      this.StatusDropdown = dropdown;
      if (this.letter_type == 'My Pending Actions Outgoing') {
        this.statusOptions.forEach((items) => {
          if ((items.LookupsID == 25) || (items.LookupsID == 26)) {
            dropdown.push(items);
          }
          this.StatusDropdown = dropdown;
        }
        );
      }
      if (this.letter_type == 'My Pending Actions Incoming') {
        this.statusOptions.forEach((items) => {
          if ((items.LookupsID == 25) || (items.LookupsID == 26)) {
            dropdown.push(items);
          }
          this.StatusDropdown = dropdown;
        }
        );
      }
      if (this.letter_type == 'Outgoing Letters') {
        this.statusOptions.forEach((items) => {
          if ((items.LookupsID != 24) && (items.LookupsID != 28)) {
            dropdown.push(items);
          }
        });
        this.StatusDropdown = dropdown;
      }
    });
    let userdropdown = [];
    let user;
    if (this.commonservice.language == 'English')
      user = { EmployeeName: 'All' };
    else {
      user = { EmployeeName: this.arabic('all') };
    }
    userdropdown.push(user)
    this.letterlistservice.userList('User/', requestData).subscribe((res: any) => {
      res.forEach(element => {
        userdropdown.push(element);
      });
      this.userList = userdropdown;
    });
  }

  resetFilter() {
    if (this.commonservice.language == 'English') {
      this.status = 'All';
      this.source = 'All';
      this.destination = 'All';
      this.date_from = '';
      this.date_to = '';
      this.priority = 'All';
      this.smartSearch = '';
      this.username = 'All';
      this.filter_data.sender_name = 'All';
    }
    else {
      this.status = this.arabic('all');
      this.source = this.arabic('all');
      this.destination = this.arabic('all');
      this.date_from = '';
      this.date_to = '';
      this.priority = this.arabic('all');
      this.smartSearch = '';
      this.username = this.arabic('all');
      this.filter_data.sender_name = this.arabic('all');
    }
    // if (this.filter_data.sender_name) {
    //   this.filter_data.sender_name = '';
    // }
  }

  public changeList() {
    //this.filter_data = this;
    this.filter = true;
    let source = '';
    let destination = '';
    let user_name = '';
    let from_date = '';
    let to_date = '';
    let sender_name = '';
    let priority = '';
    let status = '';
    if (this.filter_data.status && this.filter_data.status != 'All' && this.filter_data.status != this.arabic('all')) {
      status = this.filter_data.status;
    }
    if (this.filter_data.date_from) {
      from_date = new Date(this.filter_data.date_from).toJSON();
    }
    if (this.filter_data.date_to) {
      to_date = new Date(this.filter_data.date_to).toJSON();
    }
    if (this.filter_data.source && this.filter_data.source != 'All' && this.filter_data.source != this.arabic('all')) {
      source = this.filter_data.source;
      //source = source.replace("&", "amp;");

    }
    if (this.filter_data.destination && this.filter_data.destination != 'All' && this.filter_data.destination != this.arabic('all')) {
      destination = this.filter_data.destination;
      if (this.commonservice.language == 'English')
        destination = destination.replace("&", "amp;");

    }
    if (this.filter_data.username && this.filter_data.username != 'All' && this.filter_data.username != this.arabic('all')) {
      user_name = this.filter_data.username;
    }
    if (this.filter_data.sender_name && this.filter_data.sender_name != 'All' && this.filter_data.sender_name != this.arabic('all')) {
      sender_name = this.filter_data.sender_name;
    }
    if (this.filter_data.priority != null && this.filter_data.priority != 'All' && this.filter_data.priority != this.arabic('all')) {
      priority = this.filter_data.priority;
    }
    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let userID = userdet.id;
    this.OrgUnitID = userdet.OrgUnitID;

    this.letterlistservice.letterFilterList("InboundLetters/", this.page, this.maxSize, this.letter_type_id, userID, status, source, destination, user_name, from_date, to_date, priority, sender_name, this.filter_data.smartSearch)
      .subscribe((res: any) => {
        this.data = res.Collection;

        if (this.data) {
          this.length = res.Count;
          //this.maxSize = this.data.count
        }

        var click = "(click)='editClick()'";
        if (this.data) {
          this.data.forEach(val => {
            var high = (this.commonservice.language == 'English') ? 'High' : this.arabic('high'),
              medium = (this.commonservice.language == 'English') ? 'Medium' : this.arabic('medium'),
              low = (this.commonservice.language == 'English') ? 'Low' : this.arabic('low'),
              verylow = (this.commonservice.language == 'English') ? 'VeryLow' : this.arabic('verylow');
            if (val['Priority'] == high) {
              val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == medium) {
              val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == low) {
              val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == verylow) {
              val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            val.Date = this.datePipe.transform(val.Date, "dd/MM/yyyy");
          });
          this.onChangeTable(this.config);
        }
      });
  }
  public getservice() {
    this.progress = true;
    this.filter_data = this;
    this.filter = true;
    let source = '';
    let destination = '';
    let user_name = '';
    let from_date = '';
    let to_date = '';
    let priority = '';
    let status = '';
    if (this.filter_data.status != 'All' && this.filter_data.status != this.arabic('all')) {
      status = this.filter_data.status;
    }
    if (this.filter_data.date_from) {
      from_date = new Date(this.filter_data.date_from).toJSON();
    }
    if (this.filter_data.date_to) {
      to_date = new Date(this.filter_data.date_to).toJSON();
    }
    if (this.filter_data.source && this.filter_data.source != 'All' && this.filter_data.source != this.arabic('all')) {
      source = this.filter_data.source;
    }
    if (this.filter_data.destination && this.filter_data.destination != 'All' && this.filter_data.source != this.arabic('all')) {
    }
    if (this.filter_data.username && this.filter_data.username != 'All' && this.filter_data.source != this.arabic('all')) {
      user_name = this.filter_data.username;
    }
    if (this.filter_data.priority != null && this.filter_data.priority != 'All' && this.filter_data.source != this.arabic('all')
    ) {
      priority = this.filter_data.priority;
    }
    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let userID = userdet.id;
    let sender_name = '';

    this.letterlistservice.letterFilterList("InboundLetters/", this.page, this.maxSize, this.letter_type_id, userID, status, source, destination, user_name, from_date, to_date, priority, sender_name, this.filter_data.smartSearch)
      .subscribe((res: any) => {
        this.progress = false;
        this.data = res.Collection;
        if (this.data) {
          this.length = res.Count;
          //this.maxSize = this.data.count
        }
        //this.changePage(1,this.data);
        var click = "(click)='editClick()'";
        if (this.data) {
          this.data.forEach(val => {
            var high = (this.commonservice.language == 'English') ? 'High' : this.arabic('high'),
              medium = (this.commonservice.language == 'English') ? 'Medium' : this.arabic('medium'),
              low = (this.commonservice.language == 'English') ? 'Low' : this.arabic('low'),
              verylow = (this.commonservice.language == 'English') ? 'VeryLow' : this.arabic('verylow');
            if (val['Priority'] == high) {
              val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == medium) {
              val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == low) {
              val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            if (val['Priority'] == verylow) {
              val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
            }
            val.Date = this.datePipe.transform(val.Date, "dd/MM/yyyy");
          });
          this.onChangeTable(this.config);
        }
      });
  }
  public ngOnInit(): void {
    this.getComboList();
    this.getservice();
    this.priority = this.priorityOptions[0];
    this.commonservice.sideNavResponse('letter', this.letter_type);
    this.columns = [
      { name: 'Ref ID', prop: 'ReferenceNumber' },
      { name: 'Title', prop: 'Title' },
      { name: 'Sender Entity', prop: 'SenderEntity' },
      { name: 'Destination', prop: 'Destination' },
      { name: 'Destination Name', prop: 'UserName' },
      { name: 'Status', prop: 'Status' },
      { name: 'Linked to other letter', prop: 'LinkedToOtherLetter' },
      { name: 'Date', prop: 'Date' },
      { name: 'Need Reply', prop: 'Replied' },
      { name: 'Priority', prop: 'NewPriority' },
      { name: 'Action', prop: '', cellTemplate: this.actionTemplate }
    ];

    if (this.commonservice.language != 'English') {
      this.privateOptions = [this.arabic('yes'), this.arabic('no')];
      this.priorityOptions = [this.arabic('all'), this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
      this.priority = this.priorityOptions[0];
      this.columns = [
        { name: this.arabic('refid'), prop: 'ReferenceNumber' },
        { name: this.arabic('title'), prop: 'Title' },
        { name: this.arabic('senderentity'), prop: 'SenderEntity' },
        { name: this.arabic('destination'), prop: 'Destination' },
        { name: this.arabic('destinationname'), prop: 'UserName' },
        { name: this.arabic('status'), prop: 'Status' },
        { name: this.arabic('linkedtootherletter'), prop: 'LinkedToOtherLetter' },
        { name: this.arabic('date'), prop: 'Date' },
        { name: this.arabic('needreply'), prop: 'Replied' },
        { name: this.arabic('priority'), prop: 'NewPriority' },
        { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
      ];
      this.status = this.arabic('all');
      this.source = this.arabic('all');
      this.destination = this.arabic('all');
      this.username = this.arabic('all');
    }
    this.dest_sourceSettings = {
      singleSelection: false,
      idField: 'OrganizationID',
      textField: 'OrganizationUnits',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: false
    };
    // if (this.letter_type_id != '') {
    //   this.getservice();
    // }
    //this.onChangeTable(this.config);
  }
  openReport() {
    this.bsModalRef = this.modalService.show(LetterReportModalComponent, { class: 'modal-lg' });
  }

  onSelect({ selected }) {
    let checked_data = [];
    this.bulk_data = [];
    console.log('Select Event', selected);
    checked_data = selected;
    checked_data.forEach(data => {
      this.bulk_data.push({
        "LetterID": data.LetterID
      });
    });
  }
  public bulk_approve(type) {
    if (!this.bulkApproveEnable) {
      this.bulkApproveEnable = true;
      var approvedMsg = (this.commonservice.language == 'English') ? "Letter Approved Successfully." : this.arabic('letterapprovedsuccessfully');
      let user = localStorage.getItem("User");
      let userdet = JSON.parse(user);
      let userID = userdet.id;
      let requestData = {
        LettersID: this.bulk_data
      }
      let approveRequestData = {
        LettersID: this.bulk_data,
        ActionBy: userID,
        ActionDateTime: new Date()
      }
      if (this.bulk_data) {
        if (type == 'approve') {
          this.letterlistservice.BulkApproval('OutboundLetter/BulkApproval', userID, approveRequestData).subscribe(data => {
            console.log(data);
            this.message = approvedMsg;
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            this.getservice();
            //location.reload();
            this.bulkApproveEnable = false;
          });
        } else {
          this.letterlistservice.BulkApprovals('OutboundLetter/DeliveryNote', userID, requestData);
        }
      } else {
      }
    }

  }
  //mobile view checkbox
  public bulk_approves(type) {
    if (!this.bulkApproveEnable) {
      this.bulkApproveEnable = true;
      var approvedMsg = (this.commonservice.language == 'English') ? "Letter Approved Successfully." : this.arabic('letterapprovedsuccessfully');
      let checked_data = this.rows;
      this.bulk_data = [];
      checked_data.forEach(data => {
        if (data.check_box == true) {
          this.bulk_data.push({
            "LetterID": data.LetterID
          });
        }
      });
      let user = localStorage.getItem("User");
      let userdet = JSON.parse(user);
      let userID = userdet.id;
      let requestData = {
        LettersID: this.bulk_data
      }
      let approveRequestData = {
        LettersID: this.bulk_data,
        ActionBy: userID,
        ActionDateTime: new Date()
      }
      if (this.bulk_data.length > 0) {
        if (type == 'approve') {
          this.letterlistservice.BulkApproval('OutboundLetter/BulkApproval', userID, approveRequestData).subscribe(data => {
            console.log(data);
            this.message = approvedMsg;
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            //location.reload();
            this.bulkApproveEnable = false;
          });
        } else {
          this.letterlistservice.BulkApprovals('OutboundLetter/DeliveryNote', userID, requestData);
        }
      } else {

      }
    }

  }
  public onChangePage(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
    this.page = page;
    if (this.filter) {
      this.changeList();
    } else {
      this.getservice();
    }
  }
  public changePage(page: any, data: Array<any> = this.data): Array<any> {

    page = (page.page) ? page.page : page;

    this.itemsPerPage = (page.itemsPerPage) ? page.itemsPerPage : this.itemsPerPage;

    let start = (page - 1) * this.itemsPerPage;

    let end = this.itemsPerPage > -1 ? (start + this.itemsPerPage) : this.length;

    return data;

  }

  public changeSort(data: any, config: any): any {
    if (!config.sorting) {
      return data;
    }

    let columns = this.config.sorting.columns || [];
    let columnName: string = void 0;
    let sort: string = void 0;

    for (let i = 0; i < columns.length; i++) {
      if (columns[i].sort !== '' && columns[i].sort !== false) {
        columnName = columns[i].name;
        sort = columns[i].sort;
      }
    }

    if (!columnName) {
      return data;
    }

    // simple sorting
    return data.sort((previous: any, current: any) => {
      if (previous[columnName] > current[columnName]) {
        return sort === 'desc' ? -1 : 1;
      } else if (previous[columnName] < current[columnName]) {
        return sort === 'asc' ? -1 : 1;
      }
      return 0;
    });
  }

  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (event.srcElement.className == "edits-btn") {
      let element: HTMLElement = document.getElementsByName('edit')[0];
      element.click();
    }
    if (event.srcElement.className == "lists-btn") {
      let element: HTMLElement = document.getElementsByName('view')[0];
      element.click();
    }
  }

  viewData(type, value) {
    var param = {
      type: 'view',
      pageInfo: {
        type: 'letter',
        id: value.LetterID,
        title: this.letter_type
      }
    };
    this.commonservice.action(param);
    if (this.commonservice.language == 'English') {
      if (type == 'edit' && value.LetterType == 'Inbound Letter') {
        this.router.navigate(['en/app/letter/incomingletter-edit/' + value.LetterID]);
      } else if (type == 'edit' && value.LetterType == 'Outbound Letter') {
        this.router.navigate(['en/app/letter/outgoingletter-edit/' + value.LetterID]);
      } else if (type != 'edit' && value.LetterType == 'Inbound Letter') {
        this.router.navigate(['en/app/letter/incomingletter-view/' + value.LetterID]);
      } else if (type != 'edit' && value.LetterType == 'Outbound Letter') {
        this.router.navigate(['en/app/letter/outgoingletter-view/' + value.LetterID]);
      }
    } else {
      if (type == 'edit' && value.LetterType == 'Inbound Letter') {
        this.router.navigate(['ar/app/letter/incomingletter-edit/' + value.LetterID]);
      } else if (type == 'edit' && value.LetterType == 'Outbound Letter') {
        this.router.navigate(['ar/app/letter/outgoingletter-edit/' + value.LetterID]);
      } else if (type != 'edit' && value.LetterType == 'Inbound Letter') {
        this.router.navigate(['ar/app/letter/incomingletter-view/' + value.LetterID]);
      } else if (type != 'edit' && value.LetterType == 'Outbound Letter') {
        this.router.navigate(['ar/app/letter/outgoingletter-view/' + value.LetterID]);
      }
    }
    //this.router.navigate(['pages/letter/outgoingletter-view/'+value.LetterID]);
  }

  onCellClick(event) {
    this.letter_id = event.row.MemoID;
  }

  public changeFilter(data: any, config: any): any {
    let filteredData: Array<any> = data;
    this.columns.forEach((column: any) => {
      if (column.filtering) {
        filteredData = filteredData.filter((item: any) => {
          return item[column.name].match(column.filtering.filterString);
        });
      }
    });

    if (!config.filtering) {
      return filteredData;
    }

    if (config.filtering.columnName) {
      return filteredData.filter((item: any) =>
        item[config.filtering.columnName].match(this.config.filtering.filterString));
    }

    let tempArray: Array<any> = [];
    filteredData.forEach((item: any) => {
      let flag = true;
      this.columns.forEach((column: any) => {
        if ((item[column.name] + '').match(this.config.filtering.filterString)) {
          flag = true;
        }
      });
      if (flag) {
        tempArray.push(item);
      }
    });
    filteredData = tempArray;

    return filteredData;
  }

  public onChangeTable(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
    if (config.filtering) {
      Object.assign(this.config.filtering, config.filtering);
    }

    if (config.sorting) {
      Object.assign(this.config.sorting, config.sorting);
    }

    let filteredData = this.changeFilter(this.data, this.config);
    let sortedData = this.changeSort(filteredData, this.config);
    this.rows = page && config.paging ? this.changePage(page, sortedData) : sortedData;
    //this.length = this.length;
  }
  arabic(word) {
    return this.commonservice.arabic.words[word];
  }

  duedateValidation() {
    this.validateDueStartEndDate.msg = this.commonservice.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
    let showDueDateValidationMsg = false;
    if (!this.filter_data.date_from && this.filter_data.date_to) {
      showDueDateValidationMsg = false;
    } else if (this.filter_data.date_from && this.filter_data.date_to) {
      let startDate = new Date(this.filter_data.date_from).getTime();
      let endDate = new Date(this.filter_data.date_to).getTime();
      if (endDate < startDate) {
        showDueDateValidationMsg = true;
      } else {
        showDueDateValidationMsg = false;
      }
    } else {
      showDueDateValidationMsg = false;
    }
    return showDueDateValidationMsg;
  }

  minDate(date) {
    return this.util.minDateCheck(date);
  }
  maxDate(date) {
    return this.util.maxDateCheck(date);
  }
  allowSelect(row: any) {
    var data = JSON.parse(localStorage.getItem("User")).OrgUnitID;
    if (data == 14) {
      return true;
    } else if (row.LetterType != "Inbound Letter" && (row.StatusCode == '19' || row.StatusCode == '25')) {
      return true;
    } else {
      return false;
    }
  }


  ngOnDestroy() {
    this.sideSubscribe.unsubscribe();
  }
}
