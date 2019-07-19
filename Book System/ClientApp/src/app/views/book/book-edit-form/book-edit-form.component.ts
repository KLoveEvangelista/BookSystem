import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Book } from 'src/app/models/book.model';
import { BookService } from 'src/app/services/book.service';
import { BookDataService } from 'src/app/dataservices/book.dataservice';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog, MatDialogConfig } from '@angular/material';
import { Genre } from 'src/app/models/genre.model';
import { GenreService } from 'src/app/services/genre.service';
import { GenreDataService } from 'src/app/dataservices/genre.dataservice';
import { GenreComponent } from '../../genre/genre.component';

@Component({
  selector: 'app-book-edit-form',
  templateUrl: './book-edit-form.component.html',
  styleUrls: ['./book-edit-form.component.css']
})
export class BookEditFormComponent implements OnInit {
  isSubmit = false;
  titleBackEndErrors: string[];
  copyrightBackEndErrors: string[];

  bookContext: any;
  bookEditForm: FormGroup;
  bookToBeEditted: Book;
  genresList: Genre[];

  constructor(
    private bookService: BookService,
    private bookDataService: BookDataService,
    private genreService: GenreService,
    private genreDataService: GenreDataService,
    private dialog: MatDialog,
    private dialogRef: MatDialogRef<BookEditFormComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) { 
    this.bookEditForm= new FormGroup({
      id: new FormControl(),
      title: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      copyright: new FormControl('', [Validators.required, Validators.maxLength(4)]),
      genreID: new FormControl ('', [Validators.required]),
      genreSelect: new FormControl('', [Validators.required])
    })
    this.bookContext = data;
  }
  
  ngOnInit() {
    this.bookToBeEditted = this.bookContext.bookContext;
    this.change();
    this.genreDataService.genreSource.subscribe(data => {
      this.getGenreLists();
    })
    
  }
  get f() {return this.bookEditForm.controls;}

  change(){
    this.bookEditForm.controls['title'].setValue(this.bookToBeEditted.title)
    this.bookEditForm.controls['copyright'].setValue(this.bookToBeEditted.copyright)
    this.bookEditForm.controls['genreID'].setValue(this.bookToBeEditted.genre.id)
    this.bookEditForm.controls['genreSelect'].setValue(this.bookToBeEditted.genre.name)
  }

  close(){
    this.dialogRef.close();
  }
  async onFormSubmit(){
    let ok = confirm("Are you sure you want to submit?")
    if(!ok)
      return;
    if(!this.bookEditForm.valid)
      return;
    try{
      this.isSubmit=true;
      this.titleBackEndErrors=null;
      this.copyrightBackEndErrors=null;
      this.bookEditForm.value.id = this.bookToBeEditted.id;
      let result = await this.bookService.update(this.bookEditForm.value).toPromise();
      if(result.isSuccess){
        alert(result.message)
        this.bookEditForm.reset();
        this.bookDataService.refreshBooks();
        this.close();
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
        this.titleBackEndErrors=errs.errors.title
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
      console.log(this.genresList)
    } catch (error) {
      console.log(error)
      
    }
  }

  selectGenre($event){
    let genre = this.bookEditForm.value.genreSelect;
    if(genre.length > 2){
      if($event.timeStamp > 200){
        let selectedGenre = this.genresList.find(data=> data.name == genre);
        //console.log(genre)
       // console.log(selectedGenre)
        if(selectedGenre)
          this.bookEditForm.controls['genreID'].setValue(selectedGenre.id)
      }
    }
  }

  openGenreDialog(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '600px';
    dialogConfig.height= '600px';
    this.dialog.open(GenreComponent, dialogConfig);
  }

}
