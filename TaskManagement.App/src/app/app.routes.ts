import { Routes } from '@angular/router';
import { TaskListComponent } from './component/task-list/task-list.component';

export const routes: Routes = [
  { path: '', component: TaskListComponent },
  { path: '', pathMatch: 'full', redirectTo: '' }
];
