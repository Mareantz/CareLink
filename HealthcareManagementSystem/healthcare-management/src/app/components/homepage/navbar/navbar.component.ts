import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-navbar',
  imports : [],
  templateUrl: './navbar.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['../home/home.component.css'],
  host: {'class': 'navbar'}
})
export class NavbarComponent {
  menuOpen = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
}
