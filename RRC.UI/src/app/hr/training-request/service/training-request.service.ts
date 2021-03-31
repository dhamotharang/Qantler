import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TrainingRequest } from '../model/training-request.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TrainingRequestService {

  endUrl:string = '/Training/';

  constructor(private endpoint: EndPointService, private httpClient: HttpClient) { }

  attachment_api_url = this.endpoint.apiHostingURL+'/attachment';

  create(trainingRequest: TrainingRequest) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.endUrl, trainingRequest);
  }

  reSubmission(trainingRequest:TrainingRequest){
    return this.httpClient.put(this.endpoint.apiHostingURL + this.endUrl, trainingRequest);
  }

  update(id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + this.endUrl + id, dataToUpdate);
  }

  AddTrainingAttendance(id:any,userID:any, attachments:any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + this.endUrl +'/Attendance/'  + id +'?UserID=' +  userID, attachments);
  }

  getTraining(id:any, userID:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + this.endUrl + id +'?UserID=' +  userID);
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
