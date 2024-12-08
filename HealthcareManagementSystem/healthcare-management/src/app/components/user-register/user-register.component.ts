import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { UserRole } from '../../UserRole';

@Component({
  selector: 'app-user-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {
  userForm!: FormGroup;
  UserRole = UserRole;

  constructor(private fb: FormBuilder, private authService: AuthService) {}

  ngOnInit(): void {
    this.userForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      role: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      const payload = {
        ...this.userForm.value,
        role: Number(this.userForm.value.role),
      };
      console.log('Payload being sent:', payload);

      this.authService.register(payload).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          alert('Registration successful! You can now log in.');
        },
        error: (error) => {
          console.error('Registration failed:', error);
          alert('An error occurred. Please try again.');
        },
      });
    }
  }
}
