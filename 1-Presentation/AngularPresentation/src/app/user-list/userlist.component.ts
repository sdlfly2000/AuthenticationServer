import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserListComponent implements OnInit{
  title = 'User List';

  constructor() {
  }

  ngOnInit(): void {
  }
}
