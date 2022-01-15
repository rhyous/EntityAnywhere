/**
 * This interface forces the subscribe method to use the type paramater in next.
 *
 * Usage: onSomeEvent: TypedEventEmitter<string> = new EventEmitter<string>()
 */
export interface TypedEventEmitter<T> {
    subscribe(next: (generatorOrNext?: T) => void, error?: any, complete?: any)
    emit(val: T)
}
