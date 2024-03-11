import { EventEmitter } from '@angular/core'

/** Defines a Component that agrees to implement a Progress bar */
export interface IComponentProgressBar {
    displayProgressBar: boolean
}

/** Defines a Component that agrees to notify a parent when a HTTP request is being performed */
export interface INotifyOnHttpRequest {
    onHttpRequest: EventEmitter<boolean>
}

/** Defines a Component that agrees to listen for a HTTP request event*/
export interface IListenForHttpRequestEvent {
    onHttpRequestEventNotified(value: boolean): any
}


