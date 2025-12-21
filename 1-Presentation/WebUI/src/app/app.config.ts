import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/aura';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ConfirmationService } from 'primeng/api';
import { AuthService } from '../services/auth.service';
import { QueryStringService } from '../services/shared.QueryString.service';
import { StatusMessageService } from '../services/statusmessage.service';
import { AuthFailureInterceptor } from './auth-failure.interceptor';
import { AuthInterceptor } from './auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    providePrimeNG({
      theme: {
        preset: Aura
      },
    }),
    { provide: "BASE_URL", useValue: document.getElementsByTagName('base')[0].href },
    { provide: AuthService },
    { provide: StatusMessageService },
    { provide: QueryStringService },
    { provide: ConfirmationService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthFailureInterceptor, multi: true },
    provideHttpClient(withInterceptorsFromDi())
  ]
};
