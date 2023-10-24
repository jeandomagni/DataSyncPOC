import { Component, OnInit } from '@angular/core';
import { TodoModel } from './todo.model';
import { TodoService } from './todo.service';
import { Todo2Service } from './todo2.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  todo: TodoModel[] = [];
  todo2: TodoModel[] = [];

  constructor(private todoService: TodoService, private todo2Service: Todo2Service) { }

  ngOnInit(): void {
    this.getTodo();
    this.getTodo2();
  }

  getTodo(): void {
    this.todoService.getTodo()
      .subscribe(todo => this.todo = todo);
  }
  getTodo2(): void {
    this.todo2Service.getTodo()
      .subscribe(todo2 => this.todo2 = todo2);
  }
}