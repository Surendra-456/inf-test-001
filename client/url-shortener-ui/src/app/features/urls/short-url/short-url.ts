import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs/operators';

import { UrlService } from '../../../core/services/url-service';

@Component({
  selector: 'app-short-url',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './short-url.html',
  styleUrl: './short-url.scss'
})
export class ShortUrl {
  form: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private urlService: UrlService,
    private router: Router
  ) {
    this.form = this.fb.group({
      originalUrl: [
        '',
        [
          Validators.required,
          Validators.pattern(/^https?:\/\/.+/)
        ]
      ]
    });
  }

  create(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    this.urlService
      .createUrl(this.form.value)
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: (response) => {
          console.log(response);
          alert('Short URL created successfully.');
          this.form.reset();
        },
        error: (err) => {
          console.error(err);
          alert('Failed to create URL.');
        }
      });
  }

  logout(): void {
    localStorage.removeItem('jwt-token');
    this.router.navigate(['/login']);
  }
}