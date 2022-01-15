import { Component, OnInit } from '@angular/core'
import { Router} from '@angular/router'
import { trigger, style, transition, animate, keyframes, query, stagger } from '@angular/animations'

import { EntityService } from 'src/app/core/services/entity.service'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { environment } from 'src/environments/environment'
import { EntityGrouping } from './entity-grouping'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { UserDataService } from 'src/app/core/services/user-data.service'


@Component({
  selector: 'app-entities-list',
  templateUrl: 'entities-list.component.html',
  animations: [
    trigger('entities', [
      transition('* => *', [
        query(':enter', style({opacity: 0}), {optional: true}),

        query(':enter', stagger('20ms', [
          animate('.2s ease-in', keyframes([
            style({opacity: 0, transform: 'translateX(-50%)', offset: 0}),
            style({opacity: .5, transform: 'translateX(10px)', offset: .3}),
            style({opacity: 1, transform: 'translateX(0)', offset: 1})
          ]))
        ]), {optional: true})
      ])
    ])
  ]
})
/**
 * The entities List Component. Responsible for listing all entities such as Activation Attempts, Licensed Devices, Product Types etc
 * and then navigating to them if requested
 */
export class EntitiesListComponent implements OnInit {
  public pluralize = new PluralizePipe()
  showProgressBar = true

  public entitiesArray: Grouping<string, EntityGrouping>[]

  constructor(
    private entityService: EntityService,
    private entityMetadataService: EntityMetadataService,
    private errorReporter: ErrorReporterDialogComponent,
    private router: Router,
    private localStorageService: AppLocalStorageService,
    private userDataService: UserDataService
  ) { }

  /**
   * Navigate to the entity
   * @param entityName The name of the entity to navigate to
   */
  toEntityList(entityName: string) {
    this.router.navigate([`/admin/data-management/${this.pluralize.transform(entityName)}`])
  }

  ngOnInit() {

    this.entityService.getEntities().subscribe(entities => { // get all of the entities from the entity service
      this.entitiesArray = Object.keys(entities.EAF)
        .filter(x => x.indexOf('$') === -1) // Remove the entities starting with $
        .select<EntityGrouping>(x => new EntityGrouping(x, entities.EAF[x]['@EAF.EntityGroup'])) // create an EntityGrouping
        .filter(x =>  this.localStorageService.User.AdminRole || this.userDataService.permittedEntitiesForUser.includes(x.entityName))
        .filter(x => x.entityGroup !== undefined) // Remove the entities that haven't got a group
        .orderBy(x => x.entityName) // Order the entities alphabetically
        .groupBy<string, EntityGrouping>(x => x.entityGroup) // Group them by their groups
        .orderBy(x => x.group) // Order the groups alphabetically

      // Reset the storage every time this component is torn down and rebuilt
      localStorage.setItem(environment.metaDataLocalName, JSON.stringify(this.entityMetadataService.convertMetaDataToArray(entities.EAF)))
      this.showProgressBar = false
    },
    (error) => {
      this.errorReporter.displayMessage(error.error)
      this.showProgressBar = false
    })
  }
}
