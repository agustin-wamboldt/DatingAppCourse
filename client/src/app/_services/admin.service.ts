import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Photo } from '../_models/photo';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) { }
  
  getUsersWithRoles() {
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles');
  }
  
  updateUserRoles(username: string, roles: string[]) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }
  
  getPhotosForApproval() {
    return this.http.get<Photo[]>(this.baseUrl + "admin/photos-for-approval/");
  }
  
  approvePhoto(photoId: number) { // Easier appending to string but will leave as is for learning purposes
    let params = new HttpParams().append('photoId', photoId);
    return this.http.post(this.baseUrl + "admin/approve-photo", {}, { params: params });
  }
  
  rejectPhoto(photoId: number) { // Easier appending to string but will leave as is for learning purposes
    return this.http.post(this.baseUrl + "admin/reject-photo", {}, {
      params: {
        photoId: photoId
      }
    });
  }
}
