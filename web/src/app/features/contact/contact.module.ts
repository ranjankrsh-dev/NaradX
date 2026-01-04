import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';

import { ContactRoutingModule } from './contact-routing.module';
import { ManageContactsComponent } from './manage-contacts/manage-contacts.component';
import { ContactTableComponent } from './components/contact-table/contact-table.component';
import { AddContactModalComponent } from './components/add-contact-modal/add-contact-modal.component';
import { BulkUploadModalComponent } from './components/bulk-upload-modal/bulk-upload-modal.component';


@NgModule({
  declarations: [
    ManageContactsComponent,
    ContactTableComponent,
    AddContactModalComponent,
    BulkUploadModalComponent
  ],
  imports: [
    CommonModule,
    ContactRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    ModalModule.forRoot()
  ]
})
export class ContactModule { }
