import { MatDialogRef } from '@angular/material/dialog';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserModel } from 'src/app/entities/user-friend.model';
import { UsersService } from 'src/app/services/users.service';
import { PaginationService } from 'src/app/services/pagination.service';

@Component({
  selector: 'app-search-users',
  templateUrl: './search-users.dialog.html',
  styleUrls: ['./search-users.dialog.css'],
  providers: [PaginationService],
})
export class SearchUsersDialog implements OnInit {
  users!: UserModel[] | null;
  usersTotalCount!: number;

  @ViewChild('searchInput') searchInput!: ElementRef;

  constructor(
    private dialogRef: MatDialogRef<SearchUsersDialog>,
    private http: HttpClient,
    private usersService: UsersService,
    private paginationService: PaginationService
  ) {}

  ngOnInit(): void {}

  searchUsers(searchInput: any): void {
    this.usersService
      .getUsersBySearchQuery(
        searchInput,
        this.paginationService.pageNumber,
        this.paginationService.pageSize
      )
      .subscribe((response) => {
        this.users = response.body.payload;
        this.usersTotalCount = JSON.parse(
          response.headers.get('X-Pagination')!
        ).TotalCount;

      });
      console.log(this.users);
  }

  loadMoreUsers(): void {
    if (this.users!.length < this.paginationService.totalCount) {
      this.paginationService.change(this.usersTotalCount);

      this.usersService
        .getUsersBySearchQuery(
          this.searchInput.nativeElement.value,
          this.paginationService.pageNumber,
          this.paginationService.pageSize
        )
        .subscribe((response) => {
          this.users = this.users!.concat(
            response.body.payload
          );
          this.usersTotalCount = JSON.parse(
            response.headers.get('X-Pagination')!
          ).TotalCount;


        });
    }
  }

  onSearchInputChange(event: Event) {
    this.paginationService.resetPageNumber();
    this.users = null;
  }

  addFriend(user: UserModel){
    
  }
}
