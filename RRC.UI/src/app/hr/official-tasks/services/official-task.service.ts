import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';

@Injectable()
export class OfficialTaskService {
  url: any = '/Compensation/';
  officialUrl: any = '/OfficialTask/';
  userAvail: string = '/OfficialTask/UserAvailability/'
  constructor(private endpoint: EndPointService, private httpClient: HttpClient) { }

  create(url:string, postData: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + url, postData);
  }

  reSubmit(url:string, postData: any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + url, postData);
  }

  getById(url:string, id:any, userId:any, type?: any) {
    if (type == 'official') {
      return this.httpClient.get(`${this.endpoint.apiHostingURL}${this.officialUrl}${id}?UserId=${userId}`);
    } else {
      return this.httpClient.get(`${this.endpoint.apiHostingURL}${url}${id}?UserId=${userId}`);
    }
  }

  update(url:string, id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + url + id, dataToUpdate);
  }

  checkUsersAvailabitity(users:any, userId:any, start:any, end:any) {
    return this.httpClient.post(`${this.endpoint.apiHostingURL}${this.userAvail}?UserID=${userId}&StartDate=${start}&EndDate=${end}`, users);
  }

  generateAdministrativeReport(compensationID:any, UserID: any) {
    // tslint:disable-next-line: max-line-length
    return this.httpClient.post(`${this.endpoint.apiHostingURL}${this.url}GenerateAdministrativeDecision/${compensationID}`, '', {params:{'UserID':UserID}, responseType: 'arraybuffer'});
  }

  DownloadGenerateAdministrative() {
    return this.httpClient.get(`${this.endpoint.pdfDownloads}` + '/GeneralAdministrativeReport' + '.pdf', { responseType: 'blob' });
  }
}
