import {EventEmitter, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {KeycloakService} from 'keycloak-angular';
import {retry} from 'rxjs/operators';
import {environment} from '../../environments/environment';
import {FormControl} from '@angular/forms';
import {TranslateService} from '@ngx-translate/core';
import {CookieService} from 'ngx-cookie-service';
import {QrcService} from './qrc.service';

/**
 * User Profile Service
 */
@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  userId: number;
  user: any;
  userRoles: any;
  theme: string;
  themeColor: string;
  showHintUpdate: EventEmitter<any> = new EventEmitter();
  UserDataUpdate: EventEmitter<any> = new EventEmitter();
  clientThemeUpdate: EventEmitter<any> = new EventEmitter();
  QRCCountUpdate: EventEmitter<any> = new EventEmitter();
  QRCCount: number;

  /**
   * sets default theme and lang
   * @param http
   * @param keyCloakService
   * @param translate
   * @param cookieService
   */
  constructor(private http: HttpClient, private QRCService: QrcService, private keyCloakService: KeycloakService, private translate: TranslateService, private cookieService: CookieService) {
    this.QRCCount = 0;
    translate.setDefaultLang('en');
    translate.use(this.cookieService.get('manaraLang') ? this.cookieService.get('manaraLang') : 'en');
    this.userId = 0;
    this.user = null;
    this.theme = this.cookieService.get('theme') ? this.cookieService.get('theme') : 'blue';
    if (this.theme === 'blue') {
      this.themeColor = '#1a3464';
    } else {
      this.themeColor = '#ba9a3a';
    }
    this.clientThemeUpdate.emit();
  }

  /**
   * returns random string
   *
   * @param length
   */
  public makeid(length): string {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;
    for (let i = 0; i < length; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
  }

  /**
   *sets show hint status
   *
   * @param status
   */
  setShowHint(status) {
    this.cookieService.set('showHint', status);
    this.showHintUpdate.emit(status);
  }

  /**
   * sets theme and update user profile
   *
   * @param theme
   */
  setTheme(theme) {
    if (theme !== this.theme) {
      this.theme = theme;
      this.clientThemeUpdate.emit();
      if (theme === 'blue') {
        this.cookieService.set('theme', 'blue');
        this.themeColor = '#1a3464';
        if (this.userId && this.userId > 0) {
          this.user.theme = 'Blue';
          this.updateTheme('Blue').subscribe();
        }
      } else {
        this.cookieService.set('theme', 'gold');
        this.themeColor = '#ba9a3a';
        if (this.userId && this.userId > 0) {
          this.user.theme = 'Gold';
          this.updateTheme('Gold').subscribe();
        }
      }
    }
  }

  /**
   * returns clientThemeUpdate event emitter
   */
  getClientThemeUpdate() {
    return this.clientThemeUpdate;
  }

  /**
   * returns UserDataUpdate event emitter
   */
  getUserDataUpdate() {
    return this.UserDataUpdate;
  }

  /**
   * Sets user profile data, theme and lang
   *
   * emits clientThemeUpdate if  theme is changed, and emits UserDataUpdate
   *
   * @param user
   */
  setUser(user: any) {
    this.user = user;
    this.userId = (user && user.id) ? user.id : 0;
    if (user && user.portalLanguage && user.portalLanguage) {
      if (user.portalLanguage.id === 2) {
        this.translate.use('en');
        this.cookieService.set('manaraLang', 'en');
      } else if (user.portalLanguage.id === 1) {
        this.translate.use('ar');
        this.cookieService.set('manaraLang', 'ar');
      }
    }
    if (user && user.theme) {
      if (user.theme === 'Blue' && this.theme !== 'blue') {
        this.cookieService.set('theme', 'blue');
        this.theme = 'blue';
        this.themeColor = '#1a3464';
        this.clientThemeUpdate.emit();
      } else if (user.theme === 'Gold' && this.theme !== 'gold') {
        this.cookieService.set('theme', 'gold');
        this.theme = 'gold';
        this.themeColor = '#ba9a3a';
        this.clientThemeUpdate.emit();
      }
    }
    this.UserDataUpdate.emit();
    this.QrcCountCheck();
  }

  QrcCountCheck() {
    if (this.userId > 0) {
      this.QRCService.QrcByUser(this.userId).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.QRCCount = res.returnedObject.length;
        }
        this.QRCCountUpdate.emit();
      });
    } else {
      this.QRCCount = 0;
    }
    this.QRCCountUpdate.emit();
  }

  QRCCountUpdateSub() {
    return this.QRCCountUpdate;
  }

  /**
   * update portal language id API
   *
   * @param id
   */
  updateProtalLanguage(id) {
    this.translate.use(id === 1 ? 'ar' : 'en');
    this.postUpdateRole('userId=' + this.userId + '&portalLanguageId=' + id).subscribe((res: any) => {
      if (res.hasSucceeded) {
        const user = this.user;
        if (id === 2) {
          user.portalLanguage = {id: 2, name: 'English'};
        } else if (id === 1) {
          user.portalLanguage = {id: 1, name: 'عربي'};
        }
        this.setUser(user);
      }
    });
  }

  /**
   * update theme API
   *
   * @param theme
   */
  updateTheme(theme: string) {
    return this.http.put(environment.apiUrl + 'Profile/UpdateTheme?UserId=' + this.userId + '&Theme=' + theme, {}).pipe(
      retry(3)
    );
  }

  /**
   * update last login API
   *
   * @param id
   */
  postLogin(id) {
    return this.http.put(environment.apiUrl + 'Profile/UpdateLastLogin/' + id, {
      userId: id
    });
  }

  /**
   * log visitor API
   *
   * @param id
   */
  logVisitor(id) {
    return this.http.post(environment.apiUrl + 'Reports/AddVisiter', {
      userId: id
    });
  }

  /**
   * get Announcement by user API
   *
   * @param userId
   * @param pageNumber
   * @param pageSize
   */
  getAnnouncementsByUser(userId, pageNumber, pageSize): Observable<any> {
    return this.http.get(environment.apiUrl + 'Announcements/GetUserAnnouncements/' + userId + '/' + pageNumber + '/' + pageSize)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * returns init data for userprofile form
   */
  getMakeProfileData(): Observable<any> {
    return this.http.get(environment.apiUrl + 'AppData')
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * returns user profile by email
   *
   * @param email
   */
  checkProfileStatus(email): Observable<any> {
    return this.http.get(environment.apiUrl + 'Profile/' + email)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * add bookmark
   *
   * @param data
   */
  AddUserBookmarkedContent(data) {
    return this.http.post(environment.apiUrl + 'Profile/AddUserBookmarkedContent', data);
  }

  /**
   * delete bookmark
   *
   * @param data
   */
  DeleteUserBookmarkedContent(data) {
    return this.http.request('delete', environment.apiUrl + 'Profile/DeleteUserBookmarkedContent', {body: data});
  }

  /**
   * list bookmarked items
   *
   * @param id
   */
  ListUserBookmarkedContent(id) {
    return this.http.get(environment.apiUrl + 'Profile/GetUserBookmarkedContent/' + id);
  }

  /**
   * post initial profile
   *
   * @param data
   */
  postInitalProfile(data) {
    return this.http.post(environment.apiUrl + 'Profile/CreateInitialProfile', data);
  }

  /**
   * Update user role API
   *
   * @param data
   */
  postUpdateRole(data) {
    return this.http.post(environment.apiUrl + 'Profile/UpdateRole?' + data, {});
  }

  /**
   * deactivate user profile API
   *
   * @param data
   */
  DeActiveProfile(data) {
    return this.http.post(environment.apiUrl + 'Profile/DeActiveProfile', data);
  }

  /**
   * update email notification status API
   *
   * @param data
   */
  UpdateEmailNotification(data) {
    return this.http.put(environment.apiUrl + 'Profile/UpdateEmailNotification', data);
  }

  /**
   * return email notifications status
   *
   * @param id
   */
  GetEmailNotificationStatus(id) {
    return this.http.get(environment.apiUrl + 'Profile/GetEmailNotificationStatus?userId=' + id);
  }

  /**
   * create profile for user API
   *
   * @param data
   * @param email
   */
  postProfileData(data, email): Observable<any> {    
    let interestedSubjects = '';
    if (data.interestedSubjects && data.interestedSubjects.length > 0) {
      data.interestedSubjects.forEach((subject) => {
        if (subject) {
          interestedSubjects = interestedSubjects != '' ? interestedSubjects + ',' + subject.id : subject.id;
        }
      });
    }
    if (data.experiences && data.experiences.length > 0) {
      data.experiences.forEach((item) => {
        if (item.current) {
          item.toDate = null;
        }
      });
    }
    const postData = {
      titleId: data.title,
      firstName: data.firstName,
      middleName: data.middleName,
      lastName: data.lastName,
      countryId: data.country,
      stateId: data.state,
      gender: data.gender,
      email: email,
      portalLanguageId: data.portalLang,
      dateOfBirth: this.parse(data.dob),
      photo: data.photo,
      profileDescription: data.profileDesc,
      subjectsInterested: interestedSubjects,
      isContributor: (data.contributor && data.contributor === true) ? true : false,
      userCertifications: data.certifications,
      userEducations: data.qualifications,
      userExperiences: data.experiences,
      userLanguages: data.languages,
      userSocialMedias: data.socialLinks,
      emailUrl: environment.clientUrl + '/dashboard/community-check'
    };
    return this.http.post(environment.apiUrl + 'Profile', postData);
  }

  /**
   * update profile for user API
   *
   * @param data
   * @param email
   */
  updateProfileData(data, email): Observable<any> {    
    let interestedSubjects = '';
    if (data.interestedSubjects && data.interestedSubjects.length > 0) {
      data.interestedSubjects.forEach((subject) => {
        if (subject) {
          interestedSubjects = interestedSubjects != '' ? interestedSubjects + ',' + subject.id : subject.id;
        }
      });
    }
    if (data.experiences && data.experiences.length > 0) {
      data.experiences.forEach((item) => {
        if (item.current) {
          item.toDate = null;
        }
      });
    }
    const postData = {
      id: data.id,
      titleId: data.title,
      firstName: data.firstName,
      middleName: data.middleName,
      lastName: data.lastName,
      countryId: data.country,
      stateId: data.state,
      gender: data.gender,
      email: data.email,
      portalLanguageId: data.portalLang,
      dateOfBirth: this.parse(data.dob),
      photo: data.photo,
      profileDescription: data.profileDesc,
      subjectsInterested: interestedSubjects,
      isContributor: (data.contributor && data.contributor === true) ? true : false,
      userCertifications: data.certifications,
      userEducations: data.qualifications,
      userExperiences: data.experiences,
      userLanguages: data.languages,
      userSocialMedias: data.socialLinks,
      emailUrl: environment.clientUrl + '/dashboard/community-check'
    };
    return this.http.put(environment.apiUrl + 'Profile', postData);
  }

  /**
   * convert date string to ISOstring
   *
   * @param value
   */
  parse(value: any): string | null {
    if ((typeof value === 'string') && (value.indexOf('/') > -1)) {
      const str = value.split('/');

      const year = Number(str[2]);
      const month = Number(str[1]) - 1;
      const date = Number(str[0]);

      return new Date(year, month, date).toDateString();
    } else if ((typeof value === 'string') && value === '') {
      return new Date().toDateString();
    }
    const timestamp = typeof value === 'number' ? value : Date.parse(value);
    return isNaN(timestamp) ? null : new Date(timestamp).toDateString();
  }

  /**
   * get Course reports by user
   */
  getCourses(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Reports/GetCoursesByUserId/' + this.userId)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get resource reports by user
   */
  getResources(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Reports/GetResourceByUserId/' + this.userId)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get user dashboard data
   */
  getUserDashboardReport(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Reports/UserDashboardReport/' + this.userId)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get user dashboard data
   */
  getPublicUserDashboardReport(id): Observable<any> {
    return this.http.get(environment.apiUrl + 'Reports/UserDashboardReport/' + id)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get public user profile
   *
   * @param id
   */
  getPublicUserById(id): Observable<any> {
    return this.http.get(environment.apiUrl + 'Profile/GetUserById/' + id)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get user recomendations
   *
   * @param pageNumber
   * @param pageSize
   */
  getUserRecommendedContent(pageNumber, pageSize): Observable<any> {
    return this.http.get(environment.apiUrl + 'Reports/UserRecommendedContent/' + this.userId + '/' + pageNumber + '/' + pageSize)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get default recomendations
   *
   * @param pageNumber
   * @param pageSize
   */
  getDefaultRecommendedContent(pageNumber, pageSize): Observable<any> {
    return this.http.get(environment.apiUrl + 'Reports/UserRecommendedContent/0/' + pageNumber + '/' + pageSize)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get favourite status API
   *
   * @param UserId
   * @param ContentId
   * @param ContentType
   */
  getFavouriteStatus(UserId, ContentId, ContentType): Observable<any> {
    return this.http.get(environment.apiUrl + 'Profile/GetUserFavouritesByContentID/' + UserId + '/' + ContentId + '/' + ContentType);
  }

    /**
   * reset password
   */
  postResetPassword(): Observable<any>{
    debugger
    return this.http.post(environment.apiUrl + 'Profile/ResetPassword',null);
  }
}
