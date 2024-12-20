import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { UserRole } from '../../../UserRole';

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
      confirmPassword: ['', [Validators.required]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      role: ['', [Validators.required]],

      // Patient-specific fields (will only be required if role = Patient)
      dateOfBirth: [''],
      gender: [''],
      address: [''],

      // Doctor-specific fields (will only be required if role = Doctor)
      specialization: ['']
    });
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      const payload = {
        username: this.userForm.value.username,
        password: this.userForm.value.password,
        confirmPassword: this.userForm.value.confirmPassword,
        email: this.userForm.value.email,
        phoneNumber: this.userForm.value.phoneNumber,
        role: Number(this.userForm.value.role),
        firstName: this.userForm.value.firstName,
        lastName: this.userForm.value.lastName,
        
        // Patient-specific
        dateOfBirth: this.userForm.value.role == UserRole.Patient ? this.userForm.value.dateOfBirth : null,
        gender: this.userForm.value.role == UserRole.Patient ? this.userForm.value.gender : null,
        address: this.userForm.value.role == UserRole.Patient ? this.userForm.value.address : null,

        // Doctor-specific
        specialization: this.userForm.value.role == UserRole.Doctor ? this.userForm.value.specialization : null
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
