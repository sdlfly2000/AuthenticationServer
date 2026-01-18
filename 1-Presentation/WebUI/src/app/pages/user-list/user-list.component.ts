import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DividerModule } from 'primeng/divider';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { EnumInfoSeverity, StatusMessageModel, StatusMessageService } from '../../../services/statusmessage.service';
import { UserModel } from './models/UserModel';
import { UserListService } from './user-list.service';

@Component({
  selector: 'app-root',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
    imports: [TableModule, InputIconModule, IconFieldModule, ConfirmDialogModule, InputTextModule, ButtonModule, DividerModule ]
})
export class UserListComponent implements OnInit{
    title = 'User List';
    IsLoading: boolean = true;

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
}
