import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Dialog } from 'primeng/dialog';
import { DividerModule } from 'primeng/divider';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { UserModel } from './models/UserModel';
import { UserListService } from './user-list.service';

@Component({
  selector: 'app-root',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
    imports: [FormsModule, TableModule, InputIconModule, IconFieldModule, ConfirmDialogModule, InputTextModule, ButtonModule, DividerModule, ToolbarModule, Dialog]
})
export class UserListComponent implements OnInit{
    title = 'User List';
    IsLoading: boolean = true;
    isPopupAssignAppDialog: boolean = false;

    assignAppName: string = '';

    Users: WritableSignal<UserModel[]> = signal<UserModel[]>([]);
    SelectedUsers: UserModel[] = [];

    constructor(
        private userListService: UserListService,
        private statusMessageService: StatusMessageService) {
    }

    ngOnInit(): void {
        this.userListService.GetAllUsers().subscribe({
            next: (users) => {
                this.Users.set(users);
                this.IsLoading = false;
            },
            error: (errorResponse) => {
                if (errorResponse instanceof HttpErrorResponse) {
                    this.statusMessageService.StatusMessage = new StatusMessageModel(errorResponse.message, EnumInfoSeverity.Error);
                }
            }
        });
    }

    AssignApp(): void {

        this.SelectedUsers.forEach(user => {
            this.userListService.AssignApp(user.id.code, this.assignAppName).subscribe({
                error: (errorResponse) => {
                    if (errorResponse instanceof HttpErrorResponse) {
                        this.statusMessageService.StatusMessage = new StatusMessageModel(errorResponse.error.detail, EnumInfoSeverity.Error);
                    }
                }
            });
        });

        this.ShowAssignAppDialog(false);
    }


    ShowAssignAppDialog(isShow: boolean) {
        this.isPopupAssignAppDialog = isShow;
        if (!isShow) {
            this.assignAppName = '';
        }
    }
}
