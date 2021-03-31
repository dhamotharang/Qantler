import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { DiwanIdentityRequest } from '../model/diwan-identity-request/diwan-identity-request.model';

@Injectable()
export class DiwanIdentityService {
  currentUserData: any = JSON.parse(localStorage.getItem('User'));
  constructor(private http:HttpClient,private endpointService:EndPointService) { }
  
  diwan_identity_request_api_url = this.endpointService.apiHostingURL+'/DiwanIdentity';

  getDiwanIdentityRequestList(pageNumber:number,pageSize:number):Observable<Array<DiwanIdentityRequest>>{
    return this.http.get(`${this.diwan_identity_request_api_url}/list/${pageNumber}/${pageSize}`).pipe(map((res:any)=>res));
  }

  getDiwanIdentityRequestById(id:number,currentUserId:any):Observable<DiwanIdentityRequest>{
    let param = {
      UserID:currentUserId
    };
    return this.http.get<DiwanIdentityRequest>(`${this.diwan_identity_request_api_url}/${id}`,{params:param}).pipe(map((res:any)=>res));
  }

  addDiwanIdentityRequest(diwan_identityRequestData:DiwanIdentityRequest):Observable<any>{
    return this.http.post(`${this.diwan_identity_request_api_url}`,diwan_identityRequestData).pipe(map((res:any)=>res));
  }

  resubmitDiwanIdentityRequest(diwan_identityRequestData:DiwanIdentityRequest):Observable<any>{
    return this.http.put(`${this.diwan_identity_request_api_url}`,diwan_identityRequestData).pipe(map((res:any)=>res));
  }

  updateDiwanIdentityRequestStatus(id:number,updateDiwanIdentityRequestData:any):Observable<any>{
    return this.http.patch(`${this.diwan_identity_request_api_url}/${id}`,updateDiwanIdentityRequestData).pipe(map((res:any)=>res));
  }
}
