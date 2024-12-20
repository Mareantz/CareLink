import { Routes } from '@angular/router';
import { HomeComponent } from './components/homepage/home/home.component';
import { PatientListComponent } from './components/patient/patient-list/patient-list.component';
import { PatientCreateComponent } from './components/patient/patient-create/patient-create.component';
import { PatientUpdateComponent } from './components/patient/patient-update/patient-update.component';
import { PatientListIdComponent } from './components/patient/patient-list-id/patient-list-id.component';
import { UserRegisterComponent } from './components/user/user-register/user-register.component';
import { UserLoginComponent } from './components/user/user-login/user-login.component';
import { PatientRiskPredictionComponent } from './components/patient/patient-risk-prediction/patient-risk-prediction.component';


export const appRoutes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'patients', component: PatientListComponent },
  { path: 'patients/create', component: PatientCreateComponent },
  { path: 'patients/update/:id', component: PatientUpdateComponent },
  { path: 'patients/find', component: PatientListIdComponent },
  { path: 'register', component: UserRegisterComponent },
  { path: 'login', component: UserLoginComponent },
  { path: 'patient-risk-prediction', component: PatientRiskPredictionComponent }
];
  
