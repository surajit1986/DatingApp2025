import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgFor } from '@angular/common';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
 
  http= inject(HttpClient);
  title = 'Dating Application';
  users: any[] = [];

  ngOnInit(): void {
   this.http.get("https://localhost:5001/api/users").subscribe({
      next: (response: any) => {
        this.users = response;
      },
      error: (error) => {
        console.error('Error fetching users:', error);
      },
      complete: () => {
        console.log('User details fetch complete');
      }

   });
  }
}
