import {NgModule, CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

// APP
import {AppComponent} from './components/app.component';
import {AppRoutingModule} from './app.routing.module';
import {OverviewComponent} from './components/overview/overview.component';
import {Page1Component} from './components/page1/page1.component';
import {Page2Component} from './components/page2/page2.component';

// COLLECTIONS
import {AuiModules} from './modules/aui.modules';
import {ThirdPartyModules} from './modules/third-party.modules';
import {AuthenticationService} from './shared/services/authentication/authentication.service';

// Fix for error TS2304: Cannot find name 'process'.
// import { } from 'node';

// const DEVMODE = process.env.NODE_ENV === "DEV";

@NgModule({
    declarations: [
        AppComponent,
        OverviewComponent,
        Page1Component,
        Page2Component
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
