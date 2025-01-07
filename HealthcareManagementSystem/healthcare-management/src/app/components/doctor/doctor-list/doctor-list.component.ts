import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DoctorModule } from '../doctor.module';
import { Doctor } from '../../../models/doctor.model';
import { DoctorService } from '../../../services/doctor/doctor.service';

@Component({
  selector: 'app-doctor-list',
  standalone: false,
  templateUrl: './doctor-list.component.html',
  styleUrls: ['./doctor-list.component.css']
})
export class DoctorListComponent implements OnInit {
  doctors: Doctor[] = []; // Array to hold doctor data
  readonly stock_photo = 'assets/stock_doctor.jpg'; // Default photo

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.doctorService.getDoctors().subscribe((data: Doctor[]) => {
      this.doctors = data.map(doctor => ({
        ...doctor,
        photoUrl: this.stock_photo
      }))
    });
  }


  onPhotoUpload(event: any, doctorId: number): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        const base64Photo = reader.result as string;
        // Find the doctor by ID and update the photoUrl
        const doctor = this.doctors.find(doc => doc.id === doctorId);
        if (doctor) {
          doctor.photoUrl = base64Photo;
        }
      };
      reader.readAsDataURL(file); // Convert image to base64 string
    }
  }
}
