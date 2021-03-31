import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LandingPageComponent} from './pages/landing-page/landing-page.component';
import {AuthGuard} from './guards/auth.guard';
import {MakeProfileComponent} from './pages/make-profile/make-profile.component';
import {PublicProfileComponent} from './pages/public-profile/public-profile.component';
import {SearchResultComponent} from './pages/search-result/search-result.component';
import {DashboardComponent} from './pages/dashboard/dashboard.component';
import {UserProfileComponent} from './pages/user-profile/user-profile.component';
import {EditProfileComponent} from './pages/edit-profile/edit-profile.component';
import {AccountSettingsComponent} from './pages/account-settings/account-settings.component';
import {NotificationSettingsComponent} from './pages/notification-settings/notification-settings.component';
import {AnnouncementsComponent} from './pages/announcements/announcements.component';
import {ProfileUpdateGuard} from './guards/profile-update.guard';
import {MyResourcesListComponent} from './pages/my-resources-list/my-resources-list.component';
import {MyCoursesComponent} from './pages/my-courses/my-courses.component';
import {QRCComponent} from './pages/qrc/qrc.component';
import {DiscoverComponent} from './pages/discover/discover.component';
import {ResourcesComponent} from './pages/resources/resources.component';
import {CreateResourceComponent} from './pages/create-resource/create-resource.component';
import {ResourceDetailComponent} from './pages/resource-detail/resource-detail.component';
import {CoursesComponent} from './pages/courses/courses.component';
import {CreateCourseComponent} from './pages/create-course/create-course.component';
import {CourseDetailComponent} from './pages/course-detail/course-detail.component';
import {VerifyContentComponent} from './pages/verify-content/verify-content.component';
import {VerifyTestComponent} from './pages/verify-test/verify-test.component';
import {TakeTestComponent} from './pages/take-test/take-test.component';
import {ErrorComponent} from './common/error/error.component';
import {WCMComponent} from './pages/wcm/wcm.component';
import {ContributorGuard} from './guards/contributor.guard';
import {FavouritesComponent} from './pages/favourites/favourites.component';
import {MyNotificationsComponent} from './pages/my-notifications/my-notifications.component';
import {VerifierGuard} from './guards/verifier.guard';
import {CommunityCheckComponent} from './pages/community-check/community-check.component';
import {SensoryCheckComponent} from './pages/sensory-check/sensory-check.component';
import {MoeCheckComponent} from './pages/moe-check/moe-check.component';
import {NotSupportedComponent} from './pages/not-supported/not-supported.component';
import {BrowserCheckGuard} from './guards/browser-check.guard';

const routes: Routes = [
  {path: '', redirectTo: '/', pathMatch: 'full'},
  {path: '', component: LandingPageComponent, canActivate: [BrowserCheckGuard]},
  {path: 'make-profile', canActivate: [AuthGuard, BrowserCheckGuard], component: MakeProfileComponent},
  {path: 'user/profile/:slug', canActivate: [AuthGuard, BrowserCheckGuard], component: PublicProfileComponent},
  {path: 'search', component: SearchResultComponent, canActivate: [BrowserCheckGuard], data: {f: {}, q: ''}},
  {
    path: 'dashboard/profile/edit',
    canActivate: [AuthGuard, ProfileUpdateGuard, BrowserCheckGuard],
    component: EditProfileComponent
  },
  {path: 'dashboard/profile', canActivate: [AuthGuard, BrowserCheckGuard], component: UserProfileComponent},
  {
    path: 'dashboard/notification-settings',
    canActivate: [AuthGuard, BrowserCheckGuard],
    component: NotificationSettingsComponent
  },
  {path: 'dashboard/announcements', canActivate: [AuthGuard, BrowserCheckGuard], component: AnnouncementsComponent},
  {path: 'dashboard/favourites', canActivate: [AuthGuard, BrowserCheckGuard], component: FavouritesComponent},
  {
    path: 'dashboard/my-notifications',
    canActivate: [AuthGuard, BrowserCheckGuard],
    component: MyNotificationsComponent
  },
  {
    path: 'dashboard/resources/:type',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: MyResourcesListComponent
  },
  {
    path: 'dashboard/resources',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: MyResourcesListComponent
  },
  {
    path: 'dashboard/courses/:type',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: MyCoursesComponent
  },
  {
    path: 'dashboard/courses',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: MyCoursesComponent
  },
  {path: 'dashboard/qrc', canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard], component: QRCComponent},
  {
    path: 'dashboard/community-check',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: CommunityCheckComponent
  },
  {
    path: 'dashboard/sensitivity-check',
    canActivate: [AuthGuard, VerifierGuard, BrowserCheckGuard],
    component: SensoryCheckComponent
  },
  {
    path: 'dashboard/expert-check',
    canActivate: [AuthGuard, VerifierGuard, BrowserCheckGuard],
    component: MoeCheckComponent
  },
  {path: 'dashboard', canActivate: [AuthGuard, BrowserCheckGuard], component: DashboardComponent},
  {path: 'discover', component: DiscoverComponent, canActivate: [BrowserCheckGuard]},
  {
    path: 'resources/create/:slug',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: CreateResourceComponent
  },
  {
    path: 'resources/create',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: CreateResourceComponent
  },
  {path: 'resource/:slug', component: ResourceDetailComponent},
  {path: 'resources', component: ResourcesComponent, canActivate: [BrowserCheckGuard], data: {q: '', f: {}}},
  {
    path: 'courses/create/:slug',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: CreateCourseComponent
  },
  {
    path: 'courses/create',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: CreateCourseComponent
  },
  {path: 'course/:slug', component: CourseDetailComponent, canActivate: [BrowserCheckGuard]},
  {path: 'courses', component: CoursesComponent, canActivate: [BrowserCheckGuard], data: {q: '', f: {}}},
  {
    path: 'verify-content/:type/:id/:contentApprovalId/:checkType/:cat',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: VerifyContentComponent
  },
  {
    path: 'verify-test/:id',
    canActivate: [AuthGuard, ContributorGuard, BrowserCheckGuard],
    component: VerifyTestComponent
  },
  {path: 'take-test/:id', canActivate: [AuthGuard, BrowserCheckGuard], component: TakeTestComponent},
  {path: 'error/:code', component: ErrorComponent, canActivate: [BrowserCheckGuard]},
  {path: 'not-supported', component: NotSupportedComponent},
  {path: ':wcm', component: WCMComponent, canActivate: [BrowserCheckGuard]},
  {path: '**', redirectTo: 'error/404', canActivate: [BrowserCheckGuard]},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
