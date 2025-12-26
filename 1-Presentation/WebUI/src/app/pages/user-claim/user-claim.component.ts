import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { UserClaimService } from './user-claim.service';
import { UserClaim } from './models/UserClaim';
import { ClaimTypeValues } from './models/ClaimTypeValues';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DividerModule } from 'primeng/divider';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { Dialog } from 'primeng/dialog';
import { Select } from 'primeng/select';

@Component({
  selector: 'app-root',
  templateUrl: './user-claim.component.html',
  styleUrls: ['./user-claim.component.css'],
  imports: [FormsModule, ConfirmDialogModule, InputTextModule, FloatLabelModule, ButtonModule, DividerModule, Dialog, Select ]
})
export class UserClaimComponent implements OnInit{
  title = 'User Claims';
  UserId: string | null;
  UserClaims: UserClaim[] | undefined;

  isPopupAddClaimDialog: boolean = false;

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

  constructor(private route: ActivatedRoute, private userClaimService: UserClaimService) {
    this.UserId = route.snapshot.queryParamMap.get("userid");
  }

  ngOnInit(): void {
    this.userClaimService.GetUserClaims(this.UserId!).subscribe(claims => this.UserClaims = claims);
    this.userClaimService.GetAllClaimTypes().subscribe(types => this.ClaimTypes = types);
  }

  UpdateSelected(userClaim: UserClaim): void {
    this.UserClaimSelected = userClaim;
  }

  UpdateClaim(): void {
    this.userClaimService.UpdateUserClaim(this.UserId!, this.UserClaimSelected).subscribe(() => {
      const closeBtn = document.getElementById("closebtn");
      closeBtn?.click();
    });
  }

  AddClaim(): void {
    this.NewUserClaim.shortTypeName = this.GetTypeShortName(this.NewUserClaim.typeName)!;
    this.userClaimService.AddUserClaim(this.UserId!, this.NewUserClaim).subscribe(() => {
      const closeBtn = document.getElementById("closebtn2");
      closeBtn?.click();
    });
  }

  ShowAddClaimDialog(isShow: boolean){
    this.isPopupAddClaimDialog = isShow;
  }

  private GetTypeShortName(typeName: string): string | undefined {
    return this.ClaimTypes?.find(type => type.typeName == typeName)?.typeShortName;
  } 
}
