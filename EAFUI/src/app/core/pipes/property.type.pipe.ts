import { Pipe, PipeTransform } from '@angular/core'
import { EntityCommonPipe } from './common.pipe'

/**
 * NOT IMPLEMENTED
 */
export abstract class PropertyTypePipe implements PipeTransform {

    public abstract type: string

    transform(value: any, ...args: any[]): any {
        throw new Error('not implemented')
    }

    register(): void {
        EntityCommonPipe.registerTypePipe(this)
    }
}
