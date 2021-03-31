import { Component, OnInit, ViewChild, TemplateRef,HostListener, ElementRef } from '@angular/core';
import Chart from 'chart.js';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { CommonService } from '../../../common.service';
import { MediaRequestStaffListService } from './media-request-staff-list.service';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
@Component({
  selector: 'app-media-request-staff-list',
  templateUrl: './media-request-staff-list.component.html',
  styleUrls: ['./media-request-staff-list.component.scss']
})
export class MediaRequestStaffListComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('pieChart') pieChart: ElementRef;
  @ViewChild('barChart') barChart: ElementRef;
public rows:Array<any> = [];
 bsConfig: Partial<BsDatepickerConfig>= {
  dateInputFormat:'DD/MM/YYYY'
};
   public columns:Array<any> = [];
  public page:number = 1;
  public itemsPerPage:number = 10;
  public maxSize:number = 10;
  public numPages:number = 1;
  public length:number = 0;
  public alreadyExist:boolean=false;

  public config:any = {
    paging: true,
    sorting: {columns: this.columns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  private data:any;
  filter_data:any=[];
  media_id ='';
  filter = false;
  status = '';
  source = '';
  destination = '';
  date_from = '';
  date_to = '';
  private = '';
  priority = '';
  smartSearch = '';
  statusOptions = [];
  userList =[];
  sourceouOptions = [];
  destinationOptions = [];
  public cardDetails = [
    {
      'image': 'assets/task/clipboards1.png',
      'count': 10,
      'name': 'Request for photo',
      'progress': 50
    },
    {
      'image': 'assets/task/clipboards2.png',
      'count': 20,
      'name': 'Request for design',
      'progress': 50
    },
    {
      'image': 'assets/task/clipboards3.png',
      'count': 10,
      'name': 'Request for press release',
      'progress': 50
    }, {
      'image': 'assets/task/clipboards1.png',
      'count': 10,
      'name': 'Request for campagin',
      'progress': 50
    },
    {
      'image': 'assets/task/clipboards2.png',
      'count': 20,
      'name': 'Request for photographer',
      'progress': 50
    },
    {
      'image': 'assets/task/clipboards3.png',
      'count': 50,
      'name': 'Request to use diwan identity',
      'progress': 50
    }
  ];

  constructor(private common: CommonService,public router:Router,private mediarequeststafflistservice: MediaRequestStaffListService,public datePipe:DatePipe) {
    //this.getUserList([]);
    this.getservice();

    this.common.breadscrumChange('Media Request Staff List','List Page','');
    this.common.topBanner(true,'Media Request Staff List','+ CREATE REQUEST','media-request-design-form-creation');
    //this.rows = this.taskList;
    //this.priorityList = this.common.priorityList;
  }

  ngOnInit() {
    this.columns = [
      { name: 'Ref ID', prop: 'TaskReferenceNumber' },
      { name: 'Source', prop: 'Source' },
      { name: 'Status', prop: 'Status' },
      { name: 'Request Type', prop: 'RequestType' },
      { name: 'Request Date', prop: 'RequestDate' },
      { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
    ];
  }
    public changeList(){
      this.filter_data = this;
       this.filter = true;
       let source = '';
       let destination = '';
        let from_date = '';
        let to_date = '';
    if(this.filter_data.date_from){
       from_date = new Date(this.filter_data.date_from).toJSON();
    }
     if(this.filter_data.date_to){
       to_date = new Date(this.filter_data.date_to).toJSON();
    }
    if (this.filter_data.source !='') {
      source = this.filter_data.source[0];
    }
    if (this.filter_data.destination != '') {
      destination = this.filter_data.destination[0];
    }
        let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let username = userdet.username;
      let filterData={
        "status": this.filter_data.status.LookupsID,
        "sourceOU": source,
        "destinationOU": destination,
        "dateRangeForm": this.filter_data.date_from,
        "dateRangeTo": this.filter_data.date_to,
        "private": this.filter_data.private,
        "priority": this.filter_data.priority,
        "smartSearch": this.filter_data.smartSearch
       };
      this.mediarequeststafflistservice.memoFilterList("memos/",this.page,this.maxSize,1,username,this.filter_data.status,source,destination,this.filter_data.private,this.filter_data.priority,from_date,to_date,this.filter_data.smartSearch)
      .subscribe((res: any)=>{
      this.data = res.collection;

      if(this.data){
        this.length = res.count;
        //this.maxSize = this.data.count
      }

      var click = "(click)='editClick()'";
      this.data.forEach(val => {

       val.CreatedDateTime=this.datePipe.transform(val.CreatedDateTime,"dd/MM/yyyy");
      });
      this.onChangeTable(this.config);

    });
    }
  public getservice(){
    let user = localStorage.getItem("User");
    let userdet = JSON.parse(user);
    let username = userdet.username;
    // let res.collection =[];
    // let res.count ='0';
    this.mediarequeststafflistservice.memoList("memos/",this.page,this.maxSize,1,username).subscribe((res: any)=>{
      this.data = res.collection;
      if(this.data){
        this.length = res.count;
        //this.maxSize = this.data.count
      }
    //this.changePage(1,this.data);
      var click = "(click)='editClick()'";
      this.data.forEach(val => {
        val.CreatedDateTime=this.datePipe.transform(val.CreatedDateTime,"dd/MM/yyyy");
      });
      this.onChangeTable(this.config);
    });
  }
   public getComboList(){
    let user_id = '1';
    let letter_id = '0';
    let requestData =[];
    this.mediarequeststafflistservice.memoCombos("memo/",letter_id,user_id).subscribe((res: any)=>{
      this.statusOptions = res.M_LookupsList;
      this.sourceouOptions = res.OrganizationList;
      this.destinationOptions = res.OrganizationList;
      });
    this.mediarequeststafflistservice.userList('User/', requestData).subscribe((res: any)=>{
      this.userList = res;
      });
  }
  // async getUserList(data) {
  //   await this.common.getUserList(data).subscribe(list => {
  //     this.userList = list;
  //   });
  // }
   public onChangePage(config:any, page:any = {page: this.page, itemsPerPage: this.itemsPerPage}):any {
     this.page = page;
     if(this.filter){
       this.changeList();
     }else{
       this.getservice();
     }
    }
  public changePage(page: any, data: Array<any> = this.data): Array<any> {

 page = (page.page)?page.page:page;

 this.itemsPerPage = (page.itemsPerPage)?page.itemsPerPage:this.itemsPerPage;

 let start = (page - 1) * this.itemsPerPage;

 let end = this.itemsPerPage > -1 ? (start + this.itemsPerPage) : this.length;

 return data;

 }

  public changeSort(data:any, config:any):any {
    if (!config.sorting) {
      return data;
    }

    let columns = this.config.sorting.columns || [];
    let columnName:string = void 0;
    let sort:string = void 0;

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
    return data.sort((previous:any, current:any) => {
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
    if(event.srcElement.className=="edits-btn")
    {
let element: HTMLElement = document.getElementsByName('edit')[0];
    element.click();
    }
    if(event.srcElement.className=="lists-btn")
    {
let element: HTMLElement = document.getElementsByName('view')[0];
    element.click();
    }
  }

  viewData(type,value){
    if(type=='edit'){
      this.router.navigate(['pages/incomingletter-edit/' + value.MemoID]);
    }else{
      this.router.navigate(['pages/outgoingletter-view/'+value.MemoID]);
    }
  }

  onCellClick(event){
    this.media_id = event.row.MemoID;
  }

  public changeFilter(data:any, config:any):any {
    let filteredData:Array<any> = data;
    this.columns.forEach((column:any) => {
      if (column.filtering) {
        filteredData = filteredData.filter((item:any) => {
          return item[column.name].match(column.filtering.filterString);
        });
      }
    });

    if (!config.filtering) {
      return filteredData;
    }

    if (config.filtering.columnName) {
      return filteredData.filter((item:any) =>
        item[config.filtering.columnName].match(this.config.filtering.filterString));
    }

    let tempArray:Array<any> = [];
    filteredData.forEach((item:any) => {
      let flag = true;
      this.columns.forEach((column:any) => {
        if ((item[column.name]+'').match(this.config.filtering.filterString)) {
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

  public onChangeTable(config:any, page:any = {page: this.page, itemsPerPage: this.itemsPerPage}):any {
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
}

