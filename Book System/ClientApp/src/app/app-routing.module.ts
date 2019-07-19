import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './views/home/home.component';
import { CounterComponent } from './views/counter/counter.component';
import { FetchDataComponent } from './views/fetch-data/fetch-data.component';
import { MySampleComponent } from './views/my-sample/my-sample.component';
import { BookComponent } from './views/book/book.component';
import { GenreComponent } from './views/genre/genre.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full'},
  { path: 'counter', component: CounterComponent},
  { path: 'fetch-data', component: FetchDataComponent},
  { path: 'my-sample', component: MySampleComponent},
  { path: 'book', component: BookComponent},
  { path: 'genre', component: GenreComponent}
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]

})
export class AppRoutingModule { }
