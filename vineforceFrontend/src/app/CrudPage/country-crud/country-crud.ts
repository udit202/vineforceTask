// src/app/CrudPage/country-crud/country-crud.ts

import {
  Component,
  OnInit,
  ChangeDetectorRef,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  HttpClient,
  HttpErrorResponse
} from '@angular/common/http';

import { environment } from '../../../environments/environment';

import {
  Country,
  CreateCountryDto,
  UpdateCountryDto
} from '../../models/country.model';


@Component({
  selector: 'app-country-crud',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './country-crud.html',
  styleUrl: './country-crud.css'
})
export class CountryCrud implements OnInit {

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  private apiUrl =
    `${environment.apiBaseUrl}/CountryCrud`;


  // =====================================================
  // DATA
  // =====================================================

  countries: Country[] = [];


  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  saving = false;

  deletingId: number | null = null;

  errorMessage = '';

  successMessage = '';


  // =====================================================
  // FORM STATE
  // =====================================================

  isEditMode = false;

  editingId: number | null = null;

  form: CreateCountryDto = {
    name: '',
    code: '',
    isActive: true
  };


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    this.fetchCountries();

  }


  // =====================================================
  // GET COUNTRIES
  // =====================================================

  fetchCountries(): void {

    if (this.loading) {
      return;
    }


    this.loading = true;

    this.errorMessage = '';


    this.http
      .get<Country[]>(this.apiUrl)
      .subscribe({

        next: (data) => {

          console.log(
            'Countries API Response:',
            data
          );


          this.countries = Array.isArray(data)
            ? [...data]
            : [];


          this.loading = false;


          /*
           * Force immediate UI refresh.
           */
          this.cdr.detectChanges();

        },


        error: (err: HttpErrorResponse) => {

          console.error(
            'Countries API Error:',
            err
          );


          this.loading = false;

          this.errorMessage =
            this.getErrorMessage(err);


          this.cdr.detectChanges();

        }

      });

  }


  // =====================================================
  // SUBMIT
  // =====================================================

  onSubmit(): void {

    this.clearMessages();


    const name =
      this.form.name?.trim();

    const code =
      this.form.code?.trim();


    if (!name || !code) {

      this.errorMessage =
        'Name and Code are required.';

      this.cdr.detectChanges();

      return;

    }


    /*
     * Keep trimmed values.
     */
    this.form = {
      ...this.form,
      name,
      code
    };


    if (this.isEditMode) {

      this.updateCountry();

    }
    else {

      this.createCountry();

    }

  }


  // =====================================================
  // CREATE COUNTRY
  // =====================================================

  createCountry(): void {

    if (this.saving) {
      return;
    }


    this.saving = true;

    this.clearMessages();


    const dto: CreateCountryDto = {
      name: this.form.name.trim(),
      code: this.form.code.trim(),
      isActive: this.form.isActive
    };


    console.log(
      'Creating country:',
      dto
    );


    this.http
      .post<Country>(
        this.apiUrl,
        dto
      )
      .subscribe({

        next: (createdCountry) => {

          console.log(
            'Country created:',
            createdCountry
          );


          /*
           * Immediately add new country
           * to the UI.
           */
          if (createdCountry) {

            this.countries = [
              ...this.countries,
              createdCountry
            ];

          }


          this.saving = false;

          this.resetForm();


          this.successMessage =
            'Country created successfully.';


          /*
           * Force immediate UI update.
           */
          this.cdr.detectChanges();


          /*
           * Sync again with database.
           */
          this.fetchCountries();

        },


        error: (err: HttpErrorResponse) => {

          console.error(
            'Create Country Error:',
            err
          );


          this.saving = false;

          this.errorMessage =
            this.getErrorMessage(err);


          this.cdr.detectChanges();

        }

      });

  }


  // =====================================================
  // START EDIT
  // =====================================================

  startEdit(country: Country): void {

    this.isEditMode = true;

    this.editingId = country.id;


    this.form = {

      name: country.name,

      code: country.code,

      isActive: country.isActive

    };


    this.clearMessages();


    this.cdr.detectChanges();

  }


  // =====================================================
  // UPDATE COUNTRY
  // =====================================================

  updateCountry(): void {

    if (
      this.editingId === null ||
      this.saving
    ) {

      return;

    }


    this.saving = true;

    this.clearMessages();


    const dto: UpdateCountryDto = {

      name: this.form.name.trim(),

      code: this.form.code.trim(),

      isActive: this.form.isActive

    };


    console.log(
      'Updating country:',
      this.editingId,
      dto
    );


    this.http
      .put<Country>(
        `${this.apiUrl}/${this.editingId}`,
        dto
      )
      .subscribe({

        next: (updatedCountry) => {

          console.log(
            'Country updated:',
            updatedCountry
          );


          /*
           * Immediately update the
           * existing row in UI.
           */
          this.countries =
            this.countries.map(
              country =>
                country.id === this.editingId
                  ? updatedCountry
                  : country
            );


          this.saving = false;

          this.resetForm();


          this.successMessage =
            'Country updated successfully.';


          this.cdr.detectChanges();


          /*
           * Sync with database.
           */
          this.fetchCountries();

        },


        error: (err: HttpErrorResponse) => {

          console.error(
            'Update Country Error:',
            err
          );


          this.saving = false;

          this.errorMessage =
            this.getErrorMessage(err);


          this.cdr.detectChanges();

        }

      });

  }


  // =====================================================
  // DELETE COUNTRY
  // =====================================================

  deleteCountry(id: number): void {

    if (this.deletingId !== null) {
      return;
    }


    const confirmed =
      window.confirm(
        'Are you sure you want to delete this country?'
      );


    if (!confirmed) {
      return;
    }


    this.deletingId = id;

    this.clearMessages();


    console.log(
      'Deleting country:',
      id
    );


    this.http
      .delete<{ message: string }>(
        `${this.apiUrl}/${id}`
      )
      .subscribe({

        next: (response) => {

          console.log(
            'Country deleted:',
            response
          );


          /*
           * Immediately remove from UI.
           */
          this.countries =
            this.countries.filter(
              country =>
                country.id !== id
            );


          this.deletingId = null;


          this.successMessage =
            'Country deleted successfully.';


          /*
           * Force UI refresh.
           */
          this.cdr.detectChanges();


          /*
           * Sync with database.
           */
          this.fetchCountries();

        },


        error: (err: HttpErrorResponse) => {

          console.error(
            'Delete Country Error:',
            err
          );


          this.deletingId = null;

          this.errorMessage =
            this.getErrorMessage(err);


          this.cdr.detectChanges();

        }

      });

  }


  // =====================================================
  // CANCEL EDIT
  // =====================================================

  cancelEdit(): void {

    this.resetForm();

    this.clearMessages();

    this.cdr.detectChanges();

  }


  // =====================================================
  // RESET FORM
  // =====================================================

  private resetForm(): void {

    this.form = {

      name: '',

      code: '',

      isActive: true

    };


    this.isEditMode = false;

    this.editingId = null;

  }


  // =====================================================
  // CLEAR MESSAGES
  // =====================================================

  private clearMessages(): void {

    this.errorMessage = '';

    this.successMessage = '';

  }


  // =====================================================
  // ERROR MESSAGE
  // =====================================================

  private getErrorMessage(
    err: HttpErrorResponse
  ): string {

    if (err.error?.message) {

      return err.error.message;

    }


    if (
      err.error?.title
    ) {

      return err.error.title;

    }


    if (
      err.status === 0
    ) {

      return 'Cannot reach the server. Is the API running?';

    }


    return `Request failed (status ${err.status}).`;

  }

}