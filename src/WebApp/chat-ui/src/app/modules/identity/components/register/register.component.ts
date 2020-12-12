import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IdentityService } from 'src/app/shared/services/identity-service';
import { RegisterModel } from 'src/clients/client.generated';

@Component({
  selector: 'chat-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RegisterComponent implements OnInit {
  public registerForm = this.fb.group({
    email: [null, [ Validators.required, Validators.email ]],
    password: [null, Validators.required],
    confirmPassword: [null, Validators.required],
    username: [null, Validators.required],
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    dateOfBirth: [null, Validators.required],
    details: [null],
    phoneNumber: [null],
  });

  private get registerModel(): RegisterModel {
    return this.registerForm.getRawValue();
  }

  constructor(
    private fb: FormBuilder,
    private readonly router: Router,
    private readonly identityService: IdentityService
  ) {}

  public ngOnInit() {

  }

  public navigateToLogin(): void {
    this.router.navigate(['/identity/login'])
  }

  public setDateOfBirth(date: Date) {
    this
      .registerForm
      .get('dateOfBirth')
      .setValue(date)
    ;
  }

  public onRegister(): void {
    this.identityService
      .registerUser(this.registerModel)
      .subscribe(
        result => {
          this.navigateToLogin();
        }
      )
    ;
  }
}
