import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { EndPointService } from '../api/endpoint.service';
import { CommonService } from '../common.service';
import { Router } from '@angular/router';
@Injectable()

export class InterceptService implements HttpInterceptor {
  language: any;

  constructor(private authService: AuthService, public route: Router, public endpoint: EndPointService, public common: CommonService) {
    this.language = this.common.currentLang;
  }

  // intercept request and add token
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    var url = this.endpoint.apiHostingURL + '/User/Login',
      token = localStorage.getItem("token");
    if (request.url != url) {
      request = request.clone({
        setHeaders: {
          'Authorization': 'bearer ' + token,
          'Language': this.common.currentLang
        }
      });
    }
    else
    {
      console.log(this.common.currentLang);
      request = request.clone({
        setHeaders: {
          'Language': this.common.currentLang
        }
      });
    }
    return next.handle(request)
      .pipe(
        tap(event => {
          if (event instanceof HttpResponse) {
            if (event && event.body && event.body.TaskID) {
            } else {
              if (this.common.isNavigateTrigger) {
                this.common.triggerScrollTo('trigger-scroll');
                this.common.isNavigateTrigger = false;
              }
            }
          }
        }, error => {
          if (error.status === 400 && error.error === "Don't have access") {
            this.route.navigate(['error']);
          }
          if (error.status === 401) {
            this.authService.clearUserSession();
          }
        })
      );
  }

}

