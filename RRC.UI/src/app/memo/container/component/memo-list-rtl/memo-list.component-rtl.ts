import { Component, OnInit, ViewChild, HostListener, TemplateRef, OnDestroy } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { MemoListService } from '../../../services/memolist.service';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { ReportModalComponent } from '../../../../modal/reportModal/reportModal.component';
import { BsModalRef, BsModalService, BsDatepickerConfig, TabHeadingDirective } from 'ngx-bootstrap';
import { MemoListComponent } from '../memo-list/memo-list.component';

@Component({
  selector: 'app-memo-list-rtl',
  templateUrl: './memo-list.component-rtl.html',
  styleUrls: ['./memo-list.component-rtl.scss']
})

export class MemoListComponentRTL extends MemoListComponent {
  // @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  // bsConfig: Partial<BsDatepickerConfig>;
  // public memo_type: any = 'My Pending Actions';
  // public memo_type_id: any = 1;
  // public user_name: any = 'TestUser 1';
  // filter_data: any = [];
  // filter = false;
  // status = 'Waiting for Approval';
  // source = '';
  // destination = '';
  // date_from = '';
  // date_to = '';
  // private = '';
  // priority = '';
  // smartSearch = '';
  // statusOptions = [];
  // sourceouOptions = [];
  // destinationOptions = [];
  // privateOptions = ['نعم ', 'لا'];
  // priorityOptions = ['فائق الأهمية', 'متوسط الأهمية', 'قليل الأهمية', 'غير مهم'];
  // dest_sourceSettings: { singleSelection: boolean; idField: string; textField: string; selectAllText: string; unSelectAllText: string; itemsShowLimit: number; allowSearchFilter: boolean; };
  // bsModalRef: BsModalRef;
  // progress = false;
  // public rows: Array<any> = [];
  // //   public columns:Array<any> = [
  // //   {title: 'Ref ID',className: 'bg-color', name: 'ReferenceNumber'},
  // //   {title: 'Title',className: 'bg-color', name: 'Title'},
  // //   {title: 'Source',className: 'bg-color', name: 'SourceOU'},
  // //   {title: 'Destination',className: 'bg-color  table-120', name: 'Destination'},
  // //   {title: 'Status',className: 'bg-color', name: 'Status'},
  // //   {title: 'Date',className: 'bg-color table-120', name: 'CreatedDateTime'},
  // //   {title: 'Is Private?',className: 'bg-color table-100', name: 'Private'},
  // //   {title: 'Priority',className: 'bg-color table-100', name: 'NewPriority'},
  // //   {title: 'Action',className: 'bg-color table-120', name: 'action'}
  // // ];

  // public columns: Array<any> = [];
  // public page: number = 1;
  // public itemsPerPage: number = 10;
  // public maxSize: number = 10;
  // public numPages: number = 1;
  // public length: number = 0;
  // public alreadyExist: boolean = false;

  // public config: any = {
  //   paging: true,
  //   sorting: { columns: this.columns },
  //   filtering: { filterString: '' },
  //   className: ['table-striped', 'table-bordered', 'm-b-0']
  // };
  // statusConfig = {
  //   displayKey: "DisplayName", //if objects array passed which key to be displayed defaults to description
  //   search: true,
  //   limitTo: 3
  // };

  // sourceConfig = {
  //   displayKey: "OrganizationUnits", //if objects array passed which key to be displayed defaults to description
  //   search: true,
  //   limitTo: 3
  // };
  // statusDisable = true;


  // private data: any;
  // memo_id: any;
  // currentStatus: string = 'Waiting for Approval';
  // sideSubscribe: any;
  // constructor(public commonservice: CommonService, public router: Router,
  //   private memolistservice: MemoListService, private modalService: BsModalService, public datePipe: DatePipe) {
  //   this.commonservice.sideNavResponse('memo', this.memo_type);
  //   // this.bsConfig = {
  //   //   dateInputFormat: 'DD/MM/YYYY'
  //   // }
  //   this.sideSubscribe = this.commonservice.sideNavMemoChanged$.subscribe(data => {
  //     console.log('subscribe side nav');
  //     if (data.type == 'memo') {
  //       this.rows = [];
  //       this.resetFilter();
  //       this.getComboList();
  //       this.memo_type = data.title;
  //       this.commonservice.sideNavResponse('memo', this.memo_type);
  //       if (this.memo_type == 'My Pending Actions') {
  //         this.memo_type_id = 1;
  //         this.currentStatus = 'Waiting for Approval';
  //         this.statusDisable = true;
  //       } else if (this.memo_type == 'Incoming Memos') {
  //         this.memo_type_id = 3;
  //         this.currentStatus = 'All';
  //         this.statusDisable = false;
  //       } else if (this.memo_type == 'Outgoing Memos') {
  //         this.memo_type_id = 2;
  //         this.statusDisable = false;
  //       } else if (this.memo_type == 'Draft Memos') {
  //         this.memo_type_id = 4;
  //         this.currentStatus = 'Draft';
  //         this.statusDisable = true;
  //       } else if (this.memo_type == 'Historical Memos Incoming') {
  //         this.memo_type_id = 5;
  //         this.statusDisable = false;
  //       } else if (this.memo_type == 'Historical Memos Outgoing') {
  //         this.memo_type_id = 6;
  //         this.statusDisable = false;
  //       }
  //       if (this.memo_type_id != '') {
  //         this.getservice();
  //         this.status = this.currentStatus;
  //       }
  //       this.commonservice.breadscrumChange('المذكرات الداخلية', 'قائمة المذكرات', 'قائمة');
  //       //this.commonservice.topBanner(true, this.memo_type, '+ CREATE MEMO', 'memo/memo-create');
  //     }
  //   });
  //   this.commonservice.breadscrumChange('المذكرات الداخلية', 'قائمة المذكرات', 'قائمة');
  //   // var breadscrumData = {
  //   //   btn: "memo/memo-create",
  //   //   pageInfo: {
  //   //     menu: "memo/memo-list",
  //   //     title: "My Pending Actions",
  //   //     type: "memo"
  //   //   },
  //   //   type: "list"
  //   // }
  //   // this.commonservice.action(breadscrumData);
  // }
  // public getComboList() {
  //   let user_id = '1';
  //   let memo_id = '0';
  //   this.memolistservice.memoCombos("memo/", memo_id, user_id).subscribe((res: any) => {
  //     this.statusOptions = res.M_LookupsList;
  //     this.status = this.currentStatus;
  //     this.sourceouOptions = res.OrganizationList;
  //     this.destinationOptions = res.OrganizationList;
  //     if (this.memo_type == 'Outgoing Memos') {
  //       this.statusOptions = this.statusOptions.filter(person => person.DisplayName != 'Draft');
  //     }
  //     else if (this.memo_type == 'Incoming Memos') {
  //       this.statusOptions = this.statusOptions.filter(person => (person.DisplayName == 'Under Progress') || (person.DisplayName == 'Closed'));
  //     }
  //   });
  // }

  // resetFilter() {
  //   this.status = '';
  //   this.source = '';
  //   this.destination = '';
  //   this.date_from = '';
  //   this.date_to = '';
  //   this.private = '';
  //   this.priority = '';
  //   this.smartSearch = '';
  // }

  // public changeList() {
  //   this.progress = true;
  //   this.filter_data = this;
  //   this.filter = true;
  //   let source = '';
  //   let destination = '';
  //   let from_date = '';
  //   let to_date = '';
  //   this.filter_data.smartSearch = this.smartSearch;
  //   if (this.filter_data.date_from) {
  //     from_date = new Date(this.filter_data.date_from).toJSON();
  //   }
  //   if (this.filter_data.date_to) {
  //     to_date = new Date(this.filter_data.date_to).toJSON();
  //   }
  //   if (this.filter_data.source) {
  //     source = this.filter_data.source;
  //   }
  //   if (this.filter_data.destination) {
  //     destination = this.filter_data.destination;
  //   }
  //   if (this.filter_data.priority == null) {
  //     this.filter_data.priority = '';
  //   }

  //   let user = localStorage.getItem("User");
  //   let userdet = JSON.parse(user);
  //   let username = userdet.username;
  //   let filterData = {
  //     "status": this.filter_data.status,
  //     "sourceOU": source,
  //     "destinationOU": destination,
  //     "dateRangeForm": this.filter_data.date_from,
  //     "dateRangeTo": this.filter_data.date_to,
  //     "private": this.filter_data.private,
  //     "priority": this.filter_data.priority,
  //     "smartSearch": this.filter_data.smartSearch
  //   };
  //   this.memolistservice.memoFilterList("memos/", this.page, this.maxSize, this.memo_type_id, username, this.filter_data.status, source, destination, this.filter_data.private, this.filter_data.priority, from_date, to_date, this.filter_data.smartSearch)
  //     .subscribe((res: any) => {
  //       this.data = res.collection;
  //       this.progress = false;
  //       if (this.data) {
  //         this.length = res.count;
  //         //this.maxSize = this.data.count
  //       }

  //       var click = "(click)='editClick()'";
  //       this.data.forEach(val => {

  //         if (val['Priority'] == 'High') {
  //           val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
  //         }
  //         if (val['Priority'] == 'Medium') {
  //           val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
  //         }
  //         if (val['Priority'] == 'Low') {
  //           val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
  //         }
  //         if (val['Priority'] == 'Very low') {
  //           val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
  //         }
  //       });
  //       this.onChangeTable(this.config);

  //     });
  // }
  // public getservice() {
  //   this.progress = true;
  //   let user = localStorage.getItem("User");
  //   let userdet = JSON.parse(user);
  //   let username = userdet.username;

  //   // let res.collection =[];
  //   // let res.count ='0';
  //   this.memolistservice.memoList("memos/", this.page, this.maxSize, this.memo_type_id, username).subscribe((res: any) => {
  //     this.data = res.collection;
  //     if (this.data) {
  //       this.length = res.count;
  //       //this.maxSize = this.data.count
  //     }
  //     //     this.data =[{
  //     //     "refId": "1",
  //     //     "title": this.memo_type,
  //     //     "source": 'HR',
  //     //     "destination": "IT",
  //     //     "status": "Approved",
  //     //     "date":'02-10-2019',
  //     //     "is_private":"Yes",
  //     //     "priority":"High"

  //     // },{
  //     //     "refId": "2",
  //     //     "title": "Memorandum",
  //     //     "source": 'HR',
  //     //     "destination": "IT",
  //     //     "status": "Cancel",
  //     //     "date":'04-10-2019',
  //     //     "is_private":"No",
  //     //     "priority":"Low"

  //     // }];
  //     //this.rows = this.data;
  //     //this.length = this.data.length;
  //     //this.changePage(1,this.data);
  //     var click = "(click)='editClick()'";
  //     this.data.forEach(val => {
  //       if (val['Priority'] == 'High') {
  //         val['NewPriority'] = `<div><div class="priority-red-clr"></div><div>` + val['Priority'] + `</div></div>`;
  //       }
  //       if (val['Priority'] == 'Medium') {
  //         val['NewPriority'] = `<div><div class="priority-gl-clr"></div><div>` + val['Priority'] + `</div></div>`;
  //       }
  //       if (val['Priority'] == 'Low') {
  //         val['NewPriority'] = `<div><div class="priority-ylw-clr"></div><div>` + val['Priority'] + `</div></div>`;
  //       }
  //       if (val['Priority'] == 'Very low') {
  //         val['NewPriority'] = `<div><div class="priority-gry-clr"></div><div>` + val['Priority'] + `</div></div>`;
  //       }

  //       if (this.memo_type == 'Draft Memos' || val.Status == 'Pending for Resubmission') {
  //         val['action'] = `<div class = "lists-btn">
  //       <button click = "onCellClick();">
  //       <i class="fas fa-eye" style="cursor: pointer;"></i></button></div>&nbsp;
  //       <div class = "edits-btn">
  //       <button click = "onCellClick();">
  //       <i class="fas fa-edit" style="cursor: pointer;"></i></button>
  //       </div>`;
  //       } else {
  //         val['action'] = `<div class = "lists-btn">
  //       <button click = "onCellClick();">
  //       <i class="fas fa-eye" style="cursor: pointer;"></i></button></div>`;
  //       }
  //       val.CreatedDateTime = this.datePipe.transform(val.CreatedDateTime, "dd-MM-yyyy");
  //     });
  //     this.onChangeTable(this.config);
  //     this.progress = false;
  //   });
  // }
  // public ngOnInit(): void {
  //   this.getComboList();
  //   this.getservice();
  //   this.columns = [
  //     { name: 'الرقم المرجعي', prop: 'ReferenceNumber' },
  //     { name: 'العنوان', prop: 'Title' },
  //     { name: 'المصدر', prop: 'SourceOU' },
  //     { name: 'الوجهة', prop: 'Destination' },
  //     { name: 'الحالة', prop: 'Status' },
  //     { name: 'التاريخ', prop: 'CreatedDateTime' },
  //     { name: 'المشاركة مع الزملاء؟', prop: 'Private' },
  //     { name: 'الأولوية', prop: 'NewPriority' },
  //     //  {name: 'Action', prop: 'action'}
  //     { name: 'عرض', prop: '', cellTemplate: this.actionTemplate },
  //   ];
  //   // this.getComboList();
  //   this.dest_sourceSettings = {
  //     singleSelection: false,
  //     idField: 'OrganizationID',
  //     textField: 'OrganizationUnits',
  //     selectAllText: 'Select All',
  //     unSelectAllText: 'UnSelect All',
  //     itemsShowLimit: 3,
  //     allowSearchFilter: false
  //   };
  //   // if (this.memo_type_id != '') {
  //   //   this.getservice();
  //   // }
  //   //this.onChangeTable(this.config);
  //   // window.onbeforeunload = () => this.ngOnDestroy();
  // }

  // public onChangePage(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
  //   this.page = page;
  //   if (this.filter) {
  //     this.changeList();
  //   } else {
  //     this.getservice();
  //   }
  // }
  // public changePage(page: any, data: Array<any> = this.data): Array<any> {

  //   page = (page.page) ? page.page : page;

  //   this.itemsPerPage = (page.itemsPerPage) ? page.itemsPerPage : this.itemsPerPage;

  //   let start = (page - 1) * this.itemsPerPage;

  //   let end = this.itemsPerPage > -1 ? (start + this.itemsPerPage) : this.length;

  //   return data;

  // }

  // public changeSort(data: any, config: any): any {
  //   if (!config.sorting) {
  //     return data;
  //   }

  //   let columns = this.config.sorting.columns || [];
  //   let columnName: string = void 0;
  //   let sort: string = void 0;

  //   for (let i = 0; i < columns.length; i++) {
  //     if (columns[i].sort !== '' && columns[i].sort !== false) {
  //       columnName = columns[i].name;
  //       sort = columns[i].sort;
  //     }
  //   }

  //   if (!columnName) {
  //     return data;
  //   }

  //   // simple sorting
  //   return data.sort((previous: any, current: any) => {
  //     if (previous[columnName] > current[columnName]) {
  //       return sort === 'desc' ? -1 : 1;
  //     } else if (previous[columnName] < current[columnName]) {
  //       return sort === 'asc' ? -1 : 1;
  //     }
  //     return 0;
  //   });
  // }

  // @HostListener('document:click', ['$event'])
  // clickout(event) {
  //   if (event.srcElement.className == "edits-btn") {
  //     let element: HTMLElement = document.getElementsByName('edit')[0];
  //     element.click();
  //   }
  //   if (event.srcElement.className == "lists-btn") {
  //     let element: HTMLElement = document.getElementsByName('view')[0];
  //     element.click();
  //   }
  // }

  // async viewData(type, value: any = '') {
  //   var param = {
  //     type: 'view',
  //     pageInfo: {
  //       type: 'memo',
  //       id: value.MemoID,
  //       title: this.memo_type
  //     }
  //   };
  //   await this.commonservice.action(param);
  //   if (type == 'edit') {
  //     this.router.navigate(['ar/app/memo/memo-edit/' + value.MemoID]);
  //   } else {
  //     this.router.navigate(['ar/app/memo/memo-view/' + value.MemoID]);
  //   }
  // }
  // public changeFilter(data: any, config: any): any {
  //   let filteredData: Array<any> = data;
  //   this.columns.forEach((column: any) => {
  //     if (column.filtering) {
  //       filteredData = filteredData.filter((item: any) => {
  //         return item[column.name].match(column.filtering.filterString);
  //       });
  //     }
  //   });

  //   if (!config.filtering) {
  //     return filteredData;
  //   }

  //   if (config.filtering.columnName) {
  //     return filteredData.filter((item: any) =>
  //       item[config.filtering.columnName].match(this.config.filtering.filterString));
  //   }

  //   let tempArray: Array<any> = [];
  //   filteredData.forEach((item: any) => {
  //     let flag = true;
  //     this.columns.forEach((column: any) => {
  //       if ((item[column.name] + '').match(this.config.filtering.filterString)) {
  //         flag = true;
  //       }
  //     });
  //     if (flag) {
  //       tempArray.push(item);
  //     }
  //   });
  //   filteredData = tempArray;

  //   return filteredData;
  // }

  // public onChangeTable(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
  //   if (config.filtering) {
  //     Object.assign(this.config.filtering, config.filtering);
  //   }

  //   if (config.sorting) {
  //     Object.assign(this.config.sorting, config.sorting);
  //   }

  //   let filteredData = this.changeFilter(this.data, this.config);
  //   let sortedData = this.changeSort(filteredData, this.config);
  //   this.rows = page && config.paging ? this.changePage(page, sortedData) : sortedData;
  //   //this.length = this.length;
  // }

  // openReport() {
  //   this.bsModalRef = this.modalService.show(ReportModalComponent, { class: 'modal-lg' });
  //   this.bsModalRef.content.language = 'ar';
  // }

  // ngOnDestroy() {
  //   this.sideSubscribe.unsubscribe();
  // }
}