import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { PatientService } from '../../../services/patient/patient.service';
import { Patient } from '../../../models/patient.model';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { PageEvent } from '@angular/material/paginator';
import { AfterViewInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule
  ],
  templateUrl: './patient-list.component.html',
  styleUrls: ['./patient-list.component.css']
})

export class PatientListComponent implements OnInit, AfterViewInit, OnDestroy {
  displayedColumns: string[] = ['firstName', 'lastName', 'dateOfBirth', 'gender', 'address'];
  patients: Patient[] = [];
  pageSize: number = 10;
  totalPatients: number = 0;

  searchFirstName: string = '';
  searchLastName: string = '';
  searchGender: string = '';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // RxJS Subject for handling search input with debouncing
  private searchSubject: Subject<void> = new Subject<void>();
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(private patientService: PatientService, private router: Router) {}

  ngOnInit(): void {
    // Subscribe to searchSubject with debouncing
    this.searchSubject.pipe(
      debounceTime(300),
      takeUntil(this.unsubscribe$)
    ).subscribe(() => {
      this.applyFilter();
    });

    this.fetchPatients();
  }

  ngAfterViewInit(): void {
    // If using MatSort or MatPaginator with MatTableDataSource, assign them here
  }

  fetchPatients(page: number = 1): void {
    this.patientService
      .getFilteredPatients(page, this.pageSize, this.searchFirstName, this.searchLastName, this.searchGender)
      .subscribe(
        (res: any) => {
          this.patients = res.data.data; // Adjust according to your API response
          this.totalPatients = res.data.totalCount;
        },
        error => {
          console.error('Error fetching patients:', error);
          this.patients = [];
          this.totalPatients = 0;
        }
      );
  }

  /**
   * Emit to searchSubject whenever search inputs change
   */
  onSearchChange(): void {
    this.searchSubject.next();
  }

  applyFilter(): void {
    if (this.paginator) {
      this.paginator.firstPage(); // Reset to first page when applying a new filter
    }
    this.fetchPatients();
  }

  onPageChange(event: PageEvent): void {
    const page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.fetchPatients(page);
  }

  backToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  navigateToCreate(): void {
    this.router.navigate(['/patients/create']);
  }

  navigateToFind(): void {
    this.router.navigate(['/patients/find']);
  }

  navigateToRiskPrediction(): void {
    this.router.navigate(['/patients/risk-prediction']);
  }

  ngOnDestroy(): void {
    // Complete the subscription to prevent memory leaks
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}