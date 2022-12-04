import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { HubConnectionState, HttpClient, HttpRequest, HttpResponse } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { UserService } from "./user.service";
import { ChatInputModel } from "../model/chatInputModel";


@Injectable({
  providedIn: "root"
})
export class ChatSignalRService {

  public hubConnection: signalR.HubConnection;
  apiUrl = environment.apiUrl;

  constructor(private userService: UserService) {
    
  }

  reconnect() {
    if (this.hubConnection == null || this.hubConnection.state == HubConnectionState.Disconnected) {
      setTimeout(() => {
        this.hubConnection
          .start()
          .catch(err => {
            console.log("Error while starting reconnection: " + err);
            this.reconnect();
          });
      }, 5000);
    }
  }

  startConnection(hubName: string) {
    if (this.hubConnection == null || this.hubConnection.state == HubConnectionState.Disconnected) {
      Object.defineProperty(WebSocket, 'OPEN', { value: 1, });
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(this.apiUrl + hubName,
          {
            accessTokenFactory: () => this.userService.getToken()
          })
        .withAutomaticReconnect([0, 2000, 3000, 4000, 5000, 6000, 8000, 10000, 12000, 15000, 18000, 20000, 22000, null])
        .build();

      this.hubConnection
        .start()
        .then()
        .catch(err => {
          console.log("Error while starting connection: " + err);
          this.reconnect();
        });
    }

    this.hubConnection.onclose((e) => {
      console.log('Disconnected', e);
    });
  };

  stopConnection() {
    if (this.hubConnection != null && this.hubConnection.state != HubConnectionState.Disconnected) {
      this.hubConnection.stop()
        .catch(err => {
          console.log("Error while starting connection: " + err);
          this.reconnect();
        });
    }
  }

  invokeSendMessage(chat: ChatInputModel)
  {
    this.hubConnection.invoke('SendMessage', chat).catch(err => console.log(err));
  }
}