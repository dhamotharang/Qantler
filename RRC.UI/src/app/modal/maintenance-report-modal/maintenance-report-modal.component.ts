import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { MaintenanceService } from 'src/app/maintenance/service/maintenance.service';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-maintenance-report-modal',
  templateUrl: './maintenance-report-modal.component.html',
  styleUrls: ['./maintenance-report-modal.component.scss']
})
export class MaintenanceReportModalComponent implements OnInit {
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  statusList:Array<any> = [];
  status:any = '';
  sourceOUList:Array<any> = [];
  sourceOU: any = '';
  priorityList: any;
  priority: any = '';
  subject: string = '';
  dateTo: Date;
  dateFrom: Date;
  attendedBy: string = '';
  smartSearch: string ='';
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  lang: string = 'ar';
  arWords : any;

  constructor(public bsModalRef: BsModalRef,
    private maintenanceService: MaintenanceService,
    private common: CommonService) {
      this.lang = this.common.currentLang;
      this.arWords = this.common.arabic.words;
      this.priorityList = [
        { id: 0, name: this.lang === 'ar' ? this.arWords.all :'All', value: '' },
        { id: 1, name: this.lang === 'ar' ? this.arWords.high : 'High', value: 'high' },
        { id: 2, name: this.lang === 'ar' ? this.arWords.low : 'Low', value: 'low' }
      ];
    }

  ngOnInit() {
    this.maintenanceService.getMaintenanceList(0, 0, '')
      .subscribe((response:any) => {
        this.statusList = [{
          LookupsID: '', 
          DisplayName: this.common.currentLang == 'ar' ? this.arWords.all : 'All'
        }].concat(response.M_LookupsList);
        this.sourceOUList = [{
          OrganizationID: '', 
          OrganizationUnits: this.common.currentLang == 'ar' ? this.arWords.all : 'All'
        }].concat(response.OrganizationList);
      });
  }

  downloadReport() {
    const reportModel = {
      status: this.status,
      sourceOU: this.sourceOU,
      subject: this.subject,
      requestDateRangeFrom: this.dateFrom,
      requestDateRangeTo: this.dateTo,
      attendedBy: this.attendedBy,
      priority: this.priority,
      smartSearch: this.smartSearch,
      userID: this.currentUser.id
    };
    let date = new Date,
    cur_date = date.getDate() +'-'+(date.getMonth()+1)+'-'+date.getFullYear();
    this.maintenanceService.getReport(reportModel)
    .subscribe((resultBlob: Blob)=>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = this.lang ==='ar' ? 'الصيانة_'+cur_date+'.xlsx' : 'Maintenance_'+cur_date+'.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
      this.bsModalRef.hide();
    });
  }
}
