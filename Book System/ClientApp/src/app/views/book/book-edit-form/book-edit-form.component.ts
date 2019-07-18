import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Book } from 'src/app/models/book.model';
import { BookService } from 'src/app/services/book.service';
import { BookDataService } from 'src/app/dataservices/book.dataservice';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

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

  constructor(
    private bookService: BookService,
    private bookDataService: BookDataService,
    private dialogRef: MatDialogRef<BookEditFormComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) { 
    this.bookEditForm= new FormGroup({
      id: new FormControl(),
      title: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      copyright: new FormControl('', [Validators.required, Validators.maxLength(4)])
    })
    this.bookContext = data;
  }
  
  ngOnInit() {
    this.bookToBeEditted = this.bookContext.bookContext;
    this.change();
  }
  get f() {return this.bookEditForm.controls;}

  change(){
    this.bookEditForm.controls['title'].setValue(this.bookToBeEditted.title)
    this.bookEditForm.controls['copyright'].setValue(this.bookToBeEditted.copyright)
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

}
