import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PatientRiskPredictionService } from '../../../services/patient/patient-risk-prediction.service';

@Component({
  selector: 'app-patient-risk-prediction',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './patient-risk-prediction.component.html',
  styleUrls: ['./patient-risk-prediction.component.css']
})
export class PatientRiskPredictionComponent implements OnInit {
  riskForm!: FormGroup;
  predictionResult: any;

  constructor(
    private fb: FormBuilder,
    private patientRiskPredictionService: PatientRiskPredictionService
  ) {}

  ngOnInit(): void {
    this.riskForm = this.fb.group({
      age: [null, [Validators.required, Validators.min(0)]],
      gender: ['', [Validators.required]],
      weight: [null, [Validators.required, Validators.min(0)]],
      bloodPressure: [null, [Validators.required, Validators.min(60), Validators.max(180)]],
      cholesterolLevel: [null, [Validators.required, Validators.min(40), Validators.max(200)]],
      physicalActivityLevel: [null, [Validators.required, Validators.min(0), Validators.max(2)]],
      smokingStatus: [null, [Validators.required, Validators.min(0), Validators.max(1)]],
      stressLevel: [null, [Validators.required, Validators.min(0), Validators.max(2)]]
    });
  }

  onSubmit() {
    if (this.riskForm.valid) {
      const payload = this.riskForm.value;
  
      this.patientRiskPredictionService.predictRisk(payload).subscribe({
        next: (response: any) => {
          this.predictionResult = response;
        },
        error: (error) => {
          console.error('Prediction failed:', error);
          alert('An error occurred while predicting risk. Please try again.');
        }
      });
    } else {
      alert('Please fill out all required fields correctly.');
    }
  }
}
