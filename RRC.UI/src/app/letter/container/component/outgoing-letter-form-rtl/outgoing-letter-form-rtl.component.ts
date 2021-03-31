import { Component, OnInit } from '@angular/core';
import { OutgoingLetterFormComponent } from '../outgoing-letter-form/outgoing-letter-form.component';

@Component({
  selector: 'app-outgoing-letter-form-rtl',
  templateUrl: './outgoing-letter-form-rtl.component.html',
  styleUrls: ['../outgoing-letter-form/outgoing-letter-form.component.scss']
})
export class OutgoingLetterFormRtlComponent extends OutgoingLetterFormComponent {

  arabic(word) {
    return this.common.arabic.words[word];
  }

  // constructor() { }

  // ngOnInit() {
  // }

}