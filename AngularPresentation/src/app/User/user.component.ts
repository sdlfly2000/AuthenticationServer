import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { UserService } from './user.service';
import { UserClaim } from './Models/UserClaim';

@Component({
  selector: 'app-root',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit{
  title = 'User Profile';
  UserId: string | null;
  UserClaims: UserClaim[] | undefined;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    this.UserId = route.snapshot.queryParamMap.get("userid");
  }

  ngOnInit(): void {
    this.userService.GetUserClaims(this.UserId!).subscribe(claims => this.UserClaims = claims);
  }

  OnSubmit(form: NgForm): void {

  }

}
