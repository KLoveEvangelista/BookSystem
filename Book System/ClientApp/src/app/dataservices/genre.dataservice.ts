import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Genre } from '../models/genre.model';

@Injectable({
    providedIn: 'root'
})
export class GenreDataService implements OnDestroy{
    genreSource =  new BehaviorSubject<Genre[]>([]);
    genres = this.genreSource.asObservable();
    //refresh other component that subscribe to it

    refreshGenres(){
        this.genreSource.next(null);
    }

    ngOnDestroy(){
        this.genreSource.unsubscribe();
    }
    
}
