import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { DoctorListComponent } from './doctor-list.component';
import { DoctorService } from '../../../services/doctor/doctor.service';
import { BreakpointObserver, BreakpointState, Breakpoints } from '@angular/cdk/layout';
import { Router } from '@angular/router';
import { of, Subject } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

// Model simplificat pentru Doctor
interface Doctor {
  id: string;
  name: string;
  photoUrl?: string;
  // alte câmpuri opționale...
}

// Creăm mock-uri pentru serviciile injectate

class MockDoctorService {
  getDoctors() {
    // Returnăm o listă de doctori; dacă un doctor nu are photoUrl, componenta va seta stock_photo
    const doctors: Doctor[] = [
      { id: '1', name: 'Dr. One', photoUrl: 'photo1.jpg' },
      { id: '2', name: 'Dr. Two' } // lipsă photoUrl
    ];
    return of(doctors);
  }
}

class MockBreakpointObserver {
  // Pentru a putea emite valori controlate
  private subject = new Subject<BreakpointState>();

  observe(queries: string[]) {
    return this.subject.asObservable();
  }

  emit(state: BreakpointState) {
    this.subject.next(state);
  }
}

class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('DoctorListComponent', () => {
  let component: DoctorListComponent;
  let fixture: ComponentFixture<DoctorListComponent>;
  let doctorService: DoctorService;
  let breakpointObserver: MockBreakpointObserver;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      // Deoarece componenta este standalone, o importăm direct
      imports: [DoctorListComponent, NoopAnimationsModule],
      providers: [
        { provide: DoctorService, useClass: MockDoctorService },
        { provide: BreakpointObserver, useClass: MockBreakpointObserver },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DoctorListComponent);
    component = fixture.componentInstance;
    doctorService = TestBed.inject(DoctorService);
    breakpointObserver = TestBed.inject(BreakpointObserver) as unknown as MockBreakpointObserver;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('ar trebui să creeze componenta și să apeleze fetchDoctors() în ngOnInit', () => {
    expect(component).toBeTruthy();
    // După fetchDoctors(), lista de doctori trebuie să fie populată
    expect(component.doctors.length).toBe(2);
  });

  it('fetchDoctors() ar trebui să seteze stock_photo pentru doctorii fără photoUrl', () => {
    // Primul doctor are photoUrl, al doilea nu
    expect(component.doctors[0].photoUrl).toBe('photo1.jpg');
    expect(component.doctors[1].photoUrl).toBe(component.stock_photo);
  });

  describe('setupGridCols', () => {
    it('ar trebui să seteze cols=1 pentru breakpoints HandsetPortrait sau HandsetLandscape', fakeAsync(() => {
      // Simulam cazul pentru HandsetPortrait
      const handsetState: BreakpointState = {
        matches: true,
        breakpoints: { [Breakpoints.HandsetPortrait]: true }
      };
      breakpointObserver.emit(handsetState);
      tick();
      expect(component.cols).toBe(1);

      // Simulam cazul pentru HandsetLandscape
      const handsetLandscapeState: BreakpointState = {
        matches: true,
        breakpoints: { [Breakpoints.HandsetLandscape]: true }
      };
      breakpointObserver.emit(handsetLandscapeState);
      tick();
      expect(component.cols).toBe(1);
    }));

    it('ar trebui să seteze cols=2 pentru breakpoints TabletPortrait sau TabletLandscape', fakeAsync(() => {
      const tabletState: BreakpointState = {
        matches: true,
        breakpoints: { [Breakpoints.TabletPortrait]: true }
      };
      breakpointObserver.emit(tabletState);
      tick();
      expect(component.cols).toBe(2);

      const tabletLandscapeState: BreakpointState = {
        matches: true,
        breakpoints: { [Breakpoints.TabletLandscape]: true }
      };
      breakpointObserver.emit(tabletLandscapeState);
      tick();
      expect(component.cols).toBe(2);
    }));

    it('ar trebui să seteze cols=3 pentru celelalte breakpoints (WebPortrait/WebLandscape)', fakeAsync(() => {
      const webState: BreakpointState = {
        matches: true,
        breakpoints: { [Breakpoints.WebPortrait]: true }
      };
      breakpointObserver.emit(webState);
      tick();
      expect(component.cols).toBe(3);

      const webLandscapeState: BreakpointState = {
        matches: true,
        breakpoints: { [Breakpoints.WebLandscape]: true }
      };
      breakpointObserver.emit(webLandscapeState);
      tick();
      expect(component.cols).toBe(3);
    }));
  });

  it('backToDashboard() ar trebui să navigheze la ruta /dashboard', () => {
    const routerSpy = spyOn(router, 'navigate').and.callThrough();
    component.backToDashboard();
    expect(routerSpy).toHaveBeenCalledWith(['/dashboard']);
  });
});
