import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DoctorListComponent } from './doctor-list/doctor-list.component';
import { DoctorCardComponent } from './doctor-card/doctor-card.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    DoctorListComponent,
    DoctorCardComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
  ],
  exports: [
    DoctorListComponent,
    DoctorCardComponent
  ]
})
export class DoctorModule { }
