export type TaskListResponse = {
  page: number;
  total: number;
  content: TaskResponse[];
}

export type TaskResponse = {
  id: string;
  description: string;
  status: string;
  date: Date;
  dateFormatted: string;
  createdAt: Date;
  updatedAt: Date;
}
