import { Component, OnInit } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      const urlSegments = this.router.parseUrl(this.router.url)
      if (urlSegments.root.children.primary.segments.length === 1) {
        this.router.navigate(['dashboard'], { relativeTo: this.activatedRoute })
      }
    })
  }

}
