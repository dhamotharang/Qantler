import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {GeneralService} from '../../services/general.service';
import {ProfileService} from '../../services/profile.service';
import {URLService} from '../../services/url.service';

@Component({
  selector: 'app-community-threshold',
  templateUrl: './community-threshold.component.html',
  styleUrls: ['./community-threshold.component.css']
})
export class CommunityThresholdComponent implements OnInit {

  approval: number;
  rejection: number;

  constructor(private Generalservice: GeneralService, private profileService: ProfileService,
    private messageService: MessageService, private spinner: NgxSpinnerService,
    private confirmationService: ConfirmationService) {
  }

  ngOnInit() {
    this.approval = 0;
    this.rejection = 0;
    this.getData();
  }

  getData() {
    this.spinner.show();
    this.Generalservice.GetApprovalCount().subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.approval = res.returnedObject.approveCount;
        this.rejection = res.returnedObject.rejectCount;
      } else {
        this.messageService.add({severity: 'success', summary: 'Failed To Load Data', key: 'toast'});
      }
      this.spinner.hide();
    }, (error) => {
      this.messageService.add({severity: 'success', summary: 'Failed To Load Data', key: 'toast'});
      this.spinner.hide();
    });
  }

  UpdateData() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to update this data?',
      accept: () => {
        this.spinner.show();
        this.Generalservice.UpdateApprovalCount({
          'approveCount': this.approval,
          'rejectCount': this.rejection,
          'userId': this.profileService.user.id
        }).subscribe((res: any) => {
          if (res.hasSucceeded) {
            this.messageService.add({severity: 'success', summary: 'Successfully Updated Data', key: 'toast'});
            this.getData();
          } else {
            this.messageService.add({severity: 'success', summary: 'Failed To Update Data', key: 'toast'});
          }
          this.spinner.hide();
        }, (error) => {
          this.messageService.add({severity: 'success', summary: 'Failed To Update Data', key: 'toast'});
          this.spinner.hide();
        });
      }
    });
  }

}
