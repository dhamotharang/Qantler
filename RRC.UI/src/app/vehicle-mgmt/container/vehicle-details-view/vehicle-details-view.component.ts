import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-vehicle-details-view',
  templateUrl: './vehicle-details-view.component.html',
  styleUrls: ['./vehicle-details-view.component.scss']
})
export class VehicleDetailsViewComponent implements OnInit {
  requestId:Number;
  constructor(private route: ActivatedRoute, private commonService:CommonService) {
    this.route.params.subscribe(params => this.requestId = +params.id);  
  }

  ngOnInit() {}
}
