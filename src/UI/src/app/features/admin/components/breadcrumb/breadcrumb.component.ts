import { Component, OnInit } from '@angular/core'
import { Router, NavigationEnd } from '@angular/router'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'

interface Breadcrumb {
  url: string
  label: string
}

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {

  urlParts: Breadcrumb[] = []
  constructor(private router: Router, private pluralize: PluralizePipe) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.createUrlParts()
      }
    })
   }

  ngOnInit() {
    this.createUrlParts()
  }

  private createUrlParts() {
    this.urlParts = []
    const url = this.router.routerState.snapshot.url.substring(1, this.router.routerState.snapshot.url.length)
    const split = url.split('/')
    if (split.length > 3) {
      split[2] = this.pluralize.transform(split[2])
    }
    for (let i = 0; i < split.length; i++) {
      // Got to recreate the url from its parts
      if (i === 0) {
        this.urlParts.push({label: 'Dashboard', url: `/admin/dashboard`})
      } else {
        this.urlParts.push({label: split[i], url: split[i] = `/${split[i - 1]}/${split[i]}`})
      }
    }
    this.urlParts = this.urlParts.distinctBy(x => x.url)
  }

}
