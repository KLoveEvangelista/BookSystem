import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { GenreService } from 'src/app/services/genre.service';
import { GenreDataService } from 'src/app/dataservices/genre.dataservice';

@Component({
  selector: 'app-genre-add-form',
  templateUrl: './genre-add-form.component.html',
  styleUrls: ['./genre-add-form.component.css']
})
export class GenreAddFormComponent implements OnInit {
  genreCreateForm: FormGroup;
  isSubmit = false;

  nameBackEndErrors: string[];
  constructor(
    private genreService: GenreService,
    private genreDataService: GenreDataService
  ) { 
    this.genreCreateForm= new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    })
       
  }

  ngOnInit() {

  }
  get f(){return this.genreCreateForm.controls;}

  async onFormSubmit(){
    let ok = confirm("Are you sure you want to submit?")
    if(!ok)
      return;
    if(!this.genreCreateForm.valid)
      return;
    try{
      this.isSubmit=true;
      this.nameBackEndErrors=null;
      let result = await this.genreService.create(this.genreCreateForm.value).toPromise();
      if(result.isSuccess){
        alert(result.message)
        this.genreCreateForm.reset();
        this.genreDataService.refreshGenres();
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
        this.nameBackEndErrors=errs.error.name
      }
      alert('something went wrong')
      this.isSubmit=false;
    } finally{
      this.isSubmit=false;
    }
  }
}
