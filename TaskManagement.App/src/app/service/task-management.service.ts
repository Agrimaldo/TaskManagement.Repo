import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, of, shareReplay, tap } from 'rxjs';
import { TaskListResponse } from '../../Types/Response/task-types';
import { FormDialogData } from '../../shared/task-interfaces';

const BASE_URL = 'http://localhost:5056/api';

@Injectable({
  providedIn: 'root'
})
export class TaskManagementService {
  private taskListCache$: Observable<TaskListResponse> | null = null;

  private http = inject(HttpClient);
  constructor() { }

  public List(page: number, size: number): Observable<TaskListResponse> {
    return this.http
      .get<TaskListResponse>(`${BASE_URL}/Task?Page=${page}&Size=${size}`)
      .pipe(shareReplay(1));
  }

  public Delete(id: string): Observable<boolean> {
    return this.http
      .delete<boolean>(`${BASE_URL}/Task/${id}`)
      .pipe(shareReplay(1));
  }

  public Create(data: FormDialogData): Observable<boolean> {
    return this.http
      .post<boolean>(`${BASE_URL}/Task`, data)
      .pipe(shareReplay(1));
  }

  public Update(data: FormDialogData): Observable<boolean> {
    return this.http
      .put<boolean>(`${BASE_URL}/Task`, data)
      .pipe(shareReplay(1));
  }
}
