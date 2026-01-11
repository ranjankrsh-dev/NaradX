import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageContactsComponent } from './manage-contacts/manage-contacts.component';

const routes: Routes = [
  { path: '', component: ManageContactsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContactRoutingModule { }
