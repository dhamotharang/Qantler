import { Component, OnInit, ElementRef, ViewChild, TemplateRef } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { CitizenAffairService } from '../../../service/citizen-affair.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpEventType } from '@angular/common/http';
import { TabHeadingDirective, BsModalRef, BsModalService, BsDatepickerConfig } from 'ngx-bootstrap';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { ModalComponent } from 'src/app/modal/modalcomponent/modal.component';
import { CitizenAffairCreateComponent } from '../citizen-affair-create/citizen-affair-create.component';

@Component({
  selector: 'app-citizen-affair-create-rtl',
  templateUrl: './citizen-affair-create.component-rtl.html',
  styleUrls: ['./citizen-affair-create.component-rtl.scss']
})
export class CitizenAffairCreateComponentRTL extends CitizenAffairCreateComponent {

}