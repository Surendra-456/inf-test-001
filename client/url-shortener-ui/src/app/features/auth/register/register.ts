import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss' 
})
export class RegisterComponent {
  username = '';
  email = '';
  password = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  register(): void {
    if (!this.username || !this.email || !this.password) {
      alert('Please fill all fields');
      return;
    }

    this.authService.register({
      username: this.username,
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        alert('Registration successful');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error(error);
        alert('Registration failed');
      }
    });
  }
}