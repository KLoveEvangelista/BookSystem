import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './views/home/home.component';
import { CounterComponent } from './views/counter/counter.component';
import { FetchDataComponent } from './views/fetch-data/fetch-data.component';
import { MySampleComponent } from './views/my-sample/my-sample.component';
import { BookComponent } from './views/book/book.component';
import { GenreComponent } from './views/genre/genre.component';
import { RegisterComponent } from './views/auth/register/register.component';
import { AuthGuard } from './guard/auth.guard';
import { ForbiddenComponent } from './views/auth/forbidden/forbidden.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full'},
  { path: 'counter', component: CounterComponent, canActivate:[AuthGuard]},
  { path: 'fetch-data', component: FetchDataComponent, canActivate:[AuthGuard]},
  { path: 'my-sample', component: MySampleComponent, canActivate:[AuthGuard]},
  { path: 'book', component: BookComponent, canActivate:[AuthGuard]},
  { path: 'genre', component: GenreComponent, canActivate:[AuthGuard]},
  { path: 'register', component: RegisterComponent, canActivate: [AuthGuard], data: {permittedRoles: ['Admin']}},
  { path: 'forbidden', component: ForbiddenComponent}
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]

})
export class AppRoutingModule { }
