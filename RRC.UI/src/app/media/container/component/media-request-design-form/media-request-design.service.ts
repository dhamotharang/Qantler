import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from '../../../../api/endpoint.service';
@Injectable({
  providedIn: 'root'
})
export class MediaRequestDesignService {

  constructor(private httpClient: HttpClient, private endpoint: EndPointService) { }
  // url = "http://qtsp2016.centralus.cloudapp.azure.com/rrc/api/";
  headers = new Headers();


  getDesign(APIString: string, id: any, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?Userid=' + userid);
  }
  getDesignType(APIString: string, id: any, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString  + '?Userid=' + userid);
  }
saveRequestDesign(APIString: string, Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }
  
  updateRequestDesign(APIString: string, Data: any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }
  statusChange(APIString: string, id, Status: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, Status);

  }
cloneRequestDesign(APIString: string, id: any, userid,data) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, data);
  }
}

