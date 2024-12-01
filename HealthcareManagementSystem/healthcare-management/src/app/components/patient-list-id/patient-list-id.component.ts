import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/patient.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-patient-list-id',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './patient-list-id.component.html',
  styleUrls: ['./patient-list-id.component.css']
})
export class PatientListIdComponent implements OnInit {
  patient: Patient | null = null;
  patientId: string = '';

  constructor(private patientService: PatientService, private router: Router) {}

  ngOnInit(): void {}

  fetchPatientById(): void {
    if (!this.patientId) {
      return;
    }
    this.patientService.getPatientById(this.patientId).subscribe(patient => {
      this.patient = patient;
    }, error => {
      console.error('Error fetching patient:', error);
      this.patient = null;
    });
  }

  navigateToUpdateWithId(): void {
    if (this.patientId) {
      this.router.navigate(['/patients/update', this.patientId]);
    }
  }
  deletePatientById(): void {
    if (!this.patientId) {
      return;
    }
    this.patientService.deletePatientById(this.patientId).subscribe(() => {
      console.log('Patient deleted successfully');
      this.patient = null;
      this.patientId = '';
    }, error => {
      console.error('Error deleting patient:', error);
    });
  }
}