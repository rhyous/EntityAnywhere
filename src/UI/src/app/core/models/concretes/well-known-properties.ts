import { Injectable } from '@angular/core'

@Injectable()
export class WellKnownProperties {
    idProperty = 'Id'
    auditableProperties = ['CreateDate', 'CreatedBy', 'LastUpdated', 'LastUpdatedBy']
    fileUploadProperties = ['FileName', 'Data']
}
