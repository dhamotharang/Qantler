import { Component, OnInit } from '@angular/core';
import { IncomingLetterFormComponent } from '../incoming-letter-form/incoming-letter-form.component';

@Component({
  selector: 'app-incoming-letter-form-rtl',
  templateUrl: './incoming-letter-form-rtl.component.html',
  styleUrls: ['../incoming-letter-form/incoming-letter-form.component.scss']
})
export class IncomingLetterFormRtlComponent extends IncomingLetterFormComponent{

  arabic(word) {
    return this.common.arabic.words[word];
  }

}
