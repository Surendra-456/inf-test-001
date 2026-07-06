import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { Router } from '@angular/router';

export const authGuard:
CanActivateFn = () => {

  const router =
    inject(Router);

  const token =
    localStorage.getItem(
      'jwt-token'
    );

  if (!token) {

    router.navigateByUrl(
      '/login'
    );

    return false;
  }

  return true;
};