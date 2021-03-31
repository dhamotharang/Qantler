import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { LayoutComponent } from './layout.component';
import { PageHeaderComponent } from './components/english/page-header/page-header.component';
import { PageHeaderTopComponent } from './components/english/page-header-top/page-header-top.component';
import { PageFooterComponent } from './components/english/page-footer/page-footer.component';
import { PageBodyComponent } from './components/english/page-body/page-body.component';
import { HomeBodyComponent } from './components/english/home-body/home-body.component';
import { PageHeaderComponentRTL } from './components/arabic/page-header-rtl/page-header.component.rtl';
import { PageHeaderTopComponentRTL } from './components/arabic/page-header-top-rtl/page-header-top.component.rtl';
import { PageFooterComponentRTL } from './components/arabic/page-footer-rtl/page-footer.component.rtl';
import { PageBodyComponentRTL } from './components/arabic/page-body-rtl/page-body.component.rtl';
import { HomeBodyComponentRTL } from './components/arabic/home-body-rtl/home-body.component.rtl';
import { BreadcrumbComponent } from './components/english/breadcrumb/breadcrumb.component';
import { SideNavComponent } from './components/english/side-nav/side-nav.component';
import { BreadcrumbComponentRTL } from './components/arabic/breadcrumb-rtl/breadcrumb.component.rtl';
import { SideNavComponentRTL } from './components/arabic/side-nav-rtl/side-nav.component.rtl';
import { CommonPageModule } from '../commonpage.module';
import { LoginComponent } from '../auth/login/login.component';
import { LoginNewComponent } from './components/login/login.component';
import { LayoutRoutingModule } from './layout-routing.module';
import { MediaModule } from '../media/media.module';
import { ContactsModule } from '../contacts/contacts.module';
import { SmartSearchModule } from '../smart-search/smart-search.module';
import { ErrorPageComponent } from './error-page/error-page.component';
import { AdminGuard } from './guard/admin.guard';
import { NgxGalleryModule } from 'ngx-gallery';
import { LightboxModule } from 'ngx-lightbox';
import { PhotogalleryComponent } from './photogallery/photogallery.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CarouselModule } from 'ngx-owl-carousel-o';

@NgModule({
  declarations: [
    LayoutComponent,
    PageHeaderComponent,
    PageHeaderTopComponent,
    PageFooterComponent,
    PageBodyComponent,
    HomeBodyComponent,
    PageHeaderComponentRTL,
    PageHeaderTopComponentRTL,
    PageFooterComponentRTL,
    PageBodyComponentRTL,
    HomeBodyComponentRTL,
    BreadcrumbComponentRTL,
    SideNavComponentRTL,
    BreadcrumbComponent,
    SideNavComponent,
    LoginComponent,
    ErrorPageComponent,
    PhotogalleryComponent,
    LoginNewComponent
  ],
  imports: [
    CommonPageModule,
    // AppRoutingModule,
    // CommonModule
    LayoutRoutingModule,
    MediaModule,
    ContactsModule,
    SmartSearchModule,
    NgxGalleryModule,
    LightboxModule,
    CarouselModule
  ],
  exports: [
    LayoutComponent,
    PageHeaderComponent,
    PageHeaderTopComponent,
    PageFooterComponent,
    PageBodyComponent,
    HomeBodyComponent,
    PageHeaderComponentRTL,
    PageHeaderTopComponentRTL,
    PageFooterComponentRTL,
    PageBodyComponentRTL,
    HomeBodyComponentRTL,
    BreadcrumbComponentRTL,
    SideNavComponentRTL,
    BreadcrumbComponent,
    SideNavComponent,
    LoginComponent,

  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
  providers: [
    AdminGuard
    // PageHeaderComponent,
    // PageHeaderTopComponent,
    // PageFooterComponent,
    // PageBodyComponent,
    // HomeBodyComponent,
    // BreadcrumbComponent,
    // SideNavComponent,
  ]
})
export class LayoutModule { }
