import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PatientListIdComponent } from './patient-list-id.component';
import { PatientService } from '../../services/patient.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('PatientListIdComponent', () => {
  let component: PatientListIdComponent;
  let fixture: ComponentFixture<PatientListIdComponent>;
  let patientService: jasmine.SpyObj<PatientService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const patientServiceSpy = jasmine.createSpyObj('PatientService', ['getPatientById', 'deletePatientById']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [PatientListIdComponent, FormsModule, CommonModule, HttpClientTestingModule],
      providers: [
        { provide: PatientService, useValue: patientServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PatientListIdComponent);
    component = fixture.componentInstance;
    patientService = TestBed.inject(PatientService) as jasmine.SpyObj<PatientService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });



  it('should handle error when fetching patient by ID', () => {
    patientService.getPatientById.and.returnValue(throwError('Error fetching patient'));

    component.patientId = '1';
    component.fetchPatientById();

    expect(patientService.getPatientById).toHaveBeenCalledWith('1');
    expect(component.patient).toBeNull();
  });

  it('should navigate to update with ID', () => {
    component.patientId = '1';
    component.navigateToUpdateWithId();

    expect(router.navigate).toHaveBeenCalledWith(['/patients/update', '1']);
  });

});