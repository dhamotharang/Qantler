import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from '../api/endpoint.service';
import { Observable } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from '../common.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  returnURL: any = '';
  constructor(private httpClient: HttpClient, public common: CommonService, public activateRoute: ActivatedRoute, private endpoint: EndPointService, public router: Router) { }

  login(credential) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/User/Login', credential);
  }

  setUser(user) {
    user.IsAdmin = !!user.IsAdmin;
    localStorage.setItem("User", JSON.stringify(user));
    localStorage.setItem("UserID", '' + user.id);
    localStorage.setItem('token', user.Token);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  clearUserSession(type = '') {
    localStorage.removeItem('User');
    localStorage.removeItem('UserID');
    localStorage.clear();
    var lang = (this.common.language == 'English') ? 'en' : 'ar';
    //this.returnURL = (type == '') ? '/' + lang +this.router.url : '/' + lang + '/home';
    this.returnURL ='/' + lang + '/home';
    console.log(this.returnURL);
    this.router.navigate(['/login']);
    return false;
  }
}
