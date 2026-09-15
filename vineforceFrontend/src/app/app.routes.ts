// src/app/app.routes.ts

import { Routes } from '@angular/router';

import { CountryCrud } from './CrudPage/country-crud/country-crud';
import { Products } from './CrudPage/products/products';
import { Orders } from './CrudPage/orders/orders';

export const routes: Routes = [

  { path: '', redirectTo: 'products', pathMatch: 'full' },

  // Products
  { path: 'products', component: Products },

  // Countries
  { path: 'countries', component: CountryCrud },

  // Orders
  { path: 'orders', component: Orders },

];