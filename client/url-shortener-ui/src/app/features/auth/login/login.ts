import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html'
})
export class LoginComponent {

  email = '';
  password = '';

  constructor(
    private authService: AuthService,
    private router: Router) {
  }

  login() {

    this.authService.login({
      email: this.email,
      password: this.password
    })
    .subscribe({
      next: (response) => {

        this.authService.saveToken(
          response.token
        );

        this.router.navigateByUrl(
          '/dashboard'
        );
      }
    });
  }
}