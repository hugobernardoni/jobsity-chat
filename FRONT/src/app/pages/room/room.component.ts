import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { firstValueFrom } from 'rxjs';
import { UserService } from 'src/app/_services/user.service';
import { UserInputModel } from 'src/app/model/userInputModel';
import { RoomService } from 'src/app/_services/room.service';
import { RoomInputModel } from 'src/app/model/roomInputModel';


@Component({ templateUrl: 'room.component.html' })
export class RoomComponent implements OnInit {
    form: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private roomService: RoomService
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            name: ['', [Validators.required]]            
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    async onSubmit() {
        this.submitted = true;

        if (this.form.invalid) {
            return;
        }

        let roomInputModel: RoomInputModel = {
            name: this.f.name.value            
        }

        try {
            let data = await firstValueFrom(this.roomService.create(roomInputModel));
            alert("room saved");
            this.router.navigate(['/'], { relativeTo: this.route });
        }
        catch (ex: any) {
            alert(ex.error);
            this.loading = false;
        }
    }
}