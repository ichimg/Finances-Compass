import { Injectable } from '@angular/core';
import { PaginationModel } from '../entities/pagination.model';
import { PageEvent } from '@angular/material/paginator';

@Injectable()
export class PaginationService {
  private paginationModel: PaginationModel;

  public get pageNumber(): number {
      return this.paginationModel.pageNumber;
  }

  public get pageSize(): number {
      return this.paginationModel.pageSize;
  }

  public get totalCount(): number {
    return this.paginationModel.itemsLength;
  }

  resetPageNumber() {
    this.paginationModel.pageNumber = 1;
  }

  constructor() {
      this.paginationModel = new PaginationModel();
  }

  change(length: number) {
      this.paginationModel.pageNumber++;
      this.paginationModel.itemsLength = length;
  }
}
