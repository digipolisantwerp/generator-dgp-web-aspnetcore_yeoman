import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {OverviewComponent} from './components/overview/overview.component';
import {Page1Component} from './components/page1/page1.component';
import {Page2Component} from './components/page2/page2.component';

export const ROUTES: Routes = [
    {
        path: 'overview',
        component: OverviewComponent,
    },
    {
        path: 'page1',
        component: Page1Component,
    },
    {
        path: 'page2',
        component: Page2Component,
    },
    {
        path: '',
        redirectTo: '/overview',
        pathMatch: 'full',
    }
];

@NgModule({
    imports: [RouterModule.forRoot(ROUTES)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
