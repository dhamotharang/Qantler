import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';

/**
 * Announcements API Service
 */
@Injectable({
  providedIn: 'root'
})
export class AnnouncementsService {

  /**
   *
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {
  }

  /**
   * get all Announcements
   *
   */
  getAnnouncements() {
    return this.http.get(environment.apiUrl + 'Announcements');
  }

  /**
   * update Announcement API
   *
   * @param data contain data details
   */
  putAnnouncements(data) {
    return this.http.put(environment.apiUrl + 'Announcements', data);
  }

  /**
   * create Announcement API
   *
   * @param data contain data details
   */
  postAnnouncements(data) {
    return this.http.post(environment.apiUrl + 'Announcements', data);
  }

  /**
   * delete Announcement API
   *
   * @param id contain id details
   */
  deleteAnnouncements(id) {
    return this.http.delete(environment.apiUrl + 'Announcements/?id=' + id);
  }
}
