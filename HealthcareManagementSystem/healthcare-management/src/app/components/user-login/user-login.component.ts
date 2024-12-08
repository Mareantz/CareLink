import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {
  userForm!: FormGroup;
  constructor(private fb: FormBuilder) { }
  ngOnInit(): void {
    this.userForm = this.fb.group({
      password: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      role: ['', [Validators.required]]
    });
  }
  onSubmit(): void {
    if (this.userForm.valid) {
      console.log('Logged:', this.userForm.value);
    }
  }
}
