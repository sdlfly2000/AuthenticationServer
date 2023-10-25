import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit{
  title = 'User Profile';

  constructor() {

  }

  ngOnInit(): void {

  }

  OnSubmit(form: NgForm): void {

  }

}
