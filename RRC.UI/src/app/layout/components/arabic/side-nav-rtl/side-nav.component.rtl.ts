import { Component, OnInit, EventEmitter, Output, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskEvent } from 'src/app/task/service/task.event';
import { ArabicDataService } from 'src/app/arabic-data.service';

@Component({
  selector: 'app-side-nav-rtl',
  templateUrl: './side-nav.component.rtl.html',
  styleUrls: ['./side-nav.component.rtl.scss']
})
export class SideNavComponentRTL implements OnInit {

  @Output() NavClick = new EventEmitter<boolean>();
  data_type = '';
  taskStatus: any = {
    Mytask: 0,
    Assignee: 0,
    Participants: 0
  };
  arabicFont: any;
  currentUser = JSON.parse(localStorage.getItem('User'));

  constructor(public commonservice: CommonService, public arabic: ArabicDataService, public router: Router, public taskevent: TaskEvent, private route: ActivatedRoute) {
  }
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
    var path = '';
    this.commonservice.pageLoad$.subscribe(res => {
      this.navigationChange(res);
    });

    this.commonservice.sideNav$.subscribe(res => {
      this.sideNav(res.type, res.title);
    });

    this.commonservice.getTaskCount(this.currentUser.id)
      .subscribe((res: any) => {
        this.taskStatus.Mytask = res.MyTask || 0;
        this.taskStatus.Assignee = res.AssignedTask || 0;
        this.taskStatus.Participants = res.TaskParticipations || 0;
      });

  }

  clickNav(data) {
    this.NavClick.emit(data);
  }




  type_change(type, title, routepath) {
    console.log('side nav');
    this.sideNav(type, title);
    //await this.commonservice.topBanner(this.showTopPanelData.showTopLabel, this.showTopPanelData.title, this.showTopPanelData.buttonName, this.showTopPanelData.btn_url);
    const param = {
      type: type,
      title: title,
      menu: routepath,
    };
    this.router.navigate([param.menu]);
    setTimeout(() => {
      this.commonservice.sideNavClick(param);
    }, 500);
    //this.commonservice.sideNavClick(param);
    this.clickNav(true);
  }


  async sideNav(type, title = '') {
    switch (type) {
      case 'memo':

        this.isMemoOpen = true;
        this.data_type = (title == '') ? 'Pending Memo List' : title;
        this.showTopPanelData.title = this.commonservice.sideNaveTypeArabic(this.data_type);
        this.showTopPanelData.showTopLabel = true;
        this.showTopPanelData.buttonName = '+ '+this.arabicWord('memocreate');
        this.showTopPanelData.btn_url = type + '/memo-create';
        break;
      case 'letter':
        this.isLetterOpen = true;
        this.data_type = (title == '') ? 'My Pending Actions Incoming' : title;
        this.showTopPanelData.title = this.commonservice.sideNaveTypeArabic(this.data_type);
        this.showTopPanelData.showTopLabel = (title == 'Historical Letters') ? false : true;
        let user = localStorage.getItem("User");
        let userdet = JSON.parse(user);
        let OrgUnitID = userdet.OrgUnitID;
        this.showTopPanelData.btn_url = type + '/outgoingletter-create';
        this.showTopPanelData.buttonName = "+ انشاء كتاب جديد";
        if (OrgUnitID == 14) {
          this.showTopPanelData.btn_url = type + '/incomingletter-create';
        }
        // this.showTopPanelData.buttonName = "+ CREATE MEMO";
        // this.showTopPanelData.btn_url = type + '/memo-create';
        break;
      case 'circular':
        this.isCircularOpen = true;
        this.data_type = (title == '') ? 'My Pending Circulars' : title;
        this.showTopPanelData.title = this.commonservice.sideNaveTypeArabic(this.data_type);
        this.showTopPanelData.showTopLabel = true;
        this.showTopPanelData.buttonName = '+ ' + this.arabicWord('circularcreate');
        this.showTopPanelData.btn_url = type + '/circular-create';
        break;
      case 'task':
        this.isTaskOpen = true;
        this.data_type = (title == '') ? 'Task Dashboard' : title;
        this.showTopPanelData.title = this.commonservice.sideNaveTypeArabic(this.data_type);
        this.showTopPanelData.showTopLabel = (title == 'Create Task') ? false : true;
        this.showTopPanelData.buttonName = '+ ' + this.arabicWord('taskcreate');
        this.showTopPanelData.btn_url = type + '/task-create';
        break;
      case 'meeting':
        this.isMeetingOpen = true;
        this.data_type = (title == '') ? this.commonservice.arabic.sideNavArabic['Meeting List'] : title;
        if (title == this.commonservice.arabic.sideNavArabic['Meeting List']) {
          this.showTopPanelData.title = this.data_type;
          this.showTopPanelData.showTopLabel = true;
          this.showTopPanelData.buttonName = '+ ' + this.commonservice.arabic.words['createmeeting'];
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
    switch (path) {
      case 'memo-list':
        path = 'ar/app/memo/' + path;
        this.type_change('memo', 'Pending Memo List', path);
        break;
      case 'letter-list':
        path = 'ar/app/letter/' + path;
        this.type_change('letter', 'My Pending Actions Incoming', path);
        break;
      case 'task-dashboard':
        path = 'ar/app/task/' + path;
        this.type_change('task', 'Task Dashboard', path);
        break;
      case 'circular-list':
        path = 'ar/app/circular/' + path;
        this.type_change('circular', 'My Pending Circulars', path);
        break;
    };
  }

  eventTypeSelect(type, selectType) {
    setTimeout(() => {
      this.data_type = selectType;
    }, 300);
    this.data_type = selectType;
    this.taskevent.selectType(type);
    this.type_change('task', selectType, 'ar/app/task/task-dashboard');
  }

  arabicNav(type) {
    //var t = this.commonservice.sideNavArabic[type];
    return this.arabic.sideNavArabic[type];
  }

  // languageTopTitle(title) {
  //   switch (title) {
  //     case 'My Pending Actions':
  //       this.showTopPanelData.title = 'أفعالي المعلقة';
  //       break;
  //     case 'Incoming Memos':
  //       this.showTopPanelData.title = 'المذكرات الواردة';
  //       break;
  //     case 'Outgoing Memos':
  //       this.showTopPanelData.title = 'المذكرات الصادرة';
  //       break;
  //     case 'Draft Memos':
  //       this.showTopPanelData.title = 'مسودات المذكرات';
  //       break;
  //     case 'Historical Memos Incoming':
  //       this.showTopPanelData.title = 'سجلات المذكرات الواردة السابقة';
  //       break;
  //     case 'Historical Memos Outgoing':
  //       this.showTopPanelData.title = 'سجلات المذكرات الصادرة السابقة';
  //       break;
  //   }
  // }

  arabicWord(word) {
    return this.commonservice.arabic.words[word];
  }

}
