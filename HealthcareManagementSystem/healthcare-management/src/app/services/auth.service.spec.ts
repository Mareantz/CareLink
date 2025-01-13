import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { UserRole } from '../UserRole';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: Router, useValue: routerSpy }
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('#register', () => {
    it('should send a POST request to register endpoint', () => {
      const mockUserData = { username: 'testuser', password: 'password' };
      const mockResponse = { message: 'Registration successful' };

      service.register(mockUserData).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${service['apiUrl']}/register`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(mockUserData);
      req.flush(mockResponse);
    });
  });

  describe('#login', () => {
    it('should send a POST request to login endpoint', () => {
      const mockUserData = { username: 'testuser', password: 'password' };
      const mockResponse = { token: 'dummy-jwt-token' };

      service.login(mockUserData).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${service['apiUrl']}/login`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(mockUserData);
      req.flush(mockResponse);
    });
  });

  describe('#saveToken and #getToken', () => {
    it('should save and retrieve token from localStorage', () => {
      const token = 'sample-token';
      service.saveToken(token);
      expect(localStorage.getItem('token')).toBe(token);
      
      const retrievedToken = service.getToken();
      expect(retrievedToken).toBe(token);
    });

    it('should return null if no token is present', () => {
      const token = service.getToken();
      expect(token).toBeNull();
    });
  });

  describe('#clearToken', () => {
    it('should remove token from localStorage', () => {
      localStorage.setItem('token', 'sample-token');
      service.clearToken();
      expect(localStorage.getItem('token')).toBeNull();
    });
  });

  describe('#getAuthHeaders', () => {
    it('should return HttpHeaders with Authorization if token exists', () => {
      const token = 'sample-token';
      service.saveToken(token);

      const headers = service.getAuthHeaders();
      expect(headers.get('Authorization')).toBe(`Bearer ${token}`);
    });

    it('should return HttpHeaders without Authorization if token does not exist', () => {
      const headers = service.getAuthHeaders();
      expect(headers.get('Authorization')).toBe('Bearer null');
    });
  });

  describe('#getUserRole', () => {
    it('should return correct UserRole based on token', () => {
      // Mock token with role 'Doctor'
      const payload = {
        user_id: 'doctor123',
        role: 'Doctor'
      };
      const token = generateMockToken(payload);
      service.saveToken(token);

      const role = service.getUserRole();
      expect(role).toBe(UserRole.Doctor);
    });

    it('should return UserRole.None if token is invalid', () => {
      service.saveToken('invalid.token.here');
      const role = service.getUserRole();
      expect(role).toBe(UserRole.None);
    });

    it('should return UserRole.None if no token is present', () => {
      const role = service.getUserRole();
      expect(role).toBe(UserRole.None);
    });
  });

  describe('#getUserId', () => {
    it('should return user ID from token', () => {
      const payload = {
        user_id: 'user123',
        role: 'Patient'
      };
      const token = generateMockToken(payload);
      service.saveToken(token);

      const userId = service.getUserId();
      expect(userId).toBe('user123');
    });

    it('should return null if token is invalid', () => {
      service.saveToken('invalid.token.here');
      const userId = service.getUserId();
      expect(userId).toBeNull();
    });

    it('should return null if no token is present', () => {
      const userId = service.getUserId();
      expect(userId).toBeNull();
    });
  });

  describe('#getUserDetails', () => {
    it('should return user details from token', () => {
      const payload = {
        user_id: 'user123',
        role: 'Admin'
      };
      const token = generateMockToken(payload);
      service.saveToken(token);

      const userDetails = service.getUserDetails();
      expect(userDetails).toEqual({ id: 'user123', role: UserRole.Admin });
    });

    it('should return null if token is invalid', () => {
      service.saveToken('invalid.token.here');
      const userDetails = service.getUserDetails();
      expect(userDetails).toBeNull();
    });

    it('should return null if no token is present', () => {
      const userDetails = service.getUserDetails();
      expect(userDetails).toBeNull();
    });
  });

  describe('#logout', () => {
    it('should clear token and navigate to login', () => {
      service.saveToken('sample-token');
      service.logout();

      expect(localStorage.getItem('token')).toBeNull();
      expect(router.navigate).toHaveBeenCalledWith(['/login']);
    });
  });

  // Utility function to generate a mock JWT token
  function generateMockToken(payload: any): string {
    const header = { alg: 'HS256', typ: 'JWT' };
    const encode = (obj: any) => btoa(JSON.stringify(obj));

    return `${encode(header)}.${encode(payload)}.signature`;
  }
});