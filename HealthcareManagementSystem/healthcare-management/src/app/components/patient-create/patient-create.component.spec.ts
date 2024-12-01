import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PatientCreateComponent } from './patient-create.component';
import { PatientService } from '../../services/patient.service';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';

describe('PatientCreateComponent', () => {
  let component: PatientCreateComponent;
  let fixture: ComponentFixture<PatientCreateComponent>;
  let patientService: jasmine.SpyObj<PatientService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const patientServiceSpy = jasmine.createSpyObj('PatientService', ['createPatient']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [PatientCreateComponent, ReactiveFormsModule, CommonModule],
      providers: [
        { provide: PatientService, useValue: patientServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PatientCreateComponent);
    component = fixture.componentInstance;
    patientService = TestBed.inject(PatientService) as jasmine.SpyObj<PatientService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create patient', () => {
    patientService.createPatient.and.returnValue(of({}));

    component.patientForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      dateOfBirth: '01-01-2000',
      gender: 'Male',
      address: '123 Street'
    });

    component.onSubmit();

    expect(patientService.createPatient).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/patients']);
  });
});