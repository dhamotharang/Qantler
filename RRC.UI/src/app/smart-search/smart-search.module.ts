import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { SmartSearchRoutingModule } from './smart-search-routing.module';
import { SearchContainerComponent } from './components/search-container/search-container.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [SearchContainerComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgbPaginationModule,
    ReactiveFormsModule,
    SmartSearchRoutingModule,
    SharedModule
  ],
  exports:[
    SearchContainerComponent
  ]
})
export class SmartSearchModule { }
