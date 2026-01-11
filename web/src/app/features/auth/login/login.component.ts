import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

// Declare Toastr since we loaded it globally in angular.json
declare var toastr: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }

    this.isLoading = true;
    const credentials = {
        Email: this.loginForm.value.email,
        Password: this.loginForm.value.password,
        ReturnUrl: ''
    };

    this.authService.login(credentials).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response && response.accessToken) {
            // Success
            this.router.navigate(['/contacts']);
        } else {
            // API returned 200 but logic failed (e.g. wrong password)
            const msg = response?.message || 'Login failed';
            toastr.error(msg);
        }
      },
      error: (err) => {
        this.isLoading = false;
        toastr.error('An error occurred during login.');
        console.error(err);
      }
    });
  }
}
