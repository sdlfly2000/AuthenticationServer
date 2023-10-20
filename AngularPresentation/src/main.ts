import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { LoginModule } from "./Login/login.module"


platformBrowserDynamic().bootstrapModule(LoginModule)
  .catch(err => console.error(err));
