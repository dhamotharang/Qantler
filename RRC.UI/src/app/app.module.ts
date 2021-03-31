import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MemoComponent } from './pages/pages.component';

// import { TaskModule } from './task/task.module';
import { HrModule } from './hr/hr.module';
import { CitizenAffairModule } from './citizen-affair/citizen-affair.module';
import { ItModule } from './it/it.module';
import { LayoutModule } from './layout/layout.module';
import { MemoModule } from './memo/memo.module';
import { LetterModule } from './letter/letter.module';
import { CircularModule } from './circular/circular.module';
import { MediaModule } from './media/media.module';
import { PhotographerModule } from './media/photographer/photographer.module';
import { PopupModule } from './modal/popup.module';
import { MaintenanceModule } from './maintenance/maintenance.module';
import { HttpModule } from '@angular/http';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CommonPageModule } from './commonpage.module';
import { SharedModule } from './shared/shared.module';
import { FullCalendarModule } from '@fullcalendar/angular';
import { InterceptService } from './auth/interceptor.service';
import { AuthGuard } from './auth/auth.guard';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';
import { ScrollToModule } from '@nicky-lenaers/ngx-scroll-to';
import { UrlSerializer } from '@angular/router';
import { CustomUrlSerializer } from './CustomUrlSerializer';

@NgModule({
  declarations: [
    AppComponent,
    MemoComponent,
  ],

  imports: [
    HttpModule,
    HttpClientModule,
    BrowserModule,
    ScrollToModule.forRoot(),
    BrowserAnimationsModule,
    AppRoutingModule,
    LayoutModule,
    SharedModule,
    // MemoModule,
    // LetterModule,
    // CircularModule,
    // TaskModule,
    // HrModule,
    // ItModule,
    // MaintenanceModule,
    // CitizenAffairModule,
    PhotographerModule,
    MediaModule,
    PopupModule,
    CommonPageModule,
    FullCalendarModule
  ],
  providers: [AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: InterceptService, multi: true }
    ,{ provide: UrlSerializer, useClass: CustomUrlSerializer }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
