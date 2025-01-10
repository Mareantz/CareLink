import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';

@Component({
  selector: 'app-doctor-card',
  standalone: true,
  imports:[CommonModule, MatCardModule, MatButtonModule],
  templateUrl: './doctor-card.component.html',
  styleUrls: ['./doctor-card.component.css']
})
export class DoctorCardComponent {
  @Input() firstName!: string;
  @Input() lastName!: string;
  @Input() specialization!: string;
  @Input() bio!: string;
  @Input() photoUrl: string = 'assets/stock_doctor.jpg';
  @Input() doctorId!: string;

  constructor(private router: Router) {}

  makeAppointment() : void {
    this.router.navigate(['/appointments/scheduler', this.doctorId]);
  }
}
