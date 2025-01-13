import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { UserLoginComponent } from './user-login.component';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

// Cream un mock simplu pentru AuthService
class MockAuthService {
  login(payload: any) {
    // Implicit vom considera că login-ul este cu succes și returnăm un obiect cu data
    return of({ data: 'dummyToken' });
  }
}

class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('UserLoginComponent', () => {
  let component: UserLoginComponent;
  let fixture: ComponentFixture<UserLoginComponent>;
  let authService: AuthService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        UserLoginComponent,  // Componenta este standalone
        ReactiveFormsModule,
        NoopAnimationsModule // Pentru a evita erorile legate de animații
      ],
      providers: [
        { provide: AuthService, useClass: MockAuthService },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserLoginComponent);
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
    expect(component.userForm.controls['username']).toBeTruthy();
    expect(component.userForm.controls['password']).toBeTruthy();
  });

  describe('onSubmit', () => {
    it('ar trebui să apeleze authService.login, să salveze tokenul în localStorage și să navigheze la /dashboard la submit valid', fakeAsync(() => {
      const loginSpy = spyOn(authService, 'login').and.callThrough();
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      // Setăm valori valide în formular
      component.userForm.setValue({
        username: 'testuser',
        password: 'testpassword'
      });
      
      component.onSubmit();
      tick();
      
      // Verificăm că metoda login a fost apelată cu payload-ul corect
      expect(loginSpy).toHaveBeenCalledWith({
        username: 'testuser',
        password: 'testpassword'
      });
      
      // Verificăm ca tokenul să fie salvat în localStorage
      expect(localStorage.getItem('token')).toBe('dummyToken');
      
      // Verificăm navigarea la /dashboard
      expect(routerSpy).toHaveBeenCalledWith(['/dashboard']);
    }));
    
    it('ar trebui să seteze mesajul de eroare dacă login-ul eșuează', fakeAsync(() => {
      // Setăm formularul valid
      component.userForm.setValue({
        username: 'invaliduser',
        password: 'invalidpassword'
      });
      // Simulăm un eșec al login-ului
      spyOn(authService, 'login').and.returnValue(throwError(() => ({ error: { errorMessage: 'Invalid credentials' } })));
      const consoleErrorSpy = spyOn(console, 'error');
      
      component.onSubmit();
      tick();
      
      // Verificăm că eroarea este logată și că mesajul de eroare este setat corespunzător
      expect(consoleErrorSpy).toHaveBeenCalled();
      expect(component.errorMessage).toBe('Invalid username or password.');
    }));
  });

  describe('backToHomepage', () => {
    it('ar trebui să navigheze la homepage', () => {
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      component.backToHomepage();
      expect(routerSpy).toHaveBeenCalledWith(['/']);
    });
  });
});
