import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css'],
})
export class EmailConfirmationComponent implements OnInit {
  showSuccess!: boolean;
  showAlreadyConfirmed!: boolean;
  showError!: boolean;
  errorMessage!: string;

  constructor(
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.confirmEmail();
  }

  private confirmEmail = () => {
    this.showError = this.showSuccess = false;
    const token = this.route.snapshot.queryParams['token'];
    const email = this.route.snapshot.queryParams['email'];

    this.authService
      .confirmEmail('email-confirmation', token, email)
      .subscribe({
        next: (res: any) => {
          switch(res.statusCode) {
            case 410:
              this.showAlreadyConfirmed = true;
              break;
            case 200: 
              this.showSuccess = true;
              break;
            default:
              this.showError = true;
              break;
          }
        },
        error: (err: HttpErrorResponse) => {
          this.showError = true;
          this.errorMessage = err.message;
        },
      });
  };
}
