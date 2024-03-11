import { AfterViewInit, Component, OnInit } from '@angular/core'
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms'
import { DisplayFnData, IAutoCompleteDisplayFunction } from 'src/app/core/auto-complete/auto-complete-display-function.interface'
import { AutocompleteHelper } from 'src/app/core/infrastructure/autocomplete-helper'
import { LoginService } from 'src/app/core/login/login.service'
import { ODataObject } from 'src/app/core/models/interfaces/o-data-entities/o-data-object.interface'
import { UserRole } from 'src/app/core/models/interfaces/o-data-entities/user-role.interface'
import { EntityService } from 'src/app/core/services/entity.service'
import { ImpersonationService } from 'src/app/core/services/impersonation.service'

@Component({
  selector: 'app-impersonate-customer',
  templateUrl: './impersonate-customer.component.html',
  styleUrls: ['./impersonate-customer.component.scss']
})
export class ImpersonateCustomerComponent implements OnInit {
  displayProgressBar = false
  userRoles!: ODataObject<UserRole>[]

  form: FormGroup = new FormGroup({})
  organizationField = AutocompleteHelper.createAutocompleteField('OrganizationId', 'Organization *')

  constructor(private entityService: EntityService,
              private impersonationService: ImpersonationService,
              private loginService: LoginService,
              private fb: FormBuilder) { }

  ngOnInit() {
    this.form = this.fb.group({
      UserRole: new FormControl('', Validators.required),
    })
    this.entityService.getEntityList('UserRole').subscribe(next => {
      this.userRoles = next.Entities

      const defaultUserRole = this.userRoles.find(x => x.Object.Name === 'Customer')

      this.form.get('UserRole')?.setValue(defaultUserRole)
    })
  }

  impersonateCustomer() {
    const role: string = this.form.value.UserRole.Id

    this.impersonationService.getImpersonationToken(role).subscribe(impersonationToken => {
      this.loginService.parseToken(impersonationToken)
    })
  }

}
