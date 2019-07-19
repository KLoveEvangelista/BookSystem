import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './views/Layouts/nav-menu/nav-menu.component';
import { HomeComponent } from './views/home/home.component';
import { CounterComponent } from './views/counter/counter.component';
import { FetchDataComponent } from './views/fetch-data/fetch-data.component';
import { AppRoutingModule } from './app-routing.module';
import { MySampleComponent } from './views/my-sample/my-sample.component';
import { BookComponent } from './views/book/book.component';
import { BookAddFormComponent } from './views/book/book-add-form/book-add-form.component';
import { BookEditFormComponent } from './views/book/book-edit-form/book-edit-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatDialogModule} from '@angular/material';
import { GenreComponent } from './views/genre/genre.component';
import { GenreAddFormComponent } from './views/genre/genre-add-form/genre-add-form.component';
import { GenreEditFormComponent } from './views/genre/genre-edit-form/genre-edit-form.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    MySampleComponent,
    BookComponent,
    BookAddFormComponent,
    BookEditFormComponent,
    GenreComponent,
    GenreAddFormComponent,
    GenreEditFormComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
   ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule
  ],
  entryComponents: [BookEditFormComponent, GenreEditFormComponent],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
