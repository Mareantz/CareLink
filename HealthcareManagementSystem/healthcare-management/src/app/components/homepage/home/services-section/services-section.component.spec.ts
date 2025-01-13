import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServicesSectionComponent } from './services-section.component';
import { CommonModule } from '@angular/common';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('ServicesSectionComponent', () => {
  let component: ServicesSectionComponent;
  let fixture: ComponentFixture<ServicesSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServicesSectionComponent, CommonModule, NoopAnimationsModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ServicesSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('ar trebui să creeze componenta', () => {
    expect(component).toBeTruthy();
  });

  it('ar trebui să aibă o listă de servicii definită cu 3 elemente', () => {
    expect(component.services).toBeTruthy();
    expect(component.services.length).toBe(3);

    // Verificăm elementele individuale
    const firstService = component.services[0];
    expect(firstService.title).toBe('Fast and Efficient Appointments');
    expect(firstService.description).toContain('simplifies the way you schedule medical appointments');
    expect(firstService.image).toBe('appointment.svg');
    expect(firstService.alt).toBe('An image representing appointment scheduling');

    const secondService = component.services[1];
    expect(secondService.title).toBe('Effortless Patient Data Management');
    expect(secondService.description).toContain('All your medical information in one place');
    expect(secondService.image).toBe('record.svg');
    expect(secondService.alt).toBe('An image representing patient data management');

    const thirdService = component.services[2];
    expect(thirdService.title).toBe('AI-Powered Health Predictions');
    expect(thirdService.description).toContain('predictive module analyzes your medical data');
    expect(thirdService.image).toBe('predict.svg');
    expect(thirdService.alt).toBe('An image representing AI health predictions');
  });
});
