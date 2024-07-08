import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from './user.service';
import { UserClaim } from './Models/UserClaim';
import { ClaimTypeValues } from './Models/ClaimTypeValues';

@Component({
  selector: 'app-root',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit{
  title = 'User Profile';
  UserId: string | null;
  UserClaims: UserClaim[] | undefined;

  UserClaimSelected: UserClaim = {
    shortTypeName: "",
    value: "",
    typeName: ""
  };

  NewUserClaim: UserClaim = {
    shortTypeName: "",
    value: "",
    typeName: ""
  };

  ClaimTypes: ClaimTypeValues[] | undefined;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    this.UserId = route.snapshot.queryParamMap.get("userid");
  }

  ngOnInit(): void {
    this.userService.GetUserClaims(this.UserId!).subscribe(claims => this.UserClaims = claims);
    this.userService.GetAllClaimTypes().subscribe(types => this.ClaimTypes = types);
  }

  UpdateSelected(userClaim: UserClaim): void {
    this.UserClaimSelected = userClaim;
  }

  UpdateClaim(): void {
    this.userService.UpdateUserClaim(this.UserId!, this.UserClaimSelected).subscribe(() => {
      const closeBtn = document.getElementById("closebtn");
      closeBtn?.click();
    });
  }

  AddClaim(): void {
    this.NewUserClaim.shortTypeName = this.GetTypeShortName(this.NewUserClaim.typeName)!;
    this.userService.AddUserClaim(this.UserId!, this.NewUserClaim).subscribe(() => {

    });
  }

  private GetTypeShortName(typeName: string): string | undefined {
    return this.ClaimTypes?.find(type => type.typeName == typeName)?.typeShortName;
  } 
}
