import {
    autoUpdate,
    computePosition,
    flip
} from '../@floating-ui/dom/dist/floating-ui.dom.browser.min.mjs';

export class HBPopoverBehavior {

    /**
     * @static
     * @param {Element} container
     * @param {any} dotNetRef
     * @returns {HBPopoverBehavior}
     */
    static createInstance(container) {
        return new HBPopoverBehavior(container);
    }

    /**
     * Creates a new instance of HBPopoverBehavior.
     * @constructor
     * @param {Element} container
     * @param {any} dotNetRef
     */
    constructor(container) {
        const anchor = container.querySelector('[hb-popover-anchor]');
        const floater = container.querySelector('[hb-popover]');

        this.dispose = autoUpdate(anchor, floater, () => {
            computePosition(anchor, floater, {
                placement: 'bottom-start',
                middleware: [flip()]
            }).then(({ x, y }) => {
                Object.assign(floater.style, {
                    display: `block`,
                    left: `${x}px`,
                    top: `${y}px`,
                });
            })
        });
    }
}