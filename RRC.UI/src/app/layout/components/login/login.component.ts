import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { AuthService } from 'src/app/auth/auth.service';
import { CommonService } from 'src/app/common.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  animations: [
    trigger('showhide', [
      state('open', style({ transform: 'translateX(0px)' })),
      state('closed', style({ transform: 'translateX(-400px)' })),
      transition('*=>open', animate('500ms')),
      transition('*=>closed', animate('500ms'))
    ])
  ]
})
export class LoginNewComponent implements OnInit {
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
  loginStatement = "";
  authenticate$: Observable<any>;
  bigViewPortSubscriber: any;
  smallViewPortSubscriber: any;
  isShow: boolean = false;
  lang: any;

  formLabels: {
    logintitle: any
    loginusername: any
    loginpassword: any
    loginbottom: any
    loginuserplaceholder: any
    loginpassplaceholder: any
    loginsmartsystem:any
    loginStatement:any
    usernamepwderr:any
  }
  isLogged: boolean;

  constructor(public router: Router,
    public route: ActivatedRoute,
    public authService: AuthService,
    public common: CommonService,
  ) {
    this.lang = this.common.currentLang;
    this.formLabels = {
      logintitle: (this.lang == 'en') ? "LOGIN" : this.arabic('logintitle'),
      loginusername: (this.lang == 'en') ? "Username" : this.arabic('loginusername'),
      loginpassword: (this.lang == 'en') ? "Password" : this.arabic('loginpassword'),
      loginbottom: (this.lang == 'en') ? "LOGIN" : this.arabic('loginbottom'),
      loginuserplaceholder: (this.lang == 'en') ? "Enter the username" : this.arabic('loginuserplaceholder'),
      loginpassplaceholder: (this.lang == 'en') ? "Enter the password" : this.arabic('loginpassplaceholder'),
      loginsmartsystem:(this.lang == 'en') ? '':this.arabic('loginsmartsystem'),
      loginStatement:(this.lang == 'en') ? 'Ruler’s Representative Court – Al Dhafrah Region': this.arabic('loginStatement'),
      usernamepwderr:(this.lang == 'en') ? 'The username or password you entered is incorrect, please try again': this.arabic('usernamepwderr')     
    }
  }

  ngOnInit() {
    if(localStorage.getItem('token')) {
      if(!this.router.url.includes('login')){
        this.authService.returnURL = this.router.url;
      }else{
        this.authService.returnURL = '/'+this.common.currentLang+'/home';
      }
      let returnURL = this.authService.returnURL;
      if (returnURL != '') {
        this.router.navigate([returnURL]);
      } else {
        this.router.navigate(['/ar/home']);
      }
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngAfterViewInit() {
    setInterval(() => {
      if (window.screen.width < 768) {
        this.isShow = false;
      } else {
        this.isShow = true;
      }
    }, 50)
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
      if(res.IsWrdUser){
        this.errormsg = true;
      }else{
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
      }
    },(err: any) => {
        this.errormsg = true;
        return;
      });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

}
