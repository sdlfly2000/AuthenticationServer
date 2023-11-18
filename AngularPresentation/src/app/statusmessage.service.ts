import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class StatusMessageService {

  private statusMessage: Subject<string>;

  constructor() {
    this.statusMessage = new Subject<string>();
  }

  set StatusMessage(value: string) {
    this.statusMessage.next(value);
  }

  get StatusMessage(): Observable<string> {
    return this.statusMessage;
  }
}
