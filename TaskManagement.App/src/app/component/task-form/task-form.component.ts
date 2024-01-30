import { Component, Inject } from '@angular/core';
import {
  MatDialog,
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { FormControl, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { provideNativeDateAdapter } from '@angular/material/core';

import { FormDialogData } from '../../../shared/task-interfaces';

@Component({
  selector: 'app-task-form',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [
    MatButtonModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatCardModule,
    MatDatepickerModule
  ],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.css'
})
export class TaskFormComponent {
  constructor(
    public dialogRef: MatDialogRef<TaskFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: FormDialogData,
  ) { }

  title = this.data?.id && this.data?.id?.length > 0 ? 'Editar' : 'Registrar';
  validate = false;

  statusList: {value:string, label:string }[] = [
    { value: 'Backlog', label: 'Backlog' },
    { value: 'Doing', label: 'Doing' },
    { value: 'Done', label: 'Done' },
  ];

  description = new FormControl(this.data.description, [Validators.required]);
  status = new FormControl(this.data.status, [Validators.required]);
  date: Date = this.data.date;

  CancelClick(): void {
    this.dialogRef.close();
  }

  getErrorMessage() {
    if (this.description.hasError('required')) {
      return 'Campo deve ser preenchido';
    }
    if (this.status.hasError('required')) {
      return 'Um valor deve ser selecionado';
    }

    this.data.description = this.description.value!;
    this.data.status = this.status.value!;
    this.data.date = this.date;

    this.validate = true;
    return '';
  }
}
