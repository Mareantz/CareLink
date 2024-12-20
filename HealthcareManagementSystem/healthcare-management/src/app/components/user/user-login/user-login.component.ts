import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
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

          // Save the JWT token to localStorage or sessionStorage
          localStorage.setItem('token', response.token);

          // Redirect the user after login
          this.router.navigate(['/patients']); // Adjust the route as needed
        },
        error: (error) => {
          console.error('Login failed:', error);
          this.errorMessage = 'Invalid username or password.';
        },
      });
    }
  }
}
