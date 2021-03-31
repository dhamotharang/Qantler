import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from '../../../api/endpoint.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NewBabyAdditionService {
  APIendPoint = "/BabyAddition";
  babyAdditionData = {
    date_to: new Date(),
    sourceOU:'HR',
    sourceName: 'Mohammed',
    babyName: 'Azhar Ali',
    gender:'male',
    country: 'India',
    city: 'Trivandrum',
    hospitalName: 'Aster Medcity',
    certificate: ''
  };

  constructor(private httpClient: HttpClient, private endpoint: EndPointService) {
  }

  attachment_api_url = this.endpoint.apiHostingURL+'/attachment';
  createNewBabyAddition(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.APIendPoint, Data);
  }

  getNewBabyAddition(id:any, UserID: any) {
    let param = {
      UserID: UserID
    };
    return this.httpClient.get(this.endpoint.apiHostingURL + this.APIendPoint + '/' + id,{params:param});
  }

  updateNewBabyAddition(id:any, dataToupdate: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + this.APIendPoint + '/' + id, dataToupdate);
  }

  uploadAttachment(data:any):Observable<any>{
    var formData = new FormData();
    var headers = new HttpHeaders();
    for (var i = 0; i < data.length; i++) {
      formData.append('files', data[i]);
    }
    headers.append("Accept", 'application/json');
    headers.delete("Content-Type");
    const requestOptions = {
      headers: headers,
      reportProgress: true,
      observe:'events'
    };
    return this.httpClient.post(`${this.attachment_api_url}/Upload`,formData,{
      headers: headers,
      reportProgress: true,
      observe:'events'
    }).pipe(map((res:any)=>res));
  }
}
