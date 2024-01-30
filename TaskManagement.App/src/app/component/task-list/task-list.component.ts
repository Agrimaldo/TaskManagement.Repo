import { AfterViewInit, Component, ViewChild, inject } from '@angular/core';
import { formatDate } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { TaskManagementService } from '../../service/task-management.service';
import { Observable, catchError, EMPTY, map, tap } from 'rxjs';
import { TaskListResponse, TaskResponse } from '../../../Types/Response/task-types';
import { MatDialog } from '@angular/material/dialog';
import { TaskDeleteComponent } from '../task-delete/task-delete.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TaskFormComponent } from '../task-form/task-form.component';
import { FormDialogData } from '../../../shared/task-interfaces';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatIconModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss'
})
export class TaskListComponent implements AfterViewInit {
  constructor(private dialog: MatDialog, private snackBar: MatSnackBar) { }

  private taskManagementService = inject(TaskManagementService);
  taskList$: Observable<TaskListResponse> = new Observable<TaskListResponse>();

  displayedColumns: string[] = ['description', 'status', 'dateFormatted', 'actions'];
  dataSource = new MatTableDataSource<TaskResponse>([]);

  // MdPaginator Output
  pageEvent: PageEvent = new PageEvent();
  taskTotal = 0;

  isLoading = false;


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  ngAfterViewInit() {
    this.onPaginateChange(this.pageEvent);
    this.dataSource.paginator = this.paginator;
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, { duration: 2 * 1000 });
  }

  openDeleteConfirm(task: TaskResponse): void {
    const dialogRef = this.dialog.open(TaskDeleteComponent, {
      data: { id: task.id, description: task.description },
    });

    dialogRef.afterClosed().subscribe(id => {
      console.log('The dialog was closed', id);
      if (id) {
        this.taskManagementService.Delete(id).subscribe((result: boolean) => {
          this.showSnackBar(result, 'Tarefa removida com sucesso');
        });
      }
    });
  }

  openForm(task?: TaskResponse): void {
    const dialogRef = this.dialog.open(TaskFormComponent, {
      data: { id: task?.id ?? '', description: task?.description ?? '', status: task?.status ?? '', date: task?.date ?? new Date(), createdAt: task?.createdAt ?? new Date() } as FormDialogData,
    });

    dialogRef.afterClosed().subscribe((data: FormDialogData) => {
      if (data)
      {
        if (data?.id && data?.id?.length > 0) {
          this.taskManagementService.Update(data).subscribe((result: boolean) => {
            this.showSnackBar(result, 'Tarefa atualizada com sucesso');
          });
        } else
        {
          this.taskManagementService.Create(data).subscribe((result: boolean) => {
            this.showSnackBar(result, 'Tarefa adicionada com sucesso');
          });
        }
        console.log('The dialog was closed', data);
      }
    });
  }

  onPaginateChange(event: PageEvent): void
  {
    this.isLoading = true;
    this.dataSource = new MatTableDataSource<TaskResponse>([]);

    this.taskManagementService.List(((event?.pageIndex??0) + 1), event?.pageSize??10).subscribe((taskRes: TaskListResponse) => {
      taskRes.content.forEach((a) => { a.dateFormatted = formatDate(a.date, 'dd/MM/YYYY', 'en-US') });
      this.dataSource = new MatTableDataSource<TaskResponse>(taskRes.content);
      this.taskTotal = taskRes.total;
      this.isLoading = false;
    });
  }

  showSnackBar(result: boolean, positiveMessage: string, negativeMessage: string = 'Ocorreu um erro, tente novamente')
  {
    if (result) {
      this.openSnackBar(positiveMessage, 'Fechar');
      this.isLoading = true;
      this.dataSource = new MatTableDataSource<TaskResponse>([]);
      setTimeout(() => {
        this.onPaginateChange(this.pageEvent);
      }, 1500);
      
    }
    else
      this.openSnackBar(negativeMessage, 'Fechar');
  }
}
