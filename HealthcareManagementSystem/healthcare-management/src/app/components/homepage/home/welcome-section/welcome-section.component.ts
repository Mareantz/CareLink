import { Component } from '@angular/core';

@Component({
  selector: 'app-welcome-section',
  imports: [],
  templateUrl: './welcome-section.component.html',
  styleUrl: '../home.component.css',
  host: {'class': 'welcome', 'id': 'welcome'}
})
export class WelcomeSectionComponent {
}
