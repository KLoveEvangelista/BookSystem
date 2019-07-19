import { Genre } from './genre.model';

export interface Book {
    id: number;
    myGuid: string;
    title: string;
    copyright: number;
    genreId: number;
    genre: Genre;
    
}
