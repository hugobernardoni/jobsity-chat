import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { ChatViewModel } from '../model/chatViewModel';

@Injectable({
    providedIn: 'root'
  })
  export class ChatService {
    baseUrl = environment.apiUrl + 'chat';
  
    constructor(private http: HttpClient) {
  
    }
    
    getMessages(chatRoomId: string) {
        return this.http.get<ChatViewModel[]>(`${this.baseUrl}?roomId=${chatRoomId}`)
    }
  }
  
  