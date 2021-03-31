import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-maintenance-container',
  templateUrl: './maintenance-container.component.html',
  styleUrls: ['./maintenance-container.component.scss']
})
export class MaintenanceContainerComponent implements OnInit {

  constructor(public router: Router,route: ActivatedRoute) { }

  ngOnInit() {
    
  }

}
