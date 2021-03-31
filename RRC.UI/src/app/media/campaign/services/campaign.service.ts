import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient } from '@angular/common/http';
import { Campaign } from '../models/campaign.model';

@Injectable()
export class CampaignService {
  campaignUrl = '/Campaign/';  
  constructor(private endpoint: EndPointService, private httpClient: HttpClient) { }

  create(campaign: Campaign) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.campaignUrl, campaign);
  }

  update(id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + this.campaignUrl + id, dataToUpdate);
  }
  
  getCampaignRequestById(id:any, userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}${this.campaignUrl}${id}?UserId=${userId}`);
  }
  reSubmit(campaign: Campaign) {
    return this.httpClient.put(this.endpoint.apiHostingURL + this.campaignUrl, campaign);
  }
}
