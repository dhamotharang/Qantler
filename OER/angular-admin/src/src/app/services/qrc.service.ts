import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {retry} from 'rxjs/operators';
import {environment} from '../../environments/environment';

/**
 * QRC API Service
 */
@Injectable({
  providedIn: 'root'
})
export class QrcService {

  /**
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {
  }

  /**
   * returns all QRC's
   *
   */
  getAllQRC(id) {
    return this.http.get(environment.apiUrl + 'QRC/GetQrcByCategory/' + id).pipe(
      retry(3)
    );
  }

  getAllQRCs() {
    return this.http.get(environment.apiUrl + 'QRC').pipe(
      retry(3)
    );
  }

  /**
   * returns category by QRC
   *
   * @param qrcId contain qrcId details
   */
  getCategoryByQRC(qrcId) {
    return this.http.get(environment.apiUrl + 'QRC/GetQRCCategory/' + qrcId).pipe(
      retry(3)
    );
  }

  /**
   * returns User by id
   *
   * @param id contain id details
   */
  getUserById(id) {
    return this.http.get(environment.apiUrl + 'Profile/GetUserById/' + id).pipe(
      retry(3)
    );
  }

  /**
   * returns QRC Users
   *
   * @param qrcId contain qrcId details
   * @param categoryId contain categoryId details
   * @param pageNo contain pageNo details
   * @param pageSize contain pageSize details
   */
  getQRCUsers(qrcId, categoryId, pageNo, pageSize) {
    return this.http.get(environment.apiUrl + 'QRC/GetUsers/' + qrcId + '/' + categoryId + '/' + pageNo + '/' + pageSize).pipe(
      retry(3)
    );
  }

  /**
   * returns QRC report
   *
   */
  getQRCReport() {
    return this.http.get(environment.apiUrl + 'Reports/GetQrcReport').pipe(
      retry(3)
    );
  }

  /**
   * returns verifiers report
   *
   * @param pagenumber contain pagenumber details
   * @param pageSize contain pageSize details
   */
  getVerifiersReport(keyword , pagenumber, pageSize , sortType,  sortField ) {
    return this.http.get(environment.apiUrl + 'Verifier/GetVerifiersReport/' + pagenumber + '/' + pageSize+ '?search='+keyword+'&sortType='+sortType+'&sortField='+sortField).pipe(
      retry(3)
    );
  }

  /**
   * fetch users that needs to be added to QRC
   *
   * @param qrcId contain qrcId details
   * @param categoryId contain categoryId details
   * @param pageNo contain pageNo details
   * @param pageSize contain pageSize details
   * @param filterCategoryId contain filterCategoryId details
   */
  fetchUsersToAdd(qrcId, categoryId, pageNo, pageSize, filterCategoryId) {
    // tslint:disable-next-line:max-line-length
    return this.http.get(environment.apiUrl + 'QRC/FetchUsersToAdd/' + qrcId + '/' + categoryId + '/' + pageNo + '/' + pageSize + '/' + filterCategoryId).pipe(
      retry(3)
    );
  }

  /**
   * create new QRC
   *
   * @param data contain data details
   */
  postQrc(data) {
    return this.http.post(environment.apiUrl + 'QRC', data);
  }

  /**
   * update existing QRC
   *
   * @param data contain data details
   * @param id contain id details
   */
  patchQrc(data, id) {
    return this.http.put(environment.apiUrl + 'QRC/' + id, data);
  }

  /**
   * add users to QRC
   *
   * @param data contain data details
   */
  postQRCUsers(data) {
    return this.http.post(environment.apiUrl + 'QRC/AddQRCUsers', data);
  }

  /**
   * patch QRC Users
   *
   * @param data contain data details
   */
  patchQRCUsers(data) {
    return this.http.put(environment.apiUrl + 'QRC/UpdateQRCUser', data);
  }

  deleteQrc(id) {
    return this.http.delete(environment.apiUrl + 'QRC/DeleteUnAssignedQrc/' + id);
  }
}
