import {Component, OnInit} from '@angular/core';
import {UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import { HttpServiceService } from 'src/app/Services/http/http-service.service';
import {MessageService} from "primeng/api";
import {UserInfoModel} from "../../../../Models/UserInfoModel";
import {LocationServiceService} from "../../../../Services/helpers/location-service.service";
import {SessionStorageServiceService} from "../../../../Services/data/session-storage-service.service";
import {Title} from "@angular/platform-browser";
import {CacheServiceService} from "../../../../Services/data/cache-service.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  form!: UntypedFormGroup;
  formDisabled: boolean = false;

  constructor(
    private sessionStorageService: SessionStorageServiceService,
    private cacheService: CacheServiceService,
    private locationService: LocationServiceService,
    private httpService: HttpServiceService,
    private messageService: MessageService,
    private title: Title
  ) {
    this.title.setTitle(`PicturesUI - Logowanie`);
  }

  ngOnInit(): void {
    this.form = new UntypedFormGroup({
      nickname: new UntypedFormControl(null, Validators.required),
      password: new UntypedFormControl(null, Validators.required),
    });
  }

  onSubmit(): void {
    this.messageService.clear();
    this.disableForm();
    this.httpService
      .postLoginRequest(this.form.getRawValue())
      .subscribe({
        next: (v: UserInfoModel) => {
          if (v) {
            this.cacheService.cacheUserInfo(v);
            this.messageService.add({severity:'success', summary: 'Sukces', detail: 'Zalogowano pomyślnie.'});
            this.cacheService.loggedOnSubject.next(true);
            this.cacheService.updateUserAccount();
            this.locationService.goBack();
          }
        },
        error: (err) => {
          if (err.error === "Invalid nickname or password") {
            this.messageService.add({severity:'error', summary: 'Niepowodzenie', detail: 'Podano błędne dane logowania.', key: "login-failed"});
            this.enableForm();
          }
        }
      })
  }

  private disableForm() {
    this.form.disable();
    this.formDisabled = true;
  }
  private enableForm() {
    this.form.enable();
    this.formDisabled = false;
  }

}
