import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
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
import { BookUpdateFormComponent } from './views/book/book-update-form/book-update-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatDialogModule} from '@angular/material';
import { GenreComponent } from './views/genre/genre.component';
import { GenreAddFormComponent } from './views/genre/genre-add-form/genre-add-form.component';
import { GenreEditFormComponent } from './views/genre/genre-edit-form/genre-edit-form.component';
import { RegisterComponent } from './views/auth/register/register.component';
import {ToastrModule} from 'ngx-toastr';
import { LoginComponent } from './views/auth/login/login.component';
import { ForbiddenComponent } from './views/auth/forbidden/forbidden.component';
import { AuthInterceptor } from './guard/auth.interceptor';
import {DataTablesModule} from 'angular-datatables';
import { AuthorComponent } from './views/author/author.component';
import { AuthorAddFormComponent } from './views/author/author-add-form/author-add-form.component';
import { AuthorUpdateFormComponent } from './views/author/author-update-form/author-update-form.component';



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
    BookUpdateFormComponent,
    GenreComponent,
    GenreAddFormComponent,
    GenreEditFormComponent,
    RegisterComponent,
    LoginComponent,
    ForbiddenComponent,
    AuthorComponent,
    AuthorAddFormComponent,
    AuthorUpdateFormComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
   ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule,
    ToastrModule.forRoot(),
    DataTablesModule
  ],
  entryComponents: [BookUpdateFormComponent, GenreEditFormComponent, AuthorComponent, AuthorUpdateFormComponent],
  providers: [
    { provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
