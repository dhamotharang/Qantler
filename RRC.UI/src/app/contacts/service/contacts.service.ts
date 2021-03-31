import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';

@Injectable()

export class ContactsService {
  headers = new Headers();
  APIString = "Contact";
  // apiurl = "http://localhost:3000"
  constructor(private httpClient: HttpClient, private endpoint: EndPointService) {

  }

  contactList(APIString: string, pageNo, pageSize, type, userID) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userID);
  }


  saveContact(Data: any, userId) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString + '?UserId=' + userId, Data);
  }
  updateContact(Data: any, userId) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + this.APIString+ '/' + '?UserId=' + userId, Data);
  }
  
  getContact(id, userId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/' + id + '?UserId=' + userId);
  }

  statusChange(APIString, id, Status: any, userId) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id+ '?UserId=' + userId, Status);
  }

  getContactList(pageNumber:number,pageSize:number, Type, UserID, Department, UserName, Destination, EmailID, PhoneNumber, smartsearch ,EntityName,GovernmentEntity ) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/Home/list/'+ pageNumber + ',' + pageSize + "?UserName="+UserName 
    + "&Type=" + Type + "&UserID=" + UserID + "&Department=" + Department
    + "&Destination=" + Destination + "&EmailID=" + EmailID + "&PhoneNumber=" + PhoneNumber
    + "&SmartSearch="+smartsearch +"&EntityName="+EntityName +"&GovernmentEntity=" +GovernmentEntity);
  }

  getById(id:any, userId:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString + '/'+id+'/?UserId=' + userId);
  }

  deleteById(id:any, userId:any) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + this.APIString + '/'+id+'/?UserId=' + userId);
  }

  downlaodinternalExcel(data){
    var date = new Date,
    cur_date = date.getDate() +'-'+(date.getMonth()+1)+'-'+date.getFullYear();
    this.httpClient.post(this.endpoint.apiHostingURL + '/InternalContact/Report?type=Excel',data,{responseType: 'blob'})
    .subscribe((resultBlob: Blob)=>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = 'Internal Contact Report-'+cur_date+'.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove(); // remove the element
      // const blob = new Blob([data], { type: 'application/octet-stream' });
      // var url:any = window.URL.createObjectURL(blob);
      // window.open(url);
    });
}
downlaodExternalExcel(data){
  var date = new Date,
  cur_date = date.getDate() +'-'+(date.getMonth()+1)+'-'+date.getFullYear();
  this.httpClient.post(this.endpoint.apiHostingURL + '/ExternalContact/Report?type=Excel',data,{responseType: 'blob'})
  .subscribe((resultBlob: Blob)=>{
    var url = window.URL.createObjectURL(resultBlob);
    var a = document.createElement('a');
    document.body.appendChild(a);
    a.setAttribute('style', 'display: none');
    a.href = url;
    a.download = 'ExternalContact Report-'+cur_date+'.xlsx';
    a.click();
    window.URL.revokeObjectURL(url);
    a.remove(); // remove the element
    // const blob = new Blob([data], { type: 'application/octet-stream' });
    // var url:any = window.URL.createObjectURL(blob);
    // window.open(url);
  });
}
}