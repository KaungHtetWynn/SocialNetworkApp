import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'Social Network App';
  users: any;

  // inject http client into this component
  // private makes it available only inside this class
  constructor(private http: HttpClient) {

  }
  ngOnInit(): void {
    // different stages of stream of data
    // {} create an object with 3 properties and their corresponding callback functions
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: err => console.log(err), // error: () => {}
      complete: () => console.log('Request has completed')
    })
  }

}
