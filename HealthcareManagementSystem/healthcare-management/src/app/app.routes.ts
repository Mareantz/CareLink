import { Routes } from '@angular/router';
import { PatientListComponent } from './components/patient-list/patient-list.component';
import { PatientCreateComponent } from './components/patient-create/patient-create.component';
import { PatientUpdateComponent } from './components/patient-update/patient-update.component';
import { PatientListIdComponent } from './components/patient-list-id/patient-list-id.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { UserLoginComponent } from './components/user-login/user-login.component';


export const appRoutes: Routes = [
  { path: '', redirectTo: '/patients', pathMatch: 'full' },
  { path: 'patients', component: PatientListComponent },
  { path: 'patients/create', component: PatientCreateComponent },
  { path: 'patients/update/:id', component: PatientUpdateComponent },
  { path: 'patients/find', component: PatientListIdComponent },
  { path: 'register', component: UserRegisterComponent },
  { path: 'login', component: UserLoginComponent }
];
  
