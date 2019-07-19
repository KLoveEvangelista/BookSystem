import { Component, OnInit } from '@angular/core';
import { Genre } from 'src/app/models/genre.model';
import { GenreService } from 'src/app/services/genre.service';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { GenreDataService } from 'src/app/dataservices/genre.dataservice';
import { GenreEditFormComponent } from './genre-edit-form/genre-edit-form.component';

@Component({
  selector: 'app-genre',
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.css']
})
export class GenreComponent implements OnInit {
  genres: Genre[];

  constructor(
    private genreService: GenreService,
    private genreDataService: GenreDataService,
    private dialog: MatDialog) 
    { }
    

  ngOnInit() {
    // will trigger everytime BookDataService.refreshGenres is called
    this.genreDataService.genres.subscribe( data => {
      this.getGenres()
    })
  }

  //call the getAll function the assign to books to be displayed
  async getGenres(){
    try {
      this.genres = await this.genreService.getAll().toPromise();
    } catch (error) {
    
      console.log(error)
      alert('Something went wrong')
    }
  }
  async delete(id){
    if(confirm('Are you sure you want to delete')){
      try{
        let result = await this.genreService.delete(id).toPromise()
        if(result.isSuccess){
          alert(result.message)
          this.genreDataService.refreshGenres()
        } else{
          alert(result.message)
        }     
    } catch (error){
      alert('Something went worng')
      console.log(error)
      }
    }
  }
  update(genre){
    //instantiate dialog config object
    const dialogConfig = new MatDialogConfig();
    //add bookContext data of selected book
    dialogConfig.data = {
      genreContext: genre
    }

    dialogConfig.width = '600px';

    this.dialog.open(GenreEditFormComponent, dialogConfig);
  }
}
