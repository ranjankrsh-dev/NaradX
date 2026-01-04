export interface ContactDto {
  id?: number;
  tenantId?: number;
  firstName: string;
  middleName?: string;
  lastName: string;
  displayName?: string;
  phoneNumber: string;
  country?: string;
  countryId: number;
  language?: string;
  languageId: number;
  contactSource: string;
  channelPreference: string;
  importSource?: string;
  email?: string;
  company?: string;
  jobTitle?: string;
  tags?: string[];
  timezone?: string;
  createdOn?: Date;
}

export interface BulkUploadValidateResponseDto {
  batchId: string;
  totalRows: number;
  validRowsCount: number;
  invalidRowsCount: number;
  invalidRows: InvalidRowDto[];
  message?: string;
}

export interface InvalidRowDto {
  rowNumber: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  errors: string[];
}

export interface ContactFilters {
  tenantId?: number;
  
  pageNumber: number;
  pageSize: number;

  searchTerm?: string;
  name?: string;
  phone?: string;
  status?: string | null; // 'enabled', 'disabled', or null
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  firstItemIndex: number; 
  lastItemIndex: number;
}
