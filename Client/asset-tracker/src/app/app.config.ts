import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { UserOutline, InstagramOutline, GithubOutline, LinkedinOutline, DiscordOutline, PauseCircleOutline } from '@ant-design/icons-angular/icons';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { authInterceptor } from './interceptors/authInterceptor';
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
  withInterceptors([authInterceptor])
),
    provideAnimationsAsync(),
    importProvidersFrom(
      NzIconModule.forRoot([UserOutline, InstagramOutline, GithubOutline, LinkedinOutline, DiscordOutline, PauseCircleOutline]))
  ]
};