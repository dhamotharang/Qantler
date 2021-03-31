import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/common.service';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
@Component({
  selector: 'app-search-container',
  templateUrl: './search-container.component.html',
  styleUrls: ['./search-container.component.scss']
})
export class SearchContainerComponent implements OnInit {

  searchKey:string = '';
  public page: number = 1;
  public pageSize: number = 10;
  public pageCount: number;
  public maxSize: number = 10;
  public TypetoSearch :any;
  currentUser: any ;
  searchresult :any=[];
  isEngLang: boolean = true;
  isPaging :boolean =false;
  TotalCount:any =0;
  progress = false;
  public itemsPerPage: number = 10;
  Title:any=[];
  public config: any = {
    paging: true,
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };
  constructor( public router:Router,public common: CommonService,public modalService: BsModalService) { }

  ngOnInit() {
    this.currentUser=JSON.parse(localStorage.getItem('User'));
this.Title=[{
  title:'Memos',
  type:1,
  Count:0,
  Counttype:'Memos'
},
{
  title:'Letters',
  type:2,
  Count:0,
  Counttype:'Letters'
},
{
  title:'Duty Task',
  type:3,
  Count:0,
  Counttype:'DutyTask'
},
{
  title:'Meetings',
  type:4,
  Count:0,
  Counttype:'Meetings'
},
{
  title:'Circulars',
  type:5,
  Count:0,
  Counttype:'Circulars'
},
{
  title:'Legal',
  type:6,
  Count:0,
  Counttype:'Legal'
},
{
  title:'Protocol',
  type:7,
  Count:0,
  Counttype:'Protocol'
},
{
  title:'HR',
  type:8,
  Count:0,
  Counttype:'HR'
},{
  title:'Citizen Affair',
  type:9,
  Count:0,
  Counttype:'CitizenAffair'
},
{
  title:'Maintenance',
  type:10,
  Count:0,
  Counttype:'Maintenance'
},
{
  title:'IT',
  type:11,
  Count:0,
  Counttype:'IT'
}
];
if(this.common.currentLang == 'en'){
  this.isEngLang = true;
 }
 else{
  this.isEngLang = false;
  this.Title.forEach(element => {
    element.title=this.common.arabic.globalsearch[element.title.trim().replace(/\s+/g, '').toLowerCase()];
    
  });
 }
    }
    public onChangePage(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
      this.page = page;
      this.isPaging = true;
      this.getSearchData('');
    }
getSearchData(value:any)
{
  this.progress=true;
  if(!this.isPaging)
  {
  if(value=="")
  this.TypetoSearch =1;
  else
  this.TypetoSearch=value;
  }
  this.isPaging=false;
  this.common.getSearchDetail(this.page, this.maxSize,this.currentUser.id,this.searchKey,this.TypetoSearch).subscribe((data:any) => {
    this.Title.forEach(element => {
      if(data.ModulesCount[element.Counttype])
      {
        if(this.TypetoSearch==element.type)
        this.config.totalItems=data.ModulesCount[element.Counttype];
        element.Count=data.ModulesCount[element.Counttype];
       }
      else{
        element.Count=0;
      }
    });
    this.searchresult=data.Collection;
    this.TotalCount=data.Count;
    this.searchresult.forEach(createlink => {
      let path;
      if(this.common.currentLang == 'en')
       path='en';
      else
      path='ar'
      if(!createlink.CanEdit)
      {
      switch(createlink.Type)
      {
        case 1:
        createlink.link='rrcui/#'+path + '/app/memo/memo-view/' +createlink.ID
        break;
        case 2:
        createlink.link='rrcui/#'+path + '/app/letter/incomingletter-view/' +createlink.ID
        break;
        case 3:
        createlink.link='rrcui/#'+path + '/app/letter/outgoingletter-view/' +createlink.ID
        break;
        case 4:
        createlink.link='rrcui/#'+path + '/app/task/task-view/' +createlink.ID 
        break;
        case 5:
        createlink.link='rrcui/#'+path + '/app/meeting/view/' +createlink.ID
        break;
        case 6:
        createlink.link='rrcui/#'+path + '/app/circular/Circular-view/' +createlink.ID
        break;
        case 7:
        createlink.link='rrcui/#'+path + '/app/legal/request-view/' +createlink.ID
        break;
        case 8:
        createlink.link='rrcui/#'+path + '/app/citizen-affair/citizen-affair-view/' +createlink.ID
        break;
        case 9:
        createlink.link='rrcui/#'+path + '/app/citizen-affair/complaint-suggestion-view/' +createlink.ID
        break;
        case 10:
        createlink.link='rrcui/#'+path + '/app/maintenance/view/' +createlink.ID 
        break;
        case 11:
        createlink.link='rrcui/#'+path + '/app/media/gifts-management/request-view/' +createlink.ID
        break;
        case 12:
        createlink.link='rrcui/#'+path + '/app/media/calendar-management/view-event/' +createlink.ID
        break;
        case 13:
        createlink.link='rrcui/#'+path + '/app/media/media-request-photo/view/' +createlink.ID
        break;
        case 14:
        createlink.link='rrcui/#'+path + '/app/media/media-request-design/' +createlink.ID
        break;
        case 15:
        createlink.link='rrcui/#'+path + '/app/media/media-press-release/view/' +createlink.ID
        break;
        case 16:
        createlink.link='rrcui/#'+path + '/app/media/campaign/view/' +createlink.ID
        break;
        case 17:
        createlink.link='rrcui/#'+path + '/app/media/photographer/view/' +createlink.ID
        break;
        case 18:
        createlink.link='rrcui/#'+path + '/app/media/diwan-identity/request-view/' +createlink.ID
        break;
        case 19:
        createlink.link='rrcui/#'+path + '/app/hr/leave/request-view/' +createlink.ID
        break;
        case 20:
        createlink.link='rrcui/#'+path + '/app/hr/salary-certificate/view/' +createlink.ID
        break;
        case 21:
        createlink.link='rrcui/#'+path + '/app/hr/experience-certificate/view/' +createlink.ID
        break;
        case 22:
        createlink.link='rrcui/#'+path + '/app/hr/new-baby-addition/request-view/' +createlink.ID
        break;
        case 23:
        createlink.link='rrcui/#'+path + '/app/hr/announcement/announcement-view/' +createlink.ID
        break;
        case 24:
        createlink.link='rrcui/#'+path + '/app/hr/training-request/request-view/' +createlink.ID
        break;
        case 25:
        createlink.link='rrcui/#'+path + '/app/hr/official-tasks/request-view/official/' +createlink.ID
        break;
        case 26:
        createlink.link='rrcui/#'+path + '/app/hr/raise-complaint-suggestion-view/' +createlink.ID
        break;
        case 27:
        createlink.link='rrcui/#'+path + '/app/it/it-request-view/' +createlink.ID
        break;
      }
    }
    else{
      switch(createlink.Type){
      case 1:
      createlink.link='rrcui/#'+path + '/app/memo/memo-edit/' +createlink.ID
      break;
      case 2:
      createlink.link='rrcui/#'+path + '/app/letter/incomingletter-edit/' +createlink.ID
      break;
      case 3:
      createlink.link='rrcui/#'+path + '/app/letter/outgoingletter-edit/' +createlink.ID
      break;
      case 4:
      createlink.link='rrcui/#'+path + '/app/task/task-view/' +createlink.ID
      break;
      case 5:
      createlink.link='rrcui/#'+path + '/app/meeting/view/' +createlink.ID
      break;
      case 6:
      createlink.link='rrcui/#'+path + '/app/circular/Circular-edit/' +createlink.ID
      break;
      case 7:
      createlink.link='rrcui/#'+path + '/app/legal/request-view/' +createlink.ID
      break;
      case 8:
      createlink.link='rrcui/#'+path + '/app/citizen-affair/citizen-affair-edit/' +createlink.ID
      break;
      case 9:
      createlink.link='rrcui/#'+path + '/app/citizen-affair/complaint-suggestion-edit/' +createlink.ID
      break;
      case 10:
      createlink.link='rrcui/#'+path + '/app/maintenance/view/' +createlink.ID 
      break;
      case 11:
      createlink.link='rrcui/#'+path + '/app/media/gifts-management/request-view/' +createlink.ID
      break;
      case 12:
      createlink.link='rrcui/#'+path + '/app/media/calendar-management/view-event/' +createlink.ID
      break;
      case 13:
      createlink.link='rrcui/#'+path + '/app/media/media-request-photo/edit/' +createlink.ID
      break;
      case 14:
      createlink.link='rrcui/#'+path + '/app/media/media-request-design/edit' +createlink.ID
      break;
      case 15:
      createlink.link='rrcui/#'+path + '/app/media/media-press-release/view/' +createlink.ID
      break;
      case 16:
      createlink.link='rrcui/#'+path + '/app/media/campaign/edit/' +createlink.ID
      break;
      case 17:
      createlink.link='rrcui/#'+path + 'en/app/media/photographer/edit/' +createlink.ID
      break;
      case 18:
      createlink.link='rrcui/#'+path + '/app/media/diwan-identity/request-view/' +createlink.ID///need
      break;
      case 19:
      createlink.link='rrcui/#'+path + '/app/hr/leave/request-view/' +createlink.ID
      break;
      case 20:
      createlink.link='rrcui/#'+path + '/app/hr/salary-certificate/view/' +createlink.ID
      break;
      case 21:
      createlink.link='rrcui/#'+path + '/app/hr/experience-certificate/view/' +createlink.ID
      break;
      case 22:
      createlink.link='rrcui/#'+path + '/app/hr/new-baby-addition/request-view/' +createlink.ID
      break;
      case 23:
      createlink.link='rrcui/#'+path + '/app/hr/announcement/announcement-view/' +createlink.ID
      break;
      case 24:
      createlink.link='rrcui/#'+path + '/app/hr/training-request/request-view/' +createlink.ID
      break;
      case 25:
      createlink.link='rrcui/#'+path + '/app/hr/official-tasks/request-view/official/' +createlink.ID
      break;
      case 26:
      createlink.link='rrcui/#'+path + '/app/hr/raise-complaint-suggestion-view/' +createlink.ID
      break;
      case 27:
      createlink.link='rrcui/#'+path + '/app/it/it-request-view/' +createlink.ID
      break;
      }
    }
    
    });
    this.progress=false;
  });
}
  arabic(word) {
    return this.common.arabic.words[word];
  }
}
