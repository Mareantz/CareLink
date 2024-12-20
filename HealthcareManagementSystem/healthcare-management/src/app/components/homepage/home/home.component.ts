import { Component } from '@angular/core';
import { WelcomeSectionComponent } from './welcome-section/welcome-section.component';
import { AboutSectionComponent } from './about-section/about-section.component';
import { ServicesSectionComponent } from './services-section/services-section.component';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-home',
  imports: [WelcomeSectionComponent, AboutSectionComponent, ServicesSectionComponent, NavbarComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
