interface Grouping<TGroup, T> {
    group: TGroup
    data: T[]
}

interface FacetedFilter {
    filter: string | number | boolean
    exactMatch: boolean
}

interface Array<T> {
    /** Return the first item in an array or null if no item exits*/
    first(): T

    /** Return the first item in an array based upon the search criteria or null if no item exists*/
    firstOrDefault(selector: (item: T) => boolean): T

    /** Return the last item in an array or null*/
    lastOrDefault(): T

    /** Run code on every item in a collection*/
    each(code: (item: T, index: number) => void): Array<T>

    /** Return a new Array<T> based upon the old collection and a selection criteria
     You provide a predicate that takes a T and returns true to include it in the collection*/
    where(selector: (item: T) => boolean): Array<T>

    /** Remove an item in the collection*/
    remove(item: T): Array<T>

    /** Remove an item in collection at a given index*/
    removeAt(index: number): Array<T>

    /** Take a number of rows, skipping a number of records if you wish.*/
    take(numberOfRows: number): Array<T>
    // tslint:disable-next-line: unified-signatures
    take(numberOfRows: number, offSet: number): Array<T>

    /** Order an array using a property selector */
    orderBy(selector: (item: T) => any): Array<T>

    /** Order an array using a property selector in descending order*/
    orderByDescending(selector: (item: T) => any): Array<T>

    /** Return the sum of the property defined in the predicate */
    sum(selector: (item: T) => number): number

    /** Return only a distinct set of the items */
    distinctOnly(): Array<T>

    /** Return a selection of properties from each item in the array that are distinct */
    distinct<NewT>(selector: (item: T) => NewT): Array<NewT>

    distinctBy(selector: (item: T) => any): Array<T>

    /** Project each item in the array to a new form */
    select<NewT>(selector: (item: T) => any): Array<NewT>

    // tslint:disable-next-line: no-shadowed-variable
    groupBy<TGroup, T>(selector: (item: T) => TGroup): Grouping<TGroup, T>[]

    /** Loops over the other object's properties to see if any match the object currently iterating over */
    whereMatches(other: any): Array<T>

    /** Performs an in memory faceted search on an OData Object
     * @param other: This is an object with the same property names as the object you are searching upon
     * but it's type must be a FacetedFilter
     */
    odataFacetedSearch(other: any): Array<T>

    /** Performs an in memory faceted search
     * @param other: This is an object with the same property names as the object you are searching upon
     * but it's type must be a FacetedFilter
     */
    facetedSearch(other: any): Array<T>

    // ** Return true or false depending if array contains object with same property value. */
    contains(selector: (item: T) => boolean): T
}

/** Return the first item in an array or null if no item exits*/
Array.prototype.first = function () {
    if (this.length > 0) {
        return this[0]
    }

    return null
}

/** Return the first item in an array based upon the search criteria or null if no item exists*/
Array.prototype.firstOrDefault = function (selector: (item: any) => boolean) {
    const collection: any = []
    this.each((item: any) => {
        if (selector(item)) {
            collection.push(item)
        }
    })

    if (collection.length === 0) {
        return null
    }

    return collection[0]
}

/** Return the last item in an array or null*/
Array.prototype.lastOrDefault = function () {
    if (this.length === 0) {
        return null
    }

    return this[this.length - 1]
}

/** Run code on every item in a collection*/
Array.prototype.each = function (code: (item: any, index: number) => void) {
    for (let i = 0; i < this.length; i++) {
        code(this[i], i)
    }

    return this
}

/** Return a new Array<T> based upon the old collection and a selection criteria
     You provide a predicate that takes a T and returns true to include it in the collection*/
Array.prototype.where = function (selector: (item: any) => boolean) {
    const collection: any = []
    this.each((item: any) => {
        if (selector(item)) {
            collection.push(item)
        }
    })

    return collection
}

/** Remove an item in the collection*/
Array.prototype.remove = function (item: any) {
    return this.where((other: any) => other !== item)
}

/** Remove an item in collection at a given index*/
Array.prototype.removeAt = function (index: number) {
    this.splice(index, 1)
    return this
}

/** Take a number of rows, skipping a number of records if you wish.*/
Array.prototype.take = function (numOfRows: number, offset: number = 0) {
    const collection: any = []
    let count = 0
    this.each((row: any, i: number) => {
        if (i >= offset && count < numOfRows) {
            collection.push(row)
            count++
        }
    })

    return collection
}

Array.prototype.distinctOnly = function() {
    const result = []
    const map = new Map()
    for (const item of this) {
        if (!map.has(item)) {
            map.set(item, true)
            result.push(item)
        }
    }
    return result
}

/** Return only the distinct items from the array decided by the supplied predicate */
Array.prototype.distinct = function<NewT>(selector: (item: NewT) => NewT) {
    const result = []
    const map = new Map()
    for (const item of this) {
        const selection = selector(item)
        if (!map.has(selection)) {
            map.set(selection, true)
            result.push(selection)
        }
    }
    return result
}

/** Return the entire object based upon a selector */
Array.prototype.distinctBy = function<T>(selector: (item: T) => any) {
    const result = []
    const map = new Map()
    for (const item of this) {
        const selection = selector(item)
        if (!map.has(selection)) {
            map.set(selection, true)
            result.push(item)
        }
    }
    return result
}

/** Order an array using a property selector */
Array.prototype.orderBy = function (selector: (item: any) => any) {
    const data = [...this]

    return data.sort((a, b) => {
        const aComparable = selector(a)
        const bComparable = selector(b)

        if (aComparable > bComparable) { return 1 }
        if (aComparable < bComparable) { return -1 }

        return 0
    })
}

/** Order an array using a property selector in descending order*/
Array.prototype.orderByDescending = function (selector: (item: any) => any) {
    return this.orderBy(selector).reverse()
}

Array.prototype.sum = function (selector: (item: any) => number): number {
    let count = 0
    for (const item of this) {
        count += +selector(item)
    }
    return count
}

/** Project each item in the array to a new form */
Array.prototype.select = function<NewT>(selector: (item: any) => any): Array<NewT> {
    const arr = []
    for (const item of this) {
        const selection = selector(item)
        if (selection) {
            arr.push(selector(item))
        }
    }
    return arr
}

Array.prototype.groupBy = function<TGroup, T>(selector: (item: T) => TGroup): Grouping<TGroup, T>[] {
    const array: Grouping<TGroup, T>[] = []
    if (this && this.length > 0) {
        const distinct = this.distinct(selector)
        for (const dis of distinct) {
            const data = this.where(x => selector(x) === dis)
            array.push({
                group: dis,
                data: data
            })
        }
    }
    return array
}

Array.prototype.whereMatches = function(other: any) {
    const arr: any = []
    for (const item of this) {
        Object.keys(other).each(x => {
            if (!other[x]) {
                arr.push(item)
            } else if (<string>(other[x]).includes(item[x])) {
                arr.push(item)
            }
        })
    }

    return arr.distinctOnly()
}


Array.prototype.facetedSearch = function(other: any) {
    let arr: any = []
    let discountedItems = []
    // pull out all of the properties we are searching against
    const searchProperties = Object.keys(other)

    // pull out all the properties from the data set that we want to search against
    for (const item of this) {
        for (const property of searchProperties) {
            const filter: FacetedFilter = other[property]
            const propertyValue = item[property]
            if (filter.filter && filter.filter.toString() !== '') {
                if (filter.exactMatch) {
                    if (filter.filter.toString().toLowerCase() === propertyValue.toString().toLowerCase()) {
                        arr.push(item)
                    } else {
                        discountedItems.push(item)
                    }
                } else {
                    if (propertyValue.toString().toLowerCase().includes(filter.filter.toString().toLowerCase())) {
                        arr.push(item)
                    } else {
                        discountedItems.push(item)
                    }
                }
            } else {
                arr.push(item)
                discountedItems.push(item)
            }
        }
    }
    discountedItems = discountedItems.distinctOnly()
    arr = arr.distinctOnly()
    discountedItems.forEach(element => {
        arr = arr.remove(element)
    })

    return arr
}

Array.prototype.odataFacetedSearch = function(other: any) {
    let arr: any = []
    let discountedItems = []
    // pull out all of the properties we are searching against
    const searchProperties = Object.keys(other)

    // pull out all the properties from the data set that we want to search against
    for (const item of this) {
        for (const property of searchProperties) {
            const filter: FacetedFilter = other[property]
            const propertyValue = item.Object[property]
            if (filter.filter && filter.filter.toString() !== '') {
                if (filter.exactMatch) {
                    if (filter.filter.toString().toLowerCase() === propertyValue.toString().toLowerCase()) {
                        arr.push(item)
                    } else {
                        discountedItems.push(item)
                    }
                } else {
                    if (propertyValue && propertyValue.toString().toLowerCase().includes(filter.filter.toString().toLowerCase())) {
                        arr.push(item)
                    } else {
                        discountedItems.push(item)
                    }
                }
            }
        }
    }
    discountedItems = discountedItems.distinctOnly()
    arr = arr.distinctOnly()
    discountedItems.forEach(element => {
        arr = arr.remove(element)
    })

    return arr
}

/** Return true or false depending if array contains object with matching property values*/
Array.prototype.contains = function (selector: (item: any) => boolean) {
    return this.firstOrDefault(selector) !== null
}
