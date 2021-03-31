import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/common.service';
import { BsModalRef } from 'ngx-bootstrap';
import { LetterListService } from '../container/component/letter-list/letterlist.service';

@Component({
  selector: 'app-link-to-modal',
  templateUrl: './link-to-modal.component.html',
  styleUrls: ['./link-to-modal.component.scss']
})
export class LinkToModalComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  title: string;
  type: string;
  lang: string;
  high: string;
  medium: string;
  low: string;
  verylow: string;
  public page: number = 1;
  public itemsPerPage: number = 10;
  public maxSize: number = 10;
  public numPages: number = 1;
  public length: number = 0;
  public type_id: any = 0;
  public columns: Array<any> = [];
  public rows: Array<any> = [];
  private data: any;
  progress: boolean = false;

  public refIds: Array<any> = [];
  public selectedIds: any;

  // labels
  refIdLabel: string = 'Ref ID';
  titleLabel: string = 'Title';
  souceLabel: string = 'Source';
  destinationLabel: string = 'Destination';
  statusLabel: string = 'Status';
  dateLabel: string = 'Date';
  isPrivateLabel: string = 'Is Private?';
  priorityLabel: string = 'Priority';
  actionLabel: string = 'Action';
  senderNameLabel: string = 'Sender Name';
  repliedLabel: string = 'Replied';
  subjectLabel: string = 'Subject';
  locationLabel: string = 'Location';
  startDateLabel: string = 'Start Date/Time';
  endDateLabel: string = 'Start Date/Time';
  meetingTypeLabel: string = 'Meeting Type';
  inviteesLabel: string = 'Invitees';
  selectLabel: string = 'Select';
  closeLabel: string = 'Close';
  tableMessages: any;

  // pagination config
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  dir: string;


  constructor(
    public bsModalRef: BsModalRef,
    private common: CommonService,
    private service: LetterListService,
    public datePipe: DatePipe, ) {
    this.lang = this.common.currentLang;
    this.tableMessages = {
      emptyMessage: this.lang === 'ar' ? this.arabic('noItemsFound') : 'No Items Found'
    }
  }

  ngOnInit() {
    this.high = this.lang == 'en' ? 'High' : this.arabic('high');
    this.medium = this.lang == 'en' ? 'Medium' : this.arabic('medium');
    this.low = this.lang == 'en' ? 'Low' : this.arabic('low');
    this.verylow = this.lang == 'en' ? 'VeryLow' : this.arabic('verylow');

    // translating labels
    this.refIdLabel = this.lang == 'en' ? 'Ref ID' : this.arabic('refid');
    this.titleLabel = this.lang == 'en' ? 'Title' : this.arabic('title');
    this.souceLabel = this.lang == 'en' ? 'Source' : this.arabic('source');
    this.destinationLabel = this.lang == 'en' ? 'Destination' : this.arabic('destination');
    this.statusLabel = this.lang == 'en' ? 'Status' : this.arabic('status');
    this.dateLabel = this.lang == 'en' ? 'Date' : this.arabic('date');
    this.isPrivateLabel = this.lang == 'en' ? 'Is Private?' : this.arabic('isprivate');
    this.priorityLabel = this.lang == 'en' ? 'Priority' : this.arabic('priority');
    this.actionLabel = this.lang == 'en' ? 'Action' : this.arabic('action');
    this.senderNameLabel = this.lang == 'en' ? 'Sender Name' : this.arabic('sendername');
    this.repliedLabel = this.lang == 'en' ? 'Need Reply' : this.arabic('replied');
    this.subjectLabel = this.lang == 'en' ? 'Subject' : this.arabic('subject');
    this.locationLabel = this.lang == 'en' ? 'Location' : this.arabic('location');
    this.startDateLabel = this.lang == 'en' ? 'Start Date/Time' : this.arabic('startdate');
    this.endDateLabel = this.lang == 'en' ? 'End Date/Time' : this.arabic('enddate');
    this.meetingTypeLabel = this.lang == 'en' ? 'Meeting Type' : this.arabic('meetingtype');
    this.inviteesLabel = this.lang == 'en' ? 'Invitees' : this.arabic('invitees');
    this.selectLabel = this.lang == 'en' ? 'Select' : 'اختر';
    this.closeLabel = this.lang == 'en' ? 'Close' : 'إغلاق';

    this.dir = this.lang == 'en' ? 'ltr' : 'rtl';




    if (this.type === 'in') {
      this.initiateIncoming();
    } else if (this.type === 'out') {
      this.initiateOutgoing();
    }

  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  public onChangePage(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
    this.page = page;
    if (this.type === 'in') {
      this.getIncomingList();
    } else {
      this.getOutgoingList();
    }
  }

  // onCheckboxChange
  onCheckboxChange(event: any) {
    if (event.target.checked) {
      this.refIds.push(event.target.value);
    } else {
      let index = this.refIds.indexOf(event.target.value);
      if (index > -1) {
        this.refIds.splice(index, 1);
      }
    }
  }

  // when select clicked
  onSelect() {
    this.selectedIds = this.refIds;
    this.refIds = [];
    this.bsModalRef.hide();
  }

  onCloseModal() {
    this.selectedIds = [];
    this.refIds = [];
  }

  // initiating incoming list
  initiateIncoming() {
    this.title = this.lang == 'en' ? 'Incoming Letter List' : this.arabic('incomingletters');
    this.type_id = 6;
    this.columns = [
      { name: this.refIdLabel, prop: 'ReferenceNumber' },
      { name: this.titleLabel, prop: 'Title' },
      { name: this.souceLabel, prop: 'Source' },
      { name: this.destinationLabel, prop: 'Destination' },
      { name: this.senderNameLabel, prop: 'SenderName' },
      { name: this.statusLabel, prop: 'Status' },
      { name: this.dateLabel, prop: 'Date' },
      { name: this.repliedLabel, prop: 'Replied' },
      { name: this.priorityLabel, prop: 'NewPriority' },
      { name: this.actionLabel, prop: '', cellTemplate: this.actionTemplate },
    ];
    this.getIncomingList();
  }

  getIncomingList() {
    this.progress = true;
    let username = this.currentUser.id,
      status = '',
      source = '',
      destination = '',
      user_name = '',
      from_date = '',
      to_date = '',
      priority = '',
      sender_name = '',
      smartSearch = '';
    this.service.letterFilterList("InboundLetters/", this.page,
      this.maxSize,
      this.type_id,
      username,
      status,
      source,
      destination,
      user_name,
      from_date,
      to_date,
      priority,
      sender_name,
      smartSearch)
      .subscribe((res: any) => {
        this.progress = false;
        this.data = res.Collection;
        if (this.data) {
          this.length = res.count;
        }
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
          val.Date = this.datePipe.transform(val.Date, "dd/yyyy");
        });
        this.rows = this.data;
      });
  }
  // end of incoming

  // initiate outgoing
  initiateOutgoing() {
    this.title = this.lang == 'en' ? 'Outgoing Letter List' : this.arabic('outgoingletters');
    this.type_id = 7;
    this.columns = [
      { name: this.refIdLabel, prop: 'ReferenceNumber' },
      { name: this.titleLabel, prop: 'Title' },
      { name: this.souceLabel, prop: 'Source' },
      { name: this.destinationLabel, prop: 'Destination' },
      { name: this.statusLabel, prop: 'Status' },
      { name: this.senderNameLabel, prop: 'SenderName' },
      { name: this.dateLabel, prop: 'Date' },
      { name: this.repliedLabel, prop: 'Replied' },
      { name: this.priorityLabel, prop: 'NewPriority' },
      { name: this.actionLabel, prop: '', cellTemplate: this.actionTemplate }
    ];
    this.getOutgoingList();
  }

  getOutgoingList() {
    this.progress = true;
    let username = this.currentUser.id,
      status = '',
      source = '',
      destination = '',
      user_name = '',
      from_date = '',
      to_date = '',
      priority = '',
      sender_name = '',
      smartSearch = '';;
    this.service.letterFilterList("InboundLetters/", this.page,
      this.maxSize,
      this.type_id,
      username,
      status,
      source,
      destination,
      user_name,
      from_date,
      to_date,
      priority,
      sender_name,
      smartSearch)
      .subscribe((res: any) => {
        this.progress = false;
        this.data = res.Collection;
        if (this.data) {
          this.length = res.Count;
        }
        if (this.data) {
          this.data.forEach(val => {
            var high = 'High',
              medium = 'Medium',
              low = 'Low',
              verylow = 'Very low';
            if (this.lang != 'en') {
              high = this.arabic('high');
              medium = this.arabic('medium');
              low = this.arabic('low');
              verylow = this.arabic('verylow');
            }
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
          this.rows = this.data;
        }
      });
  }
}
