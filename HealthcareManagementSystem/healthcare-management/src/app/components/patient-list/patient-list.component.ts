import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/patient.model';
import {CommonModule} from '@angular/common';
@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.css'
})
export class PatientListComponent implements OnInit {

  patients: Patient[] = [];
  constructor(private patientService: PatientService,private router: Router) { }
  
  ngOnInit(): void {
    this.patientService.getPatients().subscribe((data : Patient[])=>{
      this.patients = data;
    });
  }
  public navigateToCreate(){
    this.router.navigate(['/patients/create']);
  }
  public navigateToUpdate(id?:string){
    this.router.navigate(['/patients/update/',id]);
  }
}
