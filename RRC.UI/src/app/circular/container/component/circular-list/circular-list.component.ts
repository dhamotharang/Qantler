import { Component, OnInit, ViewChild, HostListener, TemplateRef, OnDestroy } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { MemoListService } from '../../../../memo/services/memolist.service';
import { CircularService } from '../../../service/circular.service';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { CircularReportModalComponent } from '../../../../modal/circular-report-modal/circular-report-modal.component';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-circular-list',
  templateUrl: './circular-list.component.html',
  styleUrls: ['./circular-list.component.scss']
})
export class CircularListComponent implements OnInit, OnDestroy {
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  bsModalRef: BsModalRef;
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  public memo_type: any = 'My Pending Circulars';
  public memo_type_id: any = 1;
  public user_name: any = 'TestUser 1';
  circular: any = {};
  filter_data: any = [];
  currentUserDetails: any = JSON.parse(localStorage.getItem('User'));
  progress = false;
  filter = false;
  status = 'All';
  source = 'All';
  destination = 'All';
  date_from = '';
  date_to = '';
  private = '';
  priority = 'All';
  smartSearch = '';
  statusOptions = [];
  sourceouOptions = [];
  destinationOptions = [];
  privateOptions = ['Yes', 'No'];
  priorityOptions = ['All', 'High', 'Medium', 'Low', 'VeryLow'];
  dest_sourceSettings: { singleSelection: boolean; idField: string; textField: string; selectAllText: string; unSelectAllText: string; itemsShowLimit: number; allowSearchFilter: boolean; };
  tableMessages:any = {
    emptyMessage:''
  };
  public rows: Array<any> = [//{
    //   "Title" : 'Sample',
    //   "SourceOU" : 'asfdf',
    //   "Destination" : 'fdasf',
    //   "Status" : 'dfadfs',
    //   "CreatedDateTime" : 'fdsfsf',
    //   "NewPriority" : 'dafsd'
    // }
  ];
  public columns: Array<any> = [
    // {title: 'Title',className: 'bg-color', name: 'Title'},
    // {title: 'Source',className: 'bg-color', name: 'SourceOU'},
    // {title: 'Destination',className: 'bg-color', name: 'Destination'},
    // {title: 'Status',className: 'bg-color', name: 'Status'},
    // {title: 'Date',className: 'bg-color', name: 'CreatedDateTime'},
    // {title: 'Priority',className: 'bg-color', name: 'NewPriority'},
    // {title: 'Action',className: 'bg-color', name: 'action'}
  ];
  public page: number = 1;
  public itemsPerPage: number = 10;
  public maxSize: number = 10;
  public numPages: number = 1;
  public length: number = 0;
  public alreadyExist: boolean = false;

  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  statusConfig = {
    displayKey: "DisplayName", //if objects array passed which key to be displayed defaults to description
    search: true,
    limitTo: 3
  };

  sourceConfig = {
    displayKey: "OrganizationUnits", //if objects array passed which key to be displayed defaults to description
    search: true,
    limitTo: 3
  };

  private data: any;
  memo_id: any;
  statusDisable: boolean;
  currentStatus: string;
  sideSubscribe: any;
  currentUser: any;
  // @ViewChild("editService") newEditService;
  // @ViewChild("deleteService") newDeleteService;
  // editDataId: any;


  // public constructor(private  router: Router,private modalService: NgbModal,private requestAPI:ApplicableshipapiService,public datePipe:DatePipe) {
  //   //this.length = this.data.length;
  //   this.getservice();
  // }
  constructor(public commonservice: CommonService, public router: Router, private memolistservice: MemoListService, public datePipe: DatePipe,
    public circularservice: CircularService, public modalService: BsModalService,public utill:UtilsService,public arabicService:ArabicDataService) {
    // this.bsConfig = {
    //   dateInputFormat: 'DD/MM/YYYY'
    // }
    this.resetFilter();
    this.sideSubscribe = this.commonservice.sideNavChanged$.subscribe(data => {
      this.memo_type = data.title;
      if (data.type == 'circular') {
        this.rows = [];
        this.resetFilter();
        this.commonservice.sideNavResponse('circular', this.memo_type);
        if (this.memo_type == 'My Pending Circulars') {
          this.memo_type_id = 1;
          this.currentStatus = (this.commonservice.language == 'English') ? 'All': this.arabic('all');
          this.statusDisable = false;
        } else if (this.memo_type == 'Incoming Circulars') {
          this.memo_type_id = 3;
          this.currentStatus = (this.commonservice.language == 'English') ? 'Approved': this.arabic('CircularApproved');
          this.statusDisable = true;
        } else if (this.memo_type == 'Outgoing Circulars') {
          this.currentStatus = (this.commonservice.language == 'English') ? 'All': this.arabic('all');
          this.memo_type_id = 2;
          this.statusDisable = false;
        } else if (this.memo_type == 'Draft Circulars') {
          this.memo_type_id = 4;
          this.currentStatus = (this.commonservice.language == 'English') ? 'Draft': this.arabic('draft');
          this.statusDisable = true;
        } else if (this.memo_type == 'Historical Circulars Incoming') {
          this.memo_type_id = 5;
          this.statusDisable = false;
        } else if (this.memo_type == 'Historical Circulars Outgoing') {
          this.memo_type_id = 6;
          this.statusDisable = false;
        }
        if (this.memo_type_id != '') {
          this.status = this.currentStatus;
          this.getservice();

        }
        if (this.commonservice.language == 'English')
          this.circularservice.breadscrumChange(this.memo_type, 'List', 0);
        else
          this.circularservice.breadscrumChange(this.memo_type, 'List', 0, 'ar');
      }

      //this.commonservice.topBanner(true, this.memo_type, '+ CREATE CIRCULAR', 'circular/circular-create');
    });

    if(this.commonservice.language != 'English'){
      this.status = this.arabic('all');
      this.source = this.arabic('all');
      this.destination = this.arabic('all');
      this.priority = this.arabic('all');
    }else{
      this.status = 'All';
      this.source = 'All';
      this.destination = 'All';
      this.priority = 'All';
    }

    if (this.commonservice.language == 'English')
      this.circularservice.breadscrumChange(this.memo_type, 'List', 0);
    else
      this.circularservice.breadscrumChange(this.memo_type, 'List', 0, 'ar');


  }
  // public getComboList() {
  //   let user_id = '1';
  //   let memo_id = '0';
  //   this.memolistservice.memoCombos("memo/", memo_id, user_id).subscribe((res: any) => {
  //     this.statusOptions = res.M_LookupsList;
  //     this.status = this.currentStatus;
  //     this.sourceouOptions = res.OrganizationList;
  //     this.destinationOptions = res.OrganizationList;
  //   });
  // }
  resetFilter() {
    // this.status = '';
    // this.source = '';
    // this.destination = '';
    this.date_from = '';
    this.date_to = '';
    // this.priority = '';
    this.smartSearch = '';

    if (this.commonservice.language != 'English') {
      this.source = this.arabic('all');
      this.destination = this.arabic('all');
      this.priority = this.arabic('all');
      //this.report.priority = this.arabic('all');
    }else{
      this.source = 'All';
      this.destination = 'All';
      this.priority = 'All';
    }
  }

  public changeList() {
    this.filter_data = this;
    this.filter = true;
    let source = '';
    let destination = '';
    let from_date = '';
    let to_date = '';
    let priority = '';
    this.filter_data.smartSearch = this.smartSearch;
    if (this.filter_data.date_from) {
      from_date = new Date(this.filter_data.date_from).toJSON();
    }
    if (this.filter_data.date_to) {
      to_date = new Date(this.filter_data.date_to).toJSON();
    }
    if (this.filter_data.source) {
      source = (this.filter_data.source == 'All' || this.filter_data.source == this.arabic('all')) ? '' : this.filter_data.source;
    }
    source = source.replace("&", "amp;");
    if (this.filter_data.destination) {
      destination = (this.filter_data.destination == 'All' || this.filter_data.destination == this.arabic('all')) ? '' : this.filter_data.destination;
    }
    destination = destination.replace("&", "amp;");
    if(this.filter_data.priority == 'All' || this.filter_data.priority == this.arabic('all') || this.filter_data.priority == null) {
      priority = '';
    }else{
      priority = this.filter_data.priority;
    }
    let currStatus;
    if(this.status == "All" || this.status == this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()] || this.status == null){
      currStatus = '';
    }else{
      currStatus = this.status;
    }
    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let userid = userdet.id;
    let username = userdet.username;
    // let filterData = {
    //   "status": this.filter_data.status.LookupsID,
    //   "sourceOU": source,
    //   "destinationOU": destination,
    //   "dateRangeForm": this.filter_data.date_from,
    //   "dateRangeTo": this.filter_data.date_to,
    //   "priority": this.filter_data.priority,
    //   "smartSearch": this.filter_data.smartSearch
    // };
    debugger;
    this.circularservice.circularFilterList("Circular/", this.page, this.maxSize, this.memo_type_id, this.currentUserDetails.id, currStatus, source, destination, priority, from_date, to_date, this.filter_data.smartSearch)
      .subscribe((res: any) => {
        this.data = res.Collection;

        if (this.data) {
          this.length = res.Count;
          //this.maxSize = this.data.count
        }
        var calendar_id = environment.calendar_id;
        this.sourceouOptions = res.M_OrganizationList;
        this.destinationOptions = res.M_OrganizationList.filter(ret => calendar_id != ret.OrganizationID);
        // this.statusOptions = res.M_LookupsList;
        var AllStatusList = res.M_LookupsList;
        let statusids:any=[];
        if(this.commonservice.language == "English"){
          statusids.push({DisplayName: "All", LookupsID: 0, DisplayOrder: 0});
        }else if(this.commonservice.language != "English"){
          statusids.push({DisplayName:this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()], LookupsID: 0, DisplayOrder: 0});
        }
        AllStatusList.forEach((item)=>{
          statusids.push({DisplayName:item.DisplayName, LookupsID: item.LookupsID, DisplayOrder: item.DisplayOrder});
        });
        this.statusOptions = statusids;
        // this.priority = 'All';
        // this.source = 'All';
        // this.destination = 'All';
        // this.status = 'All';
       
          if (this.memo_type == 'Outgoing Circulars') {
            this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 12 && person.LookupsID != 17);
          }
          if (this.memo_type == 'My Pending Circulars') {
            this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 12 && person.LookupsID != 17 && person.LookupsID != 15 && person.LookupsID != 14);
          }
        

        var click = "(click)='editClick()'";
        this.data.forEach(val => {
          // val['checkbox']=`<td header="'headerCheckbox.html'"><input type="checkbox" ng-model="demo.checkboxes.items[row.id]" /></td>` ;
          if (val['Priority'] == 'High' || val['Priority'] == 'فائق الأهمية') {
            val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == 'Medium' || val['Priority'] == 'متوسط الأهمية') {
            val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == 'Low' || val['Priority'] == 'منخفض الأهمية') {
            val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == 'VeryLow' || val['Priority'] == 'غير مهم') {
            val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          val.Date = this.datePipe.transform(val.CreatedDateTime, "dd/MM/yyyy");
        });
        this.onChangeTable(this.config);

      });
  }
  public getservice() {
    debugger
    this.progress = true;
    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let userid = userdet.id;
    let username = userdet.username;
    this.currentUser = username;
    this.circularservice.circularList("Circular/", this.page, this.maxSize, this.memo_type_id, this.currentUserDetails.id)
      .subscribe((res: any) => {
        this.progress = false;
        this.data = res.Collection;
        if (this.data) {
          this.length = res.Count;
          //this.maxSize = this.data.count
        }
        var calendar_id = environment.calendar_id;
        this.sourceouOptions = res.M_OrganizationList;
        this.destinationOptions = res.M_OrganizationList.filter(ret => calendar_id != ret.OrganizationID);
        // this.statusOptions = res.M_LookupsList;
        var AllStatusList = res.M_LookupsList;
        let statusids:any=[];
        if(this.commonservice.language == "English"){
          statusids.push({DisplayName: "All", LookupsID: 0, DisplayOrder: 0});
        }else if(this.commonservice.language != "English"){
          statusids.push({DisplayName:this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()], LookupsID: 0, DisplayOrder: 0});
        }
        AllStatusList.forEach((item)=>{
          statusids.push({DisplayName:item.DisplayName, LookupsID: item.LookupsID, DisplayOrder: item.DisplayOrder});
        });
        this.statusOptions = statusids;
        // this.priority = 'All';
        // this.source = 'All';
        // this.destination = 'All';
        // this.status = 'All';
        if (this.memo_type == 'Outgoing Circulars') {
          this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 12 && person.LookupsID != 17);
        }
        if (this.memo_type == 'My Pending Circulars') {
          this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 12 && person.LookupsID != 17 && person.LookupsID != 15 && person.LookupsID != 14);
        }
        // this.status = this.currentStatus;
        // this.sourceouOptions = res.M_OrganizationList;
        // this.destinationOptions = res.M_OrganizationList;
        //this.changePage(1,this.data);
        var click = "(click)='editClick()'";
        this.data.forEach(val => {
          // val['checkbox']=`<td header="'headerCheckbox.html'"><input type="checkbox" ng-model="demo.checkboxes.items[row.id]" /></td>` ;
          if (val['Priority'] == 'High' || val['Priority'] == 'فائق الأهمية') {
            val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == 'Medium' || val['Priority'] == 'متوسط الأهمية') {
            val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == 'Low' || val['Priority'] == 'منخفض الأهمية') {
            val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == 'VeryLow' || val['Priority'] == 'غير مهم') {
            val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          val.Date = this.datePipe.transform(val.CreatedDateTime, "dd/MM/yyyy");
        });
        this.onChangeTable(this.config);
      });

  }
  public ngOnInit(): void {
    this.commonservice.sideNavResponse('circular', this.memo_type);
    this.tableMessages = {
      emptyMessage: (this.commonservice.language=='English') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
    this.columns = [
      { name: 'Ref Id', prop: 'ReferenceNumber' },
      { name: 'Title', prop: 'Title' },
      { name: 'Source', prop: 'SourceOU' },
      { name: 'Destination', prop: 'Destination' },
      { name: 'Status', prop: 'Status' },
      { name: 'Date', prop: 'Date' },
      { name: 'Priority', prop: 'NewPriority' },
      { name: 'Action', prop: '', cellTemplate: this.actionTemplate }
    ];
    if (this.commonservice.language != 'English') {
      this.privateOptions = [this.arabic('yes'), this.arabic('no')];
      this.priorityOptions = [this.arabic('all'), this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
      this.columns = [
        { name: this.arabic('refid'), prop: 'ReferenceNumber' },
        { name: this.arabic('circulartitle'), prop: 'Title' },
        { name: this.arabic('source'), prop: 'SourceOU' },
        { name: this.arabic('destination'), prop: 'Destination' },
        { name: this.arabic('status'), prop: 'Status' },
        { name: this.arabic('date'), prop: 'Date' },
        { name: this.arabic('priority'), prop: 'NewPriority' },
        { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
      ];
    }


    // this.getComboList();
    this.dest_sourceSettings = {
      singleSelection: false,
      idField: 'OrganizationID',
      textField: 'OrganizationUnits',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: false
    };
    if (this.memo_type_id != '') {
      this.getservice();
    }
    //this.onChangeTable(this.config);
  }

  public getComboList() {
    let user_id = '1';
    let cirid = '0';
    this.circularservice.circularCombos("Circular/", cirid, user_id).subscribe((res: any) => {
      // this.statusOptions = res.M_LookupsList;
      var AllStatusList = res.M_LookupsList;
      let statusids:any=[];
      if(this.commonservice.language == "English"){
        statusids.push({DisplayName: "All", LookupsID: 0, DisplayOrder: 0});
      }else if(this.commonservice.language != "English"){
        statusids.push({DisplayName:this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()], LookupsID: 0, DisplayOrder: 0});
      }
      AllStatusList.forEach((item)=>{
        statusids.push({DisplayName:item.DisplayName, LookupsID: item.LookupsID, DisplayOrder: item.DisplayOrder});
      });
      this.statusOptions = statusids;
      this.status = this.currentStatus;
      if (this.memo_type == 'Outgoing Circulars') {
        this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 12 && person.LookupsID != 17);
      }
      if (this.memo_type == 'My Pending Circulars') {
        this.statusOptions = this.statusOptions.filter(person => person.LookupsID != 12 && person.LookupsID != 17 && person.LookupsID != 15 && person.LookupsID != 14);
      }
      var calendar_id = environment.calendar_id;
      this.sourceouOptions = res.OrganizationList;
      this.destinationOptions = res.OrganizationList.filter(ret => calendar_id != ret.OrganizationID);
    });
  }

  // dateFormat(){
  //   var filteredData = this.data;
  //   filteredData.filter((t:any) => {
  //     var unform_date = t.DateTimeofMeeting;
  //     t.DateTimeofMeeting = this.datePipe.transform(unform_date,"yyyy-MM-dd");
  //     //$scope.formattedDate =   $filter('date')($scope.currDate, "dd-MM-yyyy");
  //   });
  // }
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

  async viewData(type, value) {
    var param = {
      type: 'view',
      pageInfo: {
        type: 'circular',
        id: value.CircularID,
        title: this.memo_type
      }
    };
    var typeParam = {
      typeID: this.memo_type_id,
      circularID: value.CircularID
    }
    var BCParam = {
      typeID: this.memo_type_id,
      RequestID: value.CircularID
    }
    await this.commonservice.action(param);
    await this.commonservice.viewCircular(typeParam);
    if (type == 'edit') {
      if (this.commonservice.language == 'English') {
        this.router.navigate(['en/app/circular/Circular-edit/' + value.CircularID]);
      } else {
        this.router.navigate(['ar/app/circular/Circular-edit/' + value.CircularID]);
      }
    } else {
      if (this.commonservice.language == 'English') {
        this.router.navigate(['en/app/circular/Circular-view/' + value.CircularID]);
      } else {
        this.router.navigate(['ar/app/circular/Circular-view/' + value.CircularID]);
      }
    }
  }

  onCellClick(event) {
    this.memo_id = event.row.CircularID;
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
  openReport() {
    this.bsModalRef = this.modalService.show(CircularReportModalComponent, { class: 'modal-lg' });
  }

  arabic(word) {
    return this.commonservice.arabic.words[word];
  }

  ngOnDestroy() {
    this.sideSubscribe.unsubscribe();
  }

}
