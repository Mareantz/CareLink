import { RouterModule} from "@angular/router";
import { CommonModule } from "@angular/common";
import { provideHttpClient } from "@angular/common/http";
import { PatientService } from "./services/patient/patient.service"
import {appRoutes} from "./app.routes";
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { MatToolbarModule } from "@angular/material/toolbar";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';

import { DoctorService } from "./services/doctor/doctor.service";
import { DoctorModule } from "./components/doctor/doctor.module";
import { DashboardComponent } from "./components/dashboard/dashboard.component";

@NgModule({
    declarations:[
    ],
    imports:[
        CommonModule,
        RouterModule.forRoot(appRoutes),
        BrowserModule,
        DoctorModule,
        
    ],
    providers:[PatientService,provideHttpClient(),DoctorService],
    bootstrap:[]
})
export class AppModule{}