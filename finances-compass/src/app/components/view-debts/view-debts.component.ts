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
  displayedReceivingColumns: string[] = [
    'name',
    'email',
    'amount',
    'borrowingDate',
    'deadline',
    'reason',
    'action',
  ];
  dataReceivingDebtsSource = new MatTableDataSource();
  dataUserDebtsSource = new MatTableDataSource();

  isReceivingDebtsLoaded: boolean = false;
  isUserDebtsLoaded: boolean = false;

  constructor(
    private liveAnnouncer: LiveAnnouncer,
    private debtsService: DebtsService
  ) {}
  
  ngOnInit(): void {
    this.debtsService.getAllReceivingDebts().subscribe((response) => {
      this.dataReceivingDebtsSource.data = response.payload;
      this.isReceivingDebtsLoaded = true;
    });

    this.debtsService.getAllUserDebts().subscribe((response) => {
      this.dataUserDebtsSource.data = response.payload;
      this.isUserDebtsLoaded = true;
    });
  }

  @ViewChild('debtReceivingTbSort') debtReceivingTbSort: MatSort = new MatSort();
  @ViewChild('debtUserTbSort') debtUserTbSort: MatSort = new MatSort();

  ngAfterViewInit() {
    this.dataReceivingDebtsSource.sort = this.debtReceivingTbSort;
    this.dataUserDebtsSource.sort = this.debtUserTbSort;
  }

  announceReceivingSortChange(sortState: Sort) {
    if (sortState.direction) {
      this.liveAnnouncer.announce(`Sorted ${sortState.direction} ending`);
    } else {
      this.liveAnnouncer.announce('Sorting cleared');
    }
  }

  announceUserSortChange(sortState: Sort) {
    if (sortState.direction) {
      this.liveAnnouncer.announce(`Sorted ${sortState.direction} ending`);
    } else {
      this.liveAnnouncer.announce('Sorting cleared');
    }
  }

  areDebtsToReceive(): boolean{
    return this.dataReceivingDebtsSource.data.length > 0;
  }

  areDebts(): boolean{
    return this.dataUserDebtsSource.data.length > 0;
  }
}
