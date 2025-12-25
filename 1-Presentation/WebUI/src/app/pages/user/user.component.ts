import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { UserService } from './user.service';
import { UserClaim } from './models/UserClaim';
import { ClaimTypeValues } from './models/ClaimTypeValues';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DividerModule } from 'primeng/divider';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-root',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
  imports: [FormsModule, ConfirmDialogModule, InputTextModule, FloatLabelModule, ButtonModule, DividerModule ]
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
      const closeBtn = document.getElementById("closebtn2");
      closeBtn?.click();
    });
  }

  private GetTypeShortName(typeName: string): string | undefined {
    return this.ClaimTypes?.find(type => type.typeName == typeName)?.typeShortName;
  } 
}
