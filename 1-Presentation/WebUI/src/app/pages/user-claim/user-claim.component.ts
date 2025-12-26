import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { StatusMessageService, EnumInfoSeverity, StatusMessageModel } from '../../../services/statusmessage.service';

@Component({
  selector: 'app-root',
  templateUrl: './user-claim.component.html',
  styleUrls: ['./user-claim.component.css'],
  imports: [FormsModule, ConfirmDialogModule, InputTextModule, FloatLabelModule, ButtonModule, DividerModule, Dialog, Select, AsyncPipe ]
})
export class UserClaimComponent implements OnInit{
  title = 'User Claims';
  UserId: string | null;
  UserClaims: UserClaim[] | undefined;
  ClaimTypes: ClaimTypeValues[] | undefined;

  UserClaims$: Observable<UserClaim[]> | undefined;
  ClaimTypes$: Observable<ClaimTypeValues[]> | undefined;

  isPopupAddClaimDialog: boolean = false;

  UserClaimSelected: UserClaim = {
    claimType: {
        typeShortName: '',
        typeName: ''
    },
    value: ''
  };

  NewUserClaim: UserClaim = {
    claimType: {
        typeShortName: '',
        typeName: ''
    },
    value: "",
  };  

  constructor(
      private route: ActivatedRoute,
      private userClaimService: UserClaimService,
      private statusMessageService: StatusMessageService) {
    this.UserId = route.snapshot.queryParamMap.get("userid");
  }

  ngOnInit(): void {
    this.UserClaims$ = this.userClaimService.GetUserClaims(this.UserId!);      
    this.ClaimTypes$ = this.userClaimService.GetAllClaimTypes();

    this.UserClaims$.subscribe(claims => this.UserClaims = claims);
    this.ClaimTypes$.subscribe(types => this.ClaimTypes = types);
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
    this.userClaimService.AddUserClaim(this.UserId!, this.NewUserClaim).subscribe({
      complete: () => {
        this.ShowAddClaimDialog(false);
      },
      error: (errReponse) => {
        if (errReponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = new StatusMessageModel(errReponse.message, EnumInfoSeverity.Error);
        }
      },
      next: () => {
        this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully add a Claim", EnumInfoSeverity.Info);
      },
    });
  }

  ShowAddClaimDialog(isShow: boolean){
    this.isPopupAddClaimDialog = isShow;
    if (!isShow) {
      this.NewUserClaim = {
        claimType: {
          typeShortName: '',
          typeName: ''
        },
        value: "",
      };
    }
  }

  private GetTypeShortName(typeName: string): string | undefined {
    return this.ClaimTypes?.find(type => type.typeName == typeName)?.typeShortName;
  } 
}
