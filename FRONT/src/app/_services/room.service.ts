import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { UserInputModel } from '../model/userInputModel';
import { RoomInputModel } from '../model/roomInputModel';
import { RoomViewModel } from '../model/roomViewModel';

@Injectable({
    providedIn: 'root'
  })
  export class RoomService {
    baseUrl = environment.apiUrl + 'room/';    
  
    constructor(private http: HttpClient) {
  
    }    

    create(userInput: RoomInputModel) {
      return this.http.post(this.baseUrl + 'create', userInput)
    }   
    
    getRooms() {
        return this.http.get<RoomViewModel[]>(`${this.baseUrl}`)
    }
    
  }
  
  