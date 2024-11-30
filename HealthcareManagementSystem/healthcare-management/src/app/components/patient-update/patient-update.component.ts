import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { CommonModule } from '@angular/common';
import { first } from 'rxjs';
import { BrowserModule } from '@angular/platform-browser';

@Component({
  selector: 'app-patient-update',
  imports: [ReactiveFormsModule, CommonModule, FormsModule, BrowserModule],
  templateUrl: './patient-update.component.html',
  styleUrls: ['./patient-update.component.css']
})
export class PatientUpdateComponent implements OnInit {
  patientForm: FormGroup;
  patientId: string = '';
  patientIdError: boolean = false;

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.patientForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      dateOfBirth: ['', [Validators.required, this.dateValidator]],
      gender: ['', Validators.required],
      address: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.patientId = this.route.snapshot.paramMap.get('id') || '';
    if (this.patientId) {
      this.fetchPatient();
    }
  }

  fetchPatient(): void {
    if (!this.patientId) {
      this.patientIdError = true;
      return;
    }
    this.patientIdError = false;
    this.patientService.getPatientById(this.patientId).subscribe(patient => {
      this.patientForm.patchValue({
        firstName: patient.firstName,
        lastName: patient.lastName,
        dateOfBirth: patient.dateOfBirth,
        gender: patient.gender,
        address: patient.address
      });
    }, error => {
      console.error('Error fetching patient:', error);
    });
  }

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

  onSubmit(): void {
    if (this.patientForm.valid) {
      this.patientService.updatePatient(this.patientId, this.patientForm.value).pipe(first()).subscribe(() => {
        this.router.navigate(['/patients']);
      }, error => {
        console.error('Error updating patient:', error);
      });
    }
  }
}