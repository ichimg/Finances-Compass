import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';


export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H'},
  {position: 2, name: 'Helium', weight: 4.0026, symbol: 'He'},
  {position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li'},
  {position: 4, name: 'Beryllium', weight: 9.0122, symbol: 'Be'},
  {position: 5, name: 'Boron', weight: 10.811, symbol: 'B'},
  {position: 6, name: 'Carbon', weight: 12.0107, symbol: 'C'},
  {position: 7, name: 'Nitrogen', weight: 14.0067, symbol: 'N'},
  {position: 8, name: 'Oxygen', weight: 15.9994, symbol: 'O'},
  {position: 9, name: 'Fluorine', weight: 18.9984, symbol: 'F'},
  {position: 10, name: 'Neon', weight: 20.1797, symbol: 'Ne'},
];


@Component({
  selector: 'app-view-debts',
  templateUrl: './view-debts.component.html',
  styleUrls: ['./view-debts.component.css']
})
export class ViewDebtsComponent implements AfterViewInit {

  displayedColumns: string[] = ['position', 'e-mail', 'amount', 'borrowingDate', 'deadline', 'reason', 'action'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);
  constructor(private liveAnnouncer: LiveAnnouncer){}

  @ViewChild('debtTbSort') debtTbSort: MatSort = new MatSort();

  ngAfterViewInit() {
    this.dataSource.sort = this.debtTbSort;
  }

   announceSortChange(sortState: Sort) {
   
    if (sortState.direction) {
      this.liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this.liveAnnouncer.announce('Sorting cleared');
    }
  }
}
