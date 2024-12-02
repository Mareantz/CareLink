import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { CommonModule } from '@angular/common';
import { first } from 'rxjs';
import { format, parse} from 'date-fns';

@Component({
  selector: 'app-patient-create',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './patient-create.component.html',
  styleUrls: ['./patient-create.component.css']
})
export class PatientCreateComponent implements OnInit {
  patientForm: FormGroup;

  constructor(private fb: FormBuilder, private patientService: PatientService, private router: Router) {
    this.patientForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      dateOfBirth: ['', [Validators.required, this.dateValidator]],
      gender: ['', Validators.required],
      address: ['', Validators.required]
    });
  }

  ngOnInit(): void {}

  dateValidator(control: FormControl): { [key: string]: boolean } | null {
    const dateValue = control.value;
    if (!dateValue) {
      return null;
    }
    const date = new Date(dateValue);
    if (isNaN(date.getTime())) {
      return { 'invalidDate': true };
    }
    return null;
  }

  onSubmit() {
    if (this.patientForm.valid) {
      const formData = { ...this.patientForm.value };
      // Format the date to 'dd-MM-yyyy' before sending
      formData.dateOfBirth = format(new Date(formData.dateOfBirth), 'dd-MM-yyyy');
      this.patientService.createPatient(formData).subscribe(response => {
        console.log('Patient created successfully:', response);
      }, error => {
        console.error('Error creating patient:', error);
      });
    }
  }
  
}