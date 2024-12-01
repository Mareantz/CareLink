import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { CommonModule } from '@angular/common';
import { first } from 'rxjs';

@Component({
    selector: 'app-patient-update',
    imports: [ReactiveFormsModule, CommonModule],
    templateUrl: './patient-update.component.html',
    styleUrls: ['./patient-update.component.css']
})
export class PatientUpdateComponent implements OnInit {
    updateForm: FormGroup;
    patientId: string = '';

    constructor(
        private fb: FormBuilder,
        private patientService: PatientService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.updateForm = this.fb.group({
            firstName: ['', [Validators.required, Validators.maxLength(50)]],
            lastName: ['', [Validators.required, Validators.maxLength(50)]],
            dateOfBirth: ['', [Validators.required, this.dateValidator]],
            gender: ['', Validators.required],
            address: ['', Validators.required]
        });
    }

    ngOnInit(): void {
        console.log('Component initialized');
        this.patientId = this.route.snapshot.paramMap.get('id') || '';
        console.log('Patient ID:', this.patientId);
    
        if (this.patientId) {
            this.patientService.getPatientById(this.patientId).subscribe(
                patient => {
                    console.log('Fetched patient data:', patient);
                    this.updateForm.patchValue(patient);
                },
                error => {
                    console.error('Error fetching patient:', error);
                }
            );
        }
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
        if (this.updateForm.valid) {
            const patientData = {
                id: this.patientId,
                ...this.updateForm.value
            };
            this.patientService.updatePatient(this.patientId, patientData).subscribe({
                next: () => {
                    console.log('Patient updated successfully');
                    this.router.navigate(['/patients']);
                },
                error: (err) => {
                    console.error('Error updating patient:', err);
                }
            });
        } else {
            console.log('Form is invalid');
        }
    }
    
    
    
  
  
}
