import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { UserRole } from '../../../UserRole';
import { NgModule } from '@angular/core';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIcon } from '@angular/material/icon';
@Component({
  selector: 'app-user-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatInputModule,MatIcon, MatFormFieldModule, MatSelectModule, MatButtonModule, MatDatepickerModule, MatNativeDateModule],
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {
  userForm!: FormGroup;
  UserRole = UserRole;

  constructor(private fb: FormBuilder, private authService: AuthService,private router:Router) {}

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
      dateOfBirth: [''],
      gender: [''],
      address: [''],
      specialization: ['']
    });
  }

  private getRoleNumber(role: UserRole): number {
    switch (role) {
      case UserRole.Admin:
        return 3;
      case UserRole.Doctor:
        return 1;
      case UserRole.Patient:
        return 2;
      default:
        return 0;
    }
  }
  backToHomepage(): void {
    this.router.navigate(['/']);
  }
  onSubmit(): void {
    if (this.userForm.valid) {
      const roleNumber = this.getRoleNumber(this.userForm.value.role);
      const payload = {
        username: this.userForm.value.username,
        password: this.userForm.value.password,
        confirmPassword: this.userForm.value.confirmPassword,
        email: this.userForm.value.email,
        phoneNumber: this.userForm.value.phoneNumber,
        role: roleNumber,
        firstName: this.userForm.value.firstName,
        lastName: this.userForm.value.lastName,
        
        dateOfBirth: this.userForm.value.role == UserRole.Patient ? this.userForm.value.dateOfBirth : null,
        gender: this.userForm.value.role == UserRole.Patient ? this.userForm.value.gender : null,
        address: this.userForm.value.role == UserRole.Patient ? this.userForm.value.address : null,

        specialization: this.userForm.value.role == UserRole.Doctor ? this.userForm.value.specialization : null
      };
      
      console.log('Payload being sent:', payload);

      this.authService.register(payload).subscribe({
        
        next: (response) => {
          this.router.navigate(['/login']);
          console.log('Registration successful:', response);
          alert('Registration successful! You can now log in.');
        },
        error: (error) => {
          console.error('Registration failed:', error.error.errorMessage);
          alert('An error occurred. Please try again.');
        },
      });
    }
  }
}
