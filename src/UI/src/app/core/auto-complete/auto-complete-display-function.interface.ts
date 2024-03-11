import { ODataObject } from '../models/interfaces/o-data-entities/o-data-object.interface'

/** Represents the data passed into the IAutoCompleteDisplayFunction */
export interface DisplayFnData<T> {
    key: any
    value: any
    Entity: ODataObject<T>
}

/** Represents the Interface that a component needs to take on to have an Autocomplete */
export interface IAutoCompleteDisplayFunction<T> {
    displayFn(option: DisplayFnData<T>): string | undefined
}
