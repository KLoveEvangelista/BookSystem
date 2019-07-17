import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { getBaseUrl } from 'src/main';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model';
import { stringify } from 'querystring';
import { MyResponse } from '../models/myresponse.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {
    bookApi: string;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) { 
    this.bookApi= baseUrl + 'api/Book/'
  }

  getAll(): Observable<Book[]>{
    return this.http.get<Book[]>(this.bookApi + 'GetAll');
  }

  getSingleBy(id): Observable<Book>{
    return this.http.get<Book>(this.bookApi + 'GetSingleBy/' + String(id));
  }

  create(book:Book): Observable<MyResponse>{
    return this.http.post<MyResponse>(this.bookApi + 'Create', book);

  }

  update(book: Book) : Observable<MyResponse>{
    return this.http.put<MyResponse>(this.bookApi + 'Update', book);
  }

  delete(id): Observable<MyResponse>{
    return this.http.delete<MyResponse>(this.bookApi + 'Delete/' + String (id));
  }
}
