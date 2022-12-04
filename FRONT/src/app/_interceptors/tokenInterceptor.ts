import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { UserService } from "../_services/user.service";

@Injectable()
export class HttpConfigInterceptor implements HttpInterceptor {
    constructor(
        private router: Router,
        private userService: UserService
    ) {

    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ` + this.userService.getToken(),
            }
        });

        return next.handle(request);
    }
}