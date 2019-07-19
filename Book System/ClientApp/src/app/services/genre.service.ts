import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Genre } from '../models/genre.model';
import { MyResponse } from '../models/myresponse.model';


@Injectable({
  providedIn: 'root'
})
export class GenreService {
    genreApi: string;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) { 
    this.genreApi= baseUrl + 'api/Genre/'
  }

  getAll(): Observable<Genre[]>{
    return this.http.get<Genre[]>(this.genreApi + 'GetAll');
  }

  getSingleBy(id): Observable<Genre>{
    return this.http.get<Genre>(this.genreApi + 'GetSingleBy/' + String(id));
  }

  create(genre:Genre): Observable<MyResponse>{
    return this.http.post<MyResponse>(this.genreApi + 'Create', genre);

  }

  update(genre: Genre) : Observable<MyResponse>{
    return this.http.put<MyResponse>(this.genreApi + 'Update', genre);
  }

  delete(id): Observable<MyResponse>{
    return this.http.delete<MyResponse>(this.genreApi + 'Delete/' + String (id));
  }
}
