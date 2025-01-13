import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import { Appointment } from '../../../models/appointment.model';
import { AppointmentStatus } from '../../../AppointmentStatus';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatNativeDateModule } from '@angular/material/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable, Subject } from 'rxjs';
import { map, shareReplay, takeUntil } from 'rxjs/operators';
import { ModalComponent } from '../../shared/modal/modal.component';
import { AuthService } from '../../../services/auth.service';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-appointment-scheduler',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatButtonModule,
    MatGridListModule,
    MatNativeDateModule,
    ModalComponent,
    MatIconModule
  ],
  templateUrl: './appointment-scheduler.component.html',
  styleUrls: ['./appointment-scheduler.component.css']
})
export class AppointmentSchedulerComponent implements OnInit, OnDestroy {
  doctorId!: string;
  selectedDate: Date | null = null;
  appointmentsForDay: Appointment[] = [];

  timeSlots: string[] = [
    '08:00', '08:30', '09:00', '09:30',
    '10:00', '10:30', '11:00', '11:30',
    '12:00',
    '13:30', '14:00', '14:30', '15:00',
    '15:30', '16:00', '16:30', '17:00',
    '17:30'
  ];

  cols: number = 4; 
  rowHeight: string = '1:1'; 

  isHandset$!: Observable<boolean>;

  isModalVisible: boolean = false;
  selectedSlot: string = '';
  reason: string = '';
  

  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private appointmentService: AppointmentService,
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.doctorId = this.route.snapshot.paramMap.get('userId') as string;
    this.isHandset$ = this.breakpointObserver.observe(Breakpoints.Handset)
      .pipe(
        map(result => result.matches),
        shareReplay()
      );

    this.isHandset$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(isHandset => {
      if (isHandset) {
        this.cols = 2;
        this.rowHeight = '2:1';
      } else {
        this.cols = 4;
        this.rowHeight = '4:1';
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onDateChange(date: Date | null): void {
    this.selectedDate = date;
  
    if (!date) {
      this.appointmentsForDay = [];
      return;
    }
  
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    const formattedDate = `${year}-${month}-${day}`;
    
    this.appointmentService.getAppointmentsByDoctorAndDate(this.doctorId, formattedDate)
      .subscribe({
        next: (appointments) => {
          this.appointmentsForDay = appointments;
        },
        error: (err) => console.error('Error loading appointments', err)
      });
  }

  isSlotTaken(slot: string): boolean {
    if (!this.selectedDate) return false;

    const [hour, minute] = slot.split(':').map(Number);
    const slotDate = new Date(this.selectedDate);
    slotDate.setHours(hour, minute, 0, 0);

    return this.appointmentsForDay.some(appt => {
      const apptDate = new Date(appt.appointmentDate);
      return apptDate.getTime() === slotDate.getTime();
    });
  }

  bookSlot(slot: string): void {
    if (!this.selectedDate) return;

    this.selectedSlot = slot;
    this.isModalVisible = true;
  }

  closeModal(): void {
    this.isModalVisible = false;
    this.reason = '';
  }

  confirmBooking(): void {
    if (!this.reason.trim()) {
      alert('Please enter a reason for the appointment.');
      return;
    }

    const patientId = this.authService.getUserId();

    if (!patientId) {
      alert('Unable to retrieve patient information. Please log in again.');
      return;
    }

    const [hour, minute] = this.selectedSlot.split(':').map(Number);
    const appointmentDate = new Date(this.selectedDate as Date);
    appointmentDate.setHours(hour, minute, 0, 0);

    const newAppointment = {
      patientId: patientId,
      doctorId: this.doctorId,
      date: appointmentDate.toISOString(),
      reason: this.reason
    };

    this.appointmentService.createAppointment(newAppointment)
      .subscribe({
        next: (created) => {
          this.isModalVisible = false;
          this.reason = '';
          this.onDateChange(this.selectedDate);
        },
        error: (err) => {
          console.error('Error creating appointment', err);
          alert('Could not create appointment. Please try again.');
        }
      });
  }

  disableWeekends = (d: Date | null): boolean => {
    if (!d) return false;
  
    const day = d.getDay();
  
    const today = new Date();
    today.setHours(0, 0, 0, 0);
  
    const dateToCheck = new Date(d);
    dateToCheck.setHours(0, 0, 0, 0);
  
    return dateToCheck >= today && day !== 0 && day !== 6;
  }

  allSlotsTaken(): boolean {
    return this.appointmentsForDay.length >= this.timeSlots.length;
  }

  backToDoctorList(): void {
    this.router.navigate(['/doctors']);
  }
}