// src/app/app.routes.ts

import { Routes } from '@angular/router';
import { CountryCrud } from './CrudPage/country-crud/country-crud';

export const routes: Routes = [
  { path: 'countries', component: CountryCrud },
  { path: '', redirectTo: 'countries', pathMatch: 'full' },
];