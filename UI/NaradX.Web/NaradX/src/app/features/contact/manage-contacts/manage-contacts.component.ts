import { Component, ViewChild } from '@angular/core';
import { ContactFilters } from 'src/app/core/models/contact.models';
import { ContactTableComponent } from '../components/contact-table/contact-table.component';
import { ContactService } from '../services/contact.service';

declare var toastr: any;

@Component({
  selector: 'app-manage-contacts',
  templateUrl: './manage-contacts.component.html',
  styleUrls: ['./manage-contacts.component.scss']
})
export class ManageContactsComponent {
  
  @ViewChild(ContactTableComponent) contactTable!: ContactTableComponent;

  filters: ContactFilters = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    name: '',
    phone: '',
    status: null // or '' depending on backend
  };

  showAddContactModal = false;
  showBulkUploadModal = false;
  selectedContactId: number | null = null;
  
  constructor(private contactService: ContactService) {}

  applyFilter() {
    this.filters.pageNumber = 1; // Reset to first page
    this.refreshTable();
  }

  applySearch() {
    this.filters.pageNumber = 1;
    this.refreshTable();
  }

  clearFilters() {
    this.filters = {
      pageNumber: 1,
      pageSize: 10,
      searchTerm: '',
      name: '',
      phone: '',
      status: null
    };
    this.refreshTable();
  }

  toggleFilters() {
     // Logic to toggle filter visibility if needed (Bootstrap 'd-none' is usually handled via ViewChild or simple bool)
     // For now, let's assume the HTML structure handles it or we'll add a boolean flag 'isFilterVisible' later.
  }

  refreshTable() {
    if (this.contactTable) {
      this.contactTable.loadContacts();
    }
  }

  // Actions
  openAddContactModal() {
    this.selectedContactId = null;
    this.showAddContactModal = true;
  }

  onEditContact(id: number) {
    this.selectedContactId = id;
    this.showAddContactModal = true;
  }

  closeAddContactModal() {
    this.showAddContactModal = false;
  }

  onContactSaved() {
    this.showAddContactModal = false;
    this.refreshTable();
    toastr.success('Contact saved successfully');
  }

  onDeleteContact(id: number) {
    if(confirm('Are you sure you want to delete this contact?')) {
        this.contactService.deleteContact(id).subscribe({
            next: () => {
                toastr.success('Contact deleted successfully');
                this.refreshTable();
            },
            error: (err: any) => { // Type annotation added
                toastr.error('Failed to delete contact');
                console.error(err);
            }
        });
    }
  }

  // Bulk Upload
  openBulkUploadModal() {
    this.showBulkUploadModal = true;
  }

  closeBulkUploadModal() {
      this.showBulkUploadModal = false;
      this.refreshTable(); // Refresh in case uploads happened
  }
}
