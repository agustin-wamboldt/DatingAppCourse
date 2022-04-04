import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;
  
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    // const headers = new HttpHeaders()
    //   .set('content-type', 'application/json')
    //   .set('Authorization', 'Bearer ' + );
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }
  
  getUsers() {
    this.http.get('https://localhost:5001/api/users', ).subscribe(users => this.users = users);
  }
}
