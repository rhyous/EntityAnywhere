import { Selector } from 'testcafe'

/** Represents the common things for all page models */
export class AngularComponentBaseModel {
    /** The component that this page model is responsible for */
    component!: Selector

    constructor(componentSelector: string) {
        const mainComponent = Selector('app-root')
        this.component = mainComponent.find(componentSelector)
    }
}
