import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/models/book.model';
import { BookService } from 'src/app/services/book.service';
import { BookDataService } from 'src/app/dataservices/book.dataservice';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { BookEditFormComponent } from './book-edit-form/book-edit-form.component';
import { GenreDataService } from 'src/app/dataservices/genre.dataservice';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  books: Book[];

  constructor(
    private bookService: BookService,
    private bookDataService: BookDataService,
    private genreDataService: GenreDataService,
    private dialog: MatDialog) 
    { }
    

  ngOnInit() {
    // will trigger everytime BookDataService.refreshBooks is called
    this.bookDataService.books.subscribe( data => {
      this.getBooks()
    })
    this.genreDataService.genres.subscribe( data => {
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
  async delete(id){
    if(confirm('Are you sure you want to delete')){
      try{
        let result = await this.bookService.delete(id).toPromise()
        if(result.isSuccess){
          alert(result.message)
          this.bookDataService.refreshBooks()
        } else{
          alert(result.message)
        }     
    } catch (error){
      alert('Something went worng')
      console.log(error)
      }
    }
  }
  update(book){
    //instantiate dialog config object
    const dialogConfig = new MatDialogConfig();
    //add bookContext data of selected book
    dialogConfig.data = {
      bookContext: book
    }

    dialogConfig.width = '600px';

    this.dialog.open(BookEditFormComponent, dialogConfig);
  }
}
