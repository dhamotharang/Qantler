'use strict';


customElements.define('compodoc-menu', class extends HTMLElement {
    constructor() {
        super();
        this.isNormalMode = this.getAttribute('mode') === 'normal';
    }

    connectedCallback() {
        this.render(this.isNormalMode);
    }

    render(isNormalMode) {
        let tp = lithtml.html(`
        <nav>
            <ul class="list">
                <li class="title">
                    <a href="index.html" data-type="index-link">Angular Client App documentation</a>
                </li>

                <li class="divider"></li>
                ${ isNormalMode ? `<div id="book-search-input" role="search"><input type="text" placeholder="Type to search"></div>` : '' }
                <li class="chapter">
                    <a data-type="chapter-link" href="index.html"><span class="icon ion-ios-home"></span>Getting started</a>
                    <ul class="links">
                        <li class="link">
                            <a href="overview.html" data-type="chapter-link">
                                <span class="icon ion-ios-keypad"></span>Overview
                            </a>
                        </li>
                        <li class="link">
                            <a href="index.html" data-type="chapter-link">
                                <span class="icon ion-ios-paper"></span>README
                            </a>
                        </li>
                                <li class="link">
                                    <a href="dependencies.html" data-type="chapter-link">
                                        <span class="icon ion-ios-list"></span>Dependencies
                                    </a>
                                </li>
                    </ul>
                </li>
                    <li class="chapter modules">
                        <a data-type="chapter-link" href="modules.html">
                            <div class="menu-toggler linked" data-toggle="collapse" ${ isNormalMode ?
                                'data-target="#modules-links"' : 'data-target="#xs-modules-links"' }>
                                <span class="icon ion-ios-archive"></span>
                                <span class="link-name">Modules</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                        </a>
                        <ul class="links collapse " ${ isNormalMode ? 'id="modules-links"' : 'id="xs-modules-links"' }>
                            <li class="link">
                                <a href="modules/AppModule.html" data-type="entity-link">AppModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                            'data-target="#components-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' : 'data-target="#xs-components-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' :
                                            'id="xs-components-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' }>
                                            <li class="link">
                                                <a href="components/AccountSettingsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">AccountSettingsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AnnouncementsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">AnnouncementsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AppComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">AppComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BreadcrumbsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">BreadcrumbsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CategoriesModalComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CategoriesModalComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CommunityCheckComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CommunityCheckComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CourseDetailComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CourseDetailComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CoursesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CoursesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CreateCourseComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CreateCourseComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CreateResourceComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CreateResourceComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DashboardComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">DashboardComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DashboardSideMenuComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">DashboardSideMenuComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/DiscoverComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">DiscoverComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/EditProfileComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">EditProfileComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ErrorComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ErrorComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FavouritesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FavouritesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FileViewerComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FileViewerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FooterComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">FooterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/HeaderComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">HeaderComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/IFrameViewerComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">IFrameViewerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LandingPageComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">LandingPageComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MakeProfileComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">MakeProfileComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MoeCheckComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">MoeCheckComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MyCoursesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">MyCoursesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MyNotificationsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">MyNotificationsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MyResourcesListComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">MyResourcesListComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/NotificationSettingsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">NotificationSettingsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PrivacySettingsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">PrivacySettingsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PublicProfileComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">PublicProfileComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/QRCComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">QRCComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RatingComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">RatingComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RecomendedArticlesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">RecomendedArticlesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ReportAbuseComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ReportAbuseComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ResourceDetailComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ResourceDetailComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ResourcesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ResourcesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SearchResultComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">SearchResultComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SensoryCheckComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">SensoryCheckComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ShareModalComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ShareModalComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SubBannerComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">SubBannerComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/TakeTestComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">TakeTestComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UserProfileComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">UserProfileComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/VerifyContentComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">VerifyContentComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/VerifyTestComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">VerifyTestComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WCMComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">WCMComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                        'data-target="#injectables-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' : 'data-target="#xs-injectables-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' :
                                        'id="xs-injectables-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' }>
                                        <li class="link">
                                            <a href="injectables/AuthGuard.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>AuthGuard</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/EncService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>EncService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/ProfileService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>ProfileService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/QrcService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>QrcService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/ResourceService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>ResourceService</a>
                                        </li>
                                    </ul>
                                </li>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                            'data-target="#pipes-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' : 'data-target="#xs-pipes-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' }>
                                            <span class="icon ion-md-add"></span>
                                            <span>Pipes</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="pipes-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' :
                                            'id="xs-pipes-links-module-AppModule-6dd0e10931211a38ff75f10099663904"' }>
                                            <li class="link">
                                                <a href="pipes/SafePipe.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">SafePipe</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/AppRoutingModule.html" data-type="entity-link">AppRoutingModule</a>
                            </li>
                </ul>
                </li>
                        <li class="chapter">
                            <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#injectables-links"' :
                                'data-target="#xs-injectables-links"' }>
                                <span class="icon ion-md-arrow-round-down"></span>
                                <span>Injectables</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                            <ul class="links collapse " ${ isNormalMode ? 'id="injectables-links"' : 'id="xs-injectables-links"' }>
                                <li class="link">
                                    <a href="injectables/AuthGuard.html" data-type="entity-link">AuthGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CourseService.html" data-type="entity-link">CourseService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ElasticSearchService.html" data-type="entity-link">ElasticSearchService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EncService.html" data-type="entity-link">EncService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GeneralService.html" data-type="entity-link">GeneralService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/NotificationService.html" data-type="entity-link">NotificationService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfileService.html" data-type="entity-link">ProfileService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QrcService.html" data-type="entity-link">QrcService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ResourceService.html" data-type="entity-link">ResourceService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/StorageUploadService.html" data-type="entity-link">StorageUploadService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/WCMService.html" data-type="entity-link">WCMService</a>
                                </li>
                            </ul>
                        </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#interceptors-links"' :
                            'data-target="#xs-interceptors-links"' }>
                            <span class="icon ion-ios-swap"></span>
                            <span>Interceptors</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="interceptors-links"' : 'id="xs-interceptors-links"' }>
                            <li class="link">
                                <a href="interceptors/BearerInterceptor.html" data-type="entity-link">BearerInterceptor</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#guards-links"' :
                            'data-target="#xs-guards-links"' }>
                            <span class="icon ion-ios-lock"></span>
                            <span>Guards</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="guards-links"' : 'id="xs-guards-links"' }>
                            <li class="link">
                                <a href="guards/ContributorGuard.html" data-type="entity-link">ContributorGuard</a>
                            </li>
                            <li class="link">
                                <a href="guards/ProfileUpdateGuard.html" data-type="entity-link">ProfileUpdateGuard</a>
                            </li>
                            <li class="link">
                                <a href="guards/VerifierGuard.html" data-type="entity-link">VerifierGuard</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#interfaces-links"' :
                            'data-target="#xs-interfaces-links"' }>
                            <span class="icon ion-md-information-circle-outline"></span>
                            <span>Interfaces</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? ' id="interfaces-links"' : 'id="xs-interfaces-links"' }>
                            <li class="link">
                                <a href="interfaces/list.html" data-type="entity-link">list</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ? 'data-target="#miscellaneous-links"'
                            : 'data-target="#xs-miscellaneous-links"' }>
                            <span class="icon ion-ios-cube"></span>
                            <span>Miscellaneous</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="miscellaneous-links"' : 'id="xs-miscellaneous-links"' }>
                            <li class="link">
                                <a href="miscellaneous/functions.html" data-type="entity-link">Functions</a>
                            </li>
                            <li class="link">
                                <a href="miscellaneous/variables.html" data-type="entity-link">Variables</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <a data-type="chapter-link" href="routes.html"><span class="icon ion-ios-git-branch"></span>Routes</a>
                        </li>
                    <li class="divider"></li>
                    <li class="copyright">
                        Documentation generated using <a href="https://compodoc.app/" target="_blank">
                            <img data-src="images/compodoc-vectorise.png" class="img-responsive" data-type="compodoc-logo">
                        </a>
                    </li>
            </ul>
        </nav>
        `);
        this.innerHTML = tp.strings;
    }
});