/** Represents a First In First Out mechanism for arrays */
export class Queue<T> {

    // Store the items in a private variable because we don't want
    // them exposed to the consumer
    private items: T[] = []

    /**
     * Add an item to the back of the queue
     * @param item the item to be added
     */
    queue(item: T) {
        this.items.push(item)
    }

    /**
     * Return the first item from the queue and remove it
     */
    dequeue(): T {
        return this.items.shift()
    }

    /**
     * Returns the amount of elements in the underlying collection
     */
    count() {
        return this.items.length
    }

    /**
     * Copies the elements in the underlying collection to a new list
     */
    toArray(): T[] {
        return [...this.items]
    }
}
