import { Injectable } from '@angular/core';
import { PaginationModel } from '../entities/pagination.model';
import { PageEvent } from '@angular/material/paginator';

@Injectable()
export class PaginationService {
  private paginationModel: PaginationModel;

  get page(): number {
      return this.paginationModel.pageIndex;
  }

  get selectItemsPerPage(): number[] {
      return this.paginationModel.selectItemsPerPage;
  }

  get pageCount(): number {
      return this.paginationModel.pageSize;
  }

  constructor() {
      this.paginationModel = new PaginationModel();
  }

  change(pageEvent: PageEvent) {
      this.paginationModel.pageIndex = pageEvent.pageIndex + 1;
      this.paginationModel.pageSize = pageEvent.pageSize;
      this.paginationModel.allItemsLength = pageEvent.length;
  }
}
