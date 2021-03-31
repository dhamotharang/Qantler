import { ActivatedRoute, Router } from '@angular/router';
import { CalendarManagementService } from './../../service/calendar-management.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { CommonService } from './../../../../common.service';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { BsModalService } from 'ngx-bootstrap';
import { ApologiesModalComponent } from 'src/app/modal/apologies-modal/apologies-modal.component';
import { CommentModalComponent } from 'src/app/modal/comment-modal/comment-modal.component';

@Component({
  selector: 'app-event-list-page',
  templateUrl: './event-list-page.component.html',
  styleUrls: ['./event-list-page.component.scss']
})
export class EventListPageComponent implements OnInit {
  @ViewChild('selectBoxTemplate') selectBoxTemplate: TemplateRef<any>;
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('startDateTemplate') startDateTemplate: TemplateRef<any>;
  @ViewChild('endDateTemplate') endDateTemplate: TemplateRef<any>;
  public page: number = 1;
  public pageSize: number = 10;
  public pageCount: number;
  public maxSize: number = 10;
  columns: Array<any> = [];
  statusList: Array<any> = [];
  arabicColumns: Array<any> = [];
  lang: string;
  approverList: Array<any>;
  ApproverID: any;
  ReferenceNumber: string;
  Status: any;
  ProposedBy: string;
  isChecked: boolean = false;
  id: any;
  selectedEventList:any[] = [];
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  rows: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  selected:any = [];
  message: string;
  bsModalRef: any;
  inProgress: boolean;
  ShowButtons: boolean;
  componentRef = this;
  checkSelectAll:boolean = false;
  tableMessages: { emptyMessage: any; };
  constructor(
    private common: CommonService,
    public arabic: ArabicDataService,
    public modalService: BsModalService,
    private route: ActivatedRoute,
    private router: Router,
    public calendarManagementService: CalendarManagementService
  ) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.lang == 'en') {
      this.common.breadscrumChange('Protocol Calendar', 'Event List', '');
    } else {
      this.common.breadscrumChange(this.arabic.words['protocolcalendar'], 'قائمة الأحداث', '');
    }
    this.calendarManagementService.getById(0, this.currentUser.id).subscribe(
      (response: any) => {
        if (response != null) {
          this.statusList = response.M_LookupsList;
        }
      }
    );
    this.loadUserList();
    this.loadList();
    this.ListBulkView('');
    this.getReferenceNumber();
    this.common.apologiesModalClose$.subscribe((isApologiesModalClose)=> {
      if(isApologiesModalClose == 'close'){
        this.inProgress = false;
      }
    });

    this.common.commentModalClose$.subscribe((isCommentModalClose)=> {
      if(isCommentModalClose == 'close'){
        this.inProgress = false;
      }
    });
    this.tableMessages = {
      emptyMessage: this.lang == 'en' ? 'No data to display' : this.arabic.words['nodatatodisplay']
    }
  }

  loadUserList() {
    const params = [{
      'OrganizationID': '',
      'OrganizationUnits': ''
    }];
    this.common.getUserList(params, this.currentUser.id).subscribe(
      (response: any) => {
        this.approverList = response;
      }
    );
  }

  loadList() {
    this.columns = [
      { name: '  Select All ', prop: 'SelectAll', headerCheckboxable: true, checkboxable: true },
      { name: 'Event Requestor', prop: 'EventRequestor' },
      { name: 'Event Type', prop: 'EventType' },
      { name: 'Event Details', prop: 'EventDetails' },
      { name: 'Start Date',cellTemplate: this.startDateTemplate },
      { name: 'End Date', cellTemplate: this.endDateTemplate },
      { name: 'Location', prop: 'Location' },
      { name: 'Status', prop: 'StatusName' },
      { name: 'Action', cellTemplate: this.actionTemplate }
    ];
    this.arabicColumns = [
      { name: ' '+this.arabic.words['selectall']+' ', prop: 'SelectAll', headerCheckboxable: true, checkboxable: true },
      { name: this.arabic.words['eventrequestor'], prop: 'EventRequestor' },
      { name: this.arabic.words['eventtype'], prop: 'EventType' },
      { name: this.arabic.words['eventdetails'], prop: 'EventDetails' },
      { name: this.arabic.words['startdate'], cellTemplate: this.startDateTemplate },
      { name: this.arabic.words['enddate'], cellTemplate: this.endDateTemplate },
      { name: this.arabic.words['location'], prop: 'Location' },
      { name: this.arabic.words['status'], prop: 'StatusName' },
      { name: this.arabic.words['action'], cellTemplate: this.actionTemplate }
    ];
  }

  viewData(row: any) {
    this.router.navigate(['/app/media/calendar-management/view-event/' + row.CalendarID]);
  }

  getReferenceNumber() {
    this.calendarManagementService.getevent(this.id, this.currentUser.id).subscribe(
      (response: any) => {
        this.ReferenceNumber = response.ReferenceNumber;
        this.Status = response.Status;
        this.ProposedBy = response.CreatedBy;
        this.ApproverID = response.ApproverID;
        this.ListBulkView(response.ReferenceNumber);
      }
    );
  }

  ListBulkView(RefNumber: any) {
    if(RefNumber){
      this.calendarManagementService.listBulkView(this.currentUser.id, RefNumber).subscribe(
        (allOwnRes: any) => {
        if(allOwnRes){
          this.rows = allOwnRes.Collection;
          this.rows.forEach((rowObj)=> {
            let statusInd = this.statusList.findIndex((st) => st.LookupsID == rowObj.Status);
            if(statusInd > -1){
              rowObj.StatusName = this.statusList[statusInd].DisplayName;
            }
          });
          this.config.totalItems = allOwnRes.Count;
          this.ProposedBy = allOwnRes.CreatedBy;
          this.ApproverID = allOwnRes.ApproverID;
          this.Status = allOwnRes.Status;
          this.actionButtonsControl();
        }
      });
    }
  }

  onChangePage(config,event){
    this.ListBulkView('');
  }

  eventSelectDeselect(row: any) {
    // var condition = this.isChecked == true ? 'yes' : 'no';
  }

  onSelect({ selected }) {
    let allowedEvents = [];
    selected.forEach((selObj) => {
      if(selObj.Status == 120 && selObj.ApproverID == this.currentUser.id){
        allowedEvents.push(selObj);
      }
    });
    this.selectedEventList = allowedEvents;
  }

  onMobileSelect( selectedrow ) {
    // let allowedEvents = this.selectedEventList;
    // selected.forEach((selObj) => {
      if(selectedrow.checked){
        if(selectedrow.Status == 120 && selectedrow.ApproverID == this.currentUser.id){
          this.selectedEventList.push(selectedrow);
        }
      }else{
        let delInd = this.selectedEventList.findIndex(srow => srow.CalendarID == selectedrow.CalendarID);
        if(delInd > -1){
          this.selectedEventList.splice(delInd,1);
        }
      }

    // });
    // this.selectedEventList = allowedEvents;
    console.log(this.selectedEventList);
  }

  mobileViewSelectAll(eventCheck: any){
    let allowedEvents = [];
    if (eventCheck == 'checked') {
      this.rows.forEach((selObj) => {
        if(selObj.Status == 120 && selObj.ApproverID == this.currentUser.id){
          selObj.checked = true;
          allowedEvents.push(selObj);
        }
      });
      this.selectedEventList = allowedEvents;
    }else{
      this.rows.forEach((selObj) => {
        // if(selObj.Status == 120 && selObj.ApproverID == this.currentUser.id){
          selObj.checked = false;
        // }
      });
      this.selectedEventList =  [];
    }
    console.log(this.selectedEventList);
  }

  isSelectedEmpty() {
    if(this.selectedEventList && this.selectedEventList.length >= 1) {
      return false;
    }
    return true;
  }

  onActivate(event) {
  }

  displayCheck(row) {
    return row.name !== 'Select All';
  }

  action(action: string){
    let toSendRequestData:any = {
      Action: action,
      ApproverID: this.ApproverID,
      ActionBy: this.currentUser.id,
      ActionDateTime: new Date()
    }

    toSendRequestData.CalendarID = [];
    this.selectedEventList.forEach((selObj) => {
      toSendRequestData.CalendarID.push({CalendarID:selObj.CalendarID});
    });
    this.updateAction(toSendRequestData, action);
  }

  updateAction(dataForUpload: any, action: string) {
    this.inProgress = true;
    switch(action) {
      case 'Approve':
        this.message = 'Events Request Approved Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic.words['calendarreqapprovemsg'];
        }
        break;
      case 'Reject':
        this.message = 'Events Request Rejected Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic.words['calendarreqrejectemsg'];
        }
        break;
      case 'ReturnForInfo':
        this.message = 'Events Request Returned For Info successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic.words['calendarreqreturnmsg'];
        }
        break;
      case 'Escalate':
        this.message = 'Events Request Escalated Successfully';
        if(this.common.currentLang == 'ar'){
          this.message = this.arabic.words['calendarreqescalatemsg'];
        }
        break;
    }
    this.calendarManagementService.bulkEventUpdate(dataForUpload).subscribe(
      (response: any) => {
        if(action != 'Reject'){
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/app/media/calendar-management/list']);
          });
        }else if(action == 'Reject'){
          this.openRejectModal();
        }
      }
    );
  }

  openRejectModal() {
    this.inProgress = true;
    var initialState = {
      id: this.id,
      ApproverID: null,
      ApiString: '/Calendar',
      message: 'Event Requests Rejected Successfully',
      ApiTitleString: 'APOLOGIES',
      redirectUrl: '/app/media/calendar-management/list',
      isBulkEvent:true,
      data :this.selectedEventList
    };
    if(this.common.currentLang == 'ar'){
      initialState.message = this.arabic.words['calendarreqrejectmsg'];
    }
    this.bsModalRef = this.modalService.show(ApologiesModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  openReturnForInfoModal(action: string) {
    this.inProgress = true;
    let initialState:any = {};

    if (action == 'return') {
       initialState = {
        action: 'return',
        id: this.id,
        ApproverID: this.ApproverID,
        ApiString: '/Calendar',
        message: 'Event Requests Returned for info Successfully',
        ApiTitleString: 'Comment',
        redirectUrl: '/app/media/calendar-management/list',
        data :this.selectedEventList,
      };
      if(this.common.currentLang == 'ar'){
        initialState.message = this.arabic.words['calendarreqreturnmsg'];
      }
    } else {
       initialState = {
        action: 'escalate',
        id: this.id,
        ApproverID: null,
        ApiString: '/Calendar',
        message: 'Event Requests Escalated Successfully',
        ApiTitleString: 'Escalate',
        redirectUrl: '/app/media/calendar-management/list',
        data :this.selectedEventList,
        departmentOrgID:10,
        isDepartmentisDepartmentNotNeededNeeded:false
      };
      if(this.common.currentLang == 'ar'){
        initialState.message = this.arabic.words['calendarreqescalatemsg'];
      }
    }
    this.bsModalRef = this.modalService.show(CommentModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  actionButtonsControl() {
    this.ShowButtons = false;
    if (this.Status == 120) {
      this.rows.forEach((rObj) => {
        if(rObj.ApproverID == this.currentUser.id){
          this.ShowButtons = true;
        }
      });
    }
  }

  allowSelect(row: any) {
    let getCurrentUser = JSON.parse(localStorage.getItem('User'));
    return row.Status == 120 && row.ApproverID == getCurrentUser.id;
  }
}
