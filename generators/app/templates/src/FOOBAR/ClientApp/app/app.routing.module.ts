import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {OverviewComponent} from './pages/overview/overview.component';
import {Page1Component} from './pages/page1/page1.component';
import {Page2Component} from './pages/page2/page2.component';
import {GateKeeperService} from './shared/services/gate-keeper/gate-keeper.service';
import {PermissionsEnum} from './shared/enums/permissions.enum';
import {NotAllowedComponent} from './pages/not-allowed/not-allowed.component';
import {NotFoundComponent} from './pages/not-found/not-found.component';

export const ROUTES: Routes = [
  {
    path: 'overview',
    component: OverviewComponent
  },
  {
    path: 'page1',
    component: Page1Component,
  },
  {
    path: 'page2',
    component: Page2Component,
    canActivate: [GateKeeperService],
    data: {
      roles: [PermissionsEnum.SAMPLE]
    }
  },
  {
    path: 'niet-gevonden',
    component: NotFoundComponent,
  },
  {
    path: 'verboden-toegang',
    component: NotAllowedComponent,
  },
  {
    path: '',
    redirectTo: '/overview',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'niet-gevonden'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(ROUTES)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
