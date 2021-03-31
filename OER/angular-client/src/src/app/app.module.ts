import {BrowserModule, Meta, Title} from '@angular/platform-browser';
import {APP_INITIALIZER, NgModule} from '@angular/core';
import '../icons';
import {ProgressBarModule} from 'angular-progress-bar';
import {AppComponent} from './app.component';
import {RouterModule, Routes} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {LandingPageComponent} from './pages/landing-page/landing-page.component';
import {MakeProfileComponent} from './pages/make-profile/make-profile.component';
import {KeycloakAngularModule, KeycloakService} from 'keycloak-angular';
import {AuthGuard} from './guards/auth.guard';
import {initializer} from './app-init';
import {DashboardComponent} from './pages/dashboard/dashboard.component';
import {HeaderComponent} from './includes/header/header.component';
import {FooterComponent} from './includes/footer/footer.component';
import {SubBannerComponent} from './includes/sub-banner/sub-banner.component';
import {BreadcrumbsComponent} from './includes/breadcrumbs/breadcrumbs.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {TooltipModule} from 'ngx-bootstrap';
import {
  ButtonModule,
  DialogModule,
  CheckboxModule,
  CalendarModule,
  AutoCompleteModule,
  MessageService,
  MessageModule,
  MessagesModule,
  RatingModule,
  AccordionModule,
  SliderModule,
  FieldsetModule,
  ConfirmationService,
  ConfirmDialogModule,
  InputSwitchModule,
  InputTextareaModule,
  PaginatorModule,
  LightboxModule,
  MultiSelectModule
} from 'primeng/primeng';
import {EditorModule} from '@tinymce/tinymce-angular';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import {ProfileService} from './services/profile.service';
import {ProfileUpdateGuard} from './guards/profile-update.guard';
import {CookieService} from 'ngx-cookie-service';
import {NgxSpinnerModule} from 'ngx-spinner';
import {ResourcesComponent} from './pages/resources/resources.component';
import {CoursesComponent} from './pages/courses/courses.component';
import {CreateResourceComponent} from './pages/create-resource/create-resource.component';
import {CreateCourseComponent} from './pages/create-course/create-course.component';
import {ResourceService} from './services/resource.service';
import {ToastModule} from 'primeng/toast';
import {ResourceDetailComponent} from './pages/resource-detail/resource-detail.component';
import {ReportAbuseComponent} from './common/report-abuse/report-abuse.component';
import {CategoriesModalComponent} from './common/categories-modal/categories-modal.component';
import {ShareButtonsModule} from '@ngx-share/buttons';
import {ShareModalComponent} from './common/share-modal/share-modal.component';
import {RecomendedArticlesComponent} from './common/recomended-articles/recomended-articles.component';
import {BearerInterceptor} from './interceptors/bearer.interceptor';
import {EncService} from './services/enc.service';
import {MyResourcesListComponent} from './pages/my-resources-list/my-resources-list.component';
import {UserProfileComponent} from './pages/user-profile/user-profile.component';
import {DashboardSideMenuComponent} from './common/dashboard-side-menu/dashboard-side-menu.component';
import {EditProfileComponent} from './pages/edit-profile/edit-profile.component';
import {CourseDetailComponent} from './pages/course-detail/course-detail.component';
import {QRCComponent} from './pages/qrc/qrc.component';
import {QrcService} from './services/qrc.service';
import {WCMComponent} from './pages/wcm/wcm.component';
import {VerifyContentComponent} from './pages/verify-content/verify-content.component';
import {VerifyTestComponent} from './pages/verify-test/verify-test.component';
import {TakeTestComponent} from './pages/take-test/take-test.component';
import {PDFExportModule} from '@progress/kendo-angular-pdf-export';
import {MyCoursesComponent} from './pages/my-courses/my-courses.component';
import {AccountSettingsComponent} from './pages/account-settings/account-settings.component';
import {PrivacySettingsComponent} from './pages/privacy-settings/privacy-settings.component';
import {NotificationSettingsComponent} from './pages/notification-settings/notification-settings.component';
import {OwlModule} from 'ngx-owl-carousel';
import {DiscoverComponent} from './pages/discover/discover.component';
import {PublicProfileComponent} from './pages/public-profile/public-profile.component';
import {ErrorComponent} from './common/error/error.component';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import {AnnouncementsComponent} from './pages/announcements/announcements.component';
import {SafePipe} from './pipes/safe.pipe';
import {SearchResultComponent} from './pages/search-result/search-result.component';
import {FileViewerComponent} from './common/file-viewer/file-viewer.component';
import {NgxDocViewerModule} from 'ngx-doc-viewer';
import {NgxAudioPlayerModule} from 'ngx-audio-player';
import {MatVideoModule} from 'mat-video';
import {ImageViewerModule} from 'ngx-image-viewer';
import {DatePipe} from '@angular/common';
import {AppRoutingModule} from './app-routing.module';
import {BarRatingModule} from 'ngx-bar-rating';
import {ContributorGuard} from './guards/contributor.guard';
import {IFrameViewerComponent} from './common/iframe-viewer/iframe-viewer.component';
import {TagInputModule} from 'ngx-chips';
import {FavouritesComponent} from './pages/favourites/favourites.component';
import {RatingComponent} from './common/rating/rating.component';
import {MyNotificationsComponent} from './pages/my-notifications/my-notifications.component';
import {SimpleTimer} from 'ng2-simple-timer';
import {StarRatingModule} from 'angular-star-rating';
import {CommunityCheckComponent} from './pages/community-check/community-check.component';
import {SensoryCheckComponent} from './pages/sensory-check/sensory-check.component';
import {MoeCheckComponent} from './pages/moe-check/moe-check.component';
import {DeviceDetectorModule} from 'ngx-device-detector';
import { NotSupportedComponent } from './pages/not-supported/not-supported.component';
import { AngularFileViewerModule } from '@taldor-ltd/angular-file-viewer';
import { Ng5SliderModule } from 'ng5-slider';

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

// @ts-ignore
@NgModule({
  declarations: [
    AppComponent,
    LandingPageComponent,
    MakeProfileComponent,
    DashboardComponent,
    HeaderComponent,
    FooterComponent,
    SubBannerComponent,
    BreadcrumbsComponent,
    ResourcesComponent,
    CoursesComponent,
    CreateResourceComponent,
    CreateCourseComponent,
    ResourceDetailComponent,
    ReportAbuseComponent,
    CategoriesModalComponent,
    ShareModalComponent,
    RecomendedArticlesComponent,
    MyResourcesListComponent,
    UserProfileComponent,
    DashboardSideMenuComponent,
    EditProfileComponent,
    CourseDetailComponent,
    QRCComponent,
    WCMComponent,
    VerifyContentComponent,
    VerifyTestComponent,
    TakeTestComponent,
    MyCoursesComponent,
    AccountSettingsComponent,
    PrivacySettingsComponent,
    NotificationSettingsComponent,
    DiscoverComponent,
    PublicProfileComponent,
    ErrorComponent,
    AnnouncementsComponent,
    SafePipe,
    SearchResultComponent,
    FileViewerComponent,
    IFrameViewerComponent,
    FavouritesComponent,
    RatingComponent,
    MyNotificationsComponent,
    CommunityCheckComponent,
    SensoryCheckComponent,
    MoeCheckComponent,
    NotSupportedComponent
  ],
  imports: [
    BrowserModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    FormsModule,
    DeviceDetectorModule.forRoot(),
    BrowserAnimationsModule,
    AppRoutingModule,
    KeycloakAngularModule,
    HttpClientModule,
    ButtonModule,
    DialogModule,
    PaginatorModule,
    CheckboxModule,
    InputSwitchModule,
    FieldsetModule,
    CalendarModule,
    ImageViewerModule.forRoot(),
    MatVideoModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    PDFExportModule,
    AutoCompleteModule,
    ProgressBarModule,
    EditorModule,
    ToastModule,
    MessagesModule,
    MessageModule,
    StarRatingModule.forRoot(),
    BarRatingModule,
    TagInputModule,
    MultiSelectModule,
    AccordionModule,
    ShareButtonsModule,
    ConfirmDialogModule,
    InputTextareaModule,
    SliderModule,
    FieldsetModule,
    NgxDocViewerModule,
    LightboxModule,
    NgxAudioPlayerModule,
    OwlModule,
    TooltipModule.forRoot(),
    AngularFileViewerModule,
    Ng5SliderModule
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
    Title,
    Meta,
    AuthGuard,
    ProfileService,
    CookieService,
    ProfileUpdateGuard,
    ContributorGuard,
    ResourceService,
    MessageService,
    EncService,
    ConfirmationService,
    SimpleTimer,
    QrcService,
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
