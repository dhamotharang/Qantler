import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {retry} from 'rxjs/operators';
import {environment} from '../../environments/environment';


/**
 * Web Content Management API Service
 */
@Injectable({
  providedIn: 'root'
})
export class WCMService {
  /**
   *
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {
  }

  /**
   * returns all WCM Pages
   *
   */
  getAllList(categoryId:number) {
    return this.http.get(environment.apiUrl + 'WCM/GetPages/'+categoryId).pipe(
      retry(3)
    );
  }

  /**
   * get content of WCM Page by id
   *
   * @param id contain id details
   */
  getContent(id) {
    return this.http.get(environment.apiUrl + 'WCM/GetPageContent/' + id).pipe(
      retry(3)
    );
  }

  /**
   * create WCM Content
   *
   * @param data contain data details
   */
  postContent(data) {
    return this.http.post(environment.apiUrl + 'WCM', data);
  }

  /**
   * Update WCM Content
   *
   * @param data contain data details
   */
  patchContent(data) {
    return this.http.put(environment.apiUrl + 'WCM', data);
  }

}
