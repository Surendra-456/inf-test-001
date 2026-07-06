import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-register',
    standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html'
})
export class RegisterComponent {

  username = '';
  email = '';
  password = '';

  constructor(
    private authService: AuthService,
    private router: Router) {
  }

  register() {

    this.authService.register({
      username: this.username,
      email: this.email,
      password: this.password
    })
    .subscribe({
      next: () => {
        alert('Registration successful');
        this.router.navigate(['/login']);
      },
      error: error => {
        console.error(error);
        alert('Registration failed');
      }
    });
  }
}