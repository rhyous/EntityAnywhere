import { Pipe, PipeTransform, Injectable } from '@angular/core'
import { EntityPropertyPipe } from './entityproperty.pipe'
import { PropertyTypePipe } from './property.type.pipe'

@Injectable({ providedIn: 'root' })
@Pipe({ name: 'EntityCommonPipe' , pure: true})
export class EntityCommonPipe implements PipeTransform {

    static instance = 0
    public static entityPipes = {}
    public static typePipes = {}
    myInstance: number

    static registerEntityPipe(pipe: EntityPropertyPipe): void {
        if (pipe) {
            if (!EntityCommonPipe.entityPipes[pipe.entity]) {
                EntityCommonPipe.entityPipes[pipe.entity] = {}
            }
            EntityCommonPipe.entityPipes[pipe.entity][pipe.property] = pipe
        }
    }

    static registerTypePipe(pipe: PropertyTypePipe): void {
        if (pipe) {
            EntityCommonPipe.typePipes[pipe.type] = pipe
        }
    }

    constructor() {
        this.myInstance = ++EntityCommonPipe.instance
    }

    /*
    * args[0] = Entity
    * args[1] = Property
    * args[2] = Type
    * args[3] = row data
    */
    transform(value: any, ...args: any[]): string {
        if (args && args.length > 1) {
            if (EntityCommonPipe.entityPipes[args[0]] && EntityCommonPipe.entityPipes[args[0]][args[1]]) {
                return EntityCommonPipe.entityPipes[args[0]][args[1]].transform(value, ...args)
            }
            if (args.length > 2 && args[2]) {
                return EntityCommonPipe.typePipes[args[2]].transform(value, ...args)
            }
        }
        return value
    }
}
