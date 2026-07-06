import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UrlService } from '../../../core/services/url-service';

@Component({
  selector: 'app-short-url',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './short-url.html',
  styleUrl: './short-url.scss',
})
export class ShortUrl {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private urlService: UrlService
  ) {
    this.form = this.fb.group({
      originalUrl: ['', Validators.required]
    });
  }

  create(): void {
    this.urlService.createUrl(this.form.value).subscribe((response: any) => {
      console.log(response);
    });
  }
}