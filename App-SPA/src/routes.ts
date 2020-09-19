import { AuthGuard } from './app/_guards/auth.guard';
import { MemberListComponent } from './app/member-list/member-list.component';
import { MessagesComponent } from './app/messages/messages.component';
import { ListsComponent } from './app/lists/lists.component';
import { HomeComponent } from './app/home/home.component';
import { Routes } from '@angular/router';

export const appRoutes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MemberListComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'lists', component: ListsComponent},
    ]
  },

  {path: '**', redirectTo: '', pathMatch: 'full'},
];