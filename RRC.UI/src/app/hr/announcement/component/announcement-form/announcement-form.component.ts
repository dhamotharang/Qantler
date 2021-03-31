import { ArabicDataService } from 'src/app/arabic-data.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AnnouncementService } from 'src/app/hr/announcement/service/announcement.service';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { Announcement } from '../../model/announcement.model';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { CommonService } from 'src/app/common.service';
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: 'app-announcement-form',
  templateUrl: './announcement-form.component.html',
  styleUrls: ['./announcement-form.component.scss']
})
export class AnnouncementFormComponent implements OnInit {
  @ViewChild('tinyDetail') tinyDetail: ElementRef;
  announcementType: any = null;
  Departments =[];
  announcementTypes: Object[] = [];
  announcementDate: Date;
  sourceou: string = '';
  sourceName: string = '';
  description: string = '';
  editMode: boolean = true;
  id:string = '';
  action: string = '';
  comments: string = '';
  announcement: Announcement = new Announcement();
  valid: boolean = false;
  historyLogs:[] = [];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  bsModalRef: BsModalRef;
  message: string;
  refId:string;
  status:Number;
  OrgUnitID: Number;
  IsOrgHead:boolean = false;
  isAssigned:boolean = false;
  ifAssignedToMe: boolean = false;
  AssigneeId:any;
  inProgress:boolean = false;
  lang: string;
  popupMsg: string;
  tinyConfig = {
    plugins: 'powerpaste casechange importcss tinydrive searchreplace directionality visualblocks visualchars fullscreen table charmap hr pagebreak nonbreaking toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons',
    language: this.common.language != 'English' ? "ar" : "en",
    menubar: 'file edit view insert format tools table tc help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media pageembed template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment',
    toolbar_drawer: 'sliding',
    directionality: this.common.language != 'English' ? "rtl" : "ltr"
  };
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };

  constructor(
    private annoucementService: AnnouncementService,
    private route: ActivatedRoute,
    public router: Router,
    public utils: UtilsService,
    public modalService: BsModalService,
    private common: CommonService,
    public arabic: ArabicDataService
    ) {
      this.lang = this.common.currentLang;
    }

  ngOnInit() {
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.id = this.route.snapshot.paramMap.get("id");
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 0;
    this.annoucementService.getAnnoucement(0, this.currentUser.id).subscribe((res: any) => {
      this.Departments = res.OrganizationList;
    });
    if (this.id) {
      this.editMode = false;
      this.getAnnoucement();
      if(this.lang === 'en'){
        this.common.breadscrumChange('HR', 'Announcement Requests', '');
      } else {
        this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('announcementrequests'), '');
      }
    } else {
      this.editMode = true;
      this.initiateForm();
      if (this.lang === 'en') {
        this.common.breadscrumChange('HR', 'Announcement Requests', '');
      } else {
        this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('announcementrequests'), '');
      }
    }
    this.description= "";
  }

  initiateForm() {
    this.announcementDate = new Date();
    this.annoucementService.getAnnoucement(0, this.currentUser.id).subscribe((announcement: any) => {
      this.announcementTypes = announcement.AnnouncementTypeAndDescription;
    });
    this.sourceou = this.currentUser.username;
    this.sourceName = this.currentUser.department;
    this.action = 'Submit';

  }

  onTypeSelect() {
    if (this.announcementType) {
      let types = _.filter(this.announcementTypes, { 'AnnouncementTypeID': this.announcementType});
      var type: any = types[0];
      this.description = this.lang === 'en' ? type.Description : type.DescriptionAr;
      this.description = this.description == null ? "" : this.description;
    }
    this.validate();
  }

  onClear() {
    this.description = '';
    this.validate();
  }

  validate() {
    this.valid = true;
    this.description = this.description.replace(/<[^>]*>/g, '');
    this.description = this.description.replace(/&nbsp;/g, '');
    if (this.utils.isEmptyString(this.announcementType) || this.utils.isEmptyString(this.description)) {
      this.valid = false;
      // if(event && (event['keyCode'] == 8 || event['keyCode'] == 46) && this.description){
      //   if(this.description.length == 1){
      //     this.valid = false;
      //   }
      // }
    }
    return this.valid;
  }

  getAnnoucement() {
    this.annoucementService.getAnnoucement(this.id, this.currentUser.id)
      .subscribe((announcement: any) => {
        this.announcementDate = new Date(announcement.CreatedDateTime);
        // this.sourceou = announcement.SourceOU;
        // this.sourceName = announcement.SourceName;
        this.getSouceName(announcement.SourceName,announcement.SourceOU);
        this.announcementType = parseInt(announcement.AnnouncementType);
        this.description = announcement.AnnouncementDescription;
        this.tinyDetail.nativeElement.insertAdjacentHTML('beforeend', this.description);
        this.historyLogs = announcement.HistoryLog;
        this.announcementTypes = announcement.AnnouncementTypeAndDescription;
        this.refId = announcement.ReferenceNumber;
        this.status = announcement.Status;
        this.AssigneeId = announcement.AssigneeId;
        this.checkIfAssignedToMe();
      });
  }

  async getSouceName(UserID,DepID) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.sourceou= this.Departments.find(x=> x.OrganizationID == DepID).OrganizationUnits;  
       this.sourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });
    
   }

  checkIfAssignedToMe() {
    if (this.AssigneeId && this.AssigneeId.length > 0) {
      this.isAssigned = true;
      this.AssigneeId.forEach((assignee:any) => {
        if (assignee.AssigneeId == this.currentUser.id) {
          this.ifAssignedToMe = true;
        }
      });
    }
  }

  onSubmit() {
    this.validate();
    if (this.valid) {
      this.inProgress = true;
      this.announcement.CreatedDateTime = this.announcementDate;
      this.announcement.SourceOU = this.currentUser.DepartmentID;
      this.announcement.SourceName = this.currentUser.UserID;
      this.announcement.AnnouncementType = this.announcementType;
      this.announcement.AnnouncementDescription = this.description;
      this.announcement.Action = this.action;
      this.announcement.Comments = this.comments;
      this.announcement.CreatedBy = this.currentUser.id;
      this.annoucementService.createAnnouncement(this.announcement)
        .subscribe((response:any) => {
          if (response.AnnouncementID) {
            if (this.lang === 'en') {
              this.message = "Announcement Request Submitted Successfully";
            } else {
              this.message = "تم إنشاء طلب إعلان بنجاح";
            }
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
              newSubscriber.unsubscribe();
              this.router.navigate(['/app/hr/dashboard']);
            });
            this.resetFields();
          }
          this.inProgress = false;
        });
    }
  }

  resetFields() {
    this.announcementType = null;
    this.description = '';
    this.valid = false;
  }

  onAssigneTo() {
    let modalTitleString;
    if (this.lang == 'ar') {
      this.popupMsg = this.arabicfn('announcementassignedsuccess');
      modalTitleString = this.arabicfn('announcementassignto');
    } else {
      this.popupMsg = "Announcement Assigned Successfully";
      modalTitleString = "Assign To";
    }
    const initialState = {
      id: this.id,
      ApiString: "/Announcement",
      message: this.popupMsg,
      ApiTitleString: modalTitleString,
      redirectUrl: '/app/hr/dashboard'
    };
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
  }

  onClose() {
    this.message = "Announcement Closed Successfully";
    if (this.lang == 'ar') {
      this.message = this.arabicfn('announcementclosedsuccess');
    }
    this.updateAction('Close');
  }

  onAssigneToMe() {
    this.message = "Announcement Assigned Successfully";
    if (this.lang == 'ar') {
      this.message = this.arabicfn('announcementassignedsuccess');
    }
    this.updateAction('AssignToMe')
  }

  updateAction(action: string) {
    const dataToUpdate = [{
      "value": action,
      "path": "Action",
      "op": "replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "replace"
    }, {
      "value": "",
      "path": "Comments",
      "op": "replace"
    }];
   this.update(dataToUpdate);
  }

  update(dataToUpdate: any) {
    this.inProgress = true;
    this.annoucementService.updateAnnouncement(this.id, dataToUpdate)
      .subscribe((response:any) => {
        if (response.AnnouncementID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/app/hr/dashboard']);
          });
        }
        this.inProgress = false;
      });
  }

  hisLog(status:string) {
    let sts = status.toLowerCase();
    if(this.common.currentLang != 'ar'){
      if (sts == 'submit') {
        return status + 'ted By';
      } else if (sts == 'reject' || sts == 'redirect') {
        return status + 'ed By';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        return 'Assigned By'
      } else {
        return status + 'd By';
      }
    }else if(this.common.currentLang == 'ar'){
      let arabicStatusStr = '';
      if (sts == 'reject' || sts == 'redirect') {
        arabicStatusStr = sts+'edby';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        arabicStatusStr = 'announcementassignedby';
      } else if(sts == 'submit' || sts == 'resubmit'){
        arabicStatusStr = 'salarysubmittedby';
      } else if(sts == 'close'){
        arabicStatusStr = 'salaryclosedby';
      } else {
        arabicStatusStr = sts+'dby';
      }
      return this.common.arabic.words[arabicStatusStr];
    }
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }
}
