import { autoUpdate, computePosition, flip } from '../../floating-ui.dom.browser.min.mjs';

export class HBPopoverBehavior {

    /**
     * @static
     * @param {Element} container
     * @param {any} options
     * @returns {HBPopoverBehavior}
     */
    static createInstance(container, options) {
        return new HBPopoverBehavior(container, options);
    }

    /**
     * Creates a new instance of HBPopoverBehavior.
     * @constructor
     * @param {Element} container
     * @param {any} options
     */
    constructor(container, options) {
        const anchor = container.querySelector('[hb-popover-anchor]');
        const floater = container.querySelector('[hb-popover]');
        
        const side = (options && options.Side) || 'bottom';
        const align = (options && options.Alignment) || 'start';

        this.dispose = autoUpdate(anchor, floater, () => {
            computePosition(anchor, floater, {
                placement: `${side}-${align}`,
                middleware: [flip()]
            }).then(({ x, y }) => {
                Object.assign(floater.style, {
                    left: `${x}px`,
                    top: `${y}px`,
                });
            })
        });
    }
}