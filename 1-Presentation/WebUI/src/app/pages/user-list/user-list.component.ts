import { Component, OnInit, signal, Signal, WritableSignal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TableModule } from 'primeng/table';
import { DividerModule } from 'primeng/divider';
import { InputTextModule } from 'primeng/inputtext';
import { InputIconModule } from 'primeng/inputicon';
import { IconFieldModule } from 'primeng/iconfield';
import { Observable } from 'rxjs';
import { StatusMessageService, EnumInfoSeverity, StatusMessageModel } from '../../../services/statusmessage.service';
import { UserModel } from './models/UserModel';
import { UserListService } from './user-list.service';
import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
    imports: [TableModule, InputIconModule, IconFieldModule, ConfirmDialogModule, InputTextModule, ButtonModule, DividerModule, AsyncPipe ]
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
