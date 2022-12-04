
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatSignalRService } from 'src/app/_services/chatsignalr.service';
import { ChatService } from 'src/app/_services/chat.service';
import { firstValueFrom } from 'rxjs';
import { UserService } from 'src/app/_services/user.service';
import { ChatViewModel } from 'src/app/model/chatViewModel';
import { ChatInputModel } from 'src/app/model/chatInputModel';
import { RoomViewModel } from 'src/app/model/roomViewModel';
import { RoomService } from 'src/app/_services/room.service';


@Component({
    selector: 'app-chat',
    templateUrl: './chat.component.html',
    styleUrls: ['./chat.component.scss']
  })

export class ChatComponent {

    loading = false;
    chats: ChatViewModel[] = []; 
    rooms: RoomViewModel[] = []; 
    form: FormGroup;
    chatRoomId: string = "";    
    @ViewChild('chatBox') private chatBox: ElementRef;

    constructor(public userService: UserService,
                private route: ActivatedRoute,
                private router: Router,
                private formBuilder: FormBuilder,
                private chatSignalRService: ChatSignalRService,
                private chatService: ChatService,
                private roomService: RoomService) 
    {
        
        this.form = this.formBuilder.group({
            message: ['', Validators.required],
            chatRoomSelect: [1]
        });

        this.chatSignalRService.startConnection('chatHub');
        this.chatSignalRService.hubConnection.off("ChatMessage");

        this.chatSignalRService.hubConnection.on("ChatMessage", (chat: ChatViewModel) => {
            if (chat.roomId == this.chatRoomId)
            {
              this.chats.push(chat);
              this.scrollToBottom();
            }
        });
    }

    async ngOnInit() {
      await this.getRooms();
    }

    async getMessages()
    {
      try
      {  
        this.loading = true;        
        this.chats = await firstValueFrom(this.chatService.getMessages(this.chatRoomId));
        this.loading = false;        
        this.scrollToBottom();
      }
      catch(ex: any)
      {               
          alert(ex.error);
          this.loading = false;        
      }
    }

    home() {      
      this.router.navigate(['/'], { relativeTo: this.route });
    }

    sendMessage() {    
      let chat: ChatInputModel = {
        roomId: this.chatRoomId,
        message: this.form.controls.message.value.trimEnd()
      };

      if (chat.message == '')
      {
        this.form.controls.message.setValue('');
        return;
      }

      this.chatSignalRService.invokeSendMessage(chat);      
      this.form.controls.message.setValue('');
    }

    checkIfOwnMessage(chat: ChatViewModel) : boolean {      
      return (chat.userId == this.userService.currentUser.id);
    }

    getFormattedDate(dateString: string) : string {
      return new Date(dateString).toLocaleString("en-US");
    }

    changeChatRoom() {
      this.chatRoomId = this.form.controls.chatRoomSelect.value;
      this.getMessages();
    }

    scrollToBottom() {
      setTimeout(() => {
        this.chatBox.nativeElement.scrollTop = this.chatBox.nativeElement.scrollHeight;
      }, 100);
    }

    async getRooms() {
      try
      {  
        this.loading = true;        
        this.rooms = await firstValueFrom(this.roomService.getRooms());
        this.loading = false;        
        this.scrollToBottom();
      }
      catch(ex: any)
      {               
          alert(ex.error);
          this.loading = false;        
      }
    }
}