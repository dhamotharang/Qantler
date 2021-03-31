import {EventEmitter, Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';
import {retry} from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';
import {ProfileService} from './profile.service';
import {SimpleTimer} from 'ng2-simple-timer';

/**
 * notification API Service
 */
@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  notificationList: any;
  notificationCount: number;
  notificationUpdate: EventEmitter<any> = new EventEmitter();

  /**
   * starts notification service
   *
   * @param http
   * @param profileService
   * @param st
   */
  constructor(private http: HttpClient, private profileService: ProfileService, private st: SimpleTimer) {
    this.notificationList = [];
    this.notificationCount = 0;
    this.startNotificationService();
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.startNotificationService();
    });
  }

  /**
   *starts notification check timer if user is logged in
   *
   */
  startNotificationService() {
    if (this.profileService.userId > 0) {
      this.st.newTimer('notificationService', 60);
      this.st.subscribe('notificationService', () => {
        this.getMyList();
      });
    } else {
      this.st.delTimer('notificationService');
      this.notificationList = [];
    }
  }

  /**
   *get notification list
   *
   */
  getMyList() {
    this.getMyNotifications(this.profileService.userId, 1, 10).subscribe((res) => {
      if (res.hasSucceeded) {
        this.notificationList = res.returnedObject;
        if (res.returnedObject.length > 0) {
          this.notificationCount = res.returnedObject[0].total;
        }
      } else if (res.message === 'No Records Found') {
        this.notificationList = [];
        this.notificationCount = 0;
      }
      this.notificationUpdate.emit();
    });
  }

  /**
   *get my notifications list API
   *
   * @param id
   * @param number
   * @param size
   */
  getMyNotifications(id, number, size): Observable<any> {
    return this.http.get(environment.apiUrl + 'Notifications/GetUserNotifications/' + id + '/' + number + '/' + size)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *update notification status API
   *
   * @param userId
   * @param id
   */
  updateNotifications(userId, id): Observable<any> {
    return this.http.put(environment.apiUrl + 'Notifications/UpdateNotification?UserId=' + userId + '&NotificationId=' + id, {});
  }

  /**
   *delete notification API
   *
   * @param userId
   * @param id
   */
  deleteNotifications(userId, id): Observable<any> {
    return this.http.request('delete', environment.apiUrl + 'Notifications/DeleteNotification?UserId=' + userId + '&NotificationId=' + id, {});
  }
}
