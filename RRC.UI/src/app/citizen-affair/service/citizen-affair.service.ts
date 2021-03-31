import { Injectable } from "@angular/core";
import { EndPointService } from "../../api/endpoint.service";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import {
  ScrollToService,
  ScrollToConfigOptions
} from "@nicky-lenaers/ngx-scroll-to";
import { CommonService } from "src/app/common.service";
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: "root"
})
export class CitizenAffairService {
  refid: any;
  status: any;
  assignto: any;
  requesttype: any;
  requestdate: any;
  sourcefield: any;
  createdby: any;
  phonenumber: any;
  action: any;
  personallocation: any;
  unitId = [5, 6, 7, 8];

  constructor(
    public endpoint: EndPointService,
    public common: CommonService,
    public httpClient: HttpClient,
    public _ScrollToService: ScrollToService
  ) {
    this.refid =
      this.common.language == "English" ? "Ref ID" : this.arabic("refid");
    this.status =
      this.common.language == "English" ? "Status" : this.arabic("status");
    this.assignto =
      this.common.language == "English"
        ? "Assign To"
        : this.arabic("assignedto");
    this.requesttype =
      this.common.language == "English"
        ? "Request Type"
        : this.arabic("requesttype");
    this.requestdate =
      this.common.language == "English"
        ? "Request Date"
        : this.arabic("requestdate");
    this.sourcefield =
      this.common.language == "English" ? "Reporter" : this.arabic("reporter");
    this.createdby =
      this.common.language == "English"
        ? "Created By"
        : this.arabic("createdby");
    this.phonenumber =
      this.common.language == "English"
        ? "Phone Number"
        : this.arabic("phonenumber");
    this.action =
      this.common.language == "English" ? "Action" : this.arabic("action");
    this.personallocation =
      this.common.language == "English"
        ? "Personal/ Location Name"
        : this.arabic("personal/locationname");
  }
  triggerScrollTo() {
    const config: ScrollToConfigOptions = {
      target: "destination",
      offset: 200
    };

    this._ScrollToService.scrollTo(config);
  }

  getCount(id) {
    return this.httpClient.get(
      this.endpoint.apiHostingURL +
        "/CitizenAffair/AllModulesPendingCount/" +
        id
    );
  }

  getList(pagenumber, pagesize, id, type, filter) {
    var req = Object.keys(filter);
    var param = "";
    req.map(res => {
      if (filter[res] != "") {
        // if (res == "ReqDateFrom" || res == "ReqDateTo")
        //   filter[res] = filter[res].toJSON();
        param += "&" + res + "=" + filter[res];
      }
    });
    return this.httpClient.get(
      this.endpoint.apiHostingURL +
        "/CitizenAffair/" +
        pagenumber +
        "," +
        pagesize +
        "?UserID=" +
        id +
        "&Type=" +
        type +
        param
    );
  }

  saveFormPost(APIString, data, userId) {
    return this.httpClient.post(
      this.endpoint.apiHostingURL + "/" + APIString + "?UserID=" + userId,
      data
    );
  }

  saveFormPut(APIString, data, userId) {
    return this.httpClient.put(
      this.endpoint.apiHostingURL + "/" + APIString + "?UserID=" + userId,
      data
    );
  }

  saveComplaintSuggestion(APIString, data: any) {
    return this.httpClient.post(
      this.endpoint.apiHostingURL + "/" + APIString,
      data
    );
  }

  getDataById(APIString, id, userId) {
    return this.httpClient.get(
      this.endpoint.apiHostingURL +
        "/" +
        APIString +
        "/" +
        id +
        "?UserID=" +
        userId
    );
  }
  getComplaintSuggestion(APIString, id, userId) {
    return this.httpClient.get(
      this.endpoint.apiHostingURL +
        "/" +
        APIString +
        "/" +
        id +
        "?UserID=" +
        userId
    );
  }

  deleteList(APIString, id, userId) {
    return this.httpClient.delete(
      this.endpoint.apiHostingURL +
        "/" +
        APIString +
        "/" +
        id +
        "?UserID=" +
        userId
    );
  }

  statusChange(APIString: string, id, data: any) {
    return this.httpClient.patch(
      this.endpoint.apiHostingURL + "/" + APIString + "/" + id,
      data
    );
  }

  getEmiratesList(userID) {
    return this.httpClient.get(
      this.endpoint.apiHostingURL + "/Emirates?UserID=" + userID
    );
  }

  getLocationList(userID) {
    return this.httpClient.get(
      this.endpoint.apiHostingURL + "/Location?UserID=" + userID
    );
  }

  getCityList(userID) {
    return this.httpClient.get(
      this.endpoint.apiHostingURL + "/City?UserID=" + userID
    );
  }

  getCityListbyID(userID, emiratesID) {
    return this.httpClient.get(
      this.endpoint.apiHostingURL +
        "/City?UserID=" +
        userID +
        "&EmiratesID=" +
        emiratesID
    );
  }

  getHrDocumentsList(options): Observable<any> {
    let toSendParams: any = {
      Type: "HR",
      UserID: options.UserID
    };
    if (options.Creator) {
      toSendParams.Creator = options.Creator;
    }

    if (options.SmartSearch) {
      toSendParams.SmartSearch = options.SmartSearch;
    }
    return this.httpClient.get(
      this.endpoint.apiHostingURL +
        "/HR/Document/" +
        `${options.PageNumber},${options.PageSize}`,
      { params: toSendParams }
    );
  }

  deleteHrDocument(options): Observable<any> {
    let toSendParams = {
      UserID: options.UserID
    };
    return this.httpClient.delete(
      this.endpoint.apiHostingURL + "/HR/Document/" + options.AttachmentID,
      { params: toSendParams }
    );
  }

  downlaodExcel(data) {
    var date = new Date(),
      cur_date =
        date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
    this.httpClient
      .post(
        this.endpoint.apiHostingURL + "/CitizenAffair/Report?type=Excel",
        data,
        { responseType: "blob" }
      )
      .subscribe((resultBlob: Blob) => {
        var url = window.URL.createObjectURL(resultBlob);
        var a = document.createElement("a");
        document.body.appendChild(a);
        a.setAttribute("style", "display: none");
        a.href = url;
        a.download = "CitizenAffairReport-" + cur_date + ".xlsx";
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
      });
  }

  printPreview(APIString, id, userid) {
    return this.httpClient.post(
      this.endpoint.apiHostingURL +
        "/" +
        APIString +
        "/" +
        id +
        "?UserID=" +
        userid,
      ""
    );
  }

  pdfToJson(refno) {
    return this.httpClient.get(environment.DownloadUrl + refno + ".pdf", {
      responseType: "arraybuffer" as "json"
    });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
