import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BookService } from 'src/app/services/book.service';
import { BookDataService } from 'src/app/dataservices/book.dataservice';
import { Genre } from 'src/app/models/genre.model';
import { GenreService } from 'src/app/services/genre.service';
import { GenreDataService } from 'src/app/dataservices/genre.dataservice';
import { of } from 'rxjs';
import { DateAdapter, MatDialogConfig, MatDialog } from '@angular/material';
import { GenreComponent } from '../../genre/genre.component';

@Component({
  selector: 'app-book-add-form',
  templateUrl: './book-add-form.component.html',
  styleUrls: ['./book-add-form.component.css']
})
export class BookAddFormComponent implements OnInit {
  bookCreateForm: FormGroup;
  isSubmit = false;

  titleBackEndErrors: string[];
  copyrightBackEndErrors: string[];

  genresList: Genre[]
  
  constructor(
    private bookService: BookService,
    private bookDataService: BookDataService,
    private genreService: GenreService,
    private genreDataService: GenreDataService,
    private dialog: MatDialog
  ) { 
    this.bookCreateForm= new FormGroup({
      title: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      copyright: new FormControl('', [Validators.required, Validators.maxLength(4)]),
      genreID: new FormControl ('', [Validators.required]),
      genreSelect: new FormControl('', [Validators.required])
    })
       
  }

  ngOnInit() {
    this.genreDataService.genreSource.subscribe(data => {
      this.getGenreLists();
    })
  }
  get f(){return this.bookCreateForm.controls;}

  async onFormSubmit(){
    let ok = confirm("Are you sure you want to submit?")
    if(!ok)
      return;
    if(!this.bookCreateForm.valid)
      return;
    try{
      this.isSubmit=true;
      this.titleBackEndErrors=null;
      this.copyrightBackEndErrors=null;
      let result = await this.bookService.create(this.bookCreateForm.value).toPromise();
      if(result.isSuccess){
        alert(result.message)
        this.bookCreateForm.reset();
        this.bookDataService.refreshBooks();
      } else{
        alert(result.message)
      }
    } catch (error){
      let errs = error.error

      if(errs.isSuccess === false){
        alert(errs.message);
        return;
      }
      //shows backend errors to UI
      if(errs.error){
        if('title' in errs.errors)
        this.titleBackEndErrors=errs.error.title
        if('copyright' in errs.errors)
        this.copyrightBackEndErrors = errs.error.copyright
      }
      alert('something went wrong')
      this.isSubmit=false;
    } finally{
      this.isSubmit=false;
    }
  }

  async getGenreLists(){
    try {
      this.genresList = await this.genreService.getAll().toPromise();
      //console.log(this.genresList)
    } catch (error) {
      console.log(error)
      
    }
  }

  selectGenre($event){
    let genre = this.bookCreateForm.value.genreSelect;
    if(genre.length > 2){
      if($event.timeStamp > 200){
        let selectedGenre = this.genresList.find(data=> data.name == genre);
        //console.log(genre)
       // console.log(selectedGenre)
        if(selectedGenre)
          this.bookCreateForm.controls['genreID'].setValue(selectedGenre.id)
      }
    }
  }

  openGenreDialog(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '600px';
    dialogConfig.height= '600px';
    this.dialog.open(GenreComponent, dialogConfig);
  }
  /*test(){
    console.log(this.bookCreateForm.value)
  }*/
}
