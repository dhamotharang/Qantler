import { Component, OnInit, ViewChild, HostListener, TemplateRef, OnDestroy } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { MemoListService } from '../../../services/memolist.service';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { ReportModalComponent } from '../../../../modal/reportModal/reportModal.component';
import { BsModalRef, BsModalService, BsDatepickerConfig, TabHeadingDirective } from 'ngx-bootstrap';
import { MemoService } from '../../../services/memo.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-memo-list',
  templateUrl: './memo-list.component.html',
  styleUrls: ['./memo-list.component.scss']
})

export class MemoListComponent implements OnInit, OnDestroy {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  public memo_type: any = 'Pending Memo List';
  public memo_type_id: any = 1;
  public user_name: any = 'TestUser 1';
  currentUserDetails: any = JSON.parse(localStorage.getItem('User'));
  filter_data: any = [];
  filter = false;
  status = (this.commonservice.language == 'عربي') ? this.arabic('all') : 'All';
  source = (this.commonservice.language == 'عربي') ? this.arabic('all') : 'All';
  destination = (this.commonservice.language == 'عربي') ? this.arabic('all') : 'All';
  date_from = '';
  date_to = '';
  private = 'All';
  priority = 'All';
  smartSearch = '';
  statusOptions = [];
  sourceouOptions = [];
  destinationOptions = [];
  privateOptions = ['All', 'Yes', 'No'];
  priorityOptions = ['All', 'High', 'Medium', 'Low', 'VeryLow'];
  dest_sourceSettings: { singleSelection: boolean; idField: string; textField: string; selectAllText: string; unSelectAllText: string; itemsShowLimit: number; allowSearchFilter: boolean; };
  bsModalRef: BsModalRef;
  progress = false;
  public rows: Array<any> = [];
  public columns: Array<any> = [];
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
  statusDisable = false;


  private data: any;
  memo_id: any;
  currentStatus: string = (this.commonservice.language == 'عربي') ? this.arabic('all') : 'All';
  sideSubscribe: any;
  sideNavTrigger: boolean = false;
  currentUser: any;
  high: string;
  medium: string;
  low: string;
  verylow: string;
  draft: string;
  waitingforapproval: string;
  rejected: string;
  underprogress: string;
  pendingforresubmit: string;
  closed: any;
  tableMessages: any = {
    emptyMessage: ''
  };
  constructor(public commonservice: CommonService, public router: Router,
    private memolistservice: MemoListService, public memoservice: MemoService, private modalService: BsModalService, public arabicService: ArabicDataService, public datePipe: DatePipe, public utill: UtilsService) {
    this.commonservice.sideNavResponse('memo', this.memo_type);
    if (this.commonservice.language == 'عربي') {
      this.source = this.arabic('all');
      this.destination = this.arabic('all');
      this.private = this.arabic('all');
      this.priority = this.arabic('all');
      //this.report.priority = this.arabic('all');
    }
    this.sideSubscribe = this.commonservice.sideNavMemoChanged$.subscribe(data => {
      if (data) {
        this.page = 1;
        console.log('subscribe side nav');
        this.sideNavTrigger = true;
        if (data.type == 'memo') {
          this.rows = [];
          this.resetFilter();
          // this.getComboList();
          this.memo_type = data.title;
          this.commonservice.sideNavResponse('memo', this.memo_type);
          if (this.memo_type == 'Pending Memo List') {
            this.memo_type_id = 1;
            this.currentStatus = (this.commonservice.language == 'عربي') ? this.arabic('all') : 'All';
            this.statusDisable = false;
            if (this.commonservice.language == 'عربي') {
              this.source = this.arabic('all');
              this.destination = this.arabic('all');
              this.private = this.arabic('all');
              this.priority = this.arabic('all');
              //this.report.priority = this.arabic('all');
            } else {
              this.source = 'All';
              this.destination = 'All';
              this.private = 'All';
              this.priority = 'All';
            }
          } else if (this.memo_type == 'Incoming Memos') {
            this.memo_type_id = 3;
            this.currentStatus = (this.commonservice.language == 'عربي') ? this.arabic('all') : 'All';
            this.statusDisable = false;
            if (this.commonservice.language == 'عربي') {
              this.status = this.arabic('all');
              this.source = this.arabic('all');
              this.destination = this.arabic('all');
              this.private = this.arabic('all');
              this.priority = this.arabic('all');
              //this.report.priority = this.arabic('all');
            } else {
              this.status = 'All';
              this.source = 'All';
              this.destination = 'All';
              this.private = 'All';
              this.priority = 'All';
            }
          } else if (this.memo_type == 'Outgoing Memos') {
            this.memo_type_id = 2;
            this.currentStatus = (this.commonservice.language == 'عربي') ? this.arabic('all') : 'All';
            this.statusDisable = false;
            if (this.commonservice.language == 'عربي') {
              this.status = this.arabic('all');
              this.source = this.arabic('all');
              this.destination = this.arabic('all');
              this.private = this.arabic('all');
              this.priority = this.arabic('all');
              //this.report.priority = this.arabic('all');
            } else {
              this.status = 'All';
              this.source = 'All';
              this.destination = 'All';
              this.private = 'All';
              this.priority = 'All';
            }
          } else if (this.memo_type == 'Draft Memos') {
            this.memo_type_id = 4;
            this.currentStatus = (this.commonservice.language == 'عربي') ? this.arabic('draft') : 'Draft';
            //this.currentStatus = 'Draft';
            this.statusDisable = true;
            if (this.commonservice.language == 'عربي') {
              this.status = this.arabic('draft');
              this.source = this.arabic('all');
              this.destination = this.arabic('all');
              this.private = this.arabic('all');
              this.priority = this.arabic('all');
              //this.report.priority = this.arabic('all');
            } else {
              this.status = 'Draft';
              this.source = 'All';
              this.destination = 'All';
              this.private = 'All';
              this.priority = 'All';
            }

          } else if (this.memo_type == 'Historical Memos Incoming') {
            this.memo_type_id = 5;
            this.statusDisable = false;
          } else if (this.memo_type == 'Historical Memos Outgoing') {
            this.memo_type_id = 6;
            this.statusDisable = false;
          }
          if (this.memo_type_id != '') {
            this.getservice();
            this.status = this.currentStatus;
          }
          if (this.commonservice.language == 'English')
            this.memoservice.breadscrumChange(this.memo_type, 'List', 0);
          else {
            this.memoservice.breadscrumChange(this.memo_type, 'List', 0, 'ar');
          }
        }
      } else {
        console.log("Subscribe is not called");
      }
    });




  }
  public getComboList() {
    let user_id = '1';
    let memo_id = '0';
    this.memolistservice.memoCombos("memo/", memo_id, user_id).subscribe((res: any) => {
      // this.statusOptions = res.M_LookupsList;
      this.status = this.currentStatus;
      var AllStatusList = res.M_LookupsList;
      let statusids: any = [];
      if (this.commonservice.language == "English") {
        statusids.push({ DisplayName: "All", LookupsID: 0, DisplayOrder: 0 });
      } else if (this.commonservice.language != "English") {
        statusids.push({ DisplayName: this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()], LookupsID: 0, DisplayOrder: 0 });
      }
      AllStatusList.forEach((item) => {
        statusids.push({ DisplayName: item.DisplayName, LookupsID: item.LookupsID, DisplayOrder: item.DisplayOrder });
      });
      this.statusOptions = statusids;
      var calendar_id = environment.calendar_id;
      this.sourceouOptions = res.OrganizationList;
      this.destinationOptions = res.OrganizationList.filter(ret => calendar_id != ret.OrganizationID);
      if (this.memo_type == 'Pending Memo List') {
        this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID == 5) || (person.LookupsID == 2));
      }
      else if (this.memo_type == 'Outgoing Memos') {
        this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID != 1));
      }
      else if (this.memo_type == 'Incoming Memos') {
        this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID == 3) || (person.LookupsID == 6));
      }
    });
  }

  resetFilter() {
    this.status = '';
    this.source = '';
    this.destination = '';
    this.date_from = '';
    this.date_to = '';
    this.private = '';
    this.priority = '';
    this.smartSearch = '';
  }

  public changeList() {
    this.progress = true;
    this.filter_data = this;
    this.filter = true;
    let status = '';
    let source = '';
    let destination = '';
    let from_date = '';
    let to_date = '';
    let Private = '';
    let Priority = '';
    this.filter_data.smartSearch = this.smartSearch;
    if (this.filter_data.date_from) {
      from_date = new Date(this.filter_data.date_from).toJSON();
    }
    if (this.filter_data.date_to) {
      to_date = new Date(this.filter_data.date_to).toJSON();
    }
    // if (this.filter_data.source) {
    //   source = (this.filter_data.source == 'All') ? '' : this.filter_data.source;
    // }
    if (this.filter_data.source == null || this.filter_data.source == 'All' || this.filter_data.source == this.arabic('all')) {
      source = '';
    } else {
      source = this.filter_data.source;
    }
    source = source.replace("&", "amp;");
    // if (this.filter_data.destination) {
    //   destination = (this.filter_data.destination == 'All') ? '' : this.filter_data.destination;
    // }
    if (this.filter_data.destination == null || this.filter_data.destination == 'All' || this.filter_data.destination == this.arabic('all')) {
      destination = '';
    } else {
      destination = this.filter_data.destination;
    }
    destination = destination.replace("&", "amp;");

    if (this.filter_data.priority == this.arabic('all') || this.filter_data.priority == 'All') {
      Priority = '';
    } else {
      Priority = this.filter_data.priority;
    }
    if (this.filter_data.private == this.arabic('all') || this.filter_data.private == 'All') {
      Private = '';
    } else {
      Private = this.filter_data.private;
    }

    if (this.filter_data.status != null) {
      status = this.filter_data.status.trim();
    } else { status = this.filter_data.status; }

    if (this.filter_data.status == null || this.filter_data.status == 'All' || this.filter_data.status == this.arabic('all')) {
      status = '';
    }
    if (this.filter_data.priority == null) {
      Priority = '';
    }
    if (this.filter_data.private == null) {
      Private = '';
    }
    if (this.filter_data.status == 'مسودة') {
      status = 'Draft';
    }
    // if(this.filter_data.status == 'تحت الاجراء'){
    //   status = 'Under Progress';
    // }else{
    //   status = this.filter_data.status;
    // }

    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let username = userdet.username;
    let filterData = {
      "status": status,
      "sourceOU": source,
      "destinationOU": destination,
      "dateRangeForm": this.filter_data.date_from,
      "dateRangeTo": this.filter_data.date_to,
      "private": Private,
      "priority": Priority,
      "smartSearch": this.filter_data.smartSearch
    };
    this.memolistservice.memoFilterList("memos/", this.page, this.maxSize, this.memo_type_id, this.currentUserDetails.id, status, source, destination, Private, Priority, from_date, to_date, this.filter_data.smartSearch)
      .subscribe((res: any) => {
        this.data = res.collection;
        this.progress = false;
        if (this.data) {
          this.length = res.count;
          //this.maxSize = this.data.count
        }

        var click = "(click)='editClick()'";
        this.data.forEach(val => {
          if (val['Priority'] == this.high) {
            val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == this.medium) {
            val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == this.low) {
            val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          if (val['Priority'] == this.verylow) {
            val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
          }
          val.CreatedDateTime = this.datePipe.transform(val.CreatedDateTime, "dd/MM/yyyy");
        });
        // this.statusOptions = res.lookupsList;
        // this.status = this.currentStatus;
        // this.sourceouOptions = res.organizationList;
        // this.destinationOptions = res.organizationList;
        // this.source = 'All';
        // this.destination = 'All';
        // this.private = 'All';
        // this.priority = 'All';
        if (this.memo_type == 'Pending Memo List') {
          this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID == 5) || (person.LookupsID == 2) || (person.LookupsID == 3));
        }
        else if (this.memo_type == 'Outgoing Memos') {
          this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID != 1));
        }
        else if (this.memo_type == 'Incoming Memos') {
          this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID == 3) || (person.LookupsID == 6));
        }
        this.onChangeTable(this.config);

      });
  }
  public getservice() {
    this.progress = true;
    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let username = userdet.username;
    this.currentUser = username
    this.memolistservice.memoList("memos/", this.page, this.maxSize, this.memo_type_id, this.currentUserDetails.id).subscribe((res: any) => {
      this.data = res.collection;
      if (this.data) {
        this.length = res.count;
        //this.maxSize = this.data.count
      }
      var click = "(click)='editClick()'";
      this.data.forEach(val => {
        // var high = 'High',
        //   medium = 'Medium',
        //   low = 'Low',
        //   verylow = 'VeryLow';
        // if (this.commonservice.language == 'Arabic') {
        //   high = this.commonservice.arabic.words['high'];
        //   medium = this.commonservice.arabic.words['medium'];
        //   low = this.commonservice.arabic.words['low'];
        //   verylow = this.commonservice.arabic.words['verylow'];
        // }
        if (val['Priority'] == this.high) {
          val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
        }
        if (val['Priority'] == this.medium) {
          val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
        }
        if (val['Priority'] == this.low) {
          val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
        }
        if (val['Priority'] == this.verylow) {
          val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
        }
        val.CreatedDateTime = this.datePipe.transform(val.CreatedDateTime, "dd/MM/yyyy");
      });
      // this.statusOptions = res.lookupsList;
      var AllStatusList = res.lookupsList;
      let statusids: any = [];
      if (this.commonservice.language == "English") {
        statusids.push({ DisplayName: "All", LookupsID: 0, DisplayOrder: 0 });
      } else if (this.commonservice.language != "English") {
        statusids.push({ DisplayName: this.arabicService.words["All".trim().replace(/\s+/g, '').toLowerCase()], LookupsID: 0, DisplayOrder: 0 });
      }
      AllStatusList.forEach((item) => {
        statusids.push({ DisplayName: item.DisplayName, LookupsID: item.LookupsID, DisplayOrder: item.DisplayOrder });
      });
      this.statusOptions = statusids;
      this.status = this.currentStatus;
      var calendar_id = environment.calendar_id;
      this.sourceouOptions = res.organizationList;
      this.destinationOptions = res.organizationList.filter(ret => calendar_id != ret.OrganizationID);
      // this.source = this.source;
      // this.destination = 'All';
      // this.private = 'All';
      // this.priority = 'All';
      if (this.memo_type == 'Pending Memo List') {
        this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID == 5) || (person.LookupsID == 2) || (person.LookupsID == 3));
      }
      else if (this.memo_type == 'Outgoing Memos') {
        this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID != 1));
      }
      else if (this.memo_type == 'Incoming Memos') {
        this.statusOptions = this.statusOptions.filter(person => (person.LookupsID == 0) || (person.LookupsID == 3) || (person.LookupsID == 6));
      }
      this.onChangeTable(this.config);
      this.progress = false;
    });
  }
  public ngOnInit(): void {
    //this.getComboList();
    //this.getservice();
    this.tableMessages = {
      emptyMessage: (this.commonservice.language == 'English') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
    this.columns = [
      { name: 'Ref ID', prop: 'ReferenceNumber' },
      { name: 'Title', prop: 'Title' },
      { name: 'Source', prop: 'SourceOU' },
      { name: 'Destination', prop: 'Destination' },
      { name: 'Status', prop: 'Status' },
      { name: 'Date', prop: 'CreatedDateTime' },
      { name: 'Is Private?', prop: 'Private' },
      { name: 'Priority', prop: 'NewPriority' },
      { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
    ];
    this.high = 'High';
    this.medium = 'Medium';
    this.low = 'Low';
    this.verylow = 'VeryLow';
    this.draft = 'Draft';
    this.waitingforapproval = 'Waiting for Approval';
    this.rejected = 'Rejected';
    this.underprogress = 'Under Progress';
    this.pendingforresubmit = 'Pending for Resubmission';

    if (this.commonservice.language != 'English') {
      this.privateOptions = [this.arabic('all'), this.arabic('yes'), this.arabic('no')];
      this.priorityOptions = [this.arabic('all'), this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
      this.columns = [
        { name: this.arabic('refid'), prop: 'ReferenceNumber' },
        { name: this.arabic('title'), prop: 'Title' },
        { name: this.arabic('sourceou'), prop: 'SourceOU' },
        { name: this.arabic('destination'), prop: 'Destination' },
        { name: this.arabic('status'), prop: 'Status' },
        { name: this.arabic('date'), prop: 'CreatedDateTime' },
        { name: this.arabic('private'), prop: 'Private' },
        { name: this.arabic('priority'), prop: 'NewPriority' },
        { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
      ];
      this.high = this.arabic('high');
      this.medium = this.arabic('medium');
      this.low = this.arabic('low');
      this.verylow = this.arabic('verylow');
      this.draft = this.arabic('draft');
      this.waitingforapproval = this.arabic('waitingforapproval');
      this.rejected = this.arabic('rejected');
      this.underprogress = this.arabic('underprogress');
      this.pendingforresubmit = this.arabic('pendingforresubmission');
    }


    if (this.commonservice.language == 'English') {
      this.memoservice.breadscrumChange(this.memo_type, 'List', 0);
      this.commonservice.topBanner(true, this.memo_type, '+ CREATE MEMO', '/en/app/memo/memo-create');
    }
    else {
      this.memoservice.breadscrumChange(this.memo_type, 'List', 0, 'ar');
      this.commonservice.topBanner(true, this.sideNavArabic(this.memo_type), '+ ' + this.arabic('memocreate'), '/ar/app/memo/memo-create');
    }

  }

  async ngAfterViewInit() {
    if (!this.sideNavTrigger) {
      await this.getservice();
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

  async viewData(type, value: any = '') {
    var param = {
      type: 'view',
      pageInfo: {
        type: 'memo',
        id: value.MemoID,
        title: this.memo_type
      }
    };
    var typeParam = {
      typeID: this.memo_type_id,
      memoID: value.MemoID
    }
    var BCParam = {
      typeID: this.memo_type_id,
      RequestID: value.MemoID
    }
    var path = (this.commonservice.language == 'English') ? 'en' : 'ar';
    await this.commonservice.action(param);
    await this.commonservice.viewMemo(typeParam);
    if (type == 'edit') {
      this.router.navigate([path + '/app/memo/memo-edit/' + value.MemoID]);
    } else {
      this.router.navigate([path + '/app/memo/memo-view/' + value.MemoID]);
    }
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
  }

  openReport() {
    this.bsModalRef = this.modalService.show(ReportModalComponent, { class: 'modal-lg' });
  }

  arabic(word) {
    return this.commonservice.arabic.words[word];
  }
  sideNavArabic(word) {
    return this.commonservice.sideNaveTypeArabic(word);
  }

  ngOnDestroy() {
    this.sideSubscribe.unsubscribe();
  }
}
