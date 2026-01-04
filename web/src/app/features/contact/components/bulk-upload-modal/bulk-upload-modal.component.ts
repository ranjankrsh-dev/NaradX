import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ConfigValueDto, CountryDto, LanguageDto } from 'src/app/core/models/common.models';
import { BulkUploadValidateResponseDto } from 'src/app/core/models/contact.models';
import { CommonService } from 'src/app/core/services/common.service';
import { ContactService } from '../../services/contact.service';

declare var toastr: any;

@Component({
  selector: 'app-bulk-upload-modal',
  templateUrl: './bulk-upload-modal.component.html',
  styleUrls: ['./bulk-upload-modal.component.scss']
})
export class BulkUploadModalComponent implements OnInit {
  @Output() close = new EventEmitter<void>();

  currentStep = 1;
  isValidating = false;
  isSubmitting = false;

  // Values
  selectedCountryId: number | null = null;
  selectedLanguageId: number | null = null;
  selectedContactSource: string | null = null;
  selectedChannelPreference: string | null = null;
  selectedFile: File | null = null;
  selectedFileSize: string = '';

  // Lookups
  countries: CountryDto[] = [];
  availableLanguages: LanguageDto[] = [];
  contactSources: ConfigValueDto[] = [];
  channelPreferences: ConfigValueDto[] = [];

  // Results
  validationResult: BulkUploadValidateResponseDto | null = null;

  constructor(
    private commonService: CommonService,
    private contactService: ContactService
  ) { }

  ngOnInit(): void {
    this.loadLookups();
  }

  loadLookups() {
    this.commonService.getAllCountries().subscribe(res => this.countries = res);
    
    // Assume tenantId = 0
    this.commonService.getMultipleConfigValues(['CONTACT_SOURCE', 'CHHANNEL_PREFERENCE'], 0)
      .subscribe(configs => {
        this.contactSources = configs['CONTACT_SOURCE'] || [];
        this.channelPreferences = configs['CHHANNEL_PREFERENCE'] || [];
      });
  }

  onCountryChange() {
    this.selectedLanguageId = null;
    this.availableLanguages = [];
    if (this.selectedCountryId) {
      this.commonService.getLanguagesByCountryId(this.selectedCountryId).subscribe(
        country => this.availableLanguages = country.languages || []
      );
    }
  }

  onClose() {
    this.close.emit();
  }

  // File Handling
  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length) {
      this.handleFile(event.target.files[0]);
    }
  }

  handleFile(file: File) {
    // Validate type
    const validExts = ['.csv', '.xls', '.xlsx'];
    const ext = '.' + file.name.split('.').pop()?.toLowerCase();
    if (!validExts.includes(ext)) {
      toastr.error('Invalid file type. Please upload CSV or Excel.');
      return;
    }
    // Validate size (10MB)
    if (file.size > 10 * 1024 * 1024) {
      toastr.error('File size exceeds 10MB limit.');
      return;
    }

    this.selectedFile = file;
    this.selectedFileSize = this.formatBytes(file.size);
  }

  removeFile() {
    this.selectedFile = null;
    this.selectedFileSize = '';
  }

  formatBytes(bytes: number, decimals = 2) {
    if (!+bytes) return '0 Bytes';
    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return `${parseFloat((bytes / Math.pow(k, i)).toFixed(dm))} ${sizes[i]}`;
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    // Add visual cue if needed
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    if (event.dataTransfer?.files.length) {
      this.handleFile(event.dataTransfer.files[0]);
    }
  }

  downloadTemplate() {
    this.contactService.downloadTemplate();
  }

  // Wizard Navigation
  canValidate(): boolean {
    return !!(this.selectedCountryId && this.selectedLanguageId && this.selectedContactSource && this.selectedChannelPreference && this.selectedFile);
  }

  validateFile() {
    if (!this.canValidate() || !this.selectedFile) return;

    this.isValidating = true;
    this.contactService.validateBulkUpload(
      this.selectedFile,
      this.selectedCountryId!,
      this.selectedLanguageId!,
      this.selectedContactSource!,
      this.selectedChannelPreference!
    ).subscribe({
      next: (res) => {
        this.isValidating = false;
        this.validationResult = res;
        this.nextStep();
        
        // Auto-skip valid check if no errors? logic in js says:
        // if (validationResult.invalidRowsCount === 0) { ... setTimeout -> updateReviewStep -> showStep(3) ... }
        if (res.invalidRowsCount === 0) {
            setTimeout(() => {
                this.nextStep();
            }, 1000);
        }
      },
      error: (err) => {
        this.isValidating = false;
        toastr.error('Validation failed: ' + (err.error?.message || err.message));
        console.error(err);
      }
    });
  }

  submitUpload() {
     if (!this.validationResult) return;
     this.isSubmitting = true;
     // Note: Backend might expect the validation result DTO back to confirm? 
     // MVC js: actionHelper.postJson('/Contact/SaveBulkUploadContacts', validationData);
     // My ContactService.saveBulkUpload handles this.

     this.contactService.saveBulkUpload(this.validationResult).subscribe({
        next: (res) => {
           this.isSubmitting = false;
           if(res.isSuccess) {
               toastr.success(res.message);
               // Close modal
               setTimeout(() => this.onClose(), 1500);
           } else {
               toastr.error(res.message);
           }
        },
        error: (err) => {
           this.isSubmitting = false;
           toastr.error('Upload failed');
           console.error(err);
        }
     });
  }

  nextStep() {
    this.currentStep++;
  }

  prevStep() {
    this.currentStep--;
  }
  
  // Helpers for review
  getCountryName(id: number | null): string {
    return this.countries.find(c => c.id === id)?.name || '-';
  }
  getLanguageName(id: number | null): string {
     // Search in available or re-fetch? It's in availableLanguages
     return this.availableLanguages.find(l => l.id === id)?.name || '-';
  }
  getSourceName(val: string | null): string {
     return this.contactSources.find(c => c.value === val)?.text || '-';
  }
}
