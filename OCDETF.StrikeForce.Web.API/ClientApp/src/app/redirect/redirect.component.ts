import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-redirect',
  templateUrl: './redirect.component.html',
  styleUrls: ['./redirect.component.css']
})
export class RedirectComponent implements OnInit {

  reportID: string;

  constructor(private route: ActivatedRoute, private router: Router) {

    this.reportID = this.router.url.split('/')[2];
    let child: string = this.router.url.split('/')[3];
    this.router.navigate(['/quarterly/' + this.reportID + '/' + child]);
  }

  ngOnInit(): void {
  }

}
