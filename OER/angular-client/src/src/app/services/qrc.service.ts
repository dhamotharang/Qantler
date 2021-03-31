import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {retry} from 'rxjs/operators';

/**
 * QRC API Service
 */
@Injectable({
  providedIn: 'root'
})
export class QrcService {
  /**
   *
   *
   * @param http
   */
  constructor(private http: HttpClient) {
  }

  /**
   *get QRC bu user id
   *
   * @param id
   */
  QrcByUser(id) {
    return this.http.get(environment.apiUrl + 'QRC/QrcByUser/' + id).pipe(
      retry(3)
    );
  }

  /**
   *get category by QRC id
   *
   * @param qrcId
   */
  getCategoryByQRC(qrcId) {
    return this.http.get(environment.apiUrl + 'QRC/GetQRCCategory/' + qrcId).pipe(
      retry(3)
    );
  }

  /**
   *get QRC Content
   *
   * @param data
   */
  GetContent(data) {
    return this.http.post(environment.apiUrl + 'QRC/GetContent', data).pipe(
      retry(3)
    );
  }

  /**
   *Update content status
   *
   * @param data
   */
  UpdateContentStatus(data) {
    return this.http.post(environment.apiUrl + 'QRC/UpdateContentStatus', data);
  }

  GetExpertCategories(userId) {
    return this.http.get(environment.apiUrl + 'Categories/GetMoeCategory/' + userId);
  }

  GetSensitivityCategories(userId) {
    return this.http.get(environment.apiUrl + 'Categories/GetSensoryCategory/' + userId);
  }

  GetCommunityCategories(userId) {
    return this.http.get(environment.apiUrl + 'Categories/GetCommunityCategory/' + userId);
  }

  /**
   *get community content list
   *
   * @param userId
   * @param pageNumber
   * @param pageSize
   */

  CommunityGetContent(userId, pageNumber, pageSize, category) {
    return this.http.get(environment.apiUrl + 'CommunityCheck/GetContentApproval/' + userId + '/' + pageNumber + '/' + pageSize + '/' + category);
  }

  /**
   *update community content status
   *
   * @param data
   */
  CommunityUpdateContentStatus(data) {
    return this.http.put(environment.apiUrl + 'CommunityCheck/UpdateContentStatus', data);
  }

  /**
   *get Approved community content list by UserId
   *
   * @param userId
   * @param pageNumber
   * @param pageSize
   */
  CommunityGetApprovedListByUser(userId, pageNumber, pageSize) {
    return this.http.get(environment.apiUrl + 'CommunityCheck/GetApprovedListByUser/' + userId + '/' + pageNumber + '/' + pageSize);
  }

  /**
   *get expert check content list
   *
   * @param userId
   * @param pageNumber
   * @param pageSize
   */
  MOEGetContent(userId, pageNumber, pageSize, category) {
    return this.http.get(environment.apiUrl + 'MoECheck/GetMoEContentApproval/' + userId + '/' + pageNumber + '/' + pageSize + '/' + category);
  }

  /**
   *update expert check content status
   *
   * @param data
   */
  MOEUpdateContentStatus(data) {
    return this.http.put(environment.apiUrl + 'MoECheck/MoEUpdateContentStatus', data);
  }

  /**
   *get Approved expert check content list by UserId
   *
   * @param userId
   * @param pageNumber
   * @param pageSize
   */
  MOEGetApprovedListByUser(userId, pageNumber, pageSize) {
    return this.http.get(environment.apiUrl + 'MoECheck/GetMoEApprovedListByUser/' + userId + '/' + pageNumber + '/' + pageSize);
  }

  /**
   *get sensitivity check content list
   *
   * @param userId
   * @param pageNumber
   * @param pageSize
   */
  SensoryCheckGetContent(userId, pageNumber, pageSize, category) {
    return this.http.get(environment.apiUrl + 'SensoryCheck/GetContentApproval/' + userId + '/' + pageNumber + '/' + pageSize + '/' + category);
  }

  /**
   *update  sensitivity check content status
   *
   * @param data
   */
  SensoryCheckUpdateContentStatus(data) {
    return this.http.put(environment.apiUrl + 'SensoryCheck/UpdateContentStatus', data);
  }
}
