import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DebtsService } from '../../services/debts.service';
import { Debt } from '../../interfaces/debt';

@Component({
  selector: 'app-view-debts',
  templateUrl: './view-debts.component.html',
  styleUrls: ['./view-debts.component.css'],
  providers: [DebtsService],
})
export class ViewDebtsComponent implements AfterViewInit, OnInit {
  debts!: Debt[];
  displayedColumns: string[] = [
    'name',
    'email',
    'amount',
    'borrowingDate',
    'deadline',
    'reason',
    'action',
  ];
  dataSource = new MatTableDataSource();
  constructor(
    private liveAnnouncer: LiveAnnouncer,
    private debtsService: DebtsService
  ) {}
  
  ngOnInit(): void {
    this.debtsService.getAll().subscribe((response) => {
      this.dataSource.data = response.payload;
    });
  }

  @ViewChild('debtTbSort') debtTbSort: MatSort = new MatSort();

  ngAfterViewInit() {
    this.dataSource.sort = this.debtTbSort;
  }

  announceSortChange(sortState: Sort) {
    if (sortState.direction) {
      this.liveAnnouncer.announce(`Sorted ${sortState.direction} ending`);
    } else {
      this.liveAnnouncer.announce('Sorting cleared');
    }
  }
}
