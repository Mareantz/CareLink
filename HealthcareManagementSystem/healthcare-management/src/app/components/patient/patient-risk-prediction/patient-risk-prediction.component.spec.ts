import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { PatientRiskPredictionComponent } from './patient-risk-prediction.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { of, throwError } from 'rxjs';
import { PatientRiskPredictionService } from '../../../services/patient/patient-risk-prediction.service';

// Creăm un mock pentru PatientRiskPredictionService
class MockPatientRiskPredictionService {
  predictRisk(payload: any) {
    // Pentru testele de succes, returnăm un obiect de răspuns
    return of({ risk: 'High', probability: 0.85 });
  }
}

describe('PatientRiskPredictionComponent', () => {
  let component: PatientRiskPredictionComponent;
  let fixture: ComponentFixture<PatientRiskPredictionComponent>;
  let riskPredictionService: PatientRiskPredictionService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        PatientRiskPredictionComponent,  // Componenta este standalone
        ReactiveFormsModule,
        CommonModule,
      ],
      providers: [
        { provide: PatientRiskPredictionService, useClass: MockPatientRiskPredictionService }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientRiskPredictionComponent);
    component = fixture.componentInstance;
    riskPredictionService = TestBed.inject(PatientRiskPredictionService);
    fixture.detectChanges();
  });

  it('ar trebui să creeze componenta și să initializeze formularul în ngOnInit', () => {
    expect(component).toBeTruthy();
    expect(component.riskForm).toBeTruthy();
    const controls = component.riskForm.controls;
    expect(controls['age']).toBeDefined();
    expect(controls['gender']).toBeDefined();
    expect(controls['weight']).toBeDefined();
    expect(controls['bloodPressure']).toBeDefined();
    expect(controls['cholesterolLevel']).toBeDefined();
    expect(controls['physicalActivityLevel']).toBeDefined();
    expect(controls['smokingStatus']).toBeDefined();
    expect(controls['stressLevel']).toBeDefined();
  });

  describe('onSubmit', () => {
    it('ar trebui să apeleze predictRisk și să seteze predictionResult atunci când formularul este valid', fakeAsync(() => {
      // Setăm valori valide în formular
      component.riskForm.setValue({
        age: 45,
        gender: 'male',
        weight: 80,
        bloodPressure: 120,
        cholesterolLevel: 180,
        physicalActivityLevel: 1,
        smokingStatus: 0,
        stressLevel: 1,
      });

      // Cream spy pentru serviciul de predictRisk
      const serviceSpy = spyOn(riskPredictionService, 'predictRisk').and.callThrough();

      component.onSubmit();
      tick();
      
      expect(serviceSpy).toHaveBeenCalledWith(component.riskForm.value);
      expect(component.predictionResult).toEqual({ risk: 'High', probability: 0.85 });
    }));

    it('ar trebui să afișeze alert dacă formularul este invalid', () => {
      spyOn(window, 'alert');
      // Setăm formularul ca invalid (de exemplu, age lipsă)
      component.riskForm.setValue({
        age: null,
        gender: 'male',
        weight: 80,
        bloodPressure: 120,
        cholesterolLevel: 180,
        physicalActivityLevel: 1,
        smokingStatus: 0,
        stressLevel: 1,
      });
      component.onSubmit();
      expect(window.alert).toHaveBeenCalledWith('Please fill out all required fields correctly.');
    });

    it('ar trebui să trateze eroarea dacă predictRisk eșuează', fakeAsync(() => {
      spyOn(window, 'alert');
      // Modificăm serviciul pentru a returna o eroare
      spyOn(riskPredictionService, 'predictRisk').and.returnValue(throwError(() => new Error('Prediction error')));
      // Setăm formularul valid
      component.riskForm.setValue({
        age: 50,
        gender: 'female',
        weight: 70,
        bloodPressure: 110,
        cholesterolLevel: 160,
        physicalActivityLevel: 2,
        smokingStatus: 1,
        stressLevel: 0,
      });
      component.onSubmit();
      tick();
      expect(window.alert).toHaveBeenCalledWith('An error occurred while predicting risk. Please try again.');
      // În caz de eroare, predictionResult nu este setat
      expect(component.predictionResult).toBeUndefined();
    }));
  });
});
