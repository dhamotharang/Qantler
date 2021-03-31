import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';
import {retry} from 'rxjs/operators';

/**
 * General API Service
 */
@Injectable({
  providedIn: 'root'
})
export class GeneralService {
  /**
   *
   *
   * @param http
   */
  constructor(private http: HttpClient) {
  }

  /**
   *get landing page data
   *
   */
  getLandingPageData(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Reports/GetDashboardReport')
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *post contact us request
   *
   * @param data
   */
  postContactUs(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Notifications/ContactUs', data);
  }

  /**
   *log shared count
   *
   * @param data
   */
  logSharedContent(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/SharedContent', data);
  }

}
