import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DoctorService } from '../../services/doctor/doctor.service';
import { PatientService } from '../../services/patient/patient.service';
import { AuthService } from '../../services/auth.service';
import { UserRole } from '../../UserRole';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  userId: string | null = null;
  userRole: UserRole = UserRole.None;
  userDetails: any = null;
  profileForm: FormGroup;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private doctorService: DoctorService,
    private patientService: PatientService,
    private authService: AuthService,
    private router: Router
  ) {
    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      specialization: ['', [Validators.maxLength(100)]],
      bio: ['', [Validators.maxLength(1000)]]
    });
  }

  ngOnInit(): void {
    const userDetails = this.authService.getUserDetails();
    if (userDetails) {
      this.userId = userDetails.id;
      this.userRole = userDetails.role;

      if (this.userRole === UserRole.Doctor) {
        this.getDoctorDetails();
      } else if (this.userRole === UserRole.Patient) {
        this.getPatientDetails();
      }
    }
  }

  private getDoctorDetails(): void {
    if (!this.userId) return;

    this.doctorService.getDoctorById(this.userId).subscribe({
      next: (doctor) => {
        this.populateForm(doctor);
      },
      error: (err) => console.error('Error fetching doctor details:', err)
    });
  }

  private getPatientDetails(): void {
    if (!this.userId) return;

    this.patientService.getPatientById(this.userId).subscribe({
      next: (patient) => {
        this.populateForm(patient);
      },
      error: (err) => console.error('Error fetching patient details:', err)
    });
  }

  private populateForm(data: any): void {
    const formValue = {
      firstName: data.firstName,
      lastName: data.lastName,
      specialization: data.specialization || '',
      bio: data.bio || ''
    };

    this.profileForm.patchValue(formValue);
  }

  getFormFields(): string[] {
    return Object.keys(this.profileForm.controls);
  }

  onSubmit(): void {
    if (!this.userId) {
      console.error('User ID is missing. Cannot update.');
      return;
    }

    if (this.profileForm.valid) {
      const formData = {
        id: this.userId,
        ...this.profileForm.value
      };

      if (this.userRole === UserRole.Doctor) {
        this.updateDoctor(formData.id, formData);
      } else if (this.userRole === UserRole.Patient) {
        this.updatePatient(formData);
      }
    } else {
      console.log('Form is invalid');
    }
  }

  private updateDoctor(id: string, doctorData: any): void {
    this.doctorService.updateDoctor(id, doctorData).subscribe({
      next: () => {
        console.log('Doctor updated successfully');
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Error updating doctor:', err);
      }
    });
  }

  private updatePatient(patientData: any): void {
    this.patientService.updatePatient(this.userId!, patientData).subscribe({
      next: () => {
        console.log('Patient updated successfully');
        this.router.navigate(['/patients']);
      },
      error: (err) => {
        console.error('Error updating patient:', err);
      }
    });
  }

  toggleEditMode(): void {
    this.isEditMode = !this.isEditMode;
  }
}