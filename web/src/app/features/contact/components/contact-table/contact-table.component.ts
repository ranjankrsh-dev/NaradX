import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ContactDto, ContactFilters, PagedResult } from 'src/app/core/models/contact.models';
import { ContactService } from '../../services/contact.service';

@Component({
  selector: 'app-contact-table',
  templateUrl: './contact-table.component.html',
  styleUrls: ['./contact-table.component.scss']
})
export class ContactTableComponent implements OnInit {
  @Input() filters: ContactFilters = {
    pageNumber: 1,
    pageSize: 10
  };
  
  @Output() edit = new EventEmitter<number>();
  @Output() delete = new EventEmitter<number>();

  contactList: PagedResult<ContactDto> | null = null;
  isLoading = false;
  pageNumbers: number[] = [];

  constructor(private contactService: ContactService) {}

  ngOnInit(): void {
    this.loadContacts();
  }

  loadContacts() {
    this.isLoading = true;
    this.contactService.getContacts(this.filters).subscribe({
      next: (result) => {
        this.contactList = result;
        this.generatePageNumbers();
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading contacts', err);
        this.isLoading = false;
        // Mock data for development if API fails or returns void
        // this.mockData(); 
      }
    });
  }

  changePage(page: number) {
    if (page < 1 || (this.contactList && page > this.contactList.totalPages)) return;
    this.filters.pageNumber = page;
    this.loadContacts();
  }

  generatePageNumbers() {
    if (!this.contactList) return;
    this.pageNumbers = Array(this.contactList.totalPages).fill(0).map((x, i) => i + 1);
  }

  onEdit(id: number) {
    this.edit.emit(id);
  }

  onDelete(id: number) {
    this.delete.emit(id);
  }
}
