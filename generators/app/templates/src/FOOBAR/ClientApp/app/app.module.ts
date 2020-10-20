import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

// APP
import {AppComponent} from './app.component';
import {AppRoutingModule} from './app.routing.module';
import {OverviewComponent} from './pages/overview/overview.component';
import {Page1Component} from './pages/page1/page1.component';
import {Page2Component} from './pages/page2/page2.component';

// COLLECTIONS
import {AuiModules} from './modules/aui.modules';
import {ThirdPartyModules} from './modules/third-party.modules';
import {AuthenticationService} from './shared/services/authentication/authentication.service';
import {NotAllowedComponent} from './pages/not-allowed/not-allowed.component';
import {NotFoundComponent} from './pages/not-found/not-found.component';
import {HasRoleDirective} from './shared/directives/has-role/has-role.directive';

@NgModule({
  declarations: [
    AppComponent,
    OverviewComponent,
    Page1Component,
    Page2Component,
    NotAllowedComponent,
    NotFoundComponent,
    HasRoleDirective
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule,
    AppRoutingModule,
    ...AuiModules,
    ...ThirdPartyModules
  ],
  providers: [
    AuthenticationService
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent]
})
export class AppModule {
}
