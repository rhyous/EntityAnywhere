import { BooleanStringType } from '../../types/boolean-string.type'

export interface ITestFilterMetaData {
    runInProd: BooleanStringType
}

export const TESTFILTER_PROD_METADATA: ITestFilterMetaData = {runInProd: 'true'}
