import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TokenProvider } from 'src/app/shared/providers/token-provider';
import { IdentityService } from 'src/app/shared/services/identity-service';
import { LoginModel } from 'src/clients/client.generated';

@Component({
  selector: 'chat-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
})
export class LoginComponent implements OnInit {
  public loginForm = this.fb.group({
    email: [null, Validators.email],
    password: [null, Validators.required]
  });

  private get loginModel(): LoginModel {
    return {
      email: this.loginForm.get('email').value,
      password: this.loginForm.get('password').value
    };
  }

  constructor(
    private fb: FormBuilder,
    private readonly router: Router,
    private readonly identityService: IdentityService,
    private readonly route: ActivatedRoute
  ) {}

  public ngOnInit() {
  }

  public onLogin(): void {
    this.identityService
      .loginUser(this.loginModel)
      .subscribe(
        result => {
          if (result) {
            this.navigateToChatRoom();
          }
          console.log('error on login');
        }
      )
    ;
  }

  public navToRegister(): void {
    this.router.navigate(['/identity/register']);
  }

  private navigateToChatRoom(): void {
    this.router.navigate(['/chat-rooms']);
  }

}
