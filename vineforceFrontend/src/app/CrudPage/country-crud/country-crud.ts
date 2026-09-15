// src/app/CrudPage/country-crud/country-crud.ts

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Country, CreateCountryDto, UpdateCountryDto } from '../../models/country.model';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'app-country-crud',
  styleUrl: './country-crud.css',
  templateUrl: './country-crud.html',
})
export class CountryCrud implements OnInit {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}/CountryCrud`;

  countries: Country[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';

  // form state
  isEditMode = false;
  editingId: number | null = null;
  form: CreateCountryDto = { name: '', code: '', isActive: true };

  ngOnInit(): void {
    this.fetchCountries();
  }

  fetchCountries(): void {
    this.loading = true;
    this.errorMessage = '';

    this.http.get<Country[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.countries = data;
        this.loading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = this.getErrorMessage(err);
        this.loading = false;
      },
    });
  }

  onSubmit(): void {
    if (!this.form.name.trim() || !this.form.code.trim()) {
      this.errorMessage = 'Name and Code are required.';
      return;
    }

    this.isEditMode ? this.updateCountry() : this.createCountry();
  }

  createCountry(): void {
    this.loading = true;
    this.clearMessages();
    const dto: CreateCountryDto = { ...this.form };

    this.http.post<Country>(this.apiUrl, dto).subscribe({
      next: () => {
        this.successMessage = 'Country created successfully.';
        this.resetForm();
        this.fetchCountries();
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = this.getErrorMessage(err);
        this.loading = false;
      },
    });
  }

  startEdit(country: Country): void {
    this.isEditMode = true;
    this.editingId = country.id;
    this.form = {
      name: country.name,
      code: country.code,
      isActive: country.isActive,
    };
    this.clearMessages();
  }

  updateCountry(): void {
    if (this.editingId === null) return;

    this.loading = true;
    this.clearMessages();
    const dto: UpdateCountryDto = { ...this.form };

    this.http.put<Country>(`${this.apiUrl}/${this.editingId}`, dto).subscribe({
      next: () => {
        this.successMessage = 'Country updated successfully.';
        this.resetForm();
        this.fetchCountries();
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = this.getErrorMessage(err);
        this.loading = false;
      },
    });
  }

  deleteCountry(id: number): void {
    if (!confirm('Are you sure you want to delete this country?')) return;

    this.loading = true;
    this.clearMessages();

    this.http.delete<{ message: string }>(`${this.apiUrl}/${id}`).subscribe({
      next: () => {
        this.successMessage = 'Country deleted successfully.';
        this.fetchCountries();
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = this.getErrorMessage(err);
        this.loading = false;
      },
    });
  }

  cancelEdit(): void {
    this.resetForm();
    this.clearMessages();
  }

  private resetForm(): void {
    this.form = { name: '', code: '', isActive: true };
    this.isEditMode = false;
    this.editingId = null;
    this.loading = false;
  }

  private clearMessages(): void {
    this.errorMessage = '';
    this.successMessage = '';
  }

  private getErrorMessage(err: HttpErrorResponse): string {
    if (err.error?.message) return err.error.message;
    if (err.status === 0) return 'Cannot reach the server. Is the API running?';
    return `Request failed (status ${err.status}).`;
  }
}