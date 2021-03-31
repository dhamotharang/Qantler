import {Component, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {MessageService} from 'primeng/api';
import {NgxSpinnerService} from 'ngx-spinner';
import {KeycloakService} from 'keycloak-angular';
import {environment} from '../../../environments/environment';
import {Title} from '@angular/platform-browser';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html'
})
export class AccountSettingsComponent implements OnInit {
  checked: boolean;

  constructor(private titleService: Title, private profileService: ProfileService, private messageService: MessageService, private spinner: NgxSpinnerService, private keyCloak: KeycloakService) {
  }

  ngOnInit() {
    this.checked = false;
    this.titleService.setTitle('Account Settings | UAE - Open Educational Resources');
  }

  deleteAccount() {
    if (this.profileService.userId) {
      this.spinner.show(undefined, {color: this.profileService.themeColor});
      this.profileService.DeActiveProfile({
        userId: this.profileService.userId,
        status: false
      }).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.messageService.add({severity: 'success', summary: 'Request Successfully Processed', key: 'toast'});
          this.keyCloak.logout(environment.clientUrl);
        } else {
          // this.translate.get(res.message).subscribe((translation) => {             this.messageService.add({severity: 'error', summary: translation, key: 'toast', life: 5000});           });
        }
        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        this.messageService.add({severity: 'error', summary: 'Failed to process Request', key: 'toast'});
      });
    }
  }

}
