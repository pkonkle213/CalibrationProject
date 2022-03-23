import { Component } from '@angular/core';
import { AuthService } from 'src/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(public authService:AuthService,  private router:Router) {

  }
  
  logout() {
    this.authService.logOutUser();
    this.router.navigate(['']);
  }

  leaderCheck() {
    return (this.authService.currentUser.user.role==="Leader" || this.authService.currentUser.user.role==="Admin")
  }

  adminCheck() {
    return (this.authService.currentUser.user.role==="Admin")
  }
}
