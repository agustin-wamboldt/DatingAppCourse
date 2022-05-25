export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

export class PaginatedResults<T> { // In our case, T: Member[]
    result: T;
    pagination: Pagination;
}