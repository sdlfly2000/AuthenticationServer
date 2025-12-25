import { ApplicationConfig, provideBrowserGlobalErrorListeners, InjectionToken } from '@angular/core';
import { provideRouter, withInMemoryScrolling, withEnabledBlockingInitialNavigation } from '@angular/router';
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
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { LoginService } from './pages/login/login.service';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withInMemoryScrolling({ anchorScrolling: 'enabled', scrollPositionRestoration: 'enabled' }), withEnabledBlockingInitialNavigation()),
    provideAnimationsAsync(),
    providePrimeNG({ theme: { preset: Aura, options: { darkModeSelector: '.app-dark' } } }),
    { provide: BASE_URL, useValue: document.getElementsByTagName('base')[0].href },
    { provide: LoginService },
    { provide: AuthService },
    { provide: StatusMessageService },
    { provide: QueryStringService },
    { provide: ConfirmationService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthFailureInterceptor, multi: true },
    provideHttpClient(withInterceptorsFromDi())
  ]
};
