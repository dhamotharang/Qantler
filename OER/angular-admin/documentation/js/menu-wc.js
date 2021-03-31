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
                    <a href="index.html" data-type="index-link">Angular Admin App documentation</a>
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
                                            'data-target="#components-links-module-AppModule-405620c94a5beec9fe6ca439a587e57c"' : 'data-target="#xs-components-links-module-AppModule-405620c94a5beec9fe6ca439a587e57c"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AppModule-405620c94a5beec9fe6ca439a587e57c"' :
                                            'id="xs-components-links-module-AppModule-405620c94a5beec9fe6ca439a587e57c"' }>
                                            <li class="link">
                                                <a href="components/AnnouncementsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">AnnouncementsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AppComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">AppComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CommunityThresholdComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CommunityThresholdComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ContactQueriesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ContactQueriesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CoursesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">CoursesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ErrorComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ErrorComponent</a>
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
                                                <a href="components/LandingPageComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">LandingPageComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MdmComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">MdmComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PagetopComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">PagetopComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/QrcComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">QrcComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/QrcReportsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">QrcReportsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RejectionListComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">RejectionListComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ReportAbuseComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ReportAbuseComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ResourcesComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">ResourcesComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SidebarComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">SidebarComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UrlManagementComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">UrlManagementComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UsersComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">UsersComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/VerifierReportsComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">VerifierReportsComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/WCMManagementComponent.html"
                                                    data-type="entity-link" data-context="sub-entity" data-context-id="modules">WCMManagementComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-toggle="collapse" ${ isNormalMode ?
                                        'data-target="#injectables-links-module-AppModule-405620c94a5beec9fe6ca439a587e57c"' : 'data-target="#xs-injectables-links-module-AppModule-405620c94a5beec9fe6ca439a587e57c"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-AppModule-405620c94a5beec9fe6ca439a587e57c"' :
                                        'id="xs-injectables-links-module-AppModule-405620c94a5beec9fe6ca439a587e57c"' }>
                                        <li class="link">
                                            <a href="injectables/AuthGuard.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>AuthGuard</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/CoursesService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>CoursesService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/EncService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>EncService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/MdmServiceService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>MdmServiceService</a>
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
                                            <a href="injectables/ResourcesService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>ResourcesService</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/StorageUploadService.html"
                                                data-type="entity-link" data-context="sub-entity" data-context-id="modules" }>StorageUploadService</a>
                                        </li>
                                    </ul>
                                </li>
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
                                    <a href="injectables/AbuseReportService.html" data-type="entity-link">AbuseReportService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AnnouncementsService.html" data-type="entity-link">AnnouncementsService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AuthGuard.html" data-type="entity-link">AuthGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/CoursesService.html" data-type="entity-link">CoursesService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EncService.html" data-type="entity-link">EncService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/GeneralService.html" data-type="entity-link">GeneralService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/MdmServiceService.html" data-type="entity-link">MdmServiceService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ProfileService.html" data-type="entity-link">ProfileService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/QrcService.html" data-type="entity-link">QrcService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ResourcesService.html" data-type="entity-link">ResourcesService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/StorageUploadService.html" data-type="entity-link">StorageUploadService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/URLService.html" data-type="entity-link">URLService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/UsersService.html" data-type="entity-link">UsersService</a>
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
                                <a href="guards/ProfileUpdateGuard.html" data-type="entity-link">ProfileUpdateGuard</a>
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