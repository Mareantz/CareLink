import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { DoctorCardComponent } from './doctor-card.component';
import { By } from '@angular/platform-browser';

describe('DoctorCardComponent', () => {
  let component: DoctorCardComponent;
  let fixture: ComponentFixture<DoctorCardComponent>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    // Creăm un spy pentru Router
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      // Deoarece componenta este standalone, o importăm direct aici.
      imports: [DoctorCardComponent],
      providers: [
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DoctorCardComponent);
    component = fixture.componentInstance;

    // Setăm valori de intrare necesare pentru a evita eventuale erori în template.
    component.firstName = 'Ion';
    component.lastName = 'Popescu';
    component.specialization = 'Cardiologie';
    component.bio = 'Descrierea doctorului';
    component.photoUrl = 'assets/test_doctor.jpg';
    component.doctorId = '123';

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to appointment scheduler when doctorId is provided', () => {
    // Apelăm metoda makeAppointment
    component.makeAppointment();

    expect(routerSpy.navigate).toHaveBeenCalledWith(['/appointments/scheduler', component.doctorId]);
  });

  it('should log error when doctorId is undefined', () => {
    // Folosim spy pe console.error pentru a verifica mesajul de eroare
    spyOn(console, 'error');

    // Setați doctorId ca fiind undefined
    component.doctorId = undefined as any;

    component.makeAppointment();

    expect(console.error).toHaveBeenCalledWith('doctorId is undefined');
    // De asemenea, putem verifica că nu a avut loc navigarea
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  });
  

 
});
