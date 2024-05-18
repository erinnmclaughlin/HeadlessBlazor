export class OutsideClickBehavior {

    /**
     * @param {Element} element
     * @param {any} dotNetRef
     */
    static createInstance(element, dotNetRef) {
        return new OutsideClickBehavior(element, dotNetRef);
    }

    /**
     * @param {Element} element
     * @param {any} dotNetRef
     */
    constructor(element, dotNetRef) {
        console.log('creating');
        this.element = element;
        this.dotNetRef = dotNetRef;

        this.handleDocumentClick = this.handleDocumentClick.bind(this);

        document.addEventListener('click', this.handleDocumentClick);
    }

    dispose() {
        console.log('disposing');
        document.removeEventListener('click', this.handleDocumentClick);
    }

    /**
     * @param {MouseEvent} e
     */
    handleDocumentClick(e) {
        if (!this.element.contains(e.target)) {
            this.dotNetRef.invokeMethodAsync('NotifyClickOutside');
        }
    }
}
