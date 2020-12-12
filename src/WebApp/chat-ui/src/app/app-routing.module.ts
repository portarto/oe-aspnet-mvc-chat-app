import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './auth-guard';
import { ChatRoomModule } from './modules/chat-room/chat-room.module';
import { ChatRoomsComponent } from './modules/chat-room/components/chat-rooms/chat-rooms.component';
import { LoginComponent } from './modules/identity/components/login/login.component';
import { RegisterComponent } from './modules/identity/components/register/register.component';
import { IdentityModule } from './modules/identity/identity.module';
import { AuthInterceptor } from './shared/interceptors/auth-interceptor';

const routes: Routes = [
  { path: '', redirectTo: '/identity/login', pathMatch: 'full' },
  {
    path: 'identity',
    data: { reqAuth: false },
    canActivate: [ AuthGuard ],
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
    ]
  },
  {
    path: 'chat-rooms',
    data: { reqAuth: true },
    canActivate: [ AuthGuard ],
    children: [
      { path: '', component: ChatRoomsComponent },
      { path: ':id', component: ChatRoomsComponent }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    ChatRoomModule,
    IdentityModule
  ],
  exports: [RouterModule],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
})
export class AppRoutingModule { }
