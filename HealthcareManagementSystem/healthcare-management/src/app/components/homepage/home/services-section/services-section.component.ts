import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-services-section',
  imports: [CommonModule],
  templateUrl: './services-section.component.html',
  styleUrls: ['../home.component.css']
})
export class ServicesSectionComponent {
  services = [
    {
      title: 'Fast and Efficient Appointments',
      description: `Say goodbye to long waiting times and complicated booking processes. CareLink simplifies the way you schedule medical appointments, allowing you to find the right specialist, view their availability in real-time, and secure your slot with just a few clicks. Experience a seamless booking process tailored to your needs.`,
      image: 'appointment.svg',
      alt: 'An image representing appointment scheduling',
    },
    {
      title: 'Effortless Patient Data Management',
      description: `All your medical information in one place: personal details, consultation history, and doctor recommendations. Our secure platform ensures that your data is always up-to-date and easily accessible, empowering you to make informed decisions about your health. Stay organized and take control of your medical records effortlessly.`,
      image: 'record.svg',
      alt: 'An image representing patient data management',
    },
    {
      title: 'AI-Powered Health Predictions',
      description: `With cutting-edge machine learning technology, our predictive module analyzes your medical data to identify potential health risks and trends. Receive detailed reports and personalized insights that help you take proactive steps towards better health. Prevention starts with the right insights, and our AI tools are here to support your wellness journey.`,
      image: 'predict.svg',
      alt: 'An image representing AI health predictions',
    },
  ];
  
}
