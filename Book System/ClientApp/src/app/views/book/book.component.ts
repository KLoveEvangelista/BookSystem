import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/models/book.model';
import { BookService } from 'src/app/services/book.service';
import { BookDataService } from 'src/app/dataservices/book.dataservice';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  books: Book[];

  constructor(
    private bookService: BookService,
    private bookDataService: BookDataService) 
    { }
    

  ngOnInit() {
    // will trigger everytime BookDataService.refreshBooks is called
    this.bookDataService.books.subscribe( data => {
      this.getBooks()
    })
  }

  //call the getAll function the assign to books to be displayed
  async getBooks(){
    try {
      this.books = await this.bookService.getAll().toPromise();
    } catch (error) {
    
      console.log(error)
      alert('Something went wrong')
    }
  }
}
