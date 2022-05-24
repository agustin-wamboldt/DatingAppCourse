import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter(); // Output property
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];
  
  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(30)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]], // Requires custom validator to check against password field
    })
  }
  
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.get(matchTo)!.value ? null : { isMatching: true };
      // Compares the control's value this validator has been attached to, to the control named with the value of matchTo
      // ie. compares confirmPassword value to, when setup, the password FormControl value.
    }
  }
  
  register() {
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/members');
    }, error => {
      this.validationErrors = error;
    })
  }
  
  cancel() {
    this.cancelRegister.emit(false);
  }
}
