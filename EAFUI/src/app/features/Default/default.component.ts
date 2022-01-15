import { Component, OnInit } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'


@Component({
  selector: 'app-Default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss']
})
export class DefaultComponent implements OnInit {

  constructor(private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      const urlSegments = this.router.parseUrl(this.router.url)
      if (urlSegments.root.children.primary.segments.length === 1) {
        this.router.navigate(['default'], { relativeTo: this.activatedRoute })
      }
    })
  }
}
