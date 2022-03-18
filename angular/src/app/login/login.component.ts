import { Component } from "@angular/core";
import { AuthService } from "src/services/auth.service";
import { Router } from "@angular/router";

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {
  userName:string="";
  password:string="";
  loginInvalid:boolean = false;

  constructor(private authService:AuthService, private router:Router) {

  }

  login(formValues:any) {
    this.authService.loginUser(formValues.userName,formValues.password).subscribe(response => {
      if(!response){
        this.loginInvalid=true;
      }
      else {
        this.router.navigate(['view']);
      }
    });
  }

  cancel() {
    this.router.navigate(['view']);
  }
}