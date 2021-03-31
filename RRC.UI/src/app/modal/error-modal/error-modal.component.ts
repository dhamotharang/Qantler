import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.scss']
})

export class ErrorModalComponent implements OnInit {
  message: any;
  lang: string;
  arWords: any;

  constructor(public common: CommonService,
    public bsModalRef: BsModalRef,
    public modalService: BsModalService,
    public arabicService: ArabicDataService) {
      this.lang = this.common.currentLang;
      this.arWords = this.arabicService.words;
    }

  ngOnInit() {
  }

}
