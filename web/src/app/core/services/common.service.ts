import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { ConfigValueDto, CountryDto, LanguageDto } from '../models/common.models';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private api: ApiService) { }

  getAllCountries(): Observable<CountryDto[]> {
    return this.api.get<CountryDto[]>('api/base/countries-list');
  }

  getLanguagesByCountryId(countryId: number): Observable<CountryDto> { 
    // Note: The C# service returns a single CountryDto object which likely contains the languages list, 
    // or maybe it adapts it. The C# method name is `GetLanguagesByCountryIdAsync` but returns `CountryDto`.
    // Validated C# code: public async Task<CountryDto> GetLanguagesByCountryIdAsync(int countryId) ...
    return this.api.get<CountryDto>(`api/base/language-by-country/${countryId}`);
  }

  // Helper method if we just want languages from the above call often
  getLanguagesListByCountryId(countryId: number): Observable<LanguageDto[]> {
     // This logic might need to be in the component or via a pipe, but keeping raw API call here.
     return new Observable(observer => {
       this.getLanguagesByCountryId(countryId).subscribe({
         next: (country) => {
           observer.next(country.languages || []);
           observer.complete();
         },
         error: (err) => observer.error(err)
       });
     });
  }

  getConfigValues(configKey: string, tenantId?: number): Observable<ConfigValueDto[]> {
    let path = `api/base/configKey/${configKey}`;
    if (tenantId) {
      path += `?tenantId=${tenantId}`;
    }
    return this.api.get<ConfigValueDto[]>(path);
  }

  getMultipleConfigValues(configKeys: string[], tenantId: number): Observable<{[key: string]: ConfigValueDto[]}> {
     // The endpoint expects tenantId as query param and keys as body
     return this.api.post<{[key: string]: ConfigValueDto[]}>(
        `api/base/get-all-config-values?tenantId=${tenantId}`, 
        configKeys
     );
  }
}
