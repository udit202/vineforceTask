// src/app/app.routes.ts

import { Routes } from '@angular/router';

import { CountryCrud } from './CrudPage/country-crud/country-crud';
import { Products } from './CrudPage/products/products';

export const routes: Routes = [

  { path: 'countries', component: CountryCrud },

  { path: 'products', component: Products },

  { path: '', redirectTo: 'countries', pathMatch: 'full' },

];