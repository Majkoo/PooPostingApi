import {Component, OnDestroy, OnInit} from '@angular/core';
import {UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import { CustomValidators } from 'src/CustomValidators';
import {MessageService} from "primeng/api";
import {Router} from "@angular/router";
import {BlockSpace} from "../../../shared/utils/regexes/blockSpace";
import {BlockSpaceOnStartEnd} from "../../../shared/utils/regexes/blockSpaceOnStartEnd";
import {Title} from "@angular/platform-browser";
import {environment} from "../../../../environments/environment";
import {AccountAuthService} from "../../../shared/data-access/account/account-auth.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
  form!: UntypedFormGroup;
  blockSpace: RegExp = BlockSpace;
  isName: RegExp = BlockSpaceOnStartEnd;
  formDisabled: boolean = false;

  siteKey!: string;
  captchaPassed: boolean = false;
  isNickNameTaken: boolean = false;

  private readonly subs = new Subscription();

  passCaptcha() {
    this.captchaPassed = true;
  }

  constructor(
    private authService: AccountAuthService,
    private message: MessageService,
    private router: Router,
    private title: Title,
  ) {
    this.siteKey = environment.captchaKey;
    this.title.setTitle(`PicturesUI - Rejestracja`);
  }

  ngOnInit(): void {
    this.isNickNameTaken = false;
    this.form = new UntypedFormGroup({
      nickname: new UntypedFormControl(null, [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(16),
      ]),
      email: new UntypedFormControl(null, [
        Validators.required,
        Validators.minLength(10),
        Validators.email
      ]),
      password: new UntypedFormControl(null, [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(64),
      ]),
      confirmPassword: new UntypedFormControl(null, [
        Validators.required
      ])},
      //@ts-ignore
      CustomValidators.mustMatch('password', 'confirmPassword'),
    )
  }

  onSubmit(): void {
    this.disableForm();
    this.isNickNameTaken = false;
    this.message.clear();

    this.subs.add(
      this.authService.register(this.form.getRawValue())
        .subscribe({
          next: () => {
            this.router.navigate(["/auth/login"]);
            this.message.add({severity:'success', summary: 'Sukces', detail: 'Zarejestrowano pomyślnie. Przeniesiono cię na stronę logowania.'});
          },
          error: (err) => {
            if (err.error.errors && err.error.errors.Nickname) {
              this.message.add({
                severity:'error',
                summary: 'Niepowodzenie',
                detail: err.error.errors.Nickname
              });
              this.isNickNameTaken = true;
            }
            this.enableForm();
          }
        })
    );
  }

  private disableForm() {
    this.form.disable();
    this.formDisabled = true;
  }
  private enableForm() {
    this.form.enable();
    this.formDisabled = false;
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
