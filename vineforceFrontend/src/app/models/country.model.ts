// src/app/models/country.model.ts

export interface Country {
  id: number;
  name: string;
  code: string;
  isActive: boolean;
  createdAt?: string;
  updatedAt?: string;
}

export interface CreateCountryDto {
  name: string;
  code: string;
  isActive: boolean;
}

export interface UpdateCountryDto {
  name: string;
  code: string;
  isActive: boolean;
}