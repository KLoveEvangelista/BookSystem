import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Book } from '../models/book.model';

@Injectable({
    providedIn: 'root'
})
export class BookDataService implements OnDestroy{
    bookSource =  new BehaviorSubject<Book[]>([]);
    books = this.bookSource.asObservable();
    //refresh other component that subscribe to it

    refreshBooks(){
        this.bookSource.next(null);
    }

    ngOnDestroy(){
        this.bookSource.unsubscribe();
    }
    
}
