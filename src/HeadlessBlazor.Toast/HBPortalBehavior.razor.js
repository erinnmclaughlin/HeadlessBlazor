export class HBPortalBehavior {

    /**
     * @param {Element} element
     * @returns {HBPortalBehavior}
     */
    static createInstance(element) {
        return new HBPortalBehavior(element);
    }

    /**
     * Relocates the element to the end of <body> so it escapes any ancestor
     * stacking / overflow context.
     * @constructor
     * @param {Element} element
     */
    constructor(element) {
        this.element = element;
        document.body.appendChild(element);
    }

    dispose() {
        // Blazor owns this node and removes it (via its current parent) when the
        // component is torn down, so we must not move or re-insert it here -
        // doing so would leave an orphaned element in the DOM. We only release
        // our reference.
        this.element = null;
    }
}
