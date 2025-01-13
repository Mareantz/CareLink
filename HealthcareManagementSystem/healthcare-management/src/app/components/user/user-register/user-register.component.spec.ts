import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserRegisterComponent } from './user-register.component';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { UserRole } from '../../../UserRole';

// Cream mock pentru AuthService
class MockAuthService {
  register(payload: any) {
    return of({ success: true, token: 'fakeToken' });
  }
}

// Cream mock pentru Router
class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('UserRegisterComponent', () => {
  let component: UserRegisterComponent;
  let fixture: ComponentFixture<UserRegisterComponent>;
  let authService: AuthService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        UserRegisterComponent, // componenta este standalone
        ReactiveFormsModule,
        NoopAnimationsModule
      ],
      providers: [
        FormBuilder,
        { provide: AuthService, useClass: MockAuthService },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserRegisterComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('ar trebui să creeze componenta', () => {
    expect(component).toBeTruthy();
  });

  it('ar trebui să initializeze formularul în ngOnInit', () => {
    component.ngOnInit();
    expect(component.userForm).toBeTruthy();
    const controls = component.userForm.controls;
    expect(controls['username']).toBeDefined();
    expect(controls['password']).toBeDefined();
    expect(controls['confirmPassword']).toBeDefined();
    expect(controls['firstName']).toBeDefined();
    expect(controls['lastName']).toBeDefined();
    expect(controls['email']).toBeDefined();
    expect(controls['phoneNumber']).toBeDefined();
    expect(controls['role']).toBeDefined();
    expect(controls['dateOfBirth']).toBeDefined();
    expect(controls['gender']).toBeDefined();
    expect(controls['address']).toBeDefined();
    expect(controls['specialization']).toBeDefined();
  });

  it('ar trebui să navigheze la homepage când se apelează backToHomepage', () => {
    const routerSpy = spyOn(router, 'navigate').and.callThrough();
    component.backToHomepage();
    expect(routerSpy).toHaveBeenCalledWith(['/']);
  });

  describe('onSubmit', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('ar trebui să trimită payload-ul corect și să navigheze la /login în caz de succes', () => {
      // Setăm valorile formularului pentru un user cu rol de Doctor
      component.userForm.setValue({
        username: 'testuser',
        password: 'password123',
        confirmPassword: 'password123',
        firstName: 'Test',
        lastName: 'User',
        email: 'test@example.com',
        phoneNumber: '1234567890',
        role: UserRole.Doctor,
        dateOfBirth: '2000-01-01', // Aceste câmpuri vor fi ignorate pentru rolul Doctor
        gender: 'M',
        address: 'Some Address',
        specialization: 'Cardiology'
      });

      // Cream spy-uri pe metodele register și navigate
      const authSpy = spyOn(authService, 'register').and.callThrough();
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      const alertSpy = spyOn(window, 'alert');

      component.onSubmit();

      // Determinăm valoarea rolului
      // Pentru Doctor metoda getRoleNumber returnează 1
      const expectedPayload = {
        username: 'testuser',
        password: 'password123',
        confirmPassword: 'password123',
        email: 'test@example.com',
        phoneNumber: '1234567890',
        role: 1, // rol de Doctor
        firstName: 'Test',
        lastName: 'User',
        // pentru Doctor, specialization este setat, iar cele pentru Patient sunt null
        dateOfBirth: null,
        gender: null,
        address: null,
        specialization: 'Cardiology'
      };

      expect(authSpy).toHaveBeenCalledWith(expectedPayload);
      expect(routerSpy).toHaveBeenCalledWith(['/login']);
      expect(alertSpy).toHaveBeenCalledWith('Registration successful! You can now log in.');
    });

    it('ar trebui să trateze eroarea în cazul în care înregistrarea eșuează', () => {
      // Simulăm o eroare la înregistrare
      spyOn(authService, 'register').and.returnValue(throwError(() => new Error('Registration failed')));
      const alertSpy = spyOn(window, 'alert');
      const consoleErrorSpy = spyOn(console, 'error');

      // Setăm valori valide în formular
      component.userForm.setValue({
        username: 'testuser',
        password: 'password123',
        confirmPassword: 'password123',
        firstName: 'Test',
        lastName: 'User',
        email: 'test@example.com',
        phoneNumber: '1234567890',
        role: UserRole.Patient,
        dateOfBirth: '2000-01-01',
        gender: 'F',
        address: 'Some Address',
        specialization: '' // pentru un pacient, nu se utilizează
      });

      component.onSubmit();

      expect(consoleErrorSpy).toHaveBeenCalled();
      expect(alertSpy).toHaveBeenCalledWith('An error occurred. Please try again.');
    });
  });
});
