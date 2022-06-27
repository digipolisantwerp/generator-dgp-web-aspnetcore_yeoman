import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomePageComponent } from './pages/home/home.page';
import { AboutPageComponent } from './pages/about/about.page';
import { PermissionGuard } from './shared/services/guards/permission.guard';
import { PermissionsEnum } from './shared/models/enums/permissions.enum';
import { ForbiddenPageComponent } from './pages/forbidden/forbidden.page';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  {
    path: 'home',
    component: HomePageComponent,
  },
  {
    path: 'protected',
    component: HomePageComponent,
    canActivate: [PermissionGuard],
    data: {
      roles: [PermissionsEnum.SAMPLE],
    },
  },
  {
    path: 'forbidden',
    component: ForbiddenPageComponent,
  },
  {
    path: 'about',
    component: AboutPageComponent,
  },
  {
    path: '**',
    redirectTo: 'niet-gevonden',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
