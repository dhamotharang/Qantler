import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { map } from 'rxjs/operators';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class CommentSectionService {

  private newCommentCreation:Subject<boolean> = new Subject();

  newCommentCreation$ = this.newCommentCreation.asObservable();

  constructor(private http:HttpClient, private endpointService:EndPointService) {
  }

  // getCommentSection(moduleName:string,id:number): Observable<Array<any>>{
  //   return this.http.get(`${this.endpointService.apiHostingURL}/${moduleName}/CommunicationChat/${id}`).pipe(map((res:any)=>res));
  // }

  sendComment(moduleName: string, chatData: any): Observable<any>{
    return this.http.post(`${this.endpointService.apiHostingURL}/${moduleName}/CommunicationChat`, chatData).pipe(map((res: any) => res));
  }

  newCommentCreated(isNewComment: boolean) {
    this.newCommentCreation.next(isNewComment);
  }

}
