import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {ProfileUpdateGuard} from '../guards/profile-update.guard';

/**
 * General API Service
 */
@Injectable({
  providedIn: 'root'
})
export class GeneralService {
  /**
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {
  }

  /**
   * get contact us queries API
   *
   * @param pageNumber contain pageNumber details
   * @param pageSize contain pageSize details
   */
  getContactUsQueries(pageNumber, pageSize , keyword,sortType,  sortField) {
    return this.http.get(environment.apiUrl + 'Notifications/GetContactUsQueries/' + pageNumber + '/' + pageSize + '?search='+keyword+'&sortType='+sortType+'&sortField='+sortField);
  }

  /**
   * get Approval & rejection Threshold API
   *
   *
   */
  GetApprovalCount() {
    return this.http.get(environment.apiUrl + 'AppData/GetApprovalCount');
  }

  /**
   * update  Approval & rejection Threshold API
   *
   * @param data contain data details
   *
   */
  UpdateApprovalCount(data) {
    return this.http.put(environment.apiUrl + 'AppData/UpdateCommunityApprovalCount', data);
  }

  /**
   * post contact us query response
   *
   * @param data contain data details
   */
  postContactUsQueryResponse(data) {
    return this.http.post(environment.apiUrl + 'Notifications/PostReply', data);
  }
}
