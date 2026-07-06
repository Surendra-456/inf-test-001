import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  email = '';
  password = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {
    if (!this.email || !this.password) {
      alert('Please enter email and password');
      return;
    }

    this.authService.login({
      email: this.email,
      password: this.password
    }).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        this.router.navigateByUrl('/dashboard');
      },
      error: (error) => {
        console.error(error);
        alert('Invalid email or password');
      }
    });
  }
}