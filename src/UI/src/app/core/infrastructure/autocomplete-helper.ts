export class AutocompleteHelper {
    /**
     * Creates an Autocomplete field
     * @param name The name of the field in the form that this autocomplete is tied to
     * @param label The label to be displayed
     * @param value The value for prefetched data
     * @param required Is this field required
     */
    static createAutocompleteField(name: string, label: string, value: any = null, required = true) {
        return {
            name: name,
            label: label,
            value: value,
            required: required,
            order: 1,
            flex: 40,
            type: 'number'
          }
    }
}
