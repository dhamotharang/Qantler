import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Headers, RequestOptions, URLSearchParams, Http, HttpModule } from '@angular/http';
import { environment } from 'src/environments/environment';
import { EndPointService } from '../../../app/api/endpoint.service';
import { map } from 'rxjs/operators';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';


@Injectable({
  providedIn: 'root'
})
export class VehicleMgmtServiceService {
  headers = new Headers();
  APIString = "VehicleRequest";
  driverAPI = "VehicleDriver";
  vehicleManagementApiUrl = this.endpoint.apiHostingURL + '/VehicleManagement';
  vehicleManagementLogApiUrl = this.endpoint.apiHostingURL + '/VehicleManagementLogService';
  cur_User: any;

  // renCarModalClose = new Subject<any>();
  // renCarModalClose$ = this.renCarModalClose.asObservable();
  reloadRentCar = new Subject<any>();
  reloadRentCar$ = this.reloadRentCar.asObservable();
  constructor(private httpClient: HttpClient, private endpoint: EndPointService, private http: Http, public _ScrollToService:ScrollToService) {
    this.cur_User = JSON.parse(localStorage.getItem('User'));
  }

  triggerScrollTo()
  {
    const config: ScrollToConfigOptions = {
      target: 'destination',
      offset: 300
    };

    this._ScrollToService.scrollTo(config);
  }
  resCarSave() {
    this.reloadRentCar.next();
  }

  getVehicleRequestList(pageNumber: number, pageSize: number, RequestType, Status, Requestor, TripDateFrom, TripDateTo, Destination, Smartsearch, RequestorOfficeDepartment, UserId, Type) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString + '/' + pageNumber + ',' + pageSize +
      "?RequestType=" + RequestType +
      "&Status=" + Status + "&Requestor=" + Requestor +
      "&TripDateFrom=" + TripDateFrom + "&TripDateTo=" + TripDateTo +
      "&Destination=" + Destination + "&Smartsearch=" + Smartsearch +
      "&RequestorOfficeDepartment=" + RequestorOfficeDepartment +
      "&UserID=" + UserId +
      "&Type=" + Type
    );
  }

  getVehicleRequest(pageNumber: number, pageSize: number, UserId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString + '/' + pageNumber + ',' + pageSize +
      "?UserID=" + UserId
    );
  }

  getVehicleRequestCount(userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString + '/Home/Count/' + userid);
  }

  updateRequest(id: any, dataToUpdate: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + this.APIString + '/' + id, dataToUpdate);
  }

  getVehicleRequestById(id, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString + '/' + id + '?UserID=' + userid);
  }

  getVehicleDriverTrips(id, vehicleReqID) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/VehicleDriverTrips/' + id + '?VehicelID=' + vehicleReqID);
  }

  getVehicleDetails(plateNumber, userId, APIString, VehicleID) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/'+ APIString + '?UserID='+ userId + "&PlateNumber=" + plateNumber+ "&VehicleID=" + VehicleID);
  }
  
  getVehicleIssues(id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/VehicleIssues?UserId='+ id);
  }
  sendVehicleIssues(id, dataToUpdate) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/'  + this.APIString +'/VehicleIssues/'+id, dataToUpdate);
  }

  VehicleIssues
  getdriver() {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/Master/VehicleDriver?Driver=true');
  }
  getNonDriverUsers() {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/Master/VehicleDriver?Driver=false');
  }
  getVehicleModel(id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/VehicleModel?UserID=' + id);
  }
  getTripType(id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/TripType?UserID=' + id);
  }
  getTripDestination(id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/TripDestination?UserID=' + id);
  }

  // Vehicle Car Company ------->

  getVehicleCarCompanyList(APIString, pageNo, pageSize, CreatedDateFrom, CreatedDateTo, search) {
    let param = '?UserID=' + this.cur_User.id;
    if (CreatedDateFrom) {
      param += '&CreatedDateFrom=' + CreatedDateFrom;
    }
    if (CreatedDateTo) {
      param += '&CreatedDateTo=' + CreatedDateTo;
    }
    if (search) {
      param += '&SmartSearch=' + search;
    }
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + pageNo + ',' + pageSize + param);
  }

  getVehicleCarCompanyListById(APIString, id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id);
  }

  saveCarCompany(APIString, param) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString, param);
  }

  updateCarCompany(APIString, param) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + APIString, param);
  }

  deleteCompany(APIString, id) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + APIString + '/' + id);
  }

  dowloadExcel(APIStrig , name,data) {
    var date = new Date,
      cur_date = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear(),
      param = '';
    var key = Object.keys(data);
    param += 'UserID=' + data['UserID'];
    key.map(res => {
      if (data[res] && res != 'UserID')
        param += '&' + res + '=' + data[res];
    });
    this.httpClient.get(this.endpoint.apiHostingURL + '/'+APIStrig+'/export?' + param, { responseType: 'blob' })
      .subscribe((resultBlob: Blob) => {
        var url = window.URL.createObjectURL(resultBlob);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = name + cur_date + '.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
      });
  }

  // Vehicle Fine Manegement ----->

  getFineList(APIString, pageNo, pageSize, status, department, name, from, to, plateNumber, search) {
    let userId = this.cur_User.id;
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + pageNo + ',' + pageSize + '?UserID=' + userId + '&Status=' + status + '&IssuedAgainstDepartment=' + department + '&IssuedAgainstName=' + name + '&FineDateFrom=' + from + '&FineDateTo=' + to + '&PlateNumber=' + plateNumber + '&SmartSearch=' + search);
  }

  getFineACarList(APIString, filter) {
    let param = '?UserID=' + this.cur_User.id;
    if (filter.plateNumber) {
      param += '&PlateNumber=' + filter.plateNumber;
    }
    if (filter.VehicleID) {
      param += '&VehicleID=' + filter.VehicleID;
    }
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + param);
  }

  getFineACarServiceList(APIString, VehicleID,Type) {

    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + VehicleID + '?Type=' + Type);
  }

  getFineListById(APIString, id, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userid);
  }

  saveFine(APIString, param,userid) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString+  '?UserID=' + userid, param);
  }

  updateFine(APIString, param) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + APIString, param);
  }

  deleteFine(APIString, id) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + APIString + '/' + id);
  }

  getVehicleList(pageNumber, pageSize, options): Observable<any> {
    let reqParams: any = {};

    if (options.UserID >= 0) {
      reqParams.UserID = options.UserID;
    }

    if (options.UserName) {
      reqParams.UserName = options.UserName;
    }

    if (options.DepartmentOffice) {
      reqParams.DepartmentOffice = options.DepartmentOffice;
    }

    if (options.RequestorDepartment) {
      reqParams.RequestorDepartment = options.RequestorDepartment;
    }

    if (options.PlateNumber) {
      reqParams.PlateNumber = options.PlateNumber;
    }

    if (options.PlateColor) {
      reqParams.PlateColor = options.PlateColor;
    }

    if (options.Destination) {
      reqParams.Destination = options.Destination;
    }

    if (options.SmartSearch) {
      reqParams.SmartSearch = options.SmartSearch;
    }

    if (options.AlternativeVehicle == "Yes") {
      reqParams.IsAlternativeVehicle = true;
    }else if(options.AlternativeVehicle == "No"){
      reqParams.IsAlternativeVehicle = false;
    }
    return this.httpClient.get(`${this.vehicleManagementApiUrl}/${pageNumber},${pageSize}`, { params: reqParams }).pipe(map((res: any) => res));
  }

  saveVehicle(vehicleManagementData): Observable<any> {
    return this.httpClient.post(`${this.vehicleManagementApiUrl}`, vehicleManagementData).pipe(map((res: any) => res));
  }

  editVehicle(vehicleManagementData): Observable<any> {
    return this.httpClient.put(`${this.vehicleManagementApiUrl}`, vehicleManagementData).pipe(map((res: any) => res));
  }

  updateVehicle(vehicleManagementData): Observable<any> {
    return this.httpClient.patch(`${this.vehicleManagementApiUrl}/${vehicleManagementData.VehicleID}`, vehicleManagementData).pipe(map((res: any) => res));
  }

  deleteVehicle(vehicleManagementData): Observable<any> {
    return this.httpClient.delete(`${this.vehicleManagementApiUrl}/${vehicleManagementData.VehicleID}`).pipe(map((res: any) => res));
  }

  

  getVehicleDetailsById(VehicleID, UserId): Observable<any> {
    let reqParams: any = {
      UserID: UserId
    };
    return this.httpClient.get(`${this.vehicleManagementApiUrl}/${VehicleID}`, { params: reqParams }).pipe(map((res: any) => res));
  }

  saveReminder(APIString, userid, param) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '?UserID=' + userid, param);
  }

  saveVehicleLog(vehicleManagementData): Observable<any> {
    return this.httpClient.post(`${this.vehicleManagementLogApiUrl}`, vehicleManagementData).pipe(map((res: any) => res));
  }

  // ---------------->

  // Master API -------->

  getStatus(APIString, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '?UserID=' + userid);
  }

  getCarCompanyList(): Observable<any> {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/VehicleCarCompanyList').pipe(map((res: any) => res));
  }

  getPlateNumberAndColorList(): Observable<any> {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/VehicleManagementAllPlateNumber').pipe(map((res: any) => res));
  }

  vehicleListExcelDownload(options): Observable<any> {
    // let toSendParams:any = {
    //   type:'Excel'
    // };
    // if(options.reportType){
    //   toSendParams.type = options.reportType
    // }
    return this.httpClient.get(`${this.vehicleManagementApiUrl}/export`, { responseType: 'blob', params: options }).pipe(map((res: any) => res));
  }

  saveVehicleReqeset(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }

  saveDriverList(driverList:any,userID:any):Observable<any>{
    let toSendData = {
      DriverID:driverList
    };
    return this.httpClient.post(this.endpoint.apiHostingURL + '/Master/' + this.driverAPI,toSendData,{params:{UserID:userID}}).pipe(map((res:any) => res));
  }

  addExtraCompensateHours(extraHourData:any):Observable<any>{
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.driverAPI,extraHourData).pipe(map((res:any) => res));
  }

  getDriverModalView(id:any,userId:any,dateFilter:any):Observable<any> {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.driverAPI + '/' + id, {params:{UserID:userId, DateRangeFrom:dateFilter.StartDate, DateRangeTo:dateFilter.EndDate}}).pipe(map((res:any) => res));
  }

  getAllDrivers(pageNumber: number, pageSize: number, UserID):Observable<any> {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.driverAPI + '/' + pageNumber + ',' + pageSize +
      "?UserID=" + UserID).pipe(map((res:any) => res));
  }
  public getUserList(data: any, id) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/User?UserID=' + id + '&type=', data);
  }

  driverListExcelDownload(id,options): Observable<any> {
    // let toSendParams:any = {
    //   type:'Excel'
    // };
    // if(options.reportType){
    //   toSendParams.type = options.reportType
    // }
    return this.httpClient.get(this.endpoint.apiHostingURL+`/VehicleDriver/export/`+id, { responseType: 'blob', params: options }).pipe(map((res: any) => res));
  }

  vehicleServiceListExcelDownload(id): Observable<any> {
    // let toSendParams:any = {
    //   type:'Excel'
    // };
    // if(options.reportType){
    //   toSendParams.type = options.reportType
    // }
    return this.httpClient.get(this.endpoint.apiHostingURL+`/GetVehicleManagementLogServiceReport/`+id, { responseType: 'blob' }).pipe(map((res: any) => res));
  }

  printPreview(APIString, id, userid, IsReturnForm) {
    if(IsReturnForm){
      return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userid + '&IsReturnForm=1', '');
    }else{
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userid + '&IsReturnForm=0', '');
    }
  }

  pdfToJson(refno) {
    return this.httpClient.get(environment.DownloadUrl + refno + '.pdf', { responseType: 'arraybuffer' as 'json' });
  }

    //Driver Management Calender list
    async getDriversList(PageNumber:number , PageSize:number,newDate:any): Promise<HttpResponse<any>> {
      const response = await this.httpClient.get<HttpResponse<any>>(this.endpoint.apiHostingURL+'/' + 'VehicleDriver' +'/' +PageNumber+','+ PageSize +'?CalendarViewDate='+newDate, { observe: 'response' }).toPromise();
      return response;
    }

    getDriversCalendar(PageNumber:number , PageSize:number,newDate:any){
      return this.httpClient.get<HttpResponse<any>>(this.endpoint.apiHostingURL+'/' + 'VehicleDriver' +'/' +PageNumber+','+ PageSize +'?CalendarViewDate='+newDate);
    }
  
    PreviewCalender(APIString, CalendarViewDate:any) {
      var date = new Date(); var timezone = date.getTimezoneOffset();
          return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString +'?CalendarViewDate=' + CalendarViewDate+'&TimeZone='+timezone, '');     
      }

}

