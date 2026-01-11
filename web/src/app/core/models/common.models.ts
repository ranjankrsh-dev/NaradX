export interface LanguageDto {
  id: number;
  countryId: number;
  culture: string;
  name: string;
  localName: string;
  isDefault: boolean;
  description?: string;
}

export interface CountryDto {
  id: number;
  name: string;
  code: string;
  phoneCode: string;
  currencyCode: string;
  currencySymbol: string;
  timezone: string;
  languages: LanguageDto[];
}

export interface ConfigValueDto {
  value: string;
  text: string;
}
