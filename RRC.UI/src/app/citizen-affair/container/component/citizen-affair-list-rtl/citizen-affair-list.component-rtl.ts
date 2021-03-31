import { Component, OnInit, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { Router } from '@angular/router';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CitizenAffairService } from '../../../service/citizen-affair.service';
import { DatePipe } from '@angular/common';
import { CitizenReportModalComponent } from 'src/app/modal/citizen-report-modal/citizen-report-modal.component';
import { CitizenAffairListComponent } from '../citizen-affair-list/citizen-affair-list.component';
@Component({
  selector: 'app-citizen-affair-list-rtl',
  templateUrl: './citizen-affair-list.component-rtl.html',
  styleUrls: ['./citizen-affair-list.component-rtl.scss']
})
export class CitizenAffairListComponentRTL extends CitizenAffairListComponent {

}
