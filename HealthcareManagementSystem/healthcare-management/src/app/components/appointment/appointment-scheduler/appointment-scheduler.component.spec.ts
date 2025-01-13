import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AppointmentSchedulerComponent } from './appointment-scheduler.component';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import { AuthService } from '../../../services/auth.service';
import { BreakpointObserver, BreakpointState, Breakpoints } from '@angular/cdk/layout';
import { of, Subject, throwError } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

// Creăm un model simplificat pentru Appointment (după modelul folosit în componentă)
interface Appointment {
  appointmentDate: string;
  // Alte câmpuri pot exista, dar folosim doar appointmentDate pentru testele noastre
}

// Creăm mock-uri pentru serviciile injectate

class MockActivatedRoute {
  // Simulăm un snapshot în care paramMap conține un userId
  snapshot = {
    paramMap: {
      get: (key: string) => {
        if (key === 'userId') {
          return 'doctor123';
        }
        return null;
      }
    }
  };
}

class MockAppointmentService {
  getAppointmentsByDoctorAndDate(doctorId: string, date: string) {
    // Pentru test, returnăm o listă de programări fictive
    return of([
      { appointmentDate: new Date(date + 'T08:00:00').toISOString() },
      { appointmentDate: new Date(date + 'T09:30:00').toISOString() }
    ]);
  }

  createAppointment(appointment: any) {
    // Pentru test, returnăm payload-ul primit ca răspuns de succes
    return of(appointment);
  }
}

class MockAuthService {
  getUserId() {
    return 'patient456';
  }
}

class MockBreakpointObserver {
  // Pentru testele noastre, observatorul poate fi controlat cu un Subject
  private subject = new Subject<BreakpointState>();
  observe(query: string) {
    // Returnăm un observable controlat extern
    return this.subject.asObservable();
  }
  // Metodă ajutătoare pentru a emite un state
  emit(state: BreakpointState) {
    this.subject.next(state);
  }
}

class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('AppointmentSchedulerComponent', () => {
  let component: AppointmentSchedulerComponent;
  let fixture: ComponentFixture<AppointmentSchedulerComponent>;
  let appointmentService: AppointmentService;
  let authService: AuthService;
  let breakpointObserver: MockBreakpointObserver;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AppointmentSchedulerComponent, // componenta este standalone
        NoopAnimationsModule
      ],
      providers: [
        { provide: ActivatedRoute, useClass: MockActivatedRoute },
        { provide: AppointmentService, useClass: MockAppointmentService },
        { provide: AuthService, useClass: MockAuthService },
        { provide: BreakpointObserver, useClass: MockBreakpointObserver },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointmentSchedulerComponent);
    component = fixture.componentInstance;
    appointmentService = TestBed.inject(AppointmentService);
    authService = TestBed.inject(AuthService);
    breakpointObserver = TestBed.inject(BreakpointObserver) as unknown as MockBreakpointObserver;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  afterEach(() => {
    // Asigurăm apelul ngOnDestroy pentru a curăța subscription-ul
    component.ngOnDestroy();
  });

  it('ar trebui să creeze componenta și să seteze doctorId din ActivatedRoute', () => {
    expect(component).toBeTruthy();
    expect(component.doctorId).toBe('doctor123');
  });

  it('ar trebui să ajusteze numărul de coloane și rowHeight în funcție de breakpoint (handset)', fakeAsync(() => {
    // La emiterea unui breakpoint care reprezintă handset:
    breakpointObserver.emit({ matches: true } as BreakpointState);
    tick();
    expect(component.cols).toBe(2);
    expect(component.rowHeight).toBe('2:1');

    // La emiterea unui breakpoint care NU este handset:
    breakpointObserver.emit({ matches: false } as BreakpointState);
    tick();
    expect(component.cols).toBe(4);
    expect(component.rowHeight).toBe('4:1');
  }));

  describe('onDateChange', () => {
   

    it('ar trebui să încarce programările pentru o dată validă', fakeAsync(() => {
      // Setăm data selectată (de exemplu, 2025-03-15)
      const testDate = new Date(2025, 2, 15); // Note: lunile sunt zero-based
      spyOn(appointmentService, 'getAppointmentsByDoctorAndDate').and.callThrough();

      component.onDateChange(testDate);
      tick();

      // Formatul datei așa cum e construit în componentă:
      const year = testDate.getFullYear();
      const month = ('0' + (testDate.getMonth() + 1)).slice(-2);
      const day = ('0' + testDate.getDate()).slice(-2);
      const formattedDate = `${year}-${month}-${day}`;

      expect(appointmentService.getAppointmentsByDoctorAndDate).toHaveBeenCalledWith('doctor123', formattedDate);
      // Conform mock-ului, se așteaptă două programări
      expect(component.appointmentsForDay.length).toBe(2);
    }));

    it('ar trebui să trateze eroarea la încărcarea programărilor', fakeAsync(() => {
      // Simulăm o eroare la apelul serviciului
      spyOn(appointmentService, 'getAppointmentsByDoctorAndDate').and.returnValue(throwError(() => new Error('Error loading appointments')));
      spyOn(console, 'error');

      const testDate = new Date(2025, 2, 15);
      component.onDateChange(testDate);
      tick();

      expect(console.error).toHaveBeenCalledWith('Error loading appointments', jasmine.any(Error));
    }));
  });

  describe('isSlotTaken', () => {
    it('ar trebui să returneze false dacă nu este selectată nicio dată', () => {
      component.selectedDate = null;
      expect(component.isSlotTaken('08:00')).toBeFalse();
    });

    

    it('ar trebui să returneze false pentru un slot liber', () => {
      const testDate = new Date(2025, 2, 15);
      component.selectedDate = testDate;
      component.appointmentsForDay = [];
      expect(component.isSlotTaken('09:00')).toBeFalse();
    });
  });

  describe('bookSlot și closeModal', () => {
    it('bookSlot ar trebui să seteze slotul selectat și să afișeze modalul', () => {
      component.selectedDate = new Date();
      component.bookSlot('10:00');
      expect(component.selectedSlot).toBe('10:00');
      expect(component.isModalVisible).toBeTrue();
    });

    it('closeModal ar trebui să ascundă modalul și să golească reason', () => {
      component.isModalVisible = true;
      component.reason = 'Test reason';
      component.closeModal();
      expect(component.isModalVisible).toBeFalse();
      expect(component.reason).toBe('');
    });
  });

  describe('confirmBooking', () => {
    beforeEach(() => {
      // Asigurăm că este selectată o dată și un slot
      component.selectedDate = new Date(2025, 2, 15);
      component.selectedSlot = '11:00';
    });

    it('ar trebui să solicite introducerea unui motiv dacă acesta este gol', () => {
      component.reason = '   '; // spații
      spyOn(window, 'alert');
      component.confirmBooking();
      expect(window.alert).toHaveBeenCalledWith('Please enter a reason for the appointment.');
    });

    it('ar trebui să alerteze dacă nu se poate obține patientId-ul', () => {
      // Simulăm getUserId returnând null
      spyOn(authService, 'getUserId').and.returnValue(null);
      component.reason = 'Valid reason';
      spyOn(window, 'alert');
      component.confirmBooking();
      expect(window.alert).toHaveBeenCalledWith('Unable to retrieve patient information. Please log in again.');
    });

    it('ar trebui să creeze o programare și să reîncarce lista de programări', fakeAsync(() => {
      // Asigurăm că getUserId returnează o valoare validă
      spyOn(authService, 'getUserId').and.returnValue('patient456');
      component.reason = 'Consultation reason';
      // Spy pentru createAppointment și onDateChange
      const createSpy = spyOn(appointmentService, 'createAppointment').and.callThrough();
      const onDateChangeSpy = spyOn(component, 'onDateChange').and.callThrough();
      
      component.confirmBooking();
      tick();

      // Verificăm că createAppointment a fost apelată cu un obiect care conține proprietăți așteptate
      const [hour, minute] = component.selectedSlot.split(':').map(Number);
      const expectedDate = new Date(component.selectedDate as Date);
      expectedDate.setHours(hour, minute, 0, 0);

      expect(createSpy).toHaveBeenCalledWith(jasmine.objectContaining({
        patientId: 'patient456',
        doctorId: 'doctor123',
        date: expectedDate.toISOString(),
        reason: 'Consultation reason'
      }));

      // După crearea programării, modalul se închide și se reîncarcă programările
      expect(component.isModalVisible).toBeFalse();
      expect(component.reason).toBe('');
      expect(onDateChangeSpy).toHaveBeenCalledWith(component.selectedDate);
    }));

    it('ar trebui să trateze eroarea la crearea programării', fakeAsync(() => {
      spyOn(authService, 'getUserId').and.returnValue('patient456');
      component.reason = 'Consultation reason';
      spyOn(appointmentService, 'createAppointment').and.returnValue(throwError(() => new Error('Create error')));
      spyOn(window, 'alert');
      spyOn(console, 'error');

      component.confirmBooking();
      tick();

      expect(console.error).toHaveBeenCalledWith('Error creating appointment', jasmine.any(Error));
      expect(window.alert).toHaveBeenCalledWith('Could not create appointment. Please try again.');
    }));
  });

  describe('disableWeekends și allSlotsTaken', () => {
 

    

    it('allSlotsTaken ar trebui să returneze true dacă numărul de programări este mai mare sau egal cu numărul de sloturi', () => {
      // Simulăm că toate sloturile sunt ocupate
      component.appointmentsForDay = new Array(component.timeSlots.length).fill({ appointmentDate: new Date().toISOString() });
      expect(component.allSlotsTaken()).toBeTrue();
    });

    it('allSlotsTaken ar trebui să returneze false dacă nu toate sloturile sunt ocupate', () => {
      component.appointmentsForDay = []; // lista goală
      expect(component.allSlotsTaken()).toBeFalse();
    });
  });

  it('backToDoctorList ar trebui să navigheze către ruta /doctors', () => {
    const routerSpy = spyOn(router, 'navigate').and.callThrough();
    component.backToDoctorList();
    expect(routerSpy).toHaveBeenCalledWith(['/doctors']);
  });
});
