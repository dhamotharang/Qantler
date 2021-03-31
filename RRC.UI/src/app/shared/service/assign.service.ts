import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';

@Injectable()

export class AssignService {

  constructor(private httpClient: HttpClient, private endpoint: EndPointService) {

  }

  statusChange(APIString, id, Status: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, Status);
  }
}
