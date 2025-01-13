import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DoctorModule } from '../doctor.module';
import { Doctor } from '../../../models/doctor.model';
import { DoctorService } from '../../../services/doctor/doctor.service';
import { DoctorCardComponent } from '../doctor-card/doctor-card.component';
import { CommonModule } from '@angular/common';
import { MatGridListModule, MatGridList } from '@angular/material/grid-list';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-doctor-list',
  standalone: true,
  imports:[DoctorCardComponent, CommonModule, MatGridListModule, MatButtonModule, MatIconModule],
  templateUrl: './doctor-list.component.html',
  styleUrls: ['./doctor-list.component.css']
})
export class DoctorListComponent implements OnInit {
  @ViewChild(MatGridList) grid!: MatGridList;
  doctors: Doctor[] = [];
  cols: number = 3;
  readonly stock_photo = 'assets/stock_doctor.jpg';

  constructor(private breakpointObserver: BreakpointObserver ,private doctorService: DoctorService, private router: Router) {}

  ngOnInit(): void {
    this.fetchDoctors();
    this.setupGridCols();
  }

  fetchDoctors(): void {
    this.doctorService.getDoctors().subscribe((data: Doctor[]) => {
      this.doctors = data.map(doctor => ({
        ...doctor,
        photoUrl: doctor.photoUrl || this.stock_photo
      }));
    });
  }

  setupGridCols(): void {
    this.breakpointObserver.observe([
      Breakpoints.HandsetPortrait,
      Breakpoints.HandsetLandscape,
      Breakpoints.TabletPortrait,
      Breakpoints.TabletLandscape,
      Breakpoints.WebPortrait,
      Breakpoints.WebLandscape
    ]).subscribe(result => {
      if (result.breakpoints[Breakpoints.HandsetPortrait] || result.breakpoints[Breakpoints.HandsetLandscape]) {
        this.cols = 1;
      } else if (result.breakpoints[Breakpoints.TabletPortrait] || result.breakpoints[Breakpoints.TabletLandscape]) {
        this.cols = 2;
      } else {
        this.cols = 3;
      }
    });
  }

  backToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

}
