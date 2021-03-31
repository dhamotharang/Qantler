import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {retry} from 'rxjs/operators';
import {environment} from '../../../environments/environment';

/**
 * Resource API Service
 */
@Injectable({
  providedIn: 'root'
})
export class ResourcesService {

  /**
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {

  }

  /**
   * get all resources
   *
   * @param pageNumber contain pageNumber details
   * @param pageSize contain pageSize details
   */
  getAllResources(value,pageNumber, pageSize,ascDescNo,columnNo) {
    return this.http.get(environment.apiUrl + 'Resource/' + pageNumber + '/' + pageSize + '?search='+value+'&ascDescNo='+ascDescNo+'&columnNo='+columnNo).pipe(
      retry(3)
    );
    // return this.http.get(environment.apiUrl + 'Resource');
  }

  /**
   * delete resource by id
   *
   * @param id contain id details
   */
  deleteResource(id) {
    return this.http.delete(environment.apiUrl + 'Resource/' + id);
  }

  /**
   * get rejection list
   *
   * @param pageNumber contain pageNumber details
   * @param pageSize contain pageSize details
   */
  getRejectionList(keyword ,pageNumber, pageSize,sortType , sortField) {
    return this.http.get(environment.apiUrl + 'Reports/ContentRejectedList/' + pageNumber + '/' + pageSize + '?search='+keyword+'&sortType='+sortType+'&sortField='+sortField).pipe(
      retry(3)
    );
    // return this.http.get(environment.apiUrl + 'Resource');
  }

}
