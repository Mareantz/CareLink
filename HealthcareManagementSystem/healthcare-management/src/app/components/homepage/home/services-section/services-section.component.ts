import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-services-section',
  imports: [CommonModule],
  templateUrl: './services-section.component.html',
  styleUrls: ['../home.component.css'],
})
export class ServicesSectionComponent {
  services = [
    {
      title: 'Fast and Efficient Appointments',
      description: `Say goodbye to long waiting times and complicated booking processes. CareLink simplifies the way 
      you schedule medical appointments.`,
      image: 'appointment.svg',
      alt: 'An image representing appointment scheduling',
    },
    {
      title: 'Effortless Patient Data Management',
      description: `All your medical information in one place: personal details, consultation history, and doctor 
      recommendations.`,
      image: 'record.svg',
      alt: 'An image representing patient data management',
    },
    {
      title: 'AI-Powered Health Predictions',
      description: `With cutting-edge machine learning technology, our predictive module analyzes your medical data.`,
      image: 'predict.svg',
      alt: 'An image representing AI health predictions',
    },
  ];
}
