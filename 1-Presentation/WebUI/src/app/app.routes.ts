import { Routes } from '@angular/router';
import { AppLayout } from './layout/component/app.layout';
import { UserComponent } from './pages/user/user.component';
import { LoginComponent } from './pages/login/login.component';
import { UserRegisterComponent } from './pages/user-register/user-register.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: UserRegisterComponent },
  {
    path: 'app',
    component: AppLayout,
    children: [
      { path: 'user', component: UserComponent }
    ]
  },
  //{ path: '**', redirectTo: '/notfound' }
];
