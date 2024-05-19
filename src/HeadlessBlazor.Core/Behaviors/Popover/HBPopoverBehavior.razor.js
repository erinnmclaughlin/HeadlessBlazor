import { autoUpdate, computePosition, flip } from '../../floating-ui.dom.browser.min.mjs';

export class HBPopoverBehavior {

    /**
     * @static
     * @param {Element} anchor
     * @param {Element} content
     * @param {any} options
     * @returns {HBPopoverBehavior}
     */
    static createInstance(anchor, content, options) {
        return new HBPopoverBehavior(anchor, content, options);
    }

    /**
     * Creates a new instance of HBPopoverBehavior.
     * @constructor
     * @param {Element} anchor
     * @param {Element} content
     * @param {any} options
     */
    constructor(anchor, content, options) {
        const side = options.side || 'bottom';
        const align = options.alignment || 'start';
        this.dispose = autoUpdate(anchor, content, () => {
            computePosition(anchor, content, {
                placement: `${side}-${align}`.toLowerCase(),
                middleware: [flip()]
            }).then(({ x, y }) => {
                Object.assign(content.style, {
                    left: `${x}px`,
                    top: `${y}px`,
                });
            })
        });
    }
}