import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';
import { ShortUrl } from './features/urls/short-url/short-url';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
    
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: LoginComponent
  },

  {
    path: 'register',
    component: RegisterComponent
  },

  {
    path: 'dashboard',
    component: ShortUrl,
    canActivate: [authGuard]
  }
];
