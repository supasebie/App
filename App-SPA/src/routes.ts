import { MemberListResolver } from './app/_resolvers/member-list.resolver';
import { MemberDetailResolver } from './app/_resolvers/member-detail.resolver';

import { MemberDetailComponent } from './app/members/member-detail/member-detail.component';
import { MemberListComponent } from './app/members/member-list/member-list.component';
import { MessagesComponent } from './app/messages/messages.component';
import { ListsComponent } from './app/lists/lists.component';
import { HomeComponent } from './app/home/home.component';

import { AuthGuard } from './app/_guards/auth.guard';
import { Routes } from '@angular/router';

export const appRoutes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MemberListComponent, 
        resolve: {users: MemberListResolver}},
      {path: 'members/:id', component: MemberDetailComponent,
        resolve: {user: MemberDetailResolver}},
      {path: 'messages', component: MessagesComponent},
      {path: 'lists', component: ListsComponent},
    ]
  },

  {path: '**', redirectTo: '', pathMatch: 'full'},
];
