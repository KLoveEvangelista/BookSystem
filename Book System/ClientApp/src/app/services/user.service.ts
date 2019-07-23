import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyResponse } from '../models/myresponse.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  userApi: string;

  constructor(
    private http:HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this.userApi = baseUrl + 'api/User/';
   }

   register(user: User) : Observable<MyResponse> {
     return this.http.post<MyResponse>(this.userApi + 'Register', user);
   }
}
