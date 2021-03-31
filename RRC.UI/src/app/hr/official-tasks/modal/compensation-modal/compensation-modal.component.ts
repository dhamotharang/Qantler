import { Component, OnInit, Renderer2, Inject } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Compensation } from '../../model/compensation.model';
import { OfficialTaskService } from '../../services/official-task.service';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-compensation-modal',
  templateUrl: './compensation-modal.component.html',
  styleUrls: ['./compensation-modal.component.scss']
})
export class CompensationModalComponent implements OnInit {
  lang: string;
  arWords: any;
  compensationUrl:string = '/Compensation/';

  hospitality: any;
  taskDescription: string;
  valid:boolean = false;
  inProgress:boolean = false;
  compensation:Compensation = new Compensation();
  OfficialTaskID:any;
  comments:string = "";
  compensationCreated: boolean = false;
  currentUser: any = JSON.parse(localStorage.getItem('User'));

  constructor(public common: CommonService,
    public bsModalRef: BsModalRef,
    public officialTaskService: OfficialTaskService,
    public modalService: BsModalService,
    public router: Router,
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2,
    public arabicService: ArabicDataService) {
    this.lang = this.common.currentLang;
    this.arWords = this.arabicService.words;
  }
  officialTaskUrl:string = '/OfficialTask/';

  ngOnInit() {
  }

  validate() {
    let flag = false;
    if (!this.hospitality || !this.taskDescription || (this.taskDescription && !this.taskDescription.trim())) {
      flag = true;
    }
    return flag;
  }

  submitCompensation() {
    this.inProgress = true;
    const dataToUpdate = [
      {
        "value": 'MarkasComplete',
        "path": "Action",
        "op": "Replace"
      }, {
        "value": this.comments,
        "path": "Comments",
        "op": "Replace",
      }, {
        "value": this.currentUser.id,
        "path": "UpdatedBy",
        "op": "Replace"
      }, {
        "value": new Date(),
        "path": "UpdatedDateTime",
        "op": "Replace"
      }
    ];
    this.compensation.CompensationDescription = this.taskDescription;
    this.compensation.NeedCompensation = this.hospitality === 'yes' ? true : false;
    this.officialTaskService.update(this.officialTaskUrl, this.compensation.OfficialTaskID, dataToUpdate)
    .subscribe((response:any) => {
      if (response) {
        this.bsModalRef.hide();
        this.officialTaskService.create(this.compensationUrl, this.compensation)
        .subscribe((response:any) => {
          this.inProgress = false;
          if (response.CompensationID) {
            this.compensationCreated = true;
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.lang === 'ar' ? this.arWords.compReqSubSuc : "Compensation Request Submitted Successfully";
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.closeModal();
              this.renderer.removeClass(this.document.body, 'modal-open');
              this.router.navigate([this.lang + '/app/hr/dashboard']);
            });
          }
          this.inProgress = false;
        });
      }
    })
    return;
  }

  closeModal() {
    this.bsModalRef.hide();
    // this.modalService.hide(1);
  }
}
