import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BookService } from 'src/app/services/book.service';
import { BookDataService } from 'src/app/dataservices/book.dataservice';

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
  
  constructor(
    private bookService: BookService,
    private bookDataService: BookDataService
  ) { 
    this.bookCreateForm= new FormGroup({
      title: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      copyright: new FormControl('', [Validators.required, Validators.maxLength(4)])
    })
       
  }

  ngOnInit() {
  }

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
}
