import { Component } from '@angular/core';
import { ILoginUser } from 'src/interfaces/loginUser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})

export class LoginComponent {
  user: ILoginUser[] = [];

  Login() {

  }
}
