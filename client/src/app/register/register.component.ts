import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() usersFromHomeComponent: any; // Input property
  @Output() cancelRegister = new EventEmitter(); // Output property
  model: any = {};
  
  constructor() { }

  ngOnInit(): void { 
  }

  register() {
    console.log(this.model);
  }
  
  cancel() {
    this.cancelRegister.emit(false);
  }
}
