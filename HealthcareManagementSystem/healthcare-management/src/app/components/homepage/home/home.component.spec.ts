import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HomeComponent } from './home.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';

// Stub-uri pentru componentele importate (standalone)
// Deoarece componentele reale sunt standalone, creăm stub-uri minimaliste standalone.
import { Component } from '@angular/core';

@Component({
  selector: 'app-welcome-section',
  standalone: true,
  template: '<div class="welcome-section">Welcome Section</div>',
  imports: [CommonModule]
})
class MockWelcomeSectionComponent {}

@Component({
  selector: 'app-about-section',
  standalone: true,
  template: '<div class="about-section">About Section</div>',
  imports: [CommonModule]
})
class MockAboutSectionComponent {}

@Component({
  selector: 'app-services-section',
  standalone: true,
  template: '<div class="services-section">Services Section</div>',
  imports: [CommonModule]
})
class MockServicesSectionComponent {}

@Component({
  selector: 'app-navbar',
  standalone: true,
  template: '<nav class="navbar">Navbar</nav>',
  imports: [CommonModule]
})
class MockNavbarComponent {}

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HomeComponent,
        // Înlocuim componentele reale cu stub-uri pentru test:
        MockWelcomeSectionComponent,
        MockAboutSectionComponent,
        MockServicesSectionComponent,
        MockNavbarComponent,
        RouterTestingModule,
        NoopAnimationsModule,
        CommonModule,
        RouterModule,
        MatToolbarModule,
        MatSidenavModule,
        MatButtonModule,
        MatIconModule,
        MatListModule,
        MatCardModule
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('ar trebui să creeze componenta', () => {
    expect(component).toBeTruthy();
  });



  it('ar trebui să afișeze secțiunea Welcome', () => {
    const welcomeEl = fixture.debugElement.query(By.css('app-welcome-section'));
    expect(welcomeEl).toBeTruthy();
    expect(welcomeEl.nativeElement.textContent).toContain('Welcome');
  });

  it('ar trebui să afișeze secțiunea About', () => {
    const aboutEl = fixture.debugElement.query(By.css('app-about-section'));
    expect(aboutEl).toBeTruthy();
    expect(aboutEl.nativeElement.textContent).toContain('About');
  });

  it('ar trebui să afișeze secțiunea Services', () => {
    const servicesEl = fixture.debugElement.query(By.css('app-services-section'));
    expect(servicesEl).toBeTruthy();
    expect(servicesEl.nativeElement.textContent).toContain('Services');
  });
});
