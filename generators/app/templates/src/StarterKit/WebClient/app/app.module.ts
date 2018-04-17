import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AuiModules } from './shared/aui.modules';

import { AppComponent } from './app.component';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AuiModules
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
