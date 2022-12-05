import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { firstValueFrom } from 'rxjs';
import { UserService } from 'src/app/_services/user.service';
import { UserInputModel } from 'src/app/model/userInputModel';


@Component({ templateUrl: 'register.component.html' })
export class RegisterComponent implements OnInit {
    form: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            username: ['', [Validators.required, Validators.minLength(5)]],
            password: ['', [Validators.required, Validators.minLength(6)]]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    async onSubmit() {
        this.loading = true;
        this.submitted = true;

        if (this.form.invalid) {
            this.loading = false;
            return;
        }

        let userInputModel: UserInputModel = {
            email: this.f.email.value,
            username: this.f.username.value,
            password: this.f.password.value
        }

        try {
            let data = await firstValueFrom(this.userService.register(userInputModel));
            alert("user saved");
            this.router.navigate(['/login'], { relativeTo: this.route });
        }
        catch (ex: any) {
            alert(ex.error);
            this.loading = false;
        }
    }
}