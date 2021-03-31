import { Component, OnInit } from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import {ProfileService} from '../../services/profile.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  user: any;
  constructor(private  keycloakService: KeycloakService,
    private profileService: ProfileService) { }
  ngOnInit() {
    this.user = [];
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.user = this.profileService.user;
    });
  }
  signout() {
    this.keycloakService.logout();
  }

}
