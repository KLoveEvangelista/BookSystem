import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Genre } from 'src/app/models/genre.model';
import { GenreService } from 'src/app/services/genre.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { GenreDataService } from 'src/app/dataservices/genre.dataservice';

@Component({
  selector: 'app-genre-edit-form',
  templateUrl: './genre-edit-form.component.html',
  styleUrls: ['./genre-edit-form.component.css']
})
export class GenreEditFormComponent implements OnInit {
  isSubmit = false;
  nameBackEndErrors: string[];

  genreContext: any;
  genreEditForm: FormGroup;
  genreToBeEditted: Genre;

  constructor(
    private genreService: GenreService,
    private genreDataService: GenreDataService,
    private dialogRef: MatDialogRef<GenreEditFormComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) { 
    this.genreEditForm= new FormGroup({
      id: new FormControl(),
      name: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    })
    this.genreContext = data;
  }
  
  ngOnInit() {
    this.genreToBeEditted = this.genreContext.genreContext;
    this.change();
  }
  get f() {return this.genreEditForm.controls;}

  change(){
    this.genreEditForm.controls['name'].setValue(this.genreToBeEditted.name)
  }

  close(){
    this.dialogRef.close();
  }
  async onFormSubmit(){
    let ok = confirm("Are you sure you want to submit?")
    if(!ok)
      return;
    if(!this.genreEditForm.valid)
      return;
    try{
      this.isSubmit=true;
      this.nameBackEndErrors=null;
      this.genreEditForm.value.id = this.genreToBeEditted.id;
      let result = await this.genreService.update(this.genreEditForm.value).toPromise();
      if(result.isSuccess){
        alert(result.message)
        this.genreEditForm.reset();
        this.genreDataService.refreshGenres();
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
        if('name' in errs.errors)
        this.nameBackEndErrors=errs.errors.name
      }
      alert('something went wrong')
      this.isSubmit=false;
    } finally{
      this.isSubmit=false;
    }
  }

}
