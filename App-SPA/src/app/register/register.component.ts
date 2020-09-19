import { AlertifyService } from './../_services/alertify.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
   @Output() cancelRegister: any = new EventEmitter();
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }
  register() {
    this.authService.register(this.model).subscribe(() => {
      this.alertify.success('You have succesfully registered an account');
    }, error => {
      this.alertify.error(error);
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
    this.alertify.message('Registration cancelled');
  }
}
