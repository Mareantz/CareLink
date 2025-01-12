import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIcon],
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {
  userForm!: FormGroup;
  errorMessage: string ='';
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }
  ngOnInit(): void {
    this.userForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }
  onSubmit(): void {
    if (this.userForm.valid) {
      const payload = this.userForm.value;

      console.log('Payload being sent:', payload);

      this.authService.login(payload).subscribe({
        next: (response) => {
          console.log('Login successful:', response);

          localStorage.setItem('token', response.data);

          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('Login failed:', error.error.errorMessage);
          this.errorMessage = 'Invalid username or password.';
        },
      });
    }
  }
  backToHomepage(): void {
    this.router.navigate(['/']);
  }

}
