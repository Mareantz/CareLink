import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PatientUpdateComponent } from './patient-update.component';
import { PatientService } from '../../services/patient.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { CommonModule } from '@angular/common';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('PatientUpdateComponent', () => {
  let component: PatientUpdateComponent;
  let fixture: ComponentFixture<PatientUpdateComponent>;
  let patientService: jasmine.SpyObj<PatientService>;
  let router: jasmine.SpyObj<Router>;
  let route: ActivatedRoute;

  beforeEach(async () => {
    const patientServiceSpy = jasmine.createSpyObj('PatientService', ['getPatientById', 'updatePatient']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const routeSpy = { snapshot: { paramMap: { get: () => '1' } } } as any;

    await TestBed.configureTestingModule({
      imports: [PatientUpdateComponent, ReactiveFormsModule, CommonModule, HttpClientTestingModule],
      providers: [
        { provide: PatientService, useValue: patientServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: routeSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PatientUpdateComponent);
    component = fixture.componentInstance;
    patientService = TestBed.inject(PatientService) as jasmine.SpyObj<PatientService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    route = TestBed.inject(ActivatedRoute);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });


  it('should handle error when fetching patient by ID', () => {
    patientService.getPatientById.and.returnValue(throwError('Error fetching patient'));

    component.ngOnInit();

    expect(patientService.getPatientById).toHaveBeenCalledWith('1');
  });

  it('should update patient', () => {
    patientService.updatePatient.and.returnValue(of({}));

    component.updateForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      dateOfBirth: '01-01-2000',
      gender: 'Male',
      address: '123 Street'
    });

    component.onSubmit();

    expect(patientService.updatePatient).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/patients']);
  });
});