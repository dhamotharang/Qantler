import { Component, OnInit, EventEmitter, Output, OnDestroy } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskEvent } from 'src/app/task/service/task.event';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit, OnDestroy {

  @Output() NavClick = new EventEmitter<boolean>();
  data_type = '';
  taskStatus: any = {
    Mytask: 0,
    Assignee: 0,
    Participants: 0
  };
  pageLoadEvent: any;
  sideNavEvent: any;
  navFlag: any = true;
  currentUser = JSON.parse(localStorage.getItem('User'));

  constructor(public commonservice: CommonService, public router: Router, public taskevent: TaskEvent, private route: ActivatedRoute) { }
  activePlus = 0;
  isMemoOpen = false;
  isLetterOpen = false;
  isTaskOpen = false;
  isFourOpen = false;
  isMeetingOpen = false;
  isCircularOpen = false;

  showTopPanelData = {
    showTopLabel: false,
    buttonName: '',
    btn_url: '',
    title: ''
  }

  ngOnInit() {
    this.pageLoadEvent = this.commonservice.pageLoad$.subscribe(res => {
      this.navigationChange(res);
    });

    this.sideNavEvent = this.commonservice.sideNav$.subscribe(res => {
    this.sideNav(res.type, res.title);
    });
    
    this.commonservice.getTaskCount(this.currentUser.id)
    .subscribe((res:any) => {
      this.taskStatus.Mytask = res.MyTask || 0;
      this.taskStatus.Assignee = res.AssignedTask || 0;
      this.taskStatus.Participants = res.TaskParticipations || 0;
    });
  }

  clickNav(data) {
    this.NavClick.emit(data);
  }




  type_change(type, title, routepath) {
    this.sideNav(type, title);
    const param = {
      type: type,
      title: title,
      menu: routepath,
    };
    this.router.navigate([param.menu]);
    setTimeout(() => {
      this.commonservice.sideNavClick(param);
    }, 500);
    this.clickNav(true);
  }


  async sideNav(type, title = '') {
    switch (type) {
      case 'memo':
        this.isMemoOpen = true;
        this.data_type = (title == '') ? 'Pending Memo List' : title;
        this.showTopPanelData.title = this.data_type;
        this.showTopPanelData.showTopLabel = (title == 'Historical Memos Outgoing' || title == 'Historical Memos Incoming') ? false : true;
        this.showTopPanelData.buttonName = "+ CREATE MEMO";
        this.showTopPanelData.btn_url = type + '/memo-create';
        break;
      case 'letter':
        this.isLetterOpen = true;
        this.data_type = (title == '') ? 'My Pending Actions Incoming' : title;
        this.showTopPanelData.title = this.data_type;
        this.showTopPanelData.showTopLabel = (title == 'Historical Letters') ? false: true;
        let user = localStorage.getItem("User");
        let userdet = JSON.parse(user);
        let OrgUnitID = userdet.OrgUnitID;
        this.showTopPanelData.btn_url = type + '/outgoingletter-create';
        this.showTopPanelData.buttonName = "+ CREATE LETTER";
        if (OrgUnitID == 14) {
          this.showTopPanelData.btn_url = type + '/incomingletter-create';
        }
        break;
      case 'circular':
        this.isCircularOpen = true;
        this.data_type = (title == '') ? 'My Pending Circulars' : title;
        this.showTopPanelData.title = this.data_type;
        this.showTopPanelData.showTopLabel = true;
        this.showTopPanelData.buttonName = "+ CREATE CIRCULAR";
        this.showTopPanelData.btn_url = type + '/circular-create';
        break;
      case 'task':
        this.isTaskOpen = true;
        this.data_type = (title == '') ? 'Task Dashboard' : title;
        this.showTopPanelData.title = this.data_type;
        this.showTopPanelData.showTopLabel = (title == 'Create Task') ? false : true;
        this.showTopPanelData.buttonName = "+ CREATE TASK";
        this.showTopPanelData.btn_url = type + '/task-create';
        break;
      case 'meeting':
        this.isMeetingOpen = true;
        this.data_type = (title == '') ? 'Meeting List' : title;
        if (title == 'Meeting List'){
          this.showTopPanelData.title = this.data_type;
          this.showTopPanelData.showTopLabel = true;
          this.showTopPanelData.buttonName = "+ CREATE MEETING";
          this.showTopPanelData.btn_url = type + '/create';
        }
        break;
    };
    var param = {
      type: 'list',
      title: this.data_type,
      category: type
    }
    this.commonservice.action(param);
    this.commonservice.topBanner(this.showTopPanelData.showTopLabel, this.showTopPanelData.title, this.showTopPanelData.buttonName, this.showTopPanelData.btn_url);
  }


  navigationChange(path) {
    this.navFlag = localStorage.getItem("sideNavTigger");
    if (this.navFlag) {
      switch (path) {
        case 'memo-list':
          path = 'en/app/memo/' + path;
          this.type_change('memo', 'Pending Memo List', path);
          break;
        case 'letter-list':
          path = 'en/app/letter/' + path;
          this.type_change('letter', 'My Pending Actions Incoming', path);
          break;
        case 'task-dashboard':
          path = 'en/app/task/' + path;
          this.type_change('task', 'Task Dashboard', path);
          break;
        case 'circular-list':
          path = 'en/app/circular/' + path;
          this.type_change('circular', 'My Pending Circulars', path);
          break;
      };
      localStorage.removeItem("sideNavTigger");
    }
  }

  eventTypeSelect(type, selectType) {
    setTimeout(() => {
      this.data_type = selectType;
    }, 300);
    this.data_type = selectType;
    this.taskevent.selectType(type);
    this.type_change('task', selectType, 'en/app/task/task-dashboard');
  }

  ngOnDestroy() {
    this.pageLoadEvent.unsubscribe();
    this.sideNavEvent.unsubscribe();
  }

}
