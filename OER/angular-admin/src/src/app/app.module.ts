import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {APP_INITIALIZER, NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {RouterModule, Routes} from '@angular/router';
import {EditorModule} from '@tinymce/tinymce-angular';
import {KeycloakAngularModule, KeycloakService} from 'keycloak-angular';
import {CookieService} from 'ngx-cookie-service';
import {NgxCoolDialogsModule} from 'ngx-cool-dialogs';
import {NgxSpinnerModule} from 'ngx-spinner';
import {MessageService} from 'primeng/components/common/messageservice';
import {MessageModule} from 'primeng/message';
import {MessagesModule} from 'primeng/messages';
import {
  CheckboxModule,
  ConfirmationService,
  ConfirmDialogModule,
  DialogModule,
  InputTextareaModule,
  MultiSelectModule,
  PaginatorModule,
  ProgressBarModule,
} from 'primeng/primeng';
import {TableModule} from 'primeng/table';
import {ToastModule} from 'primeng/toast';
import {initializer} from './app-init';
import {AppComponent} from './app.component';
import {ErrorComponent} from './common/error/error.component';
import {FooterComponent} from './common/footer/footer.component';
import {HeaderComponent} from './common/header/header.component';
import {PagetopComponent} from './common/pagetop/pagetop.component';
import {SidebarComponent} from './common/sidebar/sidebar.component';
import {AuthGuard} from './guards/auth.guard';
import {ProfileUpdateGuard} from './guards/profile-update.guard';
import {BearerInterceptor} from './interceptors/bearer.interceptor';
import {AnnouncementsComponent} from './pages/announcements/announcements.component';
import {CommunityThresholdComponent} from './pages/community-threshold/community-threshold.component';
import {ContactQueriesComponent} from './pages/contact-queries/contact-queries.component';
import {CoursesComponent} from './pages/courses/courses.component';
import {LandingPageComponent} from './pages/landing-page/landing-page.component';
import {MdmComponent} from './pages/mdm/mdm.component';
import {QrcReportsComponent} from './pages/qrc-reports/qrc-reports.component';
import {QrcComponent} from './pages/qrc/qrc.component';
import {RejectionListComponent} from './pages/rejection-list/rejection-list.component';
import {ReportAbuseComponent} from './pages/report-abuse/report-abuse.component';
import {ResourcesComponent} from './pages/resources/resources.component';
import {UrlManagementComponent} from './pages/url-management/url-management.component';
import {UsersComponent} from './pages/users/users.component';
import {VerifierReportsComponent} from './pages/verifier-reports/verifier-reports.component';
import {WCMManagementComponent} from './pages/wcmmanagement/wcmmanagement.component';
import {CoursesService} from './services/courses/courses.service';
import {EncService} from './services/enc.service';
import {MdmServiceService} from './services/mdm/mdm-service.service';
import {ProfileService} from './services/profile.service';
import {QrcService} from './services/qrc.service';
import {ResourcesService} from './services/resources/resources.service';
import {StorageUploadService} from './services/storage-upload.service';

/**
 * Application Routes
 */
const routes: Routes = [
  {path: '', redirectTo: '/', pathMatch: 'full'},
  {path: '', component: LandingPageComponent},
  {path: 'qrc-list', canActivate: [AuthGuard, ProfileUpdateGuard], component: QrcComponent},
  {path: 'announcements', canActivate: [AuthGuard, ProfileUpdateGuard], component: AnnouncementsComponent},
  {path: 'master-data-management', canActivate: [AuthGuard, ProfileUpdateGuard], component: MdmComponent},
  {path: 'external-url-management/:type', canActivate: [AuthGuard, ProfileUpdateGuard], component: UrlManagementComponent},
  {path: 'contact-queries', canActivate: [AuthGuard, ProfileUpdateGuard], component: ContactQueriesComponent},
  {path: 'rejection-list', canActivate: [AuthGuard, ProfileUpdateGuard], component: RejectionListComponent},
  {path: 'courses', canActivate: [AuthGuard, ProfileUpdateGuard], component: CoursesComponent},
  {path: 'resources', canActivate: [AuthGuard, ProfileUpdateGuard], component: ResourcesComponent},
  {path: 'users', canActivate: [AuthGuard, ProfileUpdateGuard], component: UsersComponent},
  {path: 'abuse-reports', canActivate: [AuthGuard, ProfileUpdateGuard], component: ReportAbuseComponent},
  {path: 'wcm-management', canActivate: [AuthGuard, ProfileUpdateGuard], component: WCMManagementComponent},
  {path: 'qrc-reports', canActivate: [AuthGuard, ProfileUpdateGuard], component: QrcReportsComponent},
  {path: 'verifier-reports', canActivate: [AuthGuard, ProfileUpdateGuard], component: VerifierReportsComponent},
  {path: 'community-threshold', canActivate: [AuthGuard, ProfileUpdateGuard], component: CommunityThresholdComponent},
  {path: 'error/:code', component: ErrorComponent},
  {path: '**', redirectTo: 'error/404'},
  ];

/**
 * Application Main Module
 */
@NgModule({
  declarations: [
    AppComponent,
    ErrorComponent,
    LandingPageComponent,
    HeaderComponent,
    SidebarComponent,
    FooterComponent,
    QrcComponent,
    MdmComponent,
    UrlManagementComponent,
    ResourcesComponent,
    CoursesComponent,
    PagetopComponent,
    WCMManagementComponent,
    UsersComponent,
    ReportAbuseComponent,
    ContactQueriesComponent,
    AnnouncementsComponent,
    QrcReportsComponent,
    RejectionListComponent,
    CommunityThresholdComponent,
    VerifierReportsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    KeycloakAngularModule,
    ProgressBarModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    PaginatorModule,
    DialogModule,
    MultiSelectModule,
    CheckboxModule,
    NgxCoolDialogsModule.forRoot({
      theme: 'default', // available themes: 'default' | 'material' | 'dark'
      okButtonText: 'Confirm',
      cancelButtonText: 'Cancel',
      color: '#243b76',
      titles: {
        alert: 'Danger!',
        confirm: 'Confirmation',
        prompt: 'Website asks...'
      }
    }),
    MessagesModule,
    TableModule,
    ConfirmDialogModule,
    InputTextareaModule,
    MessageModule,
    ToastModule,
    NgxSpinnerModule,
    EditorModule
  ],
  providers: [{
    provide: APP_INITIALIZER,
    useFactory: initializer,
    multi: true,
    deps: [KeycloakService]
  },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BearerInterceptor,
      multi: true
    },
    QrcService,
    EncService,
    ProfileService,
    CookieService,
    MdmServiceService,
    MessageService,
    ConfirmationService,
    ResourcesService,
    CoursesService,
    StorageUploadService,
    AuthGuard,
    ProfileUpdateGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
