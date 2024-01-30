import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskDeleteComponent } from './task-delete.component';

describe('TaskDeleteComponent', () => {
  let component: TaskDeleteComponent;
  let fixture: ComponentFixture<TaskDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskDeleteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TaskDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
