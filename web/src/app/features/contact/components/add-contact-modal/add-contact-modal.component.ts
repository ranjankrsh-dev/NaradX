import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfigValueDto, CountryDto, LanguageDto } from 'src/app/core/models/common.models';
import { ContactDto } from 'src/app/core/models/contact.models';
import { CommonService } from 'src/app/core/services/common.service';
import { ContactService } from '../../services/contact.service';

declare var toastr: any;

@Component({
  selector: 'app-add-contact-modal',
  templateUrl: './add-contact-modal.component.html',
  styleUrls: ['./add-contact-modal.component.scss']
})
export class AddContactModalComponent implements OnInit {
  @Input() contactId: number | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<void>();

  contactForm: FormGroup;
  isSaving = false;
  
  countries: CountryDto[] = [];
  availableLanguages: LanguageDto[] = [];
  contactSources: ConfigValueDto[] = [];
  channelPreferences: ConfigValueDto[] = [];

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService,
    private commonService: CommonService
  ) {
    this.contactForm = this.fb.group({
      id: [0],
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      middleName: [''],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      countryId: [null, Validators.required],
      languageId: [null, Validators.required], // TODO: Validate range if needed
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\+?[1-9]\d{1,14}$/)]],
      email: ['', [Validators.email]],
      company: ['', [Validators.maxLength(100)]],
      jobTitle: ['', [Validators.maxLength(50)]],
      contactSource: [null], // Required in DTO but maybe default handled
      channelPreference: [null]
    });
  }

  ngOnInit(): void {
    this.loadLookups();
    if (this.contactId) {
      this.loadContact(this.contactId);
    }
  }

  loadLookups() {
    this.commonService.getAllCountries().subscribe(countries => {
      this.countries = countries;
      // If editing and country is already set, we need to load languages for that country
      const cid = this.contactForm.get('countryId')?.value;
      if (cid) {
          this.loadLanguages(cid);
      }
    });

    // Assume tenantId is 0 or obtained from auth service. 
    // For now hardcoding 0 as per MVC session default if not found.
    const tenantId = 0; 
    
    this.commonService.getMultipleConfigValues(['CONTACT_SOURCE', 'CHHANNEL_PREFERENCE'], tenantId)
      .subscribe(configs => {
        this.contactSources = configs['CONTACT_SOURCE'] || [];
        // Note: MVC View uses "CHHANNEL_PREFERENCE" (typo)
        this.channelPreferences = configs['CHHANNEL_PREFERENCE'] || [];
      });
  }

  loadContact(id: number) {
    this.contactService.getContactById(id).subscribe(contact => {
      // Patch form
      this.contactForm.patchValue({
        id: contact.id,
        firstName: contact.firstName,
        middleName: contact.middleName,
        lastName: contact.lastName,
        countryId: contact.countryId,
        languageId: contact.languageId,
        phoneNumber: contact.phoneNumber,
        email: contact.email,
        company: contact.company,
        jobTitle: contact.jobTitle,
        contactSource: contact.contactSource,
        channelPreference: contact.channelPreference
      });

      if (contact.countryId) {
        this.loadLanguages(contact.countryId);
      }
    });
  }

  onCountryChange() {
    const countryId = this.contactForm.get('countryId')?.value;
    this.availableLanguages = [];
    this.contactForm.patchValue({ languageId: null });
    
    if (countryId) {
      this.loadLanguages(countryId);
    }
  }

  loadLanguages(countryId: number) {
     this.commonService.getLanguagesByCountryId(countryId).subscribe({
         next: (countryDto) => {
             // The API returns a CountryDto, which contains the list of languages?
             // Based on C# CountryDto: public List<LanguageDto> Languages { get; set; } = new();
             this.availableLanguages = countryDto.languages || [];
         },
         error: (err) => console.error(err)
     });
        
    // Optimized: In MVC it calls `GetLanguagesByCountry` action which calls `commonServices.GetLanguagesByCountryIdAsync`.
    // In `CommonServices.cs`, `GetLanguagesByCountryIdAsync` returns `CountryDto`. 
    // So Angular service `getLanguagesByCountryId` returning Observable<CountryDto> is correct.
  }

  shouldShowError(controlName: string): boolean {
    const control = this.contactForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onClose() {
    this.close.emit();
  }

  onSubmit() {
    if (this.contactForm.invalid) {
      this.contactForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const contactData: ContactDto = this.contactForm.value;
    
    // Ensure string nulls are handled if API expects specific values
    if (!contactData.contactSource) contactData.contactSource = 'Unknown'; // Or null if API allows
    if (!contactData.channelPreference) contactData.channelPreference = 'Unknown'; 

    const request$ = this.contactId 
      ? this.contactService.updateContact(contactData)
      : this.contactService.addContact(contactData);

    request$.subscribe({
      next: () => {
        this.isSaving = false;
        this.save.emit();
      },
      error: (err) => {
        this.isSaving = false;
        toastr.error('Failed to save contact');
        console.error(err);
      }
    });
  }
}
