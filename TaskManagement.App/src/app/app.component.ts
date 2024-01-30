import { Component, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';

const IMAGE_ICON_URL = '../assets/images/task-icon.svg';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MatSidenavModule,
    MatListModule,
    MatToolbarModule,
    MatIconModule,
    RouterLink,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Gerenciador de Tarefas';
  imageUrl = IMAGE_ICON_URL;
  @ViewChild('sidenav') sideNav!: MatSidenav
  private router = inject(Router);

  navigateTo(path: string): void {
    this.router.navigate([path]);
    this.closeSideNav();
  }

  closeSideNav(): void {
    this.sideNav.close();
  }

  toggleSideNav(): void {
    this.sideNav.toggle();
  }
}
