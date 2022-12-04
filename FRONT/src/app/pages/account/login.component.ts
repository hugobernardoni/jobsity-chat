import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { firstValueFrom } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { UserService } from 'src/app/_services/user.service';
import { UserLoginInputModel } from 'src/app/model/userLoginInputModel';


@Component({
    templateUrl: 'login.component.html'
})
export class LoginComponent implements OnInit {
    form: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });

        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    get f() { return this.form.controls; }

    async onSubmit() {
        this.submitted = true;

        if (this.form.invalid) {
            return;
        }

        let userLogin: UserLoginInputModel = {
            username: this.f.username.value,
            password: this.f.password.value
        }


        this.loading = true;
        try {
            let data = await firstValueFrom(this.userService.login(userLogin));
            this.router.navigate(['/']);
        }
        catch (ex: any) {
            this.loading = false;
        }
    }
}