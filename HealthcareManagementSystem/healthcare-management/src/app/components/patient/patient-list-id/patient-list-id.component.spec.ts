import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { PatientListIdComponent } from './patient-list-id.component';
import { PatientService } from '../../../services/patient/patient.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// Definim o interfață simplificată pentru Patient (după modelul folosit de componentă)
interface Patient {
  id: string;
  firstName: string;
  lastName: string;
}

// MOCK pentru PatientService
class MockPatientService {
  getPatientById(id: string) {
    const dummyPatient: Patient = { id, firstName: 'Test', lastName: 'Patient' };
    return of(dummyPatient);
  }
  deletePatientById(id: string) {
    return of({ success: true });
  }
}

// MOCK pentru Router
class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('PatientListIdComponent', () => {
  let component: PatientListIdComponent;
  let fixture: ComponentFixture<PatientListIdComponent>;
  let patientService: PatientService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientListIdComponent, CommonModule, FormsModule],
      providers: [
        { provide: PatientService, useClass: MockPatientService },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientListIdComponent);
    component = fixture.componentInstance;
    patientService = TestBed.inject(PatientService);
    router = TestBed.inject(Router);
    // Inițializăm o stare goală
    component.patientId = '';
    component.patient = null;
    fixture.detectChanges();
  });

  // Testele pentru fetchPatientById
  describe('fetchPatientById', () => {
    it('ar trebui să nu apeleze getPatientById dacă patientId este gol', () => {
      const spy = spyOn(patientService, 'getPatientById');
      component.patientId = '';
      component.fetchPatientById();
      expect(spy).not.toHaveBeenCalled();
    });

 

    it('ar trebui să seteze patient la null și să logheze eroarea dacă getPatientById returnează eroare', fakeAsync(() => {
      component.patientId = '123';
      const errorMessage = 'Error fetching patient data';
      spyOn(patientService, 'getPatientById').and.returnValue(throwError(() => new Error(errorMessage)));
      const consoleErrorSpy = spyOn(console, 'error');

      component.fetchPatientById();
      tick();

      expect(consoleErrorSpy).toHaveBeenCalledWith('Error fetching patient:', jasmine.any(Error));
      expect(component.patient).toBeNull();
    }));
  });

  // Testele pentru navigateToUpdateWithId
  describe('navigateToUpdateWithId', () => {
    it('ar trebui să navigheze către ruta de update atunci când patientId este specificat', () => {
      component.patientId = '456';
      const routerSpy = spyOn(router, 'navigate').and.callThrough();

      component.navigateToUpdateWithId();

      expect(routerSpy).toHaveBeenCalledWith(['/patients/update', '456']);
    });

    it('ar trebui să nu navigheze dacă patientId nu este specificat', () => {
      component.patientId = '';
      const routerSpy = spyOn(router, 'navigate').and.callThrough();

      component.navigateToUpdateWithId();

      expect(routerSpy).not.toHaveBeenCalled();
    });
  });

  // Testele pentru deletePatientById
  describe('deletePatientById', () => {
    it('ar trebui să nu apeleze deletePatientById dacă patientId este gol', () => {
      component.patientId = '';
      const spy = spyOn(patientService, 'deletePatientById').and.callThrough();
      component.deletePatientById();
      expect(spy).not.toHaveBeenCalled();
    });

  

    it('ar trebui să logheze eroarea dacă deletePatientById eșuează', fakeAsync(() => {
      component.patientId = '789';
      spyOn(patientService, 'deletePatientById').and.returnValue(throwError(() => new Error('Delete error')));
      const consoleErrorSpy = spyOn(console, 'error');

      component.deletePatientById();
      tick();

      expect(consoleErrorSpy).toHaveBeenCalledWith('Error deleting patient:', jasmine.any(Error));
    }));
  });

  // Test pentru backToPatientList
  describe('backToPatientList', () => {
    it('ar trebui să navigheze la ruta /patients', () => {
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      component.backToPatientList();
      expect(routerSpy).toHaveBeenCalledWith(['/patients']);
    });
  });
});
