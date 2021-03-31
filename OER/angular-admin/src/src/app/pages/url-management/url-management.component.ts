import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {ProfileService} from '../../services/profile.service';
import {URLService} from '../../services/url.service';
import { Paginator } from 'primeng/components/paginator/paginator';
import _ from 'lodash'

@Component({
  selector: 'app-url-management',
  templateUrl: './url-management.component.html',
  styleUrls: ['./url-management.component.css']
})
export class UrlManagementComponent implements OnInit {
  requests: any[];
  approvedRequests: any[];
  showRejectReasonForm: boolean;
  rejectReason: string;
  rejectRequest: any;
  type: string;
  searchKeyword  : string = "";
  sortType: string = 'desc';
  sortField : number = 0;
  Clickfield :string = '';
  spanColNo : any;
  searchbackup : any;

  @ViewChild('pp') paginator: Paginator;
 

  constructor(
    private URLservice: URLService, private route: ActivatedRoute,
    private spinner: NgxSpinnerService, private profileService: ProfileService,
    private messageService: MessageService, private confirmationService: ConfirmationService) {
  }

  ngOnInit() {
    this.type = 'requests';
    this.requests = [];
    this.showRejectReasonForm = false;
    this.rejectReason = null;
    this.rejectRequest = null;
    this.route.params.subscribe((params) => {
      if (params['type']) {
        this.type = params['type'];
      }
      this.getData();
    });
  }

  getData() {
    if (this.type === 'requests') {
      this.getURLRequestList();
    } else {
      this.getURLApprovedList();
    }
  }

  search(){
    var userdata = [];
    var type = this.type;
    if (type === 'requests') {
      this.requests = this.searchbackup;
      userdata = this.requests;
    }
    else {
      this.approvedRequests = this.searchbackup;
      userdata = this.approvedRequests;
    }
   // var userdata = this.users;
  
    debugger;
    const versiondata = [];
    var s = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    if(s == "" ) {
      
      this.searchKeyword = "";
      this.Clickfield = "";
      if (this.type === 'requests') {
        this.getURLRequestList();
      } else {
        this.getURLApprovedList();
      }
    } else {
      userdata &&  userdata.length
      ?  userdata.map(function mm(i) {
        if (type === 'requests') {
          if (i.url.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.requestedOn === s) {
            versiondata.push(i);
           }
        else if (i.rejectedReason === null && i.isApproved === false) {
          if("New Request".toLowerCase().includes(s.toLowerCase()))
          {
            versiondata.push(i);
           }
        }
          
           else if (!(i.rejectedReason === null && i.isApproved === false)) {
           var data = "Rejected, Reason :"+i.rejectedReason+"";
           if(data.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           }
        }
        else {
          if (i.url.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
        else if (i.requestedBy.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.requestedOn == s) {
            versiondata.push(i);
           }
           else if (i.verifiedBy.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if (i.verifiedOn == s) {
            versiondata.push(i);
           }
        }
       
        }) : [];
        userdata = versiondata;
        if (type === 'requests') {
          this.requests = userdata; 
        }
        else {
         this.approvedRequests = userdata;
        }
    }
  }

  Clickingevent(event,columnNo) {
    debugger;
    var userdata = [];
    var type = this.type;
    if (type === 'requests') {
      userdata = this.requests;
    }
    else {
      userdata = this.approvedRequests;
    }
    if(this.Clickfield == event.target.id) {
      this.sortType = this.sortType == 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.sortType = 'asc'
    }
    this.sortField = columnNo;
   
    this.spanColNo = this.sortType+"-"+columnNo;
    var sortedArray = _.sortBy(userdata, function(patient) {
      if (type === 'requests') {
        if(event.target.id == 'url'){
          return patient.url;
        }
        else if(event.target.id == 'status'){
          var data = "";
          if (!(patient.rejectedReason === null && patient.isApproved === false)) {
            data = "Rejected, Reason :"+patient.rejectedReason+"";
          } else {
            data = "New Request"
          }
          return data.toLowerCase();
        }
        else if(event.target.id == 'requestedon'){
          return patient.requestedOn;
        }
      }
      else {
        if(event.target.id == 'url'){
          return patient.url;
        }
        else if(event.target.id == 'reqby'){
          return patient.requestedBy;
        }
        else if(event.target.id == 'reqon'){
          return patient.requestedOn;
        }
        else if(event.target.id == 'verby'){
          return patient.verifiedBy;
        }
        else if(event.target.id == 'veron'){
          return patient.verifiedOn;
        }
      }
  });
  if(this.sortType == 'desc') {
    sortedArray =   sortedArray.reverse();
  }
  if (type === 'requests') {
    this.requests = sortedArray;
  }
  else {
    this.approvedRequests = sortedArray;
  }
}

  clearSearch(){
    
    this.searchKeyword = "";
    this.Clickfield = "";
    this.getData();
  }

  getURLRequestList() {
    this.URLservice.getAllRequests().subscribe((res: any) => {
      this.requests = res.returnedObject;
      this.searchbackup = res.returnedObject;
    });
  }

  getURLApprovedList() {
    this.URLservice.getAllApproved().subscribe((res: any) => {
      this.approvedRequests = res.returnedObject;
      this.searchbackup = res.returnedObject; 
    });
  }

  ApproveURL(request) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to approve this url?',
      accept: () => {
        this.spinner.show();
        this.URLservice.updateRequest({
          Id: request.id,
          VerifiedBy: this.profileService.user.id,
          IsApproved: true,
          emailUrl: environment.userClientUrl + '/dashboard'
        }, request.id).subscribe((res: any) => {
          if (res.hasSucceeded) {
            this.getData(); 
            this.messageService.add({severity: 'success', summary: 'URL Approved successfully', key: 'toast',life: 5000});
          } else {
            this.messageService.add({severity: 'error', summary: 'Failed to Approve URL', key: 'toast',life: 5000});
          }
          this.spinner.hide();
        }, (error) => {
          this.messageService.add({severity: 'error', summary: 'Failed to Approve URL', key: 'toast',life: 5000});
          this.spinner.hide();
        });
      }
    });
  }

  rejectURL(request) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to reject this url?',
      accept: () => {
        this.rejectRequest = request;
        this.showRejectReasonForm = true;
      }
    });
  }

  submitRejectRequest() {
    if (this.rejectReason) {
      this.spinner.show();
      this.URLservice.updateRequest({
        Id: this.rejectRequest.id,
        RejectedReason: this.rejectReason,
        VerifiedBy: this.profileService.user.id,
        IsApproved: false,
        emailUrl: environment.userClientUrl + '/dashboard'
      }, this.rejectRequest.id).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.getData();
          this.cancelRejectRequest();
          this.messageService.add({severity: 'success', summary: 'URL Rejected successfully', key: 'toast',life: 5000});
        } else {
          this.messageService.add({severity: 'error', summary: 'Failed to Reject URL', key: 'toast',life: 5000});
        }
        this.spinner.hide();
      }, (error) => {
        this.messageService.add({severity: 'error', summary: 'Failed to Reject URL', key: 'toast',life: 5000});
        this.spinner.hide();
      });
    }
  }
   cancelRejectRequest() {
    this.showRejectReasonForm = false;
    this.rejectReason = null;
    this.rejectRequest = null;
  }

  deleteUrl(url) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this url?',
      accept: () => {
        this.spinner.show();
        this.URLservice.deleteRequest(url.id).subscribe((res: any) => {
          if (res.hasSucceeded) {
            this.getData();
            this.messageService.add({severity: 'success', summary: 'URL Deleted successfully', key: 'toast',life: 5000});
          } else {
            this.messageService.add({severity: 'error', summary: 'Failed to Delete URL', key: 'toast',life: 5000});
          }
          this.spinner.hide();
        }, (error) => {
          this.messageService.add({severity: 'error', summary: 'Failed to Delete URL', key: 'toast',life: 5000});
          this.spinner.hide();
        });
      }
    });
  }

}
