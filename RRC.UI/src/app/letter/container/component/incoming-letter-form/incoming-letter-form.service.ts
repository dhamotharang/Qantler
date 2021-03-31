import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from '../../../../api/endpoint.service';
import { environment } from 'src/environments/environment';
import { CommonService } from '../../../../common.service';

@Injectable({
  providedIn: 'root'
})
export class IncomingLetterService {

  constructor(private httpClient: HttpClient, private endpoint: EndPointService, public common: CommonService) { }
  // url = "http://qtsp2016.centralus.cloudapp.azure.com/rrc/api/";
  headers = new Headers();

  saveLetter(APIString: string, Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }

  updateLetter(APIString: string, Data: any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }
  getLetters(APIString: string, userid, type, refno) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + userid + '?Type=' + type + '&RefNo=' + refno);
  }
  getMemo(APIString: string, id: any, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?Userid=' + userid);
  }
  getEntityByName(APIString: string, id, entityId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + 'Entity' + '/' + id + '?EntityID=' + entityId);
  }

  getEntity(APIString: string, id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + 'Entity' + '/' + id);

    // http://qtsp2016.centralus.cloudapp.azure.com/rrc/api/OutboundLetter/Entity/1

  }
  getCombo(APIString: string, id: any, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userid);
  }
  saveClone(APIString: string, id: any, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userid);
  }
  downloandLetter(APIString: string, id: any, userid, Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userid, Data);
  }
  printPdf(refno, type?: any) {
    this.httpClient.get(environment.DownloadUrl + refno + type, { responseType: 'blob' })
      .subscribe((data: Blob) => {
        var url = window.URL.createObjectURL(data);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = refno + type;
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        this.common.deleteGeneratedFiles('files/delete', refno + type).subscribe(result => {
          console.log(result);
        });
      });
  }

  getReleatedOutgoingLetterCombo(APIString: string, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + userid);
  }
  getReleatedIncomingLetterCombo(APIString: string, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + userid);
  }

  statusChange(APIString: string, id, Status: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, Status);
  }

  deleteLetter(APIString: string, id, userid) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userid);
  }
}

