import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-doctor-card',
  standalone: false,
  templateUrl: './doctor-card.component.html',
  styleUrls: ['./doctor-card.component.css']
})
export class DoctorCardComponent {
  @Input() firstName!: string;
  @Input() lastName!: string;
  @Input() specialization!: string;
  @Input() bio!: string;
  @Input() photoUrl: string = 'assets/stock_doctor.jpg'; // Default photo
}
