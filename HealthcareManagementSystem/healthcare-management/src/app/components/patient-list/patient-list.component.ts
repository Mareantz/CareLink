import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/patient.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patient-list.component.html',
  styleUrls: ['./patient-list.component.css']
})
export class PatientListComponent implements OnInit {
  patients: Patient[] = [];

  constructor(private patientService: PatientService, private router: Router) {}

  ngOnInit(): void {
    this.patientService.getPatients().subscribe((data: Patient[]) => {
      this.patients = data;
    });
  }

  public navigateToCreate(): void {
    this.router.navigate(['/patients/create']);
  }

  public navigateToFind(): void {
    this.router.navigate(['/patients/find']);
  }

  public navigateToLogin(): void {
    this.router.navigate(['/user-login']);
  }

  public fetchPatientAndNavigate(firstName: string, lastName: string, dateOfBirth: Date, gender: string, address: string): void {
    this.patientService.getPatients().subscribe((patients: Patient[]) => {
      const patient = patients.find(p => 
        p.firstName === firstName && 
        p.lastName === lastName && 
        p.dateOfBirth === dateOfBirth && 
        p.gender === gender && 
        p.address === address
      );
      if (patient) {
        this.router.navigate(['/patients/update', patient.id]);
      } else {
        console.error('Patient not found');
      }
    });
  }
}
