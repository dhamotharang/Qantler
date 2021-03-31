import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';

/**
 * Users API Service
 */
@Injectable({
  providedIn: 'root'
})
export class UsersService {

  /**
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {

  }

  /**
   * get all users
   *
   */
  getAllUsers() {
    return this.http.get(environment.apiUrl + 'Reports/GetUsers');
  }

  /**
   * delete user by id
   *
   * @param id contain id details
   */
  deleteUsers(id) {
    return this.http.delete(environment.apiUrl + 'Resource/' + id);
  }
}
