import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {retry} from 'rxjs/operators';

/**
 * WCM API Service
 */
@Injectable({
  providedIn: 'root'
})
export class WCMService {
  /**
   *
   * @param http initialized http
   */
  constructor(private http: HttpClient) {
  }

  /**
   * get WCM List
   */
  getAllList(categoryId:number) {
    return this.http.get(environment.apiUrl + 'WCM/GetPages/'+categoryId).pipe(
      retry(3)
    );
  }

  getAllPageContents() {
    return this.http.get(environment.apiUrl + 'WCM/GetAllPageContents').pipe(
      retry(3)
    );
  }

  /**
   * get WCM List by id API
   *
   * @param id Retrieves page content by id
   */
  getContent(id) {
    return this.http.get(environment.apiUrl + 'WCM/GetPageContent/' + id).pipe(
      retry(3)
    );
  }
}
