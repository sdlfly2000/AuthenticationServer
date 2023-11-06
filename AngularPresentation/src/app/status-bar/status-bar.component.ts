import { Component } from '@angular/core';
import { StatusMessageService } from '../statusmessage.service';

@Component({
  selector: 'app-status-bar',
  templateUrl: './status-bar.component.html',
  styleUrls: ['./status-bar.component.css']
})
export class StatusBarComponent {

  public loginMessage: string = "";
  
  constructor(private statusMessageService: StatusMessageService) {
    this.statusMessageService.StatusMessage.subscribe(message => this.loginMessage = message);
  }


}
