import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyResponse } from '../models/myresponse.model';
import { Person } from '../models/person.model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
personApi: string;
  constructor(
    private http: HttpClient
  ) {
    this.personApi = 'api/MySample/'
   }

   getMyName(): Observable<MyResponse>{
     return this.http.get<MyResponse>(this.personApi + 'Myname');
   }
   getYourName(name: string): Observable<MyResponse>{
    return this.http.get<MyResponse>(this.personApi + 'YourName/' + name);
  }
  checkLegalAge(person: Person): Observable<MyResponse>{
    return this.http.post<MyResponse>(this.personApi + 'LegalAge', person)
  }

 
}
