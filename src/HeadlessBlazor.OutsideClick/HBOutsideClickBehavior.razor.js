export class HBOutsideClickBehavior {

    /**
     * @param {Element} element
     * @param {any} dotNetRef
     * @returns {HBOutsideClickBehavior}
     */
    static createInstance(element, dotNetRef) {
        return new HBOutsideClickBehavior(element, dotNetRef);
    }

    /**
     * @constructor
     * @param {Element} element
     * @param {any} dotNetRef
     */
    constructor(element, dotNetRef) {
        this.element = element;
        this.dotNetRef = dotNetRef;
        
        document.addEventListener('click', this.handleDocumentClick);
    }

    dispose() {
        document.removeEventListener('click', this.handleDocumentClick);
    }

    /**
     * @param {MouseEvent} e
     */
    handleDocumentClick = e => {
        if (!this.element.contains(e.target)) {
            this.dotNetRef.invokeMethodAsync('NotifyClickOutside');
        }
    }
}
