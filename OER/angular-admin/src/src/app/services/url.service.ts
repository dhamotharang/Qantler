import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {retry} from 'rxjs/operators';
import {environment} from '../../environments/environment';

/**
 * Whitelist URL API Service
 */
@Injectable({
  providedIn: 'root'
})
export class URLService {
  /**
   *
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {
  }

  /**
   * return all url requests
   *
   */
  getAllRequests() {
    return this.http.get(environment.apiUrl + 'URLWhiteListing/GetWhiteListingRequests/false').pipe(
      retry(3)
    );
  }

  /**
   * return all approved API
   *
   */
  getAllApproved() {
    return this.http.get(environment.apiUrl + 'URLWhiteListing/GetWhiteListingRequests/true').pipe(
      retry(3)
    );
  }

  /**
   * update url whitelist request status
   *
   * @param data contain data details
   * @param id contain id details
   */
  updateRequest(data, id) {
    return this.http.put(environment.apiUrl + 'URLWhiteListing/' + id, data);
  }

  deleteRequest(id) {
    return this.http.delete(environment.apiUrl + 'URLWhiteListing/' + id);
  }
}
