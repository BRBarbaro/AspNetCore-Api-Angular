import { AuthGuard } from './auth/auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { SalaComponent } from './home/sala/sala.component';
import { ReservasComponent } from './home/sala/reservas/reservas.component';

const routes: Routes = [
  {path:'',redirectTo:'/user/login',pathMatch:'full'},
  {
    path: 'user', component: UserComponent,
    children: [
      { path: 'registration', component: RegistrationComponent },
      { path: 'login', component: LoginComponent }
    ]
  },
  {path:'home',component: HomeComponent,canActivate:[AuthGuard]},
  {path:'sala', children: [
    { path:'', component:SalaComponent},
    { path:':id', component:SalaComponent}, 
  ]},
  {path:'reservas',children: [
    { path:'', component:ReservasComponent},
    { path:':id', component:ReservasComponent}, 
  ]},
  {path:'**',redirectTo:'/user/login',pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
