import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { PatientListComponent } from './patient-list.component';
import { PatientService } from '../../../services/patient/patient.service';
import { Router } from '@angular/router';
import { of, throwError, Subject } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

// Simplificăm modelul Patient
interface Patient {
  id: string;
  firstName: string;
  lastName: string;
}

// MOCK pentru PatientService
class MockPatientService {
  getFilteredPatients(page: number, pageSize: number, firstName?: string, lastName?: string, gender?: string) {
    // Pentru succes, returnăm un obiect cu datele și totalCount
    const dummyPatients: Patient[] = [
      { id: '1', firstName: 'Alice', lastName: 'Wonderland' },
      { id: '2', firstName: 'Bob', lastName: 'Marley' }
    ];
    return of({ data: { data: dummyPatients, totalCount: 2 } });
  }
}

// MOCK pentru Router
class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('PatientListComponent', () => {
  let component: PatientListComponent;
  let fixture: ComponentFixture<PatientListComponent>;
  let patientService: PatientService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientListComponent, CommonModule, FormsModule, NoopAnimationsModule],
      providers: [
        { provide: PatientService, useClass: MockPatientService },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientListComponent);
    component = fixture.componentInstance;
    patientService = TestBed.inject(PatientService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  // Test de creare a componentei
  it('ar trebui să creeze componenta', () => {
    expect(component).toBeTruthy();
  });

  // Test pentru fetchPatients (succes)
  describe('fetchPatients', () => {
    

    it('ar trebui să trateze eroarea la fetchPatients', fakeAsync(() => {
      spyOn(patientService, 'getFilteredPatients').and.returnValue(
        throwError(() => new Error('Fetch error'))
      );
      const consoleErrorSpy = spyOn(console, 'error');
      component.fetchPatients(1);
      tick();
      expect(consoleErrorSpy).toHaveBeenCalledWith('Error fetching patients:', jasmine.any(Error));
      expect(component.patients.length).toBe(0);
      expect(component.totalPatients).toBe(0);
    }));
  });

  // Test pentru onSearchChange și applyFilter
  describe('Search and filter', () => {
    it('onSearchChange ar trebui să emite un eveniment prin searchSubject', fakeAsync(() => {
      const searchSpy = spyOn(component['searchSubject'], 'next').and.callThrough();
      component.onSearchChange();
      tick(300); // debounceTime(300)
      expect(searchSpy).toHaveBeenCalled();
    }));

    it('applyFilter ar trebui să apeleze paginator.firstPage() și fetchPatients', fakeAsync(() => {
      // Cream un fake paginator cu o metodă firstPage
      const fakePaginator = { firstPage: jasmine.createSpy('firstPage') } as unknown as MatPaginator;
      component.paginator = fakePaginator;
      const fetchSpy = spyOn(component, 'fetchPatients').and.callThrough();

      component.applyFilter();
      tick();
      expect(fakePaginator.firstPage).toHaveBeenCalled();
      expect(fetchSpy).toHaveBeenCalled();
    }));
  });

  // Test pentru onPageChange
  describe('onPageChange', () => {
    it('ar trebui să actualizeze pageSize și să apeleze fetchPatients cu pagina corectă', fakeAsync(() => {
      const fetchSpy = spyOn(component, 'fetchPatients').and.callThrough();
      // Simulăm un eveniment de paginare
      const event: PageEvent = {
        pageIndex: 1,
        pageSize: 20,
        length: 100
      };
      component.onPageChange(event);
      tick();
      expect(component.pageSize).toBe(20);
      // Pagina index 1 înseamnă pagina 2 (1+1)
      expect(fetchSpy).toHaveBeenCalledWith(2);
    }));
  });

  // Teste pentru navigații
  describe('Navigation methods', () => {
    it('backToDashboard ar trebui să navigheze la /dashboard', () => {
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      component.backToDashboard();
      expect(routerSpy).toHaveBeenCalledWith(['/dashboard']);
    });

    it('navigateToCreate ar trebui să navigheze la /patients/create', () => {
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      component.navigateToCreate();
      expect(routerSpy).toHaveBeenCalledWith(['/patients/create']);
    });

    it('navigateToFind ar trebui să navigheze la /patients/find', () => {
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      component.navigateToFind();
      expect(routerSpy).toHaveBeenCalledWith(['/patients/find']);
    });

    it('navigateToRiskPrediction ar trebui să navigheze la /patients/risk-prediction', () => {
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      component.navigateToRiskPrediction();
      expect(routerSpy).toHaveBeenCalledWith(['/patients/risk-prediction']);
    });
  });

  // Test pentru unsubscribe in ngOnDestroy
  describe('ngOnDestroy', () => {
    it('ar trebui să completeze unsubscribe$', () => {
      const nextSpy = spyOn(component['unsubscribe$'], 'next').and.callThrough();
      const completeSpy = spyOn(component['unsubscribe$'], 'complete').and.callThrough();
      component.ngOnDestroy();
      expect(nextSpy).toHaveBeenCalled();
      expect(completeSpy).toHaveBeenCalled();
    });
  });
});
