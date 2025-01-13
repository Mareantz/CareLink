import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { PatientCreateComponent } from './patient-create.component';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PatientService } from '../../../services/patient/patient.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { format } from 'date-fns';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

// Cream un mock pentru PatientService
class MockPatientService {
  createPatient(patientData: any) {
    // Pentru testul de succes
    return of({ success: true, data: patientData });
  }
}

class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('PatientCreateComponent', () => {
  let component: PatientCreateComponent;
  let fixture: ComponentFixture<PatientCreateComponent>;
  let patientService: PatientService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientCreateComponent, ReactiveFormsModule, CommonModule, NoopAnimationsModule],
      providers: [
        { provide: PatientService, useClass: MockPatientService },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientCreateComponent);
    component = fixture.componentInstance;
    patientService = TestBed.inject(PatientService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('ar trebui să creeze componenta', () => {
    expect(component).toBeTruthy();
  });

  it('ar trebui să initializeze formularul cu controalele așteptate în constructor', () => {
    expect(component.patientForm).toBeTruthy();
    const controls = component.patientForm.controls;
    expect(controls['firstName']).toBeTruthy();
    expect(controls['lastName']).toBeTruthy();
    expect(controls['dateOfBirth']).toBeTruthy();
    expect(controls['gender']).toBeTruthy();
    expect(controls['address']).toBeTruthy();
  });

  describe('dateValidator', () => {
    it('ar trebui să returneze null pentru o valoare validă', () => {
      const control = new FormControl('2020-12-31');
      const result = component.dateValidator(control);
      expect(result).toBeNull();
    });

    it('ar trebui să returneze { invalidDate: true } pentru o valoare invalidă', () => {
      const control = new FormControl('invalid-date');
      const result = component.dateValidator(control);
      expect(result).toEqual({ invalidDate: true });
    });

    it('ar trebui să returneze null dacă valoarea este nulă', () => {
      const control = new FormControl('');
      const result = component.dateValidator(control);
      expect(result).toBeNull();
    });
  });

  describe('onSubmit', () => {
    it('ar trebui să apeleze createPatient și să formateze data de naștere atunci când formularul este valid', fakeAsync(() => {
      const createPatientSpy = spyOn(patientService, 'createPatient').and.callThrough();
      // Setăm valorile formularului
      const sampleDate = '1990-01-15';
      component.patientForm.setValue({
        firstName: 'Alice',
        lastName: 'Wonderland',
        dateOfBirth: sampleDate,
        gender: 'female',
        address: '123 Main St'
      });
      // Calculăm așteptarea pentru data formatată
      const formattedDate = format(new Date(sampleDate), 'dd-MM-yyyy');

      component.onSubmit();
      tick();

      expect(createPatientSpy).toHaveBeenCalledWith(jasmine.objectContaining({
        firstName: 'Alice',
        lastName: 'Wonderland',
        dateOfBirth: formattedDate,
        gender: 'female',
        address: '123 Main St'
      }));
    }));

  

    it('ar trebui să trateze eroarea dacă createPatient eșuează', fakeAsync(() => {
      spyOn(patientService, 'createPatient').and.returnValue(throwError(() => new Error('Creation error')));
      const consoleErrorSpy = spyOn(console, 'error');
      // Setăm formularul valid
      component.patientForm.setValue({
        firstName: 'Alice',
        lastName: 'Wonderland',
        dateOfBirth: '1990-01-15',
        gender: 'female',
        address: '123 Main St'
      });
      component.onSubmit();
      tick();
      expect(consoleErrorSpy).toHaveBeenCalledWith('Error creating patient:', jasmine.any(Error));
    }));
  });

  
});
