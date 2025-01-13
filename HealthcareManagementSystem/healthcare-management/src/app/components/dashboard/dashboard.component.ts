import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { UserRole } from '../../UserRole';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { DoctorService } from '../../services/doctor/doctor.service';
import { PatientService } from '../../services/patient/patient.service';
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { MedicalHistoryService } from '../../services/medical-history/medical-history.service';
import { MedicalHistory } from '../../models/medical-history.model';
import { MatAccordion } from '@angular/material/expansion';
import { MatExpansionPanel } from '@angular/material/expansion';
import { MatExpansionModule } from '@angular/material/expansion';
import { currentEnvironment } from '../../environment.prod';

interface NavItem {
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports:[
    MatToolbarModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatCardModule,
    RouterModule,
    CommonModule,
    MatAccordion,
    MatExpansionPanel,
    MatExpansionModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  userRole: UserRole = UserRole.None;
  UserRole = UserRole;
  navItems: NavItem[] = [];
  displayName: string = 'User';
  medicalHistoryList: any[] = [];
  isHandset$ = of(false);

  private baseAttachmentUrl: string = currentEnvironment.apiUrl;

  constructor(
    private authService: AuthService,
    private router: Router,
    private breakpointObserver: BreakpointObserver,
    private doctorService: DoctorService,
    private patientService: PatientService,
    private medicalHistoryService: MedicalHistoryService
  ) {}

  ngOnInit(): void {
    const token = this.authService.getToken();
    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    this.userRole = this.authService.getUserRole();

    this.isHandset$ = this.breakpointObserver.observe([Breakpoints.Handset]).pipe(
      map(result => result.matches)
    );

    this.fetchUserName();

    this.setNavItems();

    if (this.userRole === UserRole.Patient) {
      this.loadPatientHistory();
    }
  }

  get userRoleString(): string {
    switch (this.userRole) {
      case UserRole.Admin:
        return 'admin';
      case UserRole.Doctor:
        return 'doctor';
      case UserRole.Patient:
        return 'patient';
      default:
        return 'none';
    }
  }

  getFileName(path: string): string {
    if (!path) return 'Attachment';
    const segments = path.split('/');
    return segments[segments.length - 1];
  }

  getAttachmentUrl(path: string): string {
    if (!path) return '#';
    return `${this.baseAttachmentUrl}${path}`;
  }

  loadPatientHistory(): void {
    const userId = this.authService.getUserId();
    if (!userId) {
      console.error('User ID not found.');
      return;
    }

    this.medicalHistoryService.getMedicalHistoryByPatientId(userId).subscribe({
      next: (data: MedicalHistory[]) => {
        this.medicalHistoryList = data.sort((a, b) => 
          new Date(b.dateRecorded).getTime() - new Date(a.dateRecorded).getTime()
        );
      },
      error: (err) => console.error('Error loading medical history:', err)
    });
  }

  private fetchUserName(): void {
    const userId = this.authService.getUserId();
    if (!userId) return;

    if (this.userRole === UserRole.Patient) {
      this.patientService.getPatientById(userId).subscribe({
        next: (patient: any) => {
          const firstName = patient?.firstName || 'Unknown';
          const lastName = patient?.lastName || '';
          this.displayName = `${firstName} ${lastName}`.trim();
        },
        error: (error) => {
          console.error('Error fetching patient details:', error);
        }
      });
    } else if (this.userRole === UserRole.Doctor) {
      this.doctorService.getDoctorById(userId).subscribe({
        next: (doctor: any) => {
          const firstName = doctor?.firstName || 'Unknown';
          const lastName = doctor?.lastName || '';
          this.displayName = `${firstName} ${lastName}`.trim();
        },
        error: (error) => {
          console.error('Error fetching doctor details:', error);
        }
      });
    }
  }

  setNavItems(): void {
    if (this.userRole === UserRole.Patient) {
      this.navItems = [
        { label: 'Doctors', icon: 'local_hospital', route: '/doctors' },
        { label: 'Health Risk Calculator', icon: 'calculate', route: '/health-risk' },
        { label: 'My Appointments', icon: 'event', route: '/appointments' },
      ];
    } else if (this.userRole === UserRole.Doctor) {
      this.navItems = [
        { label: 'My Profile', icon: 'person', route: '/profile' },
        { label: 'My Patients', icon: 'group', route: '/patients' },
        { label: 'Appointments', icon: 'event', route: '/appointments' },
      ];
    }
  }

  logout(): void {
    this.authService.logout();
  }
}
