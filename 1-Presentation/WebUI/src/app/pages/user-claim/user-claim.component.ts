import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  isPopupUpdateClaimDialog: boolean = false;

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
      private router: Router,
      private userClaimService: UserClaimService,
      private statusMessageService: StatusMessageService) {
    this.UserId = this.route.snapshot.queryParamMap.get("userid");
  }

  ngOnInit(): void {
    this.UserClaims$ = this.userClaimService.GetUserClaims(this.UserId!);      
    this.ClaimTypes$ = this.userClaimService.GetAllClaimTypes();

    this.UserClaims$.subscribe(claims => this.UserClaims = claims);
    this.ClaimTypes$.subscribe(types => this.ClaimTypes = types);
  }

  UpdateSelected(userClaim: UserClaim): void {
    this.UserClaimSelected = this.Clone(userClaim);
    this.ShowUpdateClaimDialog(true);
  }

  DeleteClaim(userClaim: UserClaim): void {
      this.userClaimService.DeleteUserClaim(this.UserId!, userClaim).subscribe({
      complete: () => {
        this.ngOnInit();
      },
      error: (errReponse) => {
        if (errReponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = new StatusMessageModel(errReponse.message, EnumInfoSeverity.Error);
        }
      },
      next: () => {
        this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully delete a Claim", EnumInfoSeverity.Info);
      }
    });
  }

  UpdateClaim(): void {
    this.userClaimService.UpdateUserClaim(this.UserId!, this.UserClaimSelected).subscribe({
      complete: () => {
        this.ShowUpdateClaimDialog(false);
        this.ngOnInit();
      },
      error: (errReponse) => {
        if (errReponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = new StatusMessageModel(errReponse.message, EnumInfoSeverity.Error);
        }
      },
      next: () => {
        this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully update a Claim", EnumInfoSeverity.Info);
      }
    });
  }

  AddClaim(): void {
    this.userClaimService.AddUserClaim(this.UserId!, this.NewUserClaim).subscribe({
      complete: () => {
        this.ShowAddClaimDialog(false);
        this.ngOnInit();
      },
      error: (errReponse) => {
        if (errReponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = new StatusMessageModel(errReponse.message, EnumInfoSeverity.Error);
        }
      },
      next: () => {
        this.statusMessageService.StatusMessage = new StatusMessageModel("Successfully add a Claim", EnumInfoSeverity.Info);
      }
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

  ShowUpdateClaimDialog(isShow: boolean) {
    this.isPopupUpdateClaimDialog = isShow;
    if (!isShow) {
      this.UserClaimSelected = {
        claimType: {
          typeShortName: '',
          typeName: ''
        },
        value: "",
      };
    }
  }

  private Clone(userClaim: UserClaim): UserClaim {
    return {
      claimType: {
        typeShortName: userClaim.claimType.typeShortName,
        typeName: userClaim.claimType.typeName
      },
      value: userClaim.value,
    };  
  }
}
