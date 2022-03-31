import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ // 1. services are injectable (DI)
  providedIn: 'root' // 2. root = singleton, won't be close until the service/app is closed
})  // 3. components are different, they are destroyed as soon as they are not in use
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  
  
  constructor(private http: HttpClient) { }
  
  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model);
  }
}
