import { Pipe, PipeTransform } from '@angular/core'
import { EntityCommonPipe } from './common.pipe'

export abstract class EntityPropertyPipe implements PipeTransform {

    public abstract entity: string
    public abstract property: string

    constructor() { }

    transform(value: any, ...args: any[]): any {
        throw new Error('not implemented')
    }

    register(): void {
        EntityCommonPipe.registerEntityPipe(this)
    }
}
