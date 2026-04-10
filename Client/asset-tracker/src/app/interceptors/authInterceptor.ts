import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { authService } from '../services/authService';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authServiceInstance = inject(authService);
  const token = authServiceInstance.getToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req);
};