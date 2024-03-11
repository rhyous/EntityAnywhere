import { Component, OnInit } from '@angular/core'

@Component({
  selector: 'app-Default-landing-pageComponent',
  template: `
  <div layout-padding flex><div class="eaf-dashboard-header" style="margin-top:10px;">
  Hello.
  </div>
`,
// tslint:enable: max-line-length
  styleUrls: []
})
export class DefaultLandingPageComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
