import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { CalendarOptions, EventInput } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin, { DateClickArg } from '@fullcalendar/interaction';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { MatDialog } from '@angular/material/dialog';
import { AddOrEditExpenseDialog } from '../../dialogs/add-or-edit-expense-dialog/add-or-edit-expense-dialog';
import { Expense } from '../../entities/expense.model';
import { ExpensesService } from '../../services/expenses.service';

@Component({
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.css'],
})
export class ExpensesComponent implements AfterViewInit {
  @ViewChild('calendar') calendarComponent!: FullCalendarComponent;

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    height: 500,
    aspectRatio: 1.5,
    dateClick: (arg) => this.onDateClick(arg),
    events: [
      { title: 'Expense - 500 RON', date: '2024-03-01', color: 'red', id:'guid1' },
      { title: 'event 2', date: '2019-04-02' },
    ],
    eventClick: function(info) {
      alert('View: ' + info.event.id);
      alert("Category: " + info.event.extendedProps['categoryName'])
      alert("Note: " + info.event.extendedProps['note'])

  
      // change the border color just for fun
      info.el.style.borderColor = 'red';
    }
  };

  expenses!: Expense[];

  constructor(
    private dialog: MatDialog,
    private expensesService: ExpensesService
  ) {}

  ngAfterViewInit(): void {
    let calendar = this.calendarComponent.getApi();
    //calendar.addEventSource
  }

  addExpense(): void {
    let calendar = this.calendarComponent.getApi();
    const dialogRef = this.dialog.open(AddOrEditExpenseDialog, {
      data: {
        calendarApi: calendar,
        expenses: this.expenses,
      },
    });
  }

  onDateClick(arg: DateClickArg) {
    let calendarApi = this.calendarComponent.getApi();
    console.log('ce are');
    //this.calendarOptions.events = [{ title: 'Expense - 500 RON', date: arg.dateStr }];
    calendarApi.addEvent({
      title: 'Expense - 500 RON',
      date: arg.dateStr,
      color: 'red',
    });
    let event: EventInput = { title: 'Expense - 500 RON', date: arg.dateStr };
  }
}
