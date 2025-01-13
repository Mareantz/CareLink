import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NavbarComponent } from './navbar.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarComponent, NoopAnimationsModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('ar trebui să creeze componenta', () => {
    expect(component).toBeTruthy();
  });

  it('toggleMenu() ar trebui să inverseze valoarea lui menuOpen', () => {
    // Inițial, menuOpen este false
    expect(component.menuOpen).toBeFalse();

    component.toggleMenu();
    expect(component.menuOpen).toBeTrue();

    component.toggleMenu();
    expect(component.menuOpen).toBeFalse();
  });
});
