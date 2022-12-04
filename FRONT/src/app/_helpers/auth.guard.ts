import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { UserService } from "../_services/user.service";

@Injectable({ providedIn: 'root' }) 
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const user = this.userService.currentUser;
        if (user.id != '') {
            return true;
        }   

        this.router.navigate(['/account/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}