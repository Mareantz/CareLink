// src/app/components/dashboard/dashboard.component.ts
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
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  userRole: UserRole = UserRole.None;
  navItems: NavItem[] = [];
  displayName: string = 'User';
  isHandset$ = of(false); // Will be updated via BreakpointObserver

  constructor(
    private authService: AuthService,
    private router: Router,
    private breakpointObserver: BreakpointObserver,
    private doctorService: DoctorService,
    private patientService: PatientService
  ) {}

  ngOnInit(): void {
    const token = this.authService.getToken();
    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    this.userRole = this.authService.getUserRole();

    // Responsive check using BreakpointObserver (CDK)
    this.isHandset$ = this.breakpointObserver.observe([Breakpoints.Handset]).pipe(
      map(result => result.matches)
    );

    // Fetch user first/last name
    this.fetchUserName();

    this.setNavItems();
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

  // Simple fetch for first/last name based on user role
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
    // Redirect to login page or handle accordingly
  }
}
