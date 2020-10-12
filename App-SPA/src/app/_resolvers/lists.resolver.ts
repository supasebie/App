import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { User } from '../_models/user';
import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';

@Injectable()
export class ListsResolver implements Resolve<User[]> {
  pagenumber = 1;
  pageSize = 5;
  likesParam = 'Likees';

  constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) { }

  resolve(): Observable<User[]> {
    return this.userService.getUsers(this.pagenumber, this.pageSize, null, this.likesParam).pipe(
      catchError(error => {
        this.alertify.error('Problem retreving users');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
