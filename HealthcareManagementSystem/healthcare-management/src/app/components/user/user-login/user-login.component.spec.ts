import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserLoginComponent } from './user-login.component';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

// Creăm mock-uri simple pentru AuthService și Router
class MockAuthService {
  login(payload: any) {
    // Simulează un răspuns de succes, returnând un token
    return of({ token: 'fakeToken' });
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
      // Importăm NoopAnimationsModule pentru a evita erorile legate de animații
      imports: [
        UserLoginComponent,  // componenta este standalone
        ReactiveFormsModule,
        NoopAnimationsModule
      ],
      providers: [
        FormBuilder,
        { provide: AuthService, useClass: MockAuthService },
        { provide: Router, useClass: MockRouter },
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
    expect(component.userForm).toBeTruthy();
    expect(component.userForm.controls['username']).toBeTruthy();
    expect(component.userForm.controls['password']).toBeTruthy();
  });

  it('ar trebui să apeleze authService.login și să navigheze la /dashboard la submit valid', () => {
    const spyLogin = spyOn(authService, 'login').and.returnValue(of({ token: 'fakeToken' }));
    const spyNavigate = spyOn(router, 'navigate').and.callThrough();

    component.userForm.setValue({ username: 'testuser', password: 'testpass' });
    component.onSubmit();

    expect(spyLogin).toHaveBeenCalledWith({ username: 'testuser', password: 'testpass' });
    expect(localStorage.getItem('token')).toBe('fakeToken');
    expect(spyNavigate).toHaveBeenCalledWith(['/dashboard']);
  });

  it('ar trebui să afișeze mesaj de eroare dacă login eșuează', () => {
    const spyLogin = spyOn(authService, 'login').and.returnValue(throwError(() => 'Eroare de login'));
    component.userForm.setValue({ username: 'testuser', password: 'wrongpass' });
    component.onSubmit();

    expect(spyLogin).toHaveBeenCalled();
    expect(component.errorMessage).toBe('Invalid username or password.');
  });

  it('ar trebui să navigheze la homepage când se apelează backToHomepage', () => {
    const spyNavigate = spyOn(router, 'navigate').and.callThrough();
    component.backToHomepage();
    expect(spyNavigate).toHaveBeenCalledWith(['/']);
  });
});
