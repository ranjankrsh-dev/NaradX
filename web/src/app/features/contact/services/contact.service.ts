import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/core/services/api.service';
import { Observable } from 'rxjs';
import { BulkUploadValidateResponseDto, ContactDto, ContactFilters, PagedResult } from 'src/app/core/models/contact.models';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root' // or providedIn: ContactModule if we want to scope it
})
export class ContactService {

  constructor(private api: ApiService) { }

  getContacts(filters: ContactFilters): Observable<PagedResult<ContactDto>> {
    return this.api.post<PagedResult<ContactDto>>('contact/list', filters); 
  }

  addContact(contact: ContactDto): Observable<number> {
    return this.api.post('contact/add', contact);
  }

  updateContact(contact: ContactDto): Observable<number> {
    return this.api.put('contact/update', contact);
  }

  deleteContact(id: number): Observable<number> {
    // MVC Service uses GetData for delete: string endpoint = $"{BaseUrl}/delete-contact-by-id/{contactId}";
    return this.api.get(`contact/delete-contact-by-id/${id}`);
  }

  getContactById(id: number): Observable<ContactDto> {
      return this.api.get(`contact/get-contact-by-id/${id}`);
  }

  // Bulk Upload
  validateBulkUpload(file: File, countryId: number, languageId: number, source: string, channel: string): Observable<BulkUploadValidateResponseDto> {
    const formData = new FormData();
    formData.append('UploadedFile', file);
    formData.append('CountryId', countryId.toString());
    formData.append('LanguageId', languageId.toString());
    formData.append('ContactSource', source);
    formData.append('ChannelPreference', channel);
    
    return this.api.postMultipart('contact/bulk-upload-validate', formData);
  }

  saveBulkUpload(data: BulkUploadValidateResponseDto): Observable<any> {
      return this.api.post('contact/bulk-upload-confirm', data);
  }

  downloadTemplate(): Observable<Blob> {
      return this.api.getFile('contact/bulk-upload-download-template');
  }
}
