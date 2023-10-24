import { Injectable } from '@angular/core';
import { HttpClient } from  '@angular/common/http';
import { Observable, of } from 'rxjs';

import { TodoModel } from './todo.model';

@Injectable({ providedIn: 'root' })
export class Todo2Service {
  private getTodoUrl = 'https://localhost:7046/todo2';

  constructor(private http: HttpClient) { 

  }

  getTodo(): Observable<TodoModel[]> {
    return this.http.get<TodoModel[]>(this.getTodoUrl);
  }

}