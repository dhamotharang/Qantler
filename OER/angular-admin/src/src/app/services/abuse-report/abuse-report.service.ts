import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';

/**
 * Abuse Reports API Service
 */
@Injectable({
  providedIn: 'root'
})
export class AbuseReportService {

  /**
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {

  }

  /**
   * get all abuse reports from API
   */
  getAllAbuses() {
    return this.http.get(environment.apiUrl + 'Reports/GetReportAbuseContent');
  }

  /**
   * update abuse report item status
   *
   * @param id contain id details
   * @param contentType contain contentType details
   * @param reason contain reason details
   */
  postDeleteAbuse(id, contentType, reason) {
    return this.http.put(
      environment.apiUrl + 'Reports/DeleteAbuseReport?id=' + id + '&contentType=' + contentType + '&reason=' + reason, {}
      );
  }
}
