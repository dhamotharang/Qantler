import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Subscription} from 'rxjs';
import {WCMService} from '../../services/wcm.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {GeneralService} from '../../services/general.service';
import {MessageService} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import {Title} from '@angular/platform-browser';
import {environment} from '../../../environments/environment';
import {ProfileService} from '../../services/profile.service';

@Component({
  selector: 'app-wcm',
  templateUrl: './wcm.component.html'
})
export class WCMComponent implements OnInit {
  private sub: Subscription;
  slug: string;
  content: any;
  content_Ar: any;
  contactForm: FormGroup;
  contactFormSubmitted: boolean;
  user: any;

  constructor(private profileService: ProfileService, private titleService: Title, private translate: TranslateService, private router: Router, private generalService: GeneralService, private route: ActivatedRoute, private WcmService: WCMService, private messageService: MessageService, private spinner: NgxSpinnerService) {
  }

  ngOnInit() {
    this.user = null;
    this.contactForm = new FormGroup({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      email: new FormControl(null, Validators.compose([Validators.required, Validators.email])),
      telephone: new FormControl(null),
      subject: new FormControl(null, Validators.required),
      message: new FormControl(null, Validators.required),
      url: new FormControl(environment.adminUrl + 'contact-queries', Validators.required),
    });
    this.user = this.profileService.user;
    this.patchForm();
    this.sub = this.profileService.getUserDataUpdate().subscribe(() => {
      this.user = this.profileService.user;
      this.patchForm();
    });
    this.content = null;
    this.content_Ar = null;
    this.route.params.subscribe(params => {
      if (params['wcm']) {
        if (params['wcm'] === 'about-manara-platform') {
          this.slug = 'About Organisation';
        } else {
          this.slug = params['wcm'].replace(/-/g, ' ');
        }
        this.content = null;
        this.getData();
      }
    });
  }

  patchForm() {
    if (this.user) {
      this.contactForm.patchValue({
        firstName: this.user.firstName,
        lastName: this.user.lastName,
        email: this.user.email
      });
    }
  }

  getData() {
    this.contactFormSubmitted = false;
    const categoryId: number =1;
    this.spinner.show(undefined, {color: this.profileService.themeColor});
    this.WcmService.getAllList(categoryId).subscribe((res: any) => {
      if (res.hasSucceeded) {
        const WCMs = res.returnedObject;
        let found = false;
        WCMs.forEach((item) => {
          if (item.pageName.toLowerCase() === this.slug.toLowerCase()) {
            found = true;
            this.slug = item.pageName;
            if (item.pageName === 'About Organisation') {
              this.slug = 'About “Manara” platform';
            }
            this.titleService.setTitle(this.slug + ' | UAE - Open Educational Resources');
            this.WcmService.getContent(item.id).subscribe((response: any) => {
              if (response.hasSucceeded && response.returnedObject.length > 0) {
                this.content = response.returnedObject[0].pageContent;
                this.content_Ar = response.returnedObject[0].pageContent_Ar;
              } else {
                this.content = '<p class="text-center">No Content Available</p>';
                this.content_Ar = '<p class="text-center">No Content Available</p>';
              }
              this.spinner.hide();
            }, (error) => {
              this.spinner.hide();
            });
          }
          this.spinner.hide();
        }, (error) => {
          this.spinner.hide();
        });
        if (!found) {
          this.router.navigateByUrl('code/404');
        }
      }
    });
  }

  submitContactForm(Form) {
    this.contactFormSubmitted = true;
    if (Form.valid) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.generalService.postContactUs(Form.value).subscribe((response) => {
        if (response.hasSucceeded) {
          this.contactFormSubmitted = false;          
          this.translate.get(response.message).subscribe((translation) => {
            this.messageService.add({severity: 'success', summary: translation, key: 'toast', life: 5000});
          });          
          // this.contactForm.controls['firstName'].setValue('');
          // this.contactForm.controls['lastName'].setValue('');
          // this.contactForm.controls['telephone'].setValue('');
          // this.contactForm.controls['email'].setValue('');
          // this.contactForm.controls['subject'].setValue('');
          // this.contactForm.controls['message'].setValue('');
          this.ngOnInit();
        } else {
          this.translate.get(response.message).subscribe((translation) => {
            this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});
          });
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.translate.get('Failed to query resources').subscribe((msg) => {
          this.messageService.add({severity: 'error', summary: msg, key: 'toast', life: 5000});
        });
      });
    }
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

}
