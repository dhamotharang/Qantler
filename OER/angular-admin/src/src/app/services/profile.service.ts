import {HttpClient} from '@angular/common/http';
import {EventEmitter, Injectable} from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import {Observable} from 'rxjs';
import {retry} from 'rxjs/operators';
import {environment} from '../../environments/environment';

/**
 * Profile API Service
 */
@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  user: any;
  UserDataUpdate: EventEmitter<any> = new EventEmitter();

  /**
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {
  }

  /**
   * returns random id
   *
   * @param length contain length details
   */
  public makeid(length): string {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;
    for (let i = 0; i < length; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
  }

  /**
   * returns UserDataUpdate event emitter
   *
   */
  getUserDataUpdate() {
    return this.UserDataUpdate;
  }

  /**
   * check profile status by email
   *
   * @param email contain email details
   */
  checkProfileStatus(email): Observable<any> {
    return this.http.get(environment.apiUrl + 'Profile/' + email)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * creates initial user profile
   *
   * @param data contain data details
   */
  postInitalProfile(data) {
    return this.http.post(environment.apiUrl + 'Profile/CreateInitialProfile', data);
  }

  /**
   * update user role API
   *
   * @param userId contain userId details
   * @param isAdmin contain isAdmin details
   * @param isContributor contain isContributor details
   */
  postUpdateRole(userId: number, isAdmin: boolean, isContributor: boolean) {
    return this.http.post(
      environment.apiUrl + 'Profile/UpdateRole?userId=' + userId + '&IsContributor=' + isContributor + '&IsAdmin=' + isAdmin, {}
      );
  }

  /**
   * update user data end emits UserDataUpdate event
   *
   * @param user contain user details
   */
  setUser(user: any) {
    this.user = user;
    this.UserDataUpdate.emit();
  }
}
