import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';
import { AuthService } from 'src/app/auth/auth.service';
import { EndPointService } from 'src/app/api/endpoint.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { NotificationModalComponent } from 'src/app/modal/notification-modal/notification-modal.component';
import * as moment from 'moment';
import { LayoutServiceService } from '../../../layout-service.service'

@Component({
  selector: 'app-page-header-rtl',
  templateUrl: './page-header.component.rtl.html',
  styleUrls: ['./page-header.component.rtl.scss'],
  host: {
    '(document:click)': 'onClick($event)',
  }
})
export class PageHeaderComponentRTL implements OnInit, OnDestroy {
  bsModalRef: BsModalRef;
  cur_user: string;
  userDetail: any;
  dataModel: any;
  down = false;
  languageList = ['English', 'عربي'];
  config = {
    displayKey: 'description',
    height: 'auto',
    placeholder: 'Select',
    noResultsFound: 'No results found!',
    searchPlaceholder: 'Search',
    searchOnKey: 'name',
  };
  notificationModalConfig = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  home_banner: boolean = false;
  show = false;
  text: string;
  currentSession: string;
  language: any = this.common.language;
  showContactFormrtl: boolean = false;
  contactType = 'internal';
  userProfileLink: string = '';
  subscribeShowContact: any;
  showSmartSearchrtl: boolean = false;
  empProfileImg: string = 'assets/home/user_male.png';
  isLogin: boolean;
  showNotification: boolean;
  currentUserID: any;
  notificationDetails = [];
  notificationCount: any;
  punchInDateTime: any;
  punchInTime: any;
  punchOutTime: any;
  isLatePunchIn: any;
  isEarlyPunchOut: any;
  employeeId: any;
  employeeResult: any;

  constructor(public router: Router, public authService: AuthService, private modalService: BsModalService, public route: ActivatedRoute, public common: CommonService, private endpoint: EndPointService,public service: LayoutServiceService) {
    this.isLogin = (this.common.currentScreen == 'login') ? true : false;
    if (!this.isLogin) {
      this.checkLogin();
      this.userDetail = JSON.parse(localStorage.getItem('User'));
      this.cur_user = this.userDetail.username;
      this.currentUserID = this.userDetail.id;
      // this.changeUserDisplayNametoArabic();
      this.userProfileLink = '/' + this.common.currentLang + '/app/hr/employee/view/' + this.userDetail.id;
      if (this.userDetail.AttachmentGuid && this.userDetail.AttachmentName) {
        // this.empProfileImg = this.endpoint.imageHostingURL + '/' + this.userDetail.AttachmentGuid + '/' + this.userDetail.AttachmentName;
        this.empProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.userDetail.AttachmentName + '&guid=' + this.userDetail.AttachmentGuid;
      }
      if (this.router.url) {
        this.home_banner = (this.router.url === '/ar/home') ? true : false;
      }
    }
    this.language = this.common.getCookie();
    if (this.language = '????') {
      this.language = this.common.language;
    }
  }

  // onClick($event){
  //   this.show = false;
  // }

  private wasInside = false;

  @HostListener('click')
  clickInside() {
    this.wasInside = true;
  }

  @HostListener('document:click')
  clickout() {
    if (!this.wasInside) {
      this.show = false;
    }
    this.wasInside = false;
  }

  ngOnInit() {
    this.language=this.languageList[1];
    this.showNotification = false;
    var today = new Date()
    var curHr = today.getHours()
    if (curHr < 12) {
      this.currentSession = 'صباح الخير';
    } else if (curHr < 18) {
      this.currentSession = 'مساء الخير';
    } else {
      this.currentSession = 'مساء الخير';
    }
    this.subscribeShowContact = this.common.isShowContact$.subscribe(event => {
      this.contactType = 'external';
      this.showContact();
    });
    this.getNotification();
    // if(localStorage.getItem('inTime') != null){
      if(this.userDetail.InTime != null){
        var dateStr = moment(this.userDetail.InTime,  'YYYY-MM-DDTHH:mm:ss+-HH:mm:ss'); 
        this.punchInDateTime = dateStr.toDate();
        var date = new Date(this.punchInDateTime);
        var defaultPunchInTime = new Date(date.setHours(8,15,0,0));
        if(this.punchInDateTime > defaultPunchInTime){
          this.isLatePunchIn = true;
          console.log('late punch in');
        }
        let hours = ((this.punchInDateTime.getHours() + 11) % 12 +1);
        console.log('hours: '+hours);
        this.punchInTime = ('0' + hours).slice(-2) + ':' + ('0' + this.punchInDateTime.getMinutes()).slice(-2);
      }
      // if(localStorage.getItem('outTime') != null){
      if(this.userDetail.OutTime != null){
        var TodateStr = moment(this.userDetail.OutTime,  'YYYY-MM-DDTHH:mm:ss+-HH:mm:ss'); 
        this.punchOutTime = TodateStr.toDate();
        var defaultPunchOutTime = new Date(this.punchInDateTime.setHours(this.punchInDateTime.getHours()+8));
        if(this.punchOutTime < defaultPunchOutTime){
          this.isEarlyPunchOut = true;
          console.log('punch out early');
        }
        let TOhours = ((this.punchOutTime.getHours() + 11) % 12 +1);
        console.log('TOhours: '+TOhours);
        this.punchOutTime = ('0' + TOhours).slice(-2) + ':' + ('0' + this.punchOutTime.getMinutes()).slice(-2);
      }
      this.common.IsShowSearchrtl.subscribe(data => {
        if (data != undefined) {
        this.showSmartSearchrtl = data;
        }
        })
        this.common.IsContactSearchrtl.subscribe(data => {
        if (data != undefined) {
        this.showContactFormrtl = data;
        }
        })
  }


  routeHome() {
    if(!this.isLogin) {
      this.router.navigate(['/en/home']);
    }else {
      this.router.navigate(['/login']);
    }
  }

  logout() {
    this.authService.clearUserSession('logout');
  }

  setLanguage(lang) {
    this.language = lang;
    this.languageChange();
   
  }

  languageChange() {
    this.common.setCookie('language', this.language);
    var urls = this.router.url.split('/'),
      url = '';
    url = this.routingProcess(urls, this.language);
    // this.router.navigate([url]);
    this.service.getEmpDetails(this.currentUserID, this.currentUserID).subscribe((data: any) => {
      this.employeeResult = data;
      this.cur_user = this.employeeResult.EmployeeName;

      var existing = localStorage.getItem("User");
      existing = existing ? JSON.parse(existing) : {};
      existing['username'] = this.cur_user;
      existing['DisplayName'] = this.cur_user;
      //UserJson.username = 
      localStorage.setItem("User", JSON.stringify(existing));
      this.router.navigate([url]);
    }); 
    if (this.language == 'English')
      this.common.languageChange('en');
    else
      this.common.languageChange('ar');

  }

  routingProcess(urls, lang) {
    var url = '';
    if (lang == 'English') {
      urls.map(res => {
        if (res != '' && res != 'en' && res != 'ar')
          url += '/' + res;
      });
      url = '/en' + url;
    } else {
      urls.map(res => {
        if (res != '' && res != 'en' && res != 'ar')
          url += '/' + res;
      });
      url = '/ar' + url;
    }
    return url;
  }
  showContact() {
    this.showContactFormrtl = true;
  }

  closeContact() {
    this.showContactFormrtl = false;
  }
  toggleSmartSearch() {
    this.showSmartSearchrtl = !this.showSmartSearchrtl;
  }

  goToAdminSettings() {
    this.router.navigate([`/${this.common.currentLang}/app/admin/admin-settings`]);
  }

  ngOnDestroy() {
    this.subscribeShowContact.unsubscribe();
    this.common.IsShowSearchrtl.next(false)
    this.common.IsShowSearchrtl.next(false)
  }

  viewNotification(ID){
    this.showNotification = false;
    this.common.getEachNotification('Home/NotificationRead', ID, this.currentUserID).subscribe((detail: any) =>{
      if(detail){
        this.bsModalRef = this.modalService.show(NotificationModalComponent, { class: 'modal-lg modal-dialog-centered' });
        this.bsModalRef.content.ReferenceNumber = detail.ReferenceNumber;
        this.bsModalRef.content.ServiceID = detail.ServiceID;
        this.bsModalRef.content.Subject = detail.Subject;
        this.bsModalRef.content.Content = detail.Content;
        console.log("Marked as readed notification: "+detail);
        this.getNotification();
      }
    });
    // this.common.readNotification('Home/NotificationRead', false, ID, this.currentUserID).subscribe(res =>{
    //   if(res){
    //   console.log("Marked as readed notification: "+res);
    //   this.getNotification();
    //   }
    // });
  }

  notification(){
    this.showNotification = ! this.showNotification;
  }

  readAllnotification(){
    this.showNotification = true;
    this.common.readAllNotification('Home/NotificationRead', true, this.currentUserID).subscribe(res =>{
      if(res){
      console.log("Marked All as readed notification: "+res);
      this.getNotification();
      }
    });
  }
  getNotification(){
    if(this.currentUserID){
      this.common.getNotification(1,1000,this.userDetail.id).subscribe((res:any) =>{
        if(res){
          this.notificationCount = res.NotificationCount;
          this.notificationDetails = res.Collection.sort((a,b) => {
          return b.ID - a.ID;
        });
        }
      });
    }
  }
  ngAfterViewInit(): void {
    document.onclick = (args: any) : void => {
      if(args.target.className != "image" && args.target.className != "notification-pop") {
        this.showNotification = false;
      }
      if(args.target.className == 'notification-markread'){
        this.showNotification = true;
      }
    }
  }
  getAction(name){
    if(name == 'SubmissionWorkflow' || name == 'DutyTaskSubmissionWorkflow'){
      return (this.language == 'English') ? 'Submitted': this.common.arabic.words['submitted'];
    }else if(name == 'DutyTaskDeleteWorkflow'){
      return (this.language == 'English') ? 'Deleted': this.common.arabic.words['deleted'];
    }else if(name == 'CloseWorkflow'){
      return (this.language == 'English') ? 'Closed': this.common.arabic.words['closed'];
    }else if(name == 'ApprovalWorkflow'){
      return (this.language == 'English') ? 'Approved': this.common.arabic.words['approved'];
    }else if(name == 'AssignToMeWorkflow' || name == 'AssignWorkflow'){
      return (this.language == 'English') ? 'Assigned': this.common.arabic.words['assigned'];
    }else if(name == 'EscalateWorkflow'){
      return (this.language == 'English') ? 'Escalated': this.common.arabic.words['escalated'];
    }else if(name == 'RedirectWorkflow'){
      return (this.language == 'English') ? 'Redirected': this.common.arabic.words['redirected'];
    }else if(name == 'RejectWorkflow'){
      return (this.language == 'English') ? 'Rejected': this.common.arabic.words['rejected'];
    }else if(name == 'ReopenWorkflow'){
      return (this.language == 'English') ? 'Re-Opened': this.common.arabic.words['reopened'];
    }else if(name == 'ShareWorkflow'){
      return (this.language == 'English') ? 'Shared': this.common.arabic.words['shared'];
    }else if(name == 'ReturnWorkflow'){
      return (this.language == 'English') ? 'Returned for info': this.common.arabic.words['returnedforinfo'];
    }else if(name == 'MeetingCancelWorkflow' || name == 'VehicleCancelWorkflow'){
      return (this.language == 'English') ? 'Cancelled': this.common.arabic.words['cancelled'];
    }else if(name == 'MeetingInvitesWorkflow'){
      return (this.language == 'English') ? 'Invited': this.common.arabic.words['returnedforinfo'];
    }else if(name == 'MeetingRescheduleWorkflow'){
      return (this.language == 'English') ? 'Rescheduled': this.common.arabic.words['returnedforinfo'];
    }else if(name == 'MeetingMomCreatedWorkflow'){
      return (this.language == 'English') ? 'MOM Created': this.common.arabic.words['returnedforinfo'];
    }else if(name == 'DutyTaskCommunicationBoardWorkflow'){
      return (this.language == 'English') ? 'Mentioned': this.common.arabic.words['mentioned'];
    }else if(name == 'DutyTaskCompleteWorkflow'){
      return (this.language == 'English') ? 'Completed': this.common.arabic.words['completed'];
    }else if(name == 'VehicleReleaseConfirmWorkflow'){
      return (this.language == 'English') ? 'Release Confirmed': this.common.arabic.words['releaseconfirmed'];
    }else if(name == 'VehicleReleaseWorkflow'){
      return (this.language == 'English') ? 'Released': this.common.arabic.words['released'];
    }else if(name == 'VehicleReturnWorkflow'){
      return (this.language == 'English') ? 'Returned': this.common.arabic.words['returned'];
    }else if(name == 'VehicleReturnConfirmWorkflow'){
      return (this.language == 'English') ? 'Return Confirmed': this.common.arabic.words['returnconfirmed'];
    }else if(name == 'VehicleReleaseConfirmationRejectWorkflow'){
      return (this.language == 'English') ? 'Release Rejected': this.common.arabic.words['releaserejected'];
    }else if(name == 'FineSubmissionWorkflow'){
      return (this.language == 'English') ? 'Fine Submitted': this.common.arabic.words['finesubmitted'];
    }else if(name == 'VehicleRemainderWorkflow'){
      return (this.language == 'English') ? 'Fine Reminded': this.common.arabic.words['finereminded'];
    }
    else if(name == 'TrainingNotificationWorkflow'){
      return (this.language == 'English') ? 'Certificate needs to be uploaded': this.common.arabic.words['Certificateneedstobeuploaded'];
    }
    else if(name == 'TrainingNotificationToManagerWorkflow'){
      return (this.language == 'English') ? 'Certificate uploaded': this.common.arabic.words['Certificateuploadedby'];
    }
  }

  getServiceName(process){
    process = process.replace(/ +/g, "").toLowerCase();
    return this.common.arabic.words[process];
  }

  getService(serviceName){
    if(serviceName == 'CAComplaintSuggestions'){
      return this.common.arabic.words['complaintssuggestions'];
    }else if(serviceName == 'MediaNewPressReleaseRequest'){
      return this.common.arabic.words['requestforpressrelease'];
    }else if(serviceName == 'MediaNewPhotoGrapherRequest'){
      return this.common.arabic.words['requestforphotographer'];
    }else if(serviceName == 'MediaPhotoRequest'){
      return this.common.arabic.words['requestforphoto'];
    }else if(serviceName == 'DiwanIdentity'){
      return this.common.arabic.words['requesttousediwanidentity'];
    }else if(serviceName == 'MediaDesignRequest'){
      return this.common.arabic.words['requestfordesign'];
    }else if(serviceName == 'MediaNewCampaignRequest'){
      return this.common.arabic.words['requestforcampaign'];
    }else if(serviceName == 'OfficialTask'){
      return this.common.arabic.words['officialtaskrequest'];
    }else if(serviceName == 'BabyAddition'){
      return this.common.arabic.words['newbabyadditionpopuptitle'];
    }else if(serviceName == 'Announcement'){
      return this.common.arabic.words['announcementrequestspopuptitle'];
    }else if(serviceName == 'Certificate'){
      return this.common.arabic.words['salarycertificaterequestpopuptitle'];
    }else if(serviceName == 'HRComplaintSuggestions'){
      return this.common.arabic.words['raisecomplaintssuggestions'];
    }else if(serviceName == 'Leave'){
      return this.common.arabic.words['leaverequestspopuptitle'];
    }else if(serviceName == 'Training'){
      return this.common.arabic.words['trainingrequestspopuptitle'];
    }else if(serviceName == 'ITSupport'){
      return this.common.arabic.words['itsupport'];
    }else if(serviceName == 'VehicleFine'){
      return this.common.arabic.words['vehiclefine'];
    }else {
      serviceName = serviceName.replace(/ +/g, "").toLowerCase();
      return this.common.arabic.words[serviceName];
    }
  }

  GetWordBtServiceName(name)
  {
    if(name != 'TrainingNotificationWorkflow'){
      return (this.language == 'English') ? 'by ': this.common.arabic.words['by'];
    }
    else{
      return '';
    } 
  }

  
  GetNotificationUserName(IsAnonymous,Name,FromName)
  {
    if(Name == 'TrainingNotificationWorkflow'){
      return '';
    }
    else if(IsAnonymous == true){
      return (this.language == 'English') ? 'Anonymous ': this.common.arabic.words['anonymous'];
    }
    else{
      return FromName;
    }
  }

  checkLogin() {
    if(localStorage.getItem('token')) {
      if(!this.router.url.includes('login')){
        this.authService.returnURL = this.router.url;
      }      
      let returnURL = this.authService.returnURL;
      if (returnURL != '') {
        this.router.navigate([returnURL]);
      }
    } else {
      this.router.navigate(['/login']);
    }
    return false;
  }
}
