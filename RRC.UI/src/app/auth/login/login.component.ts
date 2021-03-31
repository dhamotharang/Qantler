import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../auth.service';
import { Observable } from 'rxjs';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    animations: [
        trigger('showhide', [
            state('open', style({ transform: 'translateX(0px)' })),
            state('closed', style({ transform: 'translateX(-100px)' })),
            transition('*=>open', animate('500ms')),
            transition('*=>closed', animate('500ms'))
        ])
    ]
})
export class LoginComponent implements OnInit {
    logindet: any = {};
    errormsg: boolean = false;
    user: any = {
        username: '',
        password: '',
        department: '',
        departmentID: '',
        OrgUnitID: '',
        id: '',
        IsOrgHead: '',
        DisplayName: "TestUser1",
        IsAdmin: '',
        Password: '',
        Token: '',
        UnitID: '',
        UnitName: "",
        UserID: '',
        Username: "",
    };
    loginStatement = "Lorem ipsum dolor sit amet, consectetur adipicing elit, Nullam actor";
    authenticate$: Observable<any>;
    bigViewPortSubscriber: any;
    smallViewPortSubscriber: any;
    isShow: boolean;

    constructor(public router: Router,
        public route: ActivatedRoute,
        public authService: AuthService
    ) {

    }

    ngOnInit() {
        setInterval(() => {
            if (window.screen.width < 775) {
                this.isShow = false;
            } else {
                this.isShow = true;
            }
        }, 100)
    }

    async login() {
        let that = this;
        if (!this.logindet.username && this.logindet.username == null) {
            this.errormsg = true;
            return;
        }
        if (!this.logindet.password && this.logindet.password == null) {
            this.errormsg = true;
            return;
        }
        var param = {
            Username: this.logindet.username,
            Password: this.logindet.password
        }
        await this.authService.login(param).subscribe((res: any) => {
            this.user = res;
            this.user.username = res.DisplayName;
            this.user.password = res.Password;
            this.user.department = res.UnitName;
            this.user.OrgUnitID = res.UnitID;
            this.user.departmentID = res.UnitID;
            this.user.id = (res.UserID) ? res.UserID : 1;
            this.user.IsOrgHead = res.HOU;
            this.authService.setUser(this.user);
            // debugger;
            var returnURL = this.authService.returnURL;
            if (returnURL != '') {
                this.router.navigate([returnURL]);
            } else {
                this.router.navigate(['/ar/home']);
            }
        },
            (err: any) => {
                this.errormsg = true;
                return;
            });
    }

}
