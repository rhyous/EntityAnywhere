import { Sort } from '@angular/material/sort'

/** Helper class for Angular Material Sort */
export class MaterialSortHelpers {
    /** Custom sort helper method */
    static sort<T>(sort: Sort, dataSource: T[]) {
        return dataSource = sort.direction === 'asc' ?
        dataSource.orderBy((x: any) => x[sort.active]) :
        sort.direction === 'desc' ?
          dataSource.orderByDescending((x: any) => x[sort.active]) :
            dataSource
      }
}
