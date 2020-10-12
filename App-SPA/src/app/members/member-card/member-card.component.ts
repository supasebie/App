import { User } from './../../_models/user';
import { Component, Input, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;

  constructor(private alertify: AlertifyService, private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
  }

  sendLike(recipientId: number) {
    this.alertify.confirm('Are you sure you would like to friend this user?', () => {
      this.userService.sendLike(this.authService.decodedToken.nameid, this.user.id).subscribe(next => {
        this.alertify.success('Friend request sent');
      }, error => {
        this.alertify.error(error);
      });
    });
  }
}
