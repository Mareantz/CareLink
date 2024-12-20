import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PatientListComponent } from './patient-list.component';
import { PatientService } from '../../../services/patient/patient.service';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';

describe('PatientListComponent', () => {
  let component: PatientListComponent;
  let fixture: ComponentFixture<PatientListComponent>;
  let patientService: jasmine.SpyObj<PatientService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const patientServiceSpy = jasmine.createSpyObj('PatientService', ['getPatients', 'deletePatientById']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [PatientListComponent, CommonModule],
      providers: [
        { provide: PatientService, useValue: patientServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PatientListComponent);
    component = fixture.componentInstance;
    patientService = TestBed.inject(PatientService) as jasmine.SpyObj<PatientService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load patients on init', () => {
    const mockPatients = [
      { id: '1', firstName: 'John', lastName: 'Doe', dateOfBirth: new Date(), gender: 'Male', address: '123 Street' }
    ];
    patientService.getPatients.and.returnValue(of(mockPatients));

    component.ngOnInit();

    expect(patientService.getPatients).toHaveBeenCalled();
    expect(component.patients).toEqual(mockPatients);
  });

  it('should navigate to create patient', () => {
    component.navigateToCreate();

    expect(router.navigate).toHaveBeenCalledWith(['/patients/create']);
  });

  it('should navigate to find patient by ID', () => {
    component.navigateToFind();

    expect(router.navigate).toHaveBeenCalledWith(['/patients/find']);
  });

});