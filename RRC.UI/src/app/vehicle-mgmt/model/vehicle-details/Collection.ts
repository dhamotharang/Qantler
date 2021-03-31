import { DriverTrips } from './DriverTrips';
import { OrganizationList } from './OrganizationList';
import { M_LookupsList } from './M_LookupsList';

export class Collection{
    DriverID:number;
    UserProfileID:number;
    TotalHour:number;
    DriverName:string="";
    MobileNumber:string="";
    StartDate:any;
    EndDate:any
    CompensateExtra:string=""
    CreatedBy:number;
    UpdatedBy:number
    CreatedDateTime:any
    UpdatedDateTime:any
    DeleteFlag:boolean
    OrganizationList:OrganizationList[]=[];
    M_LookupsList:M_LookupsList[]=[];
    DriverTrips:DriverTrips[]=[];
}